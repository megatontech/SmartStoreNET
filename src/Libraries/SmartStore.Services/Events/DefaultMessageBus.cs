using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartStore.Services.Events
{
    public class DefaultMessageBus : IMessageBus
    {
        #region Private Fields

        private readonly IMessageBroker _messageBroker;

        #endregion Private Fields

        #region Public Constructors

        public DefaultMessageBus(IEnumerable<IMessageBroker> messageBrokers)
        {
            _messageBroker = messageBrokers.FirstOrDefault();
        }

        #endregion Public Constructors



        #region Public Methods

        public void Publish(string channel, string message)
        {
            if (_messageBroker == null)
                return;

            _messageBroker.Publish(channel, message);
        }

        public void Subscribe(string channel, Action<string, string> handler)
        {
            if (_messageBroker == null)
                return;

            _messageBroker.Subscribe(channel, handler);
        }

        #endregion Public Methods
    }
}