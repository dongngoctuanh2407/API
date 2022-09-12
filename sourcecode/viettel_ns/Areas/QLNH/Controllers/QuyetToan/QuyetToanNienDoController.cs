using FlexCel.Core;
using FlexCel.Render;
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
using VIETTEL.Helpers;
using VIETTEL.Flexcel;

namespace VIETTEL.Areas.QLNH.Controllers.QuyetToan
{
    public class QuyetToanNienDoController : FlexcelReportController
    {
        private readonly IQLNHService _qlnhService = QLNHService.Default;
        private readonly INganSachService _nganSachService = NganSachService.Default;
        private const string sFilePathBaoCao = "/Report_ExcelFrom/QLNH/rpt_QuyetToanNienDo3.xlsx";
        private const string sControlName = "QuyetToanNienDo";
        public List<Dropdown_QuyetToanNienDo> lstDonViVND = new List<Dropdown_QuyetToanNienDo>()
            {
                new Dropdown_QuyetToanNienDo()
                {
                    Value = 1,
                    Label = "Đồng"
                },
                 new Dropdown_QuyetToanNienDo()
                {
                    Value = 1000,
                    Label = "Nghìn đồng"
                }, new Dropdown_QuyetToanNienDo()
                {
                    Value = 1000000000,
                    Label = "Tỉ đồng"
                }
            };
        public List<Dropdown_QuyetToanNienDo> lstDonViUSD = new List<Dropdown_QuyetToanNienDo>()
            {
                new Dropdown_QuyetToanNienDo()
                {
                    Value = 1,
                    Label = "USD"
                },
                 new Dropdown_QuyetToanNienDo()
                {
                    Value = 1000,
                    Label = "Nghìn USD"
                }, new Dropdown_QuyetToanNienDo()
                {
                    Value = 1000000000,
                    Label = "Tỉ USD"
                }
            };

        public List<Dropdown_QuyetToanNienDo> lstLoaiQuyetToan = new List<Dropdown_QuyetToanNienDo>()
            {
                new Dropdown_QuyetToanNienDo()
                {
                    Value = 1,
                    Label = "Quyết toán theo dự án"
                },
                 new Dropdown_QuyetToanNienDo()
                {
                    Value = 2,
                    Label = "Quyết toán theo hợp đồng"
                }
            };
        // GET: QLNH/QuyetToanNienDo
        public ActionResult Index()
        {
            QuyetToan_QuyetToanNienDoModel vm = new QuyetToan_QuyetToanNienDoModel();
            vm._paging.CurrentPage = 1;
            vm.Items = _qlnhService.GetListQuyetToanNienDo(ref vm._paging, null, null, null, null, 0);

            List<NS_DonVi> lstDonViQL = _qlnhService.GetDonviList(PhienLamViec.NamLamViec).ToList();
            lstDonViQL.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sTen = "--Chọn đơn vị--" });
            ViewBag.ListDonVi = lstDonViQL;

            List<Dropdown_QuyetToanNienDo> lstNamKeHoach = GetListNamKeHoach();
            lstNamKeHoach.Insert(0, new Dropdown_QuyetToanNienDo { Value = 0, Label = "--Chọn năm--" });
            ViewBag.ListNamKeHoach = lstNamKeHoach;

            return View(vm);
        }
        [HttpPost]
        public ActionResult GetListTongHopQuyetToan( string sSoDeNghi, DateTime? dNgayDeNghi, Guid? iDonVi, int? iNamKeHoach)
        {
            var ListTongHopQuyetToan = _qlnhService.GetListTongHopQuyetToanNienDo(sSoDeNghi, dNgayDeNghi, iDonVi, iNamKeHoach);
            ViewBag.TabIndex = 1;

            return Json(new { data = ListTongHopQuyetToan }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(string id, bool edit)
        {
            NH_QT_QuyetToanNienDo_ChiTietView vm = new NH_QT_QuyetToanNienDo_ChiTietView();
            var quyetToanDonVi = getQuyetToanDonVi(id);
            vm.QuyetToanNienDoDetail = quyetToanDonVi[0].quyetToanNienDo;

            var listResult = getListDetailChiTiet(quyetToanDonVi, false, null, null);
            vm.ListDetailQuyetToanNienDo = listResult;
            ViewBag.IsEdit = edit;
            ViewBag.IdQuyetToan = id;
            return View(vm);
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
        public JsonResult GetListDropDownNamKeHoach()
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
        public List<NH_QT_QuyetToanNienDoByDonVi> getQuyetToanDonVi(string id)
        {
            var quyetToanNienDoDetail = _qlnhService.GetThongTinQuyetToanNienDoById(new Guid(id));
            List<NH_QT_QuyetToanNienDoByDonVi> qtndDonVi = new List<NH_QT_QuyetToanNienDoByDonVi>();
            if (quyetToanNienDoDetail.iID_DonViID != null)
            {
                var donVi = _qlnhService.GetDonviList(PhienLamViec.NamLamViec).Where(x => x.iID_Ma == quyetToanNienDoDetail.iID_DonViID).FirstOrDefault();
                quyetToanNienDoDetail.sTenDonVi = donVi.iID_MaDonVi + " - " + donVi.sTen;
                qtndDonVi.Add(new NH_QT_QuyetToanNienDoByDonVi()
                {
                    donVi = donVi,
                    quyetToanNienDo = quyetToanNienDoDetail
                });
            }
            else
            {
                foreach (var item in quyetToanNienDoDetail.sTongHop.Split(','))
                {
                    var qtndDetail = _qlnhService.GetThongTinQuyetToanNienDoById(new Guid(item));
                    var donVi = _qlnhService.GetDonviList(PhienLamViec.NamLamViec).Where(x => x.iID_Ma == qtndDetail.iID_DonViID).FirstOrDefault();
                    quyetToanNienDoDetail.sTenDonVi += donVi.iID_MaDonVi + " - " + donVi.sTen + ",";
                    qtndDonVi.Add(new NH_QT_QuyetToanNienDoByDonVi()
                    {
                        donVi = donVi,
                        quyetToanNienDo = qtndDetail
                    });
                }
            }
            return qtndDonVi;
        }
        public List<NH_QT_QuyetToanNienDo_ChiTietData> getListDetailChiTiet(List<NH_QT_QuyetToanNienDoByDonVi> quyetToanNienDoDetail, bool isPrint, int? donViUSD, int? donViVND)
        {
            var listData = new List<NH_QT_QuyetToanNienDo_ChiTietData>();
            var listResult = new List<NH_QT_QuyetToanNienDo_ChiTietData>();


            foreach (var qtdv in quyetToanNienDoDetail)
            {
                var listDetail = _qlnhService.GetDetailQuyetToanNienDoDetail(qtdv.quyetToanNienDo.iNamKeHoach, qtdv.donVi.iID_Ma, qtdv.quyetToanNienDo.ID).ToList();
                //var listTitle = new List<NH_QT_QuyetToanNienDo_ChiTietData>();
                if (listDetail.Any())
                {
                    listData = listDetail;
                }
                else
                {
                    listData = _qlnhService.GetDetailQuyetToanNienDoCreate(qtdv.quyetToanNienDo.iNamKeHoach, qtdv.donVi.iID_Ma, isPrint ? donViUSD != null ? donViUSD : donViVND : null).ToList();
                }
                var listTitle = listData.Where(x => x.iID_ParentID != null).ToList();
                if (quyetToanNienDoDetail.Count() > 1)
                {
                    var newObjDonVi = new NH_QT_QuyetToanNienDo_ChiTietData()
                    {
                        sTenNoiDungChi = qtdv.donVi.iID_MaDonVi + '-' + qtdv.donVi.sTen,
                        bIsTittle = true
                    };
                    listResult.Add(newObjDonVi);
                }
                var getAllChuongTrinh = listData.Where(x => x.iID_DonVi == qtdv.donVi.iID_Ma && x.iID_KHCTBQP_NhiemVuChiID != null).Select(x => new { x.sTenNhiemVuChi, x.iID_KHCTBQP_NhiemVuChiID }).Distinct().ToList();

                var iCountChuongTrinh = 0;
                var iCurrentID = 0;

                foreach (var chuongTrinh in getAllChuongTrinh)
                {
                    iCountChuongTrinh++;
                    iCurrentID = listResult.Where(x => x.iCurrentId != 0).Count() + 1;
                    var newObj = new NH_QT_QuyetToanNienDo_ChiTietData()
                    {
                        STT = convertLetter(iCountChuongTrinh),
                        sTenNoiDungChi = chuongTrinh.sTenNhiemVuChi,
                        bIsTittle = true,
                        iCurrentId = iCurrentID,
                        iParentId = 0
                    };
                    listResult.Add(newObj);
                    var getListDuAn = listData.Where(x => x.iID_KHCTBQP_NhiemVuChiID == chuongTrinh.iID_KHCTBQP_NhiemVuChiID && x.iID_DuAnID != null && x.iID_ParentID == null).ToList();
                    var getListHopDong = listData.Where(x => x.iID_KHCTBQP_NhiemVuChiID == chuongTrinh.iID_KHCTBQP_NhiemVuChiID && x.iID_DuAnID == null && x.iID_HopDongID != null && x.iID_ParentID == null).ToList();
                    var getListNone = qtdv.quyetToanNienDo.iLoaiQuyetToan == 1
                        ? listData.Where(x => x.iID_KHCTBQP_NhiemVuChiID == chuongTrinh.iID_KHCTBQP_NhiemVuChiID && x.iID_DuAnID == null && x.iID_HopDongID == null && x.iID_ParentID == null).ToList()
                        : new List<NH_QT_QuyetToanNienDo_ChiTietData>();
                    var iCountDuAn = 0;

                    if (getListDuAn.Any())
                    {
                        var getNameDuAn = getListDuAn.Select(x => new { x.sTenDuAn, x.iID_DuAnID, x.fHopDong_VND_DuAn, x.fHopDong_USD_DuAn })
                        .Distinct()
                        .ToList();
                        foreach (var hopDongDuAn in getNameDuAn)
                        {
                            iCountDuAn++;
                            iCurrentID = listResult.Where(x=>x.iCurrentId!=0).Count() + 1;
                            var newObjHopDongDuAn = new NH_QT_QuyetToanNienDo_ChiTietData();
                            var findTittle = listTitle.Find(x => x.iID_ParentID == hopDongDuAn.iID_DuAnID && x.iID_KHCTBQP_NhiemVuChiID == chuongTrinh.iID_KHCTBQP_NhiemVuChiID && x.iID_DuAnID == hopDongDuAn.iID_DuAnID);
                            if (findTittle != null)
                            {
                                newObjHopDongDuAn.MapFrom(findTittle);
                            }
                            newObjHopDongDuAn.STT = convertLaMa(Decimal.Parse(iCountDuAn.ToString()));
                            newObjHopDongDuAn.sTenNoiDungChi = hopDongDuAn.sTenDuAn;
                            newObjHopDongDuAn.bIsTittle = true;
                            newObjHopDongDuAn.fHopDong_VND = hopDongDuAn.fHopDong_VND_DuAn;
                            newObjHopDongDuAn.fHopDong_USD = hopDongDuAn.fHopDong_USD_DuAn;
                            newObjHopDongDuAn.bIsData = true;
                            newObjHopDongDuAn.sLevel = "1";
                            newObjHopDongDuAn.iCurrentId = iCurrentID;
                            newObjHopDongDuAn.iParentId = newObj.iCurrentId;
                            newObjHopDongDuAn.iID_ParentID = hopDongDuAn.iID_DuAnID;
                            newObjHopDongDuAn.iID_KHCTBQP_NhiemVuChiID = chuongTrinh.iID_KHCTBQP_NhiemVuChiID;
                            newObjHopDongDuAn.iID_DuAnID = hopDongDuAn.iID_DuAnID;

                            listResult.Add(newObjHopDongDuAn);
                            listResult.AddRange(returnLoaiChi(chuongTrinh.iID_KHCTBQP_NhiemVuChiID, hopDongDuAn.iID_DuAnID, iCurrentID, true, getListDuAn.Where(x => x.iID_DuAnID == hopDongDuAn.iID_DuAnID).ToList(), listTitle));

                        }
                    }
                    if (getListHopDong.Any())
                    {
                        iCountDuAn++;
                        iCurrentID = listResult.Where(x => x.iCurrentId != 0).Count() + 1;
                        var newObjHopDong = new NH_QT_QuyetToanNienDo_ChiTietData()
                        {
                            STT = convertLaMa(Decimal.Parse(iCountDuAn.ToString())),
                            sTenNoiDungChi = "Chi hợp đồng",
                            bIsTittle = true,
                            iCurrentId = iCurrentID,
                            iParentId = newObj.iCurrentId
                        };
                        listResult.Add(newObjHopDong);
                        listResult.AddRange(returnLoaiChi(chuongTrinh.iID_KHCTBQP_NhiemVuChiID, null, iCurrentID, true, getListHopDong, listTitle));
                        //
                    }
                    if (getListNone.Any())
                    {
                        iCountDuAn++;
                        iCurrentID =  listResult.Where(x => x.iCurrentId != 0).Count() + 1;
                        var newObjKhac = new NH_QT_QuyetToanNienDo_ChiTietData()
                        {
                            STT = convertLaMa(Decimal.Parse(iCountDuAn.ToString())),
                            sTenNoiDungChi = "Chi khác",
                            bIsTittle = true,
                            iCurrentId = iCurrentID,
                            iParentId = newObj.iCurrentId
                        };
                        listResult.Add(newObjKhac);
                        listResult.AddRange(returnLoaiChi(chuongTrinh.iID_KHCTBQP_NhiemVuChiID, null, iCurrentID, false, getListNone, listTitle));
                    }
                }
            }
            return listResult;
        }
        [HttpPost]
        public JsonResult SaveDetail(List<NH_QT_QuyetToanNienDo_ChiTiet> data)
        {
            if (data == null)
            {
                return Json(new { bIsComplete = false, sMessError = "Không cập nhật được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            var returnData = _qlnhService.SaveQuyetToanNienDoDetail(data, Username);
            if (!returnData.IsReturn)
            {
                return Json(new { bIsComplete = false, sMessError = "Không cập nhật được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }

        public List<NH_QT_QuyetToanNienDo_ChiTietData> returnLoaiChi(Guid? idChuongTrinh, Guid? idDuAn, int iCurrentID, bool isDuAn, List<NH_QT_QuyetToanNienDo_ChiTietData> list, List<NH_QT_QuyetToanNienDo_ChiTietData> listTittle)
        {
            List<NH_QT_QuyetToanNienDo_ChiTietData> returnData = new List<NH_QT_QuyetToanNienDo_ChiTietData>();
            var listLoaiChiPhi = list.Select(x => new { x.iLoaiNoiDungChi }).Distinct().OrderBy(x => x.iLoaiNoiDungChi)
                  .ToList();
            var countLoaiChiPhi = 0;
            var currentParent = iCurrentID;
            foreach (var loaiChiPhi in listLoaiChiPhi)
            {
                countLoaiChiPhi++;
                var newObjLoaiChiPhi = new NH_QT_QuyetToanNienDo_ChiTietData()
                {
                    STT = countLoaiChiPhi.ToString(),
                    sTenNoiDungChi = loaiChiPhi.iLoaiNoiDungChi == 1 ? "Chi ngoại tệ" : "Chi trong nước",
                    bIsTittle = true
                };
                returnData.Add(newObjLoaiChiPhi);

                if (isDuAn)
                {
                    var listNameHopDong = list.Where(x => x.iLoaiNoiDungChi == loaiChiPhi.iLoaiNoiDungChi).Select(x => new { x.sTenHopDong, x.iID_HopDongID, x.fHopDong_VND_HopDong, x.fHopDong_USD_HopDong }).Distinct()
                    .ToList();
                    var countHopDong = 0;
                    foreach (var nameHopDong in listNameHopDong)
                    {
                        countHopDong++;
                        iCurrentID++;
                        var newObjHopDongDuAn = new NH_QT_QuyetToanNienDo_ChiTietData();
                        var findTittle = listTittle.Find(x => x.iID_HopDongID == nameHopDong.iID_HopDongID && x.iID_KHCTBQP_NhiemVuChiID == idChuongTrinh && x.iID_DuAnID == idDuAn && x.iLoaiNoiDungChi == loaiChiPhi.iLoaiNoiDungChi);
                        if (findTittle != null)
                        {
                            newObjHopDongDuAn.MapFrom(findTittle);
                        }
                        newObjHopDongDuAn.STT = countLoaiChiPhi.ToString() + "." + countHopDong.ToString();
                        newObjHopDongDuAn.sTenNoiDungChi = nameHopDong.sTenHopDong;
                        newObjHopDongDuAn.fHopDong_VND = nameHopDong.fHopDong_VND_HopDong;
                        newObjHopDongDuAn.fHopDong_USD = nameHopDong.fHopDong_USD_HopDong;
                        newObjHopDongDuAn.bIsData = true;
                        newObjHopDongDuAn.bIsTittle = true;
                        newObjHopDongDuAn.sLevel = "2";
                        newObjHopDongDuAn.iCurrentId = iCurrentID;
                        newObjHopDongDuAn.iParentId = currentParent;
                        newObjHopDongDuAn.iID_ParentID = nameHopDong.iID_HopDongID;
                        newObjHopDongDuAn.iID_KHCTBQP_NhiemVuChiID = idChuongTrinh;
                        newObjHopDongDuAn.iID_HopDongID = nameHopDong.iID_HopDongID;
                        newObjHopDongDuAn.iID_DuAnID = idDuAn;

                        var listHopDong = list.Where(x => x.iLoaiNoiDungChi == loaiChiPhi.iLoaiNoiDungChi && x.iID_HopDongID == nameHopDong.iID_HopDongID).ToList();
                        newObjHopDongDuAn.iID_ThanhToan_ChiTietID = listHopDong.FirstOrDefault().iID_ThanhToan_ChiTietID;

                        returnData.Add(newObjHopDongDuAn);

                        listHopDong.ForEach(x =>
                        {
                            iCurrentID++;
                            x.bIsData = true;
                            x.sLevel = "3";
                            x.iCurrentId = iCurrentID;
                            x.iParentId = newObjHopDongDuAn.iCurrentId;
                        });
                        returnData.AddRange(listHopDong);
                    }
                }
                else
                {
                   
                    var listHopDong = list.Where(x => x.iLoaiNoiDungChi == loaiChiPhi.iLoaiNoiDungChi).ToList();
                    listHopDong.ForEach(x =>
                    {
                        x.iParentId = currentParent;
                        iCurrentID++;
                        x.bIsData = true;
                        x.sLevel = "3";
                        x.iCurrentId = iCurrentID;
                    });
                    returnData.AddRange(listHopDong);
                }

            }
            return returnData;
        }
        public ActionResult ExportFile(string txtTieuDe1, string txtTieuDe2, string txtIDQuyetToan, int? slbDonViUSD, int? slbDonViVND, string ext = "xlsx", int to = 1)
        {
            var a = getQuyetToanDonVi(txtIDQuyetToan);
            string fileName = string.Format("{0}.{1}", "Quyet toan nien do nam" + a[0].quyetToanNienDo.iNamKeHoach, ext);
            ExcelFile xls = TaoFileExel(txtTieuDe1, txtTieuDe2, a, slbDonViUSD, slbDonViVND, to);
            return Print(xls, ext, fileName);
        }
        public ExcelFile TaoFileExel(string txtTieuDe1, string txtTieuDe2, List<NH_QT_QuyetToanNienDoByDonVi> quyetToanNienDoDetail, int? slbDonViUSD, int? slbDonViVND, int to = 1)
        {
            var donViVND = lstDonViVND.Find(x => x.Value == slbDonViVND);
            var donViUSD = lstDonViUSD.Find(x => x.Value == slbDonViUSD);
            List<NH_QT_QuyetToanNienDo_ChiTietData> data = new List<NH_QT_QuyetToanNienDo_ChiTietData>();
            data = getListDetailChiTiet(quyetToanNienDoDetail, true, donViUSD.Value, donViVND.Value);
            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath(sFilePathBaoCao));
            FlexCelReport fr = new FlexCelReport();

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
                item.sTenNoiDungChi = (item.STT != null ? item.STT + "," : string.Empty) + item.sTenNoiDungChi;
            }
            fr.AddTable<NH_QT_QuyetToanNienDo_ChiTietData>("dt", data);

            fr.UseChuKy(Username)
              .UseChuKyForController(sControlName)
              .UseForm(this).Run(Result);

            return Result;
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

        [HttpPost]
        public ActionResult TimKiem(PagingInfo _paging, string sSoDeNghi, DateTime? dNgayDeNghi, Guid? iDonVi, int? iNamKeHoach, int tabIndex)
        {
            QuyetToan_QuyetToanNienDoModel vm = new QuyetToan_QuyetToanNienDoModel();
            vm._paging = _paging;
            vm.Items = _qlnhService.GetListQuyetToanNienDo(ref vm._paging, sSoDeNghi
                , (dNgayDeNghi == null ? null : dNgayDeNghi), (iDonVi == Guid.Empty ? null : iDonVi)
                , (iNamKeHoach == null ? null : iNamKeHoach), tabIndex);

            List<NS_DonVi> lstDonViQL = _qlnhService.GetDonviList(PhienLamViec.NamLamViec).ToList();
            lstDonViQL.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sTen = "--Chọn đơn vị--" });
            ViewBag.ListDonVi = lstDonViQL;

            List<Dropdown_QuyetToanNienDo> lstNamKeHoach = GetListNamKeHoach();
            lstNamKeHoach.Insert(0, new Dropdown_QuyetToanNienDo { Value = 0, Label = "--Chọn năm--" });
            ViewBag.ListNamKeHoach = lstNamKeHoach;

            return PartialView("_list", vm);
        }

        [HttpPost]
        public ActionResult GetModal(Guid? id)
        {
            NH_QT_QuyetToanNienDo data = new NH_QT_QuyetToanNienDo();

            List<NS_DonVi> lstDonViQL = _qlnhService.GetDonviList(PhienLamViec.NamLamViec).ToList();
            lstDonViQL.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, iID_MaDonVi = "", sTen = "--Chọn đơn vị--" });

            List<NH_DM_TiGia> lstTiGia = _qlnhService.GetNHDMTiGiaList().ToList();
            lstTiGia.Insert(0, new NH_DM_TiGia { ID = Guid.Empty, sTenTiGia = "--Chọn tỉ giá--" });
            ViewBag.ListDonVi = lstDonViQL;
            ViewBag.ListTiGia = lstTiGia;

            List<Dropdown_QuyetToanNienDo> lstNamKeHoach = GetListNamKeHoach();
            lstNamKeHoach.Insert(0, new Dropdown_QuyetToanNienDo { Value = 0, Label = "--Chọn năm--" });
            ViewBag.ListNamKeHoach = lstNamKeHoach;

            List<Dropdown_QuyetToanNienDo> loaiQuyetToan = lstLoaiQuyetToan;
            loaiQuyetToan.Insert(0, new Dropdown_QuyetToanNienDo { Value = 0, Label = "--Chọn loại quyết toán--" });
            ViewBag.ListLoaiQuyetToan = loaiQuyetToan;

            ViewBag.IsTongHop = false;
            if (id.HasValue)
            {
                data = _qlnhService.GetThongTinQuyetToanNienDoById(id.Value);
                ViewBag.IsTongHop = data.sTongHop != null ? true : false;

            }
            return PartialView("_modalUpdate", data);
        }

        [HttpPost]
        public ActionResult GetModalInBaoCao(string[] listId)
        {



            lstDonViVND.Insert(0, new Dropdown_QuyetToanNienDo { Value = 0, Label = "--Chọn đơn vị VND--" });
            ViewBag.ListDVVND = lstDonViVND;


            lstDonViUSD.Insert(0, new Dropdown_QuyetToanNienDo { Value = 0, Label = "--Chọn đơn vị USD--" });
            ViewBag.ListDVUSD = lstDonViUSD;

            ViewBag.IDQuyetToan = listId[0];

            return PartialView("_modalInBaoCao");
        }
        [HttpPost]
        public ActionResult GetModalDetail(Guid? id)
        {
            NH_QT_QuyetToanNienDoData data = new NH_QT_QuyetToanNienDoData();
            if (id.HasValue)
            {
                data = _qlnhService.GetThongTinQuyetToanNienDoById(id.Value);
                var donvi = _nganSachService.GetDonViById(PhienLamViec.NamLamViec.ToString(), data.iID_DonViID.ToString());
                data.sTenDonVi = donvi != null ? donvi.iID_MaDonVi + "-" + donvi.sTen : string.Empty;
                var tiGia = _qlnhService.GetNHDMTiGiaList(data.iID_TiGiaID).FirstOrDefault();
                data.sTenTiGia = tiGia != null ? tiGia.sTenTiGia + "-" + tiGia.sTenTiGia : string.Empty;
            }

            return PartialView("_modalDetail", data);
        }
        [HttpPost]
        public JsonResult Save(NH_QT_QuyetToanNienDo data)
        {
            if (data == null)
            {
                return Json(new { bIsComplete = false, sMessError = "Không cập nhật được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            var returnData = _qlnhService.SaveQuyetToanNienDo(data, Username);
            if (!returnData.IsReturn)
            {
                return Json(new { bIsComplete = false, sMessError = returnData.errorMess }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = true, dataID = returnData.QuyetToanNienDoData.ID }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveTongHop(NH_QT_QuyetToanNienDo data, string listId)
        {
            if (data == null)
            {
                return Json(new { bIsComplete = false, sMessError = "Không cập nhật được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            if (!_qlnhService.SaveTongHop(data, Username, listId))
            {
                return Json(new { bIsComplete = false, sMessError = "Không cập nhật được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Xoa(string id)
        {
            if (!_qlnhService.DeleteQuyetToanNienDo(Guid.Parse(id)))
            {
                return Json(new { bIsComplete = false, sMessError = "Không xóa được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool LockQuyetToan(Guid id)
        {
            try
            {
                NH_QT_QuyetToanNienDo entity = _qlnhService.GetThongTinQuyetToanNienDoById(id);
                if (entity != null)
                {
                    bool isLockOrUnlock = entity.bIsKhoa;
                    return _qlnhService.LockOrUnLockQuyetToanNienDo(id, !isLockOrUnlock);
                }
                return false;
            }
            catch (Exception ex)
            {
                AppLog.LogError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                return false;
            }
        }
        public List<Dropdown_QuyetToanNienDo> GetListNamKeHoach()
        {
            List<Dropdown_QuyetToanNienDo> listNam = new List<Dropdown_QuyetToanNienDo>();
            int namHienTai = DateTime.Now.Year + 1;
            for (int i = 20; i > 0; i--)
            {
                namHienTai -= 1;
                Dropdown_QuyetToanNienDo namKeHoachOpt = new Dropdown_QuyetToanNienDo()
                {
                    Value = namHienTai,
                    Label = namHienTai.ToString()
                };
                listNam.Add(namKeHoachOpt);
            }
            return listNam;
        }

        [HttpPost]
        public ActionResult GetModalTongHop(string[] listId, PagingInfo _paging, int namKeHoach)
        {
            QuyetToan_QuyetToanNienDoModel vm = new QuyetToan_QuyetToanNienDoModel();
            vm._paging = _paging;
            List<NH_QT_QuyetToanNienDoData> list = new List<NH_QT_QuyetToanNienDoData>();
            var stringId = "";
            foreach (var id in listId)
            {
                NH_QT_QuyetToanNienDoData data = _qlnhService.GetThongTinQuyetToanNienDoById(new Guid(id));
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
            ViewBag.ListNamKeHoach = namKeHoach;
            ViewBag.ListId = stringId.Substring(0, stringId.Length - 1);
            return PartialView("_modelTongHop", vm);
        }
    }
}
