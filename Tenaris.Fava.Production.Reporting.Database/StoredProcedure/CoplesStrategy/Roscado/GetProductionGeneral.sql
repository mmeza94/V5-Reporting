USE [L2_ROSCADO]
GO
/****** Object:  StoredProcedure [dbo].[GetProductionGeneralTestV5Nuevo]    Script Date: 20/01/2023 10:52:44 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
If OBJECT_ID('[dbo].[GetProductionGeneralTestV5Nuevo] ') IS NOT NULL
	DROP PROCEDURE [dbo].[GetProductionGeneralTestV5Nuevo]  
GO


ALTER PROCEDURE [dbo].[GetProductionGeneralTestV5Nuevo] 
@Orden VARCHAR(20)='0',
@Colada VARCHAR(20)='0',
@Atado VARCHAR(20)='0',
@Machine VARCHAR(100)='0'
AS
	BEGIN
		SELECT  DISTINCT PH.idHistory
			, PB.LoadedCount
			, PH.LoadedCount GoodCount
			, PH.WarnedCount
			, PH.ScrapCount
			, PH.StandardCount
			, PH.TransferenceCount
			, PH.PendingCount
			, PH.ReworkedCount
			, CM.[Description]
			, PH.UpdDateTime
			, PH.InsDateTime
			, POH.OrderNumber
			, POH.Product
			, POH.Customer
			, PHH.HeatNumber
			, PGIH.GroupItemNumber
			, (CASE WHEN PGIH.LotNumberHTR IS NULL THEN 0 ELSE PGIH.LotNumberHTR END) AS LotNumberHTR -- Se usa  este caso para evitar dbnull en los resultados
			, PGIH.Location
			, POH.idOrderHistory
			, PB.idOrderKey
			, PB.idBatch
			, PGIH.Extremo
			, PGIH.ReportBox
			, PGIH.GroupItemType
			, PKH.ShiftNumber
			, CONVERT(NVARCHAR(10)
			, PKH.ShiftDate) ShiftDate  --Nuevos
			,(CASE WHEN RPH.idHistory IS Null Then 0 ELSE 1 END ) AS Sended
			,ROW_NUMBER() OVER (PARTITION BY PGIH.GroupItemNumber ORDER BY  PH.idHistory) AS ReportSequence
			--,DENSE_RANK() over (PARTITION BY PGIH.GroupItemNumber ORDER BY  PH.idHistory) AS ReportSequence
		
			
		FROM Production.OrderHistory POH
			INNER JOIN Production.OrderKey POK		ON POK.idOrderHistory	= POH.idOrderHistory
			INNER JOIN Production.Batch PB			ON PB.idOrderKey		= POK.idOrderKey
			INNER JOIN Production.KeyHistory PKH	ON PKH.idProductionHistory = PB.idBatch
			INNER JOIN Production.History PH		ON PH.idKeyHistory		= PKH.idKeyHistory
			INNER JOIN Common.Machine CM			ON CM.idMachine			= PKH.idMachine
			INNER JOIN Production.HeatHistory PHH	ON PHH.idHeatHistory	= PB.idHeatHistory
			INNER JOIN Production.GroupItemHistory PGIH ON PB.idGroupItemHistory = PGIH.idGroupItemHistory
			LEFT JOIN Production.ReportProductionHistory AS RPH 
			ON PH.idHistory = RPH.idHistory
			INNER JOIN Manufacturing.GroupItemProgram AS GIP
			ON PB.idBatch = GIP.idBatch

		Where 
		(PH.LoadedCount > 0) AND
		(@Orden = 0 OR POH.OrderNumber = @Orden) AND
		(@Colada = 0 OR PHH.HeatNumber = @Colada) AND
		(@Atado = 0 OR PGIH.GroupItemNumber = @Atado) AND
		(@Machine ='' OR CM.Description like @Machine) 
		
		ORDER BY CM.[Description], PH.InsDateTime ASC
		
	END
