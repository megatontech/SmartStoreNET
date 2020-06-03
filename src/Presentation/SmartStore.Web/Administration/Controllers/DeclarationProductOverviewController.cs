using SmartStore.Web.Framework.Controllers;
using System.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    /// <summary>
    /// 数据报表/报单产品统计
    /// </summary>
    public class DeclarationProductOverviewController : AdminControllerBase
    {
        #region Public Methods

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