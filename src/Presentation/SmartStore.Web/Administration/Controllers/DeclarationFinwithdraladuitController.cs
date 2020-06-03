using SmartStore.Web.Framework.Controllers;
using System.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    /// <summary>
    /// 报单审核/财务提现审核
    /// </summary>
    public class DeclarationFinwithdraladuitController : AdminControllerBase
    {
        #region Public Methods

        // GET: DeclarationFinwithdraladuit/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeclarationFinwithdraladuit/Create
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

        // GET: DeclarationFinwithdraladuit/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DeclarationFinwithdraladuit/Delete/5
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

        // GET: DeclarationFinwithdraladuit/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DeclarationFinwithdraladuit/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DeclarationFinwithdraladuit/Edit/5
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

        // GET: DeclarationFinwithdraladuit
        public ActionResult Index()
        {
            return View();
        }

        #endregion Public Methods
    }
}