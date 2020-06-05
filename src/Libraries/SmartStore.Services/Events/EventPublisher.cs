using SmartStore.ComponentModel;
using SmartStore.Core.Events;
using SmartStore.Core.Logging;
using System.Linq;

namespace SmartStore.Services.Events
{
    public class EventPublisher : IEventPublisher
    {
        #region Private Fields

        private readonly IConsumerInvoker _invoker;

        private readonly IConsumerRegistry _registry;

        private readonly IConsumerResolver _resolver;

        #endregion Private Fields

        #region Public Constructors

        public EventPublisher(IConsumerRegistry registry, IConsumerResolver resolver, IConsumerInvoker invoker)
        {
            _registry = registry;
            _resolver = resolver;
            _invoker = invoker;

            Logger = NullLogger.Instance;
        }

        #endregion Public Constructors



        #region Public Properties

        public ILogger Logger { get; set; }

        #endregion Public Properties



        #region Public Methods

        public void Publish<T>(T message) where T : class
        {
            var descriptors = _registry.GetConsumers(message);

            if (!descriptors.Any())
            {
                return;
            }

            var envelopeType = typeof(ConsumeContext<>).MakeGenericType(typeof(T));
            var envelope = (ConsumeContext<T>)FastActivator.CreateInstance(envelopeType, message);

            foreach (var descriptor in descriptors)
            {
                var consumer = _resolver.Resolve(descriptor);
                if (consumer != null)
                {
                    _invoker.Invoke(descriptor, consumer, envelope);
                }
            }
        }

        #endregion Public Methods
    }
}