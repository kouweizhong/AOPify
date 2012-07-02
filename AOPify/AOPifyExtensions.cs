using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AOPify
{
    public static class AOPifyExtensions
    {

        [DebuggerStepThrough]
        public static AOPify Delay(this AOPify aopify, int milliseconds)
        {
            return aopify.Combine(process =>
                                      {
                                          System.Threading.Thread.Sleep(milliseconds);
                                          process();
                                      });
        }

        [DebuggerStepThrough]
        public static AOPify Until(this AOPify aopify, Func<bool> test)
        {
            return aopify.Combine(process =>
                                      {
                                          while (!test()) ;

                                          process();
                                      });
        }

        [DebuggerStepThrough]
        public static AOPify While(this AOPify aopify, Func<bool> test)
        {
            return aopify.Combine(process =>
                                      {
                                          while (test())
                                              process();
                                      });
        }

        [DebuggerStepThrough]
        public static AOPify WhenTrue(this AOPify aopify, params Func<bool>[] conditions)
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
        public static AOPify Log(this AOPify aopify, string beforeMessage, string afterMessage)
        {
            return aopify.Combine(process =>
            {
                aopify.Logger.Logger.Log(beforeMessage);
                process();
                aopify.Logger.Logger.Log(afterMessage);
            });
        }

        [DebuggerStepThrough]
        public static AOPify Log(this AOPify aopify, MethodBase currentMethod)
        {
            return aopify.Combine(process =>
           {
               aopify.Logger.CurrentMethod = currentMethod;
               aopify.Logger.Logger.Log(
                   "START type :{0}, method :{1}".FormatWith(aopify.Logger.Target.Name, currentMethod == null ? process.Method.Name : currentMethod.Name));
               process();
               aopify.Logger.Logger.Log("END type :{0}, method :{1}".FormatWith(aopify.Logger.Target.Name, currentMethod == null ? process.Method.Name : currentMethod.Name));
           });
        }

        [DebuggerStepThrough]
        public static AOPify Before(this AOPify aopify, Action beforeAction)
        {
            aopify.BeforeAction = beforeAction;
            return aopify;
        }

        [DebuggerStepThrough]
        public static AOPify After(this AOPify aopify, Action afterAction)
        {
            aopify.AfterAction = afterAction;
            return aopify;
        }

        [DebuggerStepThrough]
        public static AOPify HowLong(this AOPify aopify, string startMessage, string endMessage)
        {
            return aopify.Combine(process =>
            {
                DateTime start = DateTime.Now;
                aopify.Logger.Logger.Log("{0} {1} Method Name : {2}".FormatWith(startMessage, start, aopify.Logger.CurrentMethod.Name));
                process();
                DateTime end = DateTime.Now;
                TimeSpan duration = end - start;
                aopify.Logger.Logger.Log("{0} : End Time :{1} , Method Name: {2}".FormatWith(endMessage, end, aopify.Logger.CurrentMethod.Name));
            });
        }

        [DebuggerStepThrough]
        public static AOPify HowLong(this AOPify aopify)
        {
            return aopify.Combine(process =>
            {
                DateTime start = DateTime.Now;
                aopify.Logger.Logger.Log("Start Time : {0}, Method Name : {1}".FormatWith(start, aopify.Logger.CurrentMethod.Name));
                process();
                DateTime end = DateTime.Now;
                TimeSpan duration = end - start;
                aopify.Logger.Logger.Log("End Time : {0}, Method Name :{1}, TotalMs: {2}".FormatWith(end, aopify.Logger.CurrentMethod.Name, duration.TotalMilliseconds));
            });
        }

        [DebuggerStepThrough]
        public static AOPify Catch(this AOPify aopify, Action<Exception> catchAction)
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
        public static AOPify CatchAndThrow(this AOPify aspect, Action<Exception> catchAction)
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
        public static AOPify ProcessAsync(this AOPify aspect, Action completeCallback)
        {
            return aspect.Combine(process => process.BeginInvoke(asyncresult =>
                                                                 {
                                                                     process.EndInvoke(asyncresult); completeCallback();
                                                                 }, null));
        }

        [DebuggerStepThrough]
        public static AOPify ProcessAsync(this AOPify aspect)
        {
            return aspect.Combine(process => process.BeginInvoke(process.EndInvoke, null));
        }
    }
}