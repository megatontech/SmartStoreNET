﻿using System;

namespace SmartStore.Services.Cms
{
    public enum LinkStatus
    {
        Ok,

        Forbidden,

        NotFound,

        Hidden
    }

    public enum LinkType
    {
        Product = 0,

        Category,

        Manufacturer,

        Topic,

        Url = 20,

        File = 30
    }

    public static class LinkResolverExtensions
    {
        #region Public Methods

        /// <summary>
        /// Creates the full link expression including type, value and query string.
        /// </summary>
        /// <param name="includeQueryString">Whether to include the query string.</param>
        /// <returns>Link expression.</returns>
        public static string CreateExpression(this LinkResolverResult data, bool includeQueryString = true)
        {
            if (data?.Value == null)
            {
                return string.Empty;
            }

            var result = data.Type == LinkType.Url
                ? data.Value.ToString()
                : string.Concat(data.Type.ToString().ToLower(), ":", data.Value.ToString());

            if (includeQueryString && data.Type != LinkType.Url && !string.IsNullOrWhiteSpace(data.QueryString))
            {
                return string.Concat(result, "?", data.QueryString);
            }

            return result;
        }

        public static (string Icon, string ResKey) GetLinkTypeInfo(this LinkType type)
        {
            switch (type)
            {
                case LinkType.Product:
                    return ("fa fa-cube", "Common.Entity.Product");

                case LinkType.Category:
                    return ("fa fa-sitemap", "Common.Entity.Category");

                case LinkType.Manufacturer:
                    return ("far fa-building", "Common.Entity.Manufacturer");

                case LinkType.Topic:
                    return ("far fa-file-alt", "Common.Entity.Topic");

                case LinkType.Url:
                    return ("fa fa-link", "Common.Url");

                case LinkType.File:
                    return ("far fa-folder-open", "Common.File");

                default:
                    throw new SmartException("Unknown link builder type.");
            }
        }

        #endregion Public Methods
    }

    [Serializable]
    public partial class LinkResolverData : LinkResolverResult, ICloneable<LinkResolverData>
    {
        #region Public Properties

        public bool CheckLimitedToStores { get; set; } = true;

        public bool LimitedToStores { get; set; }

        public bool SubjectToAcl { get; set; }

        #endregion Public Properties



        #region Public Methods

        public LinkResolverData Clone()
        {
            return (LinkResolverData)this.MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion Public Methods
    }

    public class LinkResolverResult
    {
        #region Private Fields

        private string _link;

        #endregion Private Fields



        #region Public Properties

        /// <summary>
        /// The raw expression without query string.
        /// </summary>
        public string Expression { get; set; }

        public int Id { get; set; }

        public string Label { get; set; }

        public string Link
        {
            get
            {
                if (Type != LinkType.Url && !string.IsNullOrWhiteSpace(_link) && !string.IsNullOrWhiteSpace(QueryString))
                {
                    return string.Concat(_link, "?", QueryString);
                }

                return _link;
            }

            set
            {
                _link = value;

                if (_link != null && Type != LinkType.Url)
                {
                    var index = _link.IndexOf('?');
                    if (index != -1)
                    {
                        QueryString = _link.Substring(index + 1);
                        _link = _link.Substring(0, index);
                    }
                }
            }
        }

        public int? PictureId { get; set; }

        /// <summary>
        /// The query string part.
        /// </summary>
        public string QueryString { get; set; }

        public string Slug { get; set; }

        public LinkStatus Status { get; set; }

        public LinkType Type { get; set; }

        public object Value { get; set; }

        #endregion Public Properties



        #region Public Methods

        public override string ToString()
        {
            return this.Link.EmptyNull();
        }

        #endregion Public Methods
    }
}