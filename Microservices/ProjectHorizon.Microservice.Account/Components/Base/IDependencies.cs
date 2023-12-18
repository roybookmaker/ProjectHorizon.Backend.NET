using MassTransit;
using MassTransit.Mediator;

namespace ProjectHorizon.Microservice.Account.Components.Base
{
    public interface IDependencies
    {
        public IClientFactory ClientFactory { get; }
        public IMediator Mediator { get; }
    }
}
