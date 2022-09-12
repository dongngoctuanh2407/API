using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Viettel.Services;
using Viettel.Domain.DomainModel;
using Viettel.Models.QLNH;
using VIETTEL.Controllers;

namespace VIETTEL.Areas.QLNH.Controllers.QuyetToan
{
    public class TaiSanController : AppController
    {
        private readonly IQLNHService _qlnhService = QLNHService.Default;
         
        public ActionResult Index()
        {
            QuyetToan_ChungTuModelPaging vm = new QuyetToan_ChungTuModelPaging();
            vm._paging.CurrentPage = 1;
           
            vm.Items = _qlnhService.getListChungTuTaiSanModels(ref vm._paging, null).Items;
            return View(vm);
        }

        [HttpPost]
        public ActionResult DanhMucTaiSanSearch(PagingInfo _paging, string sTenChungTu, string sSoChungTu, DateTime? dNgayChungTu)
        {
            QuyetToan_ChungTuModelPaging vm = new QuyetToan_ChungTuModelPaging();
            vm._paging = _paging;
            vm.Items = _qlnhService.getListChungTuTaiSanModels(ref vm._paging, sTenChungTu, sSoChungTu, dNgayChungTu).Items;
            return PartialView("_list", vm);
        }

        [HttpPost]
        public ActionResult GetModalDetail(Guid? id)
        {

            Guid a = Guid.Empty;
            QuyetToan_ChungTuTaiSanModel vm = new QuyetToan_ChungTuTaiSanModel();
            QuyetToan_TaiSanModelPaging listTaiSan = new QuyetToan_TaiSanModelPaging();
            listTaiSan._paging.CurrentPage = 1;
            listTaiSan.Items = _qlnhService.getListTaiSanModels(ref listTaiSan._paging,"", id);
            if (id.HasValue)
            {
                vm.ChungTuModel = _qlnhService.GetChungTuTaiSanById(id.Value);
                vm.ListTaiSan = listTaiSan;
            }
            return PartialView("_modalDetail", vm);
        }

        [HttpPost]
        public ActionResult GetModal(Guid? id)
        {

            QuyetToan_ChungTuTaiSanModel vm = new QuyetToan_ChungTuTaiSanModel();
            QuyetToan_TaiSanModelPaging listTaiSan = new QuyetToan_TaiSanModelPaging();
            listTaiSan._paging.CurrentPage = 1;
            listTaiSan.Items = _qlnhService.getListTaiSanModels(ref listTaiSan._paging, "", id);
            if (id.HasValue)
            {
                vm.ChungTuModel = _qlnhService.GetChungTuTaiSanById(id.Value);
                vm.ListTaiSan = listTaiSan;
            }
            List<Dropdown_ChungTuTaiSan> b = new List<Dropdown_ChungTuTaiSan>()
            {
               new Dropdown_ChungTuTaiSan()
               {
                   valueId =1,
                   labelName = "Mới"
               },
                new Dropdown_ChungTuTaiSan()
               {
                   valueId =2,
                   labelName = "Cũ"
                },
                 new Dropdown_ChungTuTaiSan()
               {
                   valueId =3,
                   labelName = "Hết giá trị"
                }

            };
            List<Dropdown_ChungTuTaiSan> c = new List<Dropdown_ChungTuTaiSan>()
            {
               new Dropdown_ChungTuTaiSan()
               {
                   valueId =1,
                   labelName = "Tài sản hữu hình"
               },
                new Dropdown_ChungTuTaiSan()
               {
                   valueId =2,
                   labelName = "Tài sản vô hình"
                }
            };
            List<Dropdown_ChungTuTaiSan> d = new List<Dropdown_ChungTuTaiSan>()
            {
               new Dropdown_ChungTuTaiSan()
               {
                   valueId =1,
                   labelName = "Chưa sử dụng"
               },
                new Dropdown_ChungTuTaiSan()
               {
                   valueId =2,
                   labelName = "Đang sử dụng"
                },
                 new Dropdown_ChungTuTaiSan()
               {
                   valueId =3,
                   labelName = "Không sử dụng"
                }

            };

            List<Dropdown_ChungTuTaiSan> lstTinhTrang = b;
            lstTinhTrang.Insert(0, new Dropdown_ChungTuTaiSan { valueId = 0, labelName = "--Chọn trạng thái--" });
            ViewBag.ListTinhTrang = lstTinhTrang;

            List<Dropdown_ChungTuTaiSan> lstloaitaisan = c;
            lstloaitaisan.Insert(0, new Dropdown_ChungTuTaiSan { valueId = 0, labelName = "--Loại tài sản--" });
            ViewBag.ListLoaiTaiSan = lstloaitaisan;

            List<Dropdown_ChungTuTaiSan> lsttinhtrangsudung = d;
            lsttinhtrangsudung.Insert(0, new Dropdown_ChungTuTaiSan { valueId = 0, labelName = "--Tình trạng sử dụng--" });
            ViewBag.ListTinhTrangSuDung = lsttinhtrangsudung;

            var a = _qlnhService.GetLookupDonVi().ToSelectList("iID_Ma", "sTen");

            List<NS_DonVi> lstDonViQL = _qlnhService.GetLookupDonVi().ToList();
            lstDonViQL.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sTen = "--Chọn đơn vị--" });
            ViewBag.ListThongTinDonVi = lstDonViQL;

            List<NH_DA_DuAn> llstThongTinDuAn = _qlnhService.GetLookupDuAn().ToList();
            llstThongTinDuAn.Insert(0, new NH_DA_DuAn { ID = Guid.Empty, sTenDuAn = "--Chọn dự án--" });
            ViewBag.ListThongTinDuAn = llstThongTinDuAn;


            List<NH_DA_HopDong> llstThongTinHopDong = _qlnhService.GetLookupHopDong().ToList();
            llstThongTinHopDong.Insert(0, new NH_DA_HopDong { ID = Guid.Empty, sTenHopDong = "--Chọn hợp đồng--" });
            ViewBag.ListThongTinHopDong = llstThongTinHopDong;

            return PartialView("_modalUpdate", vm);
        }
        [HttpPost]
        public ActionResult GetCreate()
        {
            List<Dropdown_ChungTuTaiSan> b = new List<Dropdown_ChungTuTaiSan>()
            {
               new Dropdown_ChungTuTaiSan()
               {
                   valueId =1,
                   labelName = "Mới"
               },
                new Dropdown_ChungTuTaiSan()
               {
                   valueId =2,
                   labelName = "Cũ"
                },
                 new Dropdown_ChungTuTaiSan()
               {
                   valueId =3,
                   labelName = "Hết giá trị"
                }

            };
            List<Dropdown_ChungTuTaiSan> c = new List<Dropdown_ChungTuTaiSan>()
            {
               new Dropdown_ChungTuTaiSan()
               {
                   valueId =1,
                   labelName = "Tài sản hữu hình"
               },
                new Dropdown_ChungTuTaiSan()
               {
                   valueId =2,
                   labelName = "Tài sản vô hình"
                }
            };
            List<Dropdown_ChungTuTaiSan> d = new List<Dropdown_ChungTuTaiSan>()
            {
               new Dropdown_ChungTuTaiSan()
               {
                   valueId =1,
                   labelName = "Chưa sử dụng"
               },
                new Dropdown_ChungTuTaiSan()
               {
                   valueId =2,
                   labelName = "Đang sử dụng"
                },
                 new Dropdown_ChungTuTaiSan()
               {
                   valueId =3,
                   labelName = "Không sử dụng"
                }

            };

            List<Dropdown_ChungTuTaiSan> lstTinhTrang = b;
            lstTinhTrang.Insert(0, new Dropdown_ChungTuTaiSan { valueId = 0, labelName = "--Chọn trạng thái sử dụng--" });
            ViewBag.ListTinhTrang = lstTinhTrang;

            List<Dropdown_ChungTuTaiSan> lstloaitaisan = c;
            lstloaitaisan.Insert(0, new Dropdown_ChungTuTaiSan { valueId = 0, labelName = "--Chọn loại tài sản--" });
            ViewBag.ListLoaiTaiSan = lstloaitaisan;

            List<Dropdown_ChungTuTaiSan> lsttinhtrangsudung = d;
            lsttinhtrangsudung.Insert(0, new Dropdown_ChungTuTaiSan { valueId = 0, labelName = "--Chọn tình trạng sử dụng--" });
            ViewBag.ListTinhTrangSuDung = lsttinhtrangsudung;


            List<NS_DonVi> lstDonViQL = _qlnhService.GetLookupDonVi().ToList();
            lstDonViQL.Insert(0, new NS_DonVi { iID_Ma = Guid.Empty, sTen = "--Chọn đơn vị--" });
            ViewBag.ListThongTinDonVi = lstDonViQL;

            List<NH_DA_DuAn> llstThongTinDuAn = _qlnhService.GetLookupDuAn().ToList();
            llstThongTinDuAn.Insert(0, new NH_DA_DuAn { ID = Guid.Empty, sTenDuAn = "--Chọn dự án--" });
            ViewBag.ListThongTinDuAn = llstThongTinDuAn;


            List<NH_DA_HopDong> llstThongTinHopDong = _qlnhService.GetLookupHopDong().ToList();
            llstThongTinHopDong.Insert(0, new NH_DA_HopDong { ID = Guid.Empty, sTenHopDong = "--Chọn hợp đồng--" });
            ViewBag.ListThongTinHopDong = llstThongTinHopDong;
            return PartialView("_modalCreate");
        }
       
       
        [HttpPost]
        public JsonResult TaiSanDelete(Guid id)
        {
            if (!_qlnhService.DeleteTaiSan(id))
            {
                return Json(new { bIsComplete = false, sMessError = "Không xóa được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ChungTuTaiSanDelete(Guid id)
        {
            if (!_qlnhService.DeleteChungTuTaiSan(id))
            {
                return Json(new { bIsComplete = false, sMessError = "Không xóa được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TaiSanSave(List<NH_QT_TaiSan> datats,NH_QT_ChungTuTaiSan datactts)
        {
            if (!_qlnhService.SaveChungTuTaiSan(datats, datactts))
            {
                return Json(new { bIsComplete = false, sMessError = "Không cập nhật được dữ liệu !" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { bIsComplete = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDropdown()
        {
            return Json(new
            {
                data = _qlnhService.GetLookupDonVi()
            });
        }     
        
        [HttpPost]
        public JsonResult GetDropdownDuAn()
        {
            return Json(new
            {
                data = _qlnhService.GetLookupDuAn()
            });
        }   
        
        [HttpPost]
        public JsonResult GetDropdownHopDong()
        {
            return Json(new
            {
                data = _qlnhService.GetLookupHopDong()
            });
        }
        [HttpPost]
        public JsonResult GetDropdownTinhTrang()
        {
            List<Dropdown_ChungTuTaiSan> b = new List<Dropdown_ChungTuTaiSan>()
            {
               new Dropdown_ChungTuTaiSan()
               {
                   valueId =1,
                   labelName = "Mới"
               },
                new Dropdown_ChungTuTaiSan()
               {
                   valueId =2,
                   labelName = "Cũ"
                },
                 new Dropdown_ChungTuTaiSan()
               {
                   valueId =3,
                   labelName = "Hết giá trị"
                }

            };

            List<Dropdown_ChungTuTaiSan> lstTinhTrang = b;
            lstTinhTrang.Insert(0, new Dropdown_ChungTuTaiSan { valueId = 0, labelName = "--chọn tình trạng--" });
            ViewBag.ListTinhTrang = lstTinhTrang;

         
            return Json(new
            {
                data = b
            });
        }
        public JsonResult GetDropdownLoaiTaiSan()
        {
          
            List<Dropdown_ChungTuTaiSan> c = new List<Dropdown_ChungTuTaiSan>()
            {
               new Dropdown_ChungTuTaiSan()
               {
                   valueId =1,
                   labelName = "Tài sản hữu hình"
               },
                new Dropdown_ChungTuTaiSan()
               {
                   valueId =2,
                   labelName = "Tài sản vô hình"
                }
            };
          
            List<Dropdown_ChungTuTaiSan> lstloaitaisan = c;
            lstloaitaisan.Insert(0, new Dropdown_ChungTuTaiSan { valueId = 0, labelName = "-- Chọn loại tài sản--" });
            ViewBag.ListLoaiTaiSan = lstloaitaisan;
           
            return Json(new
            {
                data = c
            });
        }
        public JsonResult GetDropdownTinhTrangSuDung()
        {

            List<Dropdown_ChungTuTaiSan> d = new List<Dropdown_ChungTuTaiSan>()
            {
               new Dropdown_ChungTuTaiSan()
               {
                   valueId =1,
                   labelName = "Chưa sử dụng"
               },
                new Dropdown_ChungTuTaiSan()
               {
                   valueId =2,
                   labelName = "Đang sử dụng"
                },
                 new Dropdown_ChungTuTaiSan()
               {
                   valueId =3,
                   labelName = "Không sử dụng"
                }

            };


            List<Dropdown_ChungTuTaiSan> lsttinhtrangsudung = d;
            lsttinhtrangsudung.Insert(0, new Dropdown_ChungTuTaiSan { valueId = 0, labelName = "-- Chọn tình trạng sử dụng--" });
            ViewBag.ListTinhTrangSuDung = lsttinhtrangsudung;

            return Json(new
            {
                data = d
            });
        }
    }
}