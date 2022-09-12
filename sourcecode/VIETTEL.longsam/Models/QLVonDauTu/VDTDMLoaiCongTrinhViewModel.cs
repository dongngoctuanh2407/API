using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viettel.Models.QLVonDauTu
{
    public class VDTDMLoaiCongTrinhViewModel
    {
        public Guid iID_LoaiCongTrinh { get; set; }
        public Guid? iID_Parent { get; set; }
        public string sMaLoaiCongTrinh { get; set; }
        public string sTenLoaiCongTrinh { get; set; }
        public string sTenVietTat { get; set; }
        public int iThuTu { get; set; }
        public string sLevelTab { get; set; }
        public bool bHasChild { get; set; }
        public string sMoTa { get; set; }
        public string sTenLoaiCha { get; set; }
    }
}
