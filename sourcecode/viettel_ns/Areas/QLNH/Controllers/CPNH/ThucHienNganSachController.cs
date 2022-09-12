using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VIETTEL.Controllers;
using Viettel.Domain.DomainModel;
using Viettel.Models.CPNH;
using Viettel.Services;
using DapperExtensions;
using VIETTEL.Helpers;
using System.Data;
using FlexCel.Core;
using FlexCel.Report;
using VIETTEL.Flexcel;
using FlexCel.XlsAdapter;
using System.Text;
using System.Globalization;
using DomainModel;

namespace VIETTEL.Areas.QLNH.Controllers.CPNH
{
    public class ThucHienNganSachController : FlexcelReportController
    {
        private readonly ICPNHService _cpnhService = CPNHService.Default;
        private readonly IQLNguonNganSachService _nnsService = QLNguonNganSachService.Default;
        private const string sFilePathBaoCao1 = "/Report_ExcelFrom/QLNH/rpt_ThucHienNganSach.xlsx";
        private const string sFilePathBaoCao2 = "/Report_ExcelFrom/QLNH/rpt_ThucHienNganSach_GiaiDoan.xlsx";
        private int _columnCountBC1 = 7;
        private const string sControlName = "ThucHienNganSach";

        // GET: QLVonDauTu/QLDMTyGia
        public ActionResult Index()
        {
            CPNHThucHienNganSach_ModelPaging vm = new CPNHThucHienNganSach_ModelPaging();
            List<CPNHThucHienNganSach_Model> list = _cpnhService.getListThucHienNganSachModels(1, DateTime.Now.Year, DateTime.Now.Year  ,null, 0, DateTime.Now.Year).ToList();
            List<ThucHienNganSach_GiaiDoan_Model> lstGiaiDoan = _cpnhService.GetListGiaiDoan(DateTime.Now.Year, DateTime.Now.Year).ToList();
            List<CPNHThucHienNganSach_Model> listData = getList(list , lstGiaiDoan);
            vm.Items = listData;
            List<CPNHNhuCauChiQuy_Model> lstVoucherTypes = new List<CPNHNhuCauChiQuy_Model>()
                {
                    new CPNHNhuCauChiQuy_Model(){SQuyTypes = "--Tất cả--", iQuy = 0},
                    new CPNHNhuCauChiQuy_Model(){SQuyTypes = "Quý 1", iQuy = 1},
                    new CPNHNhuCauChiQuy_Model(){SQuyTypes = "Quý 2", iQuy = 2},
                    new CPNHNhuCauChiQuy_Model(){SQuyTypes = "Quý 3", iQuy = 3},
                    new CPNHNhuCauChiQuy_Model(){SQuyTypes = "Quý 4", iQuy = 4}
                };
            ViewBag.Count = vm.Items.Count();
            ViewBag.ListQuyTypes = lstVoucherTypes.ToSelectList("iQuy", "SQuyTypes");

            List<NS_DonVi> lstDonViQuanLy = _cpnhService.GetDonviListByYear(PhienLamViec.NamLamViec).ToList();
            lstDonViQuanLy.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sMoTa = "--Tất cả--" });
            ViewBag.ListDonViQuanLy = lstDonViQuanLy.ToSelectList("iID_Ma", "sMoTa");
            ViewBag.GiaiDoan = lstGiaiDoan;
            ViewBag.SoGiaiDoan = lstGiaiDoan.Count;
            List<Dropdown_ByYear_ThucHienNganSach> lstNam = GetListNamKeHoach().ToList();
            ViewBag.ListYear = lstNam;
            ViewBag.YearNow = DateTime.Now.Year;
            return View(vm);
        }

        [HttpPost]
        public ActionResult ThucHienNganSachSearch(int tabTable, int iTuNam, int iDenNam, Guid? iDonvi, int iQuyList, int iNam)
        {
            CPNHThucHienNganSach_ModelPaging vm = new CPNHThucHienNganSach_ModelPaging();
            List<CPNHThucHienNganSach_Model> list = _cpnhService.getListThucHienNganSachModels(tabTable, iTuNam, iDenNam, iDonvi, iQuyList, iNam).ToList();
            List<ThucHienNganSach_GiaiDoan_Model> lstGiaiDoan = _cpnhService.GetListGiaiDoan(iTuNam, iDenNam).ToList();
            List<CPNHThucHienNganSach_Model> listData = getList(list, lstGiaiDoan);
            vm.Items = listData;
            List<CPNHNhuCauChiQuy_Model> lstVoucherTypes = new List<CPNHNhuCauChiQuy_Model>()
                {
                    new CPNHNhuCauChiQuy_Model(){SQuyTypes = "--Tất cả--", iQuy = 0},
                    new CPNHNhuCauChiQuy_Model(){SQuyTypes = "Quý 1", iQuy = 1},
                    new CPNHNhuCauChiQuy_Model(){SQuyTypes = "Quý 2", iQuy = 2},
                    new CPNHNhuCauChiQuy_Model(){SQuyTypes = "Quý 3", iQuy = 3},
                    new CPNHNhuCauChiQuy_Model(){SQuyTypes = "Quý 4", iQuy = 4}
                };
            ViewBag.Count = vm.Items.Count();
            ViewBag.ListQuyTypes = lstVoucherTypes.ToSelectList("iQuy", "SQuyTypes");

            List<NS_DonVi> lstDonViQuanLy = _cpnhService.GetDonviListByYear(PhienLamViec.NamLamViec).ToList();
            lstDonViQuanLy.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sMoTa = "--Tất cả--" });
            ViewBag.ListDonViQuanLy = lstDonViQuanLy.ToSelectList("iID_Ma", "sMoTa");
            ViewBag.GiaiDoan = lstGiaiDoan;
            ViewBag.SoGiaiDoan = lstGiaiDoan.Count;
            List<Dropdown_ByYear_ThucHienNganSach> lstNam = GetListNamKeHoach().ToList();
            ViewBag.ListYear = lstNam;
            ViewBag.YearNow = DateTime.Now.Year;

            return PartialView("_list", vm);
        }

        public ActionResult ExportExcelBaoCao(string ext = "xls", int dvt = 1, int to = 1, int tabTable = 1, int iTuNam = 2022, int iDenNam = 2022, Guid? iDonvi = null, int iQuyList = 0, int iNam = 2022)
        {
            string fileName = string.Format("{0}.{1}", "BaoCaoTinhHinhThucHienNganSach", ext);
            List<CPNHThucHienNganSach_Model> list = _cpnhService.getListThucHienNganSachModels(tabTable, iTuNam, iDenNam, iDonvi, iQuyList, iNam).ToList();
            ExcelFile xls = null;
            if (tabTable == 1 )
            {
                xls = TaoFileBaoCao1(dvt, to , list);
            }
            else
            {
                xls = TaoFileBaoCao2(dvt, to , list);
            }
            return Print(xls, ext, fileName);
        }

        public ExcelFile TaoFileBaoCao1(int dvt = 1, int to = 1 , List<CPNHThucHienNganSach_Model> list = null)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(sFilePathBaoCao1));
            FlexCelReport fr = new FlexCelReport();

            int columnStart = _columnCountBC1 * (to - 1);
                        List<CPNHThucHienNganSach_Model> listData = getList(list , null);
            fr.AddTable<CPNHThucHienNganSach_Model>("dt", listData);
            fr.SetValue(new
            {
                dvt = dvt.ToStringDvt(),
                To = to,
                iQuy = 0,
                iNam = 0
            });
            fr.UseChuKy(Username)
                .UseChuKyForController(sControlName)
                .UseForm(this).Run(Result);


            return Result;
        }
        public ExcelFile TaoFileBaoCao2(int dvt = 1, int to = 1, List<CPNHThucHienNganSach_Model> list = null)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(sFilePathBaoCao2));
            FlexCelReport fr = new FlexCelReport();
            List<ThucHienNganSach_GiaiDoan_Model> lstGiaiDoan = _cpnhService.GetListGiaiDoan(DateTime.Now.Year, DateTime.Now.Year).ToList();
            foreach (var giaiDoan in lstGiaiDoan)
            {
                giaiDoan.sGiaiDoan = "Giai đoạn " + giaiDoan.iGiaiDoanTu.ToString() + " - " + giaiDoan.iGiaiDoanDen.ToString();

            }
            int columnStart = _columnCountBC1 * (to - 1);
            List<CPNHThucHienNganSach_Model> listData = getList(list , lstGiaiDoan);


            fr.SetValue(new
            {
                dvt = dvt.ToStringDvt(),
                To = to,
                iQuy = 0,
                iNam = 0,
            });
            fr.AddTable<CPNHThucHienNganSach_Model>("dt", listData);
            fr.AddTable<ThucHienNganSach_GiaiDoan_Model>("lstGiaiDoan", lstGiaiDoan);
            fr.AddTable<ThucHienNganSach_GiaiDoan_Model>("lstGiaiDoan2", lstGiaiDoan);
            fr.AddTable<ThucHienNganSach_GiaiDoan_Model>("lstGiaiDoan3", lstGiaiDoan);
            fr.UseChuKy(Username)
            .UseChuKyForController(sControlName)
            .UseForm(this).Run(Result);

            var col1 = 6 + lstGiaiDoan.Count();
            var col2 = col1 + ((lstGiaiDoan.Count() + 1) * 2);
            Result.MergeCells(6, 6, 6, col1);
            Result.MergeCells(6, col1 + 1, 6, col1 + ((lstGiaiDoan.Count() + 1) * 2));
            Result.MergeCells(6, col2 + 1, 6, col2 + ((lstGiaiDoan.Count() + 1) * 2));
            //tạo border format
            var b = Result.GetDefaultFormat;
            b.Borders.Left.Style = TFlxBorderStyle.Thin;
            b.Borders.Right.Style = TFlxBorderStyle.Thin;
            b.Borders.Top.Style = TFlxBorderStyle.Thin;
            b.Borders.Bottom.Style = TFlxBorderStyle.Thin;
            var ApplyFormat = new TFlxApplyFormat();
            ApplyFormat.SetAllMembers(false);
            ApplyFormat.Borders.SetAllMembers(true);
            TCellAddress Cell = null;
            //tìm dòng cuối cùng của bảng
            Cell = Result.Find("Cộng", null, Cell, false, true, true, false);
            //set border cho bảng
            Result.SetCellFormat(6, 2, 8+ listData.Count(), 13 + lstGiaiDoan.Count() * 5, b, ApplyFormat, false);

            return Result;
        }
        private string convertLetter(int input)
        {
            StringBuilder res = new StringBuilder((input - 1).ToString());
            for (int j = 0; j < res.Length; j++)
                res[j] += (char)(17); // '0' is 48, 'A' is 65
            return res.ToString();
        }
        private List<CPNHThucHienNganSach_Model> getList(List<CPNHThucHienNganSach_Model> list, List<ThucHienNganSach_GiaiDoan_Model> lstGiaiDoan)
        {
            List<CPNHThucHienNganSach_Model> listData = new List<CPNHThucHienNganSach_Model>().ToList();
            int SttLoai = 0;
            int SttHopDong = 0;
            int SttDuAn = 0;
            int SttChuongTrinh = 0;
            Guid? idDuAn = null;
            Guid? idHopDong = null;
            Guid? idChuongTrinh = null;
            int? idLoai = null;
            int sttTong = 0;
            CPNHThucHienNganSach_Model DataTong = new CPNHThucHienNganSach_Model();
            DataTong.lstGiaiDoanTTCP = new List<ThucHienNganSach_GiaiDoan_Model>();
            DataTong.lstGiaiDoanKinhPhiDuocCap = new List<ThucHienNganSach_GiaiDoan_Model>();
            DataTong.lstGiaiDoanKinhPhiDaGiaiNgan = new List<ThucHienNganSach_GiaiDoan_Model>();
            DataTong.sTenNoiDungChi = "Tổng Cộng: ";
            var dicTotalGiaiDoanTTCP = new Dictionary<String, double>();
            var dicTotalGiaiDoanKinhPhiDuocCap = new Dictionary<String, double>();
            var dicTotalGiaiDoanKinhPhiDaGiaiNgan = new Dictionary<String, double>();
            if(list != null)
            {
                foreach (var item in list)
                {
                    sttTong++;
                    item.KinhPhiDuocCapChuaChiUSD = item.TongKinhPhiUSD - item.TongKinhPhiDaChiUSD;
                    item.KinhPhiDuocCapChuaChiVND = item.TongKinhPhiVND - item.TongKinhPhiDaChiVND;
                    item.TongKinhPhiUSD = item.KinhPhiVND + item.KinhPhiToYVND;
                    item.TongKinhPhiVND = item.KinhPhiVND + item.KinhPhiToYVND;
                    if (lstGiaiDoan != null)
                    {
                        item.lstGiaiDoanTTCP = new List<ThucHienNganSach_GiaiDoan_Model>();
                        item.lstGiaiDoanKinhPhiDuocCap = new List<ThucHienNganSach_GiaiDoan_Model>();
                        item.lstGiaiDoanKinhPhiDaGiaiNgan = new List<ThucHienNganSach_GiaiDoan_Model>();
                        foreach (var giaiDoan in lstGiaiDoan)
                        {
                            
                            if (item.iGiaiDoanTu == giaiDoan.iGiaiDoanTu && item.iGiaiDoanDen == giaiDoan.iGiaiDoanDen)
                            {

                                item.lstGiaiDoanTTCP.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = item.NCVTTCP });
                                item.lstGiaiDoanKinhPhiDuocCap.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = item.fLuyKeKinhPhiDuocCap_USD, valueVND = item.fLuyKeKinhPhiDuocCap_VND });
                                item.lstGiaiDoanKinhPhiDaGiaiNgan.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = item.fDeNghiQTNamNay_USD, valueVND = item.fDeNghiQTNamNay_VND });
                            }
                            else
                            {
                                item.lstGiaiDoanTTCP.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = 0 });
                                item.lstGiaiDoanKinhPhiDuocCap.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = 0, valueVND = 0 });
                                item.lstGiaiDoanKinhPhiDaGiaiNgan.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = 0, valueVND = 0 });
                            }
                            if (item.iGiaiDoanDen == giaiDoan.iGiaiDoanDen && item.iGiaiDoanTu == giaiDoan.iGiaiDoanTu)
                            {
                                if (dicTotalGiaiDoanTTCP.ContainsKey(giaiDoan.ID.ToString()))
                                {
                                    if (item.NCVTTCP > 0)
                                    {
                                        dicTotalGiaiDoanTTCP[giaiDoan.ID.ToString()] += item.NCVTTCP.Value;
                                    }
                                }
                                else
                                {
                                    dicTotalGiaiDoanTTCP.Add(giaiDoan.ID.ToString(), item.NCVTTCP.Value > 0 ? item.NCVTTCP.Value : 0);
                                }

                                if (dicTotalGiaiDoanKinhPhiDuocCap.ContainsKey(giaiDoan.ID.ToString() + "USD"))
                                {
                                    if (item.fLuyKeKinhPhiDuocCap_USD > 0)
                                    {
                                        dicTotalGiaiDoanKinhPhiDuocCap[giaiDoan.ID.ToString() + "USD"] += item.fLuyKeKinhPhiDuocCap_USD.Value;
                                    }
                                }
                                else
                                {
                                    dicTotalGiaiDoanKinhPhiDuocCap.Add(giaiDoan.ID.ToString() + "USD", item.fLuyKeKinhPhiDuocCap_USD.Value > 0 ? item.fLuyKeKinhPhiDuocCap_USD.Value : 0);
                                }

                                if (dicTotalGiaiDoanKinhPhiDuocCap.ContainsKey(giaiDoan.ID.ToString() + "VND"))
                                {
                                    if (item.fLuyKeKinhPhiDuocCap_VND > 0)
                                    {
                                        dicTotalGiaiDoanKinhPhiDuocCap[giaiDoan.ID.ToString() + "VND"] += item.fLuyKeKinhPhiDuocCap_VND.Value;
                                    }
                                }
                                else
                                {
                                    dicTotalGiaiDoanKinhPhiDuocCap.Add(giaiDoan.ID.ToString() + "VND", item.fLuyKeKinhPhiDuocCap_VND.Value > 0 ? item.fLuyKeKinhPhiDuocCap_VND.Value : 0);
                                }

                                if (dicTotalGiaiDoanKinhPhiDaGiaiNgan.ContainsKey(giaiDoan.ID.ToString() + "USD"))
                                {
                                    if (item.fDeNghiQTNamNay_USD > 0)
                                    {
                                        dicTotalGiaiDoanKinhPhiDaGiaiNgan[giaiDoan.ID.ToString() + "USD"] += item.fDeNghiQTNamNay_USD.Value;
                                    }
                                }
                                else
                                {
                                    dicTotalGiaiDoanKinhPhiDaGiaiNgan.Add(giaiDoan.ID.ToString() + "USD", item.fDeNghiQTNamNay_USD.Value > 0 ? item.fDeNghiQTNamNay_USD.Value : 0);
                                }
                                if (dicTotalGiaiDoanKinhPhiDaGiaiNgan.ContainsKey(giaiDoan.ID.ToString() + "VND"))
                                {
                                    if (item.fDeNghiQTNamNay_VND > 0)
                                    {
                                        dicTotalGiaiDoanKinhPhiDaGiaiNgan[giaiDoan.ID.ToString() + "VND"] += item.fDeNghiQTNamNay_VND.Value;
                                    }
                                }
                                else
                                {
                                    dicTotalGiaiDoanKinhPhiDaGiaiNgan.Add(giaiDoan.ID.ToString() + "VND", item.fDeNghiQTNamNay_VND.Value > 0 ? item.fDeNghiQTNamNay_VND.Value : 0);
                                }
                            }

                            if (sttTong == list.Count())
                            {
                                DataTong.lstGiaiDoanTTCP.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = dicTotalGiaiDoanTTCP[giaiDoan.ID.ToString()] });
                                DataTong.lstGiaiDoanKinhPhiDuocCap.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = dicTotalGiaiDoanKinhPhiDuocCap[giaiDoan.ID.ToString() + "USD"], valueVND = dicTotalGiaiDoanKinhPhiDuocCap[giaiDoan.ID.ToString() + "VND"] });
                                DataTong.lstGiaiDoanKinhPhiDaGiaiNgan.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = dicTotalGiaiDoanKinhPhiDaGiaiNgan[giaiDoan.ID.ToString() + "USD"], valueVND = dicTotalGiaiDoanKinhPhiDaGiaiNgan[giaiDoan.ID.ToString() + "VND"] });
                            }
                        }
                    }
                    if (item.IDNhiemVuChi != idChuongTrinh && item.IDNhiemVuChi != Guid.Empty)
                    {
                        SttChuongTrinh++;
                        SttDuAn = 0;
                        SttLoai = 0;
                        SttDuAn = 0;
                        idDuAn = null;
                        idLoai = null;
                        idHopDong = null;
                        CPNHThucHienNganSach_Model DataCha = new CPNHThucHienNganSach_Model();
                        DataCha.sTenNoiDungChi = item.sTenNhiemVuChi;
                        DataCha.depth = convertLetter(SttChuongTrinh);
                        DataCha.isTitle = "font-bold-red";
                        idChuongTrinh = item.IDNhiemVuChi;
                        DataCha.lstGiaiDoanTTCP = new List<ThucHienNganSach_GiaiDoan_Model>();
                        DataCha.lstGiaiDoanKinhPhiDuocCap = new List<ThucHienNganSach_GiaiDoan_Model>();
                        DataCha.lstGiaiDoanKinhPhiDaGiaiNgan = new List<ThucHienNganSach_GiaiDoan_Model>();
                        if (lstGiaiDoan != null)
                        {
                            foreach (var giaiDoan in lstGiaiDoan)
                            {
                                DataCha.lstGiaiDoanTTCP.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = 0 });
                                DataCha.lstGiaiDoanKinhPhiDuocCap.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = 0, valueVND = 0 });
                                DataCha.lstGiaiDoanKinhPhiDaGiaiNgan.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = 0, valueVND = 0 });
                            }
                        }
                           
                        listData.Add(DataCha);
                    }
                    if (item.IDDuAn != idDuAn && item.IDDuAn != Guid.Empty)
                    {
                        SttDuAn++;
                        SttLoai = 0;
                        SttHopDong = 0;
                        idLoai = null;
                        idHopDong = null;
                        CPNHThucHienNganSach_Model DataCha = new CPNHThucHienNganSach_Model();
                        DataCha.sTenNoiDungChi = item.sTenDuAn;
                        DataCha.DuAnUSD = item.DuAnUSD;
                        DataCha.DuAnVND = item.DuAnVND;
                        DataTong.DuAnUSD = DataTong.DuAnUSD + item.DuAnUSD;
                        DataTong.DuAnVND = DataTong.DuAnVND + item.DuAnVND;
                        DataCha.isTitle = "font-bold";
                        DataCha.isDuAn = true;
                        DataCha.depth = _cpnhService.GetSTTLAMA(SttDuAn);
                        idDuAn = item.IDDuAn;
                        DataCha.lstGiaiDoanTTCP = new List<ThucHienNganSach_GiaiDoan_Model>();
                        DataCha.lstGiaiDoanKinhPhiDuocCap = new List<ThucHienNganSach_GiaiDoan_Model>();
                        DataCha.lstGiaiDoanKinhPhiDaGiaiNgan = new List<ThucHienNganSach_GiaiDoan_Model>();
                        if (lstGiaiDoan != null)
                        {
                            foreach (var giaiDoan in lstGiaiDoan)
                            {
                                DataCha.lstGiaiDoanTTCP.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = 0 });
                                DataCha.lstGiaiDoanKinhPhiDuocCap.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = 0, valueVND = 0 });
                                DataCha.lstGiaiDoanKinhPhiDaGiaiNgan.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = 0, valueVND = 0 });
                            }
                        }
                        listData.Add(DataCha);
                    }
                    if (item.iLoaiNoiDungChi != idLoai && item.iLoaiNoiDungChi != 0)
                    {
                        SttLoai++;
                        SttHopDong = 0;
                        idHopDong = null;
                        CPNHThucHienNganSach_Model DataCha = new CPNHThucHienNganSach_Model();
                        if (item.iLoaiNoiDungChi == 1)
                        {
                            DataCha.sTenNoiDungChi = "Chi ngoại tệ";
                        }
                        else if (item.iLoaiNoiDungChi == 2)
                        {
                            DataCha.sTenNoiDungChi = "Chi trong nước";
                        }
                        else
                        {
                            DataCha.sTenNoiDungChi = "Chi khác";
                        }
                        DataCha.depth = SttLoai.ToString();
                        DataCha.isTitle = "font-bold";
                        idLoai = item.iLoaiNoiDungChi;
                        DataCha.lstGiaiDoanTTCP = new List<ThucHienNganSach_GiaiDoan_Model>();
                        DataCha.lstGiaiDoanKinhPhiDuocCap = new List<ThucHienNganSach_GiaiDoan_Model>();
                        DataCha.lstGiaiDoanKinhPhiDaGiaiNgan = new List<ThucHienNganSach_GiaiDoan_Model>();
                        if (lstGiaiDoan != null)
                        {
                            foreach (var giaiDoan in lstGiaiDoan)
                            {
                                DataCha.lstGiaiDoanTTCP.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = 0 });
                                DataCha.lstGiaiDoanKinhPhiDuocCap.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = 0, valueVND = 0 });
                                DataCha.lstGiaiDoanKinhPhiDaGiaiNgan.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = 0, valueVND = 0 });
                            }
                        }
                        listData.Add(DataCha);
                    }
                    if (item.IDHopDong != idHopDong && item.IDHopDong != Guid.Empty)
                    {
                        SttHopDong++;
                        CPNHThucHienNganSach_Model DataCha = new CPNHThucHienNganSach_Model();
                        DataCha.sTenNoiDungChi = item.sTenHopDong;
                        DataCha.HopDongUSD = item.HopDongUSD;
                        DataCha.HopDongVND = item.HopDongVND;
                        DataTong.DuAnUSD = DataTong.DuAnUSD + item.HopDongUSD;
                        DataTong.DuAnVND = DataTong.DuAnVND + item.HopDongVND;
                        DataCha.isHopDong = true;
                        DataCha.depth = SttLoai.ToString() + "." + SttHopDong.ToString();
                        idHopDong = item.IDHopDong;
                        DataCha.lstGiaiDoanTTCP = new List<ThucHienNganSach_GiaiDoan_Model>();
                        DataCha.lstGiaiDoanKinhPhiDuocCap = new List<ThucHienNganSach_GiaiDoan_Model>();
                        DataCha.lstGiaiDoanKinhPhiDaGiaiNgan = new List<ThucHienNganSach_GiaiDoan_Model>();
                        if (lstGiaiDoan != null)
                        {
                            foreach (var giaiDoan in lstGiaiDoan)
                            {
                                DataCha.lstGiaiDoanTTCP.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = 0 });
                                DataCha.lstGiaiDoanKinhPhiDuocCap.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = 0, valueVND = 0 });
                                DataCha.lstGiaiDoanKinhPhiDaGiaiNgan.Add(new ThucHienNganSach_GiaiDoan_Model() { valueUSD = 0, valueVND = 0 });
                            }
                        }
                        listData.Add(DataCha);
                    }
                    item.HopDongUSD = null;
                    item.HopDongVND = null;
                    item.DuAnUSD = null;
                    item.DuAnVND = null;
                    listData.Add(item);

                    DataTong.NCVTTCP = DataTong.NCVTTCP + item.NCVTTCP;
                    DataTong.NhiemVuChi = DataTong.NhiemVuChi + item.NhiemVuChi;
                    DataTong.SumTongKinhPhiUSD = DataTong.SumTongKinhPhiUSD + item.TongKinhPhiUSD;
                    DataTong.SumTongKinhPhiVND = DataTong.SumTongKinhPhiVND + item.TongKinhPhiVND;
                    DataTong.KinhPhiUSD = DataTong.KinhPhiUSD + item.KinhPhiUSD;
                    DataTong.KinhPhiVND = DataTong.KinhPhiVND + item.KinhPhiVND;
                    DataTong.KinhPhiToYUSD = DataTong.KinhPhiToYUSD + item.KinhPhiToYUSD;
                    DataTong.KinhPhiToYVND = DataTong.KinhPhiToYVND + item.KinhPhiToYVND;
                    DataTong.SumTongKinhPhiDaChiUSD = DataTong.SumTongKinhPhiDaChiUSD + item.KinhPhiDaChiUSD;
                    DataTong.SumTongKinhPhiDaChiVND = DataTong.SumTongKinhPhiDaChiVND + item.TongKinhPhiDaChiVND;
                    DataTong.KinhPhiDaChiUSD = DataTong.KinhPhiDaChiUSD + item.KinhPhiDaChiUSD;
                    DataTong.KinhPhiDaChiVND = DataTong.KinhPhiDaChiVND + item.KinhPhiDaChiVND;
                    DataTong.KinhPhiDaChiToYUSD = DataTong.KinhPhiDaChiToYUSD + item.KinhPhiDaChiToYUSD;
                    DataTong.KinhPhiDaChiToYVND = DataTong.KinhPhiDaChiToYVND + item.KinhPhiDaChiToYVND;

                    DataTong.fLuyKeKinhPhiDuocCap_USD = DataTong.fLuyKeKinhPhiDuocCap_USD + item.fLuyKeKinhPhiDuocCap_USD;
                    DataTong.fLuyKeKinhPhiDuocCap_VND = DataTong.fLuyKeKinhPhiDuocCap_VND + item.fLuyKeKinhPhiDuocCap_VND;
                    DataTong.fDeNghiQTNamNay_USD = DataTong.fDeNghiQTNamNay_USD + item.fDeNghiQTNamNay_USD;
                    DataTong.fDeNghiQTNamNay_VND = DataTong.fDeNghiQTNamNay_VND + item.fDeNghiQTNamNay_VND;

                    if (sttTong == list.Count())
                    {
                        DataTong.SumKinhPhiChuaQuyetToanUSD = DataTong.fLuyKeKinhPhiDuocCap_USD - DataTong.fDeNghiQTNamNay_USD;
                        DataTong.SumKinhPhiChuaQuyetToanVND = DataTong.fLuyKeKinhPhiDuocCap_VND - DataTong.fDeNghiQTNamNay_VND;
                        DataTong.SumKeHoachGiaiNgan = DataTong.NCVTTCP - DataTong.fLuyKeKinhPhiDuocCap_USD;
                        DataTong.isDuAn = true;
                        DataTong.isTitle = "font-bold";
                        DataTong.isSum = true;
                        listData.Add(DataTong);
                    }
                }
            }
            
            return listData;
        } 
        public List<Dropdown_ByYear_ThucHienNganSach> GetListNamKeHoach()
        {
            List<Dropdown_ByYear_ThucHienNganSach> listNam = new List<Dropdown_ByYear_ThucHienNganSach>();
            int namHienTai = DateTime.Now.Year + 1;
            for (int i = 20; i > 0; i--)
            {
                namHienTai -= 1;
                Dropdown_ByYear_ThucHienNganSach namKeHoachOpt = new Dropdown_ByYear_ThucHienNganSach()
                {
                    Value = namHienTai,
                    Text = "Năm " + namHienTai.ToString()
                };
                listNam.Add(namKeHoachOpt);
            }
            return listNam;
        }
    }
}