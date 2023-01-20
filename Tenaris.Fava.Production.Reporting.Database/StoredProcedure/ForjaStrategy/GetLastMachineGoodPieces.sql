USE [L2_Forja]
GO
/****** Object:  StoredProcedure [Production].[GetLastMachineGoodPiecesTestV5]    Script Date: 20/01/2023 10:22:57 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
If OBJECT_ID('[Production].[GetLastMachineGoodPiecesTestV5]') IS NOT NULL
	DROP PROCEDURE [Production].[GetLastMachineGoodPiecesTestV5]
GO

ALTER PROCEDURE [Production].[GetLastMachineGoodPiecesTestV5]
@Sequence int,
@GroupItemNumber int
AS 
BEGIN
	Select SUM(GoodCount)
		From Production.ReportProductionHistory
		Where MachineSequence=@Sequence AND GroupItemNumber=@GroupItemNumber
END


