﻿using FlexCel.Core;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Viettel.Models.QLVonDauTu;
using Viettel.Services;
using VIETTEL.Controllers;

namespace VIETTEL.Areas.QLVonDauTu.Controllers.ThongTinDuAn
{
    public class TongHopThongTinDuAnController : AppController
    {
        INganSachService _iNganSachService = NganSachService.Default;
        IQLVonDauTuService _iQLVonDauTuService = QLVonDauTuService.Default;

        // GET: QLVonDauTu/TongHopThongTinDuAn
        public ActionResult Index()
        {
            ViewBag.ListDonViQuanLy = _iNganSachService.GetDonviListByUser(Username, PhienLamViec.NamLamViec).ToSelectList("iID_Ma", "sTen");
            List<VDTTongHopThongTinDuAnViewModel> arrData = new List<VDTTongHopThongTinDuAnViewModel>();
            IEnumerable<VDTTongHopThongTinDuAnViewModel> lstData = arrData.AsEnumerable();
            return View(lstData);
        }

        [HttpPost]
        public ActionResult GetThongTinTongHopDuAn(Guid iID_DonViQuanLyID, string sTenDuAn, int? iNamKeHoach)
        {
            IEnumerable<VDTTongHopThongTinDuAnViewModel> lstData = _iQLVonDauTuService.GetThongTinTongHopDuAn(iID_DonViQuanLyID, sTenDuAn, iNamKeHoach, PhienLamViec.NamLamViec);

            return PartialView("_partialListTongHopThongTinDuAn", lstData);
        }

        [HttpPost]
        public bool ExportBCTongHopTTDuAn(IEnumerable<VDTTongHopThongTinDuAnViewModel> dataReport)
        {
            TempData["dataReport"] = dataReport;
            return true;
        }

        public ActionResult ExportExcel(Guid iID_DonViQuanLyID, string sTenDuAn, int? iNamKeHoach)
        {
            IEnumerable<VDTTongHopThongTinDuAnViewModel> dataReport = null;
            if (TempData["dataReport"] != null)
            {
                dataReport = (IEnumerable<VDTTongHopThongTinDuAnViewModel>)TempData["dataReport"];
            }
            else
                return RedirectToAction("Index");

            var excel = CreateReport(dataReport, iID_DonViQuanLyID, sTenDuAn, iNamKeHoach);
            using (MemoryStream stream = new MemoryStream())
            {
                excel.Save(stream);
                
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"TongHopThongTinDuAn.xlsx");
            }

            //return RedirectToAction("Index");
        }

        public ExcelFile CreateReport(IEnumerable<VDTTongHopThongTinDuAnViewModel> dataReport, Guid iID_DonViQuanLyID, string sTenDuAn, int? iNamKeHoach)
        {
            IEnumerable<VDTTongHopThongTinDuAnViewModel> lstData = _iQLVonDauTuService.GetThongTinTongHopDuAn(iID_DonViQuanLyID, sTenDuAn, iNamKeHoach, PhienLamViec.NamLamViec);

            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath("~/Areas/QLVonDauTu/ReportExcelForm/rpt_VDT_Tong_Hop_Thong_Tin_Du_An.xlsx"));
            FlexCelReport fr = new FlexCelReport();
            fr.AddTable<VDTTongHopThongTinDuAnViewModel>("TongHop", dataReport);
            fr.AddTable<VDTTongHopThongTinDuAnViewModel>("DaTaDuAn", lstData);
            fr.SetValue("iNamKeHoach", iNamKeHoach);
            fr.SetValue("sTenDVQL", lstData.First().sTen);
            fr.Run(Result);
            return Result;
        }
    }
}