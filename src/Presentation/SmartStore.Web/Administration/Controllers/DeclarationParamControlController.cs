using SmartStore.Core.Domain.Wallet;
using SmartStore.Services.Wallet;
using SmartStore.Web.Framework.Controllers;
using System.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    /// <summary>
    /// 报单系统管理/佣金算法控制
    /// </summary>
    public class DeclarationParamControlController : AdminControllerBase
    {
        private readonly IDeclarationCalcRuleService _calcruleService;
        private readonly DeclarationCalcRule _calcrule;

        public DeclarationParamControlController(IDeclarationCalcRuleService calcruleService)
        {
            _calcruleService = calcruleService;
            _calcrule = _calcruleService.GetDeclarationCalcRule();
        }
        #region Public Methods

        // GET: DeclarationParamControl/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeclarationParamControl/Create
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

        // GET: DeclarationParamControl/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DeclarationParamControl/Delete/5
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

        // GET: DeclarationParamControl/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DeclarationParamControl/Edit/5
        public ActionResult Edit()
        {
            return View(_calcrule);
        }

        // POST: DeclarationParamControl/Edit/5
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

        // GET: DeclarationParamControl
        public ActionResult Index()
        {
            return View("edit",_calcrule);
        }

        #endregion Public Methods
    }
}