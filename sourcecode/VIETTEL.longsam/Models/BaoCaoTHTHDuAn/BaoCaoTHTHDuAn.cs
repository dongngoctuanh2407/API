using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viettel.Domain.DomainModel;
using Viettel.Extensions;

namespace Viettel.Models.QLNH.BaoCaoTHTHDuAn
{
    public class BaoCaoTHTHDuAnMoDelPaing
    {
        public PagingInfo _paging = new PagingInfo() { ItemsPerPage = Constants.ITEMS_PER_PAGE };
        public IEnumerable<NH_TT_ThanhToanViewModel> Items { get; set; } = new List<NH_TT_ThanhToanViewModel>();
    }

    public class NH_TT_ThanhToanViewModelPaging
    {
        public PagingInfo _paging = new PagingInfo() { ItemsPerPage = Constants.ITEMS_PER_PAGE };
        public IEnumerable<NH_DA_DuAn> Items { get; set; }
    }

    public class NH_TT_ThanhToanViewModel : NH_TT_ThanhToan
    {
        public virtual string ChuDauTu { get; set; }
        public virtual string sTenNhaThau { get; set; }
        public virtual string DEPTH { get; set; }
    }
    public class NH_DA_DuAnViewModel : NH_DA_DuAn
    {
        public virtual string ChuDauTu { get; set; }
        public virtual string sTen { get; set; }
        public virtual string TongThoiGian { get; set; }

    }
    public class BaoCaoTinhHinhModel
    {
        public virtual NH_DA_DuAnViewModel DuAnModel { get; set; }
        public virtual BaoCaoTHTHDuAnMoDelPaing ListChiTiet { get; set; }
    }

    public class NH_TT_ThanhToanDto
    {
        public virtual IEnumerable<NH_TT_ThanhToanViewModel> Items { get; set; }
        public virtual double Sum { get; set; }
        public virtual double Sumgn { get; set; }
        public virtual int Stt { get; set; }
    }
}
