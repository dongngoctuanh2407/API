using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viettel.Domain.DomainModel;

namespace Viettel.Models.QLVonDauTu
{
    public class VDT_QT_DeNghiQuyetToanViewModel : VDT_QT_DeNghiQuyetToan
    {
        public int STT { get; set; }
        public string sMaDuAn { get; set; }
        public string sTenDuAn { get; set; }
        public string sTenChuDauTu { get; set; }
        public string dThoiGianKhoiCongAsString { get; set; }
        public string dThoiGianHoanThanhAsString { get; set; }
        public List<VDTDuToanNguonVonModel> listNguonVon { get; set; }
        public List<VDT_DA_DuToan_ChiPhi_ViewModel> listChiPhi { get; set; }
        public List<VDT_DA_DuToan_HangMuc_ViewModel> listHangMuc { get; set; }
        public string sGiaTriDeNghiQuyetToan
        {
            get
            {
                return fGiaTriDeNghiQuyetToan.HasValue ? fGiaTriDeNghiQuyetToan.Value.ToString("##,#", CultureInfo.GetCultureInfo("vi-VN")) : "";
            }
        }
        public string sChiPhiThietHai
        {
            get
            {
                return fChiPhiThietHai.HasValue ? fChiPhiThietHai.Value.ToString("##,#", CultureInfo.GetCultureInfo("vi-VN")) : "";
            }
        }
        public string sChiPhiKhongTaoNenTaiSan
        {
            get
            {
                return fChiPhiKhongTaoNenTaiSan.HasValue ? fChiPhiKhongTaoNenTaiSan.Value.ToString("##,#", CultureInfo.GetCultureInfo("vi-VN")) : "";
            }
        }
        public string sTaiSanDaiHanThuocCDTQuanLy
        {
            get
            {
                return fTaiSanDaiHanThuocCDTQuanLy.HasValue ? fTaiSanDaiHanThuocCDTQuanLy.Value.ToString("##,#", CultureInfo.GetCultureInfo("vi-VN")) : "";
            }
        }
        public string sTaiSanDaiHanDonViKhacQuanLy
        {
            get
            {
                return fTaiSanDaiHanDonViKhacQuanLy.HasValue ? fTaiSanDaiHanDonViKhacQuanLy.Value.ToString("##,#", CultureInfo.GetCultureInfo("vi-VN")) : "";
            }
        }
        public string sTaiSanNganHanThuocCDTQuanLy
        {
            get
            {
                return fTaiSanNganHanThuocCDTQuanLy.HasValue ? fTaiSanNganHanThuocCDTQuanLy.Value.ToString("##,#", CultureInfo.GetCultureInfo("vi-VN")) : "";
            }
        }
        public string sTaiSanNganHanDonViKhacQuanLy
        {
            get
            {
                return fTaiSanNganHanDonViKhacQuanLy.HasValue ? fTaiSanNganHanDonViKhacQuanLy.Value.ToString("##,#", CultureInfo.GetCultureInfo("vi-VN")) : "";
            }
        }
        public string sThoiGianLapBaoCao
        {
            get
            {
                return dThoiGianLapBaoCao.HasValue ? dThoiGianLapBaoCao.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }
        public string sThoiGianKhoiCong
        {
            get
            {
                return dThoiGianKhoiCong.HasValue ? dThoiGianKhoiCong.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }
        public string sThoiGianHoanThanh
        {
            get
            {
                return dThoiGianHoanThanh.HasValue ? dThoiGianHoanThanh.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }
    }

    public class VDTDuToanNguonVonModel: VDT_DA_DuToan_Nguonvon
    {
        public string sTenNguonVon { get; set; }
        public string sTienPheDuyet
        {
            get
            {
                return fTienPheDuyet.HasValue ? fTienPheDuyet.Value.ToString("##,#", CultureInfo.GetCultureInfo("vi-VN")) : "";
            }
        }
        public double? fTienCDTQuyetToan { get; set; }
        public string sTienCDTQuyetToan
        {
            get
            {
                return fTienCDTQuyetToan.HasValue ? fTienCDTQuyetToan.Value.ToString("##,#", CultureInfo.GetCultureInfo("vi-VN")) : "";
            }
        }
        public string sTienToTrinh
        {
            get
            {
                return fTienToTrinh.HasValue ? fTienToTrinh.Value.ToString("##,#", CultureInfo.GetCultureInfo("vi-VN")) : "";
            }
        }
    }
}
