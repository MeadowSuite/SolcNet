using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SolCodeGen
{
    public abstract class EventLog
    {
        public virtual string EventName { get; }

        /// <summary>
        /// address from which this log originated
        /// </summary>
        public Address Address { get; protected set; }

        /// <summary>
        /// hash of the block where this log was in. null when its pending
        /// </summary>
        public Hash? BlockHash { get; protected set; }

        /// <summary>
        /// the block number where this log was in. null when its pending
        /// </summary>
        public long? BlockNumber { get; protected set; }

        /// <summary>
        /// integer of the log index position in the block
        /// </summary>
        public long? LogIndex { get; set; }

        /// <summary>
        /// hash of the transactions this log was created from. null when its pending log
        /// </summary>
        public Hash? TransactionHash { get; protected set; }

        /// <summary>
        /// The arguments coming from the event
        /// </summary>
        public (string Name, string Type, bool Index, object Value)[] LogArgs { get; protected set; }

        public EventLog(Address address, Hash? blockHash, long? blockNumber, long? logIndex, Hash? transactionHash)
        {
            Address = address;
            BlockHash = blockHash;
            BlockNumber = blockNumber;
            LogIndex = logIndex;
            TransactionHash = transactionHash;
        }

        protected (string, string, bool, object) Box<T>((string Name, string Type, bool Index, T Value) arg)
        {
            return (arg.Name, arg.Type, arg.Index, arg.Value);
        }
    }

    public class Event_TransferExample : EventLog
    {
        public override string EventName { get; } = "TransferExample";

        public readonly (string Name, string Type, bool Indexed, Address Value) from;
        public readonly (string Name, string Type, bool Indexed, Address Value) to;
        public readonly (string Name, string Type, bool Indexed, UInt256 Value) value;

        public Event_TransferExample(
            Address address, Hash? blockHash, long? blockNumber, long? logIndex, Hash? transactionHash,
            Address _from, Address _to, UInt256 _value)
            : base(address, blockHash, blockNumber, logIndex, transactionHash)
        {
            from = ("from", "address", true, _from);
            to = ("to", "address", true, _to);
            value = ("value", "uint256", false, _value);

            LogArgs = new[] { Box(from), Box(to), Box(value) };
        }
    }
}
