﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Viettel.Domain.DomainModel;
using Viettel.Models.QLVonDauTu;
using Viettel.Services;
using VIETTEL.Controllers;
using VIETTEL.Common;
using System.Text;
using DapperExtensions;
using FlexCel.Core;
using FlexCel.XlsAdapter;
using FlexCel.Report;
using System.IO;

namespace VIETTEL.Areas.QLVonDauTu.Controllers.NganSachQuocPhong
{
    public class QLThongTriThanhToanController : AppController
    {
        IQLVonDauTuService _iQLVonDauTuService = QLVonDauTuService.Default;
        INganSachService _iNganSachService = NganSachService.Default;
        private const string sFilePath = "/Report_ExcelFrom/VonDauTu/rpt_ThongTri_Danhsach.xls";

        // GET: QLVonDauTu/QLThongTriThanhToan
        public ActionResult Index()
        {
            VDTThongTriViewModel vm = new VDTThongTriViewModel();
            vm._paging.CurrentPage = 1;
            vm.Items = _iQLVonDauTuService.LayDanhSachThongTri(ref vm._paging, PhienLamViec.NamLamViec, Username);

            List<NS_DonVi> lstDonViQuanLy = _iNganSachService.GetDonviListByUser(Username, PhienLamViec.NamLamViec).ToList();
            lstDonViQuanLy.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sTen = Constants.TAT_CA });
            ViewBag.ListDonViQuanLy = lstDonViQuanLy.ToSelectList("iID_MaDonVi", "sTen");
            return View(vm);
        }

        [HttpPost]
        public ActionResult TimKiem(PagingInfo _paging, string sMaDonVi, string sMaThongTri, DateTime? dNgayThongTri, int? iNamThongTri)
        {
            VDTThongTriViewModel vm = new VDTThongTriViewModel();
            vm._paging = _paging;
            vm.Items = _iQLVonDauTuService.LayDanhSachThongTri(ref vm._paging, PhienLamViec.NamLamViec, Username, sMaDonVi, sMaThongTri, iNamThongTri, dNgayThongTri);
            List<NS_DonVi> lstDonViQuanLy = _iNganSachService.GetDonviListByUser(Username, PhienLamViec.NamLamViec).ToList();
            lstDonViQuanLy.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sTen = Constants.TAT_CA });
            ViewBag.ListDonViQuanLy = lstDonViQuanLy.ToSelectList("iID_MaDonVi", "sTen");

            // luu dieu kien tim kiem
            TempData["sMaDonvi"] = sMaDonVi;
            TempData["sMaThongTri"] = sMaThongTri;
            TempData["dNgayThongTri"] = dNgayThongTri;
            TempData["iNamThongTri"] = iNamThongTri;

            return PartialView("_list", vm);
        }

        public ActionResult TaoMoi()
        {
            List<NS_DonVi> lstDonViQuanLy = _iNganSachService.GetDonviListByUser(Username, PhienLamViec.NamLamViec).ToList();
            lstDonViQuanLy.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sTen = Constants.CHON });
            ViewBag.ListDonViQuanLy = lstDonViQuanLy.ToSelectList("iID_Ma", "sTen");

            List<NS_NguonNganSach> lstNguonVon = _iQLVonDauTuService.LayNguonVon().ToList();
            lstNguonVon.Insert(0, new NS_NguonNganSach { iID_MaNguonNganSach = 0, sTen = Constants.CHON });
            ViewBag.ListNguonVon = lstNguonVon.ToSelectList("iID_MaNguonNganSach", "sTen");

            return View();
        }

        public ActionResult ChiTiet(string id)
        {
            VDTThongTriModel model = _iQLVonDauTuService.LayChiTietThongTri(id);
            ViewBag.iLoaiCap = (model.bThanhToan.HasValue && model.bThanhToan.Value) ? 1 : 0;
            return View(model);
        }

        public ActionResult Sua(string id)
        {
            VDTThongTriModel model = _iQLVonDauTuService.LayChiTietThongTri(id);
            ViewBag.iLoaiCap = (model.bThanhToan.HasValue && model.bThanhToan.Value) ? 1 : 0;
            return View(model);
        }

        [HttpPost]
        public JsonResult Xoa(string id)
        {
            bool xoa = _iQLVonDauTuService.XoaThongTri(Guid.Parse(id));
            return Json(xoa, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDeNghiThanhToanChiTiet(string iID_MaDonVi, int iNamThongTri, int iNguonVon, DateTime? dNgayLapGanNhat, DateTime? dNgayTaoThongTri)
        {
            var sMaDonViQuanLy = _iNganSachService.GetDonViById(PhienLamViec.iNamLamViec, iID_MaDonVi).iID_MaDonVi;
            var lstDataDeNghiThanhToan = _iQLVonDauTuService.GetDeNghiThanhToanChiTiet(sMaDonViQuanLy, iNamThongTri, iNguonVon, dNgayLapGanNhat, dNgayTaoThongTri);
            return Json(new { data = lstDataDeNghiThanhToan }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDeNghiThanhToanChiTietUng(string sMaDonVi, int iNamThongTri, int iNguonVon, DateTime? dNgayLapGanNhat, DateTime? dNgayTaoThongTri)
        {
            string sMaNhomQuanLy = string.Empty;
            if (iNguonVon == (int)Viettel.Extensions.Constants.NS_NGUON_NGAN_SACH.NS_QUOC_PHONG)
                sMaNhomQuanLy = "CTC";
            else if (iNguonVon == (int)Viettel.Extensions.Constants.NS_NGUON_NGAN_SACH.NS_NHA_NUOC
                || iNguonVon == (int)Viettel.Extensions.Constants.NS_NGUON_NGAN_SACH.NS_DAC_BIET
                || iNguonVon == (int)Viettel.Extensions.Constants.NS_NGUON_NGAN_SACH.NS_KHAC)
                sMaNhomQuanLy = "CKHDT";
            var lstDataDeNghiThanhToanUng = _iQLVonDauTuService.GetDeNghiThanhToanChiTietUng(sMaDonVi, iNamThongTri, sMaNhomQuanLy, dNgayLapGanNhat, dNgayTaoThongTri);
            return Json(new { data = lstDataDeNghiThanhToanUng }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LayNgayLapGanNhat(string iIDDonViID, int iNamThongTri, int iNguonVon)
        {
            string dNgayLapGanNhat = _iQLVonDauTuService.LayNgayLapGanNhat(iIDDonViID, iNamThongTri, iNguonVon);
            return Json(dNgayLapGanNhat == null ? "" : dNgayLapGanNhat, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LayDanhSachNguonVonTheoLoaiCap(int iLoaiCap)
        {
            List<NS_NguonNganSach> lstNguonVon = _iQLVonDauTuService.LayNguonVon().ToList();
            if (iLoaiCap == (int)Viettel.Extensions.Constants.LOAI_CAP.UNG_NGOAI)
            {
                List<string> listSMoTaKeep = new List<string> { "0212", "0213", "0216", "0220" };
                lstNguonVon = lstNguonVon.Where(x => listSMoTaKeep.Contains(x.sMoTa)).ToList();
            }

            StringBuilder htmlString = new StringBuilder();
            if (lstNguonVon != null && lstNguonVon.Count > 0)
            {
                htmlString.AppendFormat("<option value='{0}'>{1}</option>", "", Constants.CHON);
                for (int i = 0; i < lstNguonVon.Count; i++)
                {
                    htmlString.AppendFormat("<option value='{0}'>{1}</option>", lstNguonVon[i].iID_MaNguonNganSach, lstNguonVon[i].sTen);
                }
            }
            return Json(htmlString.ToString(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// kiem tra trung ma thong tri
        /// </summary>
        /// <param name="sMaThongTri"></param>
        /// <returns></returns>
        public JsonResult KiemTraTrungMaThongTri(string sMaThongTri, string iID_ThongTriID = "")
        {
            bool status = _iQLVonDauTuService.KiemTraTrungMaThongTri(sMaThongTri, iID_ThongTriID);
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Luu(VDTThongTriModel model, bool? bReloadChiTiet)
        {
            bool status = _iQLVonDauTuService.LuuThongTinThongTriChiTiet(model, Username, bReloadChiTiet);
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LayThongTinChiTietThongTriChiTiet(string iID_ThongTriID)
        {
            VDTThongTriModel model = _iQLVonDauTuService.LayChiTietThongTri(iID_ThongTriID);
            IEnumerable<VDT_DM_KieuThongTri> danhSachKieuThongTri = _iQLVonDauTuService.LayDanhSachKieuThongTri();
            if (model.bThanhToan.HasValue)
            {
                int iThanhToan = model.bThanhToan.Value ? 1 : 0;
                if (iThanhToan == (int)Viettel.Extensions.Constants.LOAI_CAP.THANH_TOAN)
                {
                    var lstTab1 = _iQLVonDauTuService.LayThongTriChiTietTheoKieuThongTri(iID_ThongTriID, danhSachKieuThongTri.Where(x => x.sMaKieuThongTri == Viettel.Extensions.Constants.CAP_TT_KPQP).First().iID_KieuThongTriID.ToString());
                    var lstTab2 = _iQLVonDauTuService.LayThongTriChiTietTheoKieuThongTri(iID_ThongTriID, danhSachKieuThongTri.Where(x => x.sMaKieuThongTri == Viettel.Extensions.Constants.CAP_TAM_UNG_KPQP).First().iID_KieuThongTriID.ToString());
                    var lstTab3 = _iQLVonDauTuService.LayThongTriChiTietTheoKieuThongTri(iID_ThongTriID, danhSachKieuThongTri.Where(x => x.sMaKieuThongTri == Viettel.Extensions.Constants.THU_UNG_KPQP).First().iID_KieuThongTriID.ToString());
                    return Json(new { lstTab1 = lstTab1, lstTab2 = lstTab2, lstTab3 = lstTab3, lstTab4 = "", lstTab5 = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XuatFile()
        {
            ExcelFile excel = TaoFile();
            using (MemoryStream stream = new MemoryStream())
            {
                excel.Save(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Danh sach thong tri thanh toan.xls");
            }
        }

        public ExcelFile TaoFile()
        {
            string sMaDonVi = string.Empty;
            string sMaThongTri = string.Empty;
            DateTime? dNgayThongTri = null;
            int? iNamThongTri = null;

            // get dieu kien tiem kiem
            if (TempData["sMaDonvi"] != null)
                sMaDonVi = (string)TempData["sMaDonvi"];
            if (TempData["sMaThongTri"] != null)
                sMaThongTri = (string)TempData["sMaThongTri"];
            if (TempData["dNgayThongTri"] != null)
                dNgayThongTri = (DateTime?)TempData["dNgayThongTri"];
            if (TempData["iNamThongTri"] != null)
                iNamThongTri = (int?)TempData["iNamThongTri"];

            IEnumerable<VDTThongTriModel> listData = _iQLVonDauTuService.LayDanhSachThongTriXuatFile(PhienLamViec.NamLamViec, Username, sMaDonVi, sMaThongTri, iNamThongTri, dNgayThongTri);

            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(sFilePath));
            FlexCelReport fr = new FlexCelReport();
            fr.AddTable<VDTThongTriModel>("dt", listData);
            fr.Run(Result);
            return Result;
        }
    }
}