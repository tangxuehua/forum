----------------------------------------------------------------------------------------------
--Tables used by Forum.
----------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[Registration](
    [AccountName] [nvarchar](64) NOT NULL,
    [AccountId] [nvarchar](32) NOT NULL,
    [Sequence] [bigint] IDENTITY(1,1) NOT NULL,
    [Status] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_Registration] PRIMARY KEY CLUSTERED 
(
    [AccountName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Account](
    [Id] [nvarchar](32) NOT NULL,
    [Sequence] [bigint] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](128) NOT NULL,
    [Password] [nvarchar](128) NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [UpdatedOn] [datetime] NOT NULL,
    [Version] [bigint] NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Section](
    [Id] [nvarchar](32) NOT NULL,
    [Sequence] [bigint] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](128) NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [UpdatedOn] [datetime] NOT NULL,
    [Version] [bigint] NOT NULL,
 CONSTRAINT [PK_Section] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Post](
    [Id] [nvarchar](32) NOT NULL,
    [Sequence] [bigint] IDENTITY(1,1) NOT NULL,
    [Subject] [nvarchar](256) NOT NULL,
    [Body] [ntext] NOT NULL,
    [AuthorId] [nvarchar](32) NOT NULL,
    [SectionId] [nvarchar](32) NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [UpdatedOn] [datetime] NOT NULL,
    [Version] [bigint] NOT NULL,
 CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Reply](
    [Id] [nvarchar](32) NOT NULL,
    [Sequence] [bigint] IDENTITY(1,1) NOT NULL,
    [PostId] [nvarchar](32) NOT NULL,
    [ParentId] [nvarchar](32) NULL,
    [AuthorId] [nvarchar](32) NOT NULL,
    [Body] [ntext] NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [UpdatedOn] [datetime] NOT NULL,
    [Version] [bigint] NOT NULL,
 CONSTRAINT [PK_Reply] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

----------------------------------------------------------------------------------------------
--Tables used by ENode.
----------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[Event](
    [Sequence] [bigint] IDENTITY(1,1) NOT NULL,
    [AggregateRootTypeCode] [int] NOT NULL,
    [AggregateRootId] [nvarchar](36) NOT NULL,
    [Version] [int] NOT NULL,
    [CommitId] [nvarchar](36) NOT NULL,
    [Timestamp] [datetime] NOT NULL,
    [Events] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
    [Sequence] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE INDEX [IX_Event_VersionIndex] ON [dbo].[Event] ([AggregateRootId], [Version])
GO
CREATE UNIQUE INDEX [IX_Event_CommitIndex]  ON [dbo].[Event] ([AggregateRootId], [CommitId])
GO

CREATE TABLE [dbo].[EventPublishInfo](
    [AggregateRootId] [nvarchar](36) NOT NULL,
    [PublishedVersion] [int] NOT NULL,
 CONSTRAINT [PK_EventPublishInfo] PRIMARY KEY CLUSTERED 
(
    [AggregateRootId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[EventHandleInfo](
    [EventId] [nvarchar](36) NOT NULL,
    [EventHandlerTypeCode] [int] NOT NULL,
 CONSTRAINT [PK_EventHandleInfo] PRIMARY KEY CLUSTERED 
(
    [EventId] ASC,
    [EventHandlerTypeCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

----------------------------------------------------------------------------------------------
--Tables used by EQueue.
----------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[Message](
    [MessageOffset] [bigint] NOT NULL,
    [Topic] [varchar](128) NOT NULL,
    [QueueId] [int] NOT NULL,
    [QueueOffset] [bigint] NOT NULL,
    [Body] [varbinary](max) NOT NULL,
    [StoredTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
    [MessageOffset] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[QueueOffset](
    [Version] [bigint] NOT NULL,
    [ConsumerGroup] [nvarchar](128) NOT NULL,
    [Topic] [nvarchar](128) NOT NULL,
    [QueueId] [int] NOT NULL,
    [QueueOffset] [bigint] NOT NULL,
    [Timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_QueueOffset] PRIMARY KEY CLUSTERED 
(
    [ConsumerGroup] ASC,
    [Topic] ASC,
    [QueueId] ASC,
    [Version] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO