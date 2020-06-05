﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace SmartStore.Services.Cms.Blocks
{
    /// <summary>
    /// When implemented on <see cref="IBlock"/> types, makes the block bindable to
    /// product, category and manufacturer entities. The UI will display a 'Data binding'
    /// section which allows selection of an entity.
    /// </summary>
    /// <remarks>
    /// The handler for a bindable block type MUST implement <see cref="IBindableBlockHandler"/>.
    /// </remarks>
    public interface IBindableBlock : IBlock
    {
        #region Public Properties

        /// <summary>
        /// The id of the bound entity.
        /// </summary>
        [JsonIgnore]
        int? BindEntityId { get; set; }

        /// <summary>
        /// The name of the bound entity, e.g. 'product'.
        /// </summary>
        [JsonIgnore]
        string BindEntityName { get; set; }

        /// <summary>
        /// Returns a value to indicate whether the block can be bound. A block can be bound
		/// if both <see cref="BindEntityName"/> and <see cref="BindEntityId"/> have values.
		/// However, this property does not indicate whether the bound entity actually exists.
        /// </summary>
        bool CanBind { get; }

        /// <summary>
        /// The data item of the bound entity.
        /// </summary>
        IDictionary<string, object> DataItem { get; set; }

        /// <summary>
        /// Returns a value to indicate whether the block has been bound already.
        /// A block is considered bound if the binding source dictionary has been
        /// applied to the block instance's bindable properties.
        /// </summary>
        bool IsBound { get; set; }

        /// <summary>
        /// Returns a value to indicate whether the binding source (<see cref="DataItem"/>) has been loaded from the store already.
        /// </summary>
        bool IsLoaded { get; }

        #endregion Public Properties



        #region Public Methods

        /// <summary>
        /// Resets the data item of the bound entity. After calling this method,
        /// <see cref="DataItem"/> is <c>null</c>, <see cref="IsLoaded"/> and <see cref="IsBound"/> are <c>false</c>.
        /// </summary>
        void Reset();

        #endregion Public Methods
    }

    public abstract class BindableBlockBase : IBindableBlock
    {
        #region Public Properties

        public virtual int? BindEntityId { get; set; }

        public virtual string BindEntityName { get; set; }

        [JsonIgnore]
        public bool CanBind
        {
            get
            {
                return BindEntityName.HasValue() && BindEntityId.HasValue;
            }
        }

        [JsonIgnore]
        public IDictionary<string, object> DataItem { get; set; }

        [JsonIgnore]
        public bool IsBound { get; set; }

        [JsonIgnore]
        public bool IsLoaded
        {
            get { return DataItem != null; }
        }

        #endregion Public Properties



        #region Public Methods

        public void Reset()
        {
            DataItem = null;
            IsBound = false;
        }

        #endregion Public Methods
    }
}