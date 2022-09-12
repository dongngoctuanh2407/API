using System.Collections.Generic;
using System.Web.Mvc;
using VIETTEL.Models;

namespace VIETTEL.Helpers
{
    public static class DataHelper
    {
        public static SelectList ToSelectList(this IEnumerable<KeyViewModel> source)
        {
            return new SelectList(source, "Key", "Text");
        }

        public static IEnumerable<KeyViewModel> GetQuys()
        {
            return new List<KeyViewModel>()
            {
                new KeyViewModel("1", "Qúy I"),
                new KeyViewModel("2", "Qúy II"),
                new KeyViewModel("3", "Qúy III"),
                new KeyViewModel("4", "Qúy IV"),
                new KeyViewModel("5", "Bổ sung"),
            };
        }

        public static IEnumerable<KeyViewModel> GetNamNganSachList()
        {
            return new List<KeyViewModel>()
            {
                new KeyViewModel("1,2,4,5", "Tổng hợp"),
                new KeyViewModel("2,4", "Năm nay"),
                new KeyViewModel("1,5", "Năm trước"),
            };
        }

        public static Dictionary<string, string> GetNguonNganSachList()
        {
            return new Dictionary<string, string>()
            {
                {"-1", "Tổng hợp" },
                {"1", "1 - Ngân sách quốc phòng" },
                {"2,3", "2 - Ngân sách nhà nước" },
                {"4", "4 - Ngân sách khác" },
            };
        }
    }
}
