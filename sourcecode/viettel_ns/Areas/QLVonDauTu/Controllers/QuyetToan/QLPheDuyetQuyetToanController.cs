using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Viettel.Domain.DomainModel;
using Viettel.Models.QLVonDauTu;
using Viettel.Services;
using VIETTEL.Common;
using VIETTEL.Controllers;

namespace VIETTEL.Areas.QLVonDauTu.Controllers.QuyetToan
{
    public class QLPheDuyetQuyetToanController : AppController
    {
        private readonly IQLNguonNganSachService _qLNguonNSService = QLNguonNganSachService.Default;
        private readonly IDanhMucService _dmService = DanhMucService.Default;
        IQLVonDauTuService _iQLVonDauTuService = QLVonDauTuService.Default;
        INganSachService _iNganSachService = NganSachService.Default;
        // GET: QLVonDauTu/QLPheDuyetQuyetToan
        public ActionResult Index()
        {
            ViewBag.ListChuDauTu = _iQLVonDauTuService.LayChuDauTu(PhienLamViec.NamLamViec).ToSelectList("iID_Ma", "sTen");
            ViewBag.ListDonViQuanLy = _iNganSachService.GetDonviListByUser(Username, PhienLamViec.NamLamViec).ToSelectList("iID_Ma", "sTen");

            VDTPheDuyetQuyetToanPagingModel dataQuyetToan = new VDTPheDuyetQuyetToanPagingModel();
            dataQuyetToan._paging.CurrentPage = 1;
            dataQuyetToan.lstData = _iQLVonDauTuService.GetAllPheDuyetQuyetToan(ref dataQuyetToan._paging, string.Empty, null, null, string.Empty, null, null);

            return View(dataQuyetToan);
        }

        [HttpPost]
        public ActionResult TimKiemPheDuyetQuyetToan(PagingInfo _paging, string sSoQuyetDinh, DateTime? dNgayQuyetDinhFrom, DateTime? dNgayQuyetDinhTo, string sTenDuAn, double? fTienQuyetToanPheDuyetFrom, double? fTienQuyetToanPheDuyetTo)
        {
            VDTPheDuyetQuyetToanPagingModel vm = new VDTPheDuyetQuyetToanPagingModel();
            vm._paging = _paging;
            vm.lstData = _iQLVonDauTuService.GetAllPheDuyetQuyetToan(ref vm._paging, sSoQuyetDinh, dNgayQuyetDinhFrom, dNgayQuyetDinhTo, sTenDuAn, fTienQuyetToanPheDuyetFrom, fTienQuyetToanPheDuyetTo);
            return PartialView("_partialListPheDuyetQuyetToanDA", vm);
        }

        public ActionResult CreateNew(Guid? id)
        {
            List<NS_DonVi> lstDonViQuanLy = _iNganSachService.GetDonviListByUser(Username, PhienLamViec.NamLamViec).ToList();
            lstDonViQuanLy.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sTen = Constants.CHON });
            ViewBag.ListDonViQuanLy = lstDonViQuanLy.ToSelectList("iID_Ma", "sTen");

            List<VDT_DM_ChiPhi> lstChiPhi = _iQLVonDauTuService.LayChiPhi().ToList();
            lstChiPhi.Insert(0, new VDT_DM_ChiPhi { iID_ChiPhi = Guid.Empty, sTenChiPhi = Constants.CHON });
            ViewBag.ListChiPhi = lstChiPhi.ToSelectList("iID_ChiPhi", "sTenChiPhi");

            List<NS_NguonNganSach> lstNguonVon = _iQLVonDauTuService.LayNguonVon().ToList();
            lstNguonVon.Insert(0, new NS_NguonNganSach { iID_MaNguonNganSach = 0, sTen = Constants.CHON });
            ViewBag.ListNguonVon = lstNguonVon.ToSelectList("iID_MaNguonNganSach", "sTen");

            VDTQLPheDuyetQuyetToanModel data = new VDTQLPheDuyetQuyetToanModel();
            data.quyetToan = new VDTQLPheDuyetQuyetToanViewModel();
            data.listQuyetToanChiPhi = new List<VDTChiPhiDauTuModel>();
            data.listQuyetToanNguonVon = new List<VDTNguonVonDauTuModel>();
            data.listNguonVonChenhLech = new List<VDTNguonVonDauTuViewModel>();
            if (id.HasValue)
            {
                //Lay thong tin phe duyet quyet toan
                data.quyetToan = _iQLVonDauTuService.GetVdtQuyetToanById(id);
                //Lay danh sach chi phi dau tu
                data.listQuyetToanChiPhi = _iQLVonDauTuService.GetLstChiPhiDauTu(id);
                //Lay danh sach nguon von dau tu
                data.listQuyetToanNguonVon = _iQLVonDauTuService.GetLstNguonVonDauTu(id);

            }
            return View(data);
        }

        public bool QLPheDuyetQuyetToanSave(VDTQLPheDuyetQuyetToanModel data)
        {
            using (var conn = ConnectionFactory.Default.GetConnection())
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                if (data.quyetToan.iID_QuyetToanID == Guid.Empty)
                {
                    #region Them moi VDT_QT_QuyetToan
                    var entityQuyetToan = new VDT_QT_QuyetToan();
                    entityQuyetToan.MapFrom(data.quyetToan);
                    entityQuyetToan.sUserCreate = Username;
                    entityQuyetToan.dDateCreate = DateTime.Now;
                    conn.Insert(entityQuyetToan, trans);
                    #endregion

                    #region Them moi VDT_QT_QuyetToan_ChiPhi
                    if (data.listQuyetToanChiPhi != null && data.listQuyetToanChiPhi.Count() > 0)
                    {
                        for (int i = 0; i < data.listQuyetToanChiPhi.Count(); i++)
                        {
                            var entityQTChiPhi = new VDT_QT_QuyetToan_ChiPhi();
                            entityQTChiPhi.MapFrom(data.listQuyetToanChiPhi.ToList()[i]);
                            entityQTChiPhi.iID_QuyetToanID = entityQuyetToan.iID_QuyetToanID;
                            conn.Insert(entityQTChiPhi, trans);
                        }
                    }
                    #endregion

                    #region Them moi VDT_QT_QuyetToan_Nguonvon
                    if (data.listQuyetToanNguonVon != null && data.listQuyetToanNguonVon.Count() > 0)
                    {
                        for (int i = 0; i < data.listQuyetToanNguonVon.Count(); i++)
                        {
                            var entityQTNguonVon = new VDT_QT_QuyetToan_Nguonvon();
                            entityQTNguonVon.MapFrom(data.listQuyetToanNguonVon.ToList()[i]);
                            entityQTNguonVon.iID_QuyetToanID = entityQuyetToan.iID_QuyetToanID;
                            conn.Insert(entityQTNguonVon, trans);
                        }
                    }
                    #endregion

                    #region Them moi VDT_QT_QuyetToan_NguonVon_ChenhLech
                    if (data.listNguonVonChenhLech != null && data.listNguonVonChenhLech.Count() > 0)
                    {
                        for (int i = 0; i < data.listNguonVonChenhLech.Count(); i++)
                        {
                            var entityQTNVCL = new VDT_QT_QuyetToan_NguonVon_ChenhLech();
                            entityQTNVCL.MapFrom(data.listNguonVonChenhLech.ToList()[i]);
                            entityQTNVCL.iID_QuyetToanID = entityQuyetToan.iID_QuyetToanID;
                            entityQTNVCL.iID_NguonVonID = data.listNguonVonChenhLech.ToList()[i].iID_MaNguonNganSach;
                            entityQTNVCL.sTenNguonVonCL = data.listNguonVonChenhLech.ToList()[i].sTen;
                            entityQTNVCL.fTienChenhLech = data.listNguonVonChenhLech.ToList()[i].fGiaTriChenhLech;
                            conn.Insert(entityQTNVCL, trans);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Sua phe duyet quyet toan
                    var entityQuyetToan = conn.Get<VDT_QT_QuyetToan>(data.quyetToan.iID_QuyetToanID, trans);
                    entityQuyetToan.sSoQuyetDinh = data.quyetToan.sSoQuyetDinh;
                    entityQuyetToan.sCoQuanPheDuyet = data.quyetToan.sCoQuanPheDuyet;
                    entityQuyetToan.sNguoiKy = data.quyetToan.sNguoiKy;
                    entityQuyetToan.sNoiDung = data.quyetToan.sNoiDung;
                    entityQuyetToan.fTienQuyetToanPheDuyet = data.quyetToan.fTienQuyetToanPheDuyet;
                    entityQuyetToan.sUserUpdate = Username;
                    entityQuyetToan.dDateUpdate = DateTime.Now;
                    conn.Update(entityQuyetToan, trans);
                    #endregion

                    //delete all ChiPhi, NguonVon, NguonVon_ChenhLech
                    _iQLVonDauTuService.deleteQTChiPhiQTNguonVonQTChenhLech(data.quyetToan.iID_QuyetToanID);

                    #region Them moi VDT_QT_QuyetToan_ChiPhi
                    if (data.listQuyetToanChiPhi != null && data.listQuyetToanChiPhi.Count() > 0)
                    {
                        for (int i = 0; i < data.listQuyetToanChiPhi.Count(); i++)
                        {
                            var entityQTChiPhi = new VDT_QT_QuyetToan_ChiPhi();
                            entityQTChiPhi.MapFrom(data.listQuyetToanChiPhi.ToList()[i]);
                            entityQTChiPhi.iID_QuyetToanID = entityQuyetToan.iID_QuyetToanID;
                            conn.Insert(entityQTChiPhi, trans);
                        }
                    }
                    #endregion

                    #region Them moi VDT_QT_QuyetToan_Nguonvon
                    if (data.listQuyetToanNguonVon != null && data.listQuyetToanNguonVon.Count() > 0)
                    {
                        for (int i = 0; i < data.listQuyetToanNguonVon.Count(); i++)
                        {
                            var entityQTNguonVon = new VDT_QT_QuyetToan_Nguonvon();
                            entityQTNguonVon.MapFrom(data.listQuyetToanNguonVon.ToList()[i]);
                            entityQTNguonVon.iID_QuyetToanID = entityQuyetToan.iID_QuyetToanID;
                            conn.Insert(entityQTNguonVon, trans);
                        }
                    }
                    #endregion

                    #region Them moi VDT_QT_QuyetToan_NguonVon_ChenhLech
                    if (data.listNguonVonChenhLech != null && data.listNguonVonChenhLech.Count() > 0)
                    {
                        for (int i = 0; i < data.listNguonVonChenhLech.Count(); i++)
                        {
                            var entityQTNVCL = new VDT_QT_QuyetToan_NguonVon_ChenhLech();
                            entityQTNVCL.MapFrom(data.listNguonVonChenhLech.ToList()[i]);
                            entityQTNVCL.iID_QuyetToanID = entityQuyetToan.iID_QuyetToanID;
                            entityQTNVCL.iID_NguonVonID = data.listNguonVonChenhLech.ToList()[i].iID_MaNguonNganSach;
                            entityQTNVCL.sTenNguonVonCL = data.listNguonVonChenhLech.ToList()[i].sTen;
                            entityQTNVCL.fTienChenhLech = data.listNguonVonChenhLech.ToList()[i].fGiaTriChenhLech;
                            conn.Insert(entityQTNVCL, trans);
                        }
                    }
                    #endregion
                }
                // commit to db
                trans.Commit();
            }

            return true;
        }

        [HttpPost]
        public bool VDTQTDADelete(Guid id)
        {
            if (!_iQLVonDauTuService.deleteQTChiPhiQTNguonVonQTChenhLech(id)) return false;
            if (!_iQLVonDauTuService.deleteVDTQTDA(id)) return false;
            return true;
        }

        public ActionResult ViewDetailQTDA(Guid? id)
        {
            VDTQLPheDuyetQuyetToanModel data = new VDTQLPheDuyetQuyetToanModel();
            data.quyetToan = new VDTQLPheDuyetQuyetToanViewModel();
            data.listQuyetToanChiPhi = new List<VDTChiPhiDauTuModel>();
            data.listQuyetToanNguonVon = new List<VDTNguonVonDauTuModel>();
            data.listNguonVonChenhLech = new List<VDTNguonVonDauTuViewModel>();
            if (id.HasValue)
            {
                //Lay thong tin phe duyet quyet toan
                data.quyetToan = _iQLVonDauTuService.GetVdtQuyetToanById(id);
                //Lay danh sach chi phi dau tu
                data.listQuyetToanChiPhi = _iQLVonDauTuService.GetLstChiPhiDauTu(id);
                //Lay danh sach nguon von dau tu
                data.listQuyetToanNguonVon = _iQLVonDauTuService.GetLstNguonVonDauTu(id);

            }
            return View(data);
        }

        public JsonResult LayDanhSachDuAnTheoDonViQuanLyVaNgayQuyetDinh(string iID_DonViQuanLyID, DateTime? dNgayQuyetDinh)
        {
            List<VDTQLPheDuyetQuyetToanViewModel> lstDuAn = _iQLVonDauTuService.LayDanhSachDuAnTheoDonViQuanLyVaNgayQuyetDinhDNQT(Guid.Parse(iID_DonViQuanLyID), dNgayQuyetDinh).ToList();
            StringBuilder htmlString = new StringBuilder();
            htmlString.AppendFormat("<option value='{0}'>{1}</option>", Guid.Empty, Constants.CHON);
            if (lstDuAn != null && lstDuAn.Count > 0)
            {
                for (int i = 0; i < lstDuAn.Count; i++)
                {
                    htmlString.AppendFormat("<option value='{0}' >{1}</option>", lstDuAn[i].iID_DuAnID, lstDuAn[i].sMaDuAn + " - " + lstDuAn[i].sTenDuAn);
                }
            }
            return Json(htmlString.ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LayThongTinDuAn(Guid iID_DonViQuanLyID, Guid iID_DuAnID, DateTime dNgayQuyetDinh)
        {
            VDTQLPheDuyetQuyetToanViewModel data = new VDTQLPheDuyetQuyetToanViewModel();
            data = _iQLVonDauTuService.GetThongTinDuAn(iID_DonViQuanLyID, iID_DuAnID, dNgayQuyetDinh);
            return Json(new { bIsComplete = (data == null ? false : true), data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public double GetGiaTriDuToan(Guid iID_DuAnID, Guid iID_ChiPhiID, DateTime dNgayQuyetDinh)
        {
            double fTongMucDauTu = _iQLVonDauTuService.GetGiaTriDuToan(iID_DuAnID, iID_ChiPhiID, dNgayQuyetDinh);
            return fTongMucDauTu;
        }

        [HttpPost]
        public double GetGiaTriDuToanNguonVon(Guid iID_DuAnID, int iID_MaNguonNganSach, DateTime dNgayQuyetDinh)
        {
            double fTongMucDauTu = _iQLVonDauTuService.GetGiaTriDuToanNguonVon(iID_DuAnID, iID_MaNguonNganSach, dNgayQuyetDinh);
            return fTongMucDauTu;
        }

        [HttpPost]
        public JsonResult GetNoiDungQuyetToan(VDTQLPheDuyetQuyetToanViewModel data)
        {
            VDTQLPheDuyetQuyetToanViewModel dataResult = new VDTQLPheDuyetQuyetToanViewModel();
            double fTongGiaTriPhanBo = _iQLVonDauTuService.GetTongGiaTriPhanBo(data.iID_DonViQuanLyID, data.iID_DuAnID, data.dNgayQuyetDinh);
            IEnumerable<VDTNguonVonDauTuViewModel> lstNoiDungQuyetToan = _iQLVonDauTuService.GetLstNoiDungQuyetToan(data.arrNguonVon, data.iID_DonViQuanLyID, data.iID_DuAnID, data.dNgayQuyetDinh, PhienLamViec.NamLamViec);

            dataResult.tongGiaTriPheDuyet = data.tongGiaTriPheDuyet;
            dataResult.fTongGiaTriPhanBo = fTongGiaTriPhanBo;
            dataResult.fTongGiaTriChenhLech = fTongGiaTriPhanBo - data.tongGiaTriPheDuyet;
            dataResult.lstNoiDungQuyetToan = lstNoiDungQuyetToan;
            return Json(new { bIsComplete = (dataResult == null ? false : true), data = dataResult }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetListDataNoiDungQuyetToan(IEnumerable<VDTNguonVonDauTuViewModel> lstNoiDungQuyetToan)
        {
            VDTQLPheDuyetQuyetToanModel data = new VDTQLPheDuyetQuyetToanModel();
            data.listNguonVonChenhLech = lstNoiDungQuyetToan;
            return PartialView("_partialListNoiDungQuyetToan", data);
        }
    }
}