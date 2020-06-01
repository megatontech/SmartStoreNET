using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    /// <summary>
    /// 报单审核/财务报单审核
    /// </summary>
    public class DeclarationFinaduitController : Controller
    {
        // GET: DeclarationFinaduit
        public ActionResult Index()
        {
            return View();
        }

        // GET: DeclarationFinaduit/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DeclarationFinaduit/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeclarationFinaduit/Create
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

        // GET: DeclarationFinaduit/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DeclarationFinaduit/Edit/5
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

        // GET: DeclarationFinaduit/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DeclarationFinaduit/Delete/5
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
    }
}