using System;

namespace AOPify.Aspects.Common
{
    internal class ExecutionTimer
    {
        public DateTime StartDate { get; set; }
        public TimeSpan Duration
        {
            get
            {
                if (StartDate > default(DateTime) && EndDate > default(DateTime))
                {
                    return EndDate - StartDate;
                }

                throw new InvalidOperationException("method start and  end date is null");
            }
        }

        public DateTime EndDate { get; set; }

        public string Operation { get; set; }

        public void Start(string operation)
        {
            Operation = operation;
            StartDate = DateTime.Now;
        }

        public void Finish()
        {
            EndDate = DateTime.Now;
        }
    }









































}
