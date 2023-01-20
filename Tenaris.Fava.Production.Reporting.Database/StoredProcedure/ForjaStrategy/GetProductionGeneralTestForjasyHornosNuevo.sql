USE [L2_Forja]
GO
/****** Object:  StoredProcedure [dbo].[GetProductionGeneralTestForjasyHornosNuevo]    Script Date: 20/01/2023 10:21:38 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
If OBJECT_ID('[dbo].[GetProductionGeneralTestForjasyHornosNuevo] ') IS NOT NULL
	DROP PROCEDURE [dbo].[GetProductionGeneralTestForjasyHornosNuevo]
GO



ALTER PROCEDURE [dbo].[GetProductionGeneralTestForjasyHornosNuevo] 

	@Orden VARCHAR(20)='0',
	@Colada VARCHAR(20)='0',
	@Atado VARCHAR(20)='0',
	@Machine VARCHAR(150)='0'

AS
BEGIN
	

	DECLARE @TEMP2 
		AS TABLE(OrderNumber int
				,HeatNumber int
				,Description nvarchar(1000)
				,GroupitemNumber int
				,Extremo varchar (10) null
				,idHistory int)
	INSERT INTO @Temp2
	SELECT  
		OH.OrderNumber
		, HH.HeatNumber
		, MN.Description
		, GIH.GroupItemNumber
		, GIH.Extremo 
		, MIN(HS.idHistory) AS idHistory				
	FROM Production.OrderHistory AS OH
		INNER JOIN Production.OrderKey AS OK
			ON OH.idOrderHistory = OK.idOrderHistory 
		INNER JOIN Production.Batch 
			ON OK.idOrderKey = Production.Batch.idOrderKey 
		INNER JOIN Production.History AS HS
		INNER JOIN Production.KeyHistory AS KH
			ON HS.idKeyHistory = KH.idKeyHistory 
		INNER JOIN Common.Machine AS MN
			ON KH.idMachine = MN.idMachine 
			ON Production.Batch.idBatch = KH.idProductionHistory  
		INNER JOIN Production.HeatHistory AS HH
			ON Production.Batch.idHeatHistory = HH.idHeatHistory 
		INNER JOIN Production.GroupItemHistory AS GIH
			ON Production.Batch.idGroupItemHistory = GIH.idGroupItemHistory
		LEFT JOIN Production.ReportProductionHistory AS RPH 
			ON HS.idHistory=RPH.idHistory
		INNER JOIN Manufacturing.GroupItemProgram AS GIP
			ON Production.Batch.idBatch = GIP.idBatch
	Where 
		(HS.LoadedCount > 0) AND
		(@Orden = 0 OR OH.OrderNumber = @Orden) AND
		(@Colada = 0 OR HH.HeatNumber = @Colada) AND
		(@Atado = 0 OR GIH.GroupItemNumber = @Atado) 
		--AND MN.Description like '%Forjadora 0%'

	GROUP BY OH.OrderNumber
		, HH.HeatNumber
		, MN.Description
		, GIH.GroupItemNumber
		, GIH.Extremo







	DECLARE @TEMP 
	AS TABLE (
		idHistory INT,
		LoadedCount INT,
		GoodCount INT,
		WarnedCount INT, 
		ScrapCount INT, 
		StandardCount INT, 
		TransferenceCount INT, 
		PendingCount INT, 
		ReworkedCount INT, 
		Description NVARCHAR(1000), 
		UpdDateTime Datetimeoffset(7), 
		InsDateTime Datetimeoffset(7), 
		OrderNumber INT, 
		Product NVARCHAR(12), 
		Customer NVARCHAR(100), 
		HeatNumber INT, 
		GroupItemNumber INT, 
		LotNumberHTR NVARCHAR(100), 
		Location NVARCHAR(50), 
		idOrderHistory INT, 
		idOrderKey INT, 
		idBatch INT,
		Extremo VARCHAR(10),
		ReportBox VARCHAR(10),
		GroupItemType nVARCHAR(50),
		Sended int,
		ReportSequence int null
	)
	
	INSERT INTO @TEMP
	SELECT 
	HS.idHistory
	--HS.LoadedCount 
	--GIP.ProgrammedPieces,
	,Case WHEN T2.idHistory IS NOT NULL THEN GIP.ProgrammedPieces
			ELSE Hs.LoadedCount END AS LoadedCount
	,HS.GoodCount, 
	HS.WarnedCount, 
	HS.ScrapCount, 
	HS.StandardCount, 
	HS.TransferenceCount, 
	HS.PendingCount, 
	HS.ReworkedCount, 
	MN.Description, 
	HS.UpdDateTime, 
	HS.InsDateTime, 
	Production.OrderHistory.OrderNumber, 
	Production.OrderHistory.Product, 
	Production.OrderHistory.Customer, 
	HH.HeatNumber, 
	GIH.GroupItemNumber, 
	GIH.LotNumberHTR, 
	GIH.Location, 
	Production.OrderHistory.idOrderHistory, 
	BT.idOrderKey, 
	BT.idBatch,
	case  
		when MN.Description ='Horno de Normalizado' then ''
		when MN.Description ='Horno de Revenido' then '' 
		when MN.Description ='Granalladora' then '' 
		when (MN.Description ='Forjadora' and GIH.Extremo = '') then 'Extremo 2'
		else GIH.Extremo end as Extremo 
	,NULL as ReportBox
	,NULL as GroupItemType
	, (CASE WHEN RPH.idHistory IS Null Then 0 ELSE 1 END ) AS Sended
	,null
	

FROM Production.OrderHistory 
	INNER JOIN Production.OrderKey AS OK 
		ON Production.OrderHistory.idOrderHistory = OK.idOrderHistory
	INNER JOIN Production.Batch AS BT 
		ON OK.idOrderKey = BT.idOrderKey
	INNER JOIN Production.History AS HS
	INNER JOIN Production.KeyHistory AS KH
		ON HS.idKeyHistory = KH.idKeyHistory 
    INNER JOIN Common.Machine AS MN
		ON KH.idMachine = MN.idMachine 
	    ON BT.idBatch = KH.idProductionHistory
	INNER JOIN Production.HeatHistory AS HH
		ON BT.idHeatHistory = HH.idHeatHistory 
	INNER JOIN Production.GroupItemHistory AS GIH
		ON BT.idGroupItemHistory = GIH.idGroupItemHistory
	LEFT JOIN Production.ReportProductionHistory AS RPH 
			ON HS.idHistory=RPH.idHistory
	INNER JOIN Manufacturing.GroupItemProgram AS GIP
			ON BT.idBatch = GIP.idBatch
	Left JOIN @Temp2 as T2 ON Production.OrderHistory.OrderNumber=T2.OrderNumber AND
							   HH.HeatNumber = T2.HeatNumber AND
							   GIH.GroupItemNumber=T2.GroupitemNumber AND
							   Mn.Description=T2.Description AND
							   isnull(GIH.Extremo,'') = isnull(T2.Extremo,'') AND
							   Hs.idHistory=T2.idHistory

WHERE (HS.LoadedCount > 0) 	
	  AND (@Orden = 0 OR Production.OrderHistory.OrderNumber = @Orden) 
	  AND (@Colada = 0 OR HH.HeatNumber = @Colada) 
	  AND (@Atado = 0 OR GIH.GroupItemNumber = @Atado) 
	 -- AND  (@Machine = '0' OR MN.Description = @Machine)
	  
ORDER BY MN.Description,KH.InsDateTime asc



DECLARE @Forja0Buenos INT 
DECLARE @Forja0Malos1 INT 
DECLARE @Forja0Malos2 INT 
DECLARE @FOrja0ReprocesosBuenos INT

SELECT @Forja0Buenos  = ISNULL(SUM(GoodCount),0) FROM @TEMP WHERE [Description]='Bancal de buenas de Forjadora 0'
SELECT @Forja0Malos1 = ISNULL(SUM(GoodCount),0) FROM @TEMP WHERE [Description]='Cuneta de descartes 1 de Forjadora 0'
SELECT @Forja0Malos2 = ISNULL(SUM(GoodCount),0) FROM @TEMP WHERE [Description]='Cuneta de descartes 2 de Forjadora 0'
SELECT @Forja0ReprocesosBuenos = ISNULL(SUM(ReworkedCount),0) FROM @TEMP WHERE [Description]='Bancal de buenas de Forjadora 0'

UPDATE @TEMP
SET GoodCount = @Forja0Buenos - @Forja0ReprocesosBuenos, ScrapCount = @Forja0Malos1 + @Forja0Malos2, ReworkedCount = @Forja0ReprocesosBuenos
WHERE [Description] = 'Forjadora 0'




SELECT *
FROM @TEMP
Where Description like @Machine



END
