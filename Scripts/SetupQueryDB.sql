-------------------------------------------------------------------------
-- Tables of Forum.
-------------------------------------------------------------------------

CREATE TABLE [tb_Section](
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [varchar](128) NOT NULL,
 CONSTRAINT [PK_tb_Section] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [tb_Account](
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [varchar](128) NOT NULL,
    [Password] [varchar](128) NOT NULL,
 CONSTRAINT [PK_tb_Account] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [tb_Post](
    [Id] [uniqueidentifier] NOT NULL,
    [Subject] [varchar](256) NOT NULL,
    [Body] [ntext] NOT NULL,
    [AuthorId] [uniqueidentifier] NOT NULL,
    [AuthorName] [varchar](128) NOT NULL,
    [SectionId] [uniqueidentifier] NOT NULL,
    [ReplyCount] [int] NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [MostRecentReplierId] [uniqueidentifier] NULL,
    [MostRecentReplierName] [varchar](128) NULL,
    [MostRecentReplyCreatedOn] [datetime] NULL,
 CONSTRAINT [PK_tb_Post] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [tb_Reply](
    [Id] [uniqueidentifier] NOT NULL,
    [ParentId] [uniqueidentifier] NULL,
    [PostId] [uniqueidentifier] NOT NULL,
    [Body] [ntext] NOT NULL,
    [AuthorId] [uniqueidentifier] NOT NULL,
    [AuthorName] [varchar](128) NOT NULL,
    [FloorIndex] [int] NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_tb_Reply] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO