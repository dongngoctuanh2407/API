using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viettel.Domain.DomainModel;
using Viettel.Extensions;

namespace Viettel.Models.QLNH
{
    public class DanhmucNgoaiHoi_LoaiHopDongModelPaging
    {
        public PagingInfo _paging = new PagingInfo() { ItemsPerPage = Constants.ITEMS_PER_PAGE };
        public IEnumerable<DanhmucNgoaiHoi_LoaiHopDongModel> Items { get; set; }
    }
    public class DanhmucNgoaiHoi_LoaiHopDongModel : NH_DM_LoaiHopDong
    {
       
    }
    public partial class NH_DM_LoaiHopDongReturnData
    {
        public virtual NH_DM_LoaiHopDong LoaiHopDongData { get; set; }
        public virtual bool IsReturn { get; set; }
        public virtual string errorMess { get; set; }


    }
}
