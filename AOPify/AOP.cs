using System;
using AOPify.Logging;
using System.Diagnostics;

namespace AOPify
{
    // ReSharper disable once InconsistentNaming
    public class AOP
    {
        internal Action<Action> ActionChain;
        internal Delegate ProcessDelegate;
        internal Action BeforeAction;
        internal Action AfterAction;

        internal Loggr Logger;

        [DebuggerStepThrough]
        internal AOP Combine(Action<Action> newAspectDelegate)
        {
            if (ActionChain == null)
            {
                ActionChain = newAspectDelegate;
            }
            else
            {
                Action<Action> existingChain = ActionChain;
                void CallAnother(Action process) => existingChain(() => newAspectDelegate(process));
                ActionChain = CallAnother;
            }
            return this;
        }
        [DebuggerStepThrough]
        public void Run(Action process)
        {
            BeforeAction?.Invoke();

            if (ActionChain == null)
            {
                process();
            }
            else
            {
                ActionChain(process);
            }

            AfterAction?.Invoke();
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

        public AOP RegisterLogger(Loggr logger)
        {
            if (logger?.Logger == null)
            {
                Logger = Loggr.Instance.Using<NullLogger>();
            }
            Logger = logger;
            return this;
        }

        public static AOP Instance
        {
            [DebuggerStepThrough]
            get
            {
                return new AOP();
            }
        }
    }
}
