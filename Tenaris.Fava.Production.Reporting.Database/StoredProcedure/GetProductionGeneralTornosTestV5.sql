﻿/****** Object:  StoredProcedure [dbo].[GetProductionGeneralTornosTestV5]    Script Date: 06/01/2023 11:18:11 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
If OBJECT_ID('[dbo].[GetProductionGeneralTornosTestV5]') IS NOT NULL
	DROP PROCEDURE [dbo].[GetProductionGeneralTornosTestV5]
GO

CREATE Procedure [dbo].[GetProductionGeneralTornosTestV5]

	@Orden VARCHAR(20) = '0' ,
	@Colada VARCHAR(20)= '0',
	@Atado VARCHAR(20) = '0',
    @Machine nvarchar(255)='0'

 AS
 BEGIN
	
			IF OBJECT_ID(N'dbo.TT_ProgrammedPieces_t1', N'U') IS NOT NULL 
				DROP TABLE dbo.TT_ProgrammedPieces_t1
			--drop table dbo.TT_ProgrammedPieces_t1
	
		select tt.*
		into dbo.TT_ProgrammedPieces_t1
		from
		(
		select m.* , 'MVI Mecanizado' as Comodin,
		 isnull(ma.Extremo,'') as Extremo
		from Common.Machine m
		left join (SELECT  'Roscadora' as Description,
		'Extremo 1' as Extremo) ma
		on ma.Description=m.Description
		where Active=1
		union
		select m.* , 'MVI Mecanizado' as Comodin,
		 isnull(ma.Extremo,'') as Extremo
		from Common.Machine m
		left join (SELECT  'Roscadora' as Description,
		'Extremo 2' as Extremo) ma
		on ma.Description=m.Description
		where Active=1) as tt
	
			UPDATE dbo.TT_ProgrammedPieces_t1
			SET Comodin = 'EMBAL1'
				, Description = 'Pintado'
			WHERE Code = 'EMBAL1'






		DECLARe @TablaTemporal AS TABLE(idHistory int
												, LoadedCount int
												,GoodCount int 
												,WarnedCount int 
												,Scrapcount int 
												,StandardCount int 
												,TransferenceCount int 
												,PendingCount int 
												,ReworkedCount int 
												,[Description] nvarchar(200)
												,UpdDateTime nvarchar(200)
												,InsDateTime nvarchar(200)
												,OrderNumber int
												,Product nvarchar(150)
												,Customer nvarchar(200)
												,HeatNumber int
												,GroupItemNumber int
												,LotNumberHTR nvarchar(200)
												,Location nvarchar (200)
												,idOrderHistory int
												,idOrderKey int
												,idBatch int
												,Extremo nvarchar(150)
												,ReportBox nvarchar(150)
												,GroupItemType nvarchar(150))


		Insert into @TablaTemporal
		SELECT  
							  ROW_NUMBER() over (order by  Common.Machine.Description,Production.History.InsDateTime asc) as idHistory
							  ,Production.History.LoadedCount, Production.History.GoodCount, Production.History.WarnedCount, Production.History.ScrapCount, 
							  Production.History.StandardCount, Production.History.TransferenceCount, Production.History.PendingCount, Production.History.ReworkedCount, 
							  t1.Description,--Common.Machine.Description, 
							  Production.History.UpdDateTime, Production.History.InsDateTime, Production.OrderHistory.OrderNumber, 
							  Production.OrderHistory.Product, Production.OrderHistory.Customer, Production.HeatHistory.HeatNumber, 
							  Production.GroupItemHistory.GroupItemNumber, Production.GroupItemHistory.LotNumberHTR, Production.GroupItemHistory.Location, 
							  Production.OrderHistory.idOrderHistory, Production.Batch.idOrderKey, Production.Batch.idBatch,
							  t1.Extremo,--Production.GroupItemHistory.Extremo 
							  Production.GroupItemHistory.ReportBox,
							  Production.GroupItemHistory.GroupItemType
		FROM         Production.OrderHistory INNER JOIN
							  Production.OrderKey ON Production.OrderHistory.idOrderHistory = Production.OrderKey.idOrderHistory AND 
							  Production.OrderHistory.idOrderHistory = Production.OrderKey.idOrderHistory INNER JOIN
							  Production.Batch ON Production.OrderKey.idOrderKey = Production.Batch.idOrderKey AND 
							  Production.OrderKey.idOrderKey = Production.Batch.idOrderKey AND Production.OrderKey.idOrderKey = Production.Batch.idOrderKey INNER JOIN
							  Production.History INNER JOIN
							  Production.KeyHistory ON Production.History.idKeyHistory = Production.KeyHistory.idKeyHistory INNER JOIN
							  Common.Machine ON Production.KeyHistory.idMachine = Common.Machine.idMachine ON 
							  Production.Batch.idBatch = Production.KeyHistory.idProductionHistory AND Production.Batch.idBatch = Production.KeyHistory.idProductionHistory AND 
							  Production.Batch.idBatch = Production.KeyHistory.idProductionHistory INNER JOIN
							  Production.HeatHistory ON Production.Batch.idHeatHistory = Production.HeatHistory.idHeatHistory AND 
							  Production.Batch.idHeatHistory = Production.HeatHistory.idHeatHistory AND 
							  Production.Batch.idHeatHistory = Production.HeatHistory.idHeatHistory INNER JOIN
							  Production.GroupItemHistory ON Production.Batch.idGroupItemHistory = Production.GroupItemHistory.idGroupItemHistory AND 
							  Production.Batch.idGroupItemHistory = Production.GroupItemHistory.idGroupItemHistory AND 
							  Production.Batch.idGroupItemHistory = Production.GroupItemHistory.idGroupItemHistory
						join  dbo.TT_ProgrammedPieces_t1 t1
							  on(t1.Comodin=Common.Machine.Description  OR t1.Comodin = 'EMBAL1')
		WHERE     
		--(Production.History.LoadedCount > 0) --' + @str + '	
		--and 
		(Common.Machine.Description = 'MVI Mecanizado' OR Common.Machine.Description = 'Embalado Bateria 1')
		AND (@Orden = 0 OR Production.OrderHistory.OrderNumber = @Orden) 
		AND (@Colada = 0 OR Production.HeatHistory.HeatNumber = @Colada) 
		AND (@Atado = 0 OR Production.GroupItemHistory.GroupItemNumber = @Atado)
		ORDER BY Common.Machine.Description,Production.History.InsDateTime asc



		Select			TT.idHistory 
						--,TT.LoadedCount 
						,(CASE WHEN ROW_NUMBER() OVER (PARTITION BY TT.GroupItemNumber ORDER BY  TT.idHistory)=1 
							THEN GIP.ProgrammedPieces 
							ELSE TT.LoadedCount END) AS LoadedCount
						,TT.GoodCount  
						,TT.WarnedCount  
						,TT.Scrapcount  
						,TT.StandardCount    
						,TT.TransferenceCount    
						,TT.PendingCount    
						,TT.ReworkedCount    
						,TT.[Description]   
						,TT.UpdDateTime   
						,TT.InsDateTime   
						,TT.OrderNumber   
						,TT.Product  
						,TT.Customer   
						,TT.HeatNumber   
						,TT.GroupItemNumber   
						,TT.LotNumberHTR   
						,TT.Location    
						,TT.idOrderHistory   
						,TT.idOrderKey   
						,TT.idBatch   
						,TT.Extremo   
						,TT.ReportBox   
						,TT.GroupItemType
					   ,(CASE WHEN RPH.idHistory IS Null Then 0 ELSE 1 END ) AS Sended
					   ,1 as ReportSequence
		From @TablaTemporal AS TT
		Left Join Production.ReportProductionHistory AS RPH 
			ON TT.idHistory=RPH.idHistory
			AND TT.OrderNumber=RPH.OrderNumber
			AND TT.HeatNumber=RPH.HeatNumber
			AND TT.GroupItemNumber=RPH.GroupItemNumber
			AND RIGHT(TT.Extremo,1) = RIGHT(RPH.MachineOperation,1)
		INNER JOIN Manufacturing.GroupItemProgram AS GIP 
			ON TT.idBatch=GIP.idBatch
		Where TT.Description like @Machine




			IF OBJECT_ID(N'dbo.TT_ProgrammedPieces_t1', N'U') IS NOT NULL 
				DROP TABLE dbo.TT_ProgrammedPieces_t1



END


