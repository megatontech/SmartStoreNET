using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Wallet;
using SmartStore.Services.Calc;
using SmartStore.Services.Customers;
using SmartStore.Services.Orders;
using SmartStore.Services.Wallet;
using SmartStore.Web.Framework.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    public class DeclarationControlcenterController : AdminControllerBase
    {
        #region Private Fields

        private readonly ICalcRewardService _CalcRewardService;
        private readonly ICustomerService _CustomerService;
        private readonly IDeclarationOrderService _DeclarationOrder;
        private readonly IWithdrawalTotalService _total;
        private readonly ICustomerPointsTotalService _points;
        #endregion Private Fields

        #region Public Constructors

        public DeclarationControlcenterController(ICalcRewardService calcRewardService, IWithdrawalTotalService total, ICustomerPointsTotalService points, ICustomerService customerService, IDeclarationOrderService declarationOrder)
        {
            _CalcRewardService = calcRewardService;
            _CustomerService = customerService;
            _DeclarationOrder = declarationOrder; _points = points;
            _total = total;
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
            //Customer customer = _CustomerService.GetCustomerById(33);
            //DeclarationOrder order = _DeclarationOrder.GetOrderById(13);
            DailyTotalContribution model = _CalcRewardService.UpdateRealtimeData();
            var allcustomer = _CustomerService.BuildNoLimitAllTreeWithoutOrder();
            //钱包展示总额，可提现，冻结，以及最近入账
            model.Team = new System.Collections.Generic.List<Customer>();
            model.Team.AddRange(allcustomer.ToList());
            var allTotal = _total.GetAllSum();
            var allPoints = _points.GetAllSum();
            model.TodayPoint = allPoints;
            model.TodayWalletTotal = allTotal;
            ////3、商城查询总拨比。（总支出÷总业绩 = 总拨比）
            //var totalout = 0M;
            //var totalearn = 0M;
            //model.PastTotalOut = (decimal)(totalout/ totalearn);
            //_CalcRewardService.CalcRewardTwoAsync(0M);
            //_CalcRewardService.CalcRewardTwo(1400M);
            //_CalcRewardService.CalcRewardThree(500M);
            //_CalcRewardService.CalcRewardFour(100M);
            return View(model);
        }

        #endregion Public Methods
    }
}