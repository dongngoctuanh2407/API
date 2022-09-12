using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viettel.Domain.DomainModel;
using Viettel.Extensions;

namespace Viettel.Models.QLVonDauTu
{
    public class VDTQuanLyThongTriThanhToanModel
    {
    }

    public class VDTThongTriViewModel
    {
        public PagingInfo _paging = new PagingInfo() { ItemsPerPage = Constants.ITEMS_PER_PAGE };
        public IEnumerable<VDTThongTriModel> Items { get; set; }
    }

    public class VDTThongTriModel : VDT_ThongTri
    {
        public string sTenDonVi { get; set; }
        public string sTenNguonNganSach { get; set; }
        public DateTime? dNgayLapGanNhat { get; set; }
    }

    public class VDTTTDeNghiThanhToanChiTiet: VDT_TT_DeNghiThanhToan_ChiTiet
    {
        public string sLevelTab { get; set; }
        public bool bHasChild { get; set; }
        public Guid? iID_Parent { get; set; }
        public string sM { get; set; }
        public string sTM { get; set; }
        public string sTTM { get; set; }
        public string sNG { get; set; }
        public string sNoiDung { get; set; }
        public Guid? iID_LoaiCongTrinhID { get; set; }
        public string sTenLoaiCongTrinh { get; set; }
        public double? fGiaTriThuHoiUngNgoaiChiTieu { get; set; }
    }

    public class VDTThongTriChiTiet: VDT_ThongTri_ChiTiet
    {
        public string sLevelTab { get; set; }
        public bool bHasChild { get; set; }
        public Guid? iID_Parent { get; set; }
        public string sM { get; set; }
        public string sTM { get; set; }
        public string sTTM { get; set; }
        public string sNG { get; set; }
        public string sNoiDung { get; set; }
        public string sTenLoaiCongTrinh { get; set; }
    }
}
