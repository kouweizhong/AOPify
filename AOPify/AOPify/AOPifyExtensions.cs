using System;
using System.Diagnostics;
using System.Linq;

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