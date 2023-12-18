using MassTransit;
using MassTransit.Mediator;

namespace ProjectHorizon.Microservice.Account.Components.Base
{
    public class Dependencies : IDependencies
    {
        public IClientFactory ClientFactory { get; }
        public IMediator Mediator { get; }

        public Dependencies(IClientFactory clientFactory, IMediator mediator)
        {
            ClientFactory = clientFactory;
            Mediator = mediator;

        }
    }
}
