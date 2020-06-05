using Autofac;
using SmartStore.Core.Data;
using SmartStore.Core.Data.Hooks;
using SmartStore.Core.Domain.Customers;
using System.Collections.Generic;
using System.Linq;

namespace SmartStore.Services.Hooks
{
    [Important]
    public class UpdateCustomerFullNameHook : DbSaveHook<Customer>
    {
        #region Private Fields

        private static readonly HashSet<string> _candidateProps = new HashSet<string>(new string[]
        {
            nameof(Customer.Title),
            nameof(Customer.Salutation),
            nameof(Customer.FirstName),
            nameof(Customer.LastName)
        });

        private readonly IComponentContext _ctx;

        #endregion Private Fields

        #region Public Constructors

        public UpdateCustomerFullNameHook(IComponentContext ctx)
        {
            _ctx = ctx;
        }

        #endregion Public Constructors



        #region Protected Methods

        protected override void OnInserting(Customer entity, IHookedEntity entry)
        {
            UpdateFullName(entity, entry);
        }

        protected override void OnUpdating(Customer entity, IHookedEntity entry)
        {
            UpdateFullName(entity, entry);
        }

        #endregion Protected Methods

        #region Private Methods

        private void UpdateFullName(Customer entity, IHookedEntity entry)
        {
            var shouldUpdate = entity.IsTransientRecord();

            if (!entity.IsTransientRecord())
            {
                var modProps = _ctx.Resolve<IDbContext>().GetModifiedProperties(entity);
                shouldUpdate = _candidateProps.Any(x => modProps.ContainsKey(x));
            }

            if (shouldUpdate)
            {
                var parts = new[]
                {
                    entity.Salutation,
                    entity.Title,
                    entity.FirstName,
                    entity.LastName
                };

                entity.FullName = string.Join(" ", parts.Where(x => x.HasValue())).NullEmpty();
            }
        }

        #endregion Private Methods
    }
}