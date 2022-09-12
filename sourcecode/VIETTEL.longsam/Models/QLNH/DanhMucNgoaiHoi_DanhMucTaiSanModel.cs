using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viettel.Domain.DomainModel;
using Viettel.Extensions;

namespace Viettel.Models.QLNH
{
    public class  DanhMucNgoaiHoi_DanhMucTaiSanModelPaging
    {
        public PagingInfo _paging = new PagingInfo() { ItemsPerPage = Constants.ITEMS_PER_PAGE };
        public IEnumerable<DanhmucNgoaiHoi_TaiSanModel> Items { get; set; }
    }
    public class DanhmucNgoaiHoi_TaiSanModel : NH_DM_LoaiTaiSan
    {

    }

    public partial class NH_DM_LoaiTaiSanReturnData
    {
        public virtual NH_DM_LoaiTaiSan LoaiTaiSanData { get; set; }
        public virtual bool IsReturn { get; set; }
        public virtual string errorMess { get; set; }


    }
}
