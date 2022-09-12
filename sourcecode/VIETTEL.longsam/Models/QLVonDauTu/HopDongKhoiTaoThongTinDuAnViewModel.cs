﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viettel.Domain.DomainModel;
using Viettel.Extensions;

namespace Viettel.Models.QLVonDauTu
{
    public class HopDongKhoiTaoThongTinDuAnViewModel
    {
        public PagingInfo _paging = new PagingInfo() { ItemsPerPage = Constants.ITEMS_PER_PAGE };
        public IEnumerable<VDT_KT_KhoiTao_DuLieu_ChiTiet_ThanhToan_ViewModel> Items { get; set; }
        public Guid iID_KhoiTaoDuLieuChiTietID { get; set; }
        public Guid iID_DuAnID {get;set;}
    }

    public class VDT_KT_KhoiTao_DuLieu_ChiTiet_ThanhToan_ViewModel : VDT_KT_KhoiTao_DuLieu_ChiTiet_ThanhToan
    {
        public int STT { get; set; }
        public string sTenHopDong { get; set; }
        public string sTenNhaThau { get; set; }
        public string sGiaTriHD { get; set; }

    }
}
