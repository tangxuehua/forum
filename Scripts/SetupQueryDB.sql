-------------------------------------------------------------------------
-- Tables of Forum.
-------------------------------------------------------------------------

CREATE TABLE [tb_Account](
    [Id] [nvarchar](32) NOT NULL,
    [Name] [nvarchar](128) NOT NULL,
    [Password] [nvarchar](128) NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [UpdatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_tb_Account] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [tb_Section](
    [Id] [nvarchar](32) NOT NULL,
    [Name] [nvarchar](128) NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [UpdatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_tb_Section] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [tb_Post](
    [Id] [nvarchar](32) NOT NULL,
    [Subject] [nvarchar](256) NOT NULL,
    [Body] [ntext] NOT NULL,
    [AuthorId] [nvarchar](32) NOT NULL,
    [SectionId] [nvarchar](32) NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [UpdatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_tb_Post] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [tb_Reply](
    [Id] [nvarchar](32) NOT NULL,
    [PostId] [nvarchar](32) NOT NULL,
    [ParentId] [nvarchar](32) NULL,
    [AuthorId] [nvarchar](32) NOT NULL,
    [Body] [ntext] NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [UpdatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_tb_Reply] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO