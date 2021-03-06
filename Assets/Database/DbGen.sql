USE [UndderControl]
GO
/****** Object:  Table [dbo].[CowStatus]    Script Date: 09/09/2019 12:57:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CowStatus](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Farm_ID] [int] NOT NULL,
	[InfectedAtDryOff] [bit] NOT NULL,
	[InfectedAtCalving] [bit] NOT NULL,
	[CowIdentifier] [nvarchar](max) NULL,
	[DateAddedDryOff] [datetime] NULL,
	[DateAddedCalving] [datetime] NULL,
 CONSTRAINT [PK_dbo.CowStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Farm]    Script Date: 09/09/2019 12:57:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Farm](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[ContactName] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[HerdSize] [int] NOT NULL,
	[Type] [nvarchar](max) NULL,
	[User_ID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Farm] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Survey]    Script Date: 09/09/2019 12:57:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Survey](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[IntroText] [nvarchar](max) NULL,
	[Version] [int] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[Language] [nvarchar](max) NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Survey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SurveyQuestion]    Script Date: 09/09/2019 12:57:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SurveyQuestion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Survey_ID] [int] NOT NULL,
	[Stage_ID] [int] NOT NULL,
	[QuestionNum] [int] NOT NULL,
	[QuestionText] [nvarchar](max) NULL,
	[QuestionHelpText] [nvarchar](max) NULL,
	[QuestionStatement] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.SurveyQuestion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SurveyQuestionResponse]    Script Date: 09/09/2019 12:57:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SurveyQuestionResponse](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SurveyResponse_ID] [int] NOT NULL,
	[QuestionID] [int] NOT NULL,
	[StageID] [int] NOT NULL,
	[QuestionResponse] [bit] NOT NULL,
	[QuestionStatement] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.SurveyQuestionResponse] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SurveyResponse]    Script Date: 09/09/2019 12:57:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SurveyResponse](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Survey_ID] [int] NOT NULL,
	[SurveyVersion] [int] NOT NULL,
	[SubmittedDate] [datetime] NOT NULL,
	[Farm_ID] [int] NOT NULL,
	[User_ID] [int] NOT NULL,
	[ResponseIdentifier] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.SurveyResponse] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SurveyStage]    Script Date: 09/09/2019 12:57:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SurveyStage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Survey_ID] [int] NOT NULL,
	[StageText] [nvarchar](max) NULL,
	[ShowStageIntro] [bit] NOT NULL,
	[StageTitle] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.SurveyStage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 09/09/2019 12:57:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Token] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[CowStatus] ON 

INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (1, 1, 1, 0, N'001', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (2, 1, 1, 1, N'002', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (3, 1, 1, 0, N'003', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (4, 1, 0, 0, N'004', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (5, 1, 1, 1, N'005', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (6, 1, 0, 0, N'006', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (7, 1, 0, 0, N'007', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (8, 1, 0, 0, N'008', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (9, 1, 0, 0, N'009', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (10, 1, 1, 1, N'010', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (11, 1, 0, 1, N'101', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (12, 1, 0, 0, N'102', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (13, 1, 0, 0, N'103', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (14, 1, 0, 0, N'104', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (15, 1, 1, 0, N'105', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (16, 1, 1, 1, N'106', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (17, 1, 1, 1, N'107', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (18, 1, 1, 0, N'108', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (19, 1, 1, 1, N'109', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (20, 1, 1, 0, N'110', CAST(N'2018-07-08T23:15:22.990' AS DateTime), CAST(N'2018-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (21, 1, 1, 1, N'001', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (22, 1, 1, 0, N'002', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (23, 1, 1, 0, N'003', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (24, 1, 1, 1, N'004', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (25, 1, 1, 0, N'005', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (26, 1, 0, 0, N'006', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (27, 1, 0, 0, N'007', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (28, 1, 0, 1, N'008', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (29, 1, 0, 0, N'009', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (30, 1, 0, 1, N'010', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (31, 1, 1, 1, N'101', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (32, 1, 1, 0, N'102', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (33, 1, 1, 1, N'103', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (34, 1, 1, 0, N'104', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (35, 1, 1, 1, N'105', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (36, 1, 0, 0, N'106', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (37, 1, 0, 1, N'107', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (38, 1, 0, 0, N'108', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (39, 1, 0, 1, N'109', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
INSERT [dbo].[CowStatus] ([ID], [Farm_ID], [InfectedAtDryOff], [InfectedAtCalving], [CowIdentifier], [DateAddedDryOff], [DateAddedCalving]) VALUES (40, 1, 0, 0, N'110', CAST(N'2019-07-08T23:15:22.990' AS DateTime), CAST(N'2019-09-06T23:15:22.990' AS DateTime))
SET IDENTITY_INSERT [dbo].[CowStatus] OFF
SET IDENTITY_INSERT [dbo].[Farm] ON 

INSERT [dbo].[Farm] ([ID], [Name], [Address], [ContactName], [PhoneNumber], [HerdSize], [Type], [User_ID]) VALUES (1, N'Buttercup Farm', N'9 Laneside', N'Farmer Bob', N'99999999', 200, NULL, 1)
INSERT [dbo].[Farm] ([ID], [Name], [Address], [ContactName], [PhoneNumber], [HerdSize], [Type], [User_ID]) VALUES (2, N'Old MacDonalds', N'123 Nursery Row', N'Farmer Harry', N'1234567890', 2000, NULL, 1)
SET IDENTITY_INSERT [dbo].[Farm] OFF
SET IDENTITY_INSERT [dbo].[Survey] ON 

INSERT [dbo].[Survey] ([ID], [Name], [Description], [IntroText], [Version], [LastUpdated], [Language], [Active]) VALUES (1, N'SDCT', N'Farm Assessment Questionnaire', NULL, 1, CAST(N'2019-09-06T23:15:23.550' AS DateTime), N'EN', 1)
SET IDENTITY_INSERT [dbo].[Survey] OFF
SET IDENTITY_INSERT [dbo].[SurveyQuestion] ON 

INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (1, 1, 1, 1, N'Is the farm willing to implement SDCT?', NULL, NULL)
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (2, 1, 1, 2, N'Is the BSCC in the farm lower than 250,000?', NULL, NULL)
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (3, 1, 2, 1, N'Do you have a dry-off strategy?', N'A set dry period length, reliable calving prediction, planning', N'You should work with your vet/MSD Animal Health Respresentative to create a dry-off stategy.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (4, 1, 2, 2, N'Is poor teat-end condition present in less that 15% of cows at dry-off?', NULL, N'Poor teat-end condition should be present in less than 15% of cows at dry off.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (5, 1, 2, 3, N'Is milk production lower than 15kg/day for more than 90% of the cows?', NULL, N'Milk production should be lower than 15 kg/day for more than 90% of the cows.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (6, 1, 2, 4, N'Is milk leakage happening in less that 10% of the cows?', NULL, N'Milk leakage should be happening in less than 10% of the cows.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (7, 1, 2, 5, N'Are cows getting the appropriate energy, protein, dry-matter intake and minerals in the last weeks before dry-off?', N'If all four elements are appropriate, select yes. If fewer than four are appropriate, select no.', N'The cows should be getting the appropriate energy, protein, dry matter intake and minerals in the last weeks before dry off.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (8, 1, 3, 1, N'Does dry-off take place in a clean, comfortable environment and is the correct sequence of events followed?', N'If all three elements are being met, select yes. If fewer than three are being met, select no.', N'Dry off should take place in a clean, comfortable environment and with the correct sequence of events being followed.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (9, 1, 3, 2, N'Are you following correct hygiene protocols, such as using rubber gloves, removing dirt from teats, disinfecting for at least 30 seconds twice and partially inserting the antibiotic or teat seal?', N'If all four criteria are being met, select yes. If fewer than four are being met, select no.', N'You should be following correct hygiene protocols, such as using clean rubber gloves, removing dirt from teats, disinfecting for at least 30 seconds twice and partially inserting the antibiotic or teat seal.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (10, 1, 3, 3, N'Are you using cow somatic cell counts or other reilable test to diagnose infection?', NULL, N'You should be using cow somatic cell counts or another reliable test to diagnose infection.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (11, 1, 3, 4, N'Is your antibiotic and/or teat seal tube selection based on well-supported data?', NULL, N'Your antibiotic and/or teat seal tube selection should be based on well-supported data.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (12, 1, 3, 5, N'Are you mitigating potential stressors, such as commingling, ample space per cow, access to feed and water?', N'If all four criteria are being met, select yes. If fewer than four are being met, select no.', N'You should mitigate potential stressors, such as commingling, ample space per cow, access to feed and access to water.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (13, 1, 4, 1, N'Are the cows udders and thighs clean?', NULL, N'You must ensure cows'' udders and thighs are clean.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (14, 1, 4, 2, N'Have tails been clipped, udders shaven as needed and is bedding refreshed and disinfected regularly?', NULL, N'Tails must be clipped, udders shaven as needed and bedding refreshed and disinfected regularly.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (15, 1, 4, 3, N'Is the calculated ration, fed ration, eaten ration and dry matter intake the same and meeting the standard nutritional requirements?', N'If all four are correct, select yes. If fewer than four are correct, select no.', N'You need to calculate ration, fed ration, eaten ration and dry matter intake is the same and meeting standard nutritional requirements.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (16, 1, 4, 4, N'Are commingling and overcrowding being minimised?', NULL, N'Commingling and overcrowding should be minimised.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (17, 1, 4, 5, N'Does the housing provide proper cubicle size and comfort, ample walking space, feed bunk space and adequate ventilation?', N'If all four are correct, select yes. If fewer than four are correct, select no.', N'The housing should provide proper cubicle size and comfort, ample walking space, feed bunk space and adequate ventilation.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (18, 1, 5, 1, N'Are the cows udders and thighs clean?', NULL, N'You must ensure cows'' udders and thighs are clean.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (19, 1, 5, 2, N'Have tails been clipped, udders shaven as needed and is bedding refreshed and disinfected regularly?', NULL, N'Tails must be clipped, udders shaven as needed and bedding refreshed and disinfected regularly.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (20, 1, 5, 3, N'Is the calculated ration, fed ration, eaten ration and dry matter intake the same and meeting the standard nutritional requirements?', N'If all four are correct, select yes. If fewer than four are correct, select no.', N'You need to calculate ration, fed ration, eaten ration and dry matter intake is the same and meeting standard nutritional requirements.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (21, 1, 5, 4, N'Are commingling and overcrowding being minimised?', NULL, N'Commingling and overcrowding should be minimised.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (22, 1, 5, 5, N'Does the housing provide proper cubicle size and comfort, ample walking space, feed bunk space and adequate ventilation?', N'If all four are correct, select yes. If fewer than four are correct, select no.', N'The housing should provide proper cubicle size and comfort, ample walking space, feed bunk space and adequate ventilation.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (23, 1, 6, 1, N'Does the calving pen provide sufficient space; clean soft bedding; seclusion and available nutrition?', N'If all four are being provided, select yes. If fewer than four are being provided, select no.', N'The calving pen should provide sufficient space; clean, soft and dry bedding; seclusion; and available nutrition.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (24, 1, 6, 2, N'Is the calving pen clean, dry and well ventilated with no sick cows?', N'If all four are correct, select yes. If fewer than four are correct, select no.', N'The calving pen should be clean, dry and well ventilated with no sick cows.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (25, 1, 6, 3, N'Are less than 5% of cows showing visible milk leakage?', NULL, N'Less than 5% of cows should be showing visible milk leakage.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (26, 1, 6, 4, N'Do less than 10% of calvings need assistance?', NULL, N'Less than 10% of calvings should need assistance.')
INSERT [dbo].[SurveyQuestion] ([ID], [Survey_ID], [Stage_ID], [QuestionNum], [QuestionText], [QuestionHelpText], [QuestionStatement]) VALUES (27, 1, 6, 5, N'Is the milking machine for the first milkings after calving functioning properly and thoroughly cleaned and disinfected before and after milkings?', N'If the two are correct, select yes. If fewer than two are correct, select no.', N'The milking machine for the first milkings after calving should be functioning properly and be thoroughly cleaned and disinfected before and after milkings.')
SET IDENTITY_INSERT [dbo].[SurveyQuestion] OFF
SET IDENTITY_INSERT [dbo].[SurveyQuestionResponse] ON 

INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (1, 1, 1, 1, 0, NULL)
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (2, 1, 2, 1, 0, NULL)
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (3, 1, 3, 2, 0, N'You should work with your vet/MSD Animal Health Respresentative to create a dry-off stategy.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (4, 1, 4, 2, 1, N'Poor teat-end condition should be present in less than 15% of cows at dry off.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (5, 1, 5, 2, 0, N'Milk production should be lower than 15 kg/day for more than 90% of the cows.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (6, 1, 6, 2, 0, N'Milk leakage should be happening in less than 10% of the cows.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (7, 1, 7, 2, 1, N'The cows should be getting the appropriate energy, protein, dry matter intake and minerals in the last weeks before dry off.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (8, 1, 8, 3, 0, N'Dry off should take place in a clean, comfortable environment and with the correct sequence of events being followed.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (9, 1, 9, 3, 0, N'You should be following correct hygiene protocols, such as using clean rubber gloves, removing dirt from teats, disinfecting for at least 30 seconds twice and partially inserting the antibiotic or teat seal.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (10, 1, 10, 3, 0, N'You should be using cow somatic cell counts or another reliable test to diagnose infection.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (11, 1, 11, 3, 0, N'Your antibiotic and/or teat seal tube selection should be based on well-supported data.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (12, 1, 12, 3, 1, N'You should mitigate potential stressors, such as commingling, ample space per cow, access to feed and access to water.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (13, 1, 13, 4, 0, N'You must ensure cows'' udders and thighs are clean.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (14, 1, 14, 4, 0, N'Tails must be clipped, udders shaven as needed and bedding refreshed and disinfected regularly.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (15, 1, 15, 4, 1, N'You need to calculate ration, fed ration, eaten ration and dry matter intake is the same and meeting standard nutritional requirements.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (16, 1, 16, 4, 1, N'Commingling and overcrowding should be minimised.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (17, 1, 17, 4, 1, N'The housing should provide proper cubicle size and comfort, ample walking space, feed bunk space and adequate ventilation.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (18, 1, 18, 5, 0, N'You must ensure cows'' udders and thighs are clean.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (19, 1, 19, 5, 0, N'Tails must be clipped, udders shaven as needed and bedding refreshed and disinfected regularly.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (20, 1, 20, 5, 1, N'You need to calculate ration, fed ration, eaten ration and dry matter intake is the same and meeting standard nutritional requirements.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (21, 1, 21, 5, 1, N'Commingling and overcrowding should be minimised.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (22, 1, 22, 5, 0, N'The housing should provide proper cubicle size and comfort, ample walking space, feed bunk space and adequate ventilation.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (23, 1, 23, 6, 1, N'The calving pen should provide sufficient space; clean, soft and dry bedding; seclusion; and available nutrition.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (24, 1, 24, 6, 1, N'The calving pen should be clean, dry and well ventilated with no sick cows.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (25, 1, 25, 6, 1, N'Less than 5% of cows should be showing visible milk leakage.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (26, 1, 26, 6, 0, N'Less than 10% of calvings should need assistance.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (27, 1, 27, 6, 1, N'The milking machine for the first milkings after calving should be functioning properly and be thoroughly cleaned and disinfected before and after milkings.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (28, 2, 1, 1, 0, NULL)
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (29, 2, 2, 1, 0, NULL)
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (30, 2, 3, 2, 0, N'You should work with your vet/MSD Animal Health Respresentative to create a dry-off stategy.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (31, 2, 4, 2, 1, N'Poor teat-end condition should be present in less than 15% of cows at dry off.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (32, 2, 5, 2, 1, N'Milk production should be lower than 15 kg/day for more than 90% of the cows.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (33, 2, 6, 2, 1, N'Milk leakage should be happening in less than 10% of the cows.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (34, 2, 7, 2, 0, N'The cows should be getting the appropriate energy, protein, dry matter intake and minerals in the last weeks before dry off.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (35, 2, 8, 3, 1, N'Dry off should take place in a clean, comfortable environment and with the correct sequence of events being followed.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (36, 2, 9, 3, 1, N'You should be following correct hygiene protocols, such as using clean rubber gloves, removing dirt from teats, disinfecting for at least 30 seconds twice and partially inserting the antibiotic or teat seal.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (37, 2, 10, 3, 1, N'You should be using cow somatic cell counts or another reliable test to diagnose infection.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (38, 2, 11, 3, 1, N'Your antibiotic and/or teat seal tube selection should be based on well-supported data.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (39, 2, 12, 3, 1, N'You should mitigate potential stressors, such as commingling, ample space per cow, access to feed and access to water.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (40, 2, 13, 4, 1, N'You must ensure cows'' udders and thighs are clean.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (41, 2, 14, 4, 1, N'Tails must be clipped, udders shaven as needed and bedding refreshed and disinfected regularly.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (42, 2, 15, 4, 0, N'You need to calculate ration, fed ration, eaten ration and dry matter intake is the same and meeting standard nutritional requirements.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (43, 2, 16, 4, 0, N'Commingling and overcrowding should be minimised.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (44, 2, 17, 4, 0, N'The housing should provide proper cubicle size and comfort, ample walking space, feed bunk space and adequate ventilation.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (45, 2, 18, 5, 0, N'You must ensure cows'' udders and thighs are clean.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (46, 2, 19, 5, 0, N'Tails must be clipped, udders shaven as needed and bedding refreshed and disinfected regularly.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (47, 2, 20, 5, 1, N'You need to calculate ration, fed ration, eaten ration and dry matter intake is the same and meeting standard nutritional requirements.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (48, 2, 21, 5, 0, N'Commingling and overcrowding should be minimised.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (49, 2, 22, 5, 0, N'The housing should provide proper cubicle size and comfort, ample walking space, feed bunk space and adequate ventilation.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (50, 2, 23, 6, 1, N'The calving pen should provide sufficient space; clean, soft and dry bedding; seclusion; and available nutrition.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (51, 2, 24, 6, 1, N'The calving pen should be clean, dry and well ventilated with no sick cows.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (52, 2, 25, 6, 1, N'Less than 5% of cows should be showing visible milk leakage.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (53, 2, 26, 6, 1, N'Less than 10% of calvings should need assistance.')
INSERT [dbo].[SurveyQuestionResponse] ([ID], [SurveyResponse_ID], [QuestionID], [StageID], [QuestionResponse], [QuestionStatement]) VALUES (54, 2, 27, 6, 1, N'The milking machine for the first milkings after calving should be functioning properly and be thoroughly cleaned and disinfected before and after milkings.')
SET IDENTITY_INSERT [dbo].[SurveyQuestionResponse] OFF
SET IDENTITY_INSERT [dbo].[SurveyResponse] ON 

INSERT [dbo].[SurveyResponse] ([ID], [Survey_ID], [SurveyVersion], [SubmittedDate], [Farm_ID], [User_ID], [ResponseIdentifier]) VALUES (1, 1, 1, CAST(N'2019-09-06T23:15:22.990' AS DateTime), 1, 1, N'd2f25b72-6eef-42b5-9244-38f2f25f834d')
INSERT [dbo].[SurveyResponse] ([ID], [Survey_ID], [SurveyVersion], [SubmittedDate], [Farm_ID], [User_ID], [ResponseIdentifier]) VALUES (2, 1, 1, CAST(N'2018-09-06T23:15:22.990' AS DateTime), 1, 1, N'040e3bdc-5dab-4259-a3e8-12427734c221')
SET IDENTITY_INSERT [dbo].[SurveyResponse] OFF
SET IDENTITY_INSERT [dbo].[SurveyStage] ON 

INSERT [dbo].[SurveyStage] ([ID], [Survey_ID], [StageText], [ShowStageIntro], [StageTitle]) VALUES (1, 1, NULL, 0, N'Farm Suitability')
INSERT [dbo].[SurveyStage] ([ID], [Survey_ID], [StageText], [ShowStageIntro], [StageTitle]) VALUES (2, 1, N'Dry-off Preparation', 1, N'Stage 1')
INSERT [dbo].[SurveyStage] ([ID], [Survey_ID], [StageText], [ShowStageIntro], [StageTitle]) VALUES (3, 1, N'Dry-off', 1, N'Stage 2')
INSERT [dbo].[SurveyStage] ([ID], [Survey_ID], [StageText], [ShowStageIntro], [StageTitle]) VALUES (4, 1, N'Far-off', 1, N'Stage 3')
INSERT [dbo].[SurveyStage] ([ID], [Survey_ID], [StageText], [ShowStageIntro], [StageTitle]) VALUES (5, 1, N'Close up', 1, N'Stage 4')
INSERT [dbo].[SurveyStage] ([ID], [Survey_ID], [StageText], [ShowStageIntro], [StageTitle]) VALUES (6, 1, N'Calving', 1, N'Stage 5')
SET IDENTITY_INSERT [dbo].[SurveyStage] OFF
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([ID], [Token], [Name]) VALUES (1, N'PMN_TEST_TOKEN', N'Vicky the Vet')
SET IDENTITY_INSERT [dbo].[User] OFF
ALTER TABLE [dbo].[SurveyResponse] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [ResponseIdentifier]
GO
ALTER TABLE [dbo].[CowStatus]  WITH CHECK ADD  CONSTRAINT [FK_dbo.CowStatus_dbo.Farm_Farm_ID] FOREIGN KEY([Farm_ID])
REFERENCES [dbo].[Farm] ([ID])
GO
ALTER TABLE [dbo].[CowStatus] CHECK CONSTRAINT [FK_dbo.CowStatus_dbo.Farm_Farm_ID]
GO
ALTER TABLE [dbo].[Farm]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Farm_dbo.User_User_ID] FOREIGN KEY([User_ID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Farm] CHECK CONSTRAINT [FK_dbo.Farm_dbo.User_User_ID]
GO
ALTER TABLE [dbo].[SurveyQuestion]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SurveyQuestion_dbo.Survey_Survey_ID] FOREIGN KEY([Survey_ID])
REFERENCES [dbo].[Survey] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SurveyQuestion] CHECK CONSTRAINT [FK_dbo.SurveyQuestion_dbo.Survey_Survey_ID]
GO
ALTER TABLE [dbo].[SurveyQuestion]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SurveyQuestion_dbo.SurveyStage_Stage_ID] FOREIGN KEY([Stage_ID])
REFERENCES [dbo].[SurveyStage] ([ID])
GO
ALTER TABLE [dbo].[SurveyQuestion] CHECK CONSTRAINT [FK_dbo.SurveyQuestion_dbo.SurveyStage_Stage_ID]
GO
ALTER TABLE [dbo].[SurveyQuestionResponse]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SurveyQuestionResponse_dbo.SurveyResponse_SurveyResponse_ID] FOREIGN KEY([SurveyResponse_ID])
REFERENCES [dbo].[SurveyResponse] ([ID])
GO
ALTER TABLE [dbo].[SurveyQuestionResponse] CHECK CONSTRAINT [FK_dbo.SurveyQuestionResponse_dbo.SurveyResponse_SurveyResponse_ID]
GO
ALTER TABLE [dbo].[SurveyResponse]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SurveyResponse_dbo.Farm_Farm_ID] FOREIGN KEY([Farm_ID])
REFERENCES [dbo].[Farm] ([ID])
GO
ALTER TABLE [dbo].[SurveyResponse] CHECK CONSTRAINT [FK_dbo.SurveyResponse_dbo.Farm_Farm_ID]
GO
ALTER TABLE [dbo].[SurveyResponse]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SurveyResponse_dbo.Survey_Survey_ID] FOREIGN KEY([Survey_ID])
REFERENCES [dbo].[Survey] ([ID])
GO
ALTER TABLE [dbo].[SurveyResponse] CHECK CONSTRAINT [FK_dbo.SurveyResponse_dbo.Survey_Survey_ID]
GO
ALTER TABLE [dbo].[SurveyResponse]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SurveyResponse_dbo.User_User_ID] FOREIGN KEY([User_ID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[SurveyResponse] CHECK CONSTRAINT [FK_dbo.SurveyResponse_dbo.User_User_ID]
GO
ALTER TABLE [dbo].[SurveyStage]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SurveyStage_dbo.Survey_Survey_ID] FOREIGN KEY([Survey_ID])
REFERENCES [dbo].[Survey] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SurveyStage] CHECK CONSTRAINT [FK_dbo.SurveyStage_dbo.Survey_Survey_ID]
GO
