using System;

namespace SmartStore.Services.DataExchange.Import
{
    [Serializable]
    public class ImportExtraData
    {
        #region Public Properties

        /// <summary>
        /// Number of images per object to be imported
        /// </summary>
        public int? NumberOfPictures { get; set; }

        #endregion Public Properties
    }
}