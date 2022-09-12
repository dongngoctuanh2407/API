using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Viettel.Domain.DomainModel;
using Viettel.Services;
using VIETTEL.Models;

namespace VIETTEL.Common
{
    public static class CommonFunction
    {
        private static readonly IQLVonDauTuService _vdtService = QLVonDauTuService.Default;
        private static readonly INganSachNewService _nsService = NganSachNewService.Default;

        public static SelectList GetDataDropDownDonViQuanLy(string[] lstDonViExclude = null)
        {
            var lstData = DonViModels.Get_dtDonVi();
            List<SelectListItem> lstCbx = new List<SelectListItem>();
            foreach (DataRow dr in lstData.Rows)
            {
                if (lstDonViExclude != null && !lstDonViExclude.Contains(Convert.ToString(dr["iCapNS"]))) continue;
                lstCbx.Add(new SelectListItem()
                {
                    Text = Convert.ToString(dr["sTen"]),
                    Value = Convert.ToString(dr["iID_Ma"]) + "|" + Convert.ToString(dr["iID_MaDonVi"])
                });
            }   
            return lstCbx.ToSelectList();
        }

        public static SelectList GetDataDropDownNguonNganSach()
        {
            var lstData = DanhMucModels.NS_NguonNganSach();
            if (lstData == null || lstData.Rows.Count <= 0) return new SelectList(null);
            return lstData.ToSelectList("iID_MaNguonNganSach", "sTen");
        }

        public static SelectList GetDataDropDownMucLucNganSach(int iNamLamViec)
        {
            var lstData = _nsService.GetDataDropdownMucLucNganSachInVDT(iNamLamViec);
            List<SelectListItem> lstCbx = new List<SelectListItem>();
            foreach (var item in lstData)
            {
                lstCbx.Add(new SelectListItem()
                {
                    Text = item.sXauNoiMa,
                    Value = GetValueDropdownMLNS(item)
                });
            }
            return lstCbx.ToSelectList();
        }

        #region Helper
        private static string GetValueDropdownMLNS(NS_MucLucNganSach item)
        {
            if (!string.IsNullOrEmpty(item.sNG))
                return string.Format("{0}|{1}", "Ng", item.iID_MaMucLucNganSach.ToString());
            if (!string.IsNullOrEmpty(item.sTTM))
                return string.Format("{0}|{1}", "Ttm", item.iID_MaMucLucNganSach.ToString());
            if (!string.IsNullOrEmpty(item.sTM))
                return string.Format("{0}|{1}", "Tm", item.iID_MaMucLucNganSach.ToString());
            if (!string.IsNullOrEmpty(item.sM))
                return string.Format("{0}|{1}", "M", item.iID_MaMucLucNganSach.ToString());
            return string.Empty;
        }
        #endregion
    }
}