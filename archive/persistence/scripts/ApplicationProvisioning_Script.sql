/****** Object:  Table [dbo].[Tag]    Script Date: 03/19/2008 09:40:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Tag](
	[Id] [uniqueidentifier] NOT NULL,
	[Label] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Tag] UNIQUE NONCLUSTERED 
(
	[Label] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Task]    Script Date: 03/19/2008 09:40:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Task](
	[Id] [uniqueidentifier] NOT NULL,
	[Subject] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](400) NULL,
	[DueDate] [datetime] NULL,
	[Priority] [int] NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Task_Tags]    Script Date: 03/19/2008 09:40:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [TaskTags](
	[TaskId] [uniqueidentifier] NOT NULL,
	[TagId] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_Task_Tags_Tag]    Script Date: 03/19/2008 09:40:37 ******/
ALTER TABLE [TaskTags]  WITH CHECK ADD  CONSTRAINT [FK_Task_Tags_Tag] FOREIGN KEY([TagId])
REFERENCES [Tag] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [TaskTags] CHECK CONSTRAINT [FK_Task_Tags_Tag]
GO
/****** Object:  ForeignKey [FK_Task_Tags_Task]    Script Date: 03/19/2008 09:40:37 ******/
ALTER TABLE [TaskTags]  WITH CHECK ADD  CONSTRAINT [FK_Task_Tags_Task] FOREIGN KEY([TaskId])
REFERENCES [Task] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [TaskTags] CHECK CONSTRAINT [FK_Task_Tags_Task]
GO
