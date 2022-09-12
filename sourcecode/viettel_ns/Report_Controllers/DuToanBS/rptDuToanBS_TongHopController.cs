﻿using DomainModel;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Viettel.Data;
using Viettel.Domain.DomainModel;
using Viettel.Services;
using VIETTEL.Controllers;
using VIETTEL.Flexcel;
using VIETTEL.Helpers;
using VIETTEL.Models;

namespace VIETTEL.Report_Controllers.DuToanBS
{
    public class rptDuToanBS_TongHopController : FlexcelReportController
    {
        #region ctor

        private const string _viewPath = "~/Views/Report_Views/DuToanBS/";

        private const string _filePath = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_ChiTieuNganSach.xls";
        private const string _filePath_ngang = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_ChiTieuNganSach_Ngang.xls";
        private const string _filePath_104 = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_ChiTieuNganSach_104.xls";

        private const string _filePath_trinhky = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_ChiTieuNganSach_TrinhKy.xls";
        private const string _filePath_ngang_trinhky = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_ChiTieuNganSach_TrinhKy.xls";
        private const string _filePath_104_trinhky = "/Report_ExcelFrom/DuToanBS/rptDuToanBS_ChiTieuNganSach_104_TrinhKy.xls";

        private int dvt = 1000;
        private readonly INganSachService _nganSachService = NganSachService.Default;
        private readonly IDuToanBsService _dutoanbsService = DuToanBsService.Default;

        public ActionResult Index(int trinhKy = 0)
        {
            var _namLamViec = PhienLamViec.iNamLamViec;

            var dtPhongBan = DuToanBS_ReportModels.LayDSPhongBan(_namLamViec, Username);
            //var dtDot = DuToanBS_ReportModels.LayDSDot(_namLamViec, Username, "");


            var isTLTH = _nganSachService.GetUserRoleType(Username) == (int)UserRoleType.TroLyTongHop;
            var dtDot = isTLTH ?
                _dutoanbsService.GetDots_Gom(_namLamViec, Username) :
                _dutoanbsService.GetDots(_namLamViec, Username);

            var dotList = dtDot
                .ToSelectList("iID_MaDot", "sMoTa", "-1", "-- Chọn đợt --");


            var vm = new rptDuToanBS_ChiTieuNganSachViewModel
            {
                iNamLamViec = _namLamViec,
                PhongBanList = dtPhongBan.ToSelectList("iID_MaPhongBan", "sTenPhongBan"),
                DotList = dotList,
                //DotList = dtDot.ToSelectList("iID_MaDot", "iID_MaDot", "-1", "-- Chọn đợt --"),
                TieuDe = "Giao bổ sung dự toán ngân sách",
                QuyetDinh = "số           /QĐ-BQP ngày      tháng      năm 20      của Bộ Quốc phòng",
                TrinhKy = trinhKy,
            };

            var view = _viewPath + this.ControllerName() + ".cshtml";
            return View(view, vm);
        }

        #endregion

        #region public methods

        public ActionResult Print(
            string ext,
            string iID_MaDot,
            string dNgay,
            string sLNS,
            string iID_MaDonVi,
            string iID_MaPhongBan,
            string tenPhuLuc = null,
            string quyetDinh = null,
            string ghiChu = null,
            int trinhky = 0)
        {
            var path = getFileXls(sLNS, trinhky);

            var xls = createReport(Server.MapPath(path), sLNS, iID_MaDot, dNgay, iID_MaDonVi, iID_MaPhongBan, tenPhuLuc, quyetDinh, ghiChu, trinhky);
            return Print(xls, ext);
        }

        private string getFileXls(string sLNS, int trinhky)
        {
            var path = _filePath;
            // 104 - 7 cột, ngang
            if (sLNS == "1")
            {
                path = trinhky == 0 ? _filePath_104 : _filePath_104_trinhky;
            }
            // 2 cột, dọc
            else if (sLNS == "2")
            {
                path = trinhky == 0 ? _filePath : _filePath_trinhky;
            }
            else if (sLNS == "3")
            {
                path = trinhky == 0 ? _filePath_ngang : _filePath_ngang_trinhky;
            }

            return path;
        }

        public JsonResult Ds_DonVi(string iID_MaDot, string iID_MaPhongBan)
        {
            //var data = DuToanBS_ReportModels.dtDonVi_Dot(iID_MaDot, iID_MaPhongBan, Username);
            //var vm = new ChecklistModel("DonVi", data.ToSelectList("iID_MaDonVi", "TenHT"));

            var isTLTH = _nganSachService.GetUserRoleType(Username) == (int)UserRoleType.TroLyTongHop;
            if (isTLTH)
            {
                iID_MaDot = _dutoanbsService.GetChungTus_Gom(iID_MaDot).Join();
            }
            else
            {
                iID_MaDot = _dutoanbsService.GetChungTus_DotNgay(iID_MaDot, Username, PhienLamViec.iID_MaPhongBan).Join();
            }

            var data = _dutoanbsService.GetDonviTheoChungTus(PhienLamViec.iNamLamViec, iID_MaDot, PhienLamViec.iID_MaDonVi, iID_MaPhongBan);
            var vm = new ChecklistModel("DonVi", data.ToSelectList());
            return ToCheckboxList(vm);

            //var data = NganSachService.Default.GetDonviListByUser(Username, PhienLamViec.iNamLamViec);
            //return ToCheckboxList(new ChecklistModel("DonVi", data.ToSelectList("iID_MaDonVi", "sMoTa")));
        }

        public JsonResult Ds_LNS(
            string iID_MaDot,
            string iID_MaPhongBan,
            string iID_MaDonVi)
        {

            var data = DuToanBS_ReportModels.dtLNS_Dot(iID_MaDot, iID_MaPhongBan, iID_MaDonVi, Username);
            var vm = new ChecklistModel("LNS", data.ToSelectList("sLNS", "TenHT"));

            return ToCheckboxList(vm);
        }
        #endregion

        #region private methods

        private ExcelFile createReport(String path,
            String sLNS,
            String iID_MaDot,
            string dNgay,
            String iID_MaDonVi,
            String iID_MaPhongBan,
            string tenPhuLuc = null,
            string quyetDinh = null,
            string ghiChu = null,
            int trinhky = 0)
        {
            FlexCelReport fr = new FlexCelReport();

            //Lấy dữ liệu chi tiết
            //var data = getDataTable(iID_MaDot, iID_MaPhongBan, iID_MaDonVi);
            var ds = LoadData(fr, iID_MaDot, sLNS, iID_MaPhongBan, iID_MaDonVi, trinhky);

            String sTenDonVi = "";
            String sTenPB = "";

            if (iID_MaPhongBan == "-1")
            {
                sTenPB = "Tất cả các phòng ban ";
            }
            else
                sTenPB = "B " + iID_MaPhongBan;

            if (iID_MaDonVi == "-1")
            {
                sTenDonVi = "Tất cả các đơn vị ";
            }
            else
                sTenDonVi = _nganSachService.GetDonVi(PhienLamViec.iNamLamViec, iID_MaDonVi).sMoTa;

            fr.SetValue("DotNgay", dNgay.ToParamDate().ToStringNgay().ToLower());
            fr.SetValue("sTenPB", sTenPB);
            fr.SetValue("sTenDonVi", sTenDonVi);
            fr.SetValue("TenPhuLuc", string.IsNullOrWhiteSpace(tenPhuLuc) ? "TenPhuLuc" : tenPhuLuc.ToUpper());
            fr.SetValue("QuyetDinh", string.IsNullOrWhiteSpace(quyetDinh) ? "QuyetDinh" : quyetDinh);

            #region ghi chus

            var noteBuilder = new StringBuilder();
            var notes = ds[1].AsEnumerable()
                .Where(x => !string.IsNullOrWhiteSpace(x["sGhiChu"].ToString().Replace(" ", "")))
                .Select(x => $"{x["sXauNoiMa"]} - {x["sMoTa"]} - {x["sGhiChu"]}: {double.Parse(x["rTuChi"].ToString()).ToString("###.###")}")
                .ToList();
            if (notes.Count > 0)
            {
                notes.ForEach(x =>
                {
                    if (!string.IsNullOrWhiteSpace(x)) noteBuilder.AppendLine(x);
                });
            }

            if (!string.IsNullOrWhiteSpace(ghiChu))
                noteBuilder.AppendLine(ghiChu);

            fr.SetValue("GhiChu", noteBuilder.ToString());

            #endregion


            var sTenPhongBan = NganSachService.Default.GetPhongBanById(iID_MaPhongBan).sMoTa;
            fr.SetValue("sTenPhongBan", sTenPhongBan);

            var xls = new XlsFile(true);
            xls.Open(path);

            if (trinhky == 1)
            {
                fr.UseChuKyForController(this.ControllerName());
            }

            fr.UseChuKy(Username, iID_MaPhongBan)
                .UseCommonValue()
                .UseChuKyForController(this.ControllerName(), iID_MaPhongBan)
                .UseForm(this)
                .Run(xls);


            return xls;
        }

        private IList<DataTable> getDataTable(string iID_MaDot, string sLNS, string iID_MaPhongBan, string iID_MaDonVi, int trinhky)
        {
            var ds = new List<DataTable>();
            #region sql

            var sql = string.Empty;

            // phu luc phan cap
            if (trinhky == 2)
            {
                sql = FileHelpers.GetSqlQuery("dtbs_chitieu_phancap.sql");
            }
            else
            {
                sql = sLNS == "1" ?
                FileHelpers.GetSqlQuery("dtbs_chitieu_104.sql") :
                FileHelpers.GetSqlQuery("dtbs_chitieu.sql");
            }

            #endregion

            #region load data

            //var iID_MaChungTu = DuToanBS_ReportModels.GetChungTuList_TheoDot(iID_MaDot, Username);

            var isTLTH = _nganSachService.GetUserRoleType(Username) == (int)UserRoleType.TroLyTongHop;
            var iID_MaChungTu = isTLTH ?
                _dutoanbsService.GetChungTus_Gom(iID_MaDot).Join() :
                _dutoanbsService.GetChungTus_DotNgay(iID_MaDot, Username, iID_MaPhongBan).Join();

            using (var conn = ConnectionFactory.Default.GetConnection())
            {
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@iNamLamViec", PhienLamViec.iNamLamViec);
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                    cmd.Parameters.AddWithValue("@sLNS", DBNull.Value);
                    cmd.Parameters.AddWithValue("@dvt", 1000);

                    var dt = cmd.GetTable();
                    ds.Add(dt);
                }

                // ghi chu
                sql = FileHelpers.GetSqlQuery("dtbs_chitieu_ghichu.sql");
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@iNamLamViec", PhienLamViec.iNamLamViec);
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                    cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                    cmd.Parameters.AddWithValue("@sLNS", DBNull.Value);
                    cmd.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    cmd.Parameters.AddWithValue("@dvt", 1000);

                    var dt = cmd.GetTable();
                    ds.Add(dt);
                }

            }

            #endregion

            return ds;
        }

        private IList<DataTable> LoadData(FlexCelReport fr, string iID_MaDot, string sLNS, string iID_MaPhongBan, string iID_MaDonVi, int trinhky)
        {
            DataRow r;
            DataTable data = new DataTable();

            var ds = getDataTable(iID_MaDot, sLNS, iID_MaPhongBan, iID_MaDonVi, trinhky);

            data = ds[0];

            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtsTM = HamChung.SelectDistinct("dtsTM", data, "iID_MaDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM", "iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            DataTable dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "iID_MaDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM", "iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            DataTable dtsL = HamChung.SelectDistinct("dtsL", dtsM, "iID_MaDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK", "iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            DataTable dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "iID_MaDonVi,sLNS1,sLNS3,sLNS5,sLNS", "iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sLNS,sMoTa", "sLNS,sL");

            DataTable dtsLNS5 = HamChung.SelectDistinct("dtsLNS5", dtsLNS, "iID_MaDonVi,sLNS1,sLNS3,sLNS5", "iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sLNS5,sMoTa");
            for (int i = 0; i < dtsLNS5.Rows.Count; i++)
            {
                r = dtsLNS5.Rows[i];
                r["sMoTa"] = ReportModels.LayMoTa(Convert.ToString(r["sLNS5"]));
            }

            DataTable dtsLNS3 = HamChung.SelectDistinct("dtsLNS3", dtsLNS5, "iID_MaDonVi,sLNS1,sLNS3", "iID_MaDonVi,sTenDonVi,sLNS1,sLNS3,sMoTa");
            for (int i = 0; i < dtsLNS3.Rows.Count; i++)
            {
                r = dtsLNS3.Rows[i];
                r["sMoTa"] = ReportModels.LayMoTa(Convert.ToString(r["sLNS3"]));
            }

            DataTable dtsLNS1 = HamChung.SelectDistinct("dtsLNS1", dtsLNS3, "iID_MaDonVi,sLNS1", "sTenDonVi,iID_MaDonVi,sLNS1,sMoTa");
            for (int i = 0; i < dtsLNS1.Rows.Count; i++)
            {
                r = dtsLNS1.Rows[i];
                r["sMoTa"] = ReportModels.LayMoTa(Convert.ToString(r["sLNS1"]));
            }

            long TongTien = 0;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (data.Rows[i]["rTongSo"].ToString() != "")
                {
                    TongTien += Convert.ToInt64(data.Rows[i]["rTongSo"]);
                }

            }

            //In loại tiền bằng chữ
            String Tien = "";
            TongTien *= dvt;
            if (TongTien < 0)
            {
                TongTien *= -1;
                Tien = "Giảm " + CommonFunction.TienRaChu(TongTien).ToLower();
            }
            else
            {
                Tien = CommonFunction.TienRaChu(TongTien);
            }

            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);
            fr.AddTable("dtsLNS1", dtsLNS1);
            fr.AddTable("dtsLNS3", dtsLNS3);
            fr.AddTable("dtsLNS5", dtsLNS5);

            int count = 0;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (!Convert.ToString(data.Rows[i]["sMoTa"]).Equals(""))
                {
                    count++;
                }
            }
            for (int i = 0; i < dtsTM.Rows.Count; i++)
            {
                if (!Convert.ToString(dtsTM.Rows[i]["sMoTa"]).Equals(""))
                {
                    count++;
                }
            }
            for (int i = 0; i < dtsM.Rows.Count; i++)
            {
                if (!Convert.ToString(dtsM.Rows[i]["sMoTa"]).Equals(""))
                {
                    count++;
                }
            }
            for (int i = 0; i < dtsL.Rows.Count; i++)
            {
                if (!Convert.ToString(dtsL.Rows[i]["sMoTa"]).Equals(""))
                {
                    count++;
                }
            }
            for (int i = 0; i < dtsLNS.Rows.Count; i++)
            {
                if (!Convert.ToString(dtsLNS.Rows[i]["sMoTa"]).Equals(""))
                {
                    count++;
                }
            }
            for (int i = 0; i < dtsLNS1.Rows.Count; i++)
            {
                if (!Convert.ToString(dtsLNS1.Rows[i]["sMoTa"]).Equals(""))
                {
                    count++;
                }
            }
            for (int i = 0; i < dtsLNS3.Rows.Count; i++)
            {
                if (!Convert.ToString(dtsLNS3.Rows[i]["sMoTa"]).Equals(""))
                {
                    count++;
                }
            }
            for (int i = 0; i < dtsLNS5.Rows.Count; i++)
            {
                if (!Convert.ToString(dtsLNS5.Rows[i]["sMoTa"]).Equals(""))
                {
                    count++;
                }
            }

            int row = 20;
            DataTable dtTrang = new DataTable();
            dtTrang.Columns.Add("sTen", typeof(String));
            for (int i = 0; i < row - count; i++)
            {
                DataRow dr = dtTrang.NewRow();
                dtTrang.Rows.Add(dr);
            }
            fr.AddTable("dtTrang", dtTrang);

            fr.SetValue("Tien", Tien);
            data.Dispose();
            dtsTM.Dispose();
            dtsM.Dispose();
            dtsL.Dispose();
            dtsLNS.Dispose();
            dtsLNS1.Dispose();
            dtsLNS3.Dispose();
            dtsLNS5.Dispose();

            return ds;
        }

        #endregion

    }
}
