using SmartStore.Collections;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartStore.Services.DataExchange.Export.Internal
{
    public class CustomerExportContext
    {
        #region Protected Fields

        protected List<int> _customerIds;

        #endregion Protected Fields

        #region Private Fields

        private Func<int[], Multimap<int, GenericAttribute>> _funcGenericAttributes;

        private LazyMultimap<GenericAttribute> _genericAttributes;

        #endregion Private Fields

        #region Public Constructors

        public CustomerExportContext(
            IEnumerable<Customer> customers,
            Func<int[], Multimap<int, GenericAttribute>> genericAttributes)
        {
            if (customers == null)
            {
                _customerIds = new List<int>();
            }
            else
            {
                _customerIds = new List<int>(customers.Select(x => x.Id));
            }

            _funcGenericAttributes = genericAttributes;
        }

        #endregion Public Constructors



        #region Public Properties

        public LazyMultimap<GenericAttribute> GenericAttributes
        {
            get
            {
                if (_genericAttributes == null)
                {
                    _genericAttributes = new LazyMultimap<GenericAttribute>(keys => _funcGenericAttributes(keys), _customerIds);
                }
                return _genericAttributes;
            }
        }

        #endregion Public Properties



        #region Public Methods

        public void Clear()
        {
            if (_genericAttributes != null)
                _genericAttributes.Clear();

            _customerIds.Clear();
        }

        #endregion Public Methods
    }
}