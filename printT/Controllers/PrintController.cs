using printT.Clases;
using printT.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace printT.Controllers
{
    public class PrintController : Controller
    {
        // GET: Print
        public ActionResult Index()
        {
            return View();
        }

        // GET: Print/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Print/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Print/Create
        //[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public ActionResult printTicket(OPrintP OPrintParameters)
        {
            var oResponse = new OResponse();
            try
            {
                // TODO: Add insert logic here

                Print p = new Print();
                p.OPrintParameters = OPrintParameters;
                oResponse = p.PrintNow();

               
            }
            catch (Exception ex)
            {
                oResponse.bOK = false;
                oResponse.message = ex.Message;
            }

            return Json(oResponse, JsonRequestBehavior.AllowGet);

        }

        // GET: Print/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Print/Edit/5
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

        // GET: Print/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Print/Delete/5
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
