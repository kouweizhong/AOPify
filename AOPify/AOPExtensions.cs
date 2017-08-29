using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Transactions;

namespace AOPify
{
    // ReSharper disable once InconsistentNaming
    public static class AOPExtensions
    {

        [DebuggerStepThrough]
        public static AOP Delay(this AOP aopify, int milliseconds)
        {
            return aopify.Combine(process =>
                                      {
                                          System.Threading.Thread.Sleep(milliseconds);
                                          process();
                                      });
        }

        [DebuggerStepThrough]
        public static AOP Until(this AOP aopify, Func<bool> test)
        {
            return aopify.Combine(process =>
                                      {
                                          while (!test()) ;

                                          process();
                                      });
        }

        [DebuggerStepThrough]
        public static AOP While(this AOP aopify, Func<bool> test)
        {
            return aopify.Combine(process =>
                                      {
                                          while (test())
                                              process();
                                      });
        }

        [DebuggerStepThrough]
        public static AOP WhenTrue(this AOP aopify, params Func<bool>[] conditions)
        {
            return aopify.Combine(process =>
                                      {
                                          if (conditions.Any(condition => !condition()))
                                          {
                                              return;
                                          }

                                          process();
                                      });
        }

        [DebuggerStepThrough]
        public static AOP Log(this AOP aopify, string beforeMessage, string afterMessage)
        {
            return aopify.Combine(process =>
            {
                aopify.Logger.Logger.Info(beforeMessage);
                process();
                aopify.Logger.Logger.Info(afterMessage);
            });
        }

        [DebuggerStepThrough]
        public static AOP Log(this AOP aopify, MethodBase currentMethod)
        {
            return aopify.Combine(process =>
           {
               aopify.Logger.CurrentMethod = currentMethod;
               aopify.Logger.Logger.Info(
                   "START type :{0}, method :{1}".FormatWith(aopify.Logger.Target.Name, currentMethod == null ? process.Method.Name : currentMethod.Name));
               process();
               aopify.Logger.Logger.Info("END type :{0}, method :{1}".FormatWith(aopify.Logger.Target.Name, currentMethod == null ? process.Method.Name : currentMethod.Name));
           });
        }

        [DebuggerStepThrough]
        public static AOP Before(this AOP aopify, Action beforeAction)
        {
            aopify.BeforeAction = beforeAction;
            return aopify;
        }

        [DebuggerStepThrough]
        public static AOP After(this AOP aopify, Action afterAction)
        {
            aopify.AfterAction = afterAction;
            return aopify;
        }

        [DebuggerStepThrough]
        public static AOP HowLong(this AOP aopify, string startMessage, string endMessage)
        {
            return aopify.Combine(process =>
            {
                DateTime start = DateTime.Now;
                aopify.Logger.Logger.Info("{0} {1} Method Name : {2}".FormatWith(startMessage, start, aopify.Logger.CurrentMethod.Name));
                process();
                DateTime end = DateTime.Now;
                TimeSpan duration = end - start;
                aopify.Logger.Logger.Info("{0} : End Time :{1} , Method Name: {2}".FormatWith(endMessage, end, aopify.Logger.CurrentMethod.Name));
            });
        }

        [DebuggerStepThrough]
        public static AOP HowLong(this AOP aopify)
        {
            return aopify.Combine(process =>
            {
                DateTime start = DateTime.Now;
                aopify.Logger.Logger.Info("Start Time : {0}, Method Name : {1}".FormatWith(start, aopify.Logger.CurrentMethod.Name));
                process();
                DateTime end = DateTime.Now;
                TimeSpan duration = end - start;
                aopify.Logger.Logger.Info("End Time : {0}, Method Name :{1}, TotalMs: {2}".FormatWith(end, aopify.Logger.CurrentMethod.Name, duration.TotalMilliseconds));
            });
        }

        [DebuggerStepThrough]
        public static AOP Catch(this AOP aopify, Action<Exception> catchAction)
        {
            return aopify.Combine(process =>
                                      {
                                          try
                                          {
                                              process();
                                          }
                                          catch (Exception exception)
                                          {
                                              catchAction(exception);
                                          }
                                      });
        }

        [DebuggerStepThrough]
        public static AOP CatchAndThrow(this AOP aspect, Action<Exception> catchAction)
        {
            return aspect.Combine(process =>
                                      {
                                          try
                                          {
                                              process();
                                          }
                                          catch (Exception exception)
                                          {
                                              catchAction(exception);
                                              throw;
                                          }
                                      });
        }

        [DebuggerStepThrough]
        public static AOP ProcessAsync(this AOP aspect, Action completeCallback)
        {
            return aspect.Combine(process => process.BeginInvoke(asyncresult =>
                                                                 {
                                                                     process.EndInvoke(asyncresult); completeCallback();
                                                                 }, null));
        }

        [DebuggerStepThrough]
        public static AOP ProcessAsync(this AOP aspect)
        {
            return aspect.Combine(process => process.BeginInvoke(process.EndInvoke, null));
        }

        [DebuggerStepThrough]
        public static AOP WithTransaction(this AOP aspect, Action completeCallback, Action rollbackCallback, TransactionOptions transactionOptions, TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
        {
            return aspect.Combine(process => InnerTransactionInvoker(completeCallback, rollbackCallback, transactionOptions, transactionScopeOption, process));
        }
        [DebuggerStepThrough]
        public static AOP WithTransaction(this AOP aspect, TransactionOptions transactionOptions, TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
        {
            return aspect.Combine(process => InnerTransactionInvoker(null, null, transactionOptions, transactionScopeOption, process));
        }

        [DebuggerStepThrough]
        public static AOP WithTransaction(this AOP aspect, Action completeCallback, Action rollbackCallback, TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required)
        {
            return aspect.Combine(process => InnerTransactionInvoker(completeCallback, rollbackCallback, new TransactionOptions(), transactionScopeOption, process));
        }

        private static void InnerTransactionInvoker(Action completeCallback, Action rollbackCallback, TransactionOptions transactionOptions, TransactionScopeOption transactionScopeOption, Action process)
        {
            using (TransactionScope scope = new TransactionScope(transactionScopeOption, transactionOptions))
            {
                try
                {
                    process();
                    scope.Complete();
                    completeCallback?.Invoke();
                }
                catch (Exception)
                {
                    rollbackCallback?.Invoke();
                }
            }
        }
    }
}