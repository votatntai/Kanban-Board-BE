USE [master]
GO
/****** Object:  Database [Kanban]    Script Date: 2/1/2023 6:12:30 PM ******/
CREATE DATABASE [Kanbans]
GO
USE [Kanbans]
GO
/****** Object:  Table [dbo].[Attachment]    Script Date: 2/1/2023 6:12:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attachment](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
	[IssueId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comment]    Script Date: 2/1/2023 6:12:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comment](
	[Id] [uniqueidentifier] NOT NULL,
	[IssueId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Content] [nvarchar](256) NOT NULL,
	[CreateAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Issue]    Script Date: 2/1/2023 6:12:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Issue](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsChild] [bit] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[AssigneeId] [uniqueidentifier] NULL,
	[EstimateTime] [int] NULL,
	[PriorityId] [uniqueidentifier] NOT NULL,
	[StatusId] [uniqueidentifier] NOT NULL,
	[TypeId] [uniqueidentifier] NOT NULL,
	[DueDate] [datetime] NULL,
	[Position] [int] NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[ReporterId] [uniqueidentifier] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NULL,
	[ResolveAt] [datetime] NULL,
	[IsClose] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IssueLabel]    Script Date: 2/1/2023 6:12:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IssueLabel](
	[IssueId] [uniqueidentifier] NOT NULL,
	[LabelId] [uniqueidentifier] NOT NULL,
	[UpdateAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[IssueId] ASC,
	[LabelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Label]    Script Date: 2/1/2023 6:12:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Label](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[UpdateAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Link]    Script Date: 2/1/2023 6:12:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Link](
	[Id] [uniqueidentifier] NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IssueId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogWork]    Script Date: 2/1/2023 6:12:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogWork](
	[Id] [uniqueidentifier] NOT NULL,
	[IssueId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[SpentTime] [int] NOT NULL,
	[Description] [nvarchar](256) NULL,
	[RemainingTime] [int] NOT NULL,
	[CreateAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Priority]    Script Date: 2/1/2023 6:12:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Priority](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Value] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project]    Script Date: 2/1/2023 6:12:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[LeaderId] [uniqueidentifier] NOT NULL,
	[DefaultAssigneeId] [uniqueidentifier] NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NULL,
	[LastActivity] [datetime] NULL,
	[IsClose] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectMember]    Script Date: 2/1/2023 6:12:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectMember](
	[ProjectId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[JoinAt] [datetime] NOT NULL,
	[IsOwner] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectPriority]    Script Date: 2/1/2023 6:12:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectPriority](
	[ProjectId] [uniqueidentifier] NOT NULL,
	[PriorityId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[PriorityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 2/1/2023 6:12:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 2/1/2023 6:12:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[Position] [int] NOT NULL,
	[IsFirst] [bit] NOT NULL,
	[IsLast] [bit] NOT NULL,
	[Limit] [int] NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Type]    Script Date: 2/1/2023 6:12:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Type](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2/1/2023 6:12:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[Username] [nvarchar](256) NOT NULL,
	[Password] [nvarchar](256) NOT NULL,
	[Status] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 2/1/2023 6:12:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Comment] ([Id], [IssueId], [UserId], [Content], [CreateAt]) VALUES (N'54ae037d-e283-4c03-b6dc-9991dd34896e', N'e281855a-abdb-45d7-b13c-d2f3354e08e6', N'125ef4d8-90e1-485b-837d-0640ee9f107d', N'nam ngu l', CAST(N'2023-01-31T12:28:12.047' AS DateTime))
GO
INSERT [dbo].[Issue] ([Id], [Name], [Description], [IsChild], [ParentId], [AssigneeId], [EstimateTime], [PriorityId], [StatusId], [TypeId], [DueDate], [Position], [ProjectId], [ReporterId], [CreateAt], [UpdateAt], [ResolveAt], [IsClose]) VALUES (N'68b0fb91-6210-4c78-b27a-25fc053d0740', N'Child issue 1', NULL, 1, N'7af34860-b4a0-4c61-994d-d8b567558b64', NULL, 0, N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'ba8637dc-ed96-449b-b21e-6ff387fbe8d4', N'd5f5d1f2-7f49-4e16-9c3a-a89956d818da', NULL, 65536, N'f995eab3-30af-4678-8933-4982c381ebbb', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-02-01T17:08:49.283' AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Issue] ([Id], [Name], [Description], [IsChild], [ParentId], [AssigneeId], [EstimateTime], [PriorityId], [StatusId], [TypeId], [DueDate], [Position], [ProjectId], [ReporterId], [CreateAt], [UpdateAt], [ResolveAt], [IsClose]) VALUES (N'813738a3-a846-4438-9834-2b467a73c30f', N'Xin chào', NULL, 0, NULL, NULL, NULL, N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'9e257b1d-c9e2-4fb7-b511-6195b4d6cf31', N'd5f5d1f2-7f49-4e16-9c3a-a89956d818da', CAST(N'2023-01-19T00:00:00.000' AS DateTime), 32768, N'f995eab3-30af-4678-8933-4982c381ebbb', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-01-18T18:20:30.783' AS DateTime), CAST(N'2023-02-01T17:54:18.023' AS DateTime), NULL, 0)
INSERT [dbo].[Issue] ([Id], [Name], [Description], [IsChild], [ParentId], [AssigneeId], [EstimateTime], [PriorityId], [StatusId], [TypeId], [DueDate], [Position], [ProjectId], [ReporterId], [CreateAt], [UpdateAt], [ResolveAt], [IsClose]) VALUES (N'fd2157f7-087d-47a8-b093-30c059d103d9', N'Test as a as as as', NULL, 0, NULL, N'125ef4d8-90e1-485b-837d-0640ee9f107d', 4, N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'70cd0630-ef39-44f9-9ca2-094312dce617', N'2009e244-7356-43b5-9b46-3d92ddbddb56', CAST(N'2023-01-24T00:00:00.000' AS DateTime), 16384, N'1372d8c7-0ad1-4181-8ce0-eee5a4fdd9c7', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-01-19T12:15:46.623' AS DateTime), CAST(N'2023-01-30T12:12:12.687' AS DateTime), NULL, 0)
INSERT [dbo].[Issue] ([Id], [Name], [Description], [IsChild], [ParentId], [AssigneeId], [EstimateTime], [PriorityId], [StatusId], [TypeId], [DueDate], [Position], [ProjectId], [ReporterId], [CreateAt], [UpdateAt], [ResolveAt], [IsClose]) VALUES (N'f1b0729e-0425-45b2-a5c7-35b0a96f211e', N'Tìm kiếm gỗ để nhóm lửa sưởi ấm', N'Bất kỳ loại gỗ nào có thể nhóm lửa', 0, NULL, N'6b2a36d3-1033-4d8c-96b4-6d62e1e74e28', 10, N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'feebdf62-6bef-47bf-82af-322dc3684ee8', N'0d04c5bf-3281-4633-a529-48b788e2dfc7', CAST(N'2023-01-31T00:00:00.000' AS DateTime), 32768, N'68fd12c2-5923-4d74-b569-07d8012e836e', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-01-18T15:56:05.670' AS DateTime), CAST(N'2023-02-01T16:23:32.270' AS DateTime), CAST(N'2023-02-01T16:23:32.270' AS DateTime), 1)
INSERT [dbo].[Issue] ([Id], [Name], [Description], [IsChild], [ParentId], [AssigneeId], [EstimateTime], [PriorityId], [StatusId], [TypeId], [DueDate], [Position], [ProjectId], [ReporterId], [CreateAt], [UpdateAt], [ResolveAt], [IsClose]) VALUES (N'ecdf26cd-e2ca-4a43-9385-36663fd33df6', N'asdasd asd as as ddas', NULL, 0, NULL, N'6b2a36d3-1033-4d8c-96b4-6d62e1e74e28', 0, N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'b0d43e6b-6796-41bd-858d-db0883331b43', N'8373f567-761b-40c3-9913-29d8c16e45bb', NULL, 65536, N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-01-30T16:02:11.453' AS DateTime), CAST(N'2023-02-01T11:08:34.550' AS DateTime), NULL, 0)
INSERT [dbo].[Issue] ([Id], [Name], [Description], [IsChild], [ParentId], [AssigneeId], [EstimateTime], [PriorityId], [StatusId], [TypeId], [DueDate], [Position], [ProjectId], [ReporterId], [CreateAt], [UpdateAt], [ResolveAt], [IsClose]) VALUES (N'33530ec4-7976-4392-9283-50954d3c7d83', N'asdasd', NULL, 0, NULL, NULL, NULL, N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'ac73a46d-d2d4-4077-9963-5bce964febe5', N'0d04c5bf-3281-4633-a529-48b788e2dfc7', NULL, 65536, N'68fd12c2-5923-4d74-b569-07d8012e836e', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-02-01T15:15:39.343' AS DateTime), CAST(N'2023-02-01T16:11:11.893' AS DateTime), NULL, 0)
INSERT [dbo].[Issue] ([Id], [Name], [Description], [IsChild], [ParentId], [AssigneeId], [EstimateTime], [PriorityId], [StatusId], [TypeId], [DueDate], [Position], [ProjectId], [ReporterId], [CreateAt], [UpdateAt], [ResolveAt], [IsClose]) VALUES (N'fb7b9af6-717c-4890-b2db-87870c992c49', N'asdas', NULL, 0, NULL, NULL, 0, N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'70cd0630-ef39-44f9-9ca2-094312dce617', N'2009e244-7356-43b5-9b46-3d92ddbddb56', NULL, 8192, N'1372d8c7-0ad1-4181-8ce0-eee5a4fdd9c7', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-01-30T12:12:02.813' AS DateTime), CAST(N'2023-01-30T12:12:07.250' AS DateTime), NULL, 0)
INSERT [dbo].[Issue] ([Id], [Name], [Description], [IsChild], [ParentId], [AssigneeId], [EstimateTime], [PriorityId], [StatusId], [TypeId], [DueDate], [Position], [ProjectId], [ReporterId], [CreateAt], [UpdateAt], [ResolveAt], [IsClose]) VALUES (N'7b6971fc-e670-4e6e-b6da-c11408f591c6', N'tài đẹp chai', NULL, 1, N'f1b0729e-0425-45b2-a5c7-35b0a96f211e', N'6b2a36d3-1033-4d8c-96b4-6d62e1e74e28', NULL, N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'feebdf62-6bef-47bf-82af-322dc3684ee8', N'0d04c5bf-3281-4633-a529-48b788e2dfc7', NULL, 65536, N'68fd12c2-5923-4d74-b569-07d8012e836e', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-02-01T14:15:39.827' AS DateTime), CAST(N'2023-02-01T16:11:20.287' AS DateTime), CAST(N'2023-02-01T16:11:20.213' AS DateTime), 1)
INSERT [dbo].[Issue] ([Id], [Name], [Description], [IsChild], [ParentId], [AssigneeId], [EstimateTime], [PriorityId], [StatusId], [TypeId], [DueDate], [Position], [ProjectId], [ReporterId], [CreateAt], [UpdateAt], [ResolveAt], [IsClose]) VALUES (N'0cae1a2e-218f-4e1f-8cc7-c69c4fa91ad2', N'Lorem ipsum dolar', NULL, 0, NULL, NULL, 0, N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'b02dcfab-e1b3-4442-99d8-22129088020f', N'2009e244-7356-43b5-9b46-3d92ddbddb56', NULL, 65536, N'1372d8c7-0ad1-4181-8ce0-eee5a4fdd9c7', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-01-19T12:15:34.187' AS DateTime), CAST(N'2023-01-30T12:11:57.710' AS DateTime), NULL, 0)
INSERT [dbo].[Issue] ([Id], [Name], [Description], [IsChild], [ParentId], [AssigneeId], [EstimateTime], [PriorityId], [StatusId], [TypeId], [DueDate], [Position], [ProjectId], [ReporterId], [CreateAt], [UpdateAt], [ResolveAt], [IsClose]) VALUES (N'3736a28a-b5ac-41db-b119-cc06f8d2d95a', N'Anh chang sao ma', NULL, 0, NULL, NULL, 0, N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'8c83ab27-7e44-48b4-9d51-d2ff3481eb9a', N'bcd08550-0448-4f33-bc99-11918d6ef5a3', NULL, 16384, N'bd6eb455-b335-4bae-97ae-cc4455d1a2c5', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-01-18T12:56:58.167' AS DateTime), CAST(N'2023-01-31T12:41:29.697' AS DateTime), NULL, 0)
INSERT [dbo].[Issue] ([Id], [Name], [Description], [IsChild], [ParentId], [AssigneeId], [EstimateTime], [PriorityId], [StatusId], [TypeId], [DueDate], [Position], [ProjectId], [ReporterId], [CreateAt], [UpdateAt], [ResolveAt], [IsClose]) VALUES (N'e281855a-abdb-45d7-b13c-d2f3354e08e6', N'Mùa thu hôm qu', N'haha kia', 0, NULL, N'125ef4d8-90e1-485b-837d-0640ee9f107d', NULL, N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'3fcbac7a-8e0f-48b9-b4b8-db86a72da674', N'bcd08550-0448-4f33-bc99-11918d6ef5a3', CAST(N'2023-01-20T00:00:00.000' AS DateTime), 100, N'bd6eb455-b335-4bae-97ae-cc4455d1a2c5', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-01-12T22:22:16.877' AS DateTime), CAST(N'2023-01-31T12:41:30.697' AS DateTime), NULL, 0)
INSERT [dbo].[Issue] ([Id], [Name], [Description], [IsChild], [ParentId], [AssigneeId], [EstimateTime], [PriorityId], [StatusId], [TypeId], [DueDate], [Position], [ProjectId], [ReporterId], [CreateAt], [UpdateAt], [ResolveAt], [IsClose]) VALUES (N'f3a8c745-dd87-4f1c-b3d5-d556d23d7bfd', N'Child issue 2', NULL, 1, N'7af34860-b4a0-4c61-994d-d8b567558b64', NULL, 0, N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'ba8637dc-ed96-449b-b21e-6ff387fbe8d4', N'd5f5d1f2-7f49-4e16-9c3a-a89956d818da', NULL, 131072, N'f995eab3-30af-4678-8933-4982c381ebbb', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-02-01T17:08:52.857' AS DateTime), NULL, NULL, 0)
INSERT [dbo].[Issue] ([Id], [Name], [Description], [IsChild], [ParentId], [AssigneeId], [EstimateTime], [PriorityId], [StatusId], [TypeId], [DueDate], [Position], [ProjectId], [ReporterId], [CreateAt], [UpdateAt], [ResolveAt], [IsClose]) VALUES (N'7af34860-b4a0-4c61-994d-d8b567558b64', N'Issue cha nè mấy đứa', NULL, 0, NULL, NULL, NULL, N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'ba8637dc-ed96-449b-b21e-6ff387fbe8d4', N'd5f5d1f2-7f49-4e16-9c3a-a89956d818da', NULL, 32768, N'f995eab3-30af-4678-8933-4982c381ebbb', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-02-01T17:08:42.393' AS DateTime), CAST(N'2023-02-01T18:01:48.263' AS DateTime), NULL, 0)
INSERT [dbo].[Issue] ([Id], [Name], [Description], [IsChild], [ParentId], [AssigneeId], [EstimateTime], [PriorityId], [StatusId], [TypeId], [DueDate], [Position], [ProjectId], [ReporterId], [CreateAt], [UpdateAt], [ResolveAt], [IsClose]) VALUES (N'041b8fd5-4f0f-4211-8efa-df70ae0cc54c', N'Ngọn lửa tình chưa hẵn đã tàn phai', N'Vai lon luon ban oi haha a', 0, NULL, N'125ef4d8-90e1-485b-837d-0640ee9f107d', 8, N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'3fcbac7a-8e0f-48b9-b4b8-db86a72da674', N'bcd08550-0448-4f33-bc99-11918d6ef5a3', NULL, 200, N'bd6eb455-b335-4bae-97ae-cc4455d1a2c5', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-01-12T20:16:14.243' AS DateTime), CAST(N'2023-01-31T12:41:28.787' AS DateTime), NULL, 0)
GO
INSERT [dbo].[IssueLabel] ([IssueId], [LabelId], [UpdateAt]) VALUES (N'f1b0729e-0425-45b2-a5c7-35b0a96f211e', N'c12c0714-b501-4782-952f-09fa34b2fdcc', CAST(N'2023-02-01T16:08:44.143' AS DateTime))
INSERT [dbo].[IssueLabel] ([IssueId], [LabelId], [UpdateAt]) VALUES (N'ecdf26cd-e2ca-4a43-9385-36663fd33df6', N'091bbd7c-70c2-4d0e-a951-6babb1e74a8c', CAST(N'2023-02-01T11:08:34.550' AS DateTime))
INSERT [dbo].[IssueLabel] ([IssueId], [LabelId], [UpdateAt]) VALUES (N'7b6971fc-e670-4e6e-b6da-c11408f591c6', N'c12c0714-b501-4782-952f-09fa34b2fdcc', CAST(N'2023-02-01T16:11:20.263' AS DateTime))
INSERT [dbo].[IssueLabel] ([IssueId], [LabelId], [UpdateAt]) VALUES (N'7b6971fc-e670-4e6e-b6da-c11408f591c6', N'0f877e01-a51f-4d5a-b2a6-a051df34243b', CAST(N'2023-02-01T16:11:20.263' AS DateTime))
INSERT [dbo].[IssueLabel] ([IssueId], [LabelId], [UpdateAt]) VALUES (N'3736a28a-b5ac-41db-b119-cc06f8d2d95a', N'8ad21e5f-3f87-45ef-a245-273b44ff2bdf', CAST(N'2023-01-30T13:18:13.117' AS DateTime))
INSERT [dbo].[IssueLabel] ([IssueId], [LabelId], [UpdateAt]) VALUES (N'3736a28a-b5ac-41db-b119-cc06f8d2d95a', N'63c2f179-9aa4-4589-b41c-3a6446b06003', CAST(N'2023-01-30T13:18:13.117' AS DateTime))
INSERT [dbo].[IssueLabel] ([IssueId], [LabelId], [UpdateAt]) VALUES (N'3736a28a-b5ac-41db-b119-cc06f8d2d95a', N'18816d00-4a1c-49b4-8d52-51b2817ce976', CAST(N'2023-01-30T13:18:13.117' AS DateTime))
INSERT [dbo].[IssueLabel] ([IssueId], [LabelId], [UpdateAt]) VALUES (N'e281855a-abdb-45d7-b13c-d2f3354e08e6', N'63c2f179-9aa4-4589-b41c-3a6446b06003', CAST(N'2023-01-18T11:34:38.473' AS DateTime))
INSERT [dbo].[IssueLabel] ([IssueId], [LabelId], [UpdateAt]) VALUES (N'041b8fd5-4f0f-4211-8efa-df70ae0cc54c', N'8ad21e5f-3f87-45ef-a245-273b44ff2bdf', CAST(N'2023-01-30T16:05:27.487' AS DateTime))
INSERT [dbo].[IssueLabel] ([IssueId], [LabelId], [UpdateAt]) VALUES (N'041b8fd5-4f0f-4211-8efa-df70ae0cc54c', N'18816d00-4a1c-49b4-8d52-51b2817ce976', CAST(N'2023-01-30T16:05:27.487' AS DateTime))
GO
INSERT [dbo].[Label] ([Id], [Name], [ProjectId], [UpdateAt]) VALUES (N'c12c0714-b501-4782-952f-09fa34b2fdcc', N'chwe', N'68fd12c2-5923-4d74-b569-07d8012e836e', CAST(N'2023-01-31T12:52:06.363' AS DateTime))
INSERT [dbo].[Label] ([Id], [Name], [ProjectId], [UpdateAt]) VALUES (N'8ad21e5f-3f87-45ef-a245-273b44ff2bdf', N'Back-end', N'bd6eb455-b335-4bae-97ae-cc4455d1a2c5', NULL)
INSERT [dbo].[Label] ([Id], [Name], [ProjectId], [UpdateAt]) VALUES (N'63c2f179-9aa4-4589-b41c-3a6446b06003', N'Front-end', N'bd6eb455-b335-4bae-97ae-cc4455d1a2c5', NULL)
INSERT [dbo].[Label] ([Id], [Name], [ProjectId], [UpdateAt]) VALUES (N'18816d00-4a1c-49b4-8d52-51b2817ce976', N'Full-stack', N'bd6eb455-b335-4bae-97ae-cc4455d1a2c5', NULL)
INSERT [dbo].[Label] ([Id], [Name], [ProjectId], [UpdateAt]) VALUES (N'091bbd7c-70c2-4d0e-a951-6babb1e74a8c', N'asd', N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', CAST(N'2023-01-30T15:56:44.303' AS DateTime))
INSERT [dbo].[Label] ([Id], [Name], [ProjectId], [UpdateAt]) VALUES (N'0f877e01-a51f-4d5a-b2a6-a051df34243b', N'con cac', N'68fd12c2-5923-4d74-b569-07d8012e836e', CAST(N'2023-02-01T15:54:06.890' AS DateTime))
INSERT [dbo].[Label] ([Id], [Name], [ProjectId], [UpdateAt]) VALUES (N'd37a94fd-3e3d-4a23-ba99-e5e227a19796', N'hahaha ha', N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', CAST(N'2023-01-30T15:58:15.420' AS DateTime))
GO
INSERT [dbo].[Link] ([Id], [Url], [Description], [IssueId]) VALUES (N'e7195d94-8bfe-4d6d-9530-def237490d50', N'https://dinhtrong091297.atlassian.net/jira/software/projects/FLEX/boards/1', N'Demo link test for this issue', N'f1b0729e-0425-45b2-a5c7-35b0a96f211e')
GO
INSERT [dbo].[LogWork] ([Id], [IssueId], [UserId], [SpentTime], [Description], [RemainingTime], [CreateAt]) VALUES (N'cbae0887-c370-4f0d-a65e-f91b04bc49f1', N'7b6971fc-e670-4e6e-b6da-c11408f591c6', N'125ef4d8-90e1-485b-837d-0640ee9f107d', 3, N'Hoàn thành xong phần login', 0, CAST(N'2023-02-01T15:51:39.723' AS DateTime))
GO
INSERT [dbo].[Priority] ([Id], [Name], [Value], [Description]) VALUES (N'eb3add8a-31d9-4f56-a3e8-06f2997c2519', N'High', 2, N'High')
INSERT [dbo].[Priority] ([Id], [Name], [Value], [Description]) VALUES (N'5bbc2686-e824-4a41-91f6-0b0ad8146ec3', N'Highest', 1, N'Highest')
INSERT [dbo].[Priority] ([Id], [Name], [Value], [Description]) VALUES (N'934db542-323b-4c26-9ff3-2edde11c8995', N'Lowest', 5, N'Lowest')
INSERT [dbo].[Priority] ([Id], [Name], [Value], [Description]) VALUES (N'66c8019b-bc05-46d6-baee-c09bf0390546', N'Low', 4, N'Low')
INSERT [dbo].[Priority] ([Id], [Name], [Value], [Description]) VALUES (N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'Medium', 3, N'Medium')
GO
INSERT [dbo].[Project] ([Id], [Name], [Description], [LeaderId], [DefaultAssigneeId], [CreateAt], [UpdateAt], [LastActivity], [IsClose]) VALUES (N'68fd12c2-5923-4d74-b569-07d8012e836e', N'Thử thách sinh tồn 24h', N'Thử thách sinh tồn tại đảo Phú Quốc', N'125ef4d8-90e1-485b-837d-0640ee9f107d', NULL, CAST(N'2023-01-18T15:55:21.293' AS DateTime), NULL, CAST(N'2023-02-01T16:23:32.270' AS DateTime), 0)
INSERT [dbo].[Project] ([Id], [Name], [Description], [LeaderId], [DefaultAssigneeId], [CreateAt], [UpdateAt], [LastActivity], [IsClose]) VALUES (N'f995eab3-30af-4678-8933-4982c381ebbb', N'Dự án trung tâm thương mại', N'Khu phực hợp giải trí mua sắm 3/2
', N'125ef4d8-90e1-485b-837d-0640ee9f107d', NULL, CAST(N'2023-01-18T15:39:27.397' AS DateTime), NULL, CAST(N'2023-02-01T18:01:48.263' AS DateTime), 0)
INSERT [dbo].[Project] ([Id], [Name], [Description], [LeaderId], [DefaultAssigneeId], [CreateAt], [UpdateAt], [LastActivity], [IsClose]) VALUES (N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', N'Subtask Demo', NULL, N'125ef4d8-90e1-485b-837d-0640ee9f107d', NULL, CAST(N'2023-01-28T13:29:59.283' AS DateTime), NULL, CAST(N'2023-02-01T11:08:34.550' AS DateTime), 0)
INSERT [dbo].[Project] ([Id], [Name], [Description], [LeaderId], [DefaultAssigneeId], [CreateAt], [UpdateAt], [LastActivity], [IsClose]) VALUES (N'bd6eb455-b335-4bae-97ae-cc4455d1a2c5', N'Dự án của Janglee', N'A di đà phật', N'125ef4d8-90e1-485b-837d-0640ee9f107d', NULL, CAST(N'2023-01-09T21:01:15.343' AS DateTime), NULL, CAST(N'2023-01-31T12:41:30.697' AS DateTime), 0)
INSERT [dbo].[Project] ([Id], [Name], [Description], [LeaderId], [DefaultAssigneeId], [CreateAt], [UpdateAt], [LastActivity], [IsClose]) VALUES (N'1372d8c7-0ad1-4181-8ce0-eee5a4fdd9c7', N'Dự án Tết', NULL, N'125ef4d8-90e1-485b-837d-0640ee9f107d', NULL, CAST(N'2023-01-19T12:12:27.613' AS DateTime), NULL, CAST(N'2023-01-30T12:12:12.687' AS DateTime), 0)
GO
INSERT [dbo].[ProjectMember] ([ProjectId], [UserId], [JoinAt], [IsOwner]) VALUES (N'68fd12c2-5923-4d74-b569-07d8012e836e', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-01-18T15:55:21.293' AS DateTime), 1)
INSERT [dbo].[ProjectMember] ([ProjectId], [UserId], [JoinAt], [IsOwner]) VALUES (N'68fd12c2-5923-4d74-b569-07d8012e836e', N'6b2a36d3-1033-4d8c-96b4-6d62e1e74e28', CAST(N'2023-01-30T12:18:58.727' AS DateTime), 0)
INSERT [dbo].[ProjectMember] ([ProjectId], [UserId], [JoinAt], [IsOwner]) VALUES (N'68fd12c2-5923-4d74-b569-07d8012e836e', N'7f9a3e10-cc09-4bcc-838e-e8c08e8bfa8b', CAST(N'2023-01-31T12:48:24.523' AS DateTime), 0)
INSERT [dbo].[ProjectMember] ([ProjectId], [UserId], [JoinAt], [IsOwner]) VALUES (N'f995eab3-30af-4678-8933-4982c381ebbb', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-01-18T15:39:27.397' AS DateTime), 1)
INSERT [dbo].[ProjectMember] ([ProjectId], [UserId], [JoinAt], [IsOwner]) VALUES (N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-01-28T13:29:59.283' AS DateTime), 1)
INSERT [dbo].[ProjectMember] ([ProjectId], [UserId], [JoinAt], [IsOwner]) VALUES (N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', N'6b2a36d3-1033-4d8c-96b4-6d62e1e74e28', CAST(N'2023-01-30T12:20:43.683' AS DateTime), 0)
INSERT [dbo].[ProjectMember] ([ProjectId], [UserId], [JoinAt], [IsOwner]) VALUES (N'bd6eb455-b335-4bae-97ae-cc4455d1a2c5', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-01-17T00:00:00.000' AS DateTime), 1)
INSERT [dbo].[ProjectMember] ([ProjectId], [UserId], [JoinAt], [IsOwner]) VALUES (N'bd6eb455-b335-4bae-97ae-cc4455d1a2c5', N'7f9a3e10-cc09-4bcc-838e-e8c08e8bfa8b', CAST(N'2023-01-31T19:14:41.663' AS DateTime), 0)
INSERT [dbo].[ProjectMember] ([ProjectId], [UserId], [JoinAt], [IsOwner]) VALUES (N'1372d8c7-0ad1-4181-8ce0-eee5a4fdd9c7', N'125ef4d8-90e1-485b-837d-0640ee9f107d', CAST(N'2023-01-19T12:12:27.613' AS DateTime), 1)
INSERT [dbo].[ProjectMember] ([ProjectId], [UserId], [JoinAt], [IsOwner]) VALUES (N'1372d8c7-0ad1-4181-8ce0-eee5a4fdd9c7', N'6b2a36d3-1033-4d8c-96b4-6d62e1e74e28', CAST(N'2023-01-30T11:48:41.830' AS DateTime), 0)
GO
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'68fd12c2-5923-4d74-b569-07d8012e836e', N'eb3add8a-31d9-4f56-a3e8-06f2997c2519', N'High')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'68fd12c2-5923-4d74-b569-07d8012e836e', N'5bbc2686-e824-4a41-91f6-0b0ad8146ec3', N'Highest')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'68fd12c2-5923-4d74-b569-07d8012e836e', N'934db542-323b-4c26-9ff3-2edde11c8995', N'Lowest')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'68fd12c2-5923-4d74-b569-07d8012e836e', N'66c8019b-bc05-46d6-baee-c09bf0390546', N'Low')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'68fd12c2-5923-4d74-b569-07d8012e836e', N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'Medium')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'f995eab3-30af-4678-8933-4982c381ebbb', N'eb3add8a-31d9-4f56-a3e8-06f2997c2519', N'High')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'f995eab3-30af-4678-8933-4982c381ebbb', N'5bbc2686-e824-4a41-91f6-0b0ad8146ec3', N'Highest')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'f995eab3-30af-4678-8933-4982c381ebbb', N'934db542-323b-4c26-9ff3-2edde11c8995', N'Lowest')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'f995eab3-30af-4678-8933-4982c381ebbb', N'66c8019b-bc05-46d6-baee-c09bf0390546', N'Low')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'f995eab3-30af-4678-8933-4982c381ebbb', N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'Medium')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', N'eb3add8a-31d9-4f56-a3e8-06f2997c2519', N'High')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', N'5bbc2686-e824-4a41-91f6-0b0ad8146ec3', N'Highest')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', N'934db542-323b-4c26-9ff3-2edde11c8995', N'Lowest')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', N'66c8019b-bc05-46d6-baee-c09bf0390546', N'Low')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'Medium')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'bd6eb455-b335-4bae-97ae-cc4455d1a2c5', N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'Medium')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'1372d8c7-0ad1-4181-8ce0-eee5a4fdd9c7', N'eb3add8a-31d9-4f56-a3e8-06f2997c2519', N'High')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'1372d8c7-0ad1-4181-8ce0-eee5a4fdd9c7', N'5bbc2686-e824-4a41-91f6-0b0ad8146ec3', N'Highest')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'1372d8c7-0ad1-4181-8ce0-eee5a4fdd9c7', N'934db542-323b-4c26-9ff3-2edde11c8995', N'Lowest')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'1372d8c7-0ad1-4181-8ce0-eee5a4fdd9c7', N'66c8019b-bc05-46d6-baee-c09bf0390546', N'Low')
INSERT [dbo].[ProjectPriority] ([ProjectId], [PriorityId], [Description]) VALUES (N'1372d8c7-0ad1-4181-8ce0-eee5a4fdd9c7', N'd3f945eb-ddd8-4da5-aa88-fa65bd722497', N'Medium')
GO
INSERT [dbo].[Role] ([Id], [Name], [Description]) VALUES (N'57a48ba5-9d3a-4119-97b7-2fb73e8e6f04', N'Admin', N'Administrator')
INSERT [dbo].[Role] ([Id], [Name], [Description]) VALUES (N'4e7bd4af-3809-48a6-a580-6c244452099a', N'User', N'Normal User')
GO
INSERT [dbo].[Status] ([Id], [Name], [ProjectId], [Position], [IsFirst], [IsLast], [Limit], [Description]) VALUES (N'70cd0630-ef39-44f9-9ca2-094312dce617', N'ASD', N'1372d8c7-0ad1-4181-8ce0-eee5a4fdd9c7', 176640, 0, 0, NULL, NULL)
INSERT [dbo].[Status] ([Id], [Name], [ProjectId], [Position], [IsFirst], [IsLast], [Limit], [Description]) VALUES (N'999df6cb-abc8-44ff-8aa6-0aed7c4c0984', N'To Do', N'1372d8c7-0ad1-4181-8ce0-eee5a4fdd9c7', 165888, 0, 0, NULL, NULL)
INSERT [dbo].[Status] ([Id], [Name], [ProjectId], [Position], [IsFirst], [IsLast], [Limit], [Description]) VALUES (N'bdc6281c-e51e-460e-b3e6-1a0cc6162700', N'Sắp xong', N'bd6eb455-b335-4bae-97ae-cc4455d1a2c5', 352024, 0, 0, NULL, NULL)
INSERT [dbo].[Status] ([Id], [Name], [ProjectId], [Position], [IsFirst], [IsLast], [Limit], [Description]) VALUES (N'b02dcfab-e1b3-4442-99d8-22129088020f', N'AS', N'1372d8c7-0ad1-4181-8ce0-eee5a4fdd9c7', 82944, 0, 0, NULL, NULL)
INSERT [dbo].[Status] ([Id], [Name], [ProjectId], [Position], [IsFirst], [IsLast], [Limit], [Description]) VALUES (N'b72d9534-361b-447b-a0f3-2fd2157e4767', N'Processing', N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', 263168, 0, 1, NULL, N'Processing task')
INSERT [dbo].[Status] ([Id], [Name], [ProjectId], [Position], [IsFirst], [IsLast], [Limit], [Description]) VALUES (N'feebdf62-6bef-47bf-82af-322dc3684ee8', N'Done', N'68fd12c2-5923-4d74-b569-07d8012e836e', 720896, 0, 1, NULL, NULL)
INSERT [dbo].[Status] ([Id], [Name], [ProjectId], [Position], [IsFirst], [IsLast], [Limit], [Description]) VALUES (N'ac73a46d-d2d4-4077-9963-5bce964febe5', N's', N'68fd12c2-5923-4d74-b569-07d8012e836e', 786432, 0, 0, NULL, NULL)
INSERT [dbo].[Status] ([Id], [Name], [ProjectId], [Position], [IsFirst], [IsLast], [Limit], [Description]) VALUES (N'9e257b1d-c9e2-4fb7-b511-6195b4d6cf31', N'To Do', N'f995eab3-30af-4678-8933-4982c381ebbb', 65536, 1, 0, NULL, N'To do task')
INSERT [dbo].[Status] ([Id], [Name], [ProjectId], [Position], [IsFirst], [IsLast], [Limit], [Description]) VALUES (N'a9824b05-b563-4259-b9a5-67b41cbf865d', N'To Do', N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', 184832, 1, 0, NULL, N'To do task')
INSERT [dbo].[Status] ([Id], [Name], [ProjectId], [Position], [IsFirst], [IsLast], [Limit], [Description]) VALUES (N'ba8637dc-ed96-449b-b21e-6ff387fbe8d4', N'Processing', N'f995eab3-30af-4678-8933-4982c381ebbb', 32768, 0, 0, NULL, N'Processing task')
INSERT [dbo].[Status] ([Id], [Name], [ProjectId], [Position], [IsFirst], [IsLast], [Limit], [Description]) VALUES (N'b593209a-9950-4f81-9b52-9663c41dae7e', N'Done', N'1372d8c7-0ad1-4181-8ce0-eee5a4fdd9c7', 180224, 1, 1, NULL, N'To do task')
INSERT [dbo].[Status] ([Id], [Name], [ProjectId], [Position], [IsFirst], [IsLast], [Limit], [Description]) VALUES (N'70f8a21f-1777-41b0-b4ec-a0379f5097e1', N'To Do', N'68fd12c2-5923-4d74-b569-07d8012e836e', 65536, 1, 0, NULL, N'To do task')
INSERT [dbo].[Status] ([Id], [Name], [ProjectId], [Position], [IsFirst], [IsLast], [Limit], [Description]) VALUES (N'8abac7be-ea73-4476-8403-c6927537da67', N'Done', N'f995eab3-30af-4678-8933-4982c381ebbb', 196608, 0, 1, NULL, N'Done task')
INSERT [dbo].[Status] ([Id], [Name], [ProjectId], [Position], [IsFirst], [IsLast], [Limit], [Description]) VALUES (N'8c83ab27-7e44-48b4-9d51-d2ff3481eb9a', N'Haha', N'bd6eb455-b335-4bae-97ae-cc4455d1a2c5', 319256, 0, 0, 3, NULL)
INSERT [dbo].[Status] ([Id], [Name], [ProjectId], [Position], [IsFirst], [IsLast], [Limit], [Description]) VALUES (N'b0d43e6b-6796-41bd-858d-db0883331b43', N'asd', N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', 328704, 0, 0, NULL, NULL)
INSERT [dbo].[Status] ([Id], [Name], [ProjectId], [Position], [IsFirst], [IsLast], [Limit], [Description]) VALUES (N'3fcbac7a-8e0f-48b9-b4b8-db86a72da674', N'Đang làm', N'bd6eb455-b335-4bae-97ae-cc4455d1a2c5', 286488, 0, 0, NULL, NULL)
INSERT [dbo].[Status] ([Id], [Name], [ProjectId], [Position], [IsFirst], [IsLast], [Limit], [Description]) VALUES (N'bac84ec5-0baf-476d-be52-e0f3d8997a59', N'asdasd', N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', 394240, 0, 0, NULL, NULL)
GO
INSERT [dbo].[Type] ([Id], [Name], [ProjectId], [Description]) VALUES (N'8d1d06e1-ca48-4657-a95c-103b2f6bb0ad', N'SubTask', N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', N'Sub Task')
INSERT [dbo].[Type] ([Id], [Name], [ProjectId], [Description]) VALUES (N'bcd08550-0448-4f33-bc99-11918d6ef5a3', N'Feature', N'bd6eb455-b335-4bae-97ae-cc4455d1a2c5', N'Feature')
INSERT [dbo].[Type] ([Id], [Name], [ProjectId], [Description]) VALUES (N'8373f567-761b-40c3-9913-29d8c16e45bb', N'Task', N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', N'Task')
INSERT [dbo].[Type] ([Id], [Name], [ProjectId], [Description]) VALUES (N'3206709a-7542-4afc-bb3b-2ca24afbf686', N'Bug', N'68fd12c2-5923-4d74-b569-07d8012e836e', N'Bug')
INSERT [dbo].[Type] ([Id], [Name], [ProjectId], [Description]) VALUES (N'911a6176-8f70-4335-b1b9-2fb54ec160aa', N'Story', N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', N'Story')
INSERT [dbo].[Type] ([Id], [Name], [ProjectId], [Description]) VALUES (N'0400936c-433c-4488-b11c-3737d2b2391b', N'Bug', N'1372d8c7-0ad1-4181-8ce0-eee5a4fdd9c7', N'Bug')
INSERT [dbo].[Type] ([Id], [Name], [ProjectId], [Description]) VALUES (N'2009e244-7356-43b5-9b46-3d92ddbddb56', N'Task', N'1372d8c7-0ad1-4181-8ce0-eee5a4fdd9c7', N'Task')
INSERT [dbo].[Type] ([Id], [Name], [ProjectId], [Description]) VALUES (N'137d9b80-fb82-4191-b04b-4333ad04a032', N'Bug', N'f995eab3-30af-4678-8933-4982c381ebbb', N'Bug')
INSERT [dbo].[Type] ([Id], [Name], [ProjectId], [Description]) VALUES (N'0d04c5bf-3281-4633-a529-48b788e2dfc7', N'Task', N'68fd12c2-5923-4d74-b569-07d8012e836e', N'Task')
INSERT [dbo].[Type] ([Id], [Name], [ProjectId], [Description]) VALUES (N'a8ea4d79-e60a-4e6f-a531-7fa8428929ed', N'Story', N'68fd12c2-5923-4d74-b569-07d8012e836e', N'Story')
INSERT [dbo].[Type] ([Id], [Name], [ProjectId], [Description]) VALUES (N'd6429a5d-c6f5-4867-a377-8c3fd1c8498d', N'Bug', N'dbaa4e4a-f52d-46ba-9fb3-51dffa6bbbed', N'Bug')
INSERT [dbo].[Type] ([Id], [Name], [ProjectId], [Description]) VALUES (N'b91d1d37-d276-4a59-9585-8e8f4f375832', N'Story', N'f995eab3-30af-4678-8933-4982c381ebbb', N'Story')
INSERT [dbo].[Type] ([Id], [Name], [ProjectId], [Description]) VALUES (N'd5f5d1f2-7f49-4e16-9c3a-a89956d818da', N'Task', N'f995eab3-30af-4678-8933-4982c381ebbb', N'Task')
INSERT [dbo].[Type] ([Id], [Name], [ProjectId], [Description]) VALUES (N'049ccc21-9547-4963-8b07-f7b1b47b6fa6', N'Story', N'1372d8c7-0ad1-4181-8ce0-eee5a4fdd9c7', N'Story')
GO
INSERT [dbo].[User] ([Id], [Name], [Email], [Username], [Password], [Status]) VALUES (N'125ef4d8-90e1-485b-837d-0640ee9f107d', N'Ngoc Phieu', N'admin@gmail.com', N'admin', N'admin', 1)
INSERT [dbo].[User] ([Id], [Name], [Email], [Username], [Password], [Status]) VALUES (N'6b2a36d3-1033-4d8c-96b4-6d62e1e74e28', N'Trong Hoang', N'tronghoang@gmail.com', N'tronghoang', N'tronghoang', 1)
INSERT [dbo].[User] ([Id], [Name], [Email], [Username], [Password], [Status]) VALUES (N'7f9a3e10-cc09-4bcc-838e-e8c08e8bfa8b', N'Cam Giang', N'camgiang@gmail.com', N'camgiang', N'camgiang', 1)
GO
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (N'125ef4d8-90e1-485b-837d-0640ee9f107d', N'57a48ba5-9d3a-4119-97b7-2fb73e8e6f04')
INSERT [dbo].[UserRole] ([UserId], [RoleId]) VALUES (N'6b2a36d3-1033-4d8c-96b4-6d62e1e74e28', N'4e7bd4af-3809-48a6-a580-6c244452099a')
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__User__536C85E405C69F5C]    Script Date: 2/1/2023 6:12:30 PM ******/
ALTER TABLE [dbo].[User] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__User__A9D10534A1736D38]    Script Date: 2/1/2023 6:12:30 PM ******/
ALTER TABLE [dbo].[User] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Comment] ADD  DEFAULT (getdate()) FOR [CreateAt]
GO
ALTER TABLE [dbo].[Issue] ADD  DEFAULT ((0)) FOR [IsChild]
GO
ALTER TABLE [dbo].[Issue] ADD  DEFAULT ((0)) FOR [EstimateTime]
GO
ALTER TABLE [dbo].[Issue] ADD  DEFAULT (getdate()) FOR [CreateAt]
GO
ALTER TABLE [dbo].[Issue] ADD  DEFAULT ((0)) FOR [IsClose]
GO
ALTER TABLE [dbo].[LogWork] ADD  DEFAULT (getdate()) FOR [CreateAt]
GO
ALTER TABLE [dbo].[Project] ADD  DEFAULT (getdate()) FOR [CreateAt]
GO
ALTER TABLE [dbo].[Project] ADD  DEFAULT ((0)) FOR [IsClose]
GO
ALTER TABLE [dbo].[Status] ADD  DEFAULT ((0)) FOR [IsFirst]
GO
ALTER TABLE [dbo].[Status] ADD  DEFAULT ((0)) FOR [IsLast]
GO
ALTER TABLE [dbo].[Attachment]  WITH CHECK ADD FOREIGN KEY([IssueId])
REFERENCES [dbo].[Issue] ([Id])
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD FOREIGN KEY([IssueId])
REFERENCES [dbo].[Issue] ([Id])
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Issue]  WITH CHECK ADD FOREIGN KEY([AssigneeId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Issue]  WITH CHECK ADD FOREIGN KEY([ParentId])
REFERENCES [dbo].[Issue] ([Id])
GO
ALTER TABLE [dbo].[Issue]  WITH CHECK ADD FOREIGN KEY([PriorityId])
REFERENCES [dbo].[Priority] ([Id])
GO
ALTER TABLE [dbo].[Issue]  WITH CHECK ADD FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[Issue]  WITH CHECK ADD FOREIGN KEY([ReporterId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Issue]  WITH CHECK ADD FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([Id])
GO
ALTER TABLE [dbo].[Issue]  WITH CHECK ADD FOREIGN KEY([TypeId])
REFERENCES [dbo].[Type] ([Id])
GO
ALTER TABLE [dbo].[IssueLabel]  WITH CHECK ADD FOREIGN KEY([IssueId])
REFERENCES [dbo].[Issue] ([Id])
GO
ALTER TABLE [dbo].[IssueLabel]  WITH CHECK ADD FOREIGN KEY([LabelId])
REFERENCES [dbo].[Label] ([Id])
GO
ALTER TABLE [dbo].[Label]  WITH CHECK ADD FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[Link]  WITH CHECK ADD FOREIGN KEY([IssueId])
REFERENCES [dbo].[Issue] ([Id])
GO
ALTER TABLE [dbo].[LogWork]  WITH CHECK ADD FOREIGN KEY([IssueId])
REFERENCES [dbo].[Issue] ([Id])
GO
ALTER TABLE [dbo].[LogWork]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD FOREIGN KEY([DefaultAssigneeId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD FOREIGN KEY([LeaderId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[ProjectMember]  WITH CHECK ADD FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[ProjectMember]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[ProjectPriority]  WITH CHECK ADD FOREIGN KEY([PriorityId])
REFERENCES [dbo].[Priority] ([Id])
GO
ALTER TABLE [dbo].[ProjectPriority]  WITH CHECK ADD FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[Status]  WITH CHECK ADD FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[Type]  WITH CHECK ADD FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
USE [master]
GO
ALTER DATABASE [Kanban] SET  READ_WRITE 
GO
