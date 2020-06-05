using SmartStore.Core;
using SmartStore.Core.Domain.Localization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SmartStore.Services.Localization
{
    /// <summary>
    /// Localized entity service interface
    /// </summary>
    public partial interface ILocalizedEntityService : IScopedService
    {
        #region Public Methods

        /// <summary>
        /// Deletes a localized property
        /// </summary>
        /// <param name="localizedProperty">Localized property</param>
        void DeleteLocalizedProperty(LocalizedProperty localizedProperty);

        /// <summary>
        /// Gets localized properties
        /// </summary>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="localeKeyGroup">Locale key group</param>
        /// <returns>Localized properties</returns>
        IList<LocalizedProperty> GetLocalizedProperties(int entityId, string localeKeyGroup);

        /// <summary>
        /// Gets a localized property
        /// </summary>
        /// <param name="localizedPropertyId">Localized property identifier</param>
        /// <returns>Localized property</returns>
        LocalizedProperty GetLocalizedPropertyById(int localizedPropertyId);

        /// <summary>
        /// Gets a collection of localized properties for a range of entities in one go.
        /// </summary>
        /// <param name="localeKeyGroup">Locale key group (scope)</param>
        /// <param name="entityIds">
        /// The entity ids to load translations for. Can be null,
        /// in which case all translations for the requested scope are loaded.
        /// </param>
        /// <param name="isRange">Whether <paramref name="entityIds"/> represents a range of ids (perf).</param>
        /// <param name="isSorted">Whether <paramref name="entityIds"/> is already sorted (perf).</param>
        /// <returns>Localized property collection</returns>
        /// <remarks>
        /// Be careful not to load large amounts of data at once (e.g. for "Product" scope with large range).
        /// </remarks>
        LocalizedPropertyCollection GetLocalizedPropertyCollection(string localeKeyGroup, int[] entityIds, bool isRange = false, bool isSorted = false);

        /// <summary>
        /// Find localized value
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="localeKeyGroup">Locale key group</param>
        /// <param name="localeKey">Locale key</param>
        /// <returns>Found localized value</returns>
        string GetLocalizedValue(int languageId, int entityId, string localeKeyGroup, string localeKey);

        /// <summary>
        /// Inserts a localized property
        /// </summary>
        /// <param name="localizedProperty">Localized property</param>
        void InsertLocalizedProperty(LocalizedProperty localizedProperty);

        /// <summary>
        /// Prefetches a collection of localized properties for a range of entities in one go
        /// and caches them for the duration of the current request.
        /// </summary>
        /// <param name="localeKeyGroup">Locale key group (scope)</param>
        /// <param name="entityIds">
        /// The entity ids to prefetch translations for. Can be null,
        /// in which case all translations for the requested scope are loaded.
        /// </param>
        /// <param name="isRange">Whether <paramref name="entityIds"/> represents a range of ids (perf).</param>
        /// <param name="isSorted">Whether <paramref name="entityIds"/> is already sorted (perf).</param>
        /// <returns>Localized property collection</returns>
        /// <remarks>
        /// Be careful not to load large amounts of data at once (e.g. for "Product" scope with large range).
        /// </remarks>
        void PrefetchLocalizedProperties(string localeKeyGroup, int languageId, int[] entityIds, bool isRange = false, bool isSorted = false);

        /// <summary>
        /// Save localized value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="localeValue">Locale value</param>
        /// <param name="languageId">Language ID</param>
        void SaveLocalizedValue<T>(
            T entity,
            Expression<Func<T, string>> keySelector,
            string localeValue,
            int languageId) where T : BaseEntity, ILocalizedEntity;

        void SaveLocalizedValue<T, TPropType>(
                   T entity,
                   Expression<Func<T, TPropType>> keySelector,
                   TPropType localeValue,
                   int languageId) where T : BaseEntity, ILocalizedEntity;

        /// <summary>
        /// Updates the localized property
        /// </summary>
        /// <param name="localizedProperty">Localized property</param>
        void UpdateLocalizedProperty(LocalizedProperty localizedProperty);

        #endregion Public Methods
    }
}