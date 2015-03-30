----------------------------------------------------------------------------------------------
--Tables used by Forum.
----------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[AccountIndex] (
    [IndexId]     NVARCHAR (32) NOT NULL,
    [AccountId]   NVARCHAR (32) NOT NULL,
    [AccountName] NVARCHAR (64) NOT NULL,
    CONSTRAINT [PK_AccountIndex] PRIMARY KEY CLUSTERED ([IndexId] ASC)
)
GO
CREATE UNIQUE INDEX [IX_AccountIndex_AccountName] ON [dbo].[AccountIndex] ([AccountName])

CREATE TABLE [dbo].[Account](
    [Id] [nvarchar](32) NOT NULL,
    [Sequence] [bigint] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](128) NOT NULL,
    [Password] [nvarchar](128) NULL,
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
    [MostRecentReplyId] [nvarchar](32) NULL,
    [MostRecentReplierId] [nvarchar](32) NULL,
    [ReplyCount] [bigint] NOT NULL,
    [LastUpdateTime] [datetime] NOT NULL,
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

CREATE TABLE [dbo].[Command] (
    [Sequence]                BIGINT IDENTITY (1, 1) NOT NULL,
    [CommandId]               NVARCHAR (128)         NOT NULL,
    [CommandTypeCode]         INT                    NOT NULL,
    [Timestamp]               DATETIME               NOT NULL,
    [Payload]                 NVARCHAR (MAX)         NOT NULL,
    [AggregateRootTypeCode]   INT                    NOT NULL,
    [AggregateRootId]         NVARCHAR (36)          NULL,
    [Message]                 NVARCHAR (MAX)         NULL,
    [MessageTypeCode]         INT                    NOT NULL,
    CONSTRAINT [PK_Command] PRIMARY KEY CLUSTERED ([CommandId] ASC)
)
GO
CREATE TABLE [dbo].[EventStream] (
    [Sequence]                BIGINT IDENTITY (1, 1) NOT NULL,
    [AggregateRootTypeCode]   INT                    NOT NULL,
    [AggregateRootId]         NVARCHAR (36)          NOT NULL,
    [Version]                 INT                    NOT NULL,
    [CommandId]               NVARCHAR (128)         NOT NULL,
    [Timestamp]               DATETIME               NOT NULL,
    [Events]                  NVARCHAR (MAX)         NOT NULL,
    CONSTRAINT [PK_EventStream] PRIMARY KEY CLUSTERED ([AggregateRootId] ASC, [Version] ASC)
)
GO
CREATE TABLE [dbo].[SequenceMessagePublishedVersion] (
    [Sequence]                BIGINT IDENTITY (1, 1) NOT NULL,
    [ProcessorName]           NVARCHAR (128)         NOT NULL,
    [AggregateRootTypeCode]   INT                    NOT NULL,
    [AggregateRootId]         NVARCHAR (36)          NOT NULL,
    [PublishedVersion]        INT                    NOT NULL,
    [Timestamp]               DATETIME               NOT NULL,
    CONSTRAINT [PK_SequenceMessagePublishedVersion] PRIMARY KEY CLUSTERED ([ProcessorName] ASC, [AggregateRootId] ASC, [PublishedVersion] ASC)
)
GO
CREATE TABLE [dbo].[MessageHandleRecord] (
    [Sequence]                  BIGINT IDENTITY (1, 1) NOT NULL,
    [MessageId]                 NVARCHAR (36)          NOT NULL,
    [HandlerTypeCode]           INT                    NOT NULL,
    [MessageTypeCode]           INT                    NOT NULL,
    [AggregateRootTypeCode]     INT                    NOT NULL,
    [AggregateRootId]           NVARCHAR (36)          NULL,
    [Version]                   INT                    NULL,
    [Timestamp]                 DATETIME               NOT NULL,
    CONSTRAINT [PK_MessageHandleRecord] PRIMARY KEY CLUSTERED ([MessageId] ASC, [HandlerTypeCode] ASC)
)
GO
CREATE TABLE [dbo].[Snapshot] (
    [Sequence]               BIGINT IDENTITY (1, 1)  NOT NULL,
    [AggregateRootTypeCode]  INT                     NOT NULL,
    [AggregateRootId]        NVARCHAR (36)           NOT NULL,
    [Version]                INT                     NOT NULL,
    [Payload]                VARBINARY (MAX)         NOT NULL,
    [Timestamp]              DATETIME                NOT NULL,
    CONSTRAINT [PK_Snapshot] PRIMARY KEY CLUSTERED ([AggregateRootId] ASC, [Version] ASC)
)
GO
CREATE TABLE [dbo].[Lock] (
    [LockKey]                NVARCHAR (128)          NOT NULL,
    CONSTRAINT [PK_Lock] PRIMARY KEY CLUSTERED ([LockKey] ASC)
)
GO

----------------------------------------------------------------------------------------------
--Tables used by EQueue.
----------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[Message](
    [MessageOffset] [bigint] NOT NULL,
    [Topic] [varchar](128) NOT NULL,
    [QueueId] [int] NOT NULL,
    [QueueOffset] [bigint] NOT NULL,
    [Code] [int] NOT NULL,
    [Body] [varbinary](max) NOT NULL,
    [StoredTime] [datetime] NOT NULL,
    [RoutingKey] [varchar](128) NOT NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
    [MessageOffset] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Message_QueueIndex] ON [dbo].[Message]([Topic] ASC, [QueueId] ASC, [QueueOffset] ASC)
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