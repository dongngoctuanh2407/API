
IF(@iLoaiChungTu = 1)
BEGIN
	SELECT DISTINCT dt.iID_DuToanID as id, dt.sSoQuyetDinh, dt.dNgayQuyetDinh, dv.sTen as sTenDonVi, ISNULL(dt.fTongDuToanPheDuyet, 0) as fGiaTriPheDuyet
	FROM VDT_DA_DuToan as dt
	INNER JOIN VDT_DA_DuAn as da on dt.iID_DuAnID = da.iID_DuAnID
	INNER JOIN NS_DonVi as dv on da.iID_DonViQuanLyID = dv.iID_Ma
	WHERE dt.iID_DuAnID = @iIdDuAnId AND dt.bActive = 1
END
ELSE
BEGIN
	SELECT dt.iID_QDDauTuID as id, dt.sSoQuyetDinh, dt.dNgayQuyetDinh, dv.sTen as sTenDonVi, ISNULL(dt.fTongMucDauTuPheDuyet, 0) as fGiaTriPheDuyet
	FROM VDT_DA_QDDauTu as dt
	INNER JOIN VDT_DA_DuAn as da on dt.iID_DuAnID = da.iID_DuAnID
	INNER JOIN NS_DonVi as dv on da.iID_DonViQuanLyID = dv.iID_Ma
	WHERE dt.iID_DuAnID = @iIdDuAnId AND dt.bActive = 1
END
