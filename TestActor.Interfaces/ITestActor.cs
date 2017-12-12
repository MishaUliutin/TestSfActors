using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace TestActor.Interfaces
{
    [DataContract]
    public class Result
    {
        [DataMember(Order = 1)]
        public int Value { get; set; }

        [DataMember(Order = 2)]
        public bool ReminderExists { get; set; }

        [DataMember(Order = 3)]
        public string NodeName { get; set; }

        [DataMember(Order = 4)]
        public string PartitionId { get; set; }

        [DataMember(Order = 5)]
        public string ReplicaId { get; set; }
    }

    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface ITestActor : IActor
    {
        Task SetValueAsync(int value);

        Task UpdateValueAsync(int add);

        Task UpdateValueAndRegisterReminderAsync(int add);

        Task<Result> GetValueAsync();
    }
}
