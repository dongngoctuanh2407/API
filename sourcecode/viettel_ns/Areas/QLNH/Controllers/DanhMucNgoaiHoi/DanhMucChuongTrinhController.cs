using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Viettel.Domain.DomainModel;
using Viettel.Models.KeHoachChiTietBQP;
using Viettel.Models.QLNH;
using Viettel.Models.Shared;
using Viettel.Services;
using VIETTEL.Common;
using VIETTEL.Controllers;

namespace VIETTEL.Areas.QLNH.Controllers.DanhMucNgoaiHoi
{
    public class DanhMucChuongTrinhController : AppController
    {
        private readonly IQLNHService _qlnhService = QLNHService.Default;

        // GET: QLNH/DanhMucChuongTrinh
        public ActionResult Index()
        {
            var result = new NH_KHChiTietBQPViewModel();
            result = _qlnhService.getListDanhMucChuongTrinh(result._paging, null, null, null);

            var lstDonViQuanLy = _qlnhService.GetDonviListByYear(PhienLamViec.NamLamViec).ToList();
            lstDonViQuanLy.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sMoTa = "-- Chọn đơn vị --" });
            ViewBag.LookupDonVi = lstDonViQuanLy.ToSelectList("iID_Ma", "sMoTa");

            var lstPhongBan = _qlnhService.getLookupPhongBan().ToList();
            lstPhongBan.Insert(0, new LookupDto<Guid, string> { Id = Guid.Empty, DisplayName = "-- Chọn B quản lý --" });
            ViewBag.LookupPhongBan = lstPhongBan.ToSelectList("Id", "DisplayName");

            return View(result);
        }

        // Tìm kiếm
        public ActionResult TimKiem(DanhMucChuongTrinhFilter input, PagingInfo paging)
        {
            if (paging == null)
            {
                paging = new PagingInfo
                {
                    CurrentPage = 1,
                    ItemsPerPage = Constants.ITEMS_PER_PAGE
                };
            }
            var result = _qlnhService.getListDanhMucChuongTrinh(paging, input.sTenNhiemVuChi, input.iID_BQuanLyID, input.iID_DonViID);

            var lstDonViQuanLy = _qlnhService.GetDonviListByYear(PhienLamViec.NamLamViec).ToList();
            lstDonViQuanLy.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sMoTa = "-- Chọn đơn vị --" });
            ViewBag.LookupDonVi = lstDonViQuanLy.ToSelectList("iID_Ma", "sMoTa");

            var lstPhongBan = _qlnhService.getLookupPhongBan().ToList();
            lstPhongBan.Insert(0, new LookupDto<Guid, string> { Id = Guid.Empty, DisplayName = "-- Chọn B quản lý --" });
            ViewBag.LookupPhongBan = lstPhongBan.ToSelectList("Id", "DisplayName");

            return PartialView("_list", result);
        }

        public ActionResult GetListBQPNhiemVuChiById(Guid id, string sTenNhiemVuChi, Guid? iID_BQuanLyID, Guid? iID_DonViID)
        {
            var result = _qlnhService.GetListBQPNhiemVuChiById(id, sTenNhiemVuChi, iID_BQuanLyID, iID_DonViID);
            return Json(new { 
                datas = result
            });
        }
    }
}