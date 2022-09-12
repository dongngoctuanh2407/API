using System;
using System.Collections.Generic;
using Viettel.Domain.DomainModel;
using Viettel.Extensions;

namespace Viettel.Models.QLVonDauTu
{
    public class VDTTongHopThongTinDuAnViewModel
    {
        public string stt { get; set; }
        public string sTienTe { get; set; }
        public Guid iID_DuAnID { get; set; }
        public string sMaDuAn { get; set; }
        public string sTenDuAn { get; set; }
        public Guid iID_DonViQuanLyID { get; set; }
        public string sTen { get; set; }

        //Phe duyet chu truong dau tu
        public string sSoQuyetDinhCTDT { get; set; }
        public DateTime? dNgayQuyetDinhCTDT { get; set; }
        public double? fGiaTriDauTu { get; set; }

        //Quyet dịnh dau tu
        public string sSoQuyetDinhQDDT { get; set; }
        public DateTime? dNgayQuyetDinhQDDT { get; set; }
        public double? fTongMucDauTu { get; set; }

        //Quyet toan du an
        public string sSoQuyetDinhQT { get; set; }
        public DateTime? dNgayQuyetDinhQT { get; set; }
        public double? fGiaTriQuyetToan { get; set; }

        //Ke hoach von
        public double? fLuyKeVonNamTruoc { get; set; }
        public double? fLuyKeVonNamNay { get; set; }
        public double? fKeHoachVonNamNay { get; set; }
        public double? fDaThanhToan { get; set; }
        public double? fChuaThanhToan { get; set; }

    }
}