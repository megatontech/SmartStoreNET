using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Wallet;
using SmartStore.Services.Calc;
using SmartStore.Services.Customers;
using SmartStore.Services.Orders;
using SmartStore.Web.Framework.Controllers;
using System.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    public class DeclarationControlcenterController : AdminControllerBase
    {
        #region Private Fields

        private readonly ICalcRewardService _CalcRewardService;
        private readonly ICustomerService _CustomerService;
        private readonly IDeclarationOrderService _DeclarationOrder;
        
        #endregion Private Fields

        #region Public Constructors

        public DeclarationControlcenterController(ICalcRewardService calcRewardService, ICustomerService customerService, IDeclarationOrderService declarationOrder)
        {
            _CalcRewardService = calcRewardService;
            _CustomerService = customerService;
            _DeclarationOrder = declarationOrder;
        }

        #endregion Public Constructors



        #region Public Methods

        // GET: DeclarationControlcenter/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeclarationControlcenter/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DeclarationControlcenter/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DeclarationControlcenter/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DeclarationControlcenter/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DeclarationControlcenter/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DeclarationControlcenter/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DeclarationControlcenter
        public ActionResult Index()
        {
            Customer customer = _CustomerService.GetCustomerById(33);
            DeclarationOrder order = _DeclarationOrder.GetOrderById(13);
            DailyTotalContribution model = _CalcRewardService.UpdateRealtimeData();
            //_CalcRewardService.CalcRewardOne(customer, order);
            //_CalcRewardService.CalcRewardTwo(1400M);
            //_CalcRewardService.CalcRewardThree(500M);
            //_CalcRewardService.CalcRewardFour(100M);
            return View(model);
        }

        #endregion Public Methods
    }
}