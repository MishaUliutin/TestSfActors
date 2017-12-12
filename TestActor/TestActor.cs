using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using TestActor.Interfaces;

namespace TestActor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Volatile)]
    internal class TestActor : Actor, ITestActor, IRemindable
    {
        /// <summary>
        /// Initializes a new instance of TestActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public TestActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated");

            return base.OnActivateAsync();
        }

        protected override Task OnDeactivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor deactivated");
            return base.OnDeactivateAsync();
        }

        public async Task SetValueAsync(int value)
        {
            ActorEventSource.Current.ActorMessage(this, $"Actor SetValue: value={value}");

            await StateManager.AddStateAsync("TestState", value);
            // await StateManager.SaveStateAsync();
        }

        public async Task UpdateValueAsync(int add)
        {
            ActorEventSource.Current.ActorMessage(this, $"Actor UpdateValue: add={add}");

            // var state = await StateManager.TryGetStateAsync<int>("TestState");
            var state = await StateManager.GetStateAsync<int>("TestState");
            await StateManager.SetStateAsync("TestState", state + add);
            // await StateManager.SaveStateAsync();
        }

        public async Task UpdateValueAndRegisterReminderAsync(int add)
        {
            ActorEventSource.Current.ActorMessage(this, $"Actor UpdateValueAndRegisterReminder: add={add}");

            await RegisterReminderAsync(
                "TestReminder", null, TimeSpan.FromHours(2), TimeSpan.FromHours(2));

            var state = await StateManager.GetStateAsync<int>("TestState");
            await StateManager.SetStateAsync("TestState", state + add);
        }

        public async Task<Result> GetValueAsync()
        {
            var state = await StateManager.TryGetStateAsync<int>("TestState");

            bool val;
            try
            {
                val = GetReminder("TestReminder") != null;
            }
            catch
            {
                val = false;
            }

            ActorEventSource.Current.ActorMessage(this, $"Actor GetValue: Value={(state.HasValue ? state.Value : -1)}, ReminderExists={val}");

            return new Result
            {
                Value = state.HasValue ? state.Value : -1,
                ReminderExists = val,
                NodeName = ActorService.Context.NodeContext.NodeName,
                PartitionId = ActorService.Context.PartitionId.ToString("D"),
                ReplicaId = ActorService.Context.ReplicaId.ToString()
            };
        }

        public Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
        {
            ActorEventSource.Current.ActorMessage(this, $"Actor ReceiveReminder: reminderName={reminderName}");

            return Task.CompletedTask;
        }
    }
}
