USE [L2_GEC]
GO
/****** Object:  StoredProcedure [Common].[LoginUserTestV5]    Script Date: 20/01/2023 10:07:11 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
If OBJECT_ID('[Common].[LoginUserTestV5]') IS NOT NULL
	DROP PROCEDURE [Common].[LoginUserTestV5]
GO

ALTER Procedure [Common].[LoginUserTestV5]
@UserName nvarchar(150),
@Password nvarchar(150)
AS 
BEGIN
	Select top 1 idPeople From Common.People
	Where UserName=@UserName And Password=@Password And Active =1
END