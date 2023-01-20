USE [L2_INYEC]
GO
/****** Object:  StoredProcedure [Production].[GetRejectionCodeByMachineDescriptionTestV5]    Script Date: 20/01/2023 10:47:34 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
If OBJECT_ID('[Production].[GetRejectionCodeByMachineDescriptionTestV5] ') IS NOT NULL
	DROP PROCEDURE [Production].[GetRejectionCodeByMachineDescriptionTestV5] 
GO
ALTER Procedure [Production].[GetRejectionCodeByMachineDescriptionTestV5]
@MachineDescription varchar(150)
AS
	BEGIN
		Select RC.idRejectionCode
		,MN.idMachine as Machine_idMachine
		,MN.Code as Machine_Code
		,MN.Name as Machine_Name
		,MN.Description as Machine_Description
		,MN.Active as Machine_Active
		,RC.Code
		,RC.Name
		,RC.Description
		,Convert(int,RC.Active) AS Active

		From Common.Machine AS MN
		INNER JOIN Production.RejectionCode AS RC ON MN.idMachine=RC.idMachine
		Where MN.Description=@MachineDescription AND RC.Active=1
		Order by RC.Description
	END