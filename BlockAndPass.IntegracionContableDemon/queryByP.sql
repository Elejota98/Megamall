DECLARE @FechaCosulta DATETIME2 = 'rMES-rDIA-rANHO'
DECLARE @IdEstacionamientoConsulta bigint = rIdEstacionamiento
Declare @CuentaContableBanco varchar(50) = 'rCuentaContable'
Declare @DocumentoBanco varchar(50) = 'rDocumento'

--------------------EFECTIVO RECAUDO------------------
select 01 as 'CENTRO DE COSTO',
 'FV01' as  'DOCUMENTO',
 CONVERT(varchar(10),convert(int, DAY(p.FechaPago))) + REPLACE(str(month(p.fechapago),2),SPACE(1), '0') + '01' as 'NUMERO',
 REPLACE(str(day(p.fechapago),2),SPACE(1), '0') + '/' + REPLACE(str(month(p.fechapago),2),SPACE(1), '0') + '/' + CONVERT(varchar(10),year(p.FechaPago)) as 'FECHA',
 '110525' as 'CUENTA CONTABLE',
 'DEBITO' as 'NATURALEZA',
 '888.888.888' as 'IDENTIFICACION',
 sum(p.Total) as 'TOTAL',
  e.Nombre + ' ' +  REPLACE(str(day(p.fechapago),2),SPACE(1), '0') + '/' + REPLACE(str(month(p.fechapago),2),SPACE(1), '0') as 'CONCEPTO',
  'EFECTIVO' as 'ESTESEBORRA'
from T_Pagos as p
inner join T_Estacionamientos as e
on p.idestacionamiento=e.idestacionamiento
where day(p.fechapago)=day(@FechaCosulta) and month(p.fechapago)=MONTH(@FechaCosulta) and YEAR(p.FechaPago) = year(@FechaCosulta) and e.IdEstacionamiento = @IdEstacionamientoConsulta
group by day(p.fechapago), month(p.fechapago), year(p.fechapago), e.Nombre

union ----------------------- BANCO -------------

select 03 as 'CENTRO DE COSTO',
 @DocumentoBanco as  'DOCUMENTO',
 CONVERT(varchar(10),convert(int, DAY(a.fechafin))) + REPLACE(str(month(a.fechafin),2),SPACE(1), '0') + '01' as 'NUMERO',
 REPLACE(str(day(a.fechafin),2),SPACE(1), '0') + '/' + REPLACE(str(month(a.fechafin),2),SPACE(1), '0') + '/' + CONVERT(varchar(10),year(a.fechafin)) as 'FECHA',
 @CuentaContableBanco as 'CUENTA CONTABLE',
 'DEBITO' as 'NATURALEZA',
 '888.888.888' as 'IDENTIFICACION',
 case when sum(a.conteo) IS NULL THEN 0 ELSE sum(a.conteo) end as 'TOTAL',
  e.Nombre + ' ' +  REPLACE(str(day(a.fechafin),2),SPACE(1), '0') + '/' + REPLACE(str(month(a.fechafin),2),SPACE(1), '0') as 'CONCEPTO',
  'BANCO' as 'ESTESEBORRA'
from t_arqueos as a
inner join
T_Estacionamientos as e
on e.IdEstacionamiento=a.IdEstacionamiento
where day(a.fechafin)=day(@FechaCosulta) and month(a.fechafin)=MONTH(@FechaCosulta) and YEAR(a.fechafin) = year(@FechaCosulta) and a.IdEstacionamiento = @IdEstacionamientoConsulta
group by day(a.fechafin), month(a.fechafin), year(a.fechafin), e.Nombre

union ------------------------IVA-----------------------------

select 09 as 'CENTRO DE COSTO',
 'FV09' as  'DOCUMENTO',
 CONVERT(varchar(10),convert(int, DAY(p.FechaPago))) + REPLACE(str(month(p.fechapago),2),SPACE(1), '0') + '01' as 'NUMERO',
 REPLACE(str(day(p.fechapago),2),SPACE(1), '0') + '/' + REPLACE(str(month(p.fechapago),2),SPACE(1), '0') + '/' + CONVERT(varchar(10),year(p.FechaPago)) as 'FECHA',
 '24080519' as 'CUENTA CONTABLE',
 'CREDITO' as 'NATURALEZA',
 '888.888.888' as 'IDENTIFICACION',
 sum(p.Iva) as 'TOTAL',
  
  (select STUFF((
	select p.IdModulo + ' ' + min(numerofactura)+'-'+max(numerofactura) + '; '
	from T_Pagos as p
	where day(p.fechapago)=day(@FechaCosulta) and month(p.fechapago)=MONTH(@FechaCosulta) and YEAR(p.FechaPago) = year(@FechaCosulta) and p.IdEstacionamiento = @IdEstacionamientoConsulta
	group by p.IdModulo
	FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, ''))

   as 'CONCEPTO',
   'IVA' as 'ESTESEBORRA'
from T_Pagos as p
inner join T_TipoPago as tp
on p.IdTipoPago = tp.IdTipoPago
inner join T_Estacionamientos as e
on p.idestacionamiento=e.idestacionamiento
where day(p.fechapago)=day(@FechaCosulta) and month(p.fechapago)=MONTH(@FechaCosulta) and YEAR(p.FechaPago) = year(@FechaCosulta) and e.IdEstacionamiento = @IdEstacionamientoConsulta
group by day(p.fechapago), month(p.fechapago), year(p.fechapago), e.Nombre

union ---------------------------ESTACIONAMIENTO CARROS--------------------------

select 11 as 'CENTRO DE COSTO',
 'FV11' as  'DOCUMENTO',
 CONVERT(varchar(10),convert(int, DAY(p.FechaPago))) + REPLACE(str(month(p.fechapago),2),SPACE(1), '0') + '01' as 'NUMERO',
 REPLACE(str(day(p.fechapago),2),SPACE(1), '0') + '/' + REPLACE(str(month(p.fechapago),2),SPACE(1), '0') + '/' + CONVERT(varchar(10),year(p.FechaPago)) as 'FECHA',
 '41709001' as 'CUENTA CONTABLE',
 'CREDITO' as 'NATURALEZA',
 '888.888.888' as 'IDENTIFICACION',
 sum(p.Subtotal) as 'TOTAL',
  
  (select STUFF((
	select p.IdModulo + ' ' + min(numerofactura)+'-'+max(numerofactura) + '; '
	from T_Pagos as p
	where day(p.fechapago)=day(@FechaCosulta) and month(p.fechapago)=MONTH(@FechaCosulta) and YEAR(p.FechaPago) = year(@FechaCosulta) and p.IdEstacionamiento = @IdEstacionamientoConsulta
	group by p.IdModulo
	FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, ''))
  
   as 'CONCEPTO',
   'CARROS' as 'ESTESEBORRA'
from T_Pagos as p
inner join
T_Transacciones as t
on p.idtransaccion=t.idtransaccion
inner join T_TipoVehiculo as tv
on t.IdTipoVehiculo = tv.IdTipoVehiculo
inner join T_TipoPago as tp
on p.IdTipoPago = tp.IdTipoPago
inner join T_Estacionamientos as e
on t.idestacionamiento=e.idestacionamiento
where day(p.fechapago)=day(@FechaCosulta) and month(p.fechapago)=MONTH(@FechaCosulta) and YEAR(p.FechaPago) = year(@FechaCosulta) and tv.IdTipoVehiculo = 1 and tp.IdTipoPago = 1 and e.IdEstacionamiento = @IdEstacionamientoConsulta
group by day(p.fechapago), month(p.fechapago), year(p.fechapago), e.Nombre

union --------------------------ESTACIONAMIENTO MOTOS---------------------------

select 14 as 'CENTRO DE COSTO',
 'FV14' as  'DOCUMENTO',
 CONVERT(varchar(10),convert(int, DAY(p.FechaPago))) + REPLACE(str(month(p.fechapago),2),SPACE(1), '0') + '01' as 'NUMERO',
 REPLACE(str(day(p.fechapago),2),SPACE(1), '0') + '/' + REPLACE(str(month(p.fechapago),2),SPACE(1), '0') + '/' + CONVERT(varchar(10),year(p.FechaPago)) as 'FECHA',
 '41709003' as 'CUENTA CONTABLE',
 'CREDITO' as 'NATURALEZA',
 '888.888.888' as 'IDENTIFICACION',
 sum(p.Subtotal) as 'TOTAL',
  
  (select STUFF((
	select p.IdModulo + ' ' + min(numerofactura)+'-'+max(numerofactura) + '; '
	from T_Pagos as p
	where day(p.fechapago)=day(@FechaCosulta) and month(p.fechapago)=MONTH(@FechaCosulta) and YEAR(p.FechaPago) = year(@FechaCosulta) and p.IdEstacionamiento = @IdEstacionamientoConsulta
	group by p.IdModulo
	FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, ''))
  
   as 'CONCEPTO',
   'MOTOS' as 'ESTESEBORRA'
from T_Pagos as p
inner join
T_Transacciones as t
on p.idtransaccion=t.idtransaccion
inner join T_TipoVehiculo as tv
on t.IdTipoVehiculo = tv.IdTipoVehiculo
inner join T_TipoPago as tp
on p.IdTipoPago = tp.IdTipoPago
inner join T_Estacionamientos as e
on t.idestacionamiento=e.idestacionamiento
where day(p.fechapago)=day(@FechaCosulta) and month(p.fechapago)=MONTH(@FechaCosulta) and YEAR(p.FechaPago) = year(@FechaCosulta) and tv.IdTipoVehiculo = 2 and tp.IdTipoPago = 1 and e.IdEstacionamiento = @IdEstacionamientoConsulta
group by day(p.fechapago), month(p.fechapago), year(p.fechapago), e.Nombre

union --------------------MENSUALIDADES CARROS---------------------------------

select 19 as 'CENTRO DE COSTO',
 'FV19' as  'DOCUMENTO',
 CONVERT(varchar(10),convert(int, DAY(p.FechaPago))) + REPLACE(str(month(p.fechapago),2),SPACE(1), '0') + '01' as 'NUMERO',
 REPLACE(str(day(p.fechapago),2),SPACE(1), '0') + '/' + REPLACE(str(month(p.fechapago),2),SPACE(1), '0') + '/' + CONVERT(varchar(10),year(p.FechaPago)) as 'FECHA',
 '41709101' as 'CUENTA CONTABLE',
 'CREDITO' as 'NATURALEZA',
 '888.888.888' as 'IDENTIFICACION',
 sum(p.Subtotal) as 'TOTAL',
  
  (select STUFF((
	select p.IdModulo + ' ' + min(numerofactura)+'-'+max(numerofactura) + '; '
	from T_Pagos as p
	where day(p.fechapago)=day(@FechaCosulta) and month(p.fechapago)=MONTH(@FechaCosulta) and YEAR(p.FechaPago) = year(@FechaCosulta) and p.IdEstacionamiento = @IdEstacionamientoConsulta
	group by p.IdModulo
	FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, ''))
  
   as 'CONCEPTO',
   'MENSUALIDADES CARROS' as 'ESTESEBORRA'
from T_Pagos as p
where day(p.fechapago)=day(@FechaCosulta) and month(p.fechapago)=MONTH(@FechaCosulta) and YEAR(p.FechaPago) = year(@FechaCosulta) and p.IdTipoPago = 2 and p.IdEstacionamiento = @IdEstacionamientoConsulta and p.idautorizado=1
group by day(p.fechapago), month(p.fechapago), year(p.fechapago)

union  ------------------MENSUALIDADES MOTOS-----------------

select 20 as 'CENTRO DE COSTO',
 'FV20' as  'DOCUMENTO',
 CONVERT(varchar(10),convert(int, DAY(p.FechaPago))) + REPLACE(str(month(p.fechapago),2),SPACE(1), '0') + '01' as 'NUMERO',
 REPLACE(str(day(p.fechapago),2),SPACE(1), '0') + '/' + REPLACE(str(month(p.fechapago),2),SPACE(1), '0') + '/' + CONVERT(varchar(10),year(p.FechaPago)) as 'FECHA',
 '41709102' as 'CUENTA CONTABLE',
 'CREDITO' as 'NATURALEZA',
 '888.888.888' as 'IDENTIFICACION',
 sum(p.Subtotal) as 'TOTAL',
  
  (select STUFF((
	select p.IdModulo + ' ' + min(numerofactura)+'-'+max(numerofactura) + '; '
	from T_Pagos as p
	where day(p.fechapago)=day(@FechaCosulta) and month(p.fechapago)=MONTH(@FechaCosulta) and YEAR(p.FechaPago) = year(@FechaCosulta) and p.IdEstacionamiento = @IdEstacionamientoConsulta
	group by p.IdModulo
	FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, ''))
  
   as 'CONCEPTO',
   'MENSUALIDADES MOTOS' as 'ESTESEBORRA'
from T_Pagos as p
where day(p.fechapago)=day(@FechaCosulta) and month(p.fechapago)=MONTH(@FechaCosulta) and YEAR(p.FechaPago) = year(@FechaCosulta) and p.IdTipoPago = 2 and p.IdEstacionamiento = @IdEstacionamientoConsulta and p.idautorizado=2
group by day(p.fechapago), month(p.fechapago), year(p.fechapago)

union --------------------TARJETAS---------------------------------

select 33 as 'CENTRO DE COSTO',
 'FV33' as  'DOCUMENTO',
 CONVERT(varchar(10),convert(int, DAY(p.FechaPago))) + REPLACE(str(month(p.fechapago),2),SPACE(1), '0') + '01' as 'NUMERO',
 REPLACE(str(day(p.fechapago),2),SPACE(1), '0') + '/' + REPLACE(str(month(p.fechapago),2),SPACE(1), '0') + '/' + CONVERT(varchar(10),year(p.FechaPago)) as 'FECHA',
 '417097' as 'CUENTA CONTABLE',
 'CREDITO' as 'NATURALEZA',
 '888.888.888' as 'IDENTIFICACION',
 sum(p.Subtotal) as 'TOTAL',
  
  (select STUFF((
	select p.IdModulo + ' ' + min(numerofactura)+'-'+max(numerofactura) + '; '
	from T_Pagos as p
	where day(p.fechapago)=day(@FechaCosulta) and month(p.fechapago)=MONTH(@FechaCosulta) and YEAR(p.FechaPago) = year(@FechaCosulta) and p.IdEstacionamiento = @IdEstacionamientoConsulta
	group by p.IdModulo
	FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, ''))
  
   as 'CONCEPTO',
   'TARJETAS' as 'ESTESEBORRA'
from T_Pagos as p
inner join
T_Transacciones as t
on p.idtransaccion=t.idtransaccion
inner join T_TipoVehiculo as tv
on t.IdTipoVehiculo = tv.IdTipoVehiculo
inner join T_TipoPago as tp
on p.IdTipoPago = tp.IdTipoPago
inner join T_Estacionamientos as e
on t.idestacionamiento=e.idestacionamiento
where day(p.fechapago)=day(@FechaCosulta) and month(p.fechapago)=MONTH(@FechaCosulta) and YEAR(p.FechaPago) = year(@FechaCosulta) and tp.IdTipoPago = 3 and e.IdEstacionamiento = @IdEstacionamientoConsulta
group by day(p.fechapago), month(p.fechapago), year(p.fechapago), e.Nombre

