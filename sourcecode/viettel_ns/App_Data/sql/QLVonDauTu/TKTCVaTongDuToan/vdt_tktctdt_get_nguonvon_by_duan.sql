SELECT dtnv.iID_NguonVonID, ns.sTen as sTenNguonVon, CAST(0 as float) as fTienPheDuyet, CAST(0 as float) as fGiaTriDieuChinh, fTienPheDuyet as fTienPheDuyetQDDT
FROM VDT_DA_DuToan_NguonVon as dtnv
INNER JOIN VDT_DA_DuToan as dt on dt.iID_DuToanID = dtnv.iID_DuToanID
INNER JOIN NS_NguonNganSach as ns on dtnv.iID_NguonVonID = ns.iID_MaNguonNganSach
WHERE  dt.bActive = 1 AND dt.iID_DuAnID = @iIdDuAnId



