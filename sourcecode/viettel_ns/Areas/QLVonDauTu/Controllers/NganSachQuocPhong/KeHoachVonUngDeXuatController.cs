using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Viettel.Domain.DomainModel;
using Viettel.Models.QLVonDauTu;
using Viettel.Services;
using VIETTEL.Common;
using VIETTEL.Controllers;
using VIETTEL.Models;

namespace VIETTEL.Areas.QLVonDauTu.Controllers.NganSachQuocPhong
{
    public class KeHoachVonUngDeXuatController : AppController
    {
        private readonly IQLNguonNganSachService _qLNguonNSService = QLNguonNganSachService.Default;
        private readonly IDanhMucService _dmService = DanhMucService.Default;
        IQLVonDauTuService _qlVonDauTuService = QLVonDauTuService.Default;
        INganSachService _iNganSachService = NganSachService.Default;

        #region View
        // GET: QLVonDauTu/KeHoachVonUngDeXuat
        public ActionResult Index()
        {
            VDTKeHoachVonUngDeXuatPagingModel vm = new VDTKeHoachVonUngDeXuatPagingModel();
            vm._paging.CurrentPage = 1;

            ViewBag.ListDonViQuanLy = CreateSelectListDonVi();
            ViewBag.LstNguonVon = CommonFunction.GetDataDropDownNguonNganSach();

            List<VDTKeHoachVonUngDeXuatModel> lstKHVUDX = _qlVonDauTuService.GetKeHoachVonUngDeXuatByCondition(ref vm._paging).ToList();
            List<VDTKeHoachVonUngDeXuatModel> lstChungTuTongHopParent = lstKHVUDX.Where(x => !string.IsNullOrEmpty(x.sTongHop)).ToList();
            Dictionary<string, List<string>> dctChungTu = lstChungTuTongHopParent.GroupBy(x => x.Id).ToDictionary(x => x.Key.ToString(), x => x.Select(y => y.sTongHop).ToList());

            List<VDTKeHoachVonUngDeXuatModel> lstAllChungTuTongHopParent = new List<VDTKeHoachVonUngDeXuatModel>();
            foreach (var key in dctChungTu.Keys)
            {
                string lstValue = dctChungTu[key].ToList().FirstOrDefault();
                List<string> lstChildrentId = new List<string>();
                if (lstValue.Contains(","))
                {
                    lstChildrentId = lstValue.Split(',').ToList();
                }
                else
                {
                    lstChildrentId.Add(lstValue);
                }

                VDTKeHoachVonUngDeXuatModel itemParent = lstKHVUDX.Where(x => x.Id == Guid.Parse(key)).FirstOrDefault();
                List<VDTKeHoachVonUngDeXuatModel> lstChildrent = lstKHVUDX.Where(x => lstChildrentId.Any(y => Guid.Parse(y) == x.Id)).ToList();
                // lstChungTuTongHopParent = new List<VDT_KHV_KeHoachVonNam_DeXuat_ViewModel>();
                lstAllChungTuTongHopParent.Add(itemParent);
                lstAllChungTuTongHopParent.AddRange(lstChildrent);
            }
            List<VDTKeHoachVonUngDeXuatModel> lstChungTu = lstKHVUDX.Where(x => !lstAllChungTuTongHopParent.Any(y => y.Id == x.Id)).ToList();

            vm.chungTuTabIndex = "checked";
            vm.chungTuTongHopTabIndex = "";
            vm.lstData = lstChungTu;
            vm._paging.TotalItems = lstChungTu.Count();
            return View(vm);
        }

        public ActionResult Update(Guid? id = null, int? iNamKeHoach = null, int? iNguonVon = null, string lstTongHop = null)
        {
            ViewBag.ListDonViQuanLy = CreateSelectListDonVi();
            ViewBag.LstNguonVon = CommonFunction.GetDataDropDownNguonNganSach();
            ViewBag.lstIdTongHop = lstTongHop;
            VdtKhvuDXChiTietModel data = new VdtKhvuDXChiTietModel();
            if (!id.HasValue || id.Value == Guid.Empty)
            {
                ViewBag.Title = "Thêm mới kế hoạch vốn ứng đề xuất";
                data.dNgayDeNghi = DateTime.Now;

                if (lstTongHop != null && lstTongHop.Count() != 0)
                {
                    data.iNamKeHoach = iNamKeHoach;
                    data.iID_NguonVonID = iNguonVon;
                }
            }
            else
            {
                ViewBag.Title = "Cập nhật kế hoạch vốn ứng đề xuất";
                data = _qlVonDauTuService.GetKeHoachVonUngChiTietById(id.Value, PhienLamViec.NamLamViec);
                if (!string.IsNullOrEmpty(data.sTongHop))
                    ViewBag.lstIdTongHop = data.sTongHop;
            }
            
            return View(data);
        }

        public ActionResult Detail(Guid id)
        {
            VdtKhvuDXChiTietModel data = _qlVonDauTuService.GetKeHoachVonUngChiTietById(id, PhienLamViec.NamLamViec);
            return View(data);
        }
        #endregion

        #region Event
        [HttpGet]
        public JsonResult GetDonViQuanLy()
        {
            List<NS_DonVi> lstDonViQuanLy = _iNganSachService.GetDonviListByUser(Username, PhienLamViec.NamLamViec).ToList();
            if (lstDonViQuanLy == null || lstDonViQuanLy.Count == 0) return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            StringBuilder strDonVi = new StringBuilder();
            foreach (NS_DonVi item in lstDonViQuanLy)
            {
                strDonVi.AppendFormat("<option value='{0}' data-iIdDonVi='{1}'>{2}</option>", item.iID_MaDonVi, item.iID_Ma, item.sTen);
            }
            return Json(new { status = true, datas = strDonVi.ToString() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult TimKiemKHVUDeXuat(PagingInfo _paging, string sSoQuyetDinh, DateTime? dNgayQuyetDinhFrom,
            DateTime? dNgayQuyetDinhTo, int? iNamKeHoach, int? iIdNguonVon, string iID_DonViQuanLyID, int? tabIndex)
        {
            if (string.IsNullOrEmpty(sSoQuyetDinh))
                sSoQuyetDinh = null;
            if (string.IsNullOrEmpty(iID_DonViQuanLyID))
                iID_DonViQuanLyID = null;
            VDTKeHoachVonUngDeXuatPagingModel vm = new VDTKeHoachVonUngDeXuatPagingModel();
            vm._paging = _paging;

            ViewBag.ListDonViQuanLy = CreateSelectListDonVi();
            ViewBag.LstNguonVon = CommonFunction.GetDataDropDownNguonNganSach();

            List<VDTKeHoachVonUngDeXuatModel> lstKHVUDX = _qlVonDauTuService.GetKeHoachVonUngDeXuatByCondition
                (ref vm._paging, sSoQuyetDinh, dNgayQuyetDinhFrom, dNgayQuyetDinhTo, iNamKeHoach, iIdNguonVon, iID_DonViQuanLyID).ToList();
            List<VDTKeHoachVonUngDeXuatModel> lstChungTuTongHopParent = lstKHVUDX.Where(x => !string.IsNullOrEmpty(x.sTongHop)).ToList();
            Dictionary<string, List<string>> dctChungTu = lstChungTuTongHopParent.GroupBy(x => x.Id).ToDictionary(x => x.Key.ToString(), x => x.Select(y => y.sTongHop).ToList());

            List<VDTKeHoachVonUngDeXuatModel> lstAllChungTuTongHopParent = new List<VDTKeHoachVonUngDeXuatModel>();
            foreach (var key in dctChungTu.Keys)
            {
                string lstValue = dctChungTu[key].ToList().FirstOrDefault();
                List<string> lstChildrentId = new List<string>();
                if (lstValue.Contains(","))
                {
                    lstChildrentId = lstValue.Split(',').ToList();
                }
                else
                {
                    lstChildrentId.Add(lstValue);
                }

                VDTKeHoachVonUngDeXuatModel itemParent = lstKHVUDX.Where(x => x.Id == Guid.Parse(key)).FirstOrDefault();
                List<VDTKeHoachVonUngDeXuatModel> lstChildrent = lstKHVUDX.Where(x => lstChildrentId.Any(y => Guid.Parse(y) == x.Id)).ToList();
                // lstChungTuTongHopParent = new List<VDT_KHV_KeHoachVonNam_DeXuat_ViewModel>();
                lstAllChungTuTongHopParent.Add(itemParent);
                lstAllChungTuTongHopParent.AddRange(lstChildrent);
            }
            List<VDTKeHoachVonUngDeXuatModel> lstChungTu = lstKHVUDX.Where(x => !lstAllChungTuTongHopParent.Any(y => y.Id == x.Id)).ToList();

            if (tabIndex == null || tabIndex.Value == 1)
            {
                vm.chungTuTabIndex = "checked";
                vm.chungTuTongHopTabIndex = "";
                vm.lstData = lstChungTu;
            }
            else
            {
                vm.chungTuTabIndex = "";
                vm.chungTuTongHopTabIndex = "checked";
                vm.lstData = lstAllChungTuTongHopParent;
            }
            vm._paging.TotalItems = vm.lstData.Count();
            return PartialView("_list", vm);
        }

        [HttpPost]
        public bool Delete(Guid id)
        {
            if (!_qlVonDauTuService.deleteKHVUDXChiTiet(id)) return false;
            if (!_qlVonDauTuService.deleteKHVUDX(id)) return false;
            return true;
        }

        [HttpPost]
        public JsonResult GetKeHoachVonUngChiTiet(Guid id, bool bIsTongHop = false)
        {
            var data = _qlVonDauTuService.GetKeHoachVonUngDeXuatDetailById(id);
            if (data == null || !data.Any())
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (bIsTongHop)
                {
                    List<string> lstCbxDuAn =
                    data.Select(n => string.Format(@"<option value='{0}' data-fTongMucDauTu='{1}' data-sMaDuAn='{2}' data-sTenDuAn='{3}' data-sTrangThaiDuAnDangKy='{4}'>{2} - {3}</option>",
                    n.iID_DuAnID, n.fTongMucDauTu, n.sMaDuAn, n.sTenDuAn, n.sTrangThaiDuAnDangKy)).ToList();
                    return Json(new { status = true, lstDetail = data, sCbxDuAn = string.Join("", lstCbxDuAn) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = true, lstDetail = data }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public JsonResult CheckExistSoQuyetDinh(Guid? iID_KeHoachUngID, string sSoQuyetDinh)
        {
            var isExist = _qlVonDauTuService.CheckExistSoDeNghiKHVNDX(iID_KeHoachUngID, sSoQuyetDinh);
            return Json(new { status = isExist }, JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult QLKeHoachVonUngDxSave(VDT_KHV_KeHoachVonUng_DX data, List<VdtKhcKeHoachVonUngDeXuatChiTietModel> lstData, string id = null, bool isInsert = true)
        {
            using (var conn = ConnectionFactory.Default.GetConnection())
            {
                conn.Open();
                var trans = conn.BeginTransaction();
                if (data.Id == Guid.Empty)
                {
                    #region Them moi VDT_KHV_KeHoachVonUng_DeXuat
                    var entityKHVU = new VDT_KHV_KeHoachVonUng_DX();
                    entityKHVU.MapFrom(data);
                    
                    entityKHVU.sUserCreate = Username;
                    entityKHVU.dDateCreate = DateTime.Now;
                    entityKHVU.fGiaTriUng = lstData.Where(x => x.isDelete == false).Sum(n => n.fGiaTriDeNghi);
                    conn.Insert(entityKHVU, trans);
                    id = entityKHVU.Id.ToString();
                    #endregion
                    isInsert = true;
                    #region Them moi VDT_KHV_KeHoachVonUng_DeXuat_ChiTiet
                    if (lstData != null && lstData.Count() > 0)
                    {
                        for (int i = 0; i < lstData.Count(); i++)
                        {
                            var entityKHVUChiTiet = new VDT_KHV_KeHoachVonUng_DX_ChiTiet();
                            entityKHVUChiTiet.MapFrom(lstData.ToList()[i]);
                            if (lstData.ToList()[i].isDelete)
                                entityKHVUChiTiet.fGiaTriDeNghi = 0;
                            entityKHVUChiTiet.iID_KeHoachUngID = entityKHVU.Id;
                            conn.Insert(entityKHVUChiTiet, trans);
                        }
                    }

                    #endregion
                }
                else
                {
                    #region Sua KHVU
                    var entity = conn.Get<VDT_KHV_KeHoachVonUng_DX>(data.Id, trans);
                    entity.sSoDeNghi = data.sSoDeNghi;
                    entity.dNgayDeNghi = data.dNgayDeNghi;
                    entity.fGiaTriUng = data.fGiaTriUng;
                    entity.sUserUpdate = Username;
                    entity.dDateUpdate = DateTime.Now;
                    conn.Update(entity, trans);
                    #endregion

                    #region Them moi VDT_KHV_KeHoachVonUng_ChiTiet
                    //delete all KHVUChiTiet
                    //_qlVonDauTuService.deleteKHVUDXChiTiet(data.Id);
                    //insert new
                    if (lstData != null && lstData.Count() > 0)
                    {
                        for (int i = 0; i < lstData.Count(); i++)
                        {
                            var entityKHVUChiTiet = new VDT_KHV_KeHoachVonUng_DX_ChiTiet();
                            entityKHVUChiTiet.MapFrom(lstData.ToList()[i]);
                            if (lstData.ToList()[i].isDelete)
                                entityKHVUChiTiet.fGiaTriDeNghi = 0;
                            entityKHVUChiTiet.iID_KeHoachUngID = entity.Id;
                            conn.Insert(entityKHVUChiTiet, trans);
                        }
                    }
                    id = entity.Id.ToString();
                    isInsert = false;
                    #endregion
                }              
                // commit to db
                trans.Commit();
            }
            return Json(new { status = true, ID = id, isinsert = isInsert }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDuAnByCondition(string sMaDonVi = null, DateTime? dNgayDeNghi = null, string sTongHop = null)
        {
            if ((string.IsNullOrEmpty(sMaDonVi) || !dNgayDeNghi.HasValue))
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            var data = _qlVonDauTuService.GetDuAnInVonUngDeXuatByCondition(sMaDonVi, dNgayDeNghi, sTongHop);
            return Json(new { status = true, lstDuAn = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDuAnByChungTuTongHop(List<Guid> lstChungTuId)
        {
            List<VdtKhcKeHoachVonUngDeXuatChiTietModel> lstDuAn = new List<VdtKhcKeHoachVonUngDeXuatChiTietModel>();
            List<VdtKhvuDXChiTietModel> lstKhvuDX = new List<VdtKhvuDXChiTietModel>();
            foreach (var iId in lstChungTuId)
            {
                VdtKhvuDXChiTietModel objKhvu = _qlVonDauTuService.GetKeHoachVonUngChiTietById(iId, PhienLamViec.NamLamViec);
                if (objKhvu != null)
                {
                    lstKhvuDX.Add(objKhvu);
                    if (objKhvu.listKhvuChiTiet != null && objKhvu.listKhvuChiTiet.Any())
                        lstDuAn.AddRange(objKhvu.listKhvuChiTiet);
                }
            }
            return Json(new { status = true, lstKhvu = lstKhvuDX, lstDuAn = lstDuAn }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool KeHoachVonUngDeXuatLock(Guid id)
        {
            return _qlVonDauTuService.LockOrUnLockKeHoachVonUngDeXuat(id);
        }
        
        public SelectList CreateSelectListDonVi()
        {
            List<NS_DonVi> lstDonViQuanLy = _iNganSachService.GetDonviListByUser(Username, PhienLamViec.NamLamViec).ToList();
            List<SelectListItem> lstCbx = new List<SelectListItem>();
            foreach (NS_DonVi item in lstDonViQuanLy)
            {
                lstCbx.Add(new SelectListItem()
                {
                    Text = string.Format("{0} - {1}", item.iID_MaDonVi, item.sTen),
                    Value = string.Format("{0}", item.iID_MaDonVi),
                });
            }
            return lstCbx.ToSelectList();
        }
        #endregion
        
    }

}