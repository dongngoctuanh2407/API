using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VIETTEL.Common
{
    public static class Constants
    {
        public const byte ACTIVED = 1;

        public const byte DELETED = 0;

        public const string TAT_CA = "--Tất cả--";

        public const int ITEMS_PER_PAGE = 20;

        public const byte TONG_DU_TOAN = 1;

        public const byte DU_TOAN = 0;

        public const int LA_CHON = -1;

        public const string CHON = "--Chọn--";

        public const string LA_TONG_DU_TOAN = "Là tổng dự toán";

        public const string LA_DU_TOAN = "Là dự toán";

        public const string CHON_DON_VI_NS = "--Chọn đơn vị NS--";

        public const string CHON_DON_VI_BHXH = "--Chọn đơn vị BHXH--";

        public enum LOAI_HOP_DONG
        {
            HOP_DONG_GIAO_VIEC = 0,
            HOP_DONG_KINH_TE = 1
        };

        public enum LOAI_QUYET_DINH
        {
            DU_TOAN = 0,
            TONG_DU_TOAN = 1
        };

        public struct PTDauThauTypeName
        {
            public const string PT_1 = "1 Giai đoạn 1 túi hồ sơ";
            public const string PT_2 = "1 Giai đoạn 2 túi hồ sơ";
            public const string PT_3 = "2 Giai đoạn 1 túi hồ sơ";
            public const string PT_4 = "2 Giai đoạn 2 túi hồ sơ";
        }

        public struct HTChonNhaThauTypeName
        {
            public const string HT_1 = "Đấu thầu rộng rãi";
            public const string HT_2 = "Đấu thầu hạn chế";
            public const string HT_3 = "Chỉ định thầu";
            public const string HT_4 = "Chào hàng cạnh tranh";
            public const string HT_5 = "Mua sắm trực tiếp";
            public const string HT_6 = "Tự thực hiện";
            public const string HT_7 = "Lựa chọn NT, NĐT trong trường hợp đặc biệt";
            public const string HT_8 = "Tham gia thực hiện của cộng đồng";
            public const string HT_9 = "Tham gia thực hiện của cộng đồng";
            public const string HT_10 = "Chỉ định thầu rút gọn";
        }

        public struct HTHopDongTypeName
        {
            public const string HD_1 = "Hợp đồng trọn gói";
            public const string HD_2 = "Hợp đồng theo đơn giá cố định";
            public const string HD_3 = "Hợp đồng theo đơn giá điều chỉnh";
        }

        public enum LOAI_DON_VI
        {
            DOANH_NGHIEP = 0,
            DON_VI_DU_TOAN = 1
        };

        public static class CoQuanThanhToan
        {
            public enum Type
            {
                KHO_BAC = 1,
                CQTC = 2
            }
            public struct TypeName
            {
                public static string KHO_BAC = "Kho bạc";
                public static string CQTC = "Cơ quan tài chính bộ quốc phòng";
            }
        }

        public static class LoaiQuy
        {
            public enum Type
            {
                QUY_1 = 1,
                QUY_2 = 2,
                QUY_3 = 3,
                QUY_4 = 4
            }

            public struct TypeName
            {
                public const string QUY_1 = "Quý I";
                public const string QUY_2 = "Quý II";
                public const string QUY_3 = "Quý III";
                public const string QUY_4 = "Quý IV";
            }
        }

        public static class LoaiThanhToan
        {
            public enum Type
            {
                THANH_TOAN = 1,
                TAM_UNG = 2,
                THU_HOI = 3
            }

            public struct TypeName
            {
                public const string THANH_TOAN = "Thanh toán";
                public const string TAM_UNG = "Tạm ứng";
                public const string THU_HOI = "Thu hồi ứng";
            }

            public static string Get(int type)
            {
                switch (type)
                {
                    case (int)Type.THANH_TOAN:
                        return TypeName.THANH_TOAN;
                    case (int)Type.TAM_UNG:
                        return TypeName.TAM_UNG;
                    case (int)Type.THU_HOI:
                        return TypeName.THU_HOI;
                }
                return string.Empty;
            }
        }

        public static class LoaiNamKeHoach
        {
            public enum Type
            {
                NAM_TRUOC = 1,
                NAM_NAY = 2,
                NAM_SAU = 3
            }

            public struct TypeName
            {
                public const string NAM_TRUOC = "Năm trước";
                public const string NAM_NAY = "Năm nay";
                public const string NAM_SAU = "Năm sau";
            }
        }

        public struct LOAI_CHUNG_TU
        {
            public static string CHU_DAU_TU = "000";
            public static string NAM_TRUOC_CHUYEN_SANG = "NAM_TRUOC_CHUYEN";

            public static string KE_HOACH_VON_NAM = "KHVN";
            public static string KHVN_KHOBAC = "101";
            public static string KHVN_LENHCHI = "102";
            public static string KHVN_THUHOI_KHOBAC_NAMTRUOC = "121b";
            public static string KHVN_THUHOI_LENHCHI_NAMTRUOC = "122b";

            public static string KE_HOACH_VON_UNG = "KHVU";
            public static string KHVU_KHOBAC = "121a";
            public static string KHVU_LENHCHI = "122a";

            public static string CAP_THANH_TOAN = "THANH_TOAN";
            public static string TT_THANHTOAN_KHOBAC = "201";
            public static string TT_THANHTOAN_LENHCHI = "202";
            public static string TT_UNG_KHOBAC = "211a";
            public static string TT_UNG_LENHCHI = "212a";

            public static string TT_THUHOI_KHOBAC_NAMTRUOC = "211b1";
            public static string TT_THUHOI_LENHCHI_NAMTRUOC = "212b1";
            public static string TT_THUHOI_KHOBAC_NAMNAY = "211b2";
            public static string TT_THUHOI_LENHCHI_NAMNAY = "212b2";

            public static string QUYET_TOAN = "QUYET_TOAN";
            public static string QT_KHOBAC_CHUYENNAMTRUOC = "111";
            public static string QT_LENHCHI_CHUYENNAMTRUOC = "112";
            public static string QT_UNG_KHOBAC_CHUYENNAMTRUOC = "131";
            public static string QT_UNG_LENHCHI_CHUYENNAMTRUOC = "132";
        }

        public enum LoaiXuLy
        {
            TaoMoi = 1,
            CapNhat = 2,
            Xoa = 3,
            DieuChinh = 4
        }

        public static class LoaiNganSach
        {
            public enum Type
            {
                CHI_NGAN_SACH_NHA_NUOC = 0,
                CHI_THUONG_XUYEN_QP = 1
            }

            public struct TypeName
            {
                public const string CHI_NGAN_SACH_NHA_NUOC = "Chi ngân sách nhà nước";
                public const string CHI_THUONG_XUYEN_QP  = "Chi thường xuyên quốc phòng";
            }

            public static string Get(int type)
            {
                switch (type)
                {
                    case (int)Type.CHI_NGAN_SACH_NHA_NUOC:
                        return TypeName.CHI_NGAN_SACH_NHA_NUOC;
                    case (int)Type.CHI_THUONG_XUYEN_QP:
                        return TypeName.CHI_THUONG_XUYEN_QP;
                }
                return string.Empty;
            }
        }

        public struct DuToanType
        {
            public enum Type 
            { 
                TONG_DU_TOAN = 1,
                DU_TOAN = 0
            }

            public struct TypeName
            {
                public const string TONG_DU_TOAN = "Tổng dự toán";
                public const string DU_TOAN = "Dự toán";
            }
        }

        public struct CanCuType
        {
            public enum Type
            {
                TKTC_TONG_DU_TOAN = 1,
                QUYET_DINH_DAU_TU = 2,
                CHU_TRUONG_DAU_TU = 3
            }

            public struct TypeName
            {
                public const string TKTC_TONG_DU_TOAN = "TKTC và tổng dự toán";
                public const string QUYET_DINH_DAU_TU = "Phê duyệt dự án";
                public const string CHU_TRUONG_DAU_TU = "Chủ trương đầu tư";
            }
        }
    }
}