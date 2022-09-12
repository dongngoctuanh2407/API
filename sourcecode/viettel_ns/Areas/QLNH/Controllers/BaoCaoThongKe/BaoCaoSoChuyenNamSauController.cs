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
using static Viettel.Services.IQLNHService;

namespace VIETTEL.Areas.QLNH.Controllers.BaoCaoThongKe
{
    public class BaoCaoSoChuyenNamSauController : FlexcelReportController
    {
        // GET: QLNH/BaoCaoSoChuyenNamSau
        private readonly IQLNHService _qlnhService = QLNHService.Default;
        private readonly INganSachService _nganSachService = NganSachService.Default;
        //private const string sFilePathBaoCao = "/Report_ExcelFrom/QLNH/rpt_QuyetToanNienDo3.xlsx";
        //private const string sControlName = "QuyetToanNienDo";
        public ActionResult Index()
        {
            NH_QT_QuyetToanNienDo_ChiTietView vm = new NH_QT_QuyetToanNienDo_ChiTietView();
            vm.ListDetailQuyetToanNienDo = new List<NH_QT_QuyetToanNienDo_ChiTietData>();
            List<NS_DonVi> lstDonViQL = _qlnhService.GetDonviList(PhienLamViec.NamLamViec).ToList();
            lstDonViQL.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sTen = "--Chọn đơn vị--" });
            ViewBag.ListDonVi = lstDonViQL;

            List<Dropdown_QuyetToanNienDo> lstNamKeHoach = GetListNamKeHoach();
            lstNamKeHoach.Insert(0, new Dropdown_QuyetToanNienDo { Value = 0, Label = "--Chọn năm--" });
            ViewBag.ListNamKeHoach = lstNamKeHoach;
            return View(vm);
        }
        [HttpPost]
        public ActionResult TimKiem(Guid? iDonVi, int? iNamKeHoach)
        {
            NH_QT_QuyetToanNienDo_ChiTietView vm = new NH_QT_QuyetToanNienDo_ChiTietView();
            List<NS_DonVi> lstDonViQL = _qlnhService.GetDonviList(PhienLamViec.NamLamViec).ToList();
            lstDonViQL.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sTen = "--Chọn đơn vị--" });
            ViewBag.ListDonVi = lstDonViQL;

            List<Dropdown_QuyetToanNienDo> lstNamKeHoach = GetListNamKeHoach();
            lstNamKeHoach.Insert(0, new Dropdown_QuyetToanNienDo { Value = 0, Label = "--Chọn năm--" });
            ViewBag.ListNamKeHoach = lstNamKeHoach;

            var listResult = getListDetailChiTiet(iDonVi, iNamKeHoach, false, null, null);
            vm.ListDetailQuyetToanNienDo = listResult;

            return PartialView("_baoCaoDetail", vm);
        }

        public List<NH_QT_QuyetToanNienDo_ChiTietData> getListDetailChiTiet(Guid? idMaDonVi, int? iNamKeHoach, bool isPrint, int? donViUSD, int? donViVND)
        {
            var listData = new List<NH_QT_QuyetToanNienDo_ChiTietData>();
            var listResult = new List<NH_QT_QuyetToanNienDo_ChiTietData>();

            var listDetail = _qlnhService.GetBaoCaoChiTietSoChuyenNamSauDetail(iNamKeHoach, idMaDonVi).ToList();
            //var listTitle = new List<NH_QT_QuyetToanNienDo_ChiTietData>();
            if (listDetail.Any())
            {
                listData = listDetail;
            }
            var listTitle = listData.Where(x => x.iID_ParentID != null).ToList();

            var getAllChuongTrinh = listData.Where(x => x.iID_DonVi == idMaDonVi && x.iID_KHCTBQP_NhiemVuChiID != null).Select(x => new { x.sTenNhiemVuChi, x.iID_KHCTBQP_NhiemVuChiID }).Distinct().ToList();

            var iCountChuongTrinh = 0;

            foreach (var chuongTrinh in getAllChuongTrinh)
            {
                iCountChuongTrinh++;
                var dataSumChuongTrinh = listData.Where(x => x.iID_KHCTBQP_NhiemVuChiID == chuongTrinh.iID_KHCTBQP_NhiemVuChiID).ToList();
                var newObj = new NH_QT_QuyetToanNienDo_ChiTietData()
                {
                    STT = convertLetter(iCountChuongTrinh),
                    sTenNoiDungChi = chuongTrinh.sTenNhiemVuChi,
                    bIsTittle = true,
                    fQTKinhPhiDuocCap_TongSo_USD = dataSumChuongTrinh.Sum(x => x.fQTKinhPhiDuocCap_TongSo_USD),
                    fQTKinhPhiDuocCap_TongSo_VND = dataSumChuongTrinh.Sum(x => x.fQTKinhPhiDuocCap_TongSo_VND),
                    fQTKinhPhiDuocCap_NamNay_USD = dataSumChuongTrinh.Sum(x => x.fQTKinhPhiDuocCap_NamNay_USD),
                    fQTKinhPhiDuocCap_NamNay_VND = dataSumChuongTrinh.Sum(x => x.fQTKinhPhiDuocCap_NamNay_VND),
                    fQTKinhPhiDuocCap_NamTruocChuyenSang_USD = dataSumChuongTrinh.Sum(x => x.fQTKinhPhiDuocCap_NamTruocChuyenSang_USD),
                    fQTKinhPhiDuocCap_NamTruocChuyenSang_VND = dataSumChuongTrinh.Sum(x => x.fQTKinhPhiDuocCap_NamTruocChuyenSang_VND),
                    fDeNghiQTNamNay_USD = dataSumChuongTrinh.Sum(x => x.fDeNghiQTNamNay_USD),
                    fDeNghiQTNamNay_VND = dataSumChuongTrinh.Sum(x => x.fDeNghiQTNamNay_VND),
                    fDeNghiChuyenNamSau_USD = dataSumChuongTrinh.Sum(x => x.fDeNghiChuyenNamSau_USD),
                    fDeNghiChuyenNamSau_VND = dataSumChuongTrinh.Sum(x => x.fDeNghiChuyenNamSau_VND),
                    fThuaThieuKinhPhiTrongNam_USD = dataSumChuongTrinh.Sum(x => x.fThuaThieuKinhPhiTrongNam_USD),
                    fThuaThieuKinhPhiTrongNam_VND = dataSumChuongTrinh.Sum(x => x.fThuaThieuKinhPhiTrongNam_VND),
                    iParentId = 0
                };
                listResult.Add(newObj);
                var getListDuAn = listData.Where(x => x.iID_KHCTBQP_NhiemVuChiID == chuongTrinh.iID_KHCTBQP_NhiemVuChiID && x.iID_DuAnID != null && x.iID_ParentID == null).ToList();
                var getListHopDong = listData.Where(x => x.iID_KHCTBQP_NhiemVuChiID == chuongTrinh.iID_KHCTBQP_NhiemVuChiID && x.iID_DuAnID == null && x.iID_HopDongID != null && x.iID_ParentID == null).ToList();
                var getListNone = listData.Where(x => x.iID_KHCTBQP_NhiemVuChiID == chuongTrinh.iID_KHCTBQP_NhiemVuChiID && x.iID_DuAnID == null && x.iID_HopDongID == null && x.iID_ParentID == null).ToList();
                var iCountDuAn = 0;

                if (getListDuAn.Any())
                {
                    var getNameDuAn = getListDuAn.Select(x => new { x.sTenDuAn, x.iID_DuAnID, x.fHopDong_VND_DuAn, x.fHopDong_USD_DuAn })
                    .Distinct()
                    .ToList();
                    foreach (var hopDongDuAn in getNameDuAn)
                    {
                        iCountDuAn++;
                        var newObjHopDongDuAn = new NH_QT_QuyetToanNienDo_ChiTietData();
                        var findTittle = listTitle.Find(x => x.iID_ParentID == hopDongDuAn.iID_DuAnID && x.iID_KHCTBQP_NhiemVuChiID == chuongTrinh.iID_KHCTBQP_NhiemVuChiID && x.iID_DuAnID == hopDongDuAn.iID_DuAnID);
                        var dataSumDuAn = listData.Where(x => x.iID_KHCTBQP_NhiemVuChiID == chuongTrinh.iID_KHCTBQP_NhiemVuChiID && x.iID_DuAnID == hopDongDuAn.iID_DuAnID).ToList();
                        if (findTittle != null)
                        {
                            newObjHopDongDuAn.MapFrom(findTittle);
                            newObjHopDongDuAn.fDeNghiQTNamNay_USD = findTittle.fDeNghiQTNamNay_USD != null ? findTittle.fDeNghiQTNamNay_USD : dataSumDuAn.Sum(x => x.fDeNghiQTNamNay_USD);
                            newObjHopDongDuAn.fDeNghiQTNamNay_VND = findTittle.fDeNghiQTNamNay_VND != null ? findTittle.fDeNghiQTNamNay_VND : dataSumDuAn.Sum(x => x.fDeNghiQTNamNay_VND);
                            newObjHopDongDuAn.fDeNghiChuyenNamSau_USD = findTittle.fDeNghiChuyenNamSau_USD != null ? findTittle.fDeNghiChuyenNamSau_USD : dataSumDuAn.Sum(x => x.fDeNghiChuyenNamSau_USD);
                            newObjHopDongDuAn.fDeNghiChuyenNamSau_VND = findTittle.fDeNghiChuyenNamSau_VND != null ? findTittle.fDeNghiChuyenNamSau_VND : dataSumDuAn.Sum(x => x.fDeNghiChuyenNamSau_VND);
                        }
                        newObjHopDongDuAn.STT = convertLaMa(Decimal.Parse(iCountDuAn.ToString()));
                        newObjHopDongDuAn.sTenNoiDungChi = hopDongDuAn.sTenDuAn;
                        newObjHopDongDuAn.bIsTittle = true;
                        newObjHopDongDuAn.fHopDong_VND = hopDongDuAn.fHopDong_VND_DuAn;
                        newObjHopDongDuAn.fHopDong_USD = hopDongDuAn.fHopDong_USD_DuAn;
                        newObjHopDongDuAn.bIsData = true;
                        newObjHopDongDuAn.sLevel = "1";
                        newObjHopDongDuAn.iID_ParentID = hopDongDuAn.iID_DuAnID;
                        newObjHopDongDuAn.iID_KHCTBQP_NhiemVuChiID = chuongTrinh.iID_KHCTBQP_NhiemVuChiID;
                        newObjHopDongDuAn.iID_DuAnID = hopDongDuAn.iID_DuAnID;
                        newObjHopDongDuAn.fQTKinhPhiDuocCap_TongSo_USD = dataSumDuAn.Sum(x => x.fQTKinhPhiDuocCap_TongSo_USD);
                        newObjHopDongDuAn.fQTKinhPhiDuocCap_TongSo_VND = dataSumDuAn.Sum(x => x.fQTKinhPhiDuocCap_TongSo_VND);
                        newObjHopDongDuAn.fQTKinhPhiDuocCap_NamNay_USD = dataSumDuAn.Sum(x => x.fQTKinhPhiDuocCap_NamNay_USD);
                        newObjHopDongDuAn.fQTKinhPhiDuocCap_NamNay_VND = dataSumDuAn.Sum(x => x.fQTKinhPhiDuocCap_NamNay_VND);
                        newObjHopDongDuAn.fQTKinhPhiDuocCap_NamTruocChuyenSang_USD = dataSumDuAn.Sum(x => x.fQTKinhPhiDuocCap_NamTruocChuyenSang_USD);
                        newObjHopDongDuAn.fQTKinhPhiDuocCap_NamTruocChuyenSang_VND = dataSumDuAn.Sum(x => x.fQTKinhPhiDuocCap_NamTruocChuyenSang_VND);
                        newObjHopDongDuAn.fThuaThieuKinhPhiTrongNam_USD = dataSumDuAn.Sum(x => x.fThuaThieuKinhPhiTrongNam_USD);
                        newObjHopDongDuAn.fThuaThieuKinhPhiTrongNam_VND = dataSumDuAn.Sum(x => x.fThuaThieuKinhPhiTrongNam_VND);


                        listResult.Add(newObjHopDongDuAn);
                    }
                }
                if (getListHopDong.Any())
                {
                    iCountDuAn++;
                    var getSumHopDong = listData.Where(x => x.iID_KHCTBQP_NhiemVuChiID == chuongTrinh.iID_KHCTBQP_NhiemVuChiID && x.iID_DuAnID == null && x.iID_HopDongID != null).ToList();
                    var newObjHopDong = new NH_QT_QuyetToanNienDo_ChiTietData()
                    {
                        STT = convertLaMa(Decimal.Parse(iCountDuAn.ToString())),
                        sTenNoiDungChi = "Chi hợp đồng",
                        bIsTittle = true,
                        fQTKinhPhiDuocCap_TongSo_USD = getSumHopDong.Sum(x => x.fQTKinhPhiDuocCap_TongSo_USD),
                        fQTKinhPhiDuocCap_TongSo_VND = getSumHopDong.Sum(x => x.fQTKinhPhiDuocCap_TongSo_VND),
                        fQTKinhPhiDuocCap_NamNay_USD = getSumHopDong.Sum(x => x.fQTKinhPhiDuocCap_NamNay_USD),
                        fQTKinhPhiDuocCap_NamNay_VND = getSumHopDong.Sum(x => x.fQTKinhPhiDuocCap_NamNay_VND),
                        fQTKinhPhiDuocCap_NamTruocChuyenSang_USD = getSumHopDong.Sum(x => x.fQTKinhPhiDuocCap_NamTruocChuyenSang_USD),
                        fQTKinhPhiDuocCap_NamTruocChuyenSang_VND = getSumHopDong.Sum(x => x.fQTKinhPhiDuocCap_NamTruocChuyenSang_VND),
                        fDeNghiQTNamNay_USD = getSumHopDong.Sum(x => x.fDeNghiQTNamNay_USD),
                        fDeNghiQTNamNay_VND = getSumHopDong.Sum(x => x.fDeNghiQTNamNay_VND),
                        fDeNghiChuyenNamSau_USD = getSumHopDong.Sum(x => x.fDeNghiChuyenNamSau_USD),
                        fDeNghiChuyenNamSau_VND = getSumHopDong.Sum(x => x.fDeNghiChuyenNamSau_VND),
                        fThuaThieuKinhPhiTrongNam_USD = getSumHopDong.Sum(x => x.fThuaThieuKinhPhiTrongNam_USD),
                        fThuaThieuKinhPhiTrongNam_VND = getSumHopDong.Sum(x => x.fThuaThieuKinhPhiTrongNam_VND),
                    };
                    listResult.Add(newObjHopDong);
                    listResult.AddRange(returnLoaiChi(chuongTrinh.iID_KHCTBQP_NhiemVuChiID, null, true, getListHopDong, listTitle.Where(x => x.iID_DuAnID == null && x.iID_HopDongID != null).ToList()));
                    //
                }
                if (getListNone.Any())
                {
                    iCountDuAn++;
                    var newObjKhac = new NH_QT_QuyetToanNienDo_ChiTietData()
                    {
                        STT = convertLaMa(Decimal.Parse(iCountDuAn.ToString())),
                        sTenNoiDungChi = "Chi khác",
                        bIsTittle = true,
                        fQTKinhPhiDuocCap_TongSo_USD = getListNone.Sum(x => x.fQTKinhPhiDuocCap_TongSo_USD),
                        fQTKinhPhiDuocCap_TongSo_VND = getListNone.Sum(x => x.fQTKinhPhiDuocCap_TongSo_VND),
                        fQTKinhPhiDuocCap_NamNay_USD = getListNone.Sum(x => x.fQTKinhPhiDuocCap_NamNay_USD),
                        fQTKinhPhiDuocCap_NamNay_VND = getListNone.Sum(x => x.fQTKinhPhiDuocCap_NamNay_VND),
                        fQTKinhPhiDuocCap_NamTruocChuyenSang_USD = getListNone.Sum(x => x.fQTKinhPhiDuocCap_NamTruocChuyenSang_USD),
                        fQTKinhPhiDuocCap_NamTruocChuyenSang_VND = getListNone.Sum(x => x.fQTKinhPhiDuocCap_NamTruocChuyenSang_VND),
                        fDeNghiQTNamNay_USD = getListNone.Sum(x => x.fDeNghiQTNamNay_USD),
                        fDeNghiQTNamNay_VND = getListNone.Sum(x => x.fDeNghiQTNamNay_VND),
                        fDeNghiChuyenNamSau_USD = getListNone.Sum(x => x.fDeNghiChuyenNamSau_USD),
                        fDeNghiChuyenNamSau_VND = getListNone.Sum(x => x.fDeNghiChuyenNamSau_VND),
                        fThuaThieuKinhPhiTrongNam_USD = getListNone.Sum(x => x.fThuaThieuKinhPhiTrongNam_USD),
                        fThuaThieuKinhPhiTrongNam_VND = getListNone.Sum(x => x.fThuaThieuKinhPhiTrongNam_VND),
                    };
                    listResult.Add(newObjKhac);
                    listResult.AddRange(returnLoaiChi(chuongTrinh.iID_KHCTBQP_NhiemVuChiID, null, false, getListNone, listTitle.Where(x => x.iID_DuAnID == null && x.iID_HopDongID == null).ToList()));
                }
            }
            return listResult;
        }
        public List<NH_QT_QuyetToanNienDo_ChiTietData> returnLoaiChi(Guid? idChuongTrinh, Guid? idDuAn, bool isDuAn, List<NH_QT_QuyetToanNienDo_ChiTietData> list, List<NH_QT_QuyetToanNienDo_ChiTietData> listTittle)
        {
            List<NH_QT_QuyetToanNienDo_ChiTietData> returnData = new List<NH_QT_QuyetToanNienDo_ChiTietData>();
            var listLoaiChiPhi = list.Select(x => new { x.iLoaiNoiDungChi }).Distinct().OrderBy(x => x.iLoaiNoiDungChi)
                  .ToList();
            var countLoaiChiPhi = 0;
            foreach (var loaiChiPhi in listLoaiChiPhi)
            {
                var dataSumLoaiChi = list.Where(x => x.iLoaiNoiDungChi == loaiChiPhi.iLoaiNoiDungChi).ToList();

                countLoaiChiPhi++;
                var newObjLoaiChiPhi = new NH_QT_QuyetToanNienDo_ChiTietData()
                {
                    STT = countLoaiChiPhi.ToString(),
                    sTenNoiDungChi = loaiChiPhi.iLoaiNoiDungChi == 1 ? "Chi ngoại tệ" : "Chi trong nước",
                    bIsTittle = true,
                    fQTKinhPhiDuocCap_TongSo_USD = dataSumLoaiChi.Sum(x => x.fQTKinhPhiDuocCap_TongSo_USD),
                    fQTKinhPhiDuocCap_TongSo_VND = dataSumLoaiChi.Sum(x => x.fQTKinhPhiDuocCap_TongSo_VND),
                    fQTKinhPhiDuocCap_NamNay_USD = dataSumLoaiChi.Sum(x => x.fQTKinhPhiDuocCap_NamNay_USD),
                    fQTKinhPhiDuocCap_NamNay_VND = dataSumLoaiChi.Sum(x => x.fQTKinhPhiDuocCap_NamNay_VND),
                    fQTKinhPhiDuocCap_NamTruocChuyenSang_USD = dataSumLoaiChi.Sum(x => x.fQTKinhPhiDuocCap_NamTruocChuyenSang_USD),
                    fQTKinhPhiDuocCap_NamTruocChuyenSang_VND = dataSumLoaiChi.Sum(x => x.fQTKinhPhiDuocCap_NamTruocChuyenSang_VND),
                    fDeNghiQTNamNay_USD = dataSumLoaiChi.Sum(x => x.fDeNghiQTNamNay_USD) + listTittle.Where(x => x.iLoaiNoiDungChi == loaiChiPhi.iLoaiNoiDungChi).Sum(x => x.fDeNghiQTNamNay_USD),
                    fDeNghiQTNamNay_VND = dataSumLoaiChi.Sum(x => x.fDeNghiQTNamNay_VND) + listTittle.Where(x => x.iLoaiNoiDungChi == loaiChiPhi.iLoaiNoiDungChi).Sum(x => x.fDeNghiQTNamNay_VND),
                    fDeNghiChuyenNamSau_USD = dataSumLoaiChi.Sum(x => x.fDeNghiChuyenNamSau_USD) + listTittle.Where(x => x.iLoaiNoiDungChi == loaiChiPhi.iLoaiNoiDungChi).Sum(x => x.fDeNghiChuyenNamSau_USD),
                    fDeNghiChuyenNamSau_VND = dataSumLoaiChi.Sum(x => x.fDeNghiChuyenNamSau_VND) + listTittle.Where(x => x.iLoaiNoiDungChi == loaiChiPhi.iLoaiNoiDungChi).Sum(x => x.fDeNghiChuyenNamSau_VND),
                    fThuaThieuKinhPhiTrongNam_USD = dataSumLoaiChi.Sum(x => x.fThuaThieuKinhPhiTrongNam_USD),
                    fThuaThieuKinhPhiTrongNam_VND = dataSumLoaiChi.Sum(x => x.fThuaThieuKinhPhiTrongNam_VND),
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
                        var newObjHopDongDuAn = new NH_QT_QuyetToanNienDo_ChiTietData();
                        var findTittle = listTittle.Find(x => x.iID_HopDongID == nameHopDong.iID_HopDongID && x.iID_KHCTBQP_NhiemVuChiID == idChuongTrinh && x.iID_DuAnID == idDuAn && x.iLoaiNoiDungChi == loaiChiPhi.iLoaiNoiDungChi);
                        var listHopDong = list.Where(x => x.iLoaiNoiDungChi == loaiChiPhi.iLoaiNoiDungChi && x.iID_HopDongID == nameHopDong.iID_HopDongID).ToList();

                        if (findTittle != null)
                        {
                            newObjHopDongDuAn.MapFrom(findTittle);
                            newObjHopDongDuAn.fDeNghiQTNamNay_USD = findTittle.fDeNghiQTNamNay_USD != null ? findTittle.fDeNghiQTNamNay_USD : listHopDong.Sum(x => x.fDeNghiQTNamNay_USD);
                            newObjHopDongDuAn.fDeNghiQTNamNay_VND = findTittle.fDeNghiQTNamNay_VND != null ? findTittle.fDeNghiQTNamNay_VND : listHopDong.Sum(x => x.fDeNghiQTNamNay_VND);
                            newObjHopDongDuAn.fDeNghiChuyenNamSau_USD = findTittle.fDeNghiChuyenNamSau_USD != null ? findTittle.fDeNghiChuyenNamSau_USD : listHopDong.Sum(x => x.fDeNghiChuyenNamSau_USD);
                            newObjHopDongDuAn.fDeNghiChuyenNamSau_VND = findTittle.fDeNghiChuyenNamSau_VND != null ? findTittle.fDeNghiChuyenNamSau_VND : listHopDong.Sum(x => x.fDeNghiChuyenNamSau_VND);
                        }
                        newObjHopDongDuAn.STT = countLoaiChiPhi.ToString() + "." + countHopDong.ToString();
                        newObjHopDongDuAn.sTenNoiDungChi = nameHopDong.sTenHopDong;
                        newObjHopDongDuAn.fHopDong_VND = nameHopDong.fHopDong_VND_HopDong;
                        newObjHopDongDuAn.fHopDong_USD = nameHopDong.fHopDong_USD_HopDong;
                        newObjHopDongDuAn.bIsData = true;
                        newObjHopDongDuAn.bIsTittle = true;
                        newObjHopDongDuAn.sLevel = "2";
                        newObjHopDongDuAn.iID_ParentID = nameHopDong.iID_HopDongID;
                        newObjHopDongDuAn.iID_KHCTBQP_NhiemVuChiID = idChuongTrinh;
                        newObjHopDongDuAn.iID_HopDongID = nameHopDong.iID_HopDongID;
                        newObjHopDongDuAn.iID_DuAnID = idDuAn;

                        newObjHopDongDuAn.fQTKinhPhiDuocCap_TongSo_USD = listHopDong.Sum(x => x.fQTKinhPhiDuocCap_TongSo_USD);
                        newObjHopDongDuAn.fQTKinhPhiDuocCap_TongSo_VND = listHopDong.Sum(x => x.fQTKinhPhiDuocCap_TongSo_VND);
                        newObjHopDongDuAn.fQTKinhPhiDuocCap_NamNay_USD = listHopDong.Sum(x => x.fQTKinhPhiDuocCap_NamNay_USD);
                        newObjHopDongDuAn.fQTKinhPhiDuocCap_NamNay_VND = listHopDong.Sum(x => x.fQTKinhPhiDuocCap_NamNay_VND);
                        newObjHopDongDuAn.fQTKinhPhiDuocCap_NamTruocChuyenSang_USD = listHopDong.Sum(x => x.fQTKinhPhiDuocCap_NamTruocChuyenSang_USD);
                        newObjHopDongDuAn.fQTKinhPhiDuocCap_NamTruocChuyenSang_VND = listHopDong.Sum(x => x.fQTKinhPhiDuocCap_NamTruocChuyenSang_VND);
                        newObjHopDongDuAn.fThuaThieuKinhPhiTrongNam_USD = listHopDong.Sum(x => x.fThuaThieuKinhPhiTrongNam_USD);
                        newObjHopDongDuAn.fThuaThieuKinhPhiTrongNam_VND = listHopDong.Sum(x => x.fThuaThieuKinhPhiTrongNam_VND);
                        newObjHopDongDuAn.iID_ThanhToan_ChiTietID = listHopDong.FirstOrDefault().iID_ThanhToan_ChiTietID;
                        returnData.Add(newObjHopDongDuAn);
                    }
                }
            }
            return returnData;
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
    }
}