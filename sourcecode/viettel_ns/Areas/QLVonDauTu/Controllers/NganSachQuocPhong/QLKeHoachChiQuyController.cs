using FlexCel.Core;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Viettel.Domain.DomainModel;
using Viettel.Models.QLVonDauTu;
using Viettel.Services;
using VIETTEL.Areas.QLVonDauTu.Model.NganSachQuocPhong;
using VIETTEL.Common;
using VIETTEL.Controllers;
using VIETTEL.Models;

namespace VIETTEL.Areas.QLVonDauTu.Controllers.NganSachQuocPhong
{
    public class QLKeHoachChiQuyController : AppController
    {
        private QLKeHoachVonNamModel _modelKHV = new QLKeHoachVonNamModel();
        private GiaiNganThanhToanModel _model = new GiaiNganThanhToanModel();
        private readonly IQLVonDauTuService _vdtService = QLVonDauTuService.Default;
        private readonly INganSachService _iNganSachService = NganSachService.Default;
        private const string sFilePath = "/Report_ExcelFrom/VonDauTu/rpt_BaoCaoNhucauKeHoachChiQuy.xlsx";

        // GET: QLVonDauTu/QLKeHoachChiQuy
        public ActionResult Index()
        {
            string sMaNguoiDung = Username;

            List<SelectListItem> lstDataDonVi = new List<SelectListItem>();
            DataTable dt = NganSach_HamChungModels.DSDonViCuaNguoiDung(sMaNguoiDung);
            foreach (DataRow dr in dt.Rows)
            {
                lstDataDonVi.Add(new SelectListItem() { Text = Convert.ToString(dr["TenHT"]), Value = Convert.ToString(dr["iID_MaDonVi"]) });
            }

            lstDataDonVi.Insert(0, new SelectListItem { Text = "", Value = "" });
            ViewBag.drpDonViQuanLy = lstDataDonVi;

            List<NS_NguonNganSach> lstNguonVon = _vdtService.LayNguonVon().ToList();
            lstNguonVon.Insert(0, new NS_NguonNganSach { iID_MaNguonNganSach = 0, sTen = "" });
            ViewBag.drpNguonNganSach = lstNguonVon.ToSelectList("iID_MaNguonNganSach", "sTen");

            List<SelectListItem> lstQuy = new List<SelectListItem> {
                new SelectListItem{Text = "", Value = ""},
                new SelectListItem{Text = Constants.LoaiQuy.TypeName.QUY_1, Value=((int)Constants.LoaiQuy.Type.QUY_1).ToString()},
                new SelectListItem{Text = Constants.LoaiQuy.TypeName.QUY_2, Value=((int)Constants.LoaiQuy.Type.QUY_2).ToString()},
                new SelectListItem{Text = Constants.LoaiQuy.TypeName.QUY_3, Value=((int)Constants.LoaiQuy.Type.QUY_3).ToString()},
                new SelectListItem{Text = Constants.LoaiQuy.TypeName.QUY_4, Value=((int)Constants.LoaiQuy.Type.QUY_4).ToString()},
            };
            ViewBag.drpQuy = lstQuy.ToSelectList("Value", "Text");

            KHChiQuyPagingModel data = new KHChiQuyPagingModel();
            data._paging.CurrentPage = 1;
            data.lstData = _vdtService.GetAllKHNhuCauChiPaging(ref data._paging);

            return View(data);
        }

        public ActionResult Insert()
        {
            string sMaNguoiDung = Username;
            ViewBag.drpDonViQuanLy = _modelKHV.GetDataDropdownDonViQuanLy(sMaNguoiDung);

            List<NS_NguonNganSach> lstNguonVon = _vdtService.LayNguonVon().ToList();
            ViewBag.drpNguonNganSach = lstNguonVon.ToSelectList("iID_MaNguonNganSach", "sTen");

            List<SelectListItem> lstQuy = new List<SelectListItem> {
                new SelectListItem{Text = Constants.LoaiQuy.TypeName.QUY_1, Value=((int)Constants.LoaiQuy.Type.QUY_1).ToString()},
                new SelectListItem{Text = Constants.LoaiQuy.TypeName.QUY_2, Value=((int)Constants.LoaiQuy.Type.QUY_2).ToString()},
                new SelectListItem{Text = Constants.LoaiQuy.TypeName.QUY_3, Value=((int)Constants.LoaiQuy.Type.QUY_3).ToString()},
                new SelectListItem{Text = Constants.LoaiQuy.TypeName.QUY_4, Value=((int)Constants.LoaiQuy.Type.QUY_4).ToString()},
            };
            ViewBag.drpQuy = lstQuy.ToSelectList("Value", "Text");

            return View();
        }

        public ActionResult Update(Guid id)
        {
            var data = _vdtService.GetNhuCauChiByID(id);

            if (data == null)
                return RedirectToAction("Index");

            string sMaNguoiDung = Username;
            ViewBag.drpDonViQuanLy = _modelKHV.GetDataDropdownDonViQuanLy(sMaNguoiDung);

            List<NS_NguonNganSach> lstNguonVon = _vdtService.LayNguonVon().ToList();
            ViewBag.drpNguonNganSach = lstNguonVon.ToSelectList("iID_MaNguonNganSach", "sTen");

            List<SelectListItem> lstQuy = new List<SelectListItem> {
                new SelectListItem{Text = Constants.LoaiQuy.TypeName.QUY_1, Value=((int)Constants.LoaiQuy.Type.QUY_1).ToString()},
                new SelectListItem{Text = Constants.LoaiQuy.TypeName.QUY_2, Value=((int)Constants.LoaiQuy.Type.QUY_2).ToString()},
                new SelectListItem{Text = Constants.LoaiQuy.TypeName.QUY_3, Value=((int)Constants.LoaiQuy.Type.QUY_3).ToString()},
                new SelectListItem{Text = Constants.LoaiQuy.TypeName.QUY_4, Value=((int)Constants.LoaiQuy.Type.QUY_4).ToString()},
            };
            ViewBag.drpQuy = lstQuy.ToSelectList("Value", "Text");

            return View(data);
        }

        public ActionResult Detail(Guid id)
        {
            var data = _vdtService.GetNhuCauChiByID(id);

            if (data == null)
                return RedirectToAction("Index");

            return View(data);
        }

        #region Event
        #region PartialView
        [HttpPost]
        public ActionResult KHNhuCauChiQuyView(PagingInfo _paging, string sSoDeNghi, int? iNamKeHoach, DateTime? dNgayDeNghiFrom, DateTime? dNgayDeNghiTo, string iIDMaDonViQuanLy, int? iIDNguonVon, int? iQuy)
        {
            KHChiQuyPagingModel data = new KHChiQuyPagingModel();
            data._paging = _paging;
            data.lstData = _vdtService.GetAllKHNhuCauChiPaging(ref data._paging, sSoDeNghi, iNamKeHoach, dNgayDeNghiFrom, dNgayDeNghiTo, iIDMaDonViQuanLy, iIDNguonVon, iQuy);

            string sMaNguoiDung = Username;
            List<SelectListItem> lstDataDonVi = new List<SelectListItem>();
            DataTable dt = NganSach_HamChungModels.DSDonViCuaNguoiDung(sMaNguoiDung);
            foreach (DataRow dr in dt.Rows)
            {
                lstDataDonVi.Add(new SelectListItem() { Text = Convert.ToString(dr["TenHT"]), Value = Convert.ToString(dr["iID_MaDonVi"]) });
            }

            lstDataDonVi.Insert(0, new SelectListItem { Text = "", Value = "" });
            ViewBag.drpDonViQuanLy = lstDataDonVi;

            List<NS_NguonNganSach> lstNguonVon = _vdtService.LayNguonVon().ToList();
            lstNguonVon.Insert(0, new NS_NguonNganSach { iID_MaNguonNganSach = 0, sTen = "" });
            ViewBag.drpNguonNganSach = lstNguonVon.ToSelectList("iID_MaNguonNganSach", "sTen");

            List<SelectListItem> lstQuy = new List<SelectListItem> {
                new SelectListItem{Text = "", Value = ""},
                new SelectListItem{Text = Constants.LoaiQuy.TypeName.QUY_1, Value=((int)Constants.LoaiQuy.Type.QUY_1).ToString()},
                new SelectListItem{Text = Constants.LoaiQuy.TypeName.QUY_2, Value=((int)Constants.LoaiQuy.Type.QUY_2).ToString()},
                new SelectListItem{Text = Constants.LoaiQuy.TypeName.QUY_3, Value=((int)Constants.LoaiQuy.Type.QUY_3).ToString()},
                new SelectListItem{Text = Constants.LoaiQuy.TypeName.QUY_4, Value=((int)Constants.LoaiQuy.Type.QUY_4).ToString()},
            };
            ViewBag.drpQuy = lstQuy.ToSelectList("Value", "Text");

            return PartialView("_list", data);
        }
        #endregion

        /// <summary>
        /// get kinh phi ctc cap
        /// </summary>
        /// <param name="dNgayDeNghi"></param>
        /// <param name="iID_HopDongID"></param>
        /// <returns></returns>
        public JsonResult GetKinhPhiCucTaiChinhCap(int iNamKeHoach, string iIdDonVi, int iIdNguonVon, int iQuy)
        {
            var sMaDonViQuanLy = _iNganSachService.GetDonViById(PhienLamViec.iNamLamViec, iIdDonVi).iID_MaDonVi;
            KinhPhiCucTaiChinhCap kinhPhiCTCCap = _vdtService.GetKinhPhiCucTaiChinhCap(iNamKeHoach, sMaDonViQuanLy, iIdNguonVon, iQuy);
            return Json(new { data = kinhPhiCTCCap }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// get nhu cau chi chi tiet
        /// </summary>
        /// <returns></returns>
        public JsonResult GetNhuCauChiChiTiet(int iNamKeHoach, string iIdDonVi, int iIdNguonVon, int iQuy, Guid? iIDNhuCauChiID)
        {
            var sMaDonViQuanLy = _iNganSachService.GetDonViById(PhienLamViec.iNamLamViec, iIdDonVi).iID_MaDonVi;
            List<NcNhuCauChi_ChiTiet> data = _vdtService.GetNhuCauChiChiTiet(iNamKeHoach, sMaDonViQuanLy, iIdNguonVon, iQuy);
            if (iIDNhuCauChiID != null && iIDNhuCauChiID != Guid.Empty)
            {
                List<VDT_NC_NhuCauChi_ChiTiet> detailData = _vdtService.GetNhuCauChiChiTietByNhuCauChiID(iIDNhuCauChiID.Value);
                if (detailData != null)
                {
                    foreach (VDT_NC_NhuCauChi_ChiTiet item in detailData)
                    {
                        NcNhuCauChi_ChiTiet currentData = data.FirstOrDefault(n => n.iID_DuAnId == item.iID_DuAnId && n.sLoaiThanhToan == item.sLoaiThanhToan);
                        if (currentData != null)
                        {
                            currentData.fGiaTriDeNghi = item.fGiaTriDeNghi ?? 0;
                            currentData.sGhiChu = item.sGhiChu;
                        }
                    }
                }
            }
            return Json(this.RenderRazorViewToString("_list_nhucauchi_chitiet", data), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Process
        [HttpPost]
        public JsonResult LuuKeHoachChiQuy(VDT_NC_NhuCauChi data, List<VDT_NC_NhuCauChi_ChiTiet> lstChiTiet)
        {
            if (_vdtService.LuuNhuCauChi(data, lstChiTiet, Username))
            {
                return Json(new { bIsComplete = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool XoaKeHoachChiQuy(Guid id)
        {
            if (!_vdtService.XoaNhuCauChi(id)) return false;
            return true;
        }

        #endregion

        #region Export

        public ActionResult XuatFile(Guid id)
        {
            ExcelFile excel = TaoFile(id);
            if (excel != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    excel.Save(stream);
                    return File(stream.ToArray(), "application/vnd.ms-excel", $"Bao cao nhu cau Ke hoach chi quy.xlsx");
                }
            }
            return RedirectToAction("Index");
        }

        public ExcelFile TaoFile(Guid iID_NhuCauChiID)
        {
            var objNhuCauChi = _vdtService.GetNhuCauChiByID(iID_NhuCauChiID);
            if (objNhuCauChi == null)
                return null;

            List<NcNhuCauChi_ChiTiet> lstNhuCauChiChiTiet = _vdtService.GetNhuCauChiChiTiet(objNhuCauChi.iNamKeHoach ?? 0, objNhuCauChi.iID_MaDonViQuanLy, objNhuCauChi.iID_NguonVonID ?? 0, objNhuCauChi.iQuy ?? 0);

            List<VDT_NC_NhuCauChi_ChiTiet> detailData = _vdtService.GetNhuCauChiChiTietByNhuCauChiID(iID_NhuCauChiID);
            if (detailData != null)
            {
                foreach (VDT_NC_NhuCauChi_ChiTiet item in detailData)
                {
                    NcNhuCauChi_ChiTiet currentData = lstNhuCauChiChiTiet.FirstOrDefault(n => n.iID_DuAnId == item.iID_DuAnId && n.sLoaiThanhToan == item.sLoaiThanhToan);
                    if (currentData != null)
                    {
                        currentData.fGiaTriDeNghi = item.fGiaTriDeNghi ?? 0;
                        currentData.sGhiChu = item.sGhiChu;
                    }
                }
            }


            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(sFilePath));

            FlexCelReport fr = new FlexCelReport();
            fr.SetValue("sTenDonVi", objNhuCauChi.sDonViQuanLy);
            fr.SetValue("iQuy", objNhuCauChi.iQuy);
            fr.SetValue("iNam", objNhuCauChi.iNamKeHoach);
            fr.SetValue("sTenNguonVon", objNhuCauChi.sNguonVon);
            fr.SetValue("sNgayThangNam", string.Format("Ngày {0} tháng {1} năm {2}", DateTime.Now.ToString("dd"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("yyyy")));
            fr.AddTable<NcNhuCauChi_ChiTiet>("Items", lstNhuCauChiChiTiet);
            fr.Run(Result);
            return Result;
        }
        #endregion
    }
}