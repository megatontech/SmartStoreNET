using System;

namespace SmartStore.Services.DataExchange.Export.Deployment
{
    [Serializable]
    public class DataDeploymentResult
    {
        #region Public Properties

        public string LastError { get; set; }

        /// <summary>
        /// Last execution date
        /// </summary>
        public DateTime LastExecutionUtc { get; set; }

        /// <summary>
        /// Whether the deployment succeeded
        /// </summary>
        public bool Succeeded
        {
            get { return LastError.IsEmpty(); }
        }

        #endregion Public Properties
    }
}