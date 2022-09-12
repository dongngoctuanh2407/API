DECLARE @id  uniqueidentifier		set @id =  '00000000-0000-0000-0000-000000000000'

--#DECLARE#--

select tt.*, dv.sTen,cdt.sTenCDT,ct.sTenNhiemVuChi, nt.sTenNhaThau,
		fTongCTDeNghiCapKyNay_USD, 
		fTongCTDeNghiCapKyNay_VND, 
		fTongCtPheDuyetCapKyNay_USD, 
		fTongCTPheDuyetCapKyNay_VND,
		sTenDuAn,
		sTenHopDong
		from NH_TT_ThanhToan as tt 
		left join NS_DonVi as dv on  tt.iID_DonVi = dv.iID_Ma
		left join NH_KHChiTietBQP_NhiemVuChi as ct on ct.ID = tt.iID_KHCTBQP_NhiemVuChiID
		left join DM_ChuDauTu as cdt on cdt.ID = tt.iID_ChuDauTuID
		left join NH_DM_NhaThau as nt on nt.ID= tt.iID_NhaThauID
		left join NH_DA_DuAn as da on da.ID = tt.iID_DuAnID
		left join NH_DA_HopDong as hd on hd.ID = tt.iID_HopDongID
		
		left join (
			select 
			Sum(fDeNghiCapKyNay_USD) as fTongCTDeNghiCapKyNay_USD, 
			Sum(fDeNghiCapKyNay_VND)   as fTongCTDeNghiCapKyNay_VND, 
			Sum(fPheDuyetCapKyNay_USD)  as fTongCtPheDuyetCapKyNay_USD, 
			Sum(fPheDuyetCapKyNay_VND)  as fTongCTPheDuyetCapKyNay_VND,
			tt_ct.iID_ThanhToanID
			from NH_TT_ThanhToan_ChiTiet as tt_ct
			group by tt_ct.iID_ThanhToanID
		) as chitiet on chitiet.iID_ThanhToanID = tt.ID

		where tt.ID =  @id