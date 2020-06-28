using SmartStore.Admin.Models.Stores;
using SmartStore.Services.Customers;
using SmartStore.Web.Framework.Controllers;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    /// <summary>
    /// 数据报表/会员业绩统计
    /// </summary>
    public class DeclarationCustomOverviewController : AdminControllerBase
    {
        private readonly ICustomerService _customerService;

        public DeclarationCustomOverviewController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        #region Public Methods
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command)
        {
            var gridModel = new GridModel<DeclarationCustomerModel>();

            {
                var Models = _customerService.BuildTree()
                    .Select(x =>
                    {
                        var model = new DeclarationCustomerModel();
                        model.Name = x.Username;
                        model.Mobile = x.Mobile;
                        model.Total = x.SelfTotal;
                        model.OrderCount = x.OrderList.Count();
                        return model;
                    })
                    .ToList();
                gridModel.Data = Models;
                gridModel.Total = Models.Count();
            }


            return new JsonResult
            {
                Data = gridModel
            };
        }

        // GET: DeclarationCustomOverview/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeclarationCustomOverview/Create
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

        // GET: DeclarationCustomOverview/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DeclarationCustomOverview/Delete/5
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

        // GET: DeclarationCustomOverview/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DeclarationCustomOverview/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DeclarationCustomOverview/Edit/5
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

        // GET: DeclarationCustomOverview
        public ActionResult Index()
        {
            return View();
        }

        #endregion Public Methods
    }
}