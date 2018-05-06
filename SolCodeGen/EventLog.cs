using System;
using System.Collections.Generic;
using System.Text;

namespace SolCodeGen
{
    public abstract class EventLog<TLogData>
    {
        public readonly TLogData LogData;

        public EventLog TransactionHash { get; set; }

        public EventLog(TLogData logData, EventLog eventLog)
        {
            LogData = logData;
        }

    }

    public class ExampleEvent : EventLog<(string test, int dd)>
    {
        public ExampleEvent((string test, int dd) logData, EventLog eventLog) : base(logData, eventLog)
        {
        }
    }

    public class EventLog
    {
        // TODO

    }
}
