using System;

namespace SmartStore.Services.Events
{
    public interface IMessageBus
    {
        #region Public Methods

        void Publish(string channel, string message);

        void Subscribe(string channel, Action<string, string> handler);

        #endregion Public Methods
    }
}