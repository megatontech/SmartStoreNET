using SmartStore.Core;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Localization;
using System;
using System.Linq.Expressions;

namespace SmartStore.Services.Customers
{
    /// <summary>
    /// TODO
    /// </summary>
    public class CustomerAnonymizedEvent
    {
        #region Private Fields

        private readonly IGdprTool _gdprTool;

        #endregion Private Fields

        #region Public Constructors

        public CustomerAnonymizedEvent(Customer customer, IGdprTool gdprTool)
        {
            Guard.NotNull(customer, nameof(customer));

            Customer = customer;
            _gdprTool = gdprTool;
        }

        #endregion Public Constructors



        #region Public Properties

        public Customer Customer { get; private set; }

        #endregion Public Properties



        #region Public Methods

        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expression"></param>
        /// <param name="type"></param>
        public void AnonymizeData<TEntity>(TEntity entity, Expression<Func<TEntity, object>> expression, IdentifierDataType type, Language language = null)
            where TEntity : BaseEntity
        {
            _gdprTool.AnonymizeData(entity, expression, type, language);
        }

        #endregion Public Methods
    }
}