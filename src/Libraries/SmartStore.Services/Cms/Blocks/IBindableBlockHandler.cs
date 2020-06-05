﻿using System.Collections.Generic;

namespace SmartStore.Services.Cms.Blocks
{
    /// <summary>
    /// Retrieves information about UI mapping between template tokens and form controls.
    /// </summary>
    /// <remarks>
    /// <see cref="IBindableBlockHandler"/> only works in conjunction with <see cref="IBindableBlock"/>.
    /// Make sure that the handled block type implements <see cref="IBindableBlock"/>.
    /// </remarks>
    public interface IBindableBlockHandler : IBlockHandler
    {
        #region Public Methods

        /// <summary>
        /// Build and returns the template mapping configuration.
        /// </summary>
        /// <returns></returns>
        TemplateMappingConfiguration GetMappingConfiguration();

        #endregion Public Methods
    }

    /// <summary>
    /// Block data binding mapping definition for a single field.
    /// </summary>
    public class TemplateMapping
    {
        #region Public Properties

        /// <summary>
        /// Name of the bound field, e.g. 'Title', 'Description', 'Price' etc.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// The 'Liquid' template
        /// </summary>
        public string Template { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    /// Contans information about UI mapping between template tokens and form controls.
    /// </summary>
    public class TemplateMappingConfiguration
    {
        #region Public Properties

        /// <summary>
        /// The complete list of field names which can be bound to entity fields.
        /// </summary>
        public string[] BindableFields { get; set; }

        /// <summary>
        /// A list of template tokens and their corresponding form controls.
        /// </summary>
        public IList<TemplateMapping> Map { get; set; }

        #endregion Public Properties
    }
}