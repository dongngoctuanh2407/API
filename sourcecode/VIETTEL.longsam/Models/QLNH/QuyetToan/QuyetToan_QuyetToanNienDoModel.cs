using System;
using System.Collections.Generic;
using Viettel.Domain.DomainModel;
using Viettel.Extensions;

namespace Viettel.Models.QLNH.QuyetToan
{
    public class QuyetToan_QuyetToanNienDoModel
    {
        public PagingInfo _paging = new PagingInfo() { ItemsPerPage = Constants.ITEMS_PER_PAGE };
        public IEnumerable<NH_QT_QuyetToanNienDoData> Items { get; set; }
    }
    public class Dropdown_QuyetToanNienDo
    {
        public int? Value { get; set; }
        public string Label { get; set; }
    }

    public partial class NH_QT_QuyetToanNienDo_ChiTietView
    {
        public virtual NH_QT_QuyetToanNienDoData QuyetToanNienDoDetail { get; set; }
        public virtual NH_KHChiTietBQP_NhiemVuChi NhiemVuChiDetail { get; set; }
        public virtual NH_DA_HopDong HopDongDetail { get; set; }
        public virtual List<NH_QT_QuyetToanNienDo_ChiTietData> ListDetailQuyetToanNienDo { get; set; }


    }
    public partial class NH_QT_QuyetToanNienDoData : NH_QT_QuyetToanNienDo
    {
        public virtual string sTenDonVi { get; set; }
        public virtual string sTenTiGia { get; set; }
        public string dNgayDeNghiStr
        {
            get
            {
                return dNgayDeNghi.HasValue ? dNgayDeNghi.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }

    }

    public partial class NH_QT_QuyetToanNienDoByDonVi 
    {
        public virtual NH_QT_QuyetToanNienDoData quyetToanNienDo { get; set; }
        public virtual NS_DonVi donVi { get; set; }

    }

    public partial class NH_QT_QuyetToanNienDo_ChiTietData : NH_QT_QuyetToanNienDo_ChiTiet
    {
        public virtual string sLNS { get; set; }
        public virtual string sL { get; set; }
        public virtual string sK { get; set; }
        public virtual string sM { get; set; }
        public virtual string sTM { get; set; }
        public virtual string sTTM { get; set; }
        public virtual string sTenHopDong { get; set; }
        public virtual string sTenDuAn { get; set; }
        public virtual string sTenNhiemVuChi { get; set; }
        public virtual string sTenNoiDungChi { get; set; }
        public virtual int? iLoaiNoiDungChi { get; set; }
        public virtual string STT { get; set; }
        public virtual bool? bIsTittle { get; set; } = false;
        public virtual float? fHopDong_USD_DuAn { get; set; }
        public virtual float? fHopDong_VND_DuAn { get; set; }
        public virtual float? fHopDong_USD_HopDong { get; set; }
        public virtual float? fHopDong_VND_HopDong { get; set; }
        public virtual Guid? iID_DonVi { get; set; }
        public virtual string sTenDonVi { get; set; }
        public virtual bool? bIsData { get; set; } = false;
        public virtual string sLevel { get; set; }
        public virtual int iCurrentId { get; set; }
        public virtual int iParentId { get; set; }
    }
    public partial class NH_QT_QuyetToanNienDoReturnData
    {
        public virtual NH_QT_QuyetToanNienDo QuyetToanNienDoData { get; set; }
        public virtual bool IsReturn { get; set; }
        public virtual string errorMess { get; set; }


    }
    public partial class NH_QT_QuyetToanNienDoChiTietReturnData
    {
        public virtual NH_QT_QuyetToanNienDo_ChiTiet QuyetToanNienDoChiTietData { get; set; }
        public virtual bool IsReturn { get; set; }

    }
}
