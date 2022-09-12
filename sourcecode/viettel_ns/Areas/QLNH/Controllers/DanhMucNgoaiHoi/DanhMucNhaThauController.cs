using System;
using System.Web.Mvc;
using VIETTEL.Controllers;
using Viettel.Domain.DomainModel;
using Viettel.Services;
using Viettel.Models.QLNH;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Net;
using DomainModel;
using System.Web;
using ClosedXML.Excel;
using System.IO;
using VIETTEL.Helpers;
using System.Reflection;
using VIETTEL.Areas.z.Models;
using System.Data;

namespace VIETTEL.Areas.QLNH.Controllers.DanhMucNgoaiHoi
{
    public class DanhMucNhaThauController : AppController
    {
        private readonly IQLNHService _dmService = QLNHService.Default;
        private List<NHDMNhaThau_Dropdown_LoaiNhaThau> loaiNhaThauList = new List<NHDMNhaThau_Dropdown_LoaiNhaThau>()
        {
            new NHDMNhaThau_Dropdown_LoaiNhaThau()
            {
                valueId = 0,
                labelName = "--Loại nhà thầu--"
            },
            new NHDMNhaThau_Dropdown_LoaiNhaThau()
            {
                valueId = 1,
                labelName = "Nhà thầu"
            },
            new NHDMNhaThau_Dropdown_LoaiNhaThau()
            {
                valueId = 2,
                labelName = "Đơn vị ủy thác"
            }
        };

        public ActionResult Index()
        {
            ViewBag.ListLoaiNhaThau = loaiNhaThauList;

            NHDMNhaThauViewModel vm = new NHDMNhaThauViewModel();
            vm._paging.CurrentPage = 1;
            vm.Items = _dmService.GetAllDanhMucNhaThau(ref vm._paging);

            return View(vm);
        }

        [HttpPost]
        public ActionResult DanhMucNhaThauSearch(PagingInfo _paging, string sMaNhaThau, string sTenNhaThau, string sDiaChi, string sDaiDien, string sChucVu, string sDienThoai, string sFax, string sEmail, string sWebsite, int? iLoai)
        {
            ViewBag.ListLoaiNhaThau = loaiNhaThauList;

            sMaNhaThau = HttpUtility.HtmlDecode(sMaNhaThau);
            sTenNhaThau = HttpUtility.HtmlDecode(sTenNhaThau);
            sDiaChi = HttpUtility.HtmlDecode(sDiaChi);
            sDaiDien = HttpUtility.HtmlDecode(sDaiDien);
            sChucVu = HttpUtility.HtmlDecode(sChucVu);
            sDienThoai = HttpUtility.HtmlDecode(sDienThoai);
            sFax = HttpUtility.HtmlDecode(sFax);
            sEmail = HttpUtility.HtmlDecode(sEmail);
            sWebsite = HttpUtility.HtmlDecode(sWebsite);

            NHDMNhaThauViewModel vm = new NHDMNhaThauViewModel();
            vm._paging = _paging;
            vm.Items = _dmService.GetAllDanhMucNhaThau(ref vm._paging, sMaNhaThau, sTenNhaThau, sDiaChi, sDaiDien, sChucVu, sDienThoai, sFax, sEmail, sWebsite, iLoai);
            return PartialView("_list", vm);
        }

        [HttpPost]
        public ActionResult GetModal(Guid? id)
        {
            NHDMNhaThauModel data = new NHDMNhaThauModel();
            if (id.HasValue)
            {
                data = _dmService.GetDanhMucNhaThauById(id.Value);
            }
            ViewBag.ListLoaiNhaThau = loaiNhaThauList;

            return PartialView("Update", data);
        }

        [HttpPost]
        public ActionResult GetModalDetail(Guid? id)
        {
            NHDMNhaThauModel data = new NHDMNhaThauModel();
            if (id.HasValue)
            {
                data = _dmService.GetDanhMucNhaThauById(id.Value);
            }
            return PartialView("Detail", data);
        }

        [HttpPost]
        public JsonResult NhaThauDelete(Guid id)
        {
            if (!_dmService.DeleteNhaThau(id))
            {
                return Json(new { bIsComplete = false, sMessError = "Không xóa được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult NhaThauSave(NH_DM_NhaThau data)
        {
            if (data == null)
            {
                return Json(new { bIsComplete = false, sMessError = "Không cập nhật được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }

            List<NH_DM_NhaThau> lstNhaThau = _dmService.GetNHDMNhaThauList(null).ToList();
            var checkExistMaNhaThau = lstNhaThau.FirstOrDefault(x => x.sMaNhaThau.ToUpper().Equals(data.sMaNhaThau.ToUpper()) && x.Id != data.Id);
            if (checkExistMaNhaThau != null)
            {
                return Json(new { bIsComplete = false, sMessError = "Mã nhà thầu đã tồn tại!" }, JsonRequestBehavior.AllowGet);
            }

            data.sMaNhaThau = HttpUtility.HtmlDecode(data.sMaNhaThau);
            data.sTenNhaThau = HttpUtility.HtmlDecode(data.sTenNhaThau);
            data.sDiaChi = HttpUtility.HtmlDecode(data.sDiaChi);
            data.sMaSoThue = HttpUtility.HtmlDecode(data.sMaSoThue);
            data.sDaiDien = HttpUtility.HtmlDecode(data.sDaiDien);
            data.sChucVu = HttpUtility.HtmlDecode(data.sChucVu);
            data.sDienThoai = HttpUtility.HtmlDecode(data.sDienThoai);
            data.sFax = HttpUtility.HtmlDecode(data.sFax);
            data.sEmail = HttpUtility.HtmlDecode(data.sEmail);
            data.sWebsite = HttpUtility.HtmlDecode(data.sWebsite);
            data.sSoTaiKhoan = HttpUtility.HtmlDecode(data.sSoTaiKhoan);
            data.sNganHang = HttpUtility.HtmlDecode(data.sNganHang);
            data.sMaNganHang = HttpUtility.HtmlDecode(data.sMaNganHang);
            data.sNguoiLienHe = HttpUtility.HtmlDecode(data.sNguoiLienHe);
            data.sDienThoaiLienHe = HttpUtility.HtmlDecode(data.sDienThoaiLienHe);
            data.sSoCMND = HttpUtility.HtmlDecode(data.sSoCMND);
            data.sNoiCapCMND = HttpUtility.HtmlDecode(data.sNoiCapCMND);

            if (!_dmService.SaveNhaThau(data))
            {
                return Json(new { bIsComplete = false, sMessError = "Không cập nhật được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }

        private string GetHtmlSelectOptionLoai()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<option value='{0}' selected>{1}</option>", 0, "--Chọn loại--");
            sb.AppendFormat("<option value='{0}'>{1}</option>", 1, "Nhà thầu");
            sb.AppendFormat("<option value='{0}'>{1}</option>", 2, "Đơn vị ủy thác");
            return sb.ToString();
        }

        [HttpPost]
        public ActionResult ImportNhaThau()
        {
            return PartialView("importNT");
        }
        private byte[] GetBytes(HttpPostedFileBase file)
        {
            using (BinaryReader b = new BinaryReader(file.InputStream))
            {
                byte[] xls = b.ReadBytes(file.ContentLength);
                return xls;
            }
        }
        public ActionResult LoadDataExcel(HttpPostedFileBase file)
        {
            string data;
            try
            {
                byte[] file_data = GetBytes(file);
                DataTable dataTable = ExcelHelpers.LoadExcelDataTable(file_data);
                IEnumerable<NhaThauImportModel> dataImport = GetExcelResult(dataTable);
                data = GetHtmlDataExcel(dataImport);
            }
            catch (Exception ex)
            {
                AppLog.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
                return Json(new { sMessError = "Không thể tải dữ liệu từ file đã chọn!" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { bIsComplete = true, data = data }, JsonRequestBehavior.AllowGet);
        }
        private string GetHtmlDataExcel(IEnumerable<NhaThauImportModel> dataImport)
        {
            StringBuilder sb = new StringBuilder();


            string htmlLoai = GetHtmlSelectOptionLoai();

            int i = 0;
            foreach (NhaThauImportModel item in dataImport)
            {
                sb.AppendLine("<tr class=\"" + (item.IsDataWrong ? "wrong" : "correct") + "\" data-index=\"" + i + "\">");
                //Trạng thái
                sb.AppendLine("<td align='center' style='vertical-align:middle;' class='status-icon'>");
                if (item.IsDataWrong)
                {
                    sb.AppendLine("<i class=\"fa fa-close fa-lg color-text-red\" aria-hidden=\"true\"></i>");
                }
                else
                {
                    sb.AppendLine("<i class=\"fa fa-check fa-lg\" style=\"color:green;\" aria-hidden=\"true\"></i>");
                }
                sb.AppendLine("</td>");

                //STT
                sb.AppendLine("<td align='center' style='vertical-align:middle;'>");
                sb.AppendLine(HttpUtility.HtmlEncode(item.STT));
                sb.AppendLine("</td>");


                //Loại
                sb.AppendLine("<td align='center' style='vertical-align:middle;'>");
                sb.AppendLine("<select id=\"slbLoai" + i + "\" name=\"slbLoai" + i + "\" class=\"form-control selectLoai\" data-index=\"" + i + "\" onchange=\"ChangeSelectLoai(this);\">");
                sb.AppendLine(htmlLoai);
                sb.AppendLine("</select>");
                sb.AppendLine("</td>");
                //mã nhà thầu
                sb.AppendLine("<td align='left' style='vertical-align:middle;' class='" + (item.IsMaNhaThauWrong ? "cellWrong" : "") + "'>");
                if (item.IsMaNhaThauWrong)
                {
                    sb.AppendLine("<input type='text' class='form-control inputCommon' autocomplete='off' id='txtMaNhaThau" + i + "' value='" + HttpUtility.HtmlEncode(item.sMaNhaThau) + "'/>");
                }
                else
                {
                    sb.AppendLine("<span id='spanMaNhaThau" + i + "' class='spanCommon'>" + HttpUtility.HtmlEncode(item.sMaNhaThau) + "</span>");
                }
                sb.AppendLine("</td>");

                //nhà thầu/dơn vị uye thác
                sb.AppendLine("<td align='left' style='vertical-align:middle;' class='" + (item.IsDonViUyThacWrong ? "cellWrong" : "") + "'>");
                if (item.IsDonViUyThacWrong)
                {
                    sb.AppendLine("<input type='text' class='form-control inputCommon' autocomplete='off' id='txtDonViUyThac" + i + "' value='" + HttpUtility.HtmlEncode(item.sDonViUyThac) + "'/>");
                }
                else
                {
                    sb.AppendLine("<span id='spanDonViUyThac" + i + "' class='spanCommon'>" + HttpUtility.HtmlEncode(item.sDonViUyThac) + "</span>");
                }
                sb.AppendLine("</td>");

                //Địa chỉ
                sb.AppendLine("<td align='left' style='vertical-align:middle;' class='" + (item.IsDiaChiWrong ? "cellWrong" : "") + "'>");
                if (item.IsDiaChiWrong)
                {
                    sb.AppendLine("<input type='text' class='form-control inputCommon' autocomplete='off' id='txtDiaChi" + i + "' value='" + HttpUtility.HtmlEncode(item.sDiaChi) + "'/>");
                }
                else
                {
                    sb.AppendLine("<span id='spanDiaChi" + i + "' class='spanCommon'>" + HttpUtility.HtmlEncode(item.sDiaChi) + "</span>");
                }
                sb.AppendLine("</td>");

                //Đại diện
                sb.AppendLine("<td align='left' style='vertical-align:middle;' class='" + (item.IsDaiDienWrong ? "cellWrong" : "") + "'>");
                if (item.IsDaiDienWrong)
                {
                    sb.AppendLine("<input type='text' class='form-control inputCommon' autocomplete='off' id='txtDaiDien" + i + "' value='" + HttpUtility.HtmlEncode(item.sDaiDien) + "'/>");
                }
                else
                {
                    sb.AppendLine("<span id='spanDaiDien" + i + "' class='spanCommon'>" + HttpUtility.HtmlEncode(item.sDaiDien) + "</span>");
                }
                sb.AppendLine("</td>");

                //Chức vụ
                sb.AppendLine("<td align='left' style='vertical-align:middle;' class='" + (item.IsChucVuWrong ? "cellWrong" : "") + "'>");
                if (item.IsChucVuWrong)
                {
                    sb.AppendLine("<input type='text' class='form-control inputCommon' autocomplete='off' id='txtChucVu" + i + "' value='" + HttpUtility.HtmlEncode(item.sChucVu) + "'/>");
                }
                else
                {
                    sb.AppendLine("<span id='spanChucVu" + i + "' class='spanCommon'>" + HttpUtility.HtmlEncode(item.sChucVu) + "</span>");
                }
                sb.AppendLine("</td>");

                //số điện thoại
                sb.AppendLine("<td align='left' style='vertical-align:middle;' class='" + (item.IsSoDienThoaiWrong ? "cellWrong" : "") + "'>");
                if (item.IsSoDienThoaiWrong)
                {
                    sb.AppendLine("<input type='text' class='form-control inputCommon' autocomplete='off' id='txtSoDienThoai" + i + "' value='" + HttpUtility.HtmlEncode(item.sSoDienThoai) + "'/>");
                }
                else
                {
                    sb.AppendLine("<span id='spanSoDienThoai" + i + "' class='spanCommon'>" + HttpUtility.HtmlEncode(item.sSoDienThoai) + "</span>");
                }
                sb.AppendLine("</td>");

                //số Fax
                sb.AppendLine("<td align='left' style='vertical-align:middle;' class='" + (item.IsFaxWrong ? "cellWrong" : "") + "'>");
                if (item.IsFaxWrong)
                {
                    sb.AppendLine("<input type='text' class='form-control inputCommon' autocomplete='off' id='txtSoFax" + i + "' value='" + HttpUtility.HtmlEncode(item.sFax) + "'/>");
                }
                else
                {
                    sb.AppendLine("<span id='spanSoFax" + i + "' class='spanCommon'>" + HttpUtility.HtmlEncode(item.sFax) + "</span>");
                }
                sb.AppendLine("</td>");
                //email
                sb.AppendLine("<td align='left' style='vertical-align:middle;' class='" + (item.IsEmailWrong ? "cellWrong" : "") + "'>");
                if (item.IsEmailWrong)
                {
                    sb.AppendLine("<input type='text' class='form-control inputCommon' autocomplete='off' id='txtEmail" + i + "' value='" + HttpUtility.HtmlEncode(item.sEmail) + "'/>");
                }
                else
                {
                    sb.AppendLine("<span id='spanEmail" + i + "' class='spanCommon'>" + HttpUtility.HtmlEncode(item.sEmail) + "</span>");
                }
                sb.AppendLine("</td>");

                //website
                sb.AppendLine("<td align='left' style='vertical-align:middle;' class='" + (item.IsWebsiteWrong ? "cellWrong" : "") + "'>");
                if (item.IsWebsiteWrong)
                {
                    sb.AppendLine("<input type='text' class='form-control inputCommon' autocomplete='off' id='txtWebsite" + i + "' value='" + HttpUtility.HtmlEncode(item.sWebsite) + "'/>");
                }
                else
                {
                    sb.AppendLine("<span id='spanWebsite" + i + "' class='spanCommon'>" + HttpUtility.HtmlEncode(item.sWebsite) + "</span>");
                }
                sb.AppendLine("</td>");
                //số tài khoản
                sb.AppendLine("<td align='left' style='vertical-align:middle;' class='" + (item.IsSoTaiKhoanWrong ? "cellWrong" : "") + "'>");
                if (item.IsSoTaiKhoanWrong)
                {
                    sb.AppendLine("<input type='text' class='form-control inputCommon' autocomplete='off' id='txtSoTaiKhoan" + i + "' value='" + HttpUtility.HtmlEncode(item.sSoTaiKhoan) + "'/>");
                }
                else
                {
                    sb.AppendLine("<span id='spanSoTaiKhoan" + i + "' class='spanCommon'>" + HttpUtility.HtmlEncode(item.sSoTaiKhoan) + "</span>");
                }
                sb.AppendLine("</td>");
                //ngân hàng
                sb.AppendLine("<td align='left' style='vertical-align:middle;' class='" + (item.IsNganHangWrong ? "cellWrong" : "") + "'>");
                if (item.IsNganHangWrong)
                {
                    sb.AppendLine("<input type='text' class='form-control inputCommon' autocomplete='off' id='txtNganHang" + i + "' value='" + HttpUtility.HtmlEncode(item.sNganHang) + "'/>");
                }
                else
                {
                    sb.AppendLine("<span id='spanNganHang" + i + "' class='spanCommon'>" + HttpUtility.HtmlEncode(item.sNganHang) + "</span>");
                }
                sb.AppendLine("</td>");
                //Mã ngân hàng
                sb.AppendLine("<td align='left' style='vertical-align:middle;' class='" + (item.IsMaNganHangWrong ? "cellWrong" : "") + "'>");
                if (item.IsMaNganHangWrong)
                {
                    sb.AppendLine("<input type='text' class='form-control inputCommon' autocomplete='off' id='txtMaNganHang" + i + "' value='" + HttpUtility.HtmlEncode(item.MaNganHang) + "'/>");
                }
                else
                {
                    sb.AppendLine("<span id='spanMaNganHang" + i + "' class='spanCommon'>" + HttpUtility.HtmlEncode(item.MaNganHang) + "</span>");
                }
                sb.AppendLine("</td>");
                //Người liên hệ
                sb.AppendLine("<td align='left' style='vertical-align:middle;' class='" + (item.IsNguoiLienHeWrong ? "cellWrong" : "") + "'>");
                if (item.IsNguoiLienHeWrong)
                {
                    sb.AppendLine("<input type='text' class='form-control inputCommon' autocomplete='off' id='txtNguoiLienHe" + i + "' value='" + HttpUtility.HtmlEncode(item.sNguoiLienHe) + "'/>");
                }
                else
                {
                    sb.AppendLine("<span id='spanNguoiLienHe" + i + "' class='spanCommon'>" + HttpUtility.HtmlEncode(item.sNguoiLienHe) + "</span>");
                }
                sb.AppendLine("</td>");
                //Số điện thoại người liên hệ
                sb.AppendLine("<td align='left' style='vertical-align:middle;' class='" + (item.IsDienThoaiLienHeWrong ? "cellWrong" : "") + "'>");
                if (item.IsDienThoaiLienHeWrong)
                {
                    sb.AppendLine("<input type='text' class='form-control inputCommon' autocomplete='off' id='txtsdtNguoiLH" + i + "' value='" + HttpUtility.HtmlEncode(item.sNguoiLienHe) + "'/>");
                }
                else
                {
                    sb.AppendLine("<span id='spansdtNguoiLH" + i + "' class='spanCommon'>" + HttpUtility.HtmlEncode(item.sDienThoaiLienHe) + "</span>");
                }
                sb.AppendLine("</td>");
                //só CMND
                sb.AppendLine("<td align='left' style='vertical-align:middle;' class='" + (item.IsSoCMNDWrong ? "cellWrong" : "") + "'>");
                if (item.IsSoCMNDWrong)
                {
                    sb.AppendLine("<input type='text' class='form-control inputCommon' autocomplete='off' id='txtSoCMND" + i + "' value='" + HttpUtility.HtmlEncode(item.sSoCMND) + "'/>");
                }
                else
                {
                    sb.AppendLine("<span id='spanSoCMND" + i + "' class='spanCommon'>" + HttpUtility.HtmlEncode(item.sSoCMND) + "</span>");
                }
                sb.AppendLine("</td>");
                //Nơi cấp
                sb.AppendLine("<td align='left' style='vertical-align:middle;' class='" + (item.IsNoiCapCMNDWrong ? "cellWrong" : "") + "'>");
                if (item.IsNoiCapCMNDWrong)
                {
                    sb.AppendLine("<input type='text' class='form-control inputCommon' autocomplete='off' id='txtNoiCap" + i + "' value='" + HttpUtility.HtmlEncode(item.sNoiCapCMND) + "'/>");
                }
                else
                {
                    sb.AppendLine("<span id='spanNoiCap" + i + "' class='spanCommon'>" + HttpUtility.HtmlEncode(item.sNoiCapCMND) + "</span>");
                }
                sb.AppendLine("</td>");
                //ngày cấp

                sb.AppendLine("<td align='left' style='vertical-align:middle;' class='" + (item.IsNgayCapWrong ? "cellWrong" : "") + "'>");
                if (item.IsNgayCapWrong)
                {
                    sb.AppendLine("<input type='text' class='form-control inputCommon inputDate' required autocomplete='off' id='txtNgayCap" + i + "' value='" + HttpUtility.HtmlEncode(item.sNgayCap) + "'/>");
                }
                else
                {
                    sb.AppendLine("<span id='spanNgayCap" + i + "' class='spanCommon'>" + HttpUtility.HtmlEncode(item.sNgayCap) + "</span>");
                }
                sb.AppendLine("</td>");
                //Lỗi
                sb.AppendLine("<td align='left' class='color-text-red cellMessageError' style='vertical-align:middle;'>");
                sb.AppendLine(item.sErrorMessage);
                sb.AppendLine("</td>");

                //Thao tác
                sb.AppendLine("<td align='center' style='vertical-align:middle;'>");
                sb.AppendLine("<button type=\"button\" id=\"btn-delete" + i + "\" class=\"btn-delete\" title=\"Xóa\" onclick=\"ConfirmRemoveRowImport(" + i + ");\"><i class=\"fa fa-trash-o fa-lg\" aria-hidden=\"true\"></i></button>");
                sb.AppendLine("</td>");

                sb.AppendLine("</tr>");

                i++;
            }
            return sb.ToString();
        }
        public ActionResult DownloadTemplateImport()
        {
            try
            {
                XLWorkbook w_b = new XLWorkbook();
                var wbContractInfo = w_b.Worksheets.Add("Biểu mẫu Import nhà thầu");
                wbContractInfo.Column(1).Width = 15;
                wbContractInfo.Column(2).Width = 40;
                wbContractInfo.Column(3).Width = 30;
                wbContractInfo.Column(4).Width = 25;
                wbContractInfo.Column(5).Width = 25;
                wbContractInfo.Column(6).Width = 25;
                wbContractInfo.Column(7).Width = 25;
                wbContractInfo.Column(8).Width = 25;
                wbContractInfo.Column(9).Width = 25;
                wbContractInfo.Column(10).Width = 25;
                wbContractInfo.Column(11).Width = 25;
                wbContractInfo.Column(12).Width = 25;
                wbContractInfo.Column(13).Width = 25;
                wbContractInfo.Column(14).Width = 25;
                wbContractInfo.Column(15).Width = 25;
                wbContractInfo.Column(16).Width = 25;
                wbContractInfo.Column(17).Width = 25;
                wbContractInfo.Column(18).Width = 25;
                wbContractInfo.Column(19).Width = 25;
                wbContractInfo.Column(20).Width = 25;


                //Sheet biểu mẫu
                wbContractInfo.Style.Font.FontName = "Times New Roman";
                wbContractInfo.Style.Font.FontSize = 13;
                wbContractInfo.PageSetup.FitToPages(1, 1);
                wbContractInfo.Row(23).Height = 40;
                wbContractInfo.Cell(2, 1).Value = "BIỂU MẪU IMPORT NHÀ THẦU";
                wbContractInfo.Range(2, 1, 2, 9).Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Font.SetBold(true).Alignment.SetWrapText(true).Font.SetFontSize(15);

                wbContractInfo.Column(1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                              .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                              .Alignment.SetWrapText(true);
                wbContractInfo.Column(2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(true);
                wbContractInfo.Column(3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(true);
                wbContractInfo.Column(4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(true);
                wbContractInfo.Column(5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetWrapText(true);
                wbContractInfo.Column(6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(true);
                wbContractInfo.Column(7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetWrapText(true);
                wbContractInfo.Column(8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetWrapText(true).NumberFormat.SetFormat("0");
                wbContractInfo.Column(9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(true).NumberFormat.SetFormat("0");
                wbContractInfo.Column(10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(true);
                wbContractInfo.Column(11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetWrapText(true);
                wbContractInfo.Column(12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetWrapText(true).NumberFormat.SetFormat("0");
                wbContractInfo.Column(13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetWrapText(true);
                wbContractInfo.Column(14).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetWrapText(true).NumberFormat.SetFormat("0");
                wbContractInfo.Column(15).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetWrapText(true);
                wbContractInfo.Column(16).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetWrapText(true).NumberFormat.SetFormat("0");
                wbContractInfo.Column(17).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetWrapText(true).NumberFormat.SetFormat("0");
                wbContractInfo.Column(18).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetWrapText(true);
                wbContractInfo.Column(19).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetWrapText(true).DateFormat.SetFormat("dd/MM/yyyy");


                wbContractInfo.Cell(4, 1).Value = "Loại : Chỉ chọn giá trị trong dropdown";
                wbContractInfo.Cell(4, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(false);
                wbContractInfo.Cell(5, 1).Value = "Mã nhà thầu/ đơn vị ủy thác *: Nhập chữ, tối đa 100 ký tự ";
                wbContractInfo.Cell(5, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(false);
                wbContractInfo.Cell(6, 1).Value = "Tên nhà thầu/đơn vị ủy thác *: Nhập số, chữ, tối đa 300 ký tự ";
                wbContractInfo.Cell(6, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(false);
                wbContractInfo.Cell(7, 1).Value = "Địa chỉ: Nhập số, chữ, tối đa 300 ký tự ";
                wbContractInfo.Cell(7, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(false);
                wbContractInfo.Cell(8, 1).Value = "Đại diện: Nhập số, chữ, tối đa 100 ký tự ";
                wbContractInfo.Cell(8, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(false);
                wbContractInfo.Cell(9, 1).Value = "Chức vụ:  Nhập số, chữ, tối đa 100 ký tự ";
                wbContractInfo.Cell(9, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(false);
                wbContractInfo.Cell(10, 1).Value = "Số điện thoại: Nhập số, tối đa 100 ký tự ";
                wbContractInfo.Cell(10, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(false);
                wbContractInfo.Cell(11, 1).Value = "Số fax: Nhập số, chữ, tối đa 100 ký tự ";
                wbContractInfo.Cell(11, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(false);
                wbContractInfo.Cell(12, 1).Value = "Email: Nhập số, chữ, tối đa 100 ký tự ";
                wbContractInfo.Cell(12, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(false);
                wbContractInfo.Cell(13, 1).Value = "Website:Nhập số, chữ, tối đa 300 ký tự ";
                wbContractInfo.Cell(13, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(false);
                wbContractInfo.Cell(14, 1).Value = "Số tài khoản: Nhập số, chữ, tối đa 100 ký tự ";
                wbContractInfo.Cell(14, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(false);
                wbContractInfo.Cell(15, 1).Value = "Ngân hàng: Nhập số, chữ, tối đa 100 ký tự ";
                wbContractInfo.Cell(15, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(false);
                wbContractInfo.Cell(16, 1).Value = "Mã ngân hàng: Nhập số, chữ, tối đa 100 ký tự ";
                wbContractInfo.Cell(16, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(false);
                wbContractInfo.Cell(17, 1).Value = "Người liên hệ: Nhập số, chữ, tối đa 300 ký tự ";
                wbContractInfo.Cell(17, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(false);
                wbContractInfo.Cell(18, 1).Value = "Số điện thọai người liên hệ: Nhập số, chữ, tối đa 100 ký tự";
                wbContractInfo.Cell(18, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(false);
                wbContractInfo.Cell(19, 1).Value = "Số CMND: Nhập số, tối đa 50 ký tự ";
                wbContractInfo.Cell(19, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(false);
                wbContractInfo.Cell(20, 1).Value = "Nơi cấp CMND: Nhập số, chữ tối đa 255 ký tự ";
                wbContractInfo.Cell(20, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(false);
                wbContractInfo.Cell(21, 1).Value = "Ngày cấp CMND: Chỉ nhập định dạng dd/mm/yyyy";
                wbContractInfo.Cell(21, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetWrapText(false);





                wbContractInfo.Cell(23, 1).Value = "STT";
                wbContractInfo.Cell(23, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wbContractInfo.Cell(23, 2).Value = "Loại";
                wbContractInfo.Cell(23, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wbContractInfo.Cell(23, 3).Value = "Mã nhà thầu/đơn vị ủy thác";
                wbContractInfo.Cell(23, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wbContractInfo.Cell(23, 4).Value = "Tên nhà thầu/ Đơn vị ủy thác ";
                wbContractInfo.Cell(23, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wbContractInfo.Cell(23, 5).Value = "Địa chỉ";
                wbContractInfo.Cell(23, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wbContractInfo.Cell(23, 6).Value = "Đại diện ";
                wbContractInfo.Cell(23, 6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wbContractInfo.Cell(23, 7).Value = "Chức vụ";
                wbContractInfo.Cell(23, 7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wbContractInfo.Cell(23, 8).Value = "Số điện thoại";
                wbContractInfo.Cell(23, 8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wbContractInfo.Cell(23, 9).Value = "Số Fax";
                wbContractInfo.Cell(23, 9).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wbContractInfo.Cell(23, 10).Value = "Email";
                wbContractInfo.Cell(23, 10).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wbContractInfo.Cell(23, 11).Value = "Website";
                wbContractInfo.Cell(23, 11).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wbContractInfo.Cell(23, 12).Value = "Số tài khoản";
                wbContractInfo.Cell(23, 12).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wbContractInfo.Cell(23, 13).Value = "Ngân hàng";
                wbContractInfo.Cell(23, 13).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wbContractInfo.Cell(23, 14).Value = "Mã ngân hàng";
                wbContractInfo.Cell(23, 14).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wbContractInfo.Cell(23, 15).Value = "Người liên hệ";
                wbContractInfo.Cell(23, 15).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wbContractInfo.Cell(23, 16).Value = "Số điện thoại người liên hệ";
                wbContractInfo.Cell(23, 16).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wbContractInfo.Cell(23, 17).Value = "Số CMND";
                wbContractInfo.Cell(23, 17).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wbContractInfo.Cell(23, 18).Value = "Nơi cấp CMND";
                wbContractInfo.Cell(23, 18).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                wbContractInfo.Cell(23, 19).Value = "Ngày cấp CMND";
                wbContractInfo.Cell(23, 19).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                                                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                    .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                                                    .Alignment.SetWrapText(true);
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Biểu mẫu import nhà thầu.xlsx");
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    w_b.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                AppLog.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            return null;
        }
        private DateTime? TryParseDateTime(string sGiaTri)
        {
            DateTime dGiaTri;
            if (!DateTime.TryParse(sGiaTri, out dGiaTri))
            {
                return null;
            }
            else
            {
                return dGiaTri;
            }
        }

        public JsonResult SaveImport(List<NH_DM_NhaThau> contractList)
        {
            if (contractList == null)
            {
                return Json(new { bIsComplete = false, sMessError = "Không import được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }

            foreach (var contract in contractList)
            {
                contract.dNgayTao = DateTime.Now;
                contract.sNguoiTao = Username;
            }

            if (!_dmService.SaveImportNhaThau(contractList))
            {
                return Json(new { bIsComplete = false, sMessError = "Không import được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }
        private IEnumerable<NhaThauImportModel> GetExcelResult(DataTable dt)
        {
            List<NhaThauImportModel> dataImportList = new List<NhaThauImportModel>();
            NhaThauImportModel data = new NhaThauImportModel();
            DataRow row;

            string STT = string.Empty;
            string sMaNhaThau = string.Empty;
            string sDonViUyThac = string.Empty;
            string sDiaChi = string.Empty;
            string sDaiDien = string.Empty;
            string sChucVu = string.Empty;
            string sSoDienThoai = string.Empty;
            string sFax = string.Empty;
            string sEmail = string.Empty;
            string sWebsite = string.Empty;
            string sSoTaiKhoan = string.Empty;
            string sNganHang = string.Empty;
            string MaNganHang = string.Empty;
            string sSDTNguoiLienHe = string.Empty;
            string sNguoiLienHe = string.Empty;
            string sDienThoaiLienHe = string.Empty;
            
            string sSoCMND = string.Empty;
            string sNoiCapCMND = string.Empty;
            string sNgayCap = string.Empty;

            DateTime? dNgayCap;

            StringBuilder sErrorMessage = new StringBuilder();
            bool isDataWrong = false;
            bool IsMaNhaThauWrong = false;
            bool IsDonViUyThacWrong = false;
            bool IsDiaChiWrong = false;
            bool IsDaiDienWrong = false;
            bool IsChucVuWrong = false;
            bool IsSoDienThoaiWrong = false;
            bool IsFaxWrong = false;
            bool IsEmailWrong = false;
            bool IsWebsiteWrong = false;
            bool IsSoTaiKhoanWrong = false;
            bool IsNganHangWrong = false;
            bool IsMaNganHangWrong = false;
            bool IsSoCMNDWrong = false;
            bool IsNoiCapCMNDWrong = false;
            bool IsNguoiLienHeWrong = false;
            bool IsDienThoaiLienHeWrong = false;
            bool IsNgayCapWrong = false;



            var items = dt.AsEnumerable();
            for (var i = 23; i < items.Count(); i++)
            {

                isDataWrong = false;
                IsMaNhaThauWrong = false;
                IsDonViUyThacWrong = false;
                IsDiaChiWrong = false;
                IsDaiDienWrong = false;
                IsChucVuWrong = false;
                IsSoDienThoaiWrong = false;
                IsFaxWrong = false;
                IsEmailWrong = false;
                IsWebsiteWrong = false;
                IsSoTaiKhoanWrong = false;
                IsNganHangWrong = false;
                IsMaNganHangWrong = false;
                IsNguoiLienHeWrong = false;
                IsDienThoaiLienHeWrong = false;

                IsSoCMNDWrong = false;
                IsNoiCapCMNDWrong = false;
                IsNgayCapWrong = false;
                sErrorMessage.Clear();
                row = items.ToList()[i];

                STT = row.Field<string>(0);
                sMaNhaThau = !string.IsNullOrEmpty(row.Field<string>(2)) ? row.Field<string>(2).Trim() : string.Empty;
                if (string.IsNullOrEmpty(sMaNhaThau))
                {
                    isDataWrong = true;
                    IsMaNhaThauWrong = true;
                    sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "* Mã nhà thầu chưa được nhập.");
                }
                else if (sMaNhaThau.Length > 100)
                {
                    isDataWrong = true;
                    IsMaNhaThauWrong = true;
                    sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "* Mã nhà thầu nhập quá 100 kí tự.");
                }
                sDonViUyThac = !string.IsNullOrEmpty(row.Field<string>(3)) ? row.Field<string>(3).Trim() : string.Empty;
                if (string.IsNullOrEmpty(sDonViUyThac))
                {
                    isDataWrong = true;
                    IsDonViUyThacWrong = true;
                    sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "*Tên nha thầu chưa được nhập.");
                }
                else if (sDonViUyThac.Length > 100)
                {
                    isDataWrong = true;
                    IsDonViUyThacWrong = true;
                    sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "* Tên nhà thầu nhập quá 100 kí tự.");
                }

                sDiaChi = !string.IsNullOrEmpty(row.Field<string>(4)) ? row.Field<string>(4).Trim() : string.Empty;
                if (!string.IsNullOrEmpty(sDiaChi) && sDiaChi.Length > 300)
                {
                    isDataWrong = true;
                    IsDiaChiWrong = true;
                    sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "* Địa chỉ nhập quá 300 kí tự.");
                }
                sDaiDien = !string.IsNullOrEmpty(row.Field<string>(5)) ? row.Field<string>(5).Trim() : string.Empty;
                if (!string.IsNullOrEmpty(sDaiDien) && sDaiDien.Length > 100)
                {
                    isDataWrong = true;
                    IsDaiDienWrong = true;
                    sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "* Đại diện nhập quá 100 kí tự.");
                }
                sChucVu = !string.IsNullOrEmpty(row.Field<string>(6)) ? row.Field<string>(6).Trim() : string.Empty;
                if (!string.IsNullOrEmpty(sChucVu ) && sChucVu.Length > 100)
                {
                    isDataWrong = true;
                    IsChucVuWrong = true;
                    sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "* Chức vụ nhập quá 100 kí tự.");
                }
                sSoDienThoai = !string.IsNullOrEmpty(row.Field<string>(7)) ? row.Field<string>(7).Trim() : string.Empty;
                if (!string.IsNullOrEmpty(sSoDienThoai) && sSoDienThoai.Length > 100)
                {
                    isDataWrong = true;
                    IsSoDienThoaiWrong = true;
                    sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "* Số điện thoại nhập quá 100 kí tự.");
                }
                sFax = !string.IsNullOrEmpty(row.Field<string>(8)) ? row.Field<string>(8).Trim() : string.Empty;
                if (!string.IsNullOrEmpty(sFax) && sFax.Length > 100)
                {
                    isDataWrong = true;
                    IsFaxWrong = true;
                    sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "* Số Fax nhập quá 100 kí tự.");
                }
                sEmail = !string.IsNullOrEmpty(row.Field<string>(9)) ? row.Field<string>(9).Trim() : string.Empty;
                if (!string.IsNullOrEmpty(sEmail) && sEmail.Length > 100)
                {
                    isDataWrong = true;
                    IsEmailWrong = true;
                    sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "* Email nhập quá 100 kí tự.");
                }
                sWebsite = !string.IsNullOrEmpty(row.Field<string>(10)) ? row.Field<string>(10).Trim() : string.Empty;
                if (!string.IsNullOrEmpty(sWebsite) && sWebsite.Length > 100)
                {
                    isDataWrong = true;
                    IsWebsiteWrong = true;
                    sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "* Website nhập quá 100 kí tự.");
                }
                sSoTaiKhoan = !string.IsNullOrEmpty(row.Field<string>(11)) ? row.Field<string>(11).Trim() : string.Empty;
                if (!string.IsNullOrEmpty(sSoTaiKhoan) && sSoTaiKhoan.Length > 100)
                {
                    isDataWrong = true;
                    IsSoTaiKhoanWrong = true;
                    sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "* Số tài khoản ngân hàng nhập quá 100 kí tự.");
                }
                sNganHang = !string.IsNullOrEmpty(row.Field<string>(12)) ? row.Field<string>(12).Trim() : string.Empty;
                if (!string.IsNullOrEmpty(sNganHang) && sNganHang.Length > 100)
                {
                    isDataWrong = true;
                    IsNganHangWrong = true;
                    sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "* Ngân hàng nhập quá 100 kí tự.");
                }
                MaNganHang = !string.IsNullOrEmpty(row.Field<string>(13)) ? row.Field<string>(13).Trim() : string.Empty;
                if (!string.IsNullOrEmpty(MaNganHang) && MaNganHang.Length > 100)
                {
                    isDataWrong = true;
                    IsMaNganHangWrong = true;
                    sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "* Mã ngân hàng nhập quá 100 kí tự.");
                }
                sNguoiLienHe = !string.IsNullOrEmpty(row.Field<string>(14)) ? row.Field<string>(14).Trim() : string.Empty;
                if (!string.IsNullOrEmpty(sNguoiLienHe) && sNguoiLienHe.Length > 100)
                {
                    isDataWrong = true;
                    IsNguoiLienHeWrong = true;
                    sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "* Người liên hệ nhập quá 100 kí tự.");
                }
                sDienThoaiLienHe = !string.IsNullOrEmpty(row.Field<string>(15)) ? row.Field<string>(15).Trim() : string.Empty;
                if (!string.IsNullOrEmpty(sDienThoaiLienHe) && sDienThoaiLienHe.Length > 100)
                {
                    isDataWrong = true;
                    IsDienThoaiLienHeWrong = true;
                    sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "* Số điện thoại người liên hệ nhập quá 100 kí tự.");
                }
                sSoCMND = !string.IsNullOrEmpty(row.Field<string>(16)) ? row.Field<string>(16).Trim() : string.Empty;
                if (!string.IsNullOrEmpty(sSoCMND) && sSoCMND.Length > 100)
                {
                    isDataWrong = true;
                    IsSoCMNDWrong = true;
                    sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "* Chứng minh nhân dân nhập quá 100 kí tự.");
                }
                sNoiCapCMND = !string.IsNullOrEmpty(row.Field<string>(17)) ? row.Field<string>(17).Trim() : string.Empty;
                if (!string.IsNullOrEmpty(sNoiCapCMND) && sNoiCapCMND.Length > 100)
                {
                    isDataWrong = true;
                    IsNoiCapCMNDWrong = true;
                    sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "* Nơi cấp chứng minh nhân dân nhập quá 100 kí tự.");
                }
                sNgayCap = !string.IsNullOrEmpty(row.Field<string>(18)) ? row.Field<string>(18).Trim() : string.Empty;
                dNgayCap = TryParseDateTime(sNgayCap);
                if (!string.IsNullOrEmpty(sNgayCap))
                {
                    if (!dNgayCap.HasValue)
                    {
                        isDataWrong = true;
                        IsNgayCapWrong = true;
                        sErrorMessage.AppendLine((sErrorMessage.Length > 0 ? "<br/>" : "") + "* Ngày cấp không hợp lệ.");
                    }
                    else
                    {
                        sNgayCap = dNgayCap.Value.ToString("dd/MM/yyyy");
                    }
                }
                data = new NhaThauImportModel
                {
                    STT = STT,
                    sMaNhaThau = sMaNhaThau,
                    IsMaNhaThauWrong = IsMaNhaThauWrong,
                    sDonViUyThac = sDonViUyThac,
                    IsDonViUyThacWrong = IsDonViUyThacWrong,
                    sDiaChi = sDiaChi,
                    IsDiaChiWrong = IsDiaChiWrong,
                    sDaiDien = sDaiDien,
                    IsDaiDienWrong = IsDaiDienWrong,
                    sChucVu = sChucVu,
                    IsChucVuWrong = IsChucVuWrong,
                    sSoDienThoai = sSoDienThoai,
                    IsSoDienThoaiWrong = IsSoDienThoaiWrong,
                    sFax = sFax,
                    IsFaxWrong = IsFaxWrong,
                    sEmail = sEmail,
                    IsEmailWrong = IsEmailWrong,
                    sWebsite = sWebsite,
                    IsWebsiteWrong = IsWebsiteWrong,
                    sSoTaiKhoan = sSoTaiKhoan,
                    IsSoTaiKhoanWrong = IsSoTaiKhoanWrong,
                    sNganHang = sNganHang,
                    IsNganHangWrong = IsNganHangWrong,
                    MaNganHang = MaNganHang,
                    IsMaNganHangWrong = IsMaNganHangWrong,
                    sNguoiLienHe = sNguoiLienHe,
                    IsNguoiLienHeWrong = IsNguoiLienHeWrong,
                    sDienThoaiLienHe = sDienThoaiLienHe,
                    IsDienThoaiLienHeWrong = IsDienThoaiLienHeWrong,
                    sSoCMND = sSoCMND,
                    IsSoCMNDWrong = IsSoCMNDWrong,
                    sNoiCapCMND = sNoiCapCMND,
                    IsNoiCapCMNDWrong = IsNoiCapCMNDWrong,
                    sNgayCap = sNgayCap,
                    IsNgayCapWrong = IsNgayCapWrong,
                    sErrorMessage = sErrorMessage.ToString(),
                    IsDataWrong = isDataWrong,
                };

                dataImportList.Add(data);
            }
            return dataImportList.AsEnumerable();
        }

    }
}