using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Viettel.Domain.DomainModel;
using Viettel.Models.QLVonDauTu;
using Viettel.Services;
using VIETTEL.Areas.QLNguonNganSach.Models;
using VIETTEL.Areas.QLVonDauTu.Model.NganSachQuocPhong;
using VIETTEL.Areas.QLVonDauTu.Model.QuyetToan;
using VIETTEL.Common;
using VIETTEL.Controllers;

namespace VIETTEL.Areas.QLVonDauTu.Controllers.QuyetToan
{
    public class BcQuyetToanNienDoController : AppController
    {
        private readonly IQLVonDauTuService _vdtService = QLVonDauTuService.Default;
        private BcQuyetToanNienDoModel _model = new BcQuyetToanNienDoModel();
        private static string[] _lstDonViExclude = new string[] { "0", "1" };

        // GET: QLVonDauTu/BcQuyetToanNienDo
        #region View
        public ActionResult Index()
        {
            ViewBag.drpDonViQuanLy = CommonFunction.GetDataDropDownDonViQuanLy(_lstDonViExclude);
            ViewBag.drpNguonVon = CommonFunction.GetDataDropDownNguonNganSach();

            VdtQtBcQuyetToanNienDoPagingModel data = new VdtQtBcQuyetToanNienDoPagingModel();
            data._paging.CurrentPage = 1;
            data.lstData = _model.GetPagingIndex(ref data._paging);
            return View(data);
        }

        public ActionResult Update(Guid? id)
        {
            VDT_QT_BCQuyetToanNienDo data = new VDT_QT_BCQuyetToanNienDo();
            var drpDonViQuanLy = CommonFunction.GetDataDropDownDonViQuanLy(_lstDonViExclude);
            var drpNguonVon = CommonFunction.GetDataDropDownNguonNganSach();
            var drpCoQuanThanhToan = _model.GetCoQuanThanhToan();
            var drpLoaiThanhToan = _model.GetLoaiThanhToan();
            if (id.HasValue)
            {
                data = _vdtService.GetBaoCaoQuyetToanById(id.Value);
                if(data != null)
                {
                    foreach(var item in drpDonViQuanLy)
                    {
                        if(item.Value.IndexOf(data.iID_MaDonViQuanLy) != -1)
                        {
                            item.Selected = true;
                            break;
                        }
                    }

                    foreach(var item in drpNguonVon)
                    {
                        if(item.Value == data.iID_NguonVonID.ToString())
                        {
                            item.Selected = true;
                            break;
                        }
                    }

                    foreach (var item in drpLoaiThanhToan)
                    {
                        if (item.Value == data.iLoaiThanhToan.ToString())
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }
            ViewBag.drpDonViQuanLy = drpDonViQuanLy;
            ViewBag.drpNguonVon = drpNguonVon;
            ViewBag.drpCoQuanThanhToan = drpCoQuanThanhToan;
            ViewBag.drpLoaiThanhToan = drpLoaiThanhToan;
            return View(data);
        }
        #endregion

        #region PartialView
        [HttpPost]
        public ActionResult QuyetToanNienDoView(PagingInfo _paging, string iIdMaDonViQuanLy, int? iIdNguonVon, DateTime? dNgayDeNghiFrom, DateTime? dNgayDeNghiTo, int? iNamKeHoach)
        {
            ViewBag.drpDonViQuanLy = CommonFunction.GetDataDropDownDonViQuanLy(_lstDonViExclude);
            ViewBag.drpNguonVon = CommonFunction.GetDataDropDownNguonNganSach();

            VdtQtBcQuyetToanNienDoPagingModel data = new VdtQtBcQuyetToanNienDoPagingModel();
            data._paging.CurrentPage = 1;
            data.lstData = _model.GetPagingIndex(ref data._paging, iIdMaDonViQuanLy, iIdNguonVon, dNgayDeNghiFrom, dNgayDeNghiTo, iNamKeHoach);
            return PartialView("_list", data);
        }

        [HttpPost]
        public ActionResult QuyetToanNienDoKHVN(Guid? iIDBcQuyetToan, string sMaDonVi, int? iNamKeHoach, int? iIDNguonVonID, int? iCoQuanTaiChinh)
        {
            if(string.IsNullOrEmpty(sMaDonVi) || !iNamKeHoach.HasValue || !iIDNguonVonID.HasValue || !iCoQuanTaiChinh.HasValue)
            {
                return PartialView("_listKeHoachVonNam", new List<BcquyetToanNienDoVonNamChiTietViewModel>());
            }
            var data = _model.GetQuyetToanVonNam(iIDBcQuyetToan, sMaDonVi, iNamKeHoach.Value, iIDNguonVonID.Value, iCoQuanTaiChinh.Value);
            return PartialView("_listKeHoachVonNam", data);
        }

        [HttpPost]
        public ActionResult QuyetToanNienDoKHVN_PhanTich(Guid? iIDBcQuyetToan, string sMaDonVi, int? iNamKeHoach, int? iIDNguonVonID, int? iCoQuanTaiChinh)
        {
            if (string.IsNullOrEmpty(sMaDonVi) || !iNamKeHoach.HasValue || !iIDNguonVonID.HasValue || !iCoQuanTaiChinh.HasValue)
            {
                return PartialView("_listKeHoachVonNam_PhanTich", new List<BcquyetToanNienDoVonNamPhanTichChiTietViewModel>());
            }
            var data = _model.GetQuyetToanVonNam_PhanTich(iIDBcQuyetToan, sMaDonVi, iNamKeHoach.Value, iIDNguonVonID.Value, iCoQuanTaiChinh.Value);
            return PartialView("_listKeHoachVonNam_PhanTich", data);
        }

        [HttpPost]
        public ActionResult QuyetToanNienDoKHU(Guid? iIDBcQuyetToan, string sMaDonVi, int? iNamKeHoach, int? iIDNguonVonID, int? iCoQuanTaiChinh)
        {
            if (string.IsNullOrEmpty(sMaDonVi) || !iNamKeHoach.HasValue || !iIDNguonVonID.HasValue || !iCoQuanTaiChinh.HasValue)
            {
                return PartialView("_listKeHoachVonUng", new List<BcquyetToanNienDoVonUngChiTietViewModel>());
            }
            var data = _model.GetQuyetToanVonUng(iIDBcQuyetToan, sMaDonVi, iNamKeHoach.Value, iIDNguonVonID.Value, iCoQuanTaiChinh.Value);
            return PartialView("_listKeHoachVonUng", data);
        }
        #endregion

        #region Event
        [HttpGet]
        public JsonResult DeleteBCQuyetToanNienDo(Guid iId)
        {
            return Json(new { bIsComplete = _model.DeleteBCQuyetToanNienDo(iId) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateBcQuyetToanNienDo(VDT_QT_BCQuyetToanNienDo data)
        {
            if (data.iID_BCQuyetToanNienDoID == Guid.Empty)
                return Json(new { bIsComplete = _model.InsertBcQuyetToanNienDo(data, Username) }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { bIsComplete = _model.UpdateBcQuyetToanNienDo(data, Username) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Helper

        #endregion
    }
}