USE [L2_ROSCADO]
GO
/****** Object:  StoredProcedure [Production].[InsRejectionReportDetailTestV5]    Script Date: 20/01/2023 10:55:46 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
If OBJECT_ID('[Production].[InsRejectionReportDetailTestV5]') IS NOT NULL
	DROP PROCEDURE [Production].[InsRejectionReportDetailTestV5] 
GO
ALTER Procedure [Production].[InsRejectionReportDetailTestV5]

       @ScrapCount smallint
      ,@Observation nvarchar(255)
      ,@InsDateTime datetime
      ,@Active smallint
      ,@UpdDateTime datetime = null
      ,@idReportProductionHistory int
      ,@idRejectionCode int
      ,@Destino nvarchar(50)
      ,@Trabajado smallint
      ,@Extremo nvarchar(10)=null
AS
	BEGIN
		Insert into[Production].[RejectionReportDetail]
			   (  
			  ScrapCount 
			  ,Observation 
			  ,InsDateTime
			  ,Active
			  ,UpdDateTime
			  ,idReportProductionHistory
			  ,idRejectionCode
			  ,Destino
			  ,Trabajado
			  ,Extremo)


		Values(@ScrapCount
			  ,@Observation
			  ,@InsDateTime
			  ,@Active
			  ,@UpdDateTime
			  ,@idReportProductionHistory
			  ,@idRejectionCode
			  ,@Destino
			  ,@Trabajado
			  ,@Extremo)


	END
