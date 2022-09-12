using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Viettel.Domain.DomainModel;
using Viettel.Models.QLVonDauTu;
using Viettel.Services;
using VIETTEL.Areas.QLVonDauTu.Model.QuyetToan;
using VIETTEL.Common;
using VIETTEL.Controllers;

namespace VIETTEL.Areas.QLVonDauTu.Controllers.QuyetToan
{
    public class VDT_QT_DeNghiQuyetToanController : AppController
    {
        private readonly IQLVonDauTuService _vdtService = QLVonDauTuService.Default;
        // GET: QLVonDauTu/VDT_QT_DeNghiQuyetToan
        public ActionResult Index()
        {
            VDT_QT_DeNghiQuyetToanPagingModel data = new VDT_QT_DeNghiQuyetToanPagingModel();
            data._paging.CurrentPage = 1;
            data.lstData = _vdtService.GetAllDeNghiQuyetToanPaging(ref data._paging, "", null, null, "", "", Username).OrderBy(x => x.dThoiGianKhoiCong);
            return View(data);
        }

        [HttpPost]
        public ActionResult GetListView(PagingInfo _paging, string sSoBaoCao, decimal? sGiaDeNghiTu, decimal? sGiaDeNghiDen, string sTenDuAn, string sMaDuAn)
        {
            VDT_QT_DeNghiQuyetToanPagingModel data = new VDT_QT_DeNghiQuyetToanPagingModel();
            data._paging = _paging;
            data.lstData = _vdtService.GetAllDeNghiQuyetToanPaging(ref data._paging, sSoBaoCao, sGiaDeNghiTu, sGiaDeNghiDen, sTenDuAn, sMaDuAn, Username).OrderBy(x => x.dThoiGianKhoiCong);
            return PartialView("_list", data);
        }

        [HttpPost]
        public ActionResult GetModal(Guid? id)
        {
            var data = new VDT_QT_DeNghiQuyetToan();
            if (id.HasValue)
            {
                data = _vdtService.Get_VDT_QT_DeNghiQuyetToanById(id.Value);
            }

            return PartialView("_modalUpdate", data);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Update(Guid? id)
        {
            var model = new VDT_QT_DeNghiQuyetToan();
            if (!id.HasValue)
            {
                return View();
            }
            model = _vdtService.GetDeNghiQuyetToanDetail(id.Value.ToString(), Username);
            ViewBag.isDetail = 0;
            return View(model);
        }

        [HttpGet]
        public ActionResult Detail(Guid? id)
        {
            var model = new VDT_QT_DeNghiQuyetToan();
            if (!id.HasValue)
            {
                return View();
            }
            model = _vdtService.GetDeNghiQuyetToanDetail(id.Value.ToString(), Username);
            ViewBag.isDetail = 1;
            return View("/Areas/QLVonDauTu/Views/VDT_QT_DeNghiQuyetToan/Update.cshtml", model);
        }

        [HttpPost]
        public ActionResult GetModalDetail(Guid? id)
        {
            var data = new VDT_QT_DeNghiQuyetToan();
            if (id.HasValue)
            {
                data = _vdtService.GetDeNghiQuyetToanDetail(id.Value.ToString(), Username);
            }

            return PartialView("_modalDetail", data);
        }

        [HttpPost]
        public JsonResult GetDonViQuanLy()
        {
            var result = new List<dynamic>();
            var listModel = _vdtService.GetListAllDonVi(Username);
            if (listModel != null && listModel.Any())
            {
                result.Add(new { id = string.Empty, text = "--Chọn--" });
                foreach (var item in listModel)
                {
                    result.Add(new { id = item.iID_Ma, text = $"{item.iID_MaDonVi} - {item.sTen}" });
                }
            }
            return Json(new { status = true, data = result });
        }

        [HttpPost]
        public JsonResult GetDuAnTheoDonViQuanLy(string idDonVi, string iIdDeNghiQuyetToanId)
        {
            var result = new List<dynamic>();
            var listModel = _vdtService.GetListAllDuAn(idDonVi, iIdDeNghiQuyetToanId);
            if (listModel != null && listModel.Any())
            {
                result.Add(new { id = string.Empty, text = "--Chọn--" });
                foreach (var item in listModel)
                {
                    result.Add(new { id = item.iID_DuAnID, text = item.sTenDuAn });
                }
            }
            return Json(new { status = true, data = result });
        }

        [HttpPost]
        public JsonResult GetDuLieuDuAnByIdDonViQuanLy(string idDuAn)
        {
            VDT_QT_DeNghiQuyetToanGetDuAnModel result = _vdtService.GetDuLieuDuAnById(idDuAn, Username);
            List<VDTDuToanNguonVonModel> lstNguonVon = _vdtService.GetListDuToanNguonVonByDuAn(idDuAn);
            return Json(new { status = true, data = result, lstNguonVon = lstNguonVon });
        }

        public JsonResult GetListChiPhiHangMucTheoDuAn(Guid idDuAn, Guid? iIdDeNghiQuyetToan = null)
        {
            VDT_DA_DuToan objDuToan = _vdtService.GetDuToanIdByDuAnId(idDuAn);
            List<VDT_DA_DuToan_ChiPhi_ViewModel> listChiPhi = new List<VDT_DA_DuToan_ChiPhi_ViewModel>();
            List<VDT_DA_DuToan_HangMuc_ViewModel> listHangMuc = new List<VDT_DA_DuToan_HangMuc_ViewModel>();
            if (objDuToan != null)
            {
                listChiPhi = _vdtService.GetListChiPhiTheoTKTC(objDuToan.iID_DuToanID).ToList();
                listHangMuc = _vdtService.GetListHangMucTheoTKTC(objDuToan.iID_DuToanID).ToList();
            }
            if(iIdDeNghiQuyetToan!= null && iIdDeNghiQuyetToan != Guid.Empty)
            {
                List<VDT_QT_DeNghiQuyetToan_ChiTiet> lstQuyetToanChiTiet = _vdtService.GetDeNghiQuyetToanChiTiet(iIdDeNghiQuyetToan.Value);
                if(lstQuyetToanChiTiet != null && lstQuyetToanChiTiet.Any())
                {
                    if(listChiPhi != null && listChiPhi.Any())
                    {
                        foreach (VDT_DA_DuToan_ChiPhi_ViewModel itemCp in listChiPhi)
                        {
                            VDT_QT_DeNghiQuyetToan_ChiTiet objQuyetToanChiTiet = lstQuyetToanChiTiet.Where(x => x.iID_ChiPhiId == itemCp.iID_DuAn_ChiPhi).FirstOrDefault();
                            if(objQuyetToanChiTiet != null)
                            {
                                itemCp.fGiaTriDeNghiQuyetToan = objQuyetToanChiTiet.fGiaTriDeNghiQuyetToan;
                                itemCp.fGiaTriKiemToan = objQuyetToanChiTiet.fGiaTriKiemToan;
                                itemCp.fGiaTriQuyetToanAB = objQuyetToanChiTiet.fGiaTriQuyetToanAB;
                            }
                        }
                    }

                    if (listHangMuc != null && listHangMuc.Any())
                    {
                        foreach (VDT_DA_DuToan_HangMuc_ViewModel itemHm in listHangMuc)
                        {
                            VDT_QT_DeNghiQuyetToan_ChiTiet objQuyetToanChiTiet = lstQuyetToanChiTiet.Where(x => x.iID_HangMucId == itemHm.iID_HangMucID).FirstOrDefault();
                            if (objQuyetToanChiTiet != null)
                            {
                                itemHm.fGiaTriDeNghiQuyetToan = objQuyetToanChiTiet.fGiaTriDeNghiQuyetToan;
                                itemHm.fGiaTriKiemToan = objQuyetToanChiTiet.fGiaTriKiemToan;
                                itemHm.fGiaTriQuyetToanAB = objQuyetToanChiTiet.fGiaTriQuyetToanAB;
                            }
                        }
                    }
                }
            }
            return Json(new { lstChiPhi = listChiPhi, lstHangMuc = listHangMuc }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDuLieuDonViQuanLyByIdDuAn(string idDuAn)
        {
            var result = new NS_DonVi();
            var donVi = _vdtService.GetDuLieuDonViQuanLyByIdDuAn(idDuAn, Username);
            if (donVi != null)
            {
                result = donVi;
            }

            return Json(new { status = true, data = result });
        }

        [HttpPost]
        public JsonResult SetValueComboBoxDuAn(string id)
        {
            var result = new VDT_QT_DeNghiQuyetToan();
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { status = false });
            }
            result = _vdtService.Get_VDT_QT_DeNghiQuyetToanById(Guid.Parse(id));
            return Json(new { status = true, data = result });
        }

        [HttpPost]
        public JsonResult SaveData(VDT_QT_DeNghiQuyetToanViewModel data)
        {
            var result = _vdtService.VDT_QT_DeNghiQuyetToan_SaveData(data, Username);
            if (result == false)
                return Json(new { status = false, sMessage = "Lưu dữ liệu không thành công." });

            return Json(new { status = true, sMessage = "Lưu dữ liệu thành công." });
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { status = false, sMessage = "Xóa dữ liệu không thành công" });
            }

            var result = _vdtService.VDT_QT_DeNghiQuyetToan_Delete(id, Username);

            if (!result)
            {
                return Json(new { status = false, sMessage = "Xóa dữ liệu không thành công" });
            }

            return Json(new { status = result, sMessage = "Xóa dữ liệu thành công" });
        }

        //[HttpPost]
        //public ActionResult ExportExcelDeNghiQuyetToan(string sSoBaoCao,decimal? sGiaDeNghiTu,decimal? sGiaDeNghiDen,string sTenDuAn,string sMaDuAn)
        //{
        //    var listModel = _vdtService.ExportData(sSoBaoCao, sGiaDeNghiTu, sGiaDeNghiDen, sTenDuAn, sMaDuAn,Username);
        //    string urlFile = Server.MapPath("~/Areas/Resource/MauExcel/VDT-QT-DeNghiQuyetToan.xlsx");
        //    using (XLWorkbook workbook = new XLWorkbook(urlFile))
        //    {
        //        IXLWorksheet workSheet = workbook.Worksheet("Sheet1");
        //        if(listModel != null && listModel.Any())
        //        {
        //            var count = 2;
        //            foreach (var item in listModel)
        //            {
        //                workSheet.Cell(count, 1).Value = (count - 1).ToString();
        //                workSheet.Cell(count, 2).Value = item.sMaDuAn;
        //                workSheet.Cell(count, 3).Value = item.sTenDuAn;
        //                workSheet.Cell(count, 4).Value = item.sTenChuDauTu;
        //                workSheet.Cell(count, 5).Value = item.sSoBaoCao;
        //                workSheet.Cell(count, 6).Value = item.dThoiGianKhoiCong.Value.ToString("dd/MM/yyyy");
        //                workSheet.Cell(count, 7).Value = item.dThoiGianHoanThanh.Value.ToString("dd/MM/yyyy");
        //                workSheet.Cell(count, 8).Value = string.Format("{0:0,0}", item.fGiaTriDeNghiQuyetToan.Value);

        //                count++;
        //            }
        //        }

        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            workbook.SaveAs(stream);
        //            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "VDT_QT_DeNghiQuyetToan.xlsx");
        //        }
        //    }
        //}

        [HttpPost]
        public ActionResult ExportExcelDeNghiQuyetToan(string sSoBaoCao, decimal? sGiaDeNghiTu, decimal? sGiaDeNghiDen, string sTenDuAn, string sMaDuAn)
        {
            var excel = CreateReport(sSoBaoCao, sGiaDeNghiTu, sGiaDeNghiDen, sTenDuAn, sMaDuAn);

            using (MemoryStream stream = new MemoryStream())
            {
                excel.Save(stream);

                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Danh sách đề nghị quyết toán hoàn thành.xlsx");
            }
        }

        public ExcelFile CreateReport(string sSoBaoCao, decimal? sGiaDeNghiTu, decimal? sGiaDeNghiDen, string sTenDuAn, string sMaDuAn)
        {
            var listData = _vdtService.ExportData(sSoBaoCao, sGiaDeNghiTu, sGiaDeNghiDen, sTenDuAn, sMaDuAn, Username);

            XlsFile Result = new XlsFile(true);
            Result.Open(Server.MapPath("~/Areas/QLVonDauTu/ReportExcelForm/rpt_VDT_QT_DeNghiQuyetToan.xlsx"));
            FlexCelReport fr = new FlexCelReport();
            fr.AddTable<VDT_QT_DeNghiQuyetToanViewModel>("DeNghiQuyetToan", listData.AsEnumerable());
            fr.Run(Result);
            return Result;
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ExportWordDeNghiQuyetToan(string sSoBaoCao, decimal? sGiaDeNghiTu, decimal? sGiaDeNghiDen, string sTenDuAn, string sMaDuAn)
        {
            var listModel = _vdtService.ExportData(sSoBaoCao, sGiaDeNghiTu, sGiaDeNghiDen, sTenDuAn, sMaDuAn, Username);
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename = VDT_QT_DeNghiQuyetToan.doc");
            Response.ContentType = "application/ms-word";
            Response.Charset = "";

            var html = "<table style='border-collapse: collapse;width:100%'>";
            html += "<thead>";
            html += "<th style='border:1px solid #ddd;padding:8px;padding-top: 12px;padding-bottom: 12px;text-align: left;background-color: #04AA6D;color: white;' width='5%'>STT</th>";
            html += "<th style='border:1px solid #ddd;padding:8px;padding-top: 12px;padding-bottom: 12px;text-align: left;background-color: #04AA6D;color: white;' width='10'>Mã dự án</th>";
            html += "<th style='border:1px solid #ddd;padding:8px;padding-top: 12px;padding-bottom: 12px;text-align: left;background-color: #04AA6D;color: white;' width='15%'>Tên dự án</th>";
            html += "<th style='border:1px solid #ddd;padding:8px;padding-top: 12px;padding-bottom: 12px;text-align: left;background-color: #04AA6D;color: white;' width='15%'>Chủ đầu tư</th>";
            html += "<th style='border:1px solid #ddd;padding:8px;padding-top: 12px;padding-bottom: 12px;text-align: left;background-color: #04AA6D;color: white;' width='10%'>Số báo cáo</th>";
            html += "<th style='border:1px solid #ddd;padding:8px;padding-top: 12px;padding-bottom: 12px;text-align: left;background-color: #04AA6D;color: white;' width='10%'>Thời gian khởi công</th>";
            html += "<th style='border:1px solid #ddd;padding:8px;padding-top: 12px;padding-bottom: 12px;text-align: left;background-color: #04AA6D;color: white;' width='10%'>Thời gian hoàn thành</th>";
            html += "<th style='border:1px solid #ddd;padding:8px;padding-top: 12px;padding-bottom: 12px;text-align: left;background-color: #04AA6D;color: white;' width='15%'>Giá trị đề nghị quyết toán</th>";
            html += "</thead>";
            html += "<tbody>";
            if (listModel != null && listModel.Any())
            {
                var count = 1;
                foreach (var item in listModel)
                {
                    html += "<tr>";
                    html += "<td style='border:1px solid #ddd;padding:8px' align='center'>" + count + "</td>";
                    html += "<td style='border:1px solid #ddd;padding:8px' align='left'>" + item.sMaDuAn + "</td>";
                    html += "<td style='border:1px solid #ddd;padding:8px' align='left'>" + item.sTenDuAn + "</td>";
                    html += "<td style='border:1px solid #ddd;padding:8px' align='center'>" + item.sTenChuDauTu + "</td>";
                    html += "<td style='border:1px solid #ddd;padding:8px' align='left'>" + item.sSoBaoCao + "</td>";
                    html += "<td style='border:1px solid #ddd;padding:8px' align='center'>" + item.dThoiGianKhoiCong.Value.ToString("dd/MM/yyyy") + "</td>";
                    html += "<td style='border:1px solid #ddd;padding:8px' align='center'>" + item.dThoiGianHoanThanh.Value.ToString("dd/MM/yyyy") + "</td>";
                    html += "<td style='border:1px solid #ddd;padding:8px' align='center'>" + string.Format("{0:0,0}", item.fGiaTriDeNghiQuyetToan.Value) + "</td>";
                    html += "</tr>";

                    count++;
                }
            }

            html += "</tbody>";
            html += "</table>";

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    Response.Output.Write(html);
                    Response.Flush();
                    Response.End();
                }
            }

            return Json(new { status = true });
        }
    }
}