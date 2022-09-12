using DomainModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viettel.Domain.DomainModel;
using Viettel.Extensions;

namespace Viettel.Models.CPNH
{
    public class CPNHNhuCauChiQuy_ModelPaging
    {
        public PagingInfo _paging = new PagingInfo() { ItemsPerPage = Constants.ITEMS_PER_PAGE };
        public IEnumerable<CPNHNhuCauChiQuy_Model> Items { get; set; }
    }

    public class CPNHNhuCauChiQuy_Model : NH_NhuCauChiQuy
    {
        public string dNgayDeNghiStr
        {
            get
            {
                return dNgayDeNghi.HasValue ? dNgayDeNghi.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }
        public string BPhongBan { get; set; }
        public string BQuanLy { get; set; }
        public string sDVTinh { get; set; }
        public int iDVTinh { get; set; }
        public string sTenTiGia { get; set; }
        public string SQuyTypes { get; set; }
        public float fTiGiaChiTiet { get; set; }
        public string sTongChiNgoaiTeUSD_TongHop 
        {
            get
            {
                return fTongChiNgoaiTeUSD.HasValue ?CommonFunction.DinhDangSo(fTongChiNgoaiTeUSD.Value.ToString(CultureInfo.InvariantCulture), 2) : string.Empty;
            }
        
        }
        public string sTongChiTrongNuocVND_TongHop
        {
            get
            {
                return fTongChiNgoaiTeVND.HasValue ? CommonFunction.DinhDangSo(fTongChiNgoaiTeVND.Value.ToString(CultureInfo.InvariantCulture), 2) : string.Empty;
            }

        }
        public string depth { get; set; }
        public int STT { get; set; }
        public bool isChecked { get; set; }
        public string sSTT { get; set; }
        public string sQuyNam { get; set; }

    }
    public class CPNHNhuCauChiQuy_Create_Model : NH_NhuCauChiQuy
    {
        public string sTongChiNgoaiTeUSD { get; set; }
        public string sTongChiNgoaiTeVND { get; set; }
        public string sTongChiTrongNuocVND { get; set; }
        public string sTongChiTrongNuocUSD { get; set; }
        public IEnumerable<CPNHNhuCauChiQuy_ChiTiet_Model> ListNCCQChiTiet { get; set; }
        //public string sDonViQuanLy { get; set; }
    }
    public class CPNHNhuCauChiQuy_View_Model : CPNHNhuCauChiQuy_Model
    {
        public string sTongChiNgoaiTeUSD { get; set; }
        public string sTongChiNgoaiTeVND { get; set; }
        public string sTongChiTrongNuocVND { get; set; }
        public string sTongChiTrongNuocUSD { get; set; }
        public IEnumerable<CPNHNhuCauChiQuy_ChiTiet_Model> ListNCCQChiTiet { get; set; }
        //public string sDonViQuanLy { get; set; }
    }
    public class Dropdown_ByYear_ThucHienNganSach
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
    public class CPNHThucHienNganSach_ModelPaging
    {
        public IEnumerable<CPNHThucHienNganSach_Model> Items { get; set; }
    }
    public class ThucHienNganSach_GiaiDoan_Model: NH_KHTongTheTTCP
    {
        public Guid ID { get; set; }
        public string sGiaiDoan { get; set; }
        public double? valueUSD { get; set; } = 0;
        public double? valueVND { get; set; } = 0;
    }
    public class CPNHThucHienNganSach_Model : NH_TT_ThanhToan_ChiTiet
    {
        public Guid IDNhiemVuChi { get; set; }
        public Guid IDDuAn { get; set; }
        public Guid IDHopDong { get; set; }
        public Guid iID_DonVi { get; set; }
        public int iNamKeHoach { get; set; }
        public int iGiaiDoanDen { get; set; }
        public int iGiaiDoanTu { get; set; }
        public int iLoaiNoiDungChi { get; set; } = 0;
        public double? HopDongUSD { get; set; } = 0;
        public double? HopDongVND { get; set; } = 0;
        public double? DuAnUSD { get; set; } = 0;
        public double? DuAnVND { get; set; } = 0;
        public double? NCVTTCP { get; set; } = 0;
        public double? NhiemVuChi { get; set; } = 0;
        public double? KinhPhiUSD { get; set; } = 0;
        public double? TongKinhPhiUSD { get; set; } = 0;
        public double? SumTongKinhPhiUSD { get; set; }
        public double? KinhPhiVND { get; set; } = 0;
        public double? TongKinhPhiVND { get; set; } = 0;
        public double? SumTongKinhPhiVND { get; set; }
        public double? KinhPhiToYUSD { get; set; } = 0;
        public double? KinhPhiToYVND { get; set; } = 0;
        public double? KinhPhiDaChiUSD { get; set; } = 0;
        public double? KinhPhiDaChiVND { get; set; } = 0;
        public double? KinhPhiDaChiToYUSD { get; set; } = 0;
        public double? KinhPhiDaChiToYVND { get; set; } = 0;
        public double? TongKinhPhiDaChiUSD
        {
            get
            {
                return KinhPhiDaChiUSD + KinhPhiDaChiToYUSD;
            }
        }
        public double? SumTongKinhPhiDaChiUSD { get; set; }
        public double? TongKinhPhiDaChiVND
        {
            get
            {
                return KinhPhiDaChiVND + KinhPhiDaChiToYVND;
            }
        }
        public double? SumTongKinhPhiDaChiVND { get; set; }
        public string sTenNhiemVuChi { get; set; }
        public string sTenDuAn { get; set; }
        public string sTenHopDong { get; set; }
        public double? KinhPhiDuocCapChuaChiUSD { get; set; }
        public double? KinhPhiDuocCapChuaChiVND { get; set; }
        public double? QuyGiaiNganTheoQuy
        {
            get
            {
                return NhiemVuChi - TongKinhPhiUSD;
            }
        }
        public double? fLuyKeKinhPhiDuocCap_USD { get; set; } = 0;
        public double? fLuyKeKinhPhiDuocCap_VND { get; set; } = 0;
        public double? fDeNghiQTNamNay_USD { get; set; } = 0;
        public double? fDeNghiQTNamNay_VND { get; set; } = 0;
        public double? KinhPhiChuaQuyetToanUSD
        {
            get
            {
                return fLuyKeKinhPhiDuocCap_USD - fDeNghiQTNamNay_USD;
            }
        }
        public double? KinhPhiChuaQuyetToanVND
        {
            get
            {
                return fLuyKeKinhPhiDuocCap_VND - fDeNghiQTNamNay_VND;
            }
        }
        public double? KeHoachGiaiNgan
        {
            get
            {
                return NCVTTCP - fLuyKeKinhPhiDuocCap_USD;
            }
        }
        public double? SumKinhPhiChuaQuyetToanUSD { get; set; }
        public double? SumKinhPhiChuaQuyetToanVND { get; set; }
        public double? SumKeHoachGiaiNgan { get; set; }
        public Boolean? isSum { get; set; }
        public string isTitle { get; set; }
        public Boolean? isHopDong { get; set; }
        public Boolean? isDuAn { get; set; }
        public string sTenCDT { get; set; }
        public string sTenDonVi { get; set; }
        public string depth { get; set; }
        public List<ThucHienNganSach_GiaiDoan_Model> lstGiaiDoanTTCP { get; set; }
        public List<ThucHienNganSach_GiaiDoan_Model> lstGiaiDoanKinhPhiDaGiaiNgan { get; set; }
        public List<ThucHienNganSach_GiaiDoan_Model> lstGiaiDoanKinhPhiDuocCap { get; set; }

    }

}
