﻿USE [L2_INYEC]
GO
/****** Object:  StoredProcedure [Production].[InsReportProductionHistoryTestV5]    Script Date: 20/01/2023 10:48:59 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
If OBJECT_ID('[Production].[InsReportProductionHistoryTestV5]') IS NOT NULL
	DROP PROCEDURE [Production].[InsReportProductionHistoryTestV5] 
GO
ALTER Procedure [Production].[InsReportProductionHistoryTestV5]

	 @idHistory int
	,@TipoUdt nvarchar(250)
	,@OrderNumber int
	,@HeatNumber int
	,@GroupItemNumber int
	,@SendStatus int
	,@TotalQuantity int
	,@GoodCount int
	,@ScrapCount int
	,@ReworkedCount int
	,@IdMachine int
	,@LotNumberHtr int
	,@InsDateTime datetime
	,@InsertedBy nvarchar(255)
	,@MachineSequence int
	,@MachineOption nvarchar(255)
	,@MachineOperation nvarchar(255)
	,@Observation nvarchar(255) = null
AS
BEGIN
Declare @idReportProductionHistoryInserted int
	Insert into 
				Production.ReportProductionHistory
				(idHistory
				,OrderNumber
				,HeatNumber
				,GroupItemNumber
				,SendStatus
				,TotalQuantity
				,GoodCount
				,ScrapCount
				,ReworkedCount
				,IdMachine
				,LotNumberHtr
				,InsDateTime
				,InsertedBy
				,MachineSequence
				,MachineOption
				,MachineOperation
				,Observation
				,GroupItemType
				,ChildOrderNumber
				,ChildGroupItemNumber
				,ChildGroupItemType)

	Values(@idHistory 
		,@OrderNumber 
		,@HeatNumber 
		,@GroupItemNumber 
		,@SendStatus 
		,@TotalQuantity 
		,@GoodCount 
		,@ScrapCount 
		,@ReworkedCount 
		,@IdMachine 
		,@LotNumberHtr 
		,@InsDateTime 
		,@InsertedBy 
		,@MachineSequence 
		,@MachineOption 
		,@MachineOperation 
		,@Observation
		,@TipoUdt
		,@OrderNumber
		,@GroupItemNumber
		,@TipoUdt)

		Set @idReportProductionHistoryInserted = SCOPE_IDENTITY()

		Select @idReportProductionHistoryInserted
	
		

END