using FlexCel.Core;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Viettel.Domain.DomainModel;
using Viettel.Models.QLNH.BaoCaoTHTHDuAn;
using Viettel.Services;
using VIETTEL.Controllers;
using VIETTEL.Flexcel;
using static Viettel.Services.IQLNHService;

namespace VIETTEL.Areas.QLNH.Controllers.DuAnHopDong
{
    public class BaoCaoTinhHinhThucHienDuAnController : FlexcelReportController
    {
        // GET: QLNH/BaoCaoTinhHinhThucHienDuAn
        private readonly IQLNHService _qlnhService = QLNHService.Default;
        private const string sFilePathChiTiett = "/Report_ExcelFrom/QLNH/rpt_BaoCaoTinhHinhThucHienDuAn.xlsx";

        public ActionResult Index()
        {
            BaoCaoTinhHinhModel vm = new BaoCaoTinhHinhModel();
            vm.ListChiTiet = new BaoCaoTHTHDuAnMoDelPaing();
            vm.ListChiTiet._paging.CurrentPage = 1;
            vm.DuAnModel = new NH_DA_DuAnViewModel();
            List<NS_DonVi> lstDonViQL = _qlnhService.GetLookupDonVi().ToList();
            lstDonViQL.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sTen = "--Chọn đơn vị--" });
            ViewBag.ListThongTinDonVi = lstDonViQL;

            return View(vm);
        }

        [HttpPost]
        public JsonResult GetSeach(Guid id)
        {
            var ListThongTinDuAnByID = _qlnhService.GetLookupDuAnByID(id);

            return Json(new { data = ListThongTinDuAnByID, status = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FindDetails(DateTime? dBatDau, DateTime? dKetThuc, Guid? iID_DuAnID)
        {
            List<NS_DonVi> lstDonViQL = _qlnhService.GetLookupDonVi().ToList();
            lstDonViQL.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sTen = "--Chọn đơn vị--" });
            ViewBag.ListThongTinDonVi = lstDonViQL;
            BaoCaoTinhHinhModel vm = new BaoCaoTinhHinhModel();
            vm.ListChiTiet = new BaoCaoTHTHDuAnMoDelPaing();
            vm.ListChiTiet._paging.CurrentPage = 1;
            if (iID_DuAnID == new Guid())
            {
                vm.DuAnModel = new NH_DA_DuAnViewModel();
                vm.ListChiTiet = new BaoCaoTHTHDuAnMoDelPaing();
                vm.ListChiTiet._paging.CurrentPage = 1;
            }
            else
            {
                vm.DuAnModel = _qlnhService.GetDuAnById(iID_DuAnID);
            }
            var model = _qlnhService.getDeNghiThanhToanModels(ref vm.ListChiTiet._paging, dBatDau, dKetThuc, iID_DuAnID);
            vm.ListChiTiet.Items = model.Items;
            ViewBag.Sum = model.Sum;
            ViewBag.Sumgn = model.Sumgn;
            return PartialView("_list", vm);
        }

        public ActionResult ExportGiayDeNghiThanhToan(string ext = "xls", Guid? iID_DuAnID = null, DateTime? dBatDau = null, DateTime? dKetThuc = null)
        {
            ExcelFile xls = SFilePathChiTiet(iID_DuAnID, dBatDau, dKetThuc);
            string sFileName = "Báo cáo tình hình thực hiện dự án";
            sFileName = string.Format("{0}.{1}", sFileName, ext);
            return Print(xls, ext, sFileName);
        }
        public ExcelFile SFilePathChiTiet(Guid? iID_DuAnID, DateTime? dBatDau, DateTime? dKetThuc)
        {
            List<NS_DonVi> lstDonViQL = _qlnhService.GetLookupDonVi().ToList();
            lstDonViQL.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sTen = "--Chọn đơn vị--" });
            ViewBag.ListThongTinDonVi = lstDonViQL;
            BaoCaoTinhHinhModel vm = new BaoCaoTinhHinhModel();
            vm.ListChiTiet = new BaoCaoTHTHDuAnMoDelPaing();
            vm.ListChiTiet._paging.CurrentPage = 1;
            if (iID_DuAnID == new Guid())
            {
                vm.DuAnModel = new NH_DA_DuAnViewModel();
                vm.ListChiTiet = new BaoCaoTHTHDuAnMoDelPaing();
                vm.ListChiTiet._paging.CurrentPage = 1;
            }
            else
            {
                vm.DuAnModel = _qlnhService.GetDuAnById(iID_DuAnID);
            }

            var model = _qlnhService.getDeNghiThanhToanModels(ref vm.ListChiTiet._paging, dBatDau, dKetThuc, iID_DuAnID);
            vm.ListChiTiet.Items = model.Items;
            ViewBag.Sum = model.Sum;
            ViewBag.Sumgn = model.Sumgn;
            string sTenDonVi = "";
            foreach (var item in ViewBag.ListThongTinDonVi)
            {
                if (item.iID_Ma == vm.DuAnModel.iID_DonViID)
                {
                    sTenDonVi = item.TenDonVi;
                };
            }
            string sTenDuAn = "";
            string sSoQuyetDinhDauTu = "";
            string dNgayQuyetDinhDauTu = "";
            string ChuDauTu = "";
            string sKhoiCong = "";
            string sTen = "";
            string sKetThuc = "";
            string fGiaTriUSD = "";
            if (vm.DuAnModel.sTenDuAn != null)
            {
                sTenDuAn = vm.DuAnModel.sTenDuAn;
            }
            if (vm.DuAnModel.sSoQuyetDinhDauTu != null)
            {
                sSoQuyetDinhDauTu = vm.DuAnModel.sSoQuyetDinhDauTu;
            }
            if (vm.DuAnModel.dNgayQuyetDinhDauTu != null)
            {
                dNgayQuyetDinhDauTu = vm.DuAnModel.dNgayQuyetDinhDauTu.Value.ToString("dd/MM/yyyy");
            }
            if (vm.DuAnModel.ChuDauTu != null)
            {
                ChuDauTu = vm.DuAnModel.ChuDauTu;
            }
            if (vm.DuAnModel.sTen != null)
            {
                sTen = vm.DuAnModel.sTen;
            }
            if (vm.DuAnModel.fGiaTriUSD != null)
            {
                fGiaTriUSD = vm.DuAnModel.fGiaTriUSD.Value.ToString("");
            }
            if (vm.DuAnModel.sKhoiCong != null)
            {
                sKhoiCong = vm.DuAnModel.sKhoiCong;
            }
            if (vm.DuAnModel.sKetThuc != null)
            {
                sKetThuc = vm.DuAnModel.sKetThuc;
            }

            double Sum = ViewBag.Sum;
            double Sumgn = ViewBag.Sumgn;

            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(sFilePathChiTiett));
            FlexCelReport fr = new FlexCelReport();
            fr.SetValue(new
            {
                dvt = "1",
                To = 1,
                sTenDuAn = sTenDuAn,
                sTenDonVi = sTenDonVi,
                sSoQuyetDinhDauTu = sSoQuyetDinhDauTu,
                dNgayQuyetDinhDauTu = dNgayQuyetDinhDauTu,
                ChuDauTu = ChuDauTu,
                sTen = sTen,
                fGiaTriUSD = fGiaTriUSD,
                sKhoiCong = sKhoiCong,
                sKetThuc = sKetThuc,
                Sum = Sum,
                Sumgn = Sumgn
            });

            fr.AddTable("dt", vm.ListChiTiet.Items);

            fr.UseForm(this).Run(Result);
            return Result;
        }
    }

}