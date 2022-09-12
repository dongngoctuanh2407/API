﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using VIETTEL.Flexcel;

namespace VIETTEL.Report_Controllers.DuToan
{
    public class rptDuToan_1020200_ChonToController : AppController
    {

        public string sViewPath = "~/Report_Views/DuToan/";
        private const String sFilePath = "/Report_ExcelFrom/DuToan/TrinhKy/rptDuToan_1020200_ChonTo.xls";
        private const String sFilePath_to2 = "/Report_ExcelFrom/DuToan/TrinhKy/rptDuToan_1020200_ChonTo_To2.xls";

        [Authorize]
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                ViewData["path"] = "~/Report_Views/DuToan/TrinhKy/rptDuToan_1020200_ChonTo.aspx";
                ViewData["PageLoad"] = "0";
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// hàm lấy các giá trên form
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditSubmit(String ParentID)
        {
            
            String MaTo = Request.Form["MaTo"];
            ViewData["PageLoad"] = "1";
            ViewData["MaTo"] = MaTo;
            ViewData["path"] = "~/Report_Views/DuToan/TrinhKy/rptDuToan_1020200_ChonTo.aspx";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        [Authorize]
        public ExcelFile CreateReport(string sLNS,string MaTo)
        {

            var file = MaTo == "1" ? sFilePath : sFilePath_to2; 
            XlsFile xls = new XlsFile(true);
            xls.Open(Server.MapPath(file));

            FlexCelReport fr = new FlexCelReport();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(Username);
            String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
            DataTable dtDonVi = DuToan_ReportModels.dtDonViAll(Username, sLNS);
            //Lay ten don vi

            String iID_MaDonVi = "";
            for (int i = 0; i < dtDonVi.Rows.Count; i++)
            {
                iID_MaDonVi += dtDonVi.Rows[i]["iID_MaDonVi"].ToString() + ",";
            }
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = iID_MaDonVi.Substring(0, iID_MaDonVi.Length - 1);
            }
            String[] arrDonVi = iID_MaDonVi.Split(',');
            String DonVi = iID_MaDonVi;
            String[] TenDV;
            if (MaTo == "1")
            {
                if (arrDonVi.Length < 5)
                {
                    int a1 = 5 - arrDonVi.Length;
                    for (int i = 0; i < a1; i++)
                    {
                        DonVi += ",-1";
                    }
                }
                arrDonVi = DonVi.Split(',');
                TenDV = new String[5];
                for (int i = 0; i < 5; i++)
                {
                    if (arrDonVi[i] != null && arrDonVi[i] != "-1" && arrDonVi[i] != Guid.Empty.ToString() && arrDonVi[i] != "")
                    {
                        TenDV[i] = DonViModels.Get_TenDonVi(arrDonVi[i], Username);
                    }
                }

                for (int i = 1; i <= TenDV.Length; i++)
                {
                    fr.SetValue("DonVi" + i, TenDV[i - 1]);
                }
            }
            else
            {
                if (arrDonVi.Length < 5 + 6 * (Convert.ToInt16(MaTo) - 1))
                {
                    int a1 = 5 + 6 * (Convert.ToInt16(MaTo) - 1) - arrDonVi.Length;
                    for (int i = 0; i < a1; i++)
                    {
                        DonVi += ",-1";
                    }
                    arrDonVi = DonVi.Split(',');
                }
                TenDV = new String[6];
                int x = 1;
                for (int i = 5 + 6 * ((Convert.ToInt16(MaTo) - 2)); i < 5 + 6 * ((Convert.ToInt16(MaTo) - 1)); i++)
                {
                    if (arrDonVi[i] != null && arrDonVi[i] != "-1" && arrDonVi[i] != Guid.Empty.ToString() && arrDonVi[i] != "")
                    {
                        TenDV[x - 1] = DonViModels.Get_TenDonVi(arrDonVi[i], Username);
                        x++;
                    }
                }

                for (int i = 1; i <= TenDV.Length; i++)
                {
                    fr.SetValue("DonVi" + i, TenDV[i - 1]);
                }
            }
            dtDonVi.Dispose();
           
            LoadData(fr, Username,sLNS,MaTo); 
            fr.SetValue("MaTo", MaTo);

            fr.UseCommonValue()
                .UseChuKy(Username)
                .UseChuKyForController(this.ControllerName())
                .Run(xls);

            return xls;

        }

        //1020200
        /// <summary>
        /// Phụ lục 2c-c
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        public static DataTable DT_rptDuToan_1020200_ChonTo(String MaND, String sLNS, String MaTo)
        {

            String DKDonVi = "", DKPhongBan = "", DK = "";
            SqlCommand cmd = new SqlCommand();
            DataTable dtDonVi = DuToan_ReportModels.dtDonViAll(MaND, sLNS);
            String iID_MaDonVi = "";
            for (int i = 0; i < dtDonVi.Rows.Count; i++)
            {
                iID_MaDonVi += dtDonVi.Rows[i]["iID_MaDonVi"].ToString() + ",";
            }
            if (!String.IsNullOrEmpty(iID_MaDonVi))
            {
                iID_MaDonVi = iID_MaDonVi.Substring(0, iID_MaDonVi.Length - 1);
            }
            String[] arrDonVi = iID_MaDonVi.Split(',');
            for (int i = 0; i < arrDonVi.Length; i++)
            {
                DK += "iID_MaDonVi=@iID_MaDonVia" + i;
                if (i < arrDonVi.Length - 1)
                    DK += " OR ";
                cmd.Parameters.AddWithValue("@iID_MaDonVia" + i, arrDonVi[i]);
            }
            if (!String.IsNullOrEmpty(DK))
            {
                DK = " AND (" + DK + ")";
            }
            dtDonVi.Dispose();
            String DKSUMDonVi = "", DKCASEDonVi = "", DKHAVINGDonVi = "";
            int SoCotTrang1 = 5;
            int SoCotTrang2 = 6;
            if (MaTo == "1")
            {
                if (arrDonVi.Length < SoCotTrang1)
                {
                    int a = SoCotTrang1 - arrDonVi.Length;
                    for (int i = 0; i < a; i++)
                    {
                        iID_MaDonVi += ",-1";
                    }
                    arrDonVi = iID_MaDonVi.Split(',');
                }
                for (int i = 1; i <= SoCotTrang1; i++)
                {

                    DKSUMDonVi += ",SUM(DonVi" + i + "/" + DuToan_ReportModels.DVT + ") AS DonVi" + i;
                    DKHAVINGDonVi += " OR SUM(DonVi" + i + ")<>0 ";
                    DKCASEDonVi += " ,DonVi" + i + "=SUM(CASE WHEN (iID_MaDonVi=@MaDonVi" + i + " ) THEN {0} ELSE 0 END)";
                    cmd.Parameters.AddWithValue("@MaDonVi" + i, arrDonVi[i-1]);
                }
                DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi");
            }
            else
            {
                if (arrDonVi.Length < SoCotTrang1 + SoCotTrang2 * ((Convert.ToInt16(MaTo) - 1)))
                {
                    int a = SoCotTrang1 + SoCotTrang2 * (Convert.ToInt16(MaTo) - 1) - arrDonVi.Length;
                    for (int i = 0; i < a; i++)
                    {
                        iID_MaDonVi += ",-1";
                    }
                    arrDonVi = iID_MaDonVi.Split(',');
                }
                int tg = Convert.ToInt16(MaTo) - 2;
                int x = 1;
                for (int i = SoCotTrang1 + SoCotTrang2 * tg; i < SoCotTrang1 + SoCotTrang2 * (tg + 1); i++)
                {
                    DKSUMDonVi += ",SUM(DonVi" + x + "/"+DuToan_ReportModels.DVT+") AS DonVi" + x;
                    DKHAVINGDonVi += " OR SUM(DonVi" + x + ")<>0 ";
                    DKCASEDonVi += " ,DonVi" + x + "=SUM(CASE WHEN (iID_MaDonVi=@MaDonVi" + x + ") THEN {0} ELSE 0 END)";
                    cmd.Parameters.AddWithValue("@MaDonVi" + x, arrDonVi[i]);
                    x++;

                }
                DKCASEDonVi = String.Format(DKCASEDonVi, "rTuChi");
            }

            if (!String.IsNullOrEmpty(sLNS))
            {
                DK += " AND sLNS IN (" + sLNS + ")";
            }
            DKDonVi = ThuNopModels.DKDonVi(MaND, cmd);
            DKPhongBan = ThuNopModels.DKPhongBan_QuyetToan(MaND, cmd);
            
            String SQL = "";
            SQL =
              String.Format(@"SELECT SUBSTRING(sLNS,1,1) as sLNS1,
SUBSTRING(sLNS,1,3) as sLNS3,
SUBSTRING(sLNS,1,5) as sLNS5,
 sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
,SUM(CongTrongKy/{6}) as rTuChi
--DKSUM
{4}
FROM
(
SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
,CongTrongKy=SUM(CASE WHEN iNamLamViec=@iNamLamViec THEN rTuChi ELSE 0 END)
{3}
 FROM DT_ChungTuChiTiet
 WHERE iTrangThai=1 AND iNamLamViec=@iNamLamViec  {0} {1} {2}
 GROUP BY sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa,iID_MaDonVi) a
 GROUP BY  sLNS,sL,sK,sM,sTM,sTTM,sNG,sMoTa
 HAVING SUM(CongTrongKy)<>0
--DKHaVing 
{5}
", DK, DKDonVi, DKPhongBan, DKCASEDonVi, DKSUMDonVi, DKHAVINGDonVi,DuToan_ReportModels.DVT);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iNamLamViec", ReportModels.LayNamLamViec(MaND));
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        
        /// <summary>
        /// hàm hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="NamLamViec"></param>
        private void LoadData(FlexCelReport fr, String MaND, String sLNS, String MaTo)
        {
            DataTable data = DT_rptDuToan_1020200_ChonTo(MaND, sLNS, MaTo);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtsTM = HamChung.SelectDistinct("dtsTM", data, "sLNS,sL,sK,sM,sTM", "sLNS,sL,sK,sM,sTM,sMoTa", "sLNS,sL,sK,sM,sTM,sTTM");
            DataTable dtsM = HamChung.SelectDistinct("dtsM", dtsTM, "sLNS,sL,sK,sM", "sLNS,sL,sK,sM,sMoTa", "sLNS,sL,sK,sM,sTM");
            DataTable dtsL = HamChung.SelectDistinct("dtsL", dtsM, "sLNS,sL,sK", "sLNS,sL,sK,sMoTa", "sLNS,sL,sK,sM");
            DataTable dtsLNS = HamChung.SelectDistinct("dtsLNS", dtsL, "sLNS", "sLNS,sMoTa", "sLNS,sL");

            fr.AddTable("dtsTM", dtsTM);
            fr.AddTable("dtsM", dtsM);
            fr.AddTable("dtsL", dtsL);
            fr.AddTable("dtsLNS", dtsLNS);

            data.Dispose();
            dtsTM.Dispose();
            dtsM.Dispose();
            dtsL.Dispose();
            dtsLNS.Dispose();

        }

        
        /// <summary>
        /// hàm view PDF
        /// </summary>
        /// <param name="NamLamViec"></param>
        /// <returns></returns>
        /// 
        [Authorize]
        public ActionResult ViewPDF(string sLNS, string MaTo)
        {
            HamChung.Language(); 
            ExcelFile xls = CreateReport(sLNS, MaTo);
            return xls.ToPdfContentResult(string.Format("{0}_{1}.pdf", this.ControllerName(), DateTime.Now.GetTimeStamp()));
        }

        public ActionResult Download(string sLNS, string MaTo)
        {
            HamChung.Language();

            ExcelFile xls = CreateReport(sLNS, MaTo);
            return xls.ToFileResult(string.Format("{0}_{1}.xls", this.ControllerName(), DateTime.Now.GetTimeStamp()));
        }
    }
}

