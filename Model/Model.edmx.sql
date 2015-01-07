
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/30/2014 13:15:09
-- Generated from EDMX file: E:\学习资料\WeiXin\weixin\Model\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [WeiXin];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Message]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Message];
GO
IF OBJECT_ID(N'[dbo].[CnBlog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CnBlog];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Message'
CREATE TABLE [dbo].[Message] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ToUserName] nvarchar(max)  NOT NULL,
    [FromUserName] nvarchar(max)  NOT NULL,
    [MsgType] nvarchar(max)  NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [MsgId] nvarchar(max)  NULL,
    [CreateTime] nvarchar(max)  NOT NULL,
    [Type] nvarchar(max)  NULL
);
GO

-- Creating table 'CnBlog'
CREATE TABLE [dbo].[CnBlog] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ArticleId] nvarchar(max)  NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Summary] nvarchar(max)  NOT NULL,
    [Published] nvarchar(max)  NOT NULL,
    [Updated] nvarchar(max)  NOT NULL,
    [Link] nvarchar(max)  NOT NULL,
    [StaticLink] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Message'
ALTER TABLE [dbo].[Message]
ADD CONSTRAINT [PK_Message]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CnBlog'
ALTER TABLE [dbo].[CnBlog]
ADD CONSTRAINT [PK_CnBlog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------