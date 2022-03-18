if OBJECT_ID('zzz_view_SaleOrderQ','v') is not null
drop view zzz_view_SaleOrderQ
go
create view zzz_view_SaleOrderQ 
as
Select a.*,
b.id as keyextend_t_id_so_somain_extradefine_id,chdefine1,chdefine2,chdefine3,chdefine4,chdefine5,chdefine6,chdefine7,chdefine10,chdefine23,chdefine8,chdefine9,chdefine22,chdefine11,chdefine12,chdefine13,chdefine14,chdefine15,chdefine16,chdefine17,chdefine18,chdefine19,chdefine20,chdefine21,chdefine24,chdefine25,chdefine26,chdefine27,chdefine28 
from SaleOrderQ  a
left join so_somain_extradefine b on a.id=b.id
go

if OBJECT_ID('zzz_view_SaleOrdersQ','v') is not null
drop view zzz_view_SaleOrdersQ
go
create view zzz_view_SaleOrdersQ 
as
Select a.*,
b.iSOsID as keyextend_b_isosid_so_sodetails_extradefine_isosid ,N'' as editprop,cbdefine3,cbdefine1,cbdefine2,cbdefine4,cbdefine5,cbdefine6,cbdefine7,cbdefine8,cbdefine9,cbdefine10,cbdefine11,cbdefine12,cbdefine13,cbdefine14,cbdefine15,cbdefine16,cbdefine17,cbdefine18,cbdefine19,cbdefine20 
From SaleOrderSQ a
left join so_sodetails_extradefine b on a.isosid=b.iSOsID


--select * from zzz_view_SaleOrderQ where 1=0
--select * from zzz_view_SaleOrdersQ where 1=0