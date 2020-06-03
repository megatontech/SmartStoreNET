using SmartStore.Services.Calc;
using SmartStore.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    public class DeclarationControlcenterController : AdminControllerBase
    {
        private readonly ICalcRewardService _CalcRewardService;

        public DeclarationControlcenterController(ICalcRewardService calcRewardService)
        {
            _CalcRewardService = calcRewardService;
        }

        // GET: DeclarationControlcenter
        public ActionResult Index()
        {
            _CalcRewardService.CalcRewardTwo(1400M);
            return View();
        }

        // GET: DeclarationControlcenter/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

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
    }
}
