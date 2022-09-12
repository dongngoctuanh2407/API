using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viettel.Domain.DomainModel;
using Viettel.Extensions;


namespace Viettel.Models.QLNH
{
    public class ThongTinThanhToanPagingViewModel
    {
        public PagingInfo _paging = new PagingInfo() { ItemsPerPage = Constants.ITEMS_PER_PAGE };
        public IEnumerable<ThongTinThanhToanModel> Items { get; set; }
    }
    public class ThongTinThanhToanModel : NH_TT_ThanhToan
    {
        public string sTen { get; set; }
        public string sTenCDT { get; set; }
        public string sTenNhiemVuChi { get; set; }
        public string sTenNhaThau { get; set; }
        public decimal? fTongCTDeNghiCapKyNay_USD { get; set; }
        public decimal? fTongCTDeNghiCapKyNay_VND { get; set; }
        public decimal? fTongCtPheDuyetCapKyNay_USD { get; set; }
        public decimal? fTongCTPheDuyetCapKyNay_VND { get; set; }
        public string sTenDuAn { get; set; }
        public string sTenHopDong { get; set; }
        public string sTiGiaDeNghi { get; set; }
        public string sTiGiaPheDuyet { get; set; }
        public string sTongCTDeNghiCapKyNay_USD {
            get { return fTongCTDeNghiCapKyNay_USD != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(fTongCTDeNghiCapKyNay_USD.Value, 2).ToString(CultureInfo.InvariantCulture), 2) : String.Empty; }
        }
        public string sTongCTDeNghiCapKyNay_VND {
            get { return fTongCTDeNghiCapKyNay_VND != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(fTongCTDeNghiCapKyNay_VND.Value, 0).ToString(CultureInfo.InvariantCulture), 0) : String.Empty; }
        }
        public string sTongCtPheDuyetCapKyNay_USD {
            get { return fTongCtPheDuyetCapKyNay_USD != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(fTongCtPheDuyetCapKyNay_USD.Value, 2).ToString(CultureInfo.InvariantCulture), 2) : String.Empty; }
        }
        public string sTongCTPheDuyetCapKyNay_VND {
            get { return fTongCTPheDuyetCapKyNay_VND != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(fTongCTPheDuyetCapKyNay_VND.Value, 0).ToString(CultureInfo.InvariantCulture), 0) : String.Empty; }
        }
        public string sHopDongPheDuyet_USD { get; set; }
        public string sHopDongPheDuyet_VND { get; set; }
        public string sDuToanPheDuyet_USD { get; set; }
        public string sDuToanPheDuyet_VND { get; set; }
        public string sLuyKe_USD
        {
            get { return fLuyKeUSD != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(fLuyKeUSD.Value, 2).ToString(CultureInfo.InvariantCulture), 2) : String.Empty; }
        }
        public string sLuyKe_VND
        {
            get { return fLuyKeVND != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(fLuyKeVND.Value, 0).ToString(CultureInfo.InvariantCulture), 0) : String.Empty; }
        }

        public string _sChuyenKhoan_BangSo
        {
            get { return fChuyenKhoan_BangSo != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(fChuyenKhoan_BangSo.Value, 0).ToString(CultureInfo.InvariantCulture), 0) : String.Empty; }
        }
    }

    public class ThongTinThanhToanSearchModel
    {
        public PagingInfo _paging { get; set; }
        public Guid? iID_DonVi { get; set; }
        public string sSoDeNghi { get; set; }
        public DateTime? dNgayDeNghi { get; set; }
        public int? iLoaiNoiDungChi { get; set; }
        public Guid? iID_ChuDauTuID { get; set; }
        public Guid? iID_KHCTBQP_NhiemVuChiID { get; set; }
        public int? iNamKeHoach { get; set; }
        public int? iNamNganSach { get; set; }
        public int? iCoQuanThanhToan { get; set; }
        public Guid? iID_NhaThauID { get; set; }
        public int? iTrangThai { get; set; }

    }

    public class ThongTinThanhToanDetaiModel
    {
        public ThongTinThanhToanModel thongtinthanhtoan { get; set; }
        public IEnumerable<ThanhToanChiTietViewModel> thanhtoan_chitiet { get; set; }
    }

    public class ThanhToanChiTietViewModel : NH_TT_ThanhToan_ChiTiet
    {
        public string sMucLucNganSach { get; set; }
        public int STT { get; set; }
        public string sDeNghiCapKyNay_USD { get; set; }
        public string sDeNghiCapKyNay_VND { get; set; }
        public string sPheDuyetCapKyNay_USD { get; set; }
        public string sPheDuyetCapKyNay_VND { get; set; }

        public string _sDeNghiCapKyNay_USD
        {
            get { return fDeNghiCapKyNay_USD != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(fDeNghiCapKyNay_USD.Value,2).ToString(CultureInfo.InvariantCulture), 2) : String.Empty; }
        }
        public string _sDeNghiCapKyNay_VND
        {
            get { return fDeNghiCapKyNay_VND != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(fDeNghiCapKyNay_VND.Value, 0).ToString(CultureInfo.InvariantCulture), 0) : String.Empty; }
        }
        public string _sPheDuyetCapKyNay_USD
        {
            get { return fPheDuyetCapKyNay_USD != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(fPheDuyetCapKyNay_USD.Value, 2).ToString(CultureInfo.InvariantCulture), 2) : String.Empty; }
        }
        public string _sPheDuyetCapKyNay_VND
        {
            get { return fPheDuyetCapKyNay_VND != null ? DomainModel.CommonFunction.DinhDangSo(Math.Round(fPheDuyetCapKyNay_VND.Value, 0).ToString(CultureInfo.InvariantCulture), 0) : String.Empty; }
        }


    }

    public class ThanhToanBaoCaoModel
    {
        public int STT { get; set; }
        public decimal? TongSo_VND { get; set; }
        public decimal? ChiNgoaiTen_USD { get; set; }
        public decimal? ChiNgoaiTe_VND { get; set; }
        public decimal? ChiTrongNuocVND { get; set; }
        public int IsBold { get; set; }
        public int depth { get; set; }
        public string Muc { get; set; }
        public Guid? IDParent { get; set; }
        public string NoiDung { get; set; }

        public string Position { get; set; }

        /*Model Báo cáo BM05 -BM06*/
        public string sTenNhaThau { get; set; }
        public string sTenHopDong { get; set; }
        public string sNoiDungChi { get; set; }
        public decimal? fPheDuyetCapKyNay_USD { get; set; }
        public float? fTyGia { get; set; }
        public decimal? fPheDuyetCapKyNay_VND { get; set; }

    }
    public class NS_DonViViewModel: NS_DonVi
    {
        public string sTenDonVi
        {
            get { return iID_MaDonVi + '-' + sTen; }
        }
    }

    public class DM_ChuDauTuViewModel : DM_ChuDauTu
    {
        public string sTenChuDauTu
        {
            get { return sId_CDT + '-' + sTenCDT; }
        }
    }

}