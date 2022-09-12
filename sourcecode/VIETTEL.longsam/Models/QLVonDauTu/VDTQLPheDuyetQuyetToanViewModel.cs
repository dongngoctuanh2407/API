using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viettel.Domain.DomainModel;

namespace Viettel.Models.QLVonDauTu
{
    public class VDTQLPheDuyetQuyetToanViewModel
    {
        public Guid iID_DonViQuanLyID { get; set; }
        public Guid iID_QuyetToanID { get; set; }
        public Guid iID_DuAnID { get; set; }
        public string sSoQuyetDinh { get; set; }
        public string sNguoiLap { get; set; }
        public string sGhiChu { get; set; }
        public string sMaDuAn { get; set; }
        public string sTenDuAn { get; set; }
        public string sDiaDiem { get; set; }
        public string sKhoiCong { get; set; }
        public string sKetThuc { get; set; }
        public string sCoQuanPheDuyet { get; set; }
        public string sNguoiKy { get; set; }
        public string sNoiDung { get; set; }
        public DateTime? dNgayQuyetDinh { get; set; }
        public string sTenDonViQuanLy { get; set; }
        public double fTongMucDauTuPheDuyet { get; set; }
        public double fGiaTriUng { get; set; }
        public double fLKSoVonDaTamUng { get; set; }
        public double fLKThuHoiUng { get; set; }
        public double fConPhaiThuHoi { get; set; }
        public double fGiaTriThuHoiUngNgoaiChiTieu { get; set; }
        public double fTongTienTrungThau { get; set; }
        public double fTienQuyetToanPheDuyet { get; set; }

        //Noi dung quyet toan
        public IEnumerable<VDTNguonVonDauTuTableModel> arrNguonVon { get; set; }
        public double tongGiaTriPheDuyet { get; set; }

        //Noi dung quyet toan result view
        public IEnumerable<VDTNguonVonDauTuViewModel> lstNoiDungQuyetToan { get; set; }
        public double fTongGiaTriPhanBo { get; set; }
        public double fTongGiaTriChenhLech { get; set; }

    }

    public class VDTChiPhiDauTuModel
    {
        public Guid iID_QuyetToan_ChiPhiID { get; set; }
        public Guid iID_QuyetToanID { get; set; }
        public Guid iID_ChiPhiID { get; set; }
        public double fTienPheDuyet { get; set; }
        public string sTenChiPhi { get; set; }
    }

    public class VDTNguonVonDauTuModel
    {
        public Guid iID_QuyetToan_NguonVonID { get; set; }
        public Guid iID_QuyetToanID { get; set; }
        public int iID_NguonVonID { get; set; }
        public double fTienPheDuyet { get; set; }
        public string sTen { get; set; }
    }

    public class VDTNguonVonDauTuTableModel
    {
        public int iID_NguonVonID { get; set; }
        public double fTienPheDuyet { get; set; }
    }

    public class VDTNguonVonDauTuViewModel
    {
        public int iID_MaNguonNganSach { get; set; }
        public string sTen { get; set; }
        public double fGiaTriDuToan { get; set; }
        public double fGiaTriQuyetToan { get; set; }
        public double fGiaTriChenhLech { get; set; }
        public string sChenhLech { get; set; }
    }

    public class VDTQLPheDuyetQuyetToanModel
    {
        public VDTQLPheDuyetQuyetToanViewModel quyetToan { get; set; }
        public IEnumerable<VDTChiPhiDauTuModel> listQuyetToanChiPhi { get; set; }
        public IEnumerable<VDTNguonVonDauTuModel> listQuyetToanNguonVon { get; set; }
        public IEnumerable<VDTNguonVonDauTuViewModel> listNguonVonChenhLech { get; set; }

        public VDTQLPheDuyetQuyetToanViewModel dataDuAnQT { get; set; }
    }
}
