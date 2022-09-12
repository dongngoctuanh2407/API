using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viettel.Models.QLVonDauTu
{
    public class BcquyetToanNienDoVonUngChiTietViewModel
    {
        public Guid iID_DuAnID { get; set; }
        public string sMaDuAn { get; set; }
        public string sDiaDiem { get; set; }
        public string sTenDuAn { get; set; }
        // col 1
        public double fUngTruocChuaThuHoiNamTruoc { get; set; }
        // col 2
        public double fLuyKeThanhToanNamTruoc { get; set; }
        // col 3
        public double fKeHoachVonDuocKeoDai
        {
            get
            {
                return fUngTruocChuaThuHoiNamTruoc - fLuyKeThanhToanNamTruoc;
            }
        }
        // col 4
        public double fVonKeoDaiDaThanhToanNamNay { get; set; }
        // col 5
        public double fThuHoiVonNamNay { get; set; }
        // col 6
        public double fGiaTriThuHoiTheoGiaiNganThucTe { get; set; }
        // col 7
        public double fKHVUNamNay { get; set; }
        // col 8
        public double fVonDaThanhToanNamNay { get; set; }
        // col 9
        public double fKHVUChuaThuHoiChuyenNamSau
        {
            get
            {
                return fUngTruocChuaThuHoiNamTruoc - fThuHoiVonNamNay + fKHVUNamNay;
            }
        }
        // col 10
        public double fTongSoVonDaThanhToanThuHoi
        {
            get
            {
                return fLuyKeThanhToanNamTruoc + fVonKeoDaiDaThanhToanNamNay;
            }
        }
    }
}
