using System;
using System.Diagnostics;

namespace AOPify
{
    public class AOPify
    {

        internal Action<Action> ActionChain;
        internal Delegate ProcessDelegate;
        internal Action BeforeAction;
        internal Action AfterAction;

        internal Log Logger;

        [DebuggerStepThrough]
        public AOPify Combine(Action<Action> newAspectDelegate)
        {
            if (ActionChain == null)
            {
                ActionChain = newAspectDelegate;
            }
            else
            {
                Action<Action> existingChain = ActionChain;
                Action<Action> callAnother = process => existingChain(() => newAspectDelegate(process));
                ActionChain = callAnother;
            }
            return this;
        }
        [DebuggerStepThrough]
        public void Run(Action process)
        {
            if (BeforeAction != null) BeforeAction();

            if (ActionChain == null)
            {
                process();
            }
            else
            {
                ActionChain(process);
            }

            if (AfterAction != null) AfterAction();
        }

        [DebuggerStepThrough]
        public TReturnType Return<TReturnType>(Func<TReturnType> process)
        {
            ProcessDelegate = process;

            if (ActionChain == null)
            {
                return process();
            }
            TReturnType returnValue = default(TReturnType);
            ActionChain(() =>
                      {
                          Func<TReturnType> processDelagate = ProcessDelegate as Func<TReturnType>;
                          if (processDelagate != null)
                          {
                              returnValue = processDelagate();
                          }
                      });
            return returnValue;
        }

        public AOPify RegisterLogger(Log logger)
        {
            Logger = logger;
            return this;
        }

        public static AOPify Let
        {
            [DebuggerStepThrough]
            get
            {
                return new AOPify();
            }
        }
    }
}
