using SmartStore.Core.Plugins;
using System;

namespace SmartStore.Services.Cms.Blocks
{
    /// <summary>
    /// Represents block registration metadata
    /// </summary>
    public interface IBlockMetadata : IProviderMetadata
    {
        #region Public Properties

        string AreaName { get; }

        Type BlockClrType { get; }

        Type BlockHandlerClrType { get; }

        string Icon { get; }

        bool IsInbuilt { get; }

        bool IsInternal { get; }

        #endregion Public Properties
    }

    /// <summary>
    /// Applies metadata to concrete block types which implement <see cref="IBlock"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class BlockAttribute : Attribute
    {
        #region Public Constructors

        public BlockAttribute(string systemName)
        {
            Guard.NotNull(systemName, nameof(systemName));

            SystemName = systemName;
        }

        #endregion Public Constructors



        #region Public Properties

        /// <summary>
        /// The order of display
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// The english friendly name of the block
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// The icon class name of the block, e.g. 'fa fa-sitemap'
        /// </summary>
        public string Icon { get; set; }

        public bool IsInternal { get; set; }

        /// <summary>
        /// The block system name, e.g. 'html', 'picture' etc.
        /// </summary>
        public string SystemName { get; set; }

        #endregion Public Properties
    }

    public class BlockMetadata : IBlockMetadata, ICloneable<BlockMetadata>
    {
        #region Public Properties

        public string AreaName { get; set; }

        public Type BlockClrType { get; set; }

        public Type BlockHandlerClrType { get; set; }

        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        public string FriendlyName { get; set; }

        public string Icon { get; set; }

        public bool IsInbuilt { get; set; }

        public bool IsInternal { get; set; }

        public string ResourceKeyPattern { get; set; }

        public string SystemName { get; set; }

        #endregion Public Properties



        #region Public Methods

        public BlockMetadata Clone()
        {
            return (BlockMetadata)this.MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion Public Methods
    }
}