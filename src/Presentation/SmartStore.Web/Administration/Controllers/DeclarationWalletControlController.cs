using SmartStore.Web.Framework.Controllers;
using System.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    /// <summary>
    /// 报单系统管理/钱包算法控制
    /// </summary>
    public class DeclarationWalletControlController : AdminControllerBase
    {
        #region Public Methods

        // GET: DeclarationWalletControl/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeclarationWalletControl/Create
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

        // GET: DeclarationWalletControl/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DeclarationWalletControl/Delete/5
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

        // GET: DeclarationWalletControl/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DeclarationWalletControl/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DeclarationWalletControl/Edit/5
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

        // GET: DeclarationWalletControl
        public ActionResult Index()
        {
            return View();
        }

        #endregion Public Methods
    }
}