using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VIETTEL.Controllers;
using Viettel.Domain.DomainModel;
using Viettel.Models.QLNH;
using Viettel.Models.QLVonDauTu;
using Viettel.Services;

namespace VIETTEL.Areas.QLNH.Controllers.QuyetToan
{
    public class BaoCaoTaiSanController : AppController
    {
        private readonly IQLNHService _qlnhService = QLNHService.Default;

        public ActionResult Index()
        {
            BaoCaoTaiSanModelViewModel vm = new BaoCaoTaiSanModelViewModel();

            vm._paging.CurrentPage = 1;
            
            vm.Items = _qlnhService.getListBaoCaoTaiSanModels(ref vm._paging, null);

            vm.Items2 = _qlnhService.getListBaoCaoTaiSanModelstb2(ref vm._paging, null);

            ViewBag.ChangeTable =  true;
            List<NS_DonViModel> lstDonVi = _qlnhService.GetLookupDonViTaiSan().ToList();
            lstDonVi.Insert(0, new NS_DonViModel { iID_Ma = Guid.Empty, sDonVi = "--Chọn--" });
            vm.ListDonVi = lstDonVi;

            List<NH_DA_DuAn> lstDuAn = _qlnhService.GetLookupDuAnTaiSan().ToList();
            lstDuAn.Insert(0, new NH_DA_DuAn { ID = Guid.Empty, sTenDuAn = "--Chọn--" });
            vm.ListDuAn = lstDuAn;

            List<NH_DA_HopDong> lstHopDong = _qlnhService.GetLookupHopDongTaiSan().ToList();
            lstHopDong.Insert(0, new NH_DA_HopDong { ID = Guid.Empty, sTenHopDong = "--Chọn--" });
            vm.ListHopDong = lstHopDong;

            return View(vm);
        }
        [HttpPost]
        public ActionResult TimKiem(PagingInfo _paging, Guid? iID_DonViID = null, Guid? iID_DuAnID = null, Guid? iID_HopDongID = null)
        {
            BaoCaoTaiSanModelViewModel vm = new BaoCaoTaiSanModelViewModel();

            vm._paging = _paging;

            vm.Items = _qlnhService.getListBaoCaoTaiSanModels(ref vm._paging, iID_DonViID, iID_DuAnID, iID_HopDongID);

            vm.Items2 = _qlnhService.getListBaoCaoTaiSanModelstb2(ref vm._paging, iID_DonViID, iID_DuAnID, iID_HopDongID);

            List<NS_DonViModel> lstDonVi = _qlnhService.GetLookupDonViTaiSan().ToList();
            lstDonVi.Insert(0, new NS_DonViModel { iID_Ma = Guid.Empty, sDonVi = "--Chọn--" });
            vm.ListDonVi = lstDonVi;

            List<NH_DA_DuAn> lstDuAn = _qlnhService.GetLookupDuAnTaiSan().ToList();
            lstDuAn.Insert(0, new NH_DA_DuAn { ID = Guid.Empty, sTenDuAn = "--Chọn--" });
            vm.ListDuAn = lstDuAn;

            List<NH_DA_HopDong> lstHopDong = _qlnhService.GetLookupHopDongTaiSan().ToList();
            lstHopDong.Insert(0, new NH_DA_HopDong { ID = Guid.Empty, sTenHopDong = "--Chọn--" });
            vm.ListHopDong = lstHopDong;

            return PartialView("_list", vm);
        }

    }
}