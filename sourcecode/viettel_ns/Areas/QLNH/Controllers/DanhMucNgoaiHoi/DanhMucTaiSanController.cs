﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Viettel.Domain.DomainModel;
using Viettel.Models.QLNH;
using Viettel.Services;
using VIETTEL.Controllers;

namespace VIETTEL.Areas.QLNH.Controllers.DanhMucNgoaiHoi
{
    public class DanhMucTaiSanController : AppController
    {
        private readonly IQLNHService _qlnhService = QLNHService.Default;

        public ActionResult Index()
        {
            DanhMucNgoaiHoi_DanhMucTaiSanModelPaging vm = new DanhMucNgoaiHoi_DanhMucTaiSanModelPaging();
            vm._paging.CurrentPage = 1;
            vm.Items = _qlnhService.getListDanhMucTaiSanModels(ref vm._paging, null);

            return View(vm);
        }

        [HttpPost]
        public ActionResult DanhMucTaiSanSearch(PagingInfo _paging, string sMaLoaiTaiSan, string sTenLoaiTaiSan, string sMoTa)
        {
            DanhMucNgoaiHoi_DanhMucTaiSanModelPaging vm = new DanhMucNgoaiHoi_DanhMucTaiSanModelPaging();
            vm._paging = _paging;
            vm.Items = _qlnhService.getListDanhMucTaiSanModels(ref vm._paging, sMaLoaiTaiSan, sTenLoaiTaiSan, sMoTa);

            return PartialView("_list", vm);
        }

        [HttpPost]
        public ActionResult GetModalDetail(Guid? id)
        {
            DanhmucNgoaiHoi_TaiSanModel data = new DanhmucNgoaiHoi_TaiSanModel();
            if (id.HasValue)
            {
                data = _qlnhService.GetTaiDanhMucSanById(id.Value);
            }
            return PartialView("_modalDetail", data);
        }

        [HttpPost]
        public ActionResult GetModal(Guid? id)
        {

            DanhmucNgoaiHoi_TaiSanModel data = new  DanhmucNgoaiHoi_TaiSanModel();
            if (id.HasValue)
            {
                data = _qlnhService.GetTaiDanhMucSanById(id.Value);
            }
            return PartialView("_modalUpdate", data);
        }


        [HttpPost]
        public JsonResult TaiSanDelete(Guid id)
        {
            if (!_qlnhService.DeleteDanhMucTaiSan(id))
            {
                return Json(new { bIsComplete = false, sMessError = "Không xóa được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TaiSanSave(NH_DM_LoaiTaiSan data)
        {
            if (data == null)
            {
                return Json(new { bIsComplete = false, sMessError = "Không cập nhật được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            var returnData = _qlnhService.SaveTaiSan(data);
            if (!returnData.IsReturn)
            {
                return Json(new { bIsComplete = false, sMessError = returnData.errorMess }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }
    }
}