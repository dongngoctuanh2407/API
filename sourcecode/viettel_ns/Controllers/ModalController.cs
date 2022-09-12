using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VIETTEL.Models;

namespace VIETTEL.Controllers
{
    public class ModalController : Controller
    {
        // GET: Modal
        [HttpPost]
        public ActionResult OpenModal(ModalModels model)
        {
            return PartialView("~/Views/Shared/Modal/_modalConfirm.cshtml", model);
        }
    }
}