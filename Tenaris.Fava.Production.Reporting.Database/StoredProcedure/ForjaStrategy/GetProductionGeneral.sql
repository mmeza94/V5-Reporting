
USE [L2_Forja]
GO
/****** Object:  StoredProcedure [dbo].[GetProductionGeneralProgrammedTestV5]    Script Date: 20/01/2023 10:18:41 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
If OBJECT_ID('[dbo].[GetProductionGeneralProgrammedTestV5] ') IS NOT NULL
	DROP PROCEDURE [dbo].[GetProductionGeneralProgrammedTestV5] 
GO









ALTER PROCEDURE [dbo].[GetProductionGeneralProgrammedTestV5] 

@Orden int='0',
@Colada int='0',
@Atado int='0',
@Machine nvarchar(255)
AS 
BEGIN

	
			DECLARE @Temp AS TABLE(idMachine int
								,idArea int
								,Code nvarchar(200)
								,Name nvarchar(200)
								,Description nvarchar(200)
								,SortOrder int
								,Active bit
								,InsDateTime Datetimeoffset
								,UpdDAteTime DateTimeOffset
								,Comodin varchar(150)
								,Extremo varchar(150))



			Insert into @Temp
			Select M.*,'Granalladora' as Comodin,isnull(ma.Extremo,'') as Extremo
			From Common.Machine AS M
			left join (SELECT  'Forjadora' as Description,
								'Extremo 1' as Extremo) AS MA
			ON MA.Description=M.Description
			where Active=1

			UNION 

			Select M.*,'Granalladora' as Comodin,isnull(ma.Extremo,'') as Extremo
			From Common.Machine AS M
			left join (SELECT  'Forjadora' as Description,
								'Extremo 2' as Extremo) AS MA
			ON MA.Description=M.Description
			where Active=1;



WITH ConsultaTemporal AS(
			SELECT  
						ROW_NUMBER() over (order by  Common.Machine.Description,Production.History.InsDateTime asc) as idHistory
						,Production.History.LoadedCount
						, Production.History.GoodCount
						, Production.History.WarnedCount
						, Production.History.ScrapCount
						, Production.History.StandardCount
						, Production.History.TransferenceCount
						, Production.History.PendingCount
						, Production.History.ReworkedCount
						, t1.Description
						, Production.History.UpdDateTime
						, Production.History.InsDateTime
						, Production.OrderHistory.OrderNumber
						, Production.OrderHistory.Product
						, Production.OrderHistory.Customer
						, Production.HeatHistory.HeatNumber
						, Production.GroupItemHistory.GroupItemNumber
						, Production.GroupItemHistory.LotNumberHTR
						, Production.GroupItemHistory.Location
						, Production.OrderHistory.idOrderHistory
						, Production.Batch.idOrderKey
						, Production.Batch.idBatch
						, t1.Extremo
					FROM   Production.OrderHistory 
						INNER JOIN Production.OrderKey 
										ON Production.OrderHistory.idOrderHistory = Production.OrderKey.idOrderHistory
						INNER JOIN Production.Batch		
										ON Production.OrderKey.idOrderKey = Production.Batch.idOrderKey 			   
						INNER JOIN Production.History 
						INNER JOIN Production.KeyHistory 
										ON Production.History.idKeyHistory = Production.KeyHistory.idKeyHistory 
						INNER JOIN Common.Machine 
										ON Production.KeyHistory.idMachine = Common.Machine.idMachine 
										ON Production.Batch.idBatch = Production.KeyHistory.idProductionHistory
						INNER JOIN Production.HeatHistory 
										ON Production.Batch.idHeatHistory = Production.HeatHistory.idHeatHistory 
							
						INNER JOIN Production.GroupItemHistory 
										ON Production.Batch.idGroupItemHistory = Production.GroupItemHistory.idGroupItemHistory 
						INNER JOIN  @Temp t1
										ON (t1.Comodin=Common.Machine.Description)
	

			WHERE     
					Common.Machine.Description = 'Granalladora' 
					AND (@Orden = 0 OR Production.OrderHistory.OrderNumber = @Orden) 
					AND (@Colada = 0 OR Production.HeatHistory.HeatNumber = @Colada) 
					AND (@Atado = 0 OR Production.GroupItemHistory.GroupItemNumber = @Atado) 	

			)


		Select CT.*
			,(CASE WHEN RPH.idHistory IS Null Then 0 ELSE 1 END ) AS Sended
			,1 AS ReportSEquence
		From ConsultaTemporal AS CT
		LEFT JOIN Production.ReportProductionHistory AS RPH 
				ON CT.idHistory = RPH.idHistory 
				AND CT.OrderNumber = RPH.OrderNumber
				AND CT.HeatNumber = RPH.HeatNumber
				AND CT.GroupItemNumber=RPH.GroupItemNumber
		Where Description like 'Granalladora'
		ORDER BY CT.[Description],CT.InsDateTime asc





END







