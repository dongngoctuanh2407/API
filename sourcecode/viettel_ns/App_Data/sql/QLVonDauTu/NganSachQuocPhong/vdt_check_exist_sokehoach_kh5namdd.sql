
IF(@iID_KeHoach5NamID IS NULL)
BEGIN
	SELECT COUNT(iID_KeHoach5NamID) FROM VDT_KHV_KeHoach5Nam WHERE sSoQuyetDinh = @sSoQuyetDinh AND iNamLamViec = @iNamLamViec
END
ELSE
BEGIN
	SELECT COUNT(iID_KeHoach5NamID) FROM VDT_KHV_KeHoach5Nam WHERE sSoQuyetDinh = @sSoQuyetDinh AND iID_KeHoach5NamID <> @iID_KeHoach5NamID AND iNamLamViec = @iNamLamViec
END
