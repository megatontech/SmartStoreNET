using SmartStore.Core.Data;
using SmartStore.Core.Domain.Directory;
using SmartStore.Core.Events;
using SmartStore.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartStore.Services.Directory
{
    public partial class StateProvinceService : IStateProvinceService
    {
        #region Private Fields

        private readonly IEventPublisher _eventPublisher;

        private readonly IRepository<StateProvince> _stateProvinceRepository;

        #endregion Private Fields

        #region Public Constructors

        public StateProvinceService(
            IRepository<StateProvince> stateProvinceRepository,
            IEventPublisher eventPublisher)
        {
            _stateProvinceRepository = stateProvinceRepository;
            _eventPublisher = eventPublisher;
        }

        #endregion Public Constructors



        #region Public Methods

        public virtual void DeleteStateProvince(StateProvince stateProvince)
        {
            if (stateProvince == null)
                throw new ArgumentNullException("stateProvince");

            _stateProvinceRepository.Delete(stateProvince);
        }

        public virtual IQueryable<StateProvince> GetAllStateProvinces(bool showHidden = false)
        {
            var query = _stateProvinceRepository.Table;

            if (!showHidden)
                query = query.Where(x => x.Published);

            return query;
        }

        public virtual StateProvince GetStateProvinceByAbbreviation(string abbreviation)
        {
            var query = from sp in _stateProvinceRepository.Table
                        where sp.Abbreviation == abbreviation
                        select sp;
            var stateProvince = query.FirstOrDefault();
            return stateProvince;
        }

        public virtual StateProvince GetStateProvinceById(int stateProvinceId)
        {
            if (stateProvinceId == 0)
                return null;

            return _stateProvinceRepository.GetById(stateProvinceId);
        }

        public virtual IList<StateProvince> GetStateProvincesByCountryId(int countryId, bool showHidden = false)
        {
            var query = from sp in _stateProvinceRepository.Table
                        orderby sp.DisplayOrder
                        where sp.CountryId == countryId &&
                        (showHidden || sp.Published)
                        select sp;

            var stateProvinces = query.ToListCached("db.regions.{0}.{1}".FormatInvariant(countryId, showHidden));
            return stateProvinces;
        }

        public virtual void InsertStateProvince(StateProvince stateProvince)
        {
            if (stateProvince == null)
                throw new ArgumentNullException("stateProvince");

            _stateProvinceRepository.Insert(stateProvince);
        }

        public virtual void UpdateStateProvince(StateProvince stateProvince)
        {
            if (stateProvince == null)
                throw new ArgumentNullException("stateProvince");

            _stateProvinceRepository.Update(stateProvince);
        }

        #endregion Public Methods
    }
}