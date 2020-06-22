using SmartStore.Collections;
using SmartStore.Core;
using SmartStore.Core.Data;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Forums;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Shipping;
using SmartStore.Core.Fakes;
using SmartStore.Core.Localization;
using SmartStore.Core.Logging;
using SmartStore.Data.Caching;
using SmartStore.Services.Common;
using SmartStore.Services.Localization;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace SmartStore.Services.Customers
{
    public partial class CustomerService : ICustomerService
    {
        #region Private Fields

        private readonly IRepository<Customer> _customerRepository;

        private readonly IRepository<CustomerRole> _customerRoleRepository;

        private readonly CustomerSettings _customerSettings;

        private readonly IRepository<DeclarationOrder> _declarationOrderRepository;

        private readonly IRepository<GenericAttribute> _gaRepository;

        private readonly Lazy<IGdprTool> _gdprTool;

        private readonly IGenericAttributeService _genericAttributeService;

        private readonly HttpContextBase _httpContext;

        private readonly IRepository<RewardPointsHistory> _rewardPointsHistoryRepository;

        private readonly RewardPointsSettings _rewardPointsSettings;

        private readonly ICommonServices _services;

        private readonly IRepository<ShoppingCartItem> _shoppingCartItemRepository;

        private readonly IUserAgent _userAgent;

        #endregion Private Fields

        #region Public Constructors

        public CustomerService(
            IRepository<Customer> customerRepository,
            IRepository<DeclarationOrder> declarationOrderRepository,
            IRepository<CustomerRole> customerRoleRepository,
            IRepository<GenericAttribute> gaRepository,
            IRepository<RewardPointsHistory> rewardPointsHistoryRepository,
            IRepository<ShoppingCartItem> shoppingCartItemRepository,
            IGenericAttributeService genericAttributeService,
            RewardPointsSettings rewardPointsSettings,
            ICommonServices services,
            HttpContextBase httpContext,
            IUserAgent userAgent,
            CustomerSettings customerSettings,
            Lazy<IGdprTool> gdprTool)
        {
            _customerRepository = customerRepository;
            _customerRoleRepository = customerRoleRepository;
            _gaRepository = gaRepository;
            _rewardPointsHistoryRepository = rewardPointsHistoryRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _genericAttributeService = genericAttributeService;
            _rewardPointsSettings = rewardPointsSettings;
            _services = services;
            _httpContext = httpContext;
            _userAgent = userAgent;
            _customerSettings = customerSettings;
            _gdprTool = gdprTool;
            _declarationOrderRepository = declarationOrderRepository;
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
        }

        #endregion Public Constructors



        #region Public Properties

        public ILogger Logger { get; set; }

        public Localizer T { get; set; }

        #endregion Public Properties



        #region Public Methods

        public List<Customer> BuildAllTreeWithoutOrder()
        {
            List<Customer> tree = new List<Customer>();
            var query = from c in IncludeShoppingCart(_customerRepository.Table)
                        join d in _declarationOrderRepository.Table on c.Id equals d.CustomerId into custom
                        orderby c.Id
                        where c.IsCustomer == true
                        select c;
            var yestoday = DateTime.Now.Date.AddDays(-1);
            var today = DateTime.Now.Date;
            var dorder = from d in _declarationOrderRepository.Table
                         where d.PaidDateUtc.Value <= today && d.PaidDateUtc.Value >= yestoday //&& d.PaymentStatus == 30
                         select d;
            tree = query.ToList();
            var dList = dorder.ToList();
            foreach (var item in tree)
            {
                item.OrderList.AddRange(dList.Where(x => x.CustomerId == item.Id && x.PaymentStatus == Core.Domain.Payments.PaymentStatus.Paid));
            }

            //tree.AddRange(query);
            return tree;
        }

        public List<Customer> BuildCurrentTree()
        {
            List<Customer> tree = new List<Customer>();
            var query = from c in IncludeShoppingCart(_customerRepository.Table)
                        join d in _declarationOrderRepository.Table on c.Id equals d.CustomerId into custom
                        orderby c.Id
                        where c.IsCustomer == true
                        select c;
            var yestoday = DateTime.Now.Date.AddDays(-1);
            var today = DateTime.Now.Date;
            var tomorrow = DateTime.Now.Date.AddDays(1);
            var dorder = from d in _declarationOrderRepository.Table
                         where d.PaidDateUtc.Value <= tomorrow && d.PaidDateUtc.Value >= today //
                         select d;
            tree = query.ToList();
            var dList = dorder.ToList();
            foreach (var item in tree)
            {
                item.OrderList.Clear();
                item.OrderList.AddRange(dList.Where(x => x.CustomerId == item.Id &&  x.PaymentStatus == Core.Domain.Payments.PaymentStatus.Paid));
            }

            //tree.AddRange(query);
            return tree;
        }

        public List<Customer> BuildTree()
        {
            List<Customer> tree = new List<Customer>();
            var query = from c in IncludeShoppingCart(_customerRepository.Table)
                        join d in _declarationOrderRepository.Table on c.Id equals d.CustomerId into custom
                        orderby c.Id
                        where c.IsCustomer == true
                        select c;
            var yestoday = DateTime.Now.Date.AddDays(-1);
            var today = DateTime.Now.Date;
            var dorder = from d in _declarationOrderRepository.Table
                         where d.PaidDateUtc.Value <= today && d.PaidDateUtc.Value >= yestoday //&& d.PaymentStatus == Core.Domain.Payments.PaymentStatus.Paid
                         select d;
            tree = query.ToList();
            var dList = dorder.ToList();
            foreach (var item in tree)
            {
                item.OrderList.Clear();
                item.OrderList.AddRange(dList.Where(x => x.CustomerId == item.Id&& x.PaymentStatus== Core.Domain.Payments.PaymentStatus.Paid));
            }

            //tree.AddRange(query);
            return tree;
        }

        public virtual void DeleteCustomer(Customer customer)
        {
            Guard.NotNull(customer, nameof(customer));

            if (customer.IsSystemAccount)
                throw new SmartException(string.Format("System customer account ({0}) cannot not be deleted", customer.SystemName));

            // Soft delete
            customer.Deleted = true;

            // Anonymize IP addresses
            var language = customer.GetLanguage();

            _gdprTool.Value.AnonymizeData(customer, x => x.LastIpAddress, IdentifierDataType.IpAddress, language);

            foreach (var post in customer.ForumPosts)
            {
                _gdprTool.Value.AnonymizeData(post, x => x.IPAddress, IdentifierDataType.IpAddress, language);
            }

            // Customer Content
            foreach (var item in customer.CustomerContent)
            {
                _gdprTool.Value.AnonymizeData(item, x => x.IpAddress, IdentifierDataType.IpAddress, language);
            }

            UpdateCustomer(customer);
        }

        public virtual void DeleteCustomerRole(CustomerRole role)
        {
            Guard.NotNull(role, nameof(role));

            if (role.IsSystemRole)
                throw new SmartException("System role could not be deleted");

            _customerRoleRepository.Delete(role);
        }

        public virtual async Task<int> DeleteGuestCustomersAsync(
            DateTime? registrationFrom,
            DateTime? registrationTo,
            bool onlyWithoutShoppingCart,
            int maxItemsToDelete = 5000)
        {
            var ctx = _customerRepository.Context;

            using (var scope = new DbContextScope(ctx: ctx, autoDetectChanges: false, proxyCreation: true, validateOnSave: false, forceNoTracking: true, autoCommit: false))
            {
                var guestRole = GetCustomerRoleBySystemName(SystemCustomerRoleNames.Guests);
                if (guestRole == null)
                    throw new SmartException("'Guests' role could not be loaded");

                var query = _customerRepository.Table;

                if (registrationFrom.HasValue)
                    query = query.Where(c => registrationFrom.Value <= c.CreatedOnUtc);
                if (registrationTo.HasValue)
                    query = query.Where(c => registrationTo.Value >= c.CreatedOnUtc);

                query = query.Where(c => c.CustomerRoles.Select(cr => cr.Id).Contains(guestRole.Id));

                if (onlyWithoutShoppingCart)
                    query = query.Where(c => !c.ShoppingCartItems.Any());

                // no orders
                query = JoinWith<Order>(query, x => x.CustomerId);

                // no customer content
                query = JoinWith<CustomerContent>(query, x => x.CustomerId);

                // no private messages (guests can only receive but not send messages)
                query = JoinWith<PrivateMessage>(query, x => x.ToCustomerId);

                // no forum posts
                query = JoinWith<ForumPost>(query, x => x.CustomerId);

                // no forum topics
                query = JoinWith<ForumTopic>(query, x => x.CustomerId);

                // don't delete system accounts
                query = query.Where(c => !c.IsSystemAccount);

                // only distinct items
                query = from c in query
                        group c by c.Id
                            into cGroup
                        orderby cGroup.Key
                        select cGroup.FirstOrDefault();
                query = query.OrderBy(c => c.Id);

                var customers = await query.Take(() => maxItemsToDelete).ToListAsync();

                int numberOfDeletedCustomers = 0;
                foreach (var c in customers)
                {
                    try
                    {
                        // delete attributes (using GenericAttributeService would incorporate caching... which is bad in long running processes)
                        var gaQuery = from ga in _gaRepository.Table
                                      where ga.EntityId == c.Id && ga.KeyGroup == "Customer"
                                      select ga;
                        var attributes = await gaQuery.ToListAsync();

                        _gaRepository.DeleteRange(attributes);

                        // delete customer
                        _customerRepository.Delete(c);
                        numberOfDeletedCustomers++;

                        if (numberOfDeletedCustomers % 1000 == 0)
                        {
                            // save changes all 1000th item
                            try
                            {
                                await ctx.SaveChangesAsync();
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }

                // save the rest
                await ctx.SaveChangesAsync();

                return numberOfDeletedCustomers;
            }
        }

        public virtual Customer FindGuestCustomerByClientIdent(string clientIdent = null, int maxAgeSeconds = 60)
        {
            if (_httpContext.IsFakeContext() || _userAgent.IsBot || _userAgent.IsPdfConverter)
            {
                return null;
            }

            using (_services.Chronometer.Step("FindGuestCustomerByClientIdent"))
            {
                clientIdent = clientIdent.NullEmpty() ?? _services.WebHelper.GetClientIdent();
                if (clientIdent.IsEmpty())
                {
                    return null;
                }

                var dateFrom = DateTime.Now.AddSeconds(maxAgeSeconds * -1);

                IQueryable<Customer> query;
                if (DataSettings.Current.IsSqlServer)
                {
                    query = from a in _gaRepository.TableUntracked
                            join c in _customerRepository.Table on a.EntityId equals c.Id into Customers
                            from c in Customers.DefaultIfEmpty()
                            where c.LastActivityDateUtc >= dateFrom
                                && c.Username == null
                                && c.Email == null
                                && a.KeyGroup == "Customer"
                                && a.Key == "ClientIdent"
                                && a.Value == clientIdent
                            select c;
                }
                else
                {
                    query = from a in _gaRepository.TableUntracked
                            join c in _customerRepository.Table on a.EntityId equals c.Id into Customers
                            from c in Customers.DefaultIfEmpty()
                            where c.LastActivityDateUtc >= dateFrom
                                && c.Username == null
                                && c.Email == null
                                && a.KeyGroup == "Customer"
                                && a.Key == "ClientIdent"
                                && a.Value.Contains(clientIdent) // SQLCE doesn't like ntext in WHERE clauses
                            select c;
                }

                return query.FirstOrDefault();
            }
        }

        public virtual IList<CustomerRole> GetAllCustomerRoles(bool showHidden = false)
        {
            var query = from cr in _customerRoleRepository.Table
                        orderby cr.Name
                        where (showHidden || cr.Active)
                        select cr;

            var customerRoles = query.ToListCached();
            return customerRoles;
        }

        public virtual IPagedList<Customer> GetAllCustomersByPasswordFormat(PasswordFormat passwordFormat)
        {
            var q = new CustomerSearchQuery
            {
                PasswordFormat = passwordFormat,
                PageIndex = 0,
                PageSize = 500
            };

            var customers = SearchCustomers(q);
            return customers;
        }
        
        //    public virtual Customer GetCustomerByMobile(string mobile)
        //{
        //    if (string.IsNullOrWhiteSpace(mobile))
        //        return null;

        //    var query = from c in IncludeShoppingCart(_customerRepository.Table)
        //                orderby c.Id
        //                where c.Mobile == mobile
        //                select c;

        //    var customer = query.FirstOrDefault();
        //    return customer;
        //}
        public virtual Customer GetCustomerByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var query = from c in IncludeShoppingCart(_customerRepository.Table)
                        orderby c.Id
                        where c.Email == email
                        select c;

            var customer = query.FirstOrDefault();
            return customer;
        }

        public virtual Customer GetCustomerByGuid(Guid customerGuid)
        {
            if (customerGuid == Guid.Empty)
                return null;

            var query = from c in IncludeShoppingCart(_customerRepository.Table)
                        where c.CustomerGuid == customerGuid
                        orderby c.Id
                        select c;

            var customer = query.FirstOrDefault();
            return customer;
        }

        public virtual Customer GetCustomerById(int customerId)
        {
            if (customerId == 0)
                return null;

            // var customer = _customerRepository.GetById(customerId);
            var customer = IncludeShoppingCart(_customerRepository.Table).SingleOrDefault(x => x.Id == customerId);

            return customer;
        }

        public virtual Customer GetCustomerByMobile(string Mobile)
        {
            if (string.IsNullOrWhiteSpace(Mobile))
                return null;

            var query = from c in IncludeShoppingCart(_customerRepository.Table)
                        orderby c.Id
                        where c.Mobile == Mobile
                        select c;

            var customer = query.FirstOrDefault();
            return customer;
        }

        public virtual Customer GetCustomerBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from c in _customerRepository.Table
                        orderby c.Id
                        where c.SystemName == systemName
                        select c;

            var customer = query.FirstOrDefault();
            return customer;
        }

        public virtual Customer GetCustomerByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var query = from c in IncludeShoppingCart(_customerRepository.Table)
                        orderby c.Id
                        where c.Username == username
                        select c;

            var customer = query.FirstOrDefault();
            return customer;
        }

        public virtual CustomerRole GetCustomerRoleById(int roleId)
        {
            if (roleId == 0)
                return null;

            return _customerRoleRepository.GetById(roleId);
        }

        public virtual CustomerRole GetCustomerRoleBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from cr in _customerRoleRepository.Table
                        orderby cr.Id
                        where cr.SystemName == systemName
                        select cr;

            var customerRole = query.FirstOrDefaultCached();
            return customerRole;
        }

        public virtual IList<Customer> GetCustomersByIds(int[] customerIds)
        {
            if (customerIds == null || customerIds.Length == 0)
                return new List<Customer>();

            var query = from c in _customerRepository.Table
                        where customerIds.Contains(c.Id)
                        select c;

            var customers = query.ToList();

            // sort by passed identifier sequence
            return customers.OrderBySequence(customerIds).ToList();
        }

        public virtual IPagedList<Customer> GetOnlineCustomers(DateTime lastActivityFromUtc, int[] customerRoleIds, int pageIndex, int pageSize)
        {
            var q = new CustomerSearchQuery
            {
                LastActivityFromUtc = lastActivityFromUtc,
                CustomerRoleIds = customerRoleIds,
                IsSystemAccount = false,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var customers = SearchCustomers(q);

            customers.AlterQuery(x => x.OrderByDescending(c => c.LastActivityDateUtc));

            return customers;
        }

        public virtual Multimap<int, RewardPointsHistory> GetRewardPointsHistoriesByCustomerIds(int[] customerIds)
        {
            Guard.NotNull(customerIds, nameof(customerIds));

            var query =
                from x in _rewardPointsHistoryRepository.TableUntracked
                where customerIds.Contains(x.CustomerId)
                select x;

            var map = query
                .OrderBy(x => x.CustomerId)
                .ThenByDescending(x => x.CreatedOnUtc)
                .ThenByDescending(x => x.Id)
                .ToList()
                .ToMultimap(x => x.CustomerId, x => x);

            return map;
        }

        public virtual IList<Customer> GetSystemAccountCustomers()
        {
            return _customerRepository.Table.Where(x => x.IsSystemAccount).ToList();
        }

        public virtual void InsertCustomer(Customer customer)
        {
            Guard.NotNull(customer, nameof(customer));

            _customerRepository.Insert(customer);
        }

        public virtual void InsertCustomerRole(CustomerRole role)
        {
            Guard.NotNull(role, nameof(role));

            _customerRoleRepository.Insert(role);
        }

        public virtual Customer InsertGuestCustomer(Guid? customerGuid = null)
        {
            var customer = new Customer
            {
                CustomerGuid = customerGuid ?? Guid.NewGuid(),
                Active = true,
                CreatedOnUtc = DateTime.Now,
                LastActivityDateUtc = DateTime.Now,
            };

            // Add to 'Guests' role
            var guestRole = GetCustomerRoleBySystemName(SystemCustomerRoleNames.Guests);
            if (guestRole == null)
                throw new SmartException("'Guests' role could not be loaded");

            using (new DbContextScope(autoCommit: true))
            {
                // Ensure that entities are saved to db in any case
                customer.CustomerRoles.Add(guestRole);
                _customerRepository.Insert(customer);

                var clientIdent = _services.WebHelper.GetClientIdent();
                if (clientIdent.HasValue())
                {
                    _genericAttributeService.SaveAttribute(customer, "ClientIdent", clientIdent);
                }
            }

            //Logger.DebugFormat("Guest account created for anonymous visitor. Id: {0}, ClientIdent: {1}", customer.CustomerGuid, clientIdent ?? "n/a");

            return customer;
        }

        public virtual void ResetCheckoutData(
            Customer customer,
            int storeId,
            bool clearCouponCodes = false,
            bool clearCheckoutAttributes = false,
            bool clearRewardPoints = false,
            bool clearShippingMethod = true,
            bool clearPaymentMethod = true,
            bool clearCreditBalance = false)
        {
            Guard.NotNull(customer, nameof(customer));

            if (clearCouponCodes)
            {
                _genericAttributeService.SaveAttribute<ShippingOption>(customer, SystemCustomerAttributeNames.DiscountCouponCode, null);
                _genericAttributeService.SaveAttribute<ShippingOption>(customer, SystemCustomerAttributeNames.GiftCardCouponCodes, null);
            }

            if (clearCheckoutAttributes)
            {
                _genericAttributeService.SaveAttribute<ShippingOption>(customer, SystemCustomerAttributeNames.CheckoutAttributes, null);
            }

            if (clearRewardPoints)
            {
                _genericAttributeService.SaveAttribute<bool>(customer, SystemCustomerAttributeNames.UseRewardPointsDuringCheckout, false, storeId);
            }

            if (clearCreditBalance)
            {
                _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.UseCreditBalanceDuringCheckout, decimal.Zero, storeId);
            }

            if (clearShippingMethod)
            {
                _genericAttributeService.SaveAttribute<ShippingOption>(customer, SystemCustomerAttributeNames.SelectedShippingOption, null, storeId);
                _genericAttributeService.SaveAttribute<ShippingOption>(customer, SystemCustomerAttributeNames.OfferedShippingOptions, null, storeId);
            }

            if (clearPaymentMethod)
            {
                _genericAttributeService.SaveAttribute<string>(customer, SystemCustomerAttributeNames.SelectedPaymentMethod, null, storeId);
            }

            UpdateCustomer(customer);
        }

        public virtual void RewardPointsForProductReview(Customer customer, Product product, bool add)
        {
            if (_rewardPointsSettings.Enabled && _rewardPointsSettings.PointsForProductReview > 0)
            {
                string message = T(add ? "RewardPoints.Message.EarnedForProductReview" : "RewardPoints.Message.ReducedForProductReview", product.GetLocalized(x => x.Name));

                customer.AddRewardPointsHistoryEntry(_rewardPointsSettings.PointsForProductReview * (add ? 1 : -1), message);

                UpdateCustomer(customer);
            }
        }

        public virtual IPagedList<Customer> SearchCustomers(CustomerSearchQuery q)
        {
            Guard.NotNull(q, nameof(q));

            var isOrdered = false;
            IQueryable<Customer> query = null;

            if (q.OnlyWithCart)
            {
                var cartItemQuery = _shoppingCartItemRepository.TableUntracked.Expand(x => x.Customer);

                if (q.CartType.HasValue)
                {
                    cartItemQuery = cartItemQuery.Where(x => x.ShoppingCartTypeId == (int)q.CartType.Value);
                }

                var groupQuery =
                    from sci in cartItemQuery
                    group sci by sci.CustomerId into grp
                    select grp
                        .OrderByDescending(x => x.CreatedOnUtc)
                        .Select(x => new
                        {
                            x.Customer,
                            x.CreatedOnUtc
                        })
                        .FirstOrDefault();

                // We have to sort again because of paging.
                query = groupQuery
                    .OrderByDescending(x => x.CreatedOnUtc)
                    .Select(x => x.Customer);

                isOrdered = true;
            }
            else
            {
                query = _customerRepository.Table;
            }
            query = query.Where(x => !string.IsNullOrEmpty(x.Username));
            if (q.Email.HasValue())
            {
                query = query.Where(c => c.Email.Contains(q.Email));
            }

            if (q.Username.HasValue())
            {
                query = query.Where(c => c.Username.Contains(q.Username));
            }

            if (q.CustomerNumber.HasValue())
            {
                query = query.Where(c => c.CustomerNumber.Contains(q.CustomerNumber));
            }

            if (q.AffiliateId.GetValueOrDefault() > 0)
            {
                query = query.Where(c => c.AffiliateId == q.AffiliateId.Value);
            }

            if (q.SearchTerm.HasValue())
            {
                if (_customerSettings.CompanyEnabled)
                {
                    query = query.Where(c => c.FullName.Contains(q.SearchTerm) || c.Company.Contains(q.SearchTerm));
                }
                else
                {
                    query = query.Where(c => c.FullName.Contains(q.SearchTerm));
                }
            }

            if (q.DayOfBirth.GetValueOrDefault() > 0)
            {
                query = query.Where(c => c.BirthDate.Value.Day == q.DayOfBirth.Value);
            }

            if (q.MonthOfBirth.GetValueOrDefault() > 0)
            {
                query = query.Where(c => c.BirthDate.Value.Month == q.MonthOfBirth.Value);
            }

            if (q.RegistrationFromUtc.HasValue)
            {
                query = query.Where(c => q.RegistrationFromUtc.Value <= c.CreatedOnUtc);
            }

            if (q.RegistrationToUtc.HasValue)
            {
                query = query.Where(c => q.RegistrationToUtc.Value >= c.CreatedOnUtc);
            }

            if (q.LastActivityFromUtc.HasValue)
            {
                query = query.Where(c => q.LastActivityFromUtc.Value <= c.LastActivityDateUtc);
            }

            if (q.CustomerRoleIds != null && q.CustomerRoleIds.Length > 0)
            {
                query = query.Where(c => c.CustomerRoles.Select(cr => cr.Id).Intersect(q.CustomerRoleIds).Count() > 0);
            }

            if (q.Deleted.HasValue)
            {
                query = query.Where(c => c.Deleted == q.Deleted.Value);
            }

            if (q.Active.HasValue)
            {
                query = query.Where(c => c.Active == q.Active.Value);
            }

            if (q.IsSystemAccount.HasValue)
            {
                query = q.IsSystemAccount.Value == true
                    ? query.Where(c => !string.IsNullOrEmpty(c.SystemName))
                    : query.Where(c => string.IsNullOrEmpty(c.SystemName));
            }

            if (q.PasswordFormat.HasValue)
            {
                int passwordFormatId = (int)q.PasswordFormat.Value;
                query = query.Where(c => c.PasswordFormatId == passwordFormatId);
            }

            // Search by phone
            if (q.Phone.HasValue())
            {
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { Customer = x, Attribute = y })
                    .Where(z => z.Attribute.KeyGroup == "Customer" &&
                        z.Attribute.Key == SystemCustomerAttributeNames.Phone &&
                        z.Attribute.Value.Contains(q.Phone))
                    .Select(z => z.Customer);
            }

            // Search by zip
            if (q.ZipPostalCode.HasValue())
            {
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { Customer = x, Attribute = y })
                    .Where(z => z.Attribute.KeyGroup == "Customer" &&
                        z.Attribute.Key == SystemCustomerAttributeNames.ZipPostalCode &&
                        z.Attribute.Value.Contains(q.ZipPostalCode))
                    .Select(z => z.Customer);
            }

            if (!isOrdered)
            {
                query = query.OrderByDescending(c => c.CreatedOnUtc);
            }

            var customers = new PagedList<Customer>(query, q.PageIndex, q.PageSize);
            return customers;
        }

        public virtual void UpdateCustomer(Customer customer)
        {
            Guard.NotNull(customer, nameof(customer));

            _customerRepository.Update(customer);
        }

        public virtual void UpdateCustomerRole(CustomerRole role)
        {
            Guard.NotNull(role, nameof(role));

            _customerRoleRepository.Update(role);
        }

        #endregion Public Methods



        #region Private Methods

        private IQueryable<Customer> IncludeShoppingCart(IQueryable<Customer> query)
        {
            return query
                .Expand(x => x.ShoppingCartItems.Select(y => y.BundleItem))
                .Expand(x => x.ShoppingCartItems.Select(y => y.Product.AppliedDiscounts.Select(z => z.DiscountRequirements)));
        }

        private IQueryable<Customer> JoinWith<T>(IQueryable<Customer> query, Expression<Func<T, int>> customerIdSelector) where T : BaseEntity
        {
            var inner = _customerRepository.Context.Set<T>().AsNoTracking();

            /*
			 * Lamda join created with LinqPad. ORIGINAL:
				 from c in customers
					join inner in ctx.Set<TInner>().AsNoTracking() on c.Id equals inner.CustomerId into c_inner
					from inner in c_inner.DefaultIfEmpty()
					where !c_inner.Any()
					select c;
			*/
            query = query
                .GroupJoin(
                    inner,
                    c => c.Id,
                    customerIdSelector,
                    (c, i) => new { Customer = c, Inner = i })
                .SelectMany(
                    x => x.Inner.DefaultIfEmpty(),
                    (a, b) => new { a, b }
                )
                .Where(x => !(x.a.Inner.Any()))
                .Select(x => x.a.Customer);

            return query;
        }

        #endregion Private Methods
    }
}