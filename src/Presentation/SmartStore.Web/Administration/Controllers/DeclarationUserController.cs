using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    /// <summary>
    /// 报单系统管理/系统会员管理
    /// </summary>
    public class DeclarationUserController : Controller
    {
        // GET: DeclarationUser
        public ActionResult Index()
        {
            return View();
        }

        // GET: DeclarationUser/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DeclarationUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeclarationUser/Create
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

        // GET: DeclarationUser/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DeclarationUser/Edit/5
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

        // GET: DeclarationUser/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DeclarationUser/Delete/5
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