﻿using System.Web.Mvc;
using Viettel.Services;

namespace VIETTEL.Controllers.NguonNS
{
    public class Nguon_BTCBQPController : Controller
    {
        private readonly ISharedService _sharedService = SharedService.Default;

        public string sViewPath = "~/Views/NguonNS/BTCBQP/";

        [Authorize]
        public ActionResult Index()
        {
            if (_sharedService.checkEditByPhongBan(User.Identity.Name, "02")) {
                return View(sViewPath + "BTCBQP_Index.aspx");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }        
        [Authorize]
        public ActionResult BTCBQP_Index_Frame()
        {            
            return View(sViewPath + "BTCBQP_Index_DanhSach_Frame.aspx");
        }
        public ActionResult Edit(string loai, string Id_MaNguon)
        {
            if (_sharedService.checkEditByPhongBan(User.Identity.Name, "02"))
            {
                return View(sViewPath + "BTCBQP_Edit.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        [Authorize]
        public ActionResult BTCBQP_Edit_Frame(string loai, string Id_MaNguon, string LuuThanhCong)
        {
            if (_sharedService.checkEditByPhongBan(User.Identity.Name, "02"))
            {
                return View(sViewPath + "BTCBQP_Edit_Frame.aspx", new { loai = loai, Id_MaNguon = Id_MaNguon, LuuThanhCong = LuuThanhCong });
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        //[Authorize]
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult DetailSubmit(string maND)
        //{
        //    if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
        //    {
        //        String TenBangChiTiet = "NS_NguoiDung_DonVi";
        //        string idXauMaCacHang = Request.Form["idXauMaCacHang"];
        //        string idXauMaCacCot = Request.Form["idXauMaCacCot"];
        //        string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
        //        string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
        //        string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
        //        String[] arrMaHang = idXauMaCacHang.Split(',');
        //        String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
        //        String[] arrMaCot = idXauMaCacCot.Split(',');
        //        String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang },
        //                                                          StringSplitOptions.None);
        //        string iID_MaNguoiDungDonVi, id_dv;
        //        //Luu cac hang sua
        //        String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang },
        //                                                           StringSplitOptions.None);
        //        for (int i = 0; i < arrMaHang.Length; i++)
        //        {
        //            iID_MaNguoiDungDonVi = arrMaHang[i].Split('_')[0];
        //            id_dv = arrMaHang[i].Split('_')[1];
        //            if (arrHangDaXoa[i] == "1")
        //            {
        //                //Lưu các hàng đã xóa
        //                if (iID_MaNguoiDungDonVi != "")
        //                {
        //                    //Dữ liệu đã có
        //                    Bang bang = new Bang(TenBangChiTiet);
        //                    bang.DuLieuMoi = false;
        //                    bang.GiaTriKhoa = iID_MaNguoiDungDonVi;
        //                    bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 0);
        //                    bang.MaNguoiDungSua = User.Identity.Name;
        //                    bang.IPSua = Request.UserHostAddress;
        //                    bang.Save();
        //                }
        //            }
        //            else
        //            {
        //                String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { BangDuLieu.DauCachO },
        //                                                            StringSplitOptions.None);
        //                String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { BangDuLieu.DauCachO },
        //                                                              StringSplitOptions.None);
        //                Boolean okCoThayDoi = false;
        //                for (int j = 0; j < arrMaCot.Length; j++)
        //                {
        //                    if (arrThayDoi[j] == "1")
        //                    {
        //                        okCoThayDoi = true;
        //                        break;
        //                    }
        //                }
        //                if (okCoThayDoi)
        //                {
        //                    Bang bang = new Bang(TenBangChiTiet);
        //                    iID_MaNguoiDungDonVi = arrMaHang[i].Split('_')[0];
        //                    if (iID_MaNguoiDungDonVi == "")
        //                    {
        //                        //Du Lieu Moi
        //                        bang.DuLieuMoi = true;
        //                        bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", MucLucNganSachModels.LayNamLamViec(User.Identity.Name));
        //                        bang.CmdParams.Parameters.AddWithValue("@iID_MaDonVi", id_dv);
        //                        bang.CmdParams.Parameters.AddWithValue("@sMaNguoiDung", maND);
        //                    }
        //                    else
        //                    {
        //                        //Du Lieu Da Co
        //                        bang.GiaTriKhoa = iID_MaNguoiDungDonVi;
        //                        bang.CmdParams.Parameters.AddWithValue("@iTrangThai", arrGiaTri[1]);
        //                        bang.DuLieuMoi = false;
        //                    }
        //                    bang.MaNguoiDungSua = User.Identity.Name;
        //                    bang.IPSua = Request.UserHostAddress;

        //                    bang.Save();
        //                }
        //            }
        //        }
        //        return RedirectToAction("NguoiDung_DonVi_Edit_Frame", new { maND = maND, LuuThanhCong = "1" });

        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "PermitionMessage");
        //    }
        //}
    }
}
