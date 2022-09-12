using FlexCel.Core;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Viettel.Domain.DomainModel;
using Viettel.Models.QLVonDauTu;
using Viettel.Services;
using VIETTEL.Application.Flexcel.Functions;
using VIETTEL.Areas.QLVonDauTu.Model.NganSachQuocPhong;
using VIETTEL.Common;
using VIETTEL.Controllers;
using VIETTEL.Flexcel;
using static Viettel.Extensions.Constants;

namespace VIETTEL.Areas.QLVonDauTu.Controllers.NganSachQuocPhong
{
    public class GiaiNganThanhToanController : FlexcelReportController
    {
        private QLKeHoachVonNamModel _modelKHV = new QLKeHoachVonNamModel();
        private GiaiNganThanhToanModel _model = new GiaiNganThanhToanModel();
        private readonly IQLVonDauTuService _vdtService = QLVonDauTuService.Default;
        private const string sFilePath_PhanGhiCQTC_TamUng = "/Report_ExcelFrom/VonDauTu/rpt_PhanGhiCQTC_TamUng.xlsx";
        private const string sFilePath_PhanGhiCQTC_ThanhToan = "/Report_ExcelFrom/VonDauTu/rpt_PhanGhiCQTC_ThanhToan.xlsx";

        private const string sFilePath_GiayDeNghiCoQuanThanhToan_TamUng = "/Report_ExcelFrom/VonDauTu/rpt_GiayDeNghiCoQuanThanhToan_TamUng.xlsx";
        private const string sFilePath_GiayDeNghiCoQuanThanhToan_ThanhToan = "/Report_ExcelFrom/VonDauTu/rpt_GiayDeNghiCoQuanThanhToan_ThanhToan.xlsx";
        private const int PHAN_GHI_CQTC = 2;
        private const int GIAY_DE_NGHI_CO_QUAN_THANH_TOAN = 1;
        private const string sControlName = "GiaiNganThanhToan";

        // GET: QLVonDauTu/GiaiNganThanhToan
        public ActionResult Index()
        {
            string sMaNguoiDung = Username;
            var dataDropDown = _modelKHV.GetDataDropdownDonViQuanLy(sMaNguoiDung);
            dataDropDown.Insert(0, new SelectListItem { Text = "", Value = "" });
            ViewBag.drpDonViQuanLy = dataDropDown;
            GiaiNganThanhToanPagingModel data = new GiaiNganThanhToanPagingModel();
            data._paging.CurrentPage = 1;
            data.lstData = _vdtService.GetAllGiaiNganThanhToanPaging(ref data._paging);

            foreach (var item in data.lstData)
            {
                if (item.iCoQuanThanhToan == (int)Constants.CoQuanThanhToan.Type.KHO_BAC)
                    item.sCoQuanThanhToan = Constants.CoQuanThanhToan.TypeName.KHO_BAC;
                else if (item.iCoQuanThanhToan == (int)Constants.CoQuanThanhToan.Type.CQTC)
                    item.sCoQuanThanhToan = Constants.CoQuanThanhToan.TypeName.CQTC;
            }
            return View(data);
        }

        public ActionResult Insert()
        {
            string sMaNguoiDung = Username;
            ViewBag.drpDonViQuanLy = _modelKHV.GetDataDropdownDonViQuanLy(sMaNguoiDung);
            ViewBag.drpLoaiNganSach = _modelKHV.GetDataDropdownLoaiNganSach(DateTime.Now.Year);

            List<SelectListItem> lstLoaiThanhToan = new List<SelectListItem> {
                new SelectListItem{Text = Constants.LoaiThanhToan.TypeName.THANH_TOAN, Value=((int)Constants.LoaiThanhToan.Type.THANH_TOAN).ToString()},
                new SelectListItem{Text = Constants.LoaiThanhToan.TypeName.TAM_UNG, Value=((int)Constants.LoaiThanhToan.Type.TAM_UNG).ToString()}
            };
            ViewBag.drpLoaiThanhToan = lstLoaiThanhToan.ToSelectList("Value", "Text");

            List<SelectListItem> lstCqThanhToan = new List<SelectListItem> {
                new SelectListItem{Text = Constants.CoQuanThanhToan.TypeName.KHO_BAC, Value=((int)Constants.CoQuanThanhToan.Type.KHO_BAC).ToString()},
                new SelectListItem{Text = Constants.CoQuanThanhToan.TypeName.CQTC, Value=((int)Constants.CoQuanThanhToan.Type.CQTC).ToString()}
            };
            ViewBag.drpCoQuanThanhToan = lstCqThanhToan.ToSelectList("Value", "Text");

            List<DM_ChuDauTu> lstChuDauTu = _vdtService.LayDanhMucChuDauTu(PhienLamViec.NamLamViec).ToList();
            lstChuDauTu.Insert(0, new DM_ChuDauTu { ID = Guid.Empty, sTenCDT = Constants.CHON });
            ViewBag.ListChuDauTu = lstChuDauTu.Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = string.IsNullOrEmpty(c.sId_CDT) ? c.sTenCDT : (c.sId_CDT + " - " + c.sTenCDT)
            });

            return View();
        }

        public ActionResult Update(Guid id)
        {
            var data = _vdtService.GetDeNghiThanhToanDetailByID(id);

            if (data == null)
                return RedirectToAction("Index");

            if (data.iLoaiThanhToan == (int)Constants.LoaiThanhToan.Type.THANH_TOAN)
                data.sLoaiThanhToan = Constants.LoaiThanhToan.TypeName.THANH_TOAN;
            else if (data.iLoaiThanhToan == (int)Constants.LoaiThanhToan.Type.TAM_UNG)
                data.sLoaiThanhToan = Constants.LoaiThanhToan.TypeName.TAM_UNG;

            if (data.iCoQuanThanhToan == (int)Constants.CoQuanThanhToan.Type.KHO_BAC)
                data.sCoQuanThanhToan = Constants.CoQuanThanhToan.TypeName.KHO_BAC;
            else if (data.iCoQuanThanhToan == (int)Constants.CoQuanThanhToan.Type.CQTC)
                data.sCoQuanThanhToan = Constants.CoQuanThanhToan.TypeName.CQTC;

            double luyKeTTTN = 0;
            double luyKeTTNN = 0;
            double luyKeTUTN = 0;
            double luyKeTUNN = 0;
            double luyKeTUUngTruocTN = 0;
            double luyKeTUUngTruocNN = 0;

            Guid? iIdChungTu = (data.bThanhToanTheoHopDong.HasValue && data.bThanhToanTheoHopDong.Value) ? data.iID_HopDongId : data.iID_ChiPhiID;
            _vdtService.LoadGiaTriThanhToan(data.iCoQuanThanhToan.Value, data.dNgayDeNghi.Value, data.bThanhToanTheoHopDong.Value, iIdChungTu.ToString(), data.iID_NguonVonID ?? 0, data.iNamKeHoach ?? 0,
                    ref luyKeTTTN, ref luyKeTTNN, ref luyKeTUTN, ref luyKeTUTN, ref luyKeTUUngTruocTN, ref luyKeTUUngTruocNN);

            data.fLuyKeTTNN = luyKeTTNN;
            data.fLuyKeTTTN = luyKeTTTN;
            data.fLuyKeTUNN = luyKeTUNN;
            data.fLuyKeTUTN = luyKeTUTN;
            data.fLuyKeTUUngTruocNN = luyKeTUUngTruocNN;
            data.fLuyKeTUUngTruocTN = luyKeTUUngTruocTN;

            // get list KHV
            List<KeHoachVonModel> list = _vdtService.GetKeHoachVonCapPhatThanhToan(data.iID_DuAnId.ToString(), data.iID_NguonVonID.Value, data.dNgayDeNghi.Value, data.iNamKeHoach.Value, data.iCoQuanThanhToan.Value, data.iID_DeNghiThanhToanID);
            List<VDT_TT_DeNghiThanhToan_KHV> listKeHoachVon = _vdtService.FindDeNghiThanhToanKHVByDeNghiThanhToanID(data.iID_DeNghiThanhToanID);
            foreach (KeHoachVonModel item in list)
            {
                if (listKeHoachVon.Where(n => n.iID_KeHoachVonID == item.Id && n.iLoai == item.iPhanLoai).Count() > 0)
                {
                    item.IsChecked = true;
                }
            }
            data.listKeHoachVon = list;

            // get list chi phi
            List<VdtTtDeNghiThanhToanChiPhiQuery> listChiPhi = _vdtService.GetChiPhiInDenghiThanhToanScreen(data.dNgayDeNghi.Value, data.iID_DuAnId.Value);

            if (data.iID_ChiPhiID != null)
            {
                foreach (VdtTtDeNghiThanhToanChiPhiQuery item in listChiPhi)
                {
                    if (item.IIdChiPhiId == data.iID_ChiPhiID)
                    {
                        item.IsChecked = true;
                    }
                }
            }
            data.listChiPhi = listChiPhi;

            string sMaNguoiDung = Username;
            ViewBag.drpDonViQuanLy = _modelKHV.GetDataDropdownDonViQuanLy(sMaNguoiDung);
            ViewBag.drpLoaiNganSach = _modelKHV.GetDataDropdownLoaiNganSach(DateTime.Now.Year);

            List<SelectListItem> lstLoaiThanhToan = new List<SelectListItem> {
                new SelectListItem{Text = Constants.LoaiThanhToan.TypeName.THANH_TOAN, Value=((int)Constants.LoaiThanhToan.Type.THANH_TOAN).ToString()},
                new SelectListItem{Text = Constants.LoaiThanhToan.TypeName.TAM_UNG, Value=((int)Constants.LoaiThanhToan.Type.TAM_UNG).ToString()}
            };
            ViewBag.drpLoaiThanhToan = lstLoaiThanhToan.ToSelectList("Value", "Text");

            List<SelectListItem> lstCqThanhToan = new List<SelectListItem> {
                new SelectListItem{Text = Constants.CoQuanThanhToan.TypeName.KHO_BAC, Value=((int)Constants.CoQuanThanhToan.Type.KHO_BAC).ToString()},
                new SelectListItem{Text = Constants.CoQuanThanhToan.TypeName.CQTC, Value=((int)Constants.CoQuanThanhToan.Type.CQTC).ToString()}
            };
            ViewBag.drpCoQuanThanhToan = lstCqThanhToan.ToSelectList("Value", "Text");

            List<DM_ChuDauTu> lstChuDauTu = _vdtService.LayDanhMucChuDauTu(PhienLamViec.NamLamViec).ToList();
            lstChuDauTu.Insert(0, new DM_ChuDauTu { ID = Guid.Empty, sTenCDT = Constants.CHON });
            ViewBag.ListChuDauTu = lstChuDauTu.Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = string.IsNullOrEmpty(c.sId_CDT) ? c.sTenCDT : (c.sId_CDT + " - " + c.sTenCDT)
            });

            return View(data);
        }

        public ActionResult Detail(Guid id)
        {
            GiaiNganThanhToanViewModel data = _vdtService.GetDeNghiThanhToanDetailByID(id);

            if (data == null)
                return RedirectToAction("Index");

            if (data.iLoaiThanhToan == (int)Constants.LoaiThanhToan.Type.THANH_TOAN)
                data.sLoaiThanhToan = Constants.LoaiThanhToan.TypeName.THANH_TOAN;
            else if (data.iLoaiThanhToan == (int)Constants.LoaiThanhToan.Type.TAM_UNG)
                data.sLoaiThanhToan = Constants.LoaiThanhToan.TypeName.TAM_UNG;

            if (data.iCoQuanThanhToan == (int)Constants.CoQuanThanhToan.Type.KHO_BAC)
                data.sCoQuanThanhToan = Constants.CoQuanThanhToan.TypeName.KHO_BAC;
            else if (data.iCoQuanThanhToan == (int)Constants.CoQuanThanhToan.Type.CQTC)
                data.sCoQuanThanhToan = Constants.CoQuanThanhToan.TypeName.CQTC;

            double luyKeTTTN = 0;
            double luyKeTTNN = 0;
            double luyKeTUTN = 0;
            double luyKeTUNN = 0;
            double luyKeTUUngTruocTN = 0;
            double luyKeTUUngTruocNN = 0;

            Guid? iIdChungTu = (data.bThanhToanTheoHopDong.HasValue && data.bThanhToanTheoHopDong.Value) ? data.iID_HopDongId : data.iID_ChiPhiID;

            _vdtService.LoadGiaTriThanhToan(data.iCoQuanThanhToan.Value, data.dNgayDeNghi.Value, data.bThanhToanTheoHopDong.Value, iIdChungTu.ToString(), data.iID_NguonVonID ?? 0, data.iNamKeHoach ?? 0,
                ref luyKeTTTN, ref luyKeTTNN, ref luyKeTUTN, ref luyKeTUTN, ref luyKeTUUngTruocTN, ref luyKeTUUngTruocNN);

            data.fLuyKeTTNN = luyKeTTNN;
            data.fLuyKeTTTN = luyKeTTTN;
            data.fLuyKeTUNN = luyKeTUNN;
            data.fLuyKeTUTN = luyKeTUTN;
            data.fLuyKeTUUngTruocNN = luyKeTUUngTruocNN;
            data.fLuyKeTUUngTruocTN = luyKeTUUngTruocTN;

            // get list KHV
            List<KeHoachVonModel> listKHV = _vdtService.GetKeHoachVonCapPhatThanhToan(data.iID_DuAnId.ToString(), data.iID_NguonVonID.Value, data.dNgayDeNghi.Value, data.iNamKeHoach.Value, data.iCoQuanThanhToan.Value, data.iID_DeNghiThanhToanID);

            List<VDT_TT_DeNghiThanhToan_KHV> listKeHoachVon = _vdtService.FindDeNghiThanhToanKHVByDeNghiThanhToanID(data.iID_DeNghiThanhToanID);
            foreach (KeHoachVonModel item in listKHV)
            {
                if (listKeHoachVon.Where(n => n.iID_KeHoachVonID == item.Id && n.iLoai == item.iPhanLoai).Count() > 0)
                {
                    item.IsChecked = true;
                }
            }
            data.listKeHoachVon = listKHV;

            // get list chi phi
            List<VdtTtDeNghiThanhToanChiPhiQuery> listChiPhi = _vdtService.GetChiPhiInDenghiThanhToanScreen(data.dNgayDeNghi.Value, data.iID_DuAnId.Value);

            if (data.iID_ChiPhiID != null)
            {
                foreach (VdtTtDeNghiThanhToanChiPhiQuery item in listChiPhi)
                {
                    if (item.IIdChiPhiId == data.iID_ChiPhiID)
                    {
                        item.IsChecked = true;
                    }
                }
            }
            data.listChiPhi = listChiPhi;

            // get list phe duyet chi tiet
            data.listPheDuyetChiTiet = _vdtService.GetListPheDuyetChiTietDetail(id);

            List<VdtTtKeHoachVonQuery> _lstKeHoachVonThanhToan = _vdtService.LoadKeHoachVonThanhToan(data.iID_DuAnId.ToString(), data.iID_NguonVonID.Value,
                data.dNgayDeNghi.Value, data.iNamKeHoach.Value, data.iCoQuanThanhToan.Value, data.iID_DeNghiThanhToanID);

            foreach (var item in data.listPheDuyetChiTiet)
            {
                VdtTtKeHoachVonQuery objKHVTT = _lstKeHoachVonThanhToan.Where(x => x.IIdKeHoachVonId == item.iID_KeHoachVonID).FirstOrDefault();
                if(objKHVTT != null)
                {
                    item.sTenKHV = objKHVTT.SDisplayName;
                }
                if (item.iLoaiDeNghi == (int)Constants.LoaiThanhToan.Type.THANH_TOAN || item.iLoaiDeNghi == (int)Constants.LoaiThanhToan.Type.TAM_UNG)
                {
                    item.fDefaultValueTN = data.fGiaTriThanhToanTN;
                    item.fDefaultValueNN = data.fGiaTriThanhToanNN;
                }
                else
                {
                    item.fDefaultValueTN = data.fGiaTriThuHoiTN;
                    item.fDefaultValueNN = data.fGiaTriThuHoiNN;
                }
                item.fTongSo = item.fGiaTriNgoaiNuoc.Value + item.fGiaTriTrongNuoc.Value;
            }

            return View(data);
        }

        #region Event
        public JsonResult GetPheDuyetThanhToanChiTiet(Guid iID_DeNghiThanhToanID)
        {
            var data = _vdtService.GetDeNghiThanhToanDetailByID(iID_DeNghiThanhToanID);
            List<PheDuyetThanhToanChiTiet> listPheDuyetChiTiet = _vdtService.GetListPheDuyetChiTietDetail(iID_DeNghiThanhToanID);
            List<VdtTtKeHoachVonQuery> _lstKeHoachVonThanhToan = _vdtService.LoadKeHoachVonThanhToan(data.iID_DuAnId.ToString(), data.iID_NguonVonID.Value,
                data.dNgayDeNghi.Value, data.iNamKeHoach.Value, data.iCoQuanThanhToan.Value, data.iID_DeNghiThanhToanID);

            foreach (var item in listPheDuyetChiTiet)
            {
                VdtTtKeHoachVonQuery objKHVTT = _lstKeHoachVonThanhToan.Where(x => x.IIdKeHoachVonId == item.iID_KeHoachVonID).FirstOrDefault();
                if (objKHVTT != null)
                {
                    item.sTenKHV = objKHVTT.SDisplayName;
                }
                if (item.iLoaiDeNghi == (int)Constants.LoaiThanhToan.Type.THANH_TOAN || item.iLoaiDeNghi == (int)Constants.LoaiThanhToan.Type.TAM_UNG)
                {
                    item.fDefaultValueTN = data.fGiaTriThanhToanTN;
                    item.fDefaultValueNN = data.fGiaTriThanhToanNN;
                }
                else
                {
                    item.fDefaultValueTN = data.fGiaTriThuHoiTN;
                    item.fDefaultValueNN = data.fGiaTriThuHoiNN;
                }
                item.fTongSo = item.fGiaTriNgoaiNuoc.Value + item.fGiaTriTrongNuoc.Value;
                item.iLoaiNamKH = objKHVTT.ILoaiNamKhv;

            }
            return Json(new { lstPheDuyet = listPheDuyetChiTiet }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDataDropDownDuAn(Guid iID_DonViQuanLyID, int iID_NguonVonID, Guid iID_LoaiNguonVonID, DateTime dNgayQuyetDinh, int iNamKeHoach, Guid iID_NganhID)
        {
            var data = _model.GetDataDropdownDuAn(iID_DonViQuanLyID, iID_NguonVonID, iID_LoaiNguonVonID, dNgayQuyetDinh, iNamKeHoach, iID_NganhID);
            return Json(new { data = data, bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDataThongTinChiTietLoaiNganSachByNganh(Guid iId_Nganh, int iNamKeHoach)
        {
            var data = _modelKHV.GetDataThongTinChiTietLoaiNganSachByNganh(iId_Nganh, iNamKeHoach);
            return Json(new { data = data, bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDataDropdownHopDong(Guid iIdDuAn)
        {
            IEnumerable<VDT_DA_TT_HopDong> data = _vdtService.GetHopDongByThanhToanDuAnId(iIdDuAn);
            return Json(new { data = data, bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDetailHopDongDuAn(Guid iID_DuAnID, Guid iID_HopDongID, DateTime dNgayDeNghi, int iNamKeHoach, int iID_NguonVonID, Guid iID_LoaiNguonVonID, Guid iID_NganhID)
        {
            var data = _model.GetDetailHopDongDuAn(iID_DuAnID, iID_HopDongID, dNgayDeNghi, iNamKeHoach, iID_NguonVonID, iID_LoaiNguonVonID, iID_NganhID);
            return Json(new { data = data, bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetGiaiNganThanhToanChiTiet(Guid iId)
        {
            var data = _vdtService.GetDeNghiThanhToanChiTietByDeNghiThanhToanID(iId);
            return Json(new { data = data, bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }

        #region PartialView
        [HttpPost]
        public ActionResult GiaiNganThanhToanView(PagingInfo _paging, string sSoDeNghi, int? iNamKeHoach, DateTime? dNgayDeNghiFrom, DateTime? dNgayDeNghiTo, Guid? sDonViQuanLy)
        {
            GiaiNganThanhToanPagingModel data = new GiaiNganThanhToanPagingModel();
            data._paging = _paging;
            data.lstData = _vdtService.GetAllGiaiNganThanhToanPaging(ref data._paging, sSoDeNghi, iNamKeHoach, dNgayDeNghiFrom, dNgayDeNghiTo, sDonViQuanLy);

            foreach (var item in data.lstData)
            {
                if (item.iCoQuanThanhToan == (int)Constants.CoQuanThanhToan.Type.KHO_BAC)
                    item.sCoQuanThanhToan = Constants.CoQuanThanhToan.TypeName.KHO_BAC;
                else if (item.iCoQuanThanhToan == (int)Constants.CoQuanThanhToan.Type.CQTC)
                    item.sCoQuanThanhToan = Constants.CoQuanThanhToan.TypeName.CQTC;
            }

            string sMaNguoiDung = Username;
            var dataDropDown = _modelKHV.GetDataDropdownDonViQuanLy(sMaNguoiDung);
            dataDropDown.Insert(0, new SelectListItem { Text = "", Value = "" });
            ViewBag.drpDonViQuanLy = dataDropDown;
            return PartialView("_list", data);
        }
        #endregion

        //public JsonResult GetDataThongTinChiTietLoaiNganSach(Guid iIdDuAn, string iIdMaDonViQuanLy, int iIdNguonVon, DateTime? dNgayLap, int iNamKeHoach)
        //{
        //    IEnumerable<VDTKHVPhanBoVonChiTietViewModel> data = _vdtService.GetMucLucNganSachByKeHoachVon(iIdDuAn, iIdMaDonViQuanLy, iIdNguonVon, dNgayLap, iNamKeHoach);
        //    List<SelectListItem> lstData = new List<SelectListItem>();
        //    lstData.Add(new SelectListItem()
        //    {
        //        Text = Constants.CHON,
        //        Value = string.Empty
        //    });
        //    foreach (VDTKHVPhanBoVonChiTietViewModel item in data)
        //    {
        //        lstData.Add(new SelectListItem()
        //        {
        //            Text = string.Format("{0} - {1} - {2} - {3}", item.sM, item.sTM, item.sTTM, item.sNG),
        //            Value = item.iID_NganhID.ToString()
        //        });
        //    }
        //    return Json(new { data = lstData, bIsComplete = true }, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetDataNguonVon(Guid iIdDuAn)
        {
            List<NS_NguonNganSach> data = _vdtService.LayDanhSachNguonVonTheoDuAnInQDDauTu(iIdDuAn).ToList();
            StringBuilder htmlString = new StringBuilder();
            if (data != null && data.Count() > 0)
            {
                htmlString.AppendFormat("<option value='{0}'>{1}</option>", Guid.Empty, Constants.CHON);
                for (int i = 0; i < data.Count(); i++)
                {
                    htmlString.AppendFormat("<option value='{0}'>{1}</option>", data[i].iID_MaNguonNganSach, data[i].sTen);
                }
            }
            return Json(htmlString.ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataNhaThau(Guid iIdHopDong)
        {
            List<VDT_DM_NhaThau> data = _vdtService.GetNhaThauByHopDong(iIdHopDong);
            StringBuilder htmlString = new StringBuilder();
            if (data != null && data.Count() > 0)
            {
                htmlString.AppendFormat("<option value='{0}'>{1}</option>", Guid.Empty, Constants.CHON);
                for (int i = 0; i < data.Count(); i++)
                {
                    htmlString.AppendFormat("<option value='{0}'>{1}</option>", data[i].iID_NhaThauID, data[i].sTenNhaThau);
                }
            }
            return Json(htmlString.ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult LayDanhSachDuAnTheoChuDauTu(string iIDChuDauTuID)
        {
            //List<VDT_DA_DuAn> lstDuAn = _vdtService.LayDanhSachDuAnTheoChuDauTu(Guid.Parse(iIDChuDauTuID)).ToList();
            List<VDT_DA_DuAn> lstDuAn = _vdtService.LayDanhSachDuAnTheoChuDauTu(iIDChuDauTuID).ToList();
            StringBuilder htmlString = new StringBuilder();
            if (lstDuAn != null && lstDuAn.Count > 0)
            {
                htmlString.AppendFormat("<option value='{0}'>{1}</option>", Guid.Empty, Constants.CHON);
                for (int i = 0; i < lstDuAn.Count; i++)
                {
                    htmlString.AppendFormat("<option value='{0}' data-sMaDuAn='{1}'>{2}</option>", lstDuAn[i].iID_DuAnID, lstDuAn[i].sMaDuAn, lstDuAn[i].sTenDuAn);
                }
            }
            return Json(htmlString.ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult LayDataDropdownPheDuyet(Guid iIdDeNghiThanhToanId)
        {
            GiaiNganThanhToanViewModel data = _vdtService.GetDeNghiThanhToanDetailByID(iIdDeNghiThanhToanId);
            List<VdtTtKeHoachVonQuery> _lstKeHoachVonThanhToan = _vdtService.LoadKeHoachVonThanhToan(data.iID_DuAnId.ToString(), data.iID_NguonVonID.Value,
                data.dNgayDeNghi.Value, data.iNamKeHoach.Value, data.iCoQuanThanhToan.Value, data.iID_DeNghiThanhToanID);

            Dictionary<Guid, List<MlnsByKeHoachVonModel>> _dicMucLucByKHV = new Dictionary<Guid, List<MlnsByKeHoachVonModel>>();

            if (_lstKeHoachVonThanhToan != null && _lstKeHoachVonThanhToan.Any())
            {
                List<TongHopNguonNSDauTuQuery> lstChungTu = _lstKeHoachVonThanhToan.Select(n => new TongHopNguonNSDauTuQuery()
                {
                    iID_ChungTu = n.IIdKeHoachVonId,
                    iID_DuAnID = data.iID_DuAnId.Value,
                    sMaNguon = n.ILoaiKeHoachVon == 1 ? LOAI_CHUNG_TU.KE_HOACH_VON_NAM : LOAI_CHUNG_TU.KE_HOACH_VON_UNG
                }).ToList();
                var lstDataMlns = _vdtService.GetMucLucNganSachByKeHoachVon(PhienLamViec.NamLamViec, lstChungTu);
                if (lstDataMlns != null)
                {
                    foreach (var item in lstDataMlns)
                    {
                        if (!_dicMucLucByKHV.ContainsKey(item.IidKeHoachVonId))
                            _dicMucLucByKHV.Add(item.IidKeHoachVonId, new List<MlnsByKeHoachVonModel>());
                        _dicMucLucByKHV[item.IidKeHoachVonId].Add(item);
                    }
                }
            }

            List<ComboboxItem> lstData = new List<ComboboxItem>();
            if (data.iLoaiThanhToan == (int)PaymentTypeEnum.Type.THANH_TOAN)
            {
                lstData.Add(new ComboboxItem()
                {
                    ValueItem = ((int)PaymentTypeEnum.Type.THANH_TOAN).ToString(),
                    DisplayItem = PaymentTypeEnum.TypeName.THANH_TOAN_KLHT
                });
                if (data.fGiaTriThuHoiNN != 0 || data.fGiaTriThuHoiTN != 0)
                {
                    lstData.Add(new ComboboxItem()
                    {
                        ValueItem = ((int)PaymentTypeEnum.Type.THU_HOI_NAM_TRUOC).ToString(),
                        DisplayItem = PaymentTypeEnum.Get((int)PaymentTypeEnum.Type.THU_HOI_NAM_TRUOC)
                    });
                    lstData.Add(new ComboboxItem()
                    {
                        ValueItem = ((int)PaymentTypeEnum.Type.THU_HOI_NAM_NAY).ToString(),
                        DisplayItem = PaymentTypeEnum.Get((int)PaymentTypeEnum.Type.THU_HOI_NAM_NAY)
                    });
                }
                if (data.fGiaTriThuHoiUngTruocNN != 0 || data.fGiaTriThuHoiUngTruocTN != 0)
                {
                    if (_lstKeHoachVonThanhToan != null && _lstKeHoachVonThanhToan.Any()
                        && (_lstKeHoachVonThanhToan.FirstOrDefault().ILoaiKeHoachVon == (int)LOAI_KHV.Type.KE_HOACH_VON_NAM
                        || _lstKeHoachVonThanhToan.FirstOrDefault().ILoaiKeHoachVon == (int)LOAI_KHV.Type.KE_HOACH_VON_NAM_CHUYEN_SANG))
                    {
                        lstData.Add(new ComboboxItem()
                        {
                            ValueItem = ((int)PaymentTypeEnum.Type.THU_HOI_UNG_TRUOC_NAM_TRUOC).ToString(),
                            DisplayItem = PaymentTypeEnum.Get((int)PaymentTypeEnum.Type.THU_HOI_UNG_TRUOC_NAM_TRUOC)
                        });
                    }
                    else
                    {
                        lstData.Add(new ComboboxItem()
                        {
                            ValueItem = ((int)PaymentTypeEnum.Type.THU_HOI_UNG_TRUOC_NAM_NAY).ToString(),
                            DisplayItem = PaymentTypeEnum.Get((int)PaymentTypeEnum.Type.THU_HOI_UNG_TRUOC_NAM_NAY)
                        });
                    }
                }
            }
            else
            {
                if (data.fGiaTriThuHoiUngTruocNN != 0 || data.fGiaTriThuHoiUngTruocTN != 0)
                {
                    if (_lstKeHoachVonThanhToan != null
                       && (_lstKeHoachVonThanhToan.FirstOrDefault().ILoaiKeHoachVon == (int)LOAI_KHV.Type.KE_HOACH_VON_NAM
                       || _lstKeHoachVonThanhToan.FirstOrDefault().ILoaiKeHoachVon == (int)LOAI_KHV.Type.KE_HOACH_VON_NAM_CHUYEN_SANG))
                    {
                        lstData.Add(new ComboboxItem()
                        {
                            ValueItem = ((int)PaymentTypeEnum.Type.TAM_UNG).ToString(),
                            DisplayItem = PaymentTypeEnum.Get((int)PaymentTypeEnum.Type.TAM_UNG)
                        });

                        lstData.Add(new ComboboxItem()
                        {
                            ValueItem = ((int)PaymentTypeEnum.Type.THU_HOI_UNG_TRUOC_NAM_TRUOC).ToString(),
                            DisplayItem = PaymentTypeEnum.Get((int)PaymentTypeEnum.Type.THU_HOI_UNG_TRUOC_NAM_TRUOC)
                        });

                    }
                }
                //else
                //{
                //    lstData.Add(new ComboboxItem()
                //    {
                //        ValueItem = ((int)PaymentTypeEnum.Type.TAM_UNG).ToString(),
                //        DisplayItem = PaymentTypeEnum.Get((int)PaymentTypeEnum.Type.TAM_UNG)
                //    });
                //}    
                   
             }

            return Json(new { bStatus = true, listLoaiThanhToan = lstData, listKHVTT = _lstKeHoachVonThanhToan, listMLNS = JsonConvert.SerializeObject(_dicMucLucByKHV) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListKeHoachVonThanhToan(GiaiNganThanhToanViewModel model)
        {
            List<VdtTtKeHoachVonQuery> _lstKeHoachVonThanhToan = _vdtService.LoadKeHoachVonThanhToan(model.iID_DuAnId.ToString(), model.iID_NguonVonID.Value,
                model.dNgayDeNghi.Value, model.iNamKeHoach.Value, model.iCoQuanThanhToan.Value, model.iID_DeNghiThanhToanID);

            Dictionary<Guid, List<MlnsByKeHoachVonModel>> _dicMucLucByKHV = new Dictionary<Guid, List<MlnsByKeHoachVonModel>>();

            if (_lstKeHoachVonThanhToan != null && _lstKeHoachVonThanhToan.Any())
            {
                List<TongHopNguonNSDauTuQuery> lstChungTu = _lstKeHoachVonThanhToan.Select(n => new TongHopNguonNSDauTuQuery()
                {
                    iID_ChungTu = n.IIdKeHoachVonId,
                    iID_DuAnID = model.iID_DuAnId.Value,
                    sMaNguon = n.ILoaiKeHoachVon == 1 ? LOAI_CHUNG_TU.KE_HOACH_VON_NAM : LOAI_CHUNG_TU.KE_HOACH_VON_UNG
                }).ToList();
                var lstData = _vdtService.GetMucLucNganSachByKeHoachVon(PhienLamViec.NamLamViec, lstChungTu);
                if (lstData != null)
                {
                    foreach (var item in lstData)
                    {
                        if (!_dicMucLucByKHV.ContainsKey(item.IidKeHoachVonId))
                            _dicMucLucByKHV.Add(item.IidKeHoachVonId, new List<MlnsByKeHoachVonModel>());
                        _dicMucLucByKHV[item.IidKeHoachVonId].Add(item);
                    }
                }
            }

            return Json(new { data = _lstKeHoachVonThanhToan, dataMlns = JsonConvert.SerializeObject(_dicMucLucByKHV) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LayDanhSachLoaiThanhToan(GiaiNganThanhToanViewModel model)
        {
            List<VdtTtKeHoachVonQuery> _lstKeHoachVonThanhToan = _vdtService.LoadKeHoachVonThanhToan(model.iID_DuAnId.ToString(), model.iID_NguonVonID.Value,
                model.dNgayDeNghi.Value, model.iNamKeHoach.Value, model.iCoQuanThanhToan.Value, model.iID_DeNghiThanhToanID);

            List<ComboboxItem> lstData = new List<ComboboxItem>();
            if (model.iLoaiThanhToan == (int)PaymentTypeEnum.Type.THANH_TOAN)
            {
                lstData.Add(new ComboboxItem()
                {
                    ValueItem = ((int)PaymentTypeEnum.Type.THANH_TOAN).ToString(),
                    DisplayItem = PaymentTypeEnum.TypeName.THANH_TOAN_KLHT
                });
                if (model.fGiaTriThuHoiNN != 0 || model.fGiaTriThuHoiTN != 0)
                {
                    lstData.Add(new ComboboxItem()
                    {
                        ValueItem = ((int)PaymentTypeEnum.Type.THU_HOI_NAM_TRUOC).ToString(),
                        DisplayItem = PaymentTypeEnum.Get((int)PaymentTypeEnum.Type.THU_HOI_NAM_TRUOC)
                    });
                    lstData.Add(new ComboboxItem()
                    {
                        ValueItem = ((int)PaymentTypeEnum.Type.THU_HOI_NAM_NAY).ToString(),
                        DisplayItem = PaymentTypeEnum.Get((int)PaymentTypeEnum.Type.THU_HOI_NAM_NAY)
                    });
                }
                if (model.fGiaTriThuHoiUngTruocNN != 0 || model.fGiaTriThuHoiUngTruocTN != 0)
                {
                    if (_lstKeHoachVonThanhToan != null && _lstKeHoachVonThanhToan.Any()
                        && (_lstKeHoachVonThanhToan.FirstOrDefault().ILoaiKeHoachVon == (int)LOAI_KHV.Type.KE_HOACH_VON_NAM
                        || _lstKeHoachVonThanhToan.FirstOrDefault().ILoaiKeHoachVon == (int)LOAI_KHV.Type.KE_HOACH_VON_NAM_CHUYEN_SANG))
                    {
                        lstData.Add(new ComboboxItem()
                        {
                            ValueItem = ((int)PaymentTypeEnum.Type.THU_HOI_UNG_TRUOC_NAM_TRUOC).ToString(),
                            DisplayItem = PaymentTypeEnum.Get((int)PaymentTypeEnum.Type.THU_HOI_UNG_TRUOC_NAM_TRUOC)
                        });
                    }
                    else
                    {
                        lstData.Add(new ComboboxItem()
                        {
                            ValueItem = ((int)PaymentTypeEnum.Type.THU_HOI_UNG_TRUOC_NAM_NAY).ToString(),
                            DisplayItem = PaymentTypeEnum.Get((int)PaymentTypeEnum.Type.THU_HOI_UNG_TRUOC_NAM_NAY)
                        });
                    }
                }
            }
            else
            {
                if (model.fGiaTriThuHoiUngTruocNN != 0 || model.fGiaTriThuHoiUngTruocTN != 0)
                {
                    if (_lstKeHoachVonThanhToan != null
                        && (_lstKeHoachVonThanhToan.FirstOrDefault().ILoaiKeHoachVon == (int)LOAI_KHV.Type.KE_HOACH_VON_NAM
                        || _lstKeHoachVonThanhToan.FirstOrDefault().ILoaiKeHoachVon == (int)LOAI_KHV.Type.KE_HOACH_VON_NAM_CHUYEN_SANG))
                    {
                        lstData.Add(new ComboboxItem()
                        {
                            ValueItem = ((int)PaymentTypeEnum.Type.TAM_UNG).ToString(),
                            DisplayItem = PaymentTypeEnum.Get((int)PaymentTypeEnum.Type.TAM_UNG)
                        });

                        lstData.Add(new ComboboxItem()
                        {
                            ValueItem = ((int)PaymentTypeEnum.Type.THU_HOI_UNG_TRUOC_NAM_TRUOC).ToString(),
                            DisplayItem = PaymentTypeEnum.Get((int)PaymentTypeEnum.Type.THU_HOI_UNG_TRUOC_NAM_TRUOC)
                        });
                    }
                }
            }
            return Json(new { bStatus = true, listLoaiThanhToan = lstData }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// load gia tri thanh toan
        /// </summary>
        /// <param name="dNgayDeNghi"></param>
        /// <param name="iID_HopDongID"></param>
        /// <returns></returns>
        public JsonResult LoadGiaTriThanhToan(bool bThanhToanTheoHopDong, string iIdChungTu, DateTime dNgayDeNghi, int iIDNguonVon, int iNamKeHoach, int iCoQuanThanhToan)
        {
            double luyKeTTTN = 0;
            double luyKeTTNN = 0;
            double luyKeTUTN = 0;
            double luyKeTUNN = 0;
            double luyKeTUUngTruocTN = 0;
            double luyKeTUUngTruocNN = 0;
            _vdtService.LoadGiaTriThanhToan(iCoQuanThanhToan, dNgayDeNghi, bThanhToanTheoHopDong, iIdChungTu, iIDNguonVon, iNamKeHoach, ref luyKeTTTN, ref luyKeTTNN, ref luyKeTUTN, ref luyKeTUNN, ref luyKeTUUngTruocTN, ref luyKeTUUngTruocNN);

            return Json(new { luyKeTTTN = luyKeTTTN, luyKeTTNN = luyKeTTNN, luyKeTUTN = luyKeTUTN, luyKeTUNN = luyKeTUNN, luyKeTUUngTruocTN = luyKeTUUngTruocTN, luyKeTUUngTruocNN = luyKeTUUngTruocNN }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadKeHoachVon(Guid? iID_DeNghiThanhToanID, string iIdDuAn, int iIdNguonVon, DateTime dNgayDeNghi, int inamKeHoach, int iCoQuanThanhToan)
        {
            List<KeHoachVonModel> list = _vdtService.GetKeHoachVonCapPhatThanhToan(iIdDuAn, iIdNguonVon, dNgayDeNghi, inamKeHoach, iCoQuanThanhToan, iID_DeNghiThanhToanID.Value);

            if (iID_DeNghiThanhToanID != null && iID_DeNghiThanhToanID != Guid.Empty)
            {
                List<VDT_TT_DeNghiThanhToan_KHV> listKeHoachVon = _vdtService.FindDeNghiThanhToanKHVByDeNghiThanhToanID(iID_DeNghiThanhToanID.Value);
                foreach (KeHoachVonModel item in list)
                {
                    if (listKeHoachVon.Where(n => n.iID_KeHoachVonID == item.Id && n.iLoai == item.iPhanLoai).Count() > 0)
                    {
                        item.IsChecked = true;
                    }
                }
            }
            return Json(new { data = list, bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadChiPhi(DateTime dNgayDeNghi, Guid iIdDuAnId, Guid? iID_DeNghiThanhToanID)
        {
            List<VdtTtDeNghiThanhToanChiPhiQuery> list = _vdtService.GetChiPhiInDenghiThanhToanScreen(dNgayDeNghi, iIdDuAnId);

            if (iID_DeNghiThanhToanID != null && iID_DeNghiThanhToanID != Guid.Empty)
            {
                VDT_TT_DeNghiThanhToan objDeNghiThanhToan = _vdtService.GetDeNghiThanhToanByID(iID_DeNghiThanhToanID.Value);
                if (objDeNghiThanhToan != null && objDeNghiThanhToan.iID_ChiPhiID != null)
                {
                    foreach (VdtTtDeNghiThanhToanChiPhiQuery item in list)
                    {
                        if (item.IIdChiPhiId == objDeNghiThanhToan.iID_ChiPhiID)
                        {
                            item.IsChecked = true;
                        }
                    }
                }
            }
            return Json(new { data = list, bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadDeNghiTamUng(Guid? iID_DeNghiThanhToanID, string iIdDuAn, int iIdNguonVon, DateTime dNgayDeNghi, int inamKeHoach, int iCoQuanThanhToan)
        {
            List<KeHoachVonModel> lstDeNghiTamUngNamTruoc = new List<KeHoachVonModel>();
            List<KeHoachVonModel> lstDeNghiTamUngNamNay = new List<KeHoachVonModel>();

            List<KeHoachVonModel> list = _vdtService.GetDeNghiTamUngCapPhatThanhToan(iIdDuAn, iIdNguonVon, dNgayDeNghi, inamKeHoach, iCoQuanThanhToan, iID_DeNghiThanhToanID.Value);
            if (list == null)
                return Json(new { lstDeNghiTamUngNamTruoc = lstDeNghiTamUngNamTruoc, lstDeNghiTamUngNamNay = lstDeNghiTamUngNamNay }, JsonRequestBehavior.AllowGet);

            if (list.Any(n => n.iPhanLoai == 5))
            {
                lstDeNghiTamUngNamTruoc = list.Where(n => n.iPhanLoai == 5).ToList();
                //_dicGiaTriUngNamTruoc = lstDeNghiTamUngNamTruoc.GroupBy(n => new { n.Id, n.PhanLoai })
                //    .ToDictionary(n => (n.Key.Id.ToString() + "\t" + n.Key.PhanLoai.ToString()), n => n.Sum(k => (k.LenhChi ?? 0)));
            }
            if (list.Any(n => n.iPhanLoai == 6))
            {
                lstDeNghiTamUngNamNay = list.Where(n => n.iPhanLoai == 6).ToList();
                //_dicGiaTriUngNamNay = lstDeNghiTamUngNamNay.GroupBy(n => new { n.Id, n.PhanLoai })
                //    .ToDictionary(n => (n.Key.Id.ToString() + "\t" + n.Key.PhanLoai.ToString()), n => n.Sum(k => (k.LenhChi ?? 0)));
            }
            return Json(new { bIsComplete = true, lstDeNghiTamUngNamTruoc = lstDeNghiTamUngNamTruoc, lstDeNghiTamUngNamNay = lstDeNghiTamUngNamNay }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Process
        [HttpPost]
        public JsonResult InsertDeNghiThanhToan(GiaiNganThanhToanViewModel data)
        {
            if (_vdtService.LuuThanhToan(data, Username))
            {
                return Json(new { bIsComplete = true, iIdDeNghiThanhToanId = data.iID_DeNghiThanhToanID }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertPheDuyetThanhToanChiTiet(List<PheDuyetThanhToanChiTiet> data, Guid iIdDeNghiThanhToanId)
        {
            if (_vdtService.LuuPheDuyetThanhToanChiTiet(iIdDeNghiThanhToanId, data, Username, PhienLamViec.NamLamViec))
            {
                return Json(new { bIsComplete = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdatePheDuyetThanhToanChiTiet(List<PheDuyetThanhToanChiTiet> lstData, Guid iID_DeNghiThanhToanID)
        {
            return Json(new { bIsComplete = _vdtService.UpdatePheDuyetThanhToanChiTiet(lstData, iID_DeNghiThanhToanID, Username, PhienLamViec.NamLamViec) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateDeNghiThanhToan(VDT_TT_DeNghiThanhToan data)
        {
            return Json(new { bIsComplete = _model.UpdateDeNghiThanhToan(data, Username) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool DeleteDeNghiThanhToan(Guid id)
        {
            if (!_model.DeleteDeNghiThanhToan(id, Username)) return false;
            return true;
        }

        #endregion

        #region Export

        public ActionResult XuatFile(Guid id, int type = 1, string ext = "pdf", int dvt = 1)
        {
            ExcelFile excel = null;
            string sFileName = string.Empty;
            if (type == PHAN_GHI_CQTC)
            {
                excel = TaoFilePhanGhiCQTC(id, ext, dvt);
                sFileName = "Phan ghi cua co quan tai chinh";
            }
            else
            {
                excel = TaoFileGiayDeNghiThanhToan(id, ext, dvt);
                sFileName = "Giay de nghi cua co quan thanh toan";
            }
            return Print(excel, ext, string.Format("{0}.{1}", sFileName, ext));
        }

        public ExcelFile TaoFilePhanGhiCQTC(Guid id, string ext = "pdf", int dvt = 1)
        {
            GiaiNganThanhToanViewModel data = _vdtService.GetDeNghiThanhToanDetailByID(id);
            if (data == null)
                return null;
            List<PheDuyetThanhToanChiTiet> listData = _vdtService.GetListPheDuyetChiTietByDeNghiId(id);
            if (listData == null || listData.Count == 0)
                return null;

            List<PheDuyetThanhToanChiTiet> listDataThuHoi = listData.Where(n => (string.IsNullOrEmpty(n.sM) && string.IsNullOrEmpty(n.sTM)
                                                            && string.IsNullOrEmpty(n.sTTM) && string.IsNullOrEmpty(n.sNG))).ToList();

            double thuHoiNamTrcTN = (listDataThuHoi != null && listDataThuHoi.Count > 0) ?
                        listDataThuHoi.Select(n => n.fGiaTriThuHoiNamTruocTN.HasValue ? n.fGiaTriThuHoiNamTruocTN.Value : 0).Sum() : 0;
            double thuHoiNamTrcNN = (listDataThuHoi != null && listDataThuHoi.Count > 0) ?
                listDataThuHoi.Select(n => n.fGiaTriThuHoiNamTruocNN.HasValue ? n.fGiaTriThuHoiNamTruocNN.Value : 0).Sum() : 0;
            double thuHoiNamNayTN = (listData != null && listData.Count > 0) ?
                listData.Select(n => n.fGiaTriThuHoiNamNayTN.HasValue ? n.fGiaTriThuHoiNamNayTN.Value : 0).Sum() : 0;
            double thuHoiNamNayNN = (listData != null && listData.Count > 0) ?
                listData.Select(n => n.fGiaTriThuHoiNamNayNN.HasValue ? n.fGiaTriThuHoiNamNayNN.Value : 0).Sum() : 0;

            var objGiaTriPheDuyet = _vdtService.LoadGiaTriPheDuyetThanhToanByParentId(data.iID_DeNghiThanhToanID);
            var fGiaTriTuChoiTN = data.fGiaTriThanhToanTN - ((data.iLoaiThanhToan == (int)PaymentTypeEnum.Type.THANH_TOAN) ? objGiaTriPheDuyet.ThanhToanTN : objGiaTriPheDuyet.TamUngTN);
            var fGiaTriTuChoiNN = data.fGiaTriThanhToanNN - ((data.iLoaiThanhToan == (int)PaymentTypeEnum.Type.THANH_TOAN) ? objGiaTriPheDuyet.ThanhToanNN : objGiaTriPheDuyet.TamUngNN);

            var fTraDonViThuHuongTN = listData.Where(n => (!string.IsNullOrEmpty(n.sM) || !string.IsNullOrEmpty(n.sTM) || !string.IsNullOrEmpty(n.sTTM) || !string.IsNullOrEmpty(n.sNG))).Sum(n => n.fGiaTriThanhToanTN ?? 0)
                    - thuHoiNamTrcTN - thuHoiNamNayTN;
            var fTraDonViThuHuongNN = listData.Where(n => (!string.IsNullOrEmpty(n.sM) || !string.IsNullOrEmpty(n.sTM) || !string.IsNullOrEmpty(n.sTTM) || !string.IsNullOrEmpty(n.sNG))).Sum(n => n.fGiaTriThanhToanNN ?? 0)
                - thuHoiNamTrcNN - thuHoiNamNayNN;
            var fTongTraDonViThuHuong = fTraDonViThuHuongTN + fTraDonViThuHuongNN;

            XlsFile Result = new XlsFile(true);
            if (data.iLoaiThanhToan == (int)Constants.LoaiThanhToan.Type.THANH_TOAN)
                Result.Open(Server.MapPath(sFilePath_PhanGhiCQTC_ThanhToan));
            else
                Result.Open(Server.MapPath(sFilePath_PhanGhiCQTC_TamUng));

            FlexCelReport fr = new FlexCelReport();
            fr.AddTable("Items", listData.Where(n => (!string.IsNullOrEmpty(n.sM) || !string.IsNullOrEmpty(n.sTM) || !string.IsNullOrEmpty(n.sTTM) || !string.IsNullOrEmpty(n.sNG))).ToList());
            fr.SetValue("ThuHoiNamTrcTN", (thuHoiNamTrcTN / dvt).ToString("#,###", CultureInfo.GetCultureInfo("vi-VN")));
            fr.SetValue("ThuHoiNamTrcNN", (thuHoiNamTrcNN / dvt).ToString("#,###", CultureInfo.GetCultureInfo("vi-VN")));
            fr.SetValue("ThuHoiNamNayTN", (thuHoiNamNayTN / dvt).ToString("#,###", CultureInfo.GetCultureInfo("vi-VN")));
            fr.SetValue("ThuHoiNamNayNN", (thuHoiNamNayNN / dvt).ToString("#,###", CultureInfo.GetCultureInfo("vi-VN")));
            fr.SetValue("ThuHoiNamTrcTong", ((thuHoiNamTrcTN + thuHoiNamTrcNN) / dvt).ToString("#,###", CultureInfo.GetCultureInfo("vi-VN")));
            fr.SetValue("ThuHoiNamNayTong", ((thuHoiNamNayTN + thuHoiNamNayNN) / dvt).ToString("#,###", CultureInfo.GetCultureInfo("vi-VN")));
            fr.SetValue("GiaTriTuChoiTN", fGiaTriTuChoiTN.HasValue ? (fGiaTriTuChoiTN.Value / dvt).ToString("#,###", CultureInfo.GetCultureInfo("vi-VN")) : "");
            fr.SetValue("GiaTriTuChoiNN", fGiaTriTuChoiNN.HasValue ? (fGiaTriTuChoiNN.Value / dvt).ToString("#,###", CultureInfo.GetCultureInfo("vi-VN")) : "");
            fr.SetValue("TongGiaTriTuChoi", (fGiaTriTuChoiTN ?? 0 + fGiaTriTuChoiNN ?? 0).ToString("#,###", CultureInfo.GetCultureInfo("vi-VN")));
            fr.SetValue("TraDonViThuHuongTN", fTraDonViThuHuongTN.ToString("#,###", CultureInfo.GetCultureInfo("vi-VN")));
            fr.SetValue("TraDonViThuHuongNN", fTraDonViThuHuongNN.ToString("#,###", CultureInfo.GetCultureInfo("vi-VN")));
            fr.SetValue("TongTraDonViThuHuong", fTongTraDonViThuHuong.ToString("#,###", CultureInfo.GetCultureInfo("vi-VN")));
            fr.SetValue("TongTraDonViThuHuongText", DomainModel.CommonFunction.TienRaChu((long)fTongTraDonViThuHuong));
            fr.SetValue("LyDoTuChoi", data.sLyDoTuChoi);
            fr.SetValue("GhiChuPheDuyet", data.sGhiChuPheDuyet);
            fr.SetValue("Ngay", data.dNgayPheDuyet.HasValue ? data.dNgayPheDuyet.Value.ToString("dd/MM/yyyy") : string.Empty);

            fr.UseChuKy(Username)
                 .UseChuKyForController(sControlName)
                 .UseForm(this).Run(Result);
            return Result;
        }

        public ExcelFile TaoFileGiayDeNghiThanhToan(Guid id, string ext = "pdf", int dvt = 1)
        {
            FlexCelReport fr = new FlexCelReport();
            CapPhatThanhToanReportQuery dataReport = _vdtService.GetThongTinPhanGhiCoQuanTaiChinh(id, PhienLamViec.NamLamViec);

            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(dataReport.iLoaiThanhToan == (int)PaymentTypeEnum.Type.THANH_TOAN ? sFilePath_GiayDeNghiCoQuanThanhToan_ThanhToan : sFilePath_GiayDeNghiCoQuanThanhToan_TamUng));

            fr.SetValue("DonViTinh", dvt.ToStringDvt());
            fr.SetValue("TenDuAn", dataReport.TenDuAn);
            fr.SetValue("MaDuAn", dataReport.MaDuAn);
            fr.SetValue("ChuDauTu", dataReport.TenChuDauTu);
            fr.SetValue("TenHopDong", dataReport.TenHopDong);
            fr.SetValue("NgayHopDong", dataReport.NgayHopDong.HasValue ? dataReport.NgayHopDong.Value.Day.ToString() : string.Empty);
            fr.SetValue("ThangHopDong", dataReport.NgayHopDong.HasValue ? dataReport.NgayHopDong.Value.Month.ToString() : string.Empty);
            fr.SetValue("NamHopDong", dataReport.NgayHopDong.HasValue ? dataReport.NgayHopDong.Value.Year.ToString() : string.Empty);
            fr.SetValue("NguonVon", dataReport.TenNguonVon);
            fr.SetValue("NamKeHoach", dataReport.NamKeHoach);
            fr.SetValue("GiaTriHopDong", dataReport.GiaTriHopDong / dvt);
            fr.SetValue("NoiDung", dataReport.NoiDung);
            fr.SetValue("ThanhToanTN", dataReport.ThanhToanTN / dvt);
            fr.SetValue("ThanhToanNN", dataReport.ThanhToanNN / dvt);
            fr.SetValue("ThueGiaTriGiaTang", dataReport.ThueGiaTriGiaTang / dvt);
            fr.SetValue("ChuyenTienBaoHanh", dataReport.ChuyenTienBaoHanh / dvt);
            fr.SetValue("ThuHuongTN", (dataReport.ThanhToanTN - dataReport.ThuHoiTN) / dvt);
            fr.SetValue("ThuHuongNN", (dataReport.ThanhToanNN - dataReport.ThuHoiNN) / dvt);
            fr.SetValue("TenNhaThau", dataReport.sTenDonViThuHuong);
            fr.SetValue("SoTaiKhoanNhaThau", dataReport.sSoTaiKhoanNhaThau);
            fr.SetValue("CoQuanThanhToan", dataReport.sMaNganHang);
            fr.SetValue("SSoBangKlht", dataReport.sSoBangKLHT);
            fr.SetValue("MaSoDVSDNS", dataReport.MaSoDVSDNS);
            fr.SetValue("STKTrongNuoc", dataReport.STKTrongNuoc);
            fr.SetValue("ChiNhanhTrongNuoc", dataReport.ChiNhanhTrongNuoc);
            fr.SetValue("STKNuocNgoai", dataReport.STKNuocNgoai);
            fr.SetValue("ChiNhanhNuocNgoai", dataReport.ChiNhanhNuocNgoai);
            if (dataReport.dNgayBangKLHT.HasValue)
                fr.SetValue("SNgayBangKlht", String.Format("ngày {0} tháng {1} năm {2}", dataReport.dNgayBangKLHT.Value.Day, dataReport.dNgayBangKLHT.Value.Month, dataReport.dNgayBangKLHT.Value.Year));
            else
                fr.SetValue("SNgayBangKlht", String.Format("ngày {0} tháng {0} năm {0}", "..."));
            fr.SetValue("FLuyKeGiaTriNghiemThuKlht", dataReport.fLuyKeGiaTriNghiemThuKLHT);
            fr.SetValue("FTongThanhToan", (dataReport.fGiaTriThanhToanTN + dataReport.fGiaTriThanhToanNN) / dvt);
            fr.SetValue("sTongThanhToan", DomainModel.CommonFunction.TienRaChu((long)((dataReport.fGiaTriThanhToanTN + dataReport.fGiaTriThanhToanNN)) / dvt));
            fr.SetValue("FThuHoiTamUng", (dataReport.fGiaTriThuHoiUngTruocTN + dataReport.fGiaTriThuHoiUngTruocNN + dataReport.fGiaTriThuHoiTN + dataReport.fGiaTriThuHoiNN) / dvt);
            fr.SetValue("ThuHoiTN", (dataReport.fGiaTriThuHoiUngTruocTN + dataReport.fGiaTriThuHoiTN) / dvt);
            fr.SetValue("ThuHoiNN", (dataReport.fGiaTriThuHoiUngTruocNN + dataReport.fGiaTriThuHoiNN) / dvt);


            if (dataReport.iCoQuanThanhToan.HasValue && dataReport.iCoQuanThanhToan.Value == (int)CoQuanThanhToan.Type.KHO_BAC)
            {
                fr.SetValue("TenCoQuan", "Kho bạc nhà nước Thành phố Hà Nội");
            }
            else if (dataReport.iCoQuanThanhToan.HasValue && dataReport.iCoQuanThanhToan.Value == (int)CoQuanThanhToan.Type.CQTC)
            {
                fr.SetValue("TenCoQuan", "Cơ quan thanh toán");
            }

            double luyKeTTTN = 0;
            double luyKeTTNN = 0;
            double luyKeTUTN = 0;
            double luyKeTUNN = 0;
            double luyKeTUUngTruocTN = 0;
            double luyKeTUUngTruocNN = 0;

            Guid iIdChungTu = new Guid();
            if (dataReport.bThanhToanTheoHopDong.HasValue && dataReport.bThanhToanTheoHopDong.Value)
                iIdChungTu = (dataReport.iID_HopDongId ?? Guid.Empty);
            else
                iIdChungTu = (dataReport.iID_ChiPhiID ?? Guid.Empty);

            if (dataReport.dNgayDeNghi.HasValue && iIdChungTu != Guid.Empty)
            {
                _vdtService.LoadGiaTriThanhToan(dataReport.iCoQuanThanhToan.Value, dataReport.dNgayDeNghi.Value, dataReport.bThanhToanTheoHopDong.Value, iIdChungTu.ToString(), dataReport.iID_NguonVonID.Value, dataReport.iNamKeHoach.Value,
                    ref luyKeTTTN, ref luyKeTTNN, ref luyKeTUTN, ref luyKeTUNN, ref luyKeTUUngTruocTN, ref luyKeTUUngTruocNN);
            }
            fr.SetValue("LuyKeTN", (luyKeTTTN + luyKeTUTN + luyKeTUUngTruocTN) / dvt);
            fr.SetValue("LuyKeNN", (luyKeTTNN + luyKeTUNN + luyKeTUUngTruocNN) / dvt);

            fr.UseChuKy(Username)
                 .UseChuKyForController(sControlName)
                 .UseForm(this).Run(Result);
            return Result;
        }
        #endregion
    }
}