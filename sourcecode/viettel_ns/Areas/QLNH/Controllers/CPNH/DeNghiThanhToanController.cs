using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Viettel.Domain.DomainModel;
using Viettel.Models.QLNH;
using Viettel.Services;
using VIETTEL.Common;
using VIETTEL.Controllers;
using DapperExtensions;
using VIETTEL.Helpers;
using VIETTEL.Flexcel;
using FlexCel.Core;
using FlexCel.XlsAdapter;
using FlexCel.Report;
using Viettel.Models.QLVonDauTu;
using System.Globalization;
using DomainModel;

namespace VIETTEL.Areas.QLNH.Controllers.DanhMucNgoaiHoi
{
    public class DeNghiThanhToanController : FlexcelReportController
    {

        private readonly IQLNHService _qlnhService = QLNHService.Default;
        private const string sFilePathGiayDeNghiThanhToanNgoaiTe = "/Report_ExcelFrom/QLNH/rpt_GiayDeNghi_ThanhToan_NgoaiTe.xlsx";
        private const string sFilePathGiayDeNghiThanhToanTrongNuoc = "/Report_ExcelFrom/QLNH/rpt_GiayDeNghi_ThanhToan_ChiTrongNuoc.xlsx";
        private const string sFilePathThongBaoChiNganSach = "/Report_ExcelFrom/QLNH/rpt_ThongBaoChiNganSach.xlsx";
        private const string sFilePathThongBaoCapKinhPhiNgoaiTe = "/Report_ExcelFrom/QLNH/rpt_ThongBao_ChiNgoaiTe.xlsx";
        private const string sFilePathThongBaoCapKinhPhiTrongNuoc = "/Report_ExcelFrom/QLNH/rpt_ThongBaoChiTrongNuoc.xlsx";
        private const string sControlName = "DeNghiThanhToan";

        public ActionResult Index()
        {
            ThongTinThanhToanPagingViewModel tt = new ThongTinThanhToanPagingViewModel();
            tt._paging.CurrentPage = 1;
            tt.Items = _qlnhService.GetAllThongTinThanhToanPaging(ref tt._paging, null, null, null, null, null, null, null, null, null, null, null);
            ViewBag.lstNSDonVi = _qlnhService.GetAllNSDonVi(int.Parse(PhienLamViec.iNamLamViec)).ToSelectList("iID_Ma", "sTenDonVi");
            ViewBag.lstNhiemVuChi = _qlnhService.GetAllNhiemVuChiByDonVi().ToSelectList("ID", "sTenNhiemVuChi");
            ViewBag.lstChuDauTu = _qlnhService.GetAllDMChuDauTu().ToSelectList("ID", "sTenChuDauTu");
            ViewBag.lstDanhMucNhaThau = _qlnhService.GetAllDMNhaThau().ToSelectList("Id", "sTenNhaThau");
            return View(tt);
        }

        [HttpPost]
        public ActionResult DeNghiThanhToanSearch(ThongTinThanhToanSearchModel data)
        {
            ThongTinThanhToanPagingViewModel tt = new ThongTinThanhToanPagingViewModel();
            tt._paging.CurrentPage = data._paging.CurrentPage;
            tt.Items = _qlnhService.GetAllThongTinThanhToanPaging(ref tt._paging, data.iID_DonVi, data.sSoDeNghi, data.dNgayDeNghi, data.iLoaiNoiDungChi, data.iID_ChuDauTuID, data.iID_KHCTBQP_NhiemVuChiID, data.iNamKeHoach, data.iNamNganSach, data.iCoQuanThanhToan, data.iID_NhaThauID, data.iTrangThai);
            ViewBag.lstNSDonVi = _qlnhService.GetAllNSDonVi(int.Parse(PhienLamViec.iNamLamViec)).ToSelectList("iID_Ma", "sTenDonVi");
            ViewBag.lstChuDauTu = _qlnhService.GetAllDMChuDauTu().ToSelectList("ID", "sTenChuDauTu");
            ViewBag.lstNhiemVuChi = _qlnhService.GetAllNhiemVuChiByDonVi(data.iID_DonVi).ToSelectList("ID", "sTenNhiemVuChi");
            ViewBag.lstDanhMucNhaThau = _qlnhService.GetAllDMNhaThau().ToSelectList("Id", "sTenNhaThau");
            return PartialView("_list", tt);
        }

        [HttpPost]
        public JsonResult GetAllNhiemVuChiByDonVi(Guid iDDonVi)
        {
            var lstNhiemVuChi = _qlnhService.GetAllNhiemVuChiByDonVi(iDDonVi);
            return Json(new { data = lstNhiemVuChi, status = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Insert()
        {
            ViewBag.lstNSDonVi = _qlnhService.GetAllNSDonVi(int.Parse(PhienLamViec.iNamLamViec)).ToSelectList("iID_Ma", "sTenDonVi");
            ViewBag.lstNhiemVuChi = _qlnhService.GetAllNhiemVuChiByDonVi().ToSelectList("ID", "sTenNhiemVuChi");
            ViewBag.lstChuDauTu = _qlnhService.GetAllDMChuDauTu().ToSelectList("ID", "sTenChuDauTu");
            ViewBag.lstTyGiaDeNghi = _qlnhService.GetThongTinTyGia().ToSelectList("ID", "sTenTiGia");
            ViewBag.lstTyGiaPheDuyet = _qlnhService.GetThongTinTyGia().ToSelectList("ID", "sTenTiGia");
            ViewBag.lstDonViHuongThu = _qlnhService.GetAllDMNhaThau().ToSelectList("Id", "sTenNhaThau");
            return View();
        }

        public ActionResult Update(Guid?id)
        {
            ThongTinThanhToanDetaiModel model = new ThongTinThanhToanDetaiModel();
            var thanhtoan = _qlnhService.GetThongTinThanhToanByID(id);
            var thanhtoan_chitiet = _qlnhService.GetThongTinThanhToanChiTietById(id);
            model.thongtinthanhtoan = thanhtoan; 
            model.thanhtoan_chitiet = thanhtoan_chitiet;

            var lstduan = _qlnhService.GetDADuAn(thanhtoan == null ? null : thanhtoan.iID_KHCTBQP_NhiemVuChiID, thanhtoan == null ? null : thanhtoan.iID_ChuDauTuID);
            var lsthopdong = _qlnhService.GetThongTinHopDong(thanhtoan == null ? null : thanhtoan.iID_KHCTBQP_NhiemVuChiID); 

            ViewBag.lstNSDonVi = _qlnhService.GetAllNSDonVi(int.Parse(PhienLamViec.iNamLamViec)).ToSelectList("iID_Ma", "sTenDonVi");
            ViewBag.lstNhiemVuChi = _qlnhService.GetAllNhiemVuChiByDonVi(thanhtoan == null ? null : thanhtoan.iID_DonVi).ToSelectList("ID", "sTenNhiemVuChi");
            ViewBag.lstChuDauTu = _qlnhService.GetAllDMChuDauTu().ToSelectList("ID", "sTenChuDauTu");
            ViewBag.lstTyGiaDeNghi = _qlnhService.GetThongTinTyGia().ToSelectList("ID", "sTenTiGia");
            ViewBag.lstTyGiaPheDuyet = _qlnhService.GetThongTinTyGia().ToSelectList("ID", "sTenTiGia");
            ViewBag.lstDonViHuongThu = _qlnhService.GetAllDMNhaThau().ToSelectList("Id", "sTenNhaThau");
            ViewBag.lstHopDong = lsthopdong.ToSelectList("ID", "sTenHopDong"); ;
            ViewBag.lstDuAn = lstduan.ToSelectList("ID", "sTenDuAn");

            var duan = lstduan.Where(x => x.ID == thanhtoan.iID_DuAnID).FirstOrDefault();
            var hopdong = lsthopdong.Where(x => x.ID == thanhtoan.iID_HopDongID).FirstOrDefault();

            if(duan != null)
            {
                model.thongtinthanhtoan.sDuToanPheDuyet_USD = duan.fGiaTriUSD != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(duan.fGiaTriUSD.Value, 2).ToString(CultureInfo.InvariantCulture), 2) : String.Empty;
                model.thongtinthanhtoan.sDuToanPheDuyet_VND = duan.fGiaTriVND != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(duan.fGiaTriVND.Value, 0).ToString(CultureInfo.InvariantCulture), 0) : String.Empty;
            }
            if (hopdong != null)
            {
                model.thongtinthanhtoan.sHopDongPheDuyet_USD = hopdong.fGiaTriUSD != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(hopdong.fGiaTriUSD.Value, 2).ToString(CultureInfo.InvariantCulture), 2) : String.Empty;
                model.thongtinthanhtoan.sHopDongPheDuyet_VND = hopdong.fGiaTriVND != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(hopdong.fGiaTriVND.Value, 0).ToString(CultureInfo.InvariantCulture), 0) : String.Empty;
            }

            return View(model);
        }

        public ActionResult Detail(Guid id)
        {
            ThongTinThanhToanDetaiModel model = new ThongTinThanhToanDetaiModel();
            var thanhtoan = _qlnhService.GetThongTinThanhToanByID(id);
            var thanhtoan_chitiet = _qlnhService.GetThongTinThanhToanChiTietById(id);
            model.thongtinthanhtoan = thanhtoan;
            ViewBag.sTongDeNghi_USD = thanhtoan.fTongDeNghi_USD != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(thanhtoan.fTongDeNghi_USD.Value, 2).ToString(CultureInfo.InvariantCulture), 2) : String.Empty;
            ViewBag.sTongPheDuyet_USD = thanhtoan.fTongPheDuyet_USD != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(thanhtoan.fTongPheDuyet_USD.Value, 2).ToString(CultureInfo.InvariantCulture), 2) : String.Empty;
            ViewBag.sLuyKeUSD = thanhtoan.fLuyKeUSD != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(thanhtoan.fLuyKeUSD.Value, 2).ToString(CultureInfo.InvariantCulture), 2) : String.Empty;
            ViewBag.sLuyKeVND = thanhtoan.fLuyKeVND != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(thanhtoan.fLuyKeVND.Value, 2).ToString(CultureInfo.InvariantCulture), 0) : String.Empty;
            model.thanhtoan_chitiet = thanhtoan_chitiet;
            if (model.thanhtoan_chitiet != null)
            {
                foreach (var item in model.thanhtoan_chitiet)
                {
                    item.sDeNghiCapKyNay_USD = item.fDeNghiCapKyNay_USD != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(item.fDeNghiCapKyNay_USD.Value, 2).ToString(CultureInfo.InvariantCulture), 2) : String.Empty;
                    item.sPheDuyetCapKyNay_USD = item.fPheDuyetCapKyNay_USD != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(item.fPheDuyetCapKyNay_USD.Value, 2).ToString(CultureInfo.InvariantCulture), 2) : String.Empty;

                }
            }

            model.thongtinthanhtoan.sTiGiaDeNghi = _qlnhService.GetThongTinTyGia().Where(x => x.ID == thanhtoan.iID_TiGiaDeNghiID).FirstOrDefault().sTenTiGia;
            model.thongtinthanhtoan.sTiGiaPheDuyet = _qlnhService.GetThongTinTyGia().Where(x => x.ID == thanhtoan.iID_TiGiaPheDuyetID).FirstOrDefault().sTenTiGia;
            var hopdong = _qlnhService.GetThongTinHopDong(thanhtoan.iID_KHCTBQP_NhiemVuChiID.Value).Where(x => x.ID == thanhtoan.iID_HopDongID).FirstOrDefault();
            var duan = _qlnhService.GetDADuAn(thanhtoan.iID_KHCTBQP_NhiemVuChiID.Value, thanhtoan.iID_ChuDauTuID.Value).Where(x => x.ID == thanhtoan.iID_DuAnID).FirstOrDefault();
            if (duan != null)
            {
                model.thongtinthanhtoan.sDuToanPheDuyet_USD = duan.fGiaTriUSD != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(duan.fGiaTriUSD.Value, 2).ToString(CultureInfo.InvariantCulture), 2) : String.Empty;
                model.thongtinthanhtoan.sDuToanPheDuyet_VND = duan.fGiaTriVND != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(duan.fGiaTriVND.Value, 0).ToString(CultureInfo.InvariantCulture), 0) : String.Empty;
            }
            if (hopdong != null)
            {
                model.thongtinthanhtoan.sHopDongPheDuyet_USD = hopdong.fGiaTriUSD != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(hopdong.fGiaTriUSD.Value, 2).ToString(CultureInfo.InvariantCulture), 2) : String.Empty;
                model.thongtinthanhtoan.sHopDongPheDuyet_VND = hopdong.fGiaTriVND != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(hopdong.fGiaTriVND.Value, 0).ToString(CultureInfo.InvariantCulture), 0) : String.Empty;
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult GetThongTinDuAn(Guid? iID_NhiemVuChi, Guid? iID_ChuDauTu)
        {
            var lstThongTinDuAn = _qlnhService.GetDADuAn(iID_NhiemVuChi, iID_ChuDauTu);
            return Json(new { data = lstThongTinDuAn, status = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetThongHopDong(Guid? iID_NhiemVuChi)
        {
            var lstThongTinHopDong = _qlnhService.GetThongTinHopDong(iID_NhiemVuChi);
            return Json(new { data = lstThongTinHopDong, status = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetChuyenDoiTyGia(Guid? matygia, float sotiennhap, int loaitiennhap)
        {
            double tygiasauchuyendoi = _qlnhService.ChuyenDoiTyGia(matygia, sotiennhap, loaitiennhap);
            return Json(new { chuyendoi = tygiasauchuyendoi, status = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MucNganSachSearch(ThongTinThanhToanSearchModel data)
        {
            MucLucNganSachPagingViewModel vm = new MucLucNganSachPagingViewModel();
            if (data._paging == null)
            {
                vm._paging.CurrentPage = 1;
            }
            else
            {
                vm._paging.CurrentPage = data._paging.CurrentPage;
            }

            vm.Items = _qlnhService.GetAllMucLucNganSach(ref vm._paging);
            return PartialView("_listmuclucngansachsearch", vm);
        }

        public JsonResult CheckTrungMaDeNghi(string sodenghi, int type_action, Guid? idenghi)
        {
            Boolean results = _qlnhService.CheckTrungMaDeNghi(sodenghi, type_action, idenghi);
            return Json(new { results = results, status = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool SaveDeNghiThanhToan(NH_TT_ThanhToanModel data, string listIDChiTietXoa)
        {
            try
            {
                using (var conn = ConnectionFactory.Default.GetConnection())
                {
                    conn.Open();
                    var trans = conn.BeginTransaction();

                    var entityThongTinThanhToan = new NH_TT_ThanhToan();

                    if (data.lstTTThanToan_ChiTiet != null)
                    {
                        //Convert so tien
                        foreach (var item in data.lstTTThanToan_ChiTiet)
                        {
                            item.fPheDuyetCapKyNay_VND = TryParseDouble(item.sPheDuyetCapKyNay_VND);
                            item.fPheDuyetCapKyNay_USD = TryParseDouble(item.sPheDuyetCapKyNay_USD);
                            item.fDeNghiCapKyNay_USD = TryParseDouble(item.sDeNghiCapKyNay_USD);
                            item.fDeNghiCapKyNay_VND = TryParseDouble(item.sDeNghiCapKyNay_VND);
                        }
                    }

                    if (data.TT_ThanToan.iThanhToanTheo == 1 && data.TT_ThanToan.iID_HopDongID != null)
                    {
                        var lstThongTinHopDong = _qlnhService.GetThongTinHopDong(data.TT_ThanToan.iID_KHCTBQP_NhiemVuChiID.Value);
                        NH_DA_HopDong hopdong = lstThongTinHopDong.Where(x => x.ID == data.TT_ThanToan.iID_HopDongID).FirstOrDefault();
                        data.TT_ThanToan.iID_DuAnID = hopdong != null ? hopdong.iID_DuAnID : null;
                    }

                    //Thực hiện thêm mới đề nghị thanh toán
                    if (data.TT_ThanToan.ID == Guid.Empty || data.TT_ThanToan.ID == null)
                    {

                        entityThongTinThanhToan.MapFrom(data.TT_ThanToan);
                        entityThongTinThanhToan.dNgayTao = DateTime.Now;
                        entityThongTinThanhToan.sNguoiTao = Username;
                        if (data.lstTTThanToan_ChiTiet != null)
                        {
                            entityThongTinThanhToan.fTongDeNghi_USD = data.lstTTThanToan_ChiTiet.Sum(x => x.fDeNghiCapKyNay_USD);
                            entityThongTinThanhToan.fTongDeNghi_VND = data.lstTTThanToan_ChiTiet.Sum(x => x.fDeNghiCapKyNay_VND);
                            entityThongTinThanhToan.fTongPheDuyet_USD = data.lstTTThanToan_ChiTiet.Sum(x => x.fPheDuyetCapKyNay_USD);
                            entityThongTinThanhToan.fTongPheDuyet_VND = data.lstTTThanToan_ChiTiet.Sum(x => x.fPheDuyetCapKyNay_VND);
                        }
                        entityThongTinThanhToan.fLuyKeUSD = TryParseDouble(data.TT_ThanToan.sLuyKeUSD);
                        entityThongTinThanhToan.fLuyKeVND = TryParseDouble(data.TT_ThanToan.sLuyKeVND);

                        conn.Insert(entityThongTinThanhToan, trans);

                        //Thực hiện thêm mới thông tin thanh toán chi tiết
                        if (data.lstTTThanToan_ChiTiet != null && data.lstTTThanToan_ChiTiet.Count() > 0)
                        {
                            for (int i = 0; i < data.lstTTThanToan_ChiTiet.Count(); i++)
                            {
                                var entityChiTiet = new NH_TT_ThanhToan_ChiTiet();
                                entityChiTiet.MapFrom(data.lstTTThanToan_ChiTiet.ToList()[i]);
                                entityChiTiet.iID_ThanhToanID = entityThongTinThanhToan.ID;
                                conn.Insert(entityChiTiet, trans);
                            }
                        }
                    }
                    else
                    {
                        //Thực hiện update đề nghị thanh toán

                        ThongTinThanhToanModel thanhtoan = _qlnhService.GetThongTinThanhToanByID(data.TT_ThanToan.ID);
                        entityThongTinThanhToan.MapFrom(data.TT_ThanToan);
                        entityThongTinThanhToan.sNguoiSua = Username;
                        entityThongTinThanhToan.dNgaySua = DateTime.Now;
                        if (data.lstTTThanToan_ChiTiet != null)
                        {
                            entityThongTinThanhToan.fTongDeNghi_USD = data.lstTTThanToan_ChiTiet.Sum(x => x.fDeNghiCapKyNay_USD);
                            entityThongTinThanhToan.fTongDeNghi_VND = data.lstTTThanToan_ChiTiet.Sum(x => x.fDeNghiCapKyNay_VND);
                            entityThongTinThanhToan.fTongPheDuyet_USD = data.lstTTThanToan_ChiTiet.Sum(x => x.fPheDuyetCapKyNay_USD);
                            entityThongTinThanhToan.fTongPheDuyet_VND = data.lstTTThanToan_ChiTiet.Sum(x => x.fPheDuyetCapKyNay_VND);
                        }
                        entityThongTinThanhToan.fLuyKeUSD = TryParseDouble(data.TT_ThanToan.sLuyKeUSD);
                        entityThongTinThanhToan.fLuyKeVND = TryParseDouble(data.TT_ThanToan.sLuyKeVND);
                        entityThongTinThanhToan.dNgayTao = thanhtoan.dNgayTao;
                        conn.Update(entityThongTinThanhToan, trans);
                        //Thực hiện update các thông tin thanh toán chi tiết
                        if (data.lstTTThanToan_ChiTiet != null && data.lstTTThanToan_ChiTiet.Count() > 0)
                        {
                            for (int i = 0; i < data.lstTTThanToan_ChiTiet.Count(); i++)
                            {
                                var entityChiTiet = new NH_TT_ThanhToan_ChiTiet();
                                entityChiTiet.MapFrom(data.lstTTThanToan_ChiTiet.ToList()[i]);
                                if (data.lstTTThanToan_ChiTiet.ToList()[i].ID == Guid.Empty)
                                {
                                    entityChiTiet.iID_ThanhToanID = entityThongTinThanhToan.ID;
                                    conn.Insert(entityChiTiet, trans);
                                }
                                else
                                {
                                    entityChiTiet.iID_ThanhToanID = entityThongTinThanhToan.ID;
                                    conn.Update(entityChiTiet, trans);
                                }

                            }
                        }
                        //Xóa các data đã bị xóa
                        string[] lsID = listIDChiTietXoa.Split(',');
                        for (int i = 0; i < lsID.Length; i++)
                        {
                            if (lsID[i] != "")
                            {
                                var entityXoa = _qlnhService.GetThongTinThanhToanChiTiet(Guid.Parse(lsID[i]));
                                conn.Delete(entityXoa, trans);
                            }
                        }
                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {

            }
            return true;
        }
        private double? TryParseDouble(string sGiaTri)
        {
            double fGiaTri;
            if (!double.TryParse(sGiaTri, NumberStyles.Any, CultureInfo.InvariantCulture, out fGiaTri))
            {
                return null;
            }
            else
            {
                return fGiaTri;
            }
        }

        [HttpPost]
        public JsonResult GetHopDongByID(Guid id)
        {
            var result = _qlnhService.GetThongTinHopDongById(id);
            return Json(new { result = result, status = true }, JsonRequestBehavior.AllowGet) ;
        }

        [HttpPost]
        public JsonResult GetDuAnByID(Guid id)
        {
            var result = _qlnhService.GetDuAnById(id);
            return Json(new { result = result, status = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LayThongTinLuyKe(Guid? id, Guid? idonvi, Guid? inhiemvuchi, Guid? ichudautu, Guid? ihopdong, Guid? iduan)
        {
            ThongTinThanhToanModel model = _qlnhService.GetThongTinThanhToanByID(id);
            DateTime? dngaytao = model == null ? DateTime.Now : model.dNgayTao;
            var thanhtoan = _qlnhService.GetThanhToanGanNhat(dngaytao.Value, idonvi, inhiemvuchi, ichudautu, ihopdong, iduan);
            return Json(new { thanhtoan = thanhtoan, status = true }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public bool DeleteDeNghiThanhToan(Guid id)
        {
            try
            {
                return _qlnhService.DeleteDeNghiThanhToan(id);
            }
            catch (Exception ex)
            {
                AppLog.LogError(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                return false;
            }
        }

        public ActionResult onThongBaoXoa(Guid id)
        {
            ThongTinThanhToanModel model = _qlnhService.GetThongTinThanhToanByID(id);
            return PartialView("_modalDelete", model);
        }

        [HttpPost]
        public ActionResult OnModalBaoCao(int type)
        {
            if (type == 1)
            {
                ViewBag.Title = "GIẤY ĐỀ NGHỊ THANH TOÁN KINH PHÍ TỪ NGUỒN DỰ TRỮ NGOẠI HỐI (CHI NGOẠI TỆ)";
            }
            if (type == 2)
            {
                ViewBag.Title = "ĐỀ NGHỊ THANH TOÁN KINH PHÍ TỪ NGUỒN QUỸ DỰ TRỮ NGOẠI HỐI (CHI TRONG NƯỚC)";
            }
            if (type == 3)
            {
                ViewBag.Title = "THÔNG BÁO CHI NGÂN SÁCH TỪ NGUỒN QUỸ DỰ TRỮ NGOẠI HỐI";
            }
            if (type == 4)
            {
                ViewBag.Title = "THÔNG BÁO CẤP KINH PHÍ BẰNG NGOẠI TỆ TỪ NGUỒN QUỸ DỰ TRỮ NGOẠI HỐI";
            }
            if (type == 5)
            {
                ViewBag.Title = "THÔNG BÁO CẤP KINH PHÍ CHI TRONG NƯỚC TỪ NGUỒN QUỸ DỰ TRỮ NGOẠI HỐI";
            }

            ViewBag.lstNSPhongBan = _qlnhService.GetAllNSPhongBan().ToSelectList("iID_MaPhongBan", "sMoTa");
            return PartialView("_viewbaocao");
        }

        public ActionResult ExportGiayDeNghiThanhToan(Guid? idThanhtoan, Guid? idPhongBan, string sNoiDung, int dvt = 1, int type = 1, string ext = "pdf")
        {
            ExcelFile xls = FileGiayDeNghiThanhToan(idThanhtoan, idPhongBan, sNoiDung, dvt, type);
            string sFileName = "Giấy đề nghị thanh toán";
            sFileName = string.Format("{0}.{1}", sFileName, ext);
            return Print(xls, ext, sFileName);
        }

        public ExcelFile FileGiayDeNghiThanhToan(Guid? idThanhtoan, Guid? idPhongBan, string sNoiDung = "", int dvt = 1, int type = 1)
        {
            XlsFile Result = new XlsFile(true);
            string sTiTle = "";
            if (type == 1)
            {
                sTiTle = "CHI NGOẠI TỆ";
                Result.Open(Server.MapPath(sFilePathGiayDeNghiThanhToanNgoaiTe));
            }
            else
            {
                sTiTle = "CHI TRONG NƯỚC";
                Result.Open(Server.MapPath(sFilePathGiayDeNghiThanhToanTrongNuoc));
            }

            FlexCelReport fr = new FlexCelReport();

            ThongTinThanhToanModel thanhtoan = _qlnhService.GetThongTinThanhToanByID(idThanhtoan);
            NS_PhongBan phongban = _qlnhService.GetAllNSPhongBan().Where(x => x.iID_MaPhongBan == idPhongBan).FirstOrDefault();

            NH_DA_HopDong hopdong = _qlnhService.GetThongTinHopDong(thanhtoan.iID_KHCTBQP_NhiemVuChiID.Value).Where(x => x.ID == thanhtoan.iID_HopDongID).FirstOrDefault();
            double? sDuToanDuocDuyetVND = hopdong == null ? null : hopdong.fGiaTriVND;
            double? sDuToanDuocDuyetUSD = hopdong == null ? null : hopdong.fGiaTriUSD;

            List<ThanhToanChiTietViewModel> lst = _qlnhService.GetThongTinThanhToanChiTietById(idThanhtoan).Select(x => new ThanhToanChiTietViewModel
            {
                STT = x.STT,
                ID = x.ID,
                iID_ThanhToanID = x.iID_ThanhToanID,
                sTenNoiDungChi = x.sTenNoiDungChi,
                fDeNghiCapKyNay_USD = x.fDeNghiCapKyNay_USD,
                fDeNghiCapKyNay_VND = x.fDeNghiCapKyNay_VND,
                fPheDuyetCapKyNay_USD = x.fPheDuyetCapKyNay_USD,
                fPheDuyetCapKyNay_VND = x.fPheDuyetCapKyNay_VND,
                sMucLucNganSach = x.sMucLucNganSach
            }).ToList();

            string sLoaiKinhPhi = "";
            if (thanhtoan.iLoaiDeNghi == 1)
            {
                sLoaiKinhPhi = "cấp kinh phí";
            }
            if (thanhtoan.iLoaiDeNghi == 2)
            {
                sLoaiKinhPhi = "thanh toán";
            }
            if (thanhtoan.iLoaiDeNghi == 3)
            {
                sLoaiKinhPhi = "tạm ứng";
            }

            double? fLuyKeVND = (lst.Count == 0) ? null : thanhtoan.fLuyKeVND;
            double? fLuyKeUSD = (lst.Count == 0) ? null : thanhtoan.fLuyKeUSD;
            sDuToanDuocDuyetVND = (lst.Count == 0) ? null : sDuToanDuocDuyetVND;
            sDuToanDuocDuyetUSD = (lst.Count == 0) ? null : sDuToanDuocDuyetUSD;

            fr.AddTable<ThanhToanChiTietViewModel>("dt", lst);
            fr.SetValue("sDuToanDuocDuyetVND", sDuToanDuocDuyetVND);
            fr.SetValue("sDuToanDuocDuyetUSD", sDuToanDuocDuyetUSD);
            fr.SetValue("sPhongBan", phongban.sTen);
            fr.SetValue("sTenChuongTrinh", thanhtoan.sTenNhiemVuChi);
            fr.SetValue("sTenCDT", thanhtoan.sTenCDT);
            fr.SetValue("sCanCu", thanhtoan.sCanCu);
            fr.SetValue("sDonViThuHuong", thanhtoan.sTenNhaThau);
            fr.SetValue("fBangSo", thanhtoan.fChuyenKhoan_BangSo.HasValue ? thanhtoan.fChuyenKhoan_BangSo.Value.ToString("###,###") : String.Empty);
            fr.SetValue("sBangChu", thanhtoan.sChuyenKhoan_BangChu);
            fr.SetValue("sNganHang", thanhtoan.sNganHang);
            fr.SetValue("sSoTaiKhoan", thanhtoan.sSoTaiKhoan);
            fr.SetValue("fTongCTDeNghiCapKyNay_USD", thanhtoan.fTongCTDeNghiCapKyNay_USD);
            fr.SetValue("fTongCTDeNghiCapKyNay_VND", thanhtoan.fTongCTDeNghiCapKyNay_VND);
            fr.SetValue("fTongCtPheDuyetCapKyNay_USD", thanhtoan.fTongCtPheDuyetCapKyNay_USD);
            fr.SetValue("fTongCTPheDuyetCapKyNay_VND", thanhtoan.fTongCTPheDuyetCapKyNay_VND);
            fr.SetValue("sTiTle", sTiTle);
            fr.SetValue("sNoiDung", sNoiDung);
            fr.SetValue("fLuyKeVND", fLuyKeVND);
            fr.SetValue("fLuyKeUSD", fLuyKeUSD);
            fr.SetValue("dvt", dvt);
            fr.SetValue("sLoaiKinhPhi", sLoaiKinhPhi);
            fr.SetValue(new
            {
                To = 1
            });

            fr.UseChuKy(Username)
               .UseChuKyForController(sControlName)
               .UseForm(this).Run(Result);

            return Result;
        }

        public ActionResult ExportThongBaoChiNganSach(Guid? idPhongBan, int nam, int thang = 1, int quy = 1, string ext = "pdf", string lstIdThanhToan = "", string sNoiDung = "", string sCanCu = "", int dvt = 1)
        {
            ExcelFile xls = FileThongBaoChiNganSach(idPhongBan, nam, thang, quy, lstIdThanhToan, sNoiDung, sCanCu, dvt);
            string sFileName = "Giấy đề nghị thanh toán";
            sFileName = string.Format("{0}.{1}", sFileName, ext);
            return Print(xls, ext, sFileName);
        }

        public ExcelFile FileThongBaoChiNganSach(Guid? idPhongBan, int? nam, int? thang, int? quy, string lstIdThanhToan, string sNoiDung, string sCanCu, int dvt)
        {
            XlsFile Result = new XlsFile(true);
            FlexCelReport fr = new FlexCelReport();
            Result.Open(Server.MapPath(sFilePathThongBaoChiNganSach));
            fr.SetValue(new
            {
                To = 1
            });

            List<ThanhToanBaoCaoModel> lst = _qlnhService.ExportBaoCaoChiThanhToan(lstIdThanhToan, thang.Value, quy.Value, nam.Value).Select(x => new ThanhToanBaoCaoModel
            {
                NoiDung = x.NoiDung,
                TongSo_VND = x.TongSo_VND,
                ChiNgoaiTen_USD = x.ChiNgoaiTen_USD,
                ChiNgoaiTe_VND = x.ChiNgoaiTe_VND,
                ChiTrongNuocVND = x.ChiTrongNuocVND,
                IsBold = x.IsBold,
                depth = x.depth,
                IDParent = x.IDParent,
                Position = x.Position,
                Muc = x.Muc
            }).ToList();
            decimal? fTongSo_VND = lst.Where(x => x.IDParent == null).Sum(x => x.TongSo_VND);
            decimal? fChiNgoaiTen_USD = lst.Where(x => x.IDParent == null).Sum(x => x.ChiNgoaiTen_USD);
            decimal? fChiNgoaiTe_VND = lst.Where(x => x.IDParent == null).Sum(x => x.ChiNgoaiTe_VND);
            decimal? fChiTrongNuocVND = lst.Where(x => x.IDParent == null).Sum(x => x.ChiTrongNuocVND);
            NS_PhongBan phongban = _qlnhService.GetAllNSPhongBan().Where(x => x.iID_MaPhongBan == idPhongBan).FirstOrDefault();
            fr.AddTable<ThanhToanBaoCaoModel>("dt", lst);
            fr.SetValue("fTongSo_VND", fTongSo_VND);
            fr.SetValue("fChiNgoaiTen_USD", fChiNgoaiTen_USD);
            fr.SetValue("fChiNgoaiTe_VND", fChiNgoaiTe_VND);
            fr.SetValue("fChiTrongNuocVND", fChiTrongNuocVND);
            fr.SetValue("thang", thang);
            fr.SetValue("nam", nam);
            fr.SetValue("sPhongBan", phongban.sTen);
            fr.SetValue("sCanCu", sCanCu);
            fr.SetValue("sNoiDung", sNoiDung);
            fr.SetValue("dvt", sNoiDung);

            fr.UseChuKy(Username)
              .UseChuKyForController(sControlName)
              .UseForm(this).Run(Result);
            return Result;
        }


        public ActionResult ExportThongBaoCapKinhPhi(string lstIdThanhToan, string tungay, string denngay, string sNoiDung, Guid? idquanly, int dvt = 1, string ext = "pdf", int type = 4)
        {
            ExcelFile xls = FileThongBaoCapKinhPhi(lstIdThanhToan, tungay, denngay, sNoiDung, idquanly, dvt, type);
            string sFileName = "Thông báo cấp kinh phí bằng ngoại tệ";
            sFileName = string.Format("{0}.{1}", sFileName, ext);
            return Print(xls, ext, sFileName);
        }

        public ExcelFile FileThongBaoCapKinhPhi(string lstIdThanhToan, string tungay, string denngay, string sNoiDung, Guid? idquanly, int dvt = 1, int type = 4)
        {
            XlsFile Result = new XlsFile(true);
            FlexCelReport fr = new FlexCelReport();
            if (type == 4)
            {
                Result.Open(Server.MapPath(sFilePathThongBaoCapKinhPhiNgoaiTe));
            }
            else
            {
                Result.Open(Server.MapPath(sFilePathThongBaoCapKinhPhiTrongNuoc));
            }

            DateTime dtungay = DateTime.Parse(tungay);
            DateTime ddenngay = DateTime.Parse(denngay);
            List<ThanhToanBaoCaoModel> lst = new List<ThanhToanBaoCaoModel>();
            decimal? fTongUSD = null;
            decimal? fTongVND = null;
            if (type == 4)
            {
                lst = _qlnhService.ExportBaoCaoThongBaoCapKinhPhiChiNgoaiTe(lstIdThanhToan, dtungay, ddenngay).Select(x => new ThanhToanBaoCaoModel
                {
                    STT = x.STT,
                    sTenNhaThau = x.sTenNhaThau,
                    sTenHopDong = x.sTenHopDong,
                    sNoiDungChi = x.sNoiDungChi,
                    fPheDuyetCapKyNay_USD = x.fPheDuyetCapKyNay_USD,
                    fTyGia = x.fTyGia,
                    fPheDuyetCapKyNay_VND = x.fPheDuyetCapKyNay_VND
                }).ToList();

                fTongUSD = lst.Sum(x => x.fPheDuyetCapKyNay_USD);
                fTongVND = lst.Sum(x => x.fPheDuyetCapKyNay_VND);
            }
            else
            {
                lst = _qlnhService.ExportBaoCaoThongBaoCapKinhPhiChiTrongNuoc(lstIdThanhToan, dtungay, ddenngay).Select(x => new ThanhToanBaoCaoModel
                {
                    STT = x.STT,
                    sTenNhaThau = x.sTenNhaThau,
                    sTenHopDong = x.sTenHopDong,
                    sNoiDungChi = x.sNoiDungChi,
                    fPheDuyetCapKyNay_VND = x.fPheDuyetCapKyNay_VND
                }).ToList();
                fTongVND = lst.Sum(x => x.fPheDuyetCapKyNay_VND);
            }

            NS_PhongBan phongban = _qlnhService.GetAllNSPhongBan().Where(x => x.iID_MaPhongBan == idquanly).FirstOrDefault();
            fr.AddTable<ThanhToanBaoCaoModel>("dt", lst);
            fr.SetValue("tungay", tungay);
            fr.SetValue("sPhongBan", phongban.sTen);
            fr.SetValue("denngay", denngay);
            fr.SetValue("sNoiDung", sNoiDung);
            fr.SetValue("dvt", dvt);
            fr.SetValue("fTongUSD", fTongUSD);
            fr.SetValue("fTongVND", fTongVND);
            fr.SetValue(new
            {
                To = 1
            });

            fr.UseChuKy(Username)
              .UseChuKyForController(sControlName)
              .UseForm(this).Run(Result);
            return Result;
        }
    }

}