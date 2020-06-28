using SmartStore.Admin.Models.Stores;
using SmartStore.Services.Catalog;
using SmartStore.Services.Orders;
using SmartStore.Web.Framework.Controllers;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    /// <summary>
    /// 数据报表/报单产品统计
    /// </summary>
    public class DeclarationProductOverviewController : AdminControllerBase
    {
        public IDeclarationProductService _productService;
        public IDeclarationOrderService declarationOrderService;

        public DeclarationProductOverviewController(IDeclarationProductService productService, IDeclarationOrderService declarationOrderService)
        {
            _productService = productService;
            this.declarationOrderService = declarationOrderService;
        }

        #region Public Methods
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command)
        {
            var gridModel = new GridModel<DeclarationProductDispModel>();

            {
                var Models = _productService.GetAllProductsDisplayedOnHomePage()
                    .Select(x =>
                    {
                        var model = new DeclarationProductDispModel();
                        model.Name = x.Name;
                        model.Price = x.Price;
                        model.Level = x.IsEsd ? "白金产品" : "金卡产品";
                        var temp = declarationOrderService.GetOrderTotalByProduct(x.Id);
                        model.Count = temp.Item1;
                        model.Amount = temp.Item2;
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

        // GET: DeclarationProductOverview/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeclarationProductOverview/Create
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

        // GET: DeclarationProductOverview/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DeclarationProductOverview/Delete/5
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

        // GET: DeclarationProductOverview/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DeclarationProductOverview/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DeclarationProductOverview/Edit/5
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

        // GET: DeclarationProductOverview
        public ActionResult Index()
        {
            return View();
        }

        #endregion Public Methods
    }
}