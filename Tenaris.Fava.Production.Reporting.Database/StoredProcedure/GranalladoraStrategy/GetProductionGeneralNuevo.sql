USE [L2_GEC]
GO
/****** Object:  StoredProcedure [dbo].[GetProductionGeneralTestV5Nuevo]    Script Date: 20/01/2023 10:09:34 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
If OBJECT_ID('[dbo].[GetProductionGeneralTestV5Nuevo]') IS NOT NULL
	DROP PROCEDURE [dbo].[GetProductionGeneralTestV5Nuevo]
GO






ALTER PROCEDURE [dbo].[GetProductionGeneralTestV5Nuevo]
		@Orden VARCHAR(20)='0',
	    @Colada VARCHAR(20)='0',
	    @Atado VARCHAR(20)='0',
		@Machine VARCHAR(150)='0'
AS
BEGIN
   
   SELECT  
		 HS.idHistory
		--,Hs.LoadedCount AS RealLoadedCount
		--,GIP.ProgrammedPieces AS ProgrammedPieces
		,(CASE WHEN ROW_NUMBER() OVER (PARTITION BY GIH.GroupItemNumber ORDER BY  HS.idHistory)=1 
				THEN GIP.ProgrammedPieces 
				ELSE Hs.LoadedCount END) AS LoadedCount
		, HS.GoodCount
		, HS.WarnedCount
		, HS.ScrapCount
		, HS.StandardCount
		, HS.TransferenceCount
		, HS.PendingCount
		, HS.ReworkedCount
		, MN.Description
		, HS.UpdDateTime
		, HS.InsDateTime
		, OH.OrderNumber
		, OH.Product
		, OH.Customer
		, HH.HeatNumber
		, GIH.GroupItemNumber
		, GIH.LotNumberHTR
		, GIH.Location
		, OH.idOrderHistory
		, Production.Batch.idOrderKey
		, Production.Batch.idBatch
		, GIH.Extremo 
		, 1 AS Sended--(CASE WHEN RPH.idHistory IS Null Then 0 ELSE 1 END ) AS Sended
		,ROW_NUMBER() OVER (PARTITION BY GIH.GroupItemNumber ORDER BY  HS.idHistory) AS ReportSequence 
		
		
		
		
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
		--Inner JOIN Production.ReportProductionHistory AS RPH 
			--ON HS.idHistory=RPH.idHistory 
		INNER JOIN Manufacturing.GroupItemProgram AS GIP
			ON Production.Batch.idBatch = GIP.idBatch
		
	Where 
		(HS.LoadedCount > 0) AND
		(@Orden = 0 OR OH.OrderNumber = @Orden) AND
		(@Colada = 0 OR HH.HeatNumber = @Colada) AND
		(@Atado = 0 OR GIH.GroupItemNumber = @Atado) AND
		(@Machine ='' OR MN.Description like @Machine)

	ORDER BY 
		 GIH.GroupItemNumber DESC,
		 Hs.InsDateTime ,
		 ReportSequence DESC
		 
		 
		



END