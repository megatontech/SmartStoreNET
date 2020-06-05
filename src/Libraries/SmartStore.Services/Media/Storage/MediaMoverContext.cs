﻿using System.Collections.Generic;

namespace SmartStore.Services.Media.Storage
{
    public class MediaMoverContext
    {
        #region Internal Constructors

        internal MediaMoverContext(string sourceSystemName, string targetSystemName)
        {
            SourceSystemName = sourceSystemName;
            TargetSystemName = targetSystemName;
            AffectedFiles = new List<string>();
            CustomProperties = new Dictionary<string, object>();
        }

        #endregion Internal Constructors



        #region Public Properties

        /// <summary>
        /// Paths of affected media files
        /// </summary>
        public List<string> AffectedFiles { get; private set; }

        /// <summary>
        /// Use this dictionary for any custom data required along the move operation
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; }

        /// <summary>
        /// Current number of moved media items
        /// </summary>
        public int MovedItems { get; internal set; }

        /// <summary>
        /// Whether to shrink database after succesful moving
        /// </summary>
        public bool ShrinkDatabase { get; set; }

        /// <summary>
        /// The system name of the source provider
        /// </summary>
        public string SourceSystemName { get; private set; }

        /// <summary>
        /// The system name of the target provider
        /// </summary>
        public string TargetSystemName { get; private set; }

        #endregion Public Properties
    }
}