SELECT NCCQ.iID_DonViID , CT.*, (CT.fChiNgoaiTeUSD/@fUSD) as fChiNgoaiTeUSDTyGia , (CT.fChiTrongNuocVND/@fVND) as fChiTrongNuocVNDTyGia ,
Case When CT.iID_HopDongID is not null then HD.sTenHopDong
 		Else CT.sNoiDung
	End as sTenHopDong ,
	NS_DonVi.sTen as sTenDonvi , BQP.ID as ID_NhiemVuChi,
	Case When BQP.sTenNhiemVuChi is not null then BQP.sTenNhiemVuChi
 		Else N'Chưa có chương trình'
	End as sTenNhiemVuChi,
Case 
	when HD.iID_DuAnID is null then (HD.fGiaTriUSD/@fUSD)
	else (DA.fGiaTriUSD/@fUSD)
	End as GiaTriHopDongUSD ,
Case 
	when HD.iID_DuAnID is null then (HD.fGiaTriVND/@fVND)
	else (DA.fGiaTriVND/@fVND)
	End as GiaTriHopDongVND,
	(TTCPTongThe.fGiaTri/@fUSD) as GiaTriTongTheUSD,
	(TTCPGiaiDon.fGiaTri/@fUSD) as GiaTriGaiDoanUSD,
	(BQP.fGiaTriUSD/@fUSD) as GiaTriBQPUSD,
	(QTND.KinhPhiUSD/@fUSD) as KinhPhiUSD , (QTND.KinhPhiVND/@fVND) as KinhPhiVND,
	(KPDC.KinhPhiDaChiUSD/@fUSD) as KinhPhiDaChiUSD , (KPDCVND.KinhPhiDaChiVND/@fVND) as KinhPhiDaChiVND,
	(QTNDToY.KinhPhiToYUSD/@fUSD) as KinhPhiToYUSD , (QTNDToY.KinhPhiToYVND/@fVND) as KinhPhiToYVND,
	(KPDCToY.KinhPhiDaChiUSD/@fUSD) as KinhPhiDaChiToYUSD , (KPDCVNDToY.KinhPhiDaChiVND/@fVND) as KinhPhiDaChiToYVND
FROM NH_NhuCauChiQuy_ChiTiet CT
left join NH_NhuCauChiQuy NCCQ on NCCQ.ID = CT.iID_NhuCauChiQuyID  
left join NH_DA_HopDong HD on HD.ID = CT.iID_HopDongID  
left join NH_DA_DuAn DA on DA.ID = HD.iID_DuAnID 
left join NS_DonVi on NS_DonVi.iID_Ma = NCCQ.iID_DonViID  
left join NH_KHChiTietBQP_NhiemVuChi BQP on BQP.ID = HD.iID_KHCTBQP_ChuongTrinhID  
left join NH_KHTongTheTTCP_NhiemVuChi TTCPTongThe on TTCPTongThe.ID = BQP.iID_ParentID   
left join NH_KHTongTheTTCP_NhiemVuChi TTCPGiaiDon on TTCPGiaiDon.ID = BQP.iID_KHTTTTCP_NhiemVuChiID   
left join (
		Select NDCT.iID_HopDongID, Sum(NDCT.fQTKinhPhiDuocCap_TongSo_USD) as KinhPhiUSD, sum(NDCT.fQTKinhPhiDuocCap_TongSo_VND) as KinhPhiVND
		from NH_QT_QuyetToanNienDo_ChiTiet NDCT group by NDCT.iID_HopDongID)
	QTND on QTND.iID_HopDongID = HD.ID
left join (
		select ThanhToan.iLoaiDeNghi , ThanhToan.iID_HopDongID , sum(ThanhToan.fChuyenKhoan_BangSo) as KinhPhiDaChiUSD from NH_TT_ThanhToan ThanhToan
		join NH_TT_ThanhToan_ChiTiet b 
		on ThanhToan.ID = b.iID_ThanhToanID 
		where ThanhToan.iLoaiNoiDungChi = 1 and ThanhToan.iNamKeHoach = DATEPART(YEAR,GETDATE()) and ThanhToan.dNgayDeNghi < convert(datetime,(concat(year(GETDATE())-1,'-12-31 00:00:00.000')))
		Group BY ThanhToan.iLoaiDeNghi, ThanhToan.iID_HopDongID having ThanhToan.iLoaiDeNghi = 2 or ThanhToan.iLoaiDeNghi = 3
	) KPDC on KPDC.iID_HopDongID = HD.ID
left join (
		select ThanhToan.iLoaiDeNghi , ThanhToan.iID_HopDongID , sum(ThanhToan.fChuyenKhoan_BangSo) as KinhPhiDaChiVND from NH_TT_ThanhToan ThanhToan
		join NH_TT_ThanhToan_ChiTiet b 
		on ThanhToan.ID = b.iID_ThanhToanID 
		where ThanhToan.iLoaiNoiDungChi = 2 and ThanhToan.iNamKeHoach = DATEPART(YEAR,GETDATE()) and ThanhToan.dNgayDeNghi < convert(datetime,(concat(year(GETDATE())-1,'-12-31 00:00:00.000')))
		Group BY ThanhToan.iLoaiDeNghi, ThanhToan.iID_HopDongID having ThanhToan.iLoaiDeNghi = 2 or ThanhToan.iLoaiDeNghi = 3
	) KPDCVND on KPDCVND.iID_HopDongID = HD.ID
left join (
	Select NDCT.iID_HopDongID, Sum(NDCT.fQTKinhPhiDuocCap_TongSo_USD) as KinhPhiToYUSD, sum(NDCT.fQTKinhPhiDuocCap_TongSo_VND) as KinhPhiToYVND
		from NH_QT_QuyetToanNienDo_ChiTiet NDCT
		left join NH_QT_QuyetToanNienDo NDCTParent on NDCTParent.ID = NDCT.iID_QuyetToanNienDoID
		Where NDCTParent.iNamKeHoach = DATEPART(YEAR,GETDATE())
		group by NDCT.iID_HopDongID) QTNDToY on QTNDToY.iID_HopDongID = HD.ID
left join (
		select ThanhToan.iLoaiDeNghi , ThanhToan.iID_HopDongID , sum(ThanhToan.fChuyenKhoan_BangSo) as KinhPhiDaChiUSD from NH_TT_ThanhToan ThanhToan
		join NH_TT_ThanhToan_ChiTiet b 
		on ThanhToan.ID = b.iID_ThanhToanID 
		where ThanhToan.iLoaiNoiDungChi = 1 and ThanhToan.dNgayDeNghi > convert(datetime,(concat(year(GETDATE()),'-1-1 00:00:00.000'))) and 
			((ThanhToan.dNgayDeNghi < convert(datetime,(concat(year(GETDATE()),'-9-30 00:00:00.000'))) and DATEPART(quarter,GETDATE()) = 4)
			or (ThanhToan.dNgayDeNghi < convert(datetime,(concat(year(GETDATE()),'-6-30 00:00:00.000'))) and DATEPART(quarter,GETDATE()) = 3)
			or (ThanhToan.dNgayDeNghi < convert(datetime,(concat(year(GETDATE()),'-3-31 00:00:00.000'))) and DATEPART(quarter,GETDATE()) = 2)
			)
		Group BY ThanhToan.iLoaiDeNghi, ThanhToan.iID_HopDongID having ThanhToan.iLoaiDeNghi = 2 or ThanhToan.iLoaiDeNghi = 3
	) KPDCToY on KPDCToY.iID_HopDongID = HD.ID
left join (
		select ThanhToan.iLoaiDeNghi , ThanhToan.iID_HopDongID , sum(ThanhToan.fChuyenKhoan_BangSo) as KinhPhiDaChiVND from NH_TT_ThanhToan ThanhToan
		join NH_TT_ThanhToan_ChiTiet b 
		on ThanhToan.ID = b.iID_ThanhToanID 
		where ThanhToan.iLoaiNoiDungChi = 2 and ThanhToan.dNgayDeNghi > convert(datetime,(concat(year(GETDATE()),'-1-1 00:00:00.000'))) and 
			((ThanhToan.dNgayDeNghi < convert(datetime,(concat(year(GETDATE()),'-9-30 00:00:00.000'))) and DATEPART(quarter,GETDATE()) = 4)
			or (ThanhToan.dNgayDeNghi < convert(datetime,(concat(year(GETDATE()),'-6-30 00:00:00.000'))) and DATEPART(quarter,GETDATE()) = 3)
			or (ThanhToan.dNgayDeNghi < convert(datetime,(concat(year(GETDATE()),'-3-31 00:00:00.000'))) and DATEPART(quarter,GETDATE()) = 2)
			)
		Group BY ThanhToan.iLoaiDeNghi, ThanhToan.iID_HopDongID having ThanhToan.iLoaiDeNghi = 2 or ThanhToan.iLoaiDeNghi = 3
	) KPDCVNDToY on KPDCVNDToY.iID_HopDongID = HD.ID
Where (NCCQ.iID_BQuanLyID = @iID_DonViID or @iID_DonViID is null) and (NCCQ.iQuy = @iQuy or @iQuy = 0 or @iQuy is null)
      and (NCCQ.iNamKeHoach = @iNam or @iNam is null or @iNam = 0)
ORDER BY ID_NhiemVuChi desc,sTenNhiemVuChi, sTenDonvi , iID_NhuCauChiQuyID;
