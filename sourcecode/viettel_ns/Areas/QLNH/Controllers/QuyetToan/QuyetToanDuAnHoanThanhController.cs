using FlexCel.Core;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Viettel.Domain.DomainModel;
using Viettel.Models.QLNH.QuyetToan;
using Viettel.Services;
using VIETTEL.Controllers;
using VIETTEL.Flexcel;
using VIETTEL.Helpers;
using static Viettel.Services.IQLNHService;

namespace VIETTEL.Areas.QLNH.Controllers.QuyetToan
{
    public class QuyetToanDuAnHoanThanhController : FlexcelReportController
    {
        // GET: QLNH/QuyetToanDuAnHoanThanh
        private readonly IQLNHService _qlnhService = QLNHService.Default;
        private readonly INganSachService _nganSachService = NganSachService.Default;
        private const string sFilePathBaoCao = "/Report_ExcelFrom/QLNH/rpt_QuyetToanDuAnHoanThanh.xlsx";
        private const string sControlName = "QuyetToanDuAnHoanThanh";
        public List<Dropdown_QuyetToanDAHT> lstDonViVND = new List<Dropdown_QuyetToanDAHT>()
            {
                new Dropdown_QuyetToanDAHT()
                {
                    Value = 1,
                    Label = "Đồng"
                },
                 new Dropdown_QuyetToanDAHT()
                {
                    Value = 1000,
                    Label = "Nghìn đồng"
                }, new Dropdown_QuyetToanDAHT()
                {
                    Value = 1000000000,
                    Label = "Tỉ đồng"
                }
            };
        public List<Dropdown_QuyetToanDAHT> lstDonViUSD = new List<Dropdown_QuyetToanDAHT>()
            {
                new Dropdown_QuyetToanDAHT()
                {
                    Value = 1,
                    Label = "USD"
                },
                 new Dropdown_QuyetToanDAHT()
                {
                    Value = 1000,
                    Label = "Nghìn USD"
                }, new Dropdown_QuyetToanDAHT()
                {
                    Value = 1000000000,
                    Label = "Tỉ USD"
                }
            };
        public ActionResult Index()
        {
            QuyetToan_QuyetToanDuAnModel vm = new QuyetToan_QuyetToanDuAnModel();
            vm._paging.CurrentPage = 1;
            vm.Items = _qlnhService.GetListQuyetToanDuAnHT(ref vm._paging, null, null, null, null, null,0);
            List<NS_DonVi> lstDonViQL = _qlnhService.GetDonviList(PhienLamViec.NamLamViec).ToList();
            lstDonViQL.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sTen = "--Chọn đơn vị--" });
            ViewBag.ListDonVi = lstDonViQL;

            List<Dropdown_QuyetToanDAHT> lstNamBaoCaoTu = GetListNamKeHoach();
            lstNamBaoCaoTu.Insert(0, new Dropdown_QuyetToanDAHT { Value = 0, Label = "--Chọn năm--" });
            ViewBag.ListNamBaoCaoTu = lstNamBaoCaoTu;

            List<Dropdown_QuyetToanDAHT> lstNamBaoCaoDen = GetListNamKeHoach();
            lstNamBaoCaoDen.Insert(0, new Dropdown_QuyetToanDAHT { Value = 0, Label = "--Chọn năm--" });
            ViewBag.ListNamBaoCaoDen = lstNamBaoCaoDen;
            return View(vm);
        }

        [HttpPost]
        public ActionResult TimKiem(PagingInfo _paging, string sSoDeNghi, DateTime? dNgayDeNghi, Guid? iDonVi, int? iNamBaoCaoTu, int? iNamBaoCaoDen, int tabIndex)
        {
            QuyetToan_QuyetToanDuAnModel vm = new QuyetToan_QuyetToanDuAnModel();
            vm._paging = _paging;
            vm.Items = _qlnhService.GetListQuyetToanDuAnHT(ref vm._paging, sSoDeNghi
                , (dNgayDeNghi == null ? null : dNgayDeNghi), (iDonVi == Guid.Empty ? null : iDonVi)
                , (iNamBaoCaoTu == null ? null : iNamBaoCaoTu), (iNamBaoCaoDen == null ? null : iNamBaoCaoDen), tabIndex);

            List<NS_DonVi> lstDonViQL = _qlnhService.GetDonviList(PhienLamViec.NamLamViec).ToList();
            lstDonViQL.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sTen = "--Chọn đơn vị--" });
            ViewBag.ListDonVi = lstDonViQL;

            List<Dropdown_QuyetToanDAHT> lstNamBaoCaoTu = GetListNamKeHoach();
            lstNamBaoCaoTu.Insert(0, new Dropdown_QuyetToanDAHT { Value = 0, Label = "--Chọn năm--" });
            ViewBag.ListNamBaoCaoTu = lstNamBaoCaoTu;

            List<Dropdown_QuyetToanDAHT> lstNamBaoCaoDen = GetListNamKeHoach();
            lstNamBaoCaoDen.Insert(0, new Dropdown_QuyetToanDAHT { Value = 0, Label = "--Chọn năm--" });
            ViewBag.ListNamBaoCaoDen = lstNamBaoCaoDen;

            return PartialView("_list", vm);
        }

        [HttpPost]
        public ActionResult GetModal(Guid? id)
        {
            NH_QT_QuyetToanDAHT data = new NH_QT_QuyetToanDAHT();

            List<NS_DonVi> lstDonViQL = _qlnhService.GetDonviList(PhienLamViec.NamLamViec).ToList();
            lstDonViQL.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, iID_MaDonVi = "", sTen = "--Chọn đơn vị--" });

            List<NH_DM_TiGia> lstTiGia = _qlnhService.GetNHDMTiGiaList().ToList();
            lstTiGia.Insert(0, new NH_DM_TiGia { ID = Guid.Empty, sTenTiGia = "--Chọn đơn vị--" });
            ViewBag.ListDonVi = lstDonViQL;
            ViewBag.ListTiGia = lstTiGia;

            List<Dropdown_QuyetToanDAHT> lstNamBaoCaoTu = GetListNamKeHoach();
            lstNamBaoCaoTu.Insert(0, new Dropdown_QuyetToanDAHT { Value = 0, Label = "--Chọn năm--" });
            ViewBag.ListNamBaoCaoTu = lstNamBaoCaoTu;

            List<Dropdown_QuyetToanDAHT> lstNamBaoCaoDen = GetListNamKeHoach();
            lstNamBaoCaoDen.Insert(0, new Dropdown_QuyetToanDAHT { Value = 0, Label = "--Chọn năm--" });
            ViewBag.ListNamBaoCaoDen = lstNamBaoCaoDen;
            ViewBag.IsTongHop = false;
            if (id.HasValue)
            {
                data = _qlnhService.GetThongTinQuyetToanDuAnHTById(id.Value);
                ViewBag.IsTongHop = data.sTongHop != null ? true : false;
            }
            return PartialView("_modalUpdate", data);
        }

        [HttpPost]
        public JsonResult Save(NH_QT_QuyetToanDAHT data)
        {
            if (data == null)
            {
                return Json(new { bIsComplete = false, sMessError = "Không cập nhật được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            var returnData = _qlnhService.SaveQuyetToanDuAnHT(data, Username);
            if (!returnData.IsReturn)
            {
                return Json(new { bIsComplete = false, sMessError = "Không cập nhật được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = true, dataID = returnData.QuyetToanDuAnHTData.ID }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Xoa(string id)
        {
            if (!_qlnhService.DeleteQuyetToanDuAnHT(Guid.Parse(id)))
            {
                return Json(new { bIsComplete = false, sMessError = "Không xóa được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public bool LockDuAn(Guid id)
        {
            try
            {
                NH_QT_QuyetToanDAHT entity = _qlnhService.GetThongTinQuyetToanDuAnHTById(id);
                if (entity != null)
                {
                    return _qlnhService.LockOrUnLockQuyetToanDuAnHT(id, !entity.bIsKhoa);
                }
                return false;
            }
            catch (Exception ex)
            {
                AppLog.LogError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                return false;
            }
        }
        [HttpPost]
        public ActionResult GetModalTongHop(string[] listId, PagingInfo _paging, string[] listNamBaoCao)
        {
            QuyetToan_QuyetToanDuAnModel vm = new QuyetToan_QuyetToanDuAnModel();
            vm._paging = _paging;
            List<NH_QT_QuyetToanDAHTData> list = new List<NH_QT_QuyetToanDAHTData>();
            var stringId = "";
            foreach (var id in listId)
            {
                NH_QT_QuyetToanDAHTData data = _qlnhService.GetThongTinQuyetToanDuAnHTById(new Guid(id));
                List<NS_DonVi> lstDonViQL = _qlnhService.GetDonviList(PhienLamViec.NamLamViec).ToList();
                if (data.iID_DonViID != null)
                {
                    var donVi = lstDonViQL.Find(x => x.iID_Ma == data.iID_DonViID);
                    data.sTenDonVi += donVi.iID_MaDonVi + "-" + donVi.sTen;
                }
                stringId += id + ",";
                list.Add(data);
            }
            vm.Items = list;
            ViewBag.ListNamBaoCaoTu = listNamBaoCao.OrderBy(x => x).First();
            ViewBag.ListNamBaoCaoDen = listNamBaoCao.OrderBy(x => x).Last();

            ViewBag.ListId = stringId.Substring(0, stringId.Length - 1);
            return PartialView("_modelTongHop", vm);
        }
        [HttpPost]
        public ActionResult GetModalDetail(Guid? id)
        {
            NH_QT_QuyetToanDAHTData data = new NH_QT_QuyetToanDAHTData();
            if (id.HasValue)
            {
                data = _qlnhService.GetThongTinQuyetToanDuAnHTById(id.Value);
                if (data.iID_DonViID == null)
                {
                    foreach (var idTongHop in data.sTongHop.Split(','))
                    {
                        var tongHop = _qlnhService.GetThongTinQuyetToanDuAnHTById(new Guid(idTongHop));
                        var donvi = _nganSachService.GetDonViById(PhienLamViec.NamLamViec.ToString(), tongHop.iID_DonViID.ToString());
                        data.sTenDonVi += (donvi != null ? donvi.iID_MaDonVi + " - " + donvi.sTen : string.Empty) + " , ";
                    }
                }
                else
                {
                    var donvi = _nganSachService.GetDonViById(PhienLamViec.NamLamViec.ToString(), data.iID_DonViID.ToString());
                    data.sTenDonVi = (donvi != null ? donvi.iID_MaDonVi + " - " + donvi.sTen : string.Empty) + " , ";
                }
                data.sTenDonVi = data.sTenDonVi.Remove(data.sTenDonVi.Length - 2);
                var tiGia = _qlnhService.GetNHDMTiGiaList(data.iID_TiGiaID).FirstOrDefault();
                data.sTenTiGia = tiGia != null ? tiGia.sTenTiGia + " - " + tiGia.sTenTiGia : string.Empty;
            }

            return PartialView("_modalDetail", data);
        }

        public ActionResult ExportFile(string txtTieuDe1, string txtTieuDe2, string txtIDQuyetToan, int? slbDonViUSD, int? slbDonViVND, string ext = "xlsx", int to = 1)
        {
            var a = getQuyetToanDonVi(txtIDQuyetToan);
            string fileName = string.Format("{0}.{1}", "Quyet toan du an hoan thanh giai doan " + a[0].quyetToanDuAn.iNamBaoCaoTu + " - " + a[0].quyetToanDuAn.iNamBaoCaoDen, ext);
            ExcelFile xls = TaoFileExel(txtTieuDe1, txtTieuDe2, a, slbDonViUSD, slbDonViVND, to);
            return Print(xls, ext, fileName);
        }
        public ExcelFile TaoFileExel(string txtTieuDe1, string txtTieuDe2, List<NH_QT_QuyetToanDuAnByDonVi> quyetToanNienDoDetail, int? slbDonViUSD, int? slbDonViVND, int to = 1)
        {
            var donViVND = lstDonViVND.Find(x => x.Value == slbDonViVND);
            var donViUSD = lstDonViUSD.Find(x => x.Value == slbDonViUSD);
            List<NH_QT_QuyetToanDAHT_ChiTietData> data = new List<NH_QT_QuyetToanDAHT_ChiTietData>();
            data = getListDetailChiTiet(quyetToanNienDoDetail, true, donViUSD.Value, donViVND.Value);
            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(sFilePathBaoCao));
            FlexCelReport fr = new FlexCelReport();
            var giaiDoans = data.Select(x => new
            {
                x.iNamBaoCaoTu,
                x.iNamBaoCaoDen
            }).Where(x => x.iNamBaoCaoTu != null && x.iNamBaoCaoDen != null).OrderBy(x => x.iNamBaoCaoTu).Distinct().ToList();
            foreach (var item in data)
            {
                item.listDataTTCP = new List<NH_QT_QuyetToanDuAnDataGiaiDoan>();
                item.listDataKPDC = new List<NH_QT_QuyetToanDuAnDataGiaiDoan>();
                item.listDataQTDD = new List<NH_QT_QuyetToanDuAnDataGiaiDoan>();


                foreach (var giaiDoan in giaiDoans)
                {
                    if (item.iNamBaoCaoTu == giaiDoan.iNamBaoCaoTu)
                    {
                        item.listDataTTCP.Add(new NH_QT_QuyetToanDuAnDataGiaiDoan() { value = item.fKeHoach_TTCP_USD });
                        item.listDataKPDC.Add(new NH_QT_QuyetToanDuAnDataGiaiDoan() { valueUSD = item.fKinhPhiDuocCap_Tong_USD, valueVND = item.fKinhPhiDuocCap_Tong_VND });
                        item.listDataQTDD.Add(new NH_QT_QuyetToanDuAnDataGiaiDoan() { valueUSD = item.fQuyetToanDuocDuyet_Tong_USD, valueVND = item.fQuyetToanDuocDuyet_Tong_VND });

                    }
                    else
                    {
                        item.listDataTTCP.Add(new NH_QT_QuyetToanDuAnDataGiaiDoan());
                        item.listDataKPDC.Add(new NH_QT_QuyetToanDuAnDataGiaiDoan());
                        item.listDataQTDD.Add(new NH_QT_QuyetToanDuAnDataGiaiDoan());
                    }
                }
            }
            fr.SetValue(new
            {
                To = to,
                txtTieuDe1 = txtTieuDe1,
                txtTieuDe2 = txtTieuDe2,
                donViUSD = donViUSD.Label,
                donViVND = donViVND.Label,

            });
            //fr.SetValue("iTongSoNgayDieuTri", iTongSoNgayDieuTri.ToString("##,#", CultureInfo.GetCultureInfo("vi-VN")));
            foreach (var item in data)
            {
                item.sTenNoiDungChi = (item.STT != null ? item.STT + ", " : " - ") + item.sTenNoiDungChi;
            }
            List<NH_QT_QuyetToanDuAnGiaiDoan> lstGiaiDoan = new List<NH_QT_QuyetToanDuAnGiaiDoan>();
            foreach (var giaiDoan in giaiDoans)
            {
                lstGiaiDoan.Add(new NH_QT_QuyetToanDuAnGiaiDoan()
                {
                    giaiDoan = "Giai đoạn " + giaiDoan.iNamBaoCaoTu.ToString() + " - " + giaiDoan.iNamBaoCaoDen.ToString()
                });

            }

            fr.AddTable<NH_QT_QuyetToanDAHT_ChiTietData>("dt", data);
            fr.AddTable<NH_QT_QuyetToanDuAnGiaiDoan>("lstGiaiDoan", lstGiaiDoan);
            fr.AddTable<NH_QT_QuyetToanDuAnGiaiDoan>("lstGiaiDoan2", lstGiaiDoan);
            fr.AddTable<NH_QT_QuyetToanDuAnGiaiDoan>("lstGiaiDoan3", lstGiaiDoan);
            fr.UseChuKy(Username)
              .UseChuKyForController(sControlName)
              .UseForm(this).Run(Result);

            //count merge cột
            var col1 = 5 + lstGiaiDoan.Count();
            var col2 = col1 + ((lstGiaiDoan.Count() + 1) * 2);
            Result.MergeCells(9, 5, 9, col1);
            Result.MergeCells(9, col1 + 1, 9, col1 + ((lstGiaiDoan.Count() + 1) * 2));
            Result.MergeCells(9, col2 + 1, 9, col2 + ((lstGiaiDoan.Count() + 1) * 2));
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
            Result.SetCellFormat(9, 2, Cell.Row, 13 + lstGiaiDoan.Count() * 5, b, ApplyFormat, false);

            return Result;
        }
        [HttpPost]
        public JsonResult GetListDonvi()
        {
            var result = new List<dynamic>();
            var listModel = _qlnhService.GetDonviListByYear(PhienLamViec.NamLamViec).ToList();
            if (listModel != null && listModel.Any())
            {
                foreach (var item in listModel)
                {
                    result.Add(new
                    {
                        id = item.iID_Ma,
                        text = item.iID_MaDonVi + " - " + item.sTen
                    });
                }
            }
            return Json(new { status = true, data = result });
        }
        [HttpPost]
        public JsonResult GetListDropDownNamBaoCao()
        {
            var result = new List<dynamic>();
            var listModel = GetListNamKeHoach();
            if (listModel != null && listModel.Any())
            {
                foreach (var item in listModel)
                {
                    result.Add(new
                    {
                        id = item.Value,
                        text = item.Label
                    });
                }
            }
            return Json(new { status = true, data = result });
        }
        public ActionResult Detail(string id, bool edit)
        {
            NH_QT_QuyetToanDAHT_ChiTietView vm = new NH_QT_QuyetToanDAHT_ChiTietView();
            var quyetToanDonVi = getQuyetToanDonVi(id);
            vm.QuyetToanDAHTDetail = quyetToanDonVi[0].quyetToanDuAn;
            var listResult = getListDetailChiTiet(quyetToanDonVi, false, null, null);
            vm.ListDetailQuyetToanDAHT = listResult;
            ViewBag.IsEdit = edit;
            ViewBag.IdQuyetToan = id;

            return View(vm);
        }
        [HttpPost]
        public ActionResult GetListTongHopQuyetToan(string sSoDeNghi, DateTime? dNgayDeNghi, Guid? iDonVi, int? iNamBaoCaoTu,int? iNamBaoCaoDen)
        {
            var ListTongHopQuyetToan = _qlnhService.GetListTongHopQuyetToanDAHT(sSoDeNghi, dNgayDeNghi, iDonVi, iNamBaoCaoTu,iNamBaoCaoDen);
            ViewBag.TabIndex = 1;

            return Json(new { data = ListTongHopQuyetToan }, JsonRequestBehavior.AllowGet);
        }
        public List<NH_QT_QuyetToanDuAnByDonVi> getQuyetToanDonVi(string id)
        {
            var quyetToanDuAnDetail = _qlnhService.GetThongTinQuyetToanDuAnHTById(new Guid(id));
            List<NH_QT_QuyetToanDuAnByDonVi> qtdaDonVi = new List<NH_QT_QuyetToanDuAnByDonVi>();
            if (quyetToanDuAnDetail.iID_DonViID != null)
            {
                var donVi = _qlnhService.GetDonviList(PhienLamViec.NamLamViec).Where(x => x.iID_Ma == quyetToanDuAnDetail.iID_DonViID).FirstOrDefault();
                quyetToanDuAnDetail.sTenDonVi = donVi.iID_MaDonVi + " - " + donVi.sTen;
                qtdaDonVi.Add(new NH_QT_QuyetToanDuAnByDonVi()
                {
                    donVi = donVi,
                    quyetToanDuAn = quyetToanDuAnDetail
                });
            }
            else
            {
                foreach (var item in quyetToanDuAnDetail.sTongHop.Split(','))
                {
                    var qtdaDetail = _qlnhService.GetThongTinQuyetToanDuAnHTById(new Guid(item));
                    var donVi = _qlnhService.GetDonviList(PhienLamViec.NamLamViec).Where(x => x.iID_Ma == qtdaDetail.iID_DonViID).FirstOrDefault();
                    quyetToanDuAnDetail.sTenDonVi += donVi.iID_MaDonVi + " - " + donVi.sTen + ",";
                    qtdaDonVi.Add(new NH_QT_QuyetToanDuAnByDonVi()
                    {
                        donVi = donVi,
                        quyetToanDuAn = qtdaDetail
                    });
                }
            }
            return qtdaDonVi;
        }
        public List<NH_QT_QuyetToanDAHT_ChiTietData> getListDetailChiTiet(List<NH_QT_QuyetToanDuAnByDonVi> quyetToanDuAnDetail, bool isPrint, int? donViUSD, int? donViVND)
        {
            var listData = new List<NH_QT_QuyetToanDAHT_ChiTietData>();
            var listResult = new List<NH_QT_QuyetToanDAHT_ChiTietData>();

            foreach (var qtdv in quyetToanDuAnDetail)
            {
                var listQuyetToanDetail = _qlnhService.GetDetailQuyetToanDuAnDetail(qtdv.quyetToanDuAn.iNamBaoCaoTu, qtdv.quyetToanDuAn.iNamBaoCaoDen, qtdv.donVi.iID_Ma, qtdv.quyetToanDuAn.ID).ToList();
                if (listQuyetToanDetail.Any())
                {
                    listData = listQuyetToanDetail;
                }
                else
                {
                    listData = _qlnhService.GetDetailQuyetToanDuAnCreate(qtdv.quyetToanDuAn.iNamBaoCaoTu, qtdv.quyetToanDuAn.iNamBaoCaoDen, qtdv.donVi.iID_Ma, isPrint ? donViUSD != null ? donViUSD : donViVND : null).ToList();
                }

                var giaiDoans = listData.Select(x => new
                {
                    x.iNamBaoCaoTu,
                    x.iNamBaoCaoDen
                }).Distinct().ToList();
                ViewBag.ListGiaiDoan = giaiDoans.Select(x => new NH_QT_QuyetToanDuAnGiaiDoan
                {
                    giaiDoan = "Giai đoạn " + x.iNamBaoCaoTu.ToString() + " - " + x.iNamBaoCaoDen.ToString(),
                    key = x.iNamBaoCaoTu ?? 0
                }).ToList();
                ViewBag.CountGiaiDoan = giaiDoans.Count();

                if (quyetToanDuAnDetail.Count() > 1)
                {
                    var newObjDonVi = new NH_QT_QuyetToanDAHT_ChiTietData()
                    {
                        sTenNoiDungChi = qtdv.donVi.iID_MaDonVi + '-' + qtdv.donVi.sTen,
                        bIsTittle = true
                    };
                    listResult.Add(newObjDonVi);
                }
                var getAllChuongTrinh = listData.Where(x => x.iID_DonVi == qtdv.donVi.iID_Ma).Select(x => new { x.sTenNhiemVuChi, x.iID_KHCTBQP_NhiemVuChiID }).Distinct().ToList();

                var iCountChuongTrinh = 0;
                foreach (var chuongTrinh in getAllChuongTrinh)
                {
                    iCountChuongTrinh++;
                    var newObj = new NH_QT_QuyetToanDAHT_ChiTietData()
                    {
                        STT = convertLetter(iCountChuongTrinh),
                        sTenNoiDungChi = chuongTrinh.sTenNhiemVuChi,
                        bIsTittle = true
                    };
                    listResult.Add(newObj);
                    var getListDuAn = listData.Where(x => x.iID_KHCTBQP_NhiemVuChiID == chuongTrinh.iID_KHCTBQP_NhiemVuChiID && x.iID_DuAnID != null).ToList();
                    var getListHopDong = listData.Where(x => x.iID_KHCTBQP_NhiemVuChiID == chuongTrinh.iID_KHCTBQP_NhiemVuChiID && x.iID_DuAnID == null && x.iID_HopDongID != null).ToList();
                    var getListNone = listData.Where(x => x.iID_KHCTBQP_NhiemVuChiID == chuongTrinh.iID_KHCTBQP_NhiemVuChiID && x.iID_DuAnID == null && x.iID_HopDongID == null).ToList();

                    var iCountDuAn = 0;

                    if (getListDuAn.Any())
                    {
                        var getNameDuAn = getListDuAn.Select(x => new { x.sTenDuAn, x.iID_DuAnID, x.fHopDong_VND_DuAn, x.fHopDong_USD_DuAn })
                        .Distinct()
                        .ToList();
                        foreach (var hopDongDuAn in getNameDuAn)
                        {
                            iCountDuAn++;
                            var newObjHopDongDuAn = new NH_QT_QuyetToanDAHT_ChiTietData()
                            {
                                STT = convertLaMa(Decimal.Parse(iCountDuAn.ToString())),
                                sTenNoiDungChi = hopDongDuAn.sTenDuAn,
                                bIsTittle = true,
                                fHopDong_VND = hopDongDuAn.fHopDong_VND_DuAn,
                                fHopDong_USD = hopDongDuAn.fHopDong_USD_DuAn,
                                sumTTCP = getListDuAn.Sum(x => x.fKeHoach_TTCP_USD),
                                sumKPDCUSD = getListDuAn.Sum(x => x.fKinhPhiDuocCap_Tong_USD),
                                sumKPDCVND = getListDuAn.Sum(x => x.fKinhPhiDuocCap_Tong_VND),
                                sumQTDDUSD = getListDuAn.Sum(x => x.fQuyetToanDuocDuyet_Tong_USD),
                                sumQTDDVND = getListDuAn.Sum(x => x.fQuyetToanDuocDuyet_Tong_VND)
                            };
                            listResult.Add(newObjHopDongDuAn);
                            listResult.AddRange(returnLoaiChi(iCountDuAn.ToString(), true, getListDuAn.Where(x => x.iID_DuAnID == hopDongDuAn.iID_DuAnID).ToList()));
                        }
                    }
                    if (getListHopDong.Any())
                    {
                        iCountDuAn++;
                        var newObjHopDong = new NH_QT_QuyetToanDAHT_ChiTietData()
                        {
                            STT = convertLaMa(Decimal.Parse(iCountDuAn.ToString())),
                            sTenNoiDungChi = "Chi hợp đồng",
                            bIsTittle = true,
                        };
                        listResult.Add(newObjHopDong);
                        listResult.AddRange(returnLoaiChi(iCountChuongTrinh.ToString(), true, getListHopDong));
                        //
                    }
                    if (getListNone.Any())
                    {
                        iCountDuAn++;
                        var newObjKhac = new NH_QT_QuyetToanDAHT_ChiTietData()
                        {
                            STT = convertLaMa(Decimal.Parse(iCountDuAn.ToString())),
                            sTenNoiDungChi = "Chi khác",
                            bIsTittle = true,
                            sumTTCP = getListNone.Sum(x => x.fKeHoach_TTCP_USD),
                            sumKPDCUSD = getListNone.Sum(x => x.fKinhPhiDuocCap_Tong_USD),
                            sumKPDCVND = getListNone.Sum(x => x.fKinhPhiDuocCap_Tong_VND),
                            sumQTDDUSD = getListNone.Sum(x => x.fQuyetToanDuocDuyet_Tong_USD),
                            sumQTDDVND = getListNone.Sum(x => x.fQuyetToanDuocDuyet_Tong_VND)
                        };
                        listResult.Add(newObjKhac);
                        listResult.AddRange(returnLoaiChi("1", false, getListNone));
                    }
                }
            }
            return listResult;
        }
        public List<NH_QT_QuyetToanDAHT_ChiTietData> returnLoaiChi(string stt, bool idDuAn, List<NH_QT_QuyetToanDAHT_ChiTietData> list)
        {
            List<NH_QT_QuyetToanDAHT_ChiTietData> returnData = new List<NH_QT_QuyetToanDAHT_ChiTietData>();
            var listLoaiChiPhi = list.Select(x => new { x.iLoaiNoiDungChi }).Distinct().OrderBy(x => x.iLoaiNoiDungChi)
                  .ToList();
            var countLoaiChiPhi = 0;
            foreach (var loaiChiPhi in listLoaiChiPhi)
            {
                countLoaiChiPhi++;
                var newObjLoaiChiPhi = new NH_QT_QuyetToanDAHT_ChiTietData()
                {
                    STT = countLoaiChiPhi.ToString(),
                    sTenNoiDungChi = loaiChiPhi.iLoaiNoiDungChi == 1 ? "Chi ngoại tệ" : "Chi trong nước",
                    bIsTittle = true
                };
                returnData.Add(newObjLoaiChiPhi);

                if (idDuAn)
                {
                    var listNameHopDong = list.Where(x => x.iLoaiNoiDungChi == loaiChiPhi.iLoaiNoiDungChi).Select(x => new { x.sTenHopDong, x.iID_HopDongID, x.fHopDong_VND_HopDong, x.fHopDong_USD_HopDong }).Distinct()
                    .ToList();
                    var countHopDong = 0;
                    foreach (var nameHopDong in listNameHopDong)
                    {
                        countHopDong++;
                        var listHopDong = list.Where(x => x.iLoaiNoiDungChi == loaiChiPhi.iLoaiNoiDungChi && x.iID_HopDongID == nameHopDong.iID_HopDongID)
                            .ToList();
                        var newObjHopDongDuAn = new NH_QT_QuyetToanDAHT_ChiTietData()
                        {
                            STT = countLoaiChiPhi.ToString() + "." + countHopDong.ToString(),
                            sTenNoiDungChi = nameHopDong.sTenHopDong,
                            bIsTittle = null,
                            fHopDong_VND = nameHopDong.fHopDong_VND_HopDong,
                            fHopDong_USD = nameHopDong.fHopDong_USD_HopDong,
                            sumTTCP = listHopDong.Sum(x => x.fKeHoach_TTCP_USD),
                            sumKPDCUSD = listHopDong.Sum(x => x.fKinhPhiDuocCap_Tong_USD),
                            sumKPDCVND = listHopDong.Sum(x => x.fKinhPhiDuocCap_Tong_VND),
                            sumQTDDUSD = listHopDong.Sum(x => x.fQuyetToanDuocDuyet_Tong_USD),
                            sumQTDDVND = listHopDong.Sum(x => x.fQuyetToanDuocDuyet_Tong_VND)
                        };
                        returnData.Add(newObjHopDongDuAn);
                        returnData.AddRange(listHopDong);
                    }
                }
                else
                {
                    var listHopDong = list.Where(x => x.iLoaiNoiDungChi == loaiChiPhi.iLoaiNoiDungChi).ToList();
                    returnData.AddRange(listHopDong);
                }

            }
            return returnData;
        }
        [HttpPost]
        public JsonResult SaveDetail(List<NH_QT_QuyetToanDAHT_ChiTiet> data)
        {
            if (data == null)
            {
                return Json(new { bIsComplete = false, sMessError = "Không cập nhật được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            var returnData = _qlnhService.SaveQuyetToanDuAnDetail(data, Username);
            if (!returnData.IsReturn)
            {
                return Json(new { bIsComplete = false, sMessError = "Không cập nhật được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetModalInBaoCao(string[] listId)
        {

            List<NS_DonVi> lstDonViQL = _qlnhService.GetDonviList(PhienLamViec.NamLamViec).ToList();
            lstDonViQL.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, iID_MaDonVi = "", sTen = "--Chọn đơn vị--" });
            ViewBag.ListDonVi = lstDonViQL;


            lstDonViVND.Insert(0, new Dropdown_QuyetToanDAHT { Value = 0, Label = "--Chọn đơn vị VND--" });
            ViewBag.ListDVVND = lstDonViVND;


            lstDonViUSD.Insert(0, new Dropdown_QuyetToanDAHT { Value = 0, Label = "--Chọn đơn vị USD--" });
            ViewBag.ListDVUSD = lstDonViUSD;


            ViewBag.IDQuyetToan = listId[0];
            return PartialView("_modalInBaoCao");
        }
        [HttpPost]
        public JsonResult SaveTongHop(NH_QT_QuyetToanDAHT data, string listId)
        {
            if (data == null)
            {
                return Json(new { bIsComplete = false, sMessError = "Không cập nhật được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            if (!_qlnhService.SaveTongHopQuyetToanDAHT(data, Username, listId))
            {
                return Json(new { bIsComplete = false, sMessError = "Không cập nhật được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }
        public List<Dropdown_QuyetToanDAHT> GetListNamKeHoach()
        {
            List<Dropdown_QuyetToanDAHT> listNam = new List<Dropdown_QuyetToanDAHT>();
            int namHienTai = DateTime.Now.Year + 1;
            for (int i = 20; i > 0; i--)
            {
                namHienTai -= 1;
                Dropdown_QuyetToanDAHT namKeHoachOpt = new Dropdown_QuyetToanDAHT()
                {
                    Value = namHienTai,
                    Label = namHienTai.ToString()
                };
                listNam.Add(namKeHoachOpt);
            }
            return listNam;
        }
        private string convertLetter(int input)
        {
            StringBuilder res = new StringBuilder((input - 1).ToString());
            for (int j = 0; j < res.Length; j++)
                res[j] += (char)(17); // '0' is 48, 'A' is 65
            return res.ToString();
        }
        private string convertLaMa(decimal num)
        {
            string strRet = string.Empty;
            decimal _Number = num;
            Boolean _Flag = true;
            string[] ArrLama = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
            int[] ArrNumber = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            int i = 0;
            while (_Flag)
            {
                while (_Number >= ArrNumber[i])
                {
                    _Number -= ArrNumber[i];
                    strRet += ArrLama[i];
                    if (_Number < 1)
                        _Flag = false;
                }
                i++;
            }
            return strRet;
        }
    }
}