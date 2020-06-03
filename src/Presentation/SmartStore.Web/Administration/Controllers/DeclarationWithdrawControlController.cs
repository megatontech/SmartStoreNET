using SmartStore.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    public class DeclarationWithdrawControlController : AdminControllerBase
    {
        // GET: DeclarationWithdrawControl
        public ActionResult Index()
        {
            return View();
        }

        // GET: DeclarationWithdrawControl/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DeclarationWithdrawControl/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeclarationWithdrawControl/Create
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

        // GET: DeclarationWithdrawControl/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DeclarationWithdrawControl/Edit/5
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

        // GET: DeclarationWithdrawControl/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DeclarationWithdrawControl/Delete/5
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
