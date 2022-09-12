using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VIETTEL.Controllers;
using Viettel.Domain.DomainModel;
using Viettel.Models.QLNH;
using Viettel.Models.QLVonDauTu;
using Viettel.Services;

namespace VIETTEL.Areas.QLNH.Controllers.DuAnHopDong
{
    public class DuAnHopDongController : AppController
    {
        private readonly IQLNHService _qlnhService = QLNHService.Default;

        // GET: QLVonDauTu/QLDMTyGia
        public ActionResult Index()
        {
            DanhmucNgoaiHoi_TiGiaModelPaging vm = new DanhmucNgoaiHoi_TiGiaModelPaging();
            vm._paging.CurrentPage = 1;
            vm.Items = _qlnhService.getListTiGiaModels(ref vm._paging, null);

            return View(vm);
        }

        [HttpPost]
        public ActionResult DanhMucTiGiaSearch(PagingInfo _paging, DateTime? dNgayTao, string sMaTiGia, string sTenTiGia, string sMoTaTiGia, string sMaTienTeGoc)
        {
            DanhmucNgoaiHoi_TiGiaModelPaging vm = new DanhmucNgoaiHoi_TiGiaModelPaging();
            vm._paging = _paging;
            vm.Items = _qlnhService.getListTiGiaModels(ref vm._paging, dNgayTao, sMaTiGia, sTenTiGia, sMoTaTiGia, sMaTienTeGoc);

            return PartialView("_list", vm);
        }

        [HttpPost]
        public ActionResult GetModalDetail(Guid? id)
        {
            DanhmucNgoaiHoi_TiGiaModel data = new DanhmucNgoaiHoi_TiGiaModel();
            if (id.HasValue)
            {
                data = _qlnhService.GetTyGiaById(id.Value);
            }
            return PartialView("_modalDetail", data);
        }

        [HttpPost]
        public ActionResult GetModal(Guid? id)
        {
            DanhmucNgoaiHoi_TiGiaModel data = new DanhmucNgoaiHoi_TiGiaModel();
            if (id.HasValue)
            {
                data = _qlnhService.GetTyGiaById(id.Value);
            }
            return PartialView("_modalUpdate", data);
        }

        [HttpPost]
        public JsonResult TyGiaDelete(Guid id)
        {
            if (!_qlnhService.DeleteTyGia(id))
            {
                return Json(new { bIsComplete = false, sMessError = "Không xóa được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TyGiaSave(NH_DM_TiGia data)
        {
            if (!_qlnhService.SaveTyGia(data, Username))
            {
                return Json(new { bIsComplete = false, sMessError = "Không cập nhật được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }
    }
}