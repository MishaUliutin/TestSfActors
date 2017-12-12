using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace TestActor
{
    public class CustomActorService : ActorService
    {
        public CustomActorService(StatefulServiceContext context, ActorTypeInformation actorTypeInfo, Func<ActorService, ActorId, ActorBase> actorFactory = null, Func<ActorBase, IActorStateProvider, IActorStateManager> stateManagerFactory = null, IActorStateProvider stateProvider = null, ActorServiceSettings settings = null) 
            : base(context, actorTypeInfo, actorFactory, stateManagerFactory, stateProvider, settings)
        {
        }

        protected override void OnAbort()
        {
            ActorEventSource.Current.Message(
                $"Start OnAbort: NodeName={Context.NodeContext.NodeName}, ReplicaId={Context.ReplicaId}, PartitionId={Context.PartitionId}");

            base.OnAbort();

            ActorEventSource.Current.Message(
                $"Stop OnAbort: NodeName={Context.NodeContext.NodeName}, ReplicaId={Context.ReplicaId}, PartitionId={Context.PartitionId}");
        }

        protected override async Task OnChangeRoleAsync(ReplicaRole newRole, CancellationToken cancellationToken)
        {
            ActorEventSource.Current.Message(
                $"Start OnChangeRoleAsync: newRole={newRole}, NodeName={Context.NodeContext.NodeName}, ReplicaId={Context.ReplicaId}, PartitionId={Context.PartitionId}");

            await base.OnChangeRoleAsync(newRole, cancellationToken);

            ActorEventSource.Current.Message(
                $"Stop OnChangeRoleAsync: newRole={newRole}, NodeName={Context.NodeContext.NodeName}, ReplicaId={Context.ReplicaId}, PartitionId={Context.PartitionId}");
        }

        protected override async Task OnCloseAsync(CancellationToken cancellationToken)
        {
            ActorEventSource.Current.Message(
                $"Start OnCloseAsync: NodeName={Context.NodeContext.NodeName}, ReplicaId={Context.ReplicaId}, PartitionId={Context.PartitionId}");

            await base.OnCloseAsync(cancellationToken);

            ActorEventSource.Current.Message(
                $"Stop OnCloseAsync: NodeName={Context.NodeContext.NodeName}, ReplicaId={Context.ReplicaId}, PartitionId={Context.PartitionId}");
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            ActorEventSource.Current.Message(
                $"Start RunAsync: NodeName={Context.NodeContext.NodeName}, ReplicaId={Context.ReplicaId}, PartitionId={Context.PartitionId}");

            await base.RunAsync(cancellationToken);

            ActorEventSource.Current.Message(
                $"Stop RunAsync: NodeName={Context.NodeContext.NodeName}, ReplicaId={Context.ReplicaId}, PartitionId={Context.PartitionId}");
        }
    }
}