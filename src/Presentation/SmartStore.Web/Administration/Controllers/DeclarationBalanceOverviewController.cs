using SmartStore.Web.Framework.Controllers;
using System.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    /// <summary>
    /// 数据报表/财务收支统计
    /// </summary>
    public class DeclarationBalanceOverviewController : AdminControllerBase
    {
        #region Public Methods

        // GET: DeclarationBalanceOverview/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeclarationBalanceOverview/Create
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

        // GET: DeclarationBalanceOverview/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DeclarationBalanceOverview/Delete/5
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

        // GET: DeclarationBalanceOverview/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DeclarationBalanceOverview/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DeclarationBalanceOverview/Edit/5
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

        // GET: DeclarationBalanceOverview
        public ActionResult Index()
        {
            return View();
        }

        #endregion Public Methods
    }
}