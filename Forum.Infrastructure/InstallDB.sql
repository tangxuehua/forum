-------------------------------------------------------------------------
-- Tables of Forum.
-------------------------------------------------------------------------

CREATE TABLE [Registration](
    [AccountName] [nvarchar](64) NOT NULL,
    [AccountId] [nvarchar](32) NOT NULL,
    [Status] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_Registration] PRIMARY KEY CLUSTERED
(
    [AccountName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [Account](
    [Id] [nvarchar](32) NOT NULL,
    [Name] [nvarchar](128) NOT NULL,
    [Password] [nvarchar](128) NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [UpdatedOn] [datetime] NOT NULL,
    [Version]   [BIGINT]  NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [Section](
    [Id] [nvarchar](32) NOT NULL,
    [Name] [nvarchar](128) NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [UpdatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Section] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [Post](
    [Id] [nvarchar](32) NOT NULL,
    [Subject] [nvarchar](256) NOT NULL,
    [Body] [ntext] NOT NULL,
    [AuthorId] [nvarchar](32) NOT NULL,
    [SectionId] [nvarchar](32) NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [UpdatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [Reply](
    [Id] [nvarchar](32) NOT NULL,
    [PostId] [nvarchar](32) NOT NULL,
    [ParentId] [nvarchar](32) NULL,
    [AuthorId] [nvarchar](32) NOT NULL,
    [Body] [ntext] NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [UpdatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Reply] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Tables for enode framework.
CREATE TABLE [dbo].[Event] (
    [Sequence]                BIGINT          IDENTITY (1, 1) NOT NULL,
    [AggregateRootTypeCode]   INT  NOT NULL,
    [AggregateRootId]         NVARCHAR (36)   NOT NULL,
    [Version]                 INT             NOT NULL,
    [CommitId]                NVARCHAR (36)   NOT NULL,
    [Timestamp]               DATETIME        NOT NULL,
    [Events]                  VARBINARY (MAX) NOT NULL,
    CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED ([Sequence] ASC)
)
GO
CREATE UNIQUE INDEX [IX_Event_VersionIndex] ON [dbo].[Event] ([AggregateRootId], [Version])
GO
CREATE UNIQUE INDEX [IX_Event_CommitIndex]  ON [dbo].[Event] ([AggregateRootId], [CommitId])
GO

CREATE TABLE [dbo].[EventPublishInfo] (
    [AggregateRootId]  NVARCHAR (36) NOT NULL,
    [PublishedVersion] INT           NOT NULL,
    CONSTRAINT [PK_EventPublishInfo] PRIMARY KEY CLUSTERED ([AggregateRootId] ASC)
)
GO
CREATE TABLE [dbo].[EventHandleInfo] (
    [EventId]              NVARCHAR (36)  NOT NULL,
    [EventHandlerTypeCode] INT NOT NULL,
    CONSTRAINT [PK_EventHandleInfo] PRIMARY KEY CLUSTERED ([EventId] ASC, [EventHandlerTypeCode] ASC)
)
GO