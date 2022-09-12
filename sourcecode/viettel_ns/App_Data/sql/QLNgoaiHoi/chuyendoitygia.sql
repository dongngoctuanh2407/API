DECLARE @loaitiennhap int 		set @loaitiennhap = 1 /* 1 - Chuy?n ??i tù USD -> VND, 2: Chuy?n d?i t? VND -> USD*/
DECLARE @sotiennhap float		set @sotiennhap = 1000
DECLARE @matygia uniqueidentifier		set @matygia = '00000000-0000-0000-0000-000000000000'
--#DECLARE#--

/* L?y danh sách m?c l?c ngân sách theo tham s? truy?n vào*/

declare @matientegoc nvarchar(20);
declare @tygia float;
declare @tienchuyendoi float;

set @matientegoc = (select sMaTienTeGoc from  NH_DM_TiGia as tg where ID = @matygia);

/*N?u lo?i ti?n nh?p là USD thì chuy?n sang VND và ng??c l?i*/

	if @loaitiennhap = 1
			begin
				if @matientegoc = 'USD'
					begin
						set @tygia = (select fTiGia from NH_DM_TiGia as tg
											left join NH_DM_TiGia_ChiTiet as ct on tg.ID = ct.iID_TiGiaID
											where tg.ID = @matygia and sMaTienTeQuyDoi ='VND');
						set @tienchuyendoi = @sotiennhap * @tygia;
					end
				else 
					begin
						if(@matientegoc = 'VND')
							begin
								set @tygia = (select fTiGia from NH_DM_TiGia as tg
												left join NH_DM_TiGia_ChiTiet as ct on tg.ID = ct.iID_TiGiaID
												where tg.ID = @matygia and sMaTienTeQuyDoi ='USD')
								set @tienchuyendoi = @sotiennhap / @tygia;
							end
						
						else
							begin
								--chuy?n ??i t? USD -> t? giá ???c ch?n
								set @tygia = (select fTiGia from NH_DM_TiGia as tg
												left join NH_DM_TiGia_ChiTiet as ct on tg.ID = ct.iID_TiGiaID
												where tg.ID = @matygia and sMaTienTeQuyDoi ='USD')
								set @tienchuyendoi = @sotiennhap / @tygia;

								---chuy?n ??i t? t? giá ???c ch?n sang vnd
								set @tygia = (select fTiGia from NH_DM_TiGia as tg
												left join NH_DM_TiGia_ChiTiet as ct on tg.ID = ct.iID_TiGiaID
												where tg.ID = @matygia and sMaTienTeQuyDoi ='VND')

								set @tienchuyendoi =  @tienchuyendoi * @tygia;
							end
					end;
			end;

	else
		  
		  begin
			if @matientegoc = 'USD'
				begin
					set @tygia = (select fTiGia from NH_DM_TiGia as tg
										left join NH_DM_TiGia_ChiTiet as ct on tg.ID = ct.iID_TiGiaID
										where tg.ID = @matygia and sMaTienTeQuyDoi ='VND');
					set @tienchuyendoi = @sotiennhap /  @tygia;
				end
			else 
				begin
					if(@matientegoc = 'VND')
						begin
							set @tygia = (select fTiGia from NH_DM_TiGia as tg
											left join NH_DM_TiGia_ChiTiet as ct on tg.ID = ct.iID_TiGiaID
											where tg.ID = @matygia and sMaTienTeQuyDoi ='USD')
							set @tienchuyendoi = @sotiennhap * @tygia;
						end
						
					else
						begin
							--chuy?n ??i t? VND -> t? giá ???c ch?n
							set @tygia = (select fTiGia from NH_DM_TiGia as tg
											left join NH_DM_TiGia_ChiTiet as ct on tg.ID = ct.iID_TiGiaID
											where tg.ID = @matygia and sMaTienTeQuyDoi ='VND')
							set @tienchuyendoi = @sotiennhap / @tygia;

							---chuy?n ??i t? t? giá ???c ch?n sang USD
							set @tygia = (select fTiGia from NH_DM_TiGia as tg
											left join NH_DM_TiGia_ChiTiet as ct on tg.ID = ct.iID_TiGiaID
											where tg.ID = @matygia and sMaTienTeQuyDoi ='USD')

							set @tienchuyendoi =  @tienchuyendoi * @tygia;
						end
				end;
		end;

select @tienchuyendoi;