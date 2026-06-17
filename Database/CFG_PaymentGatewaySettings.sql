USE [DEV_PulseLogicHMR_INT]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CFG_PaymentGatewaySettings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CFG_PaymentGatewaySettings](
	[GatewayId] [int] IDENTITY(1,1) NOT NULL,
	[GatewayName] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[PublicKey] [nvarchar](500) NULL,
	[SecretKey] [nvarchar](500) NULL,
	[MerchantId] [nvarchar](500) NULL,
	[WebhookSecret] [nvarchar](500) NULL,
	[Environment] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_CFG_PaymentGatewaySettings] PRIMARY KEY CLUSTERED 
(
	[GatewayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
