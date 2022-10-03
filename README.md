# <center> Web API Kalum .NET Kinal 2022 </center>

# Script creación de base de datos SQL
## Código para creación de entidades

```sql
create database kalum_test
go
use kalum_test
go
create table ExamenAdmision
(
	ExamenId varchar(128) primary key not null,
	FechaExamen datetime not null
)
go
create table CarreraTecnica(
	CarreraId varchar(128) primary key not null,
	Nombre varchar(128) not null
)
go
create table Jornada(
	JornadaId varchar(128) primary key not null,
	Prefijo varchar(2) not null,
	Descripcion varchar(128) not null
)
go
create table Aspirante(
	NoExpediente varchar(128) primary key not null,
	Apellidos varchar(128) not null,
	Nombres varchar(128) not null,
	Direccion varchar(128) not null,
	Telefono varchar(64) not null,
	Email varchar(128) not null unique,
	Estatus varchar(32) default 'No Asignado',
	CarreraId varchar(128) not null,
	ExamenId varchar(128) not null,
	JornadaId varchar(128) not null,
	constraint FK_ASPIRANTE_CARRERA_TECNICA foreign key (CarreraId) references CarreraTecnica(CarreraId),
	constraint FK_ASPIRANTE_EXAMEN_ADMISION foreign key (ExamenId) references ExamenAdmision(ExamenId),
	constraint FK_ASPIRANTE_JORNADA foreign key (JornadaId) references Jornada(JornadaId)
)
go
create table ResultadoExamenAdmision(
	NoExpediente varchar(128) not null,
	Anio varchar(4) not null,
	Descripcion varchar(128) not null,
	nota int default 0,
	primary key (NoExpediente, Anio),
	constraint FK_RESULTADO_EXAMEN_ADMISION_ASPIRANTE foreign key (NoExpediente) references Aspirante(NoExpediente)
)
go
create table InscripcionPago(
	BoletaPago varchar(128) not null,
	NoExpediente varchar(128) not null,
	Anio varchar(4) not null,
	FechaPago datetime not null,
	Monto decimal(10,2) not null,
	primary key (BoletaPago, NoExpediente, Anio),
	constraint FK_INSCRIPCION_PAGO_ASPIRANTE foreign key (NoExpediente) references Aspirante(NoExpediente)
)
go
create table InversionCarreraTecnica(
	InversionId varchar(128) not null,
	CarreraId varchar(128) not null,
	MontoInscripcion decimal(10,2) not null,
	NumeroPagos int not null,
	MontoPago decimal(10,2) not null,
	primary key (InversionId),
	constraint FK_INVERSION_CARRERA_TECNICA foreign key(CarreraId) references CarreraTecnica(CarreraId)
)
go
create table Alumno(
	Carne varchar(8) not null,
	Apellidos varchar(128) not null,
	Nombres varchar(128) not null,
	Direccion varchar(128) not null,
	Telefono varchar(64) not null,
	Email varchar(64) not null,
	primary key (Carne)
)
go
create table Cargo(
	CargoId varchar(128) not null,
	Descripcion varchar(128) not null,
	Prefijo varchar(64) not null,
	Monto decimal(10,2) not null,
	GeneraMora bit not null,
	PorcentajeMora int not null,
	primary key (CargoId)
)
go
create table Inscripcion(
	InscripcionId varchar(128) not null,
	Carne varchar(8) not null,
	CarreraId varchar(128) not null,
	JornadaId varchar(128) not null,
	Ciclo varchar(4) not null,
	FechaInscripcion datetime not null,
	primary key(InscripcionId),
	constraint FK_INSCRIPCION_ALUMNO foreign key (Carne) references Alumno(Carne),
	constraint FK_INSCRIPCION_CARRERA_TECNICA foreign key (CarreraId) references CarreraTecnica(CarreraId),
	constraint FK_INSCRIPCION_JORNADA foreign key (JornadaId) references Jornada(JornadaId)
)
go
create table CuentaXCobrar(
	Cargo varchar(128) not null,
	Anio varchar(4) not null,
	Carne varchar(8) not null,
	CargoId varchar(128) not null,
	Descripcion varchar(128) not null,
	FechaCargo datetime not null,
	FechaAplica datetime not null,
	Monto decimal(10,2) not null,
	Mora decimal(10,2) not null,
	Descuento decimal(10,2) not null,
	primary key (Cargo),
	constraint FK_CUENTA_X_COBRAR_ALUMNO foreign key (Carne) references Alumno(Carne),
	constraint FK_CUENTA_X_COBRAR_CARGO foreign key (CargoId) references Cargo(CargoId)
)
```

## Creación trigger

Disparador para la evaluación de un aspirante si pasó el examen de admision y seguir el proceso de inscripción.

```sql
create trigger tg_ActualizarEstadoAspirante on ResultadoExamenAdmision after insert
AS
BEGIN
	declare @Nota int = 0
	declare @Expediente varchar(128)
	declare @Estatus varchar(64) = 'NO ASIGNADO'
	set @Nota = (select nota from inserted)
	set @Expediente = (select NoExpediente from inserted)
	if @Nota >= 70
	begin
		set @Estatus = 'SIGUE PROCESO DE ADMISION'
	end
	ELSE
	begin
		set @Estatus = 'NO SIGUE PROCESO DE ADMISION'
	end
	update Aspirante set Estatus = @Estatus where NoExpediente = @Expediente
END
```

## Creación de funciones

```sql
create function RPAD(
	@string varchar(MAX),
	@length int,
	@pad char
)
returns varchar(MAX)
as
begin
	return @string + replicate(@pad, @length - len(@string))
end
go
create function LPAD(
	@string varchar(MAX),
	@length int,
	@pad char
)
returns varchar(MAX)
as
begin
	return replicate(@pad, @length - len(@string)) + @string
end
```

## Insert en entidades

#### Carreras Técnicas

```sql
insert into CarreraTecnica(CarreraId, Nombre)
	values(NEWID(), 'Desarrollo de aplicaciones empresariales con .NET Core');
insert into CarreraTecnica(CarreraId, Nombre)
	values(NEWID(), 'Desarrollo de aplicaciones empresariales con Java EE');
insert into CarreraTecnica(CarreraId, Nombre)
	values(NEWID(), 'Desarrollo de aplicaciones Moviles');
```

#### Examenes de admisión

```sql
insert into ExamenAdmision(ExamenId, FechaExamen)
	values(NEWID(), '2022-04-30');
insert into ExamenAdmision(ExamenId, FechaExamen)
	values(NEWID(), '2022-05-30');
insert into ExamenAdmision(ExamenId, FechaExamen)
	values(NEWID(), '2022-06-20');
```

#### Jornadas

```sql
insert into Jornada(JornadaId, Prefijo, Descripcion)
	values(NEWID(), 'JM', 'Jornada Matutina');
insert into Jornada(JornadaId, Prefijo, Descripcion)
	values(NEWID(), 'JV', 'Jornada Vespertina');
```

#### Aspirantes

Agregar los códigos correspondientes en lineas con comentario.

```sql
insert into Aspirante(NoExpediente, Apellidos, Nombres, Direccion, Telefono, Email, CarreraId, ExamenId, JornadaId)
	values('EXP-20220001',
		'Lopez Martin',
		'Juan Alberto',
		'Guatemala, Guatemala',
		'23438019',
		'jlopez@kalum.edu.gt',
		'', -- .NET
		'', -- 30/05
		''); -- Matutina
insert into Aspirante(NoExpediente, Apellidos, Nombres, Direccion, Telefono, Email, CarreraId, ExamenId, JornadaId)
	values('EXP-20220002',
		'Mancilla Paz',
		'Nancy Elizabeth',
		'Guatemala, Guatemala',
		'23438024',
		'nmancilla@kalum.edu.gt',
		'', -- .NET
		'', -- 30/04
		''); -- Matutina
insert into Aspirante(NoExpediente, Apellidos, Nombres, Direccion, Telefono, Email, CarreraId, ExamenId, JornadaId)
	values('EXP-20220003',
		'Revolorio Paz',
		'Raul Antonio',
		'Guatemala, Guatemala',
		'234338847',
		'rrevolorio@kalum.edu.gt',
		'', -- JAVA
		'', -- 30/04
		''); -- Vespertina
```

#### Inversión Carrera Técnica

Agregar en espacio vacío ID correspondiente acorde al comentario.

```sql
insert into InversionCarreraTecnica VALUES (NEWID(), '', 1200, 5,750) -- Carrera JAVA
insert into InversionCarreraTecnica VALUES (NEWID(), '', 850, 5,850) -- Carrera .NET
```

#### Cargos

```sql
insert into Cargo VALUES (NEWID(), 'Pago de inscripcion de carrera tecnica Plan Fin de Semana', 'INSCT', 1200, 0,0)
insert into Cargo VALUES (NEWID(), 'Pago mensual carrera tecnica', 'PGMCT', 850, 0, 0)
insert into Cargo VALUES (NEWID(), 'Carne', 'CARNE', 30, 0, 0)
```

## Creación de una vista

```sql
create view vw_ListarAspirantesPorFechaExamen
as
select a.NoExpediente, a.Apellidos, a.Nombres, a.Estatus, ea.FechaExamen
	from Aspirante a inner join ExamenAdmision ea on a.ExamenId = ea.ExamenId;
```

> select \* from vw_ListarAspirantesPorFechaExamen where FechaExamen = '2022-04-30' order by Apellidos;

## Creación procedimiento almacenado

Cambiar los codigos con los creados aleatoriamente anteriormente en líneas con comentario

```sql
create procedure sp_EnrollmentProcess @NoExpediente varchar(12), @Ciclo varchar(4), @MesInicioPago int, @CarreraId varchar(128)
as
begin
	-- Variables para informacion de aspirantes
	declare @Apellidos varchar(128)
	declare @Nombres varchar(128)
	declare @Direccion varchar(128)
	declare @Telefono varchar(64)
	declare @Email varchar(64)
	declare @JornadaId varchar(128)
	-- Variables para generar numero de carne
	declare @Exists int
	declare @Carne varchar(12)
	-- Variables para proceso de pago
	declare @MontoInscripcion numeric(10,2)
	declare @NumeroPagos int
	declare @MontoPago numeric(10,2)
	declare @Descripcion varchar(128)
	declare @Prefijo varchar(6)
	declare @CargoId varchar(128)
	declare @Monto numeric(10,2)
	declare @CorrelativoPagos int
	-- Inicio transaccion
	begin transaction
	begin try
		-- Informacion del aspirante
		select @Apellidos = apellidos,
		@Nombres = Nombres,
		@Direccion = Direccion,
		@Telefono = Telefono,
		@Email = Email,
		@JornadaId = JornadaId
		from Aspirante where NoExpediente = @NoExpediente
		-- Generar Carne
		set @Exists = (select top 1 a.Carne from Alumno a where SUBSTRING(a.Carne, 1,4) = @Ciclo order by a.Carne desc)
		if @Exists is NULL
		begin
			set @Carne = (@Ciclo * 10000) + 1
		end
		else
		begin
			set @Carne = (select top 1 a.Carne from Alumno a where SUBSTRING(a.Carne, 1,4) = @Ciclo order by a.Carne desc) + 1
		end
		-- Registrar Alumno
		insert into Alumno VALUES (@Carne, @Apellidos, @Nombres, @Direccion, @Telefono, concat(@Carne,@Email))
		-- Registrar Inscripcion
		insert into Inscripcion VALUES (NEWID(), @Carne, @CarreraId, @JornadaId, @Ciclo,getDate())
		-- Actualizar estado de aspirante
		update Aspirante set Estatus = 'INSCRITO CICLO ' + @Ciclo where NoExpediente = @NoExpediente

	-- Proceso de cargos
		select @MontoInscripcion = MontoInscripcion, @NumeroPagos = NumeroPagos, @MontoPago = MontoPago
			from InversionCarreraTecnica where CarreraId = @CarreraId
		select @CargoId = CargoId, @Descripcion = Descripcion, @Prefijo = Prefijo
			from Cargo where CargoId = '' /* Codigo pago de inscripcion tabla Cargo */
		-- Insertar cargos
		insert into CuentaXCobrar VALUES(CONCAT(@Prefijo, SUBSTRING(@Ciclo, 3,2), dbo.LPAD('1', 2, '0')), @Ciclo, @Carne, @cargoId, @Descripcion, getdate(), getdate(), @MontoInscripcion, 0,0)
		-- Insertar Cargo Carne
		select @CargoId = CargoId, @Descripcion = Descripcion, @Prefijo = Prefijo, @Monto = Monto
			from Cargo where CargoId = '' /* Codigo cargo carne de tabla cargo */
		insert into CuentaXCobrar VALUES(CONCAT(@Prefijo, SUBSTRING(@Ciclo, 3,2), dbo.LPAD('1', 2, '0')), @Ciclo, @Carne, @cargoId, @Descripcion, getdate(), getdate(), @Monto, 0,0)

		-- Cargos Mensuales
		set @CorrelativoPagos = 1
		select @CargoId = CargoId, @Descripcion = Descripcion, @Prefijo = Prefijo from Cargo where CargoId = '' /* Codigo cargo pago mensual de carrera de tabla cargo */
		while(@CorrelativoPagos <= @NumeroPagos)
		begin
		insert into CuentaXCobrar VALUES(CONCAT(@Prefijo, SUBSTRING(@Ciclo, 3,2), dbo.LPAD(@CorrelativoPagos, 2, '0')), @Ciclo, @Carne, @cargoId, @Descripcion, getdate(), CONCAT(@ciclo,'-',dbo.LPAD(@MesInicioPago,2,'0'),'-','05'), @MontoPago, 0,0)
		set @CorrelativoPagos += 1
		set @MesInicioPago += 1
		end

		commit transaction
		select 'TRANSACTION SUCCESS' AS status, @Carne as carne
	end try
	begin catch
		/*SELECT
            ERROR_NUMBER() AS ErrorNumber
            ,ERROR_SEVERITY() AS ErrorSeverity
            ,ERROR_STATE() AS ErrorState
            ,ERROR_PROCEDURE() AS ErrorProcedure
            ,ERROR_LINE() AS ErrorLine
            ,ERROR_MESSAGE() AS ErrorMessage*/
		rollback transaction
		select 'TRANSACTION ERROR' AS status, 0 as carne
	end catch
end
```

> Utilización de procedimiento almacenado
> SQL execute sp_EnrollmentProcess 'EXP-20220001', '2022', 2, 'DE1E8D9B-3B96-4B83-B83A-2A3CD4AB72D5' -- ID carrera .NET

## Tabla Entidad Relación

![Tabla Entidad Relacion de base de datos](https://i.imgur.com/bPWvYS5.png)

## Test API Postman
[JSON Postman](./Test%20Api%20KalumManagement.postman_collection.json)

## Script SQL 
[Script SQL](./ScriptSQL.ipynb)
