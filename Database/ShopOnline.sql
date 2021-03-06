USE [master]
GO
/****** Object:  Database [ShopOnline]    Script Date: 12/28/2021 10:13:06 AM ******/
CREATE DATABASE [ShopOnline]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DbShopOnline', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\DbShopOnline.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'DbShopOnline_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\DbShopOnline_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ShopOnline] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ShopOnline].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ShopOnline] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ShopOnline] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ShopOnline] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ShopOnline] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ShopOnline] SET ARITHABORT OFF 
GO
ALTER DATABASE [ShopOnline] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ShopOnline] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ShopOnline] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ShopOnline] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ShopOnline] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ShopOnline] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ShopOnline] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ShopOnline] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ShopOnline] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ShopOnline] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ShopOnline] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ShopOnline] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ShopOnline] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ShopOnline] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ShopOnline] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ShopOnline] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ShopOnline] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ShopOnline] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ShopOnline] SET  MULTI_USER 
GO
ALTER DATABASE [ShopOnline] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ShopOnline] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ShopOnline] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ShopOnline] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [ShopOnline] SET DELAYED_DURABILITY = DISABLED 
GO
USE [ShopOnline]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 12/28/2021 10:13:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[AccountID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[FullName] [nvarchar](50) NULL,
	[Address] [nvarchar](max) NULL,
	[Phone] [nvarchar](50) NULL,
	[Birthday] [datetime] NULL,
	[Role] [nvarchar](50) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Accounts_Active]  DEFAULT ((1)),
	[LastLogin] [datetime] NULL,
	[CreateAt] [datetime] NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Categories]    Script Date: 12/28/2021 10:13:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CatID] [int] IDENTITY(1,1) NOT NULL,
	[CatName] [nvarchar](250) NULL,
	[Thumb] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[Alias] [nvarchar](250) NULL,
	[ParentID] [int] NULL,
	[Levels] [int] NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Categories_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CatID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CategoryProducts]    Script Date: 12/28/2021 10:13:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryProducts](
	[CatDetailID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[CatID] [int] NULL,
 CONSTRAINT [PK_CatDetails] PRIMARY KEY CLUSTERED 
(
	[CatDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderProducts]    Script Date: 12/28/2021 10:13:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OrderProducts](
	[OrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[ProductID] [int] NULL,
	[Size] [varchar](50) NULL,
	[Quantity] [int] NULL,
	[Total] [int] NULL,
 CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[OrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 12/28/2021 10:13:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NULL,
	[Amount] [nchar](10) NULL,
	[OrderDate] [datetime] NULL,
	[Status] [int] NULL,
	[Deleted] [bit] NULL,
	[Note] [nvarchar](max) NULL,
	[Address] [nvarchar](1000) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Posts]    Script Date: 12/28/2021 10:13:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Posts](
	[PostID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](500) NULL,
	[Context] [nvarchar](max) NULL,
	[Thumb] [nvarchar](250) NULL,
	[Alias] [nvarchar](250) NULL,
	[CreateAt] [datetime] NULL,
	[AccountID] [int] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED 
(
	[PostID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Products]    Script Date: 12/28/2021 10:13:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](250) NULL,
	[Price] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[Discount] [int] NULL CONSTRAINT [DF_Products_Discount]  DEFAULT ((0)),
	[Thumb] [nvarchar](250) NULL,
	[Brand] [nvarchar](250) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Products_Active]  DEFAULT ((1)),
	[BestSeller] [bit] NOT NULL CONSTRAINT [DF_Products_BestSeller]  DEFAULT ((0)),
	[HomeFlag] [bit] NOT NULL CONSTRAINT [DF_Products_HomeFlag]  DEFAULT ((0)),
	[Alias] [nvarchar](250) NULL,
	[Color] [nvarchar](50) NULL,
	[SizeS] [int] NULL,
	[SizeM] [int] NULL,
	[SizeL] [int] NULL,
	[SizeXL] [int] NULL,
	[Stock] [int] NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Accounts] ON 

INSERT [dbo].[Accounts] ([AccountID], [Email], [Password], [FullName], [Address], [Phone], [Birthday], [Role], [Active], [LastLogin], [CreateAt]) VALUES (1, N'admin@gmail.com', N'4297f44b13955235245b2497399d7a93', N'Trần Minh Chiến', N'An Gian', N'0977396251', CAST(N'2000-02-08 00:00:00.000' AS DateTime), N'Admin', 1, NULL, NULL)
INSERT [dbo].[Accounts] ([AccountID], [Email], [Password], [FullName], [Address], [Phone], [Birthday], [Role], [Active], [LastLogin], [CreateAt]) VALUES (2, N'Hieu@gmail.com', N'4297f44b13955235245b2497399d7a93', N'Nguyễn ngọc hiếu ', N'TP HCM', N'0989289221', CAST(N'2000-02-04 00:00:00.000' AS DateTime), N'Admin', 1, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Accounts] OFF
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([CatID], [CatName], [Thumb], [Description], [Alias], [ParentID], [Levels], [Active]) VALUES (1, N'Câu Lạc Bộ', NULL, NULL, N'cau-lac-bo', NULL, NULL, 1)
INSERT [dbo].[Categories] ([CatID], [CatName], [Thumb], [Description], [Alias], [ParentID], [Levels], [Active]) VALUES (4, N'Đội Tuyển Quóc Gia', NULL, NULL, N'doi-tuyen-quoc-gia', NULL, NULL, 1)
INSERT [dbo].[Categories] ([CatID], [CatName], [Thumb], [Description], [Alias], [ParentID], [Levels], [Active]) VALUES (5, N'Giải Ngoại Hạng Anh', NULL, NULL, N'giai-ngoai-hang-anh', NULL, NULL, 1)
INSERT [dbo].[Categories] ([CatID], [CatName], [Thumb], [Description], [Alias], [ParentID], [Levels], [Active]) VALUES (6, N'Giải Tây Ban Nha', NULL, NULL, N'giai-tay-ban-nha', NULL, NULL, 1)
INSERT [dbo].[Categories] ([CatID], [CatName], [Thumb], [Description], [Alias], [ParentID], [Levels], [Active]) VALUES (7, N'Giải Đức', NULL, NULL, N'giai-duc', NULL, NULL, 1)
INSERT [dbo].[Categories] ([CatID], [CatName], [Thumb], [Description], [Alias], [ParentID], [Levels], [Active]) VALUES (8, N'Giải Ý', NULL, NULL, N'giai-y', NULL, NULL, 1)
INSERT [dbo].[Categories] ([CatID], [CatName], [Thumb], [Description], [Alias], [ParentID], [Levels], [Active]) VALUES (9, N'Giải Pháp', NULL, NULL, N'giai-phap', NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Categories] OFF
SET IDENTITY_INSERT [dbo].[CategoryProducts] ON 

INSERT [dbo].[CategoryProducts] ([CatDetailID], [ProductID], [CatID]) VALUES (32, 5, 1)
INSERT [dbo].[CategoryProducts] ([CatDetailID], [ProductID], [CatID]) VALUES (33, 5, 5)
INSERT [dbo].[CategoryProducts] ([CatDetailID], [ProductID], [CatID]) VALUES (35, 3, 1)
INSERT [dbo].[CategoryProducts] ([CatDetailID], [ProductID], [CatID]) VALUES (36, 3, 5)
SET IDENTITY_INSERT [dbo].[CategoryProducts] OFF
SET IDENTITY_INSERT [dbo].[OrderProducts] ON 

INSERT [dbo].[OrderProducts] ([OrderDetailID], [OrderID], [ProductID], [Size], [Quantity], [Total]) VALUES (1, 1, 3, N'S', 1, 199000)
SET IDENTITY_INSERT [dbo].[OrderProducts] OFF
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([OrderID], [AccountID], [Amount], [OrderDate], [Status], [Deleted], [Note], [Address]) VALUES (1, 1, N'199000    ', CAST(N'2021-12-24 00:00:00.000' AS DateTime), 1, 0, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Orders] OFF
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ProductID], [ProductName], [Price], [Description], [Discount], [Thumb], [Brand], [Active], [BestSeller], [HomeFlag], [Alias], [Color], [SizeS], [SizeM], [SizeL], [SizeXL], [Stock]) VALUES (3, N'Áo đấu Manchester United', 1990000, N'<p><span style="color: rgb(51, 51, 51); font-family: Roboto; font-size: 15px; text-align: justify; background-color: rgb(250, 250, 250);">Trọn bộ&nbsp;</span><a href="https://aobongda.net/manchester-united" style="color: inherit; background-color: rgb(250, 250, 250); font-family: Roboto; font-size: 15px; text-align: justify;"><span style="color: rgb(0, 102, 255);"><span style="font-weight: bolder;">áo Chelsea 2021</span></span></a><span style="color: rgb(51, 51, 51); font-family: Roboto; font-size: 15px; text-align: justify; background-color: rgb(250, 250, 250);">&nbsp;sân nhà, sân khách chính thức với đầy đủ kích thước cho bạn lựa chọn. Hãy mặc áo đấu của bạn với niềm tự hào và thể hiện sự ủng hộ của bạn dành cho "Quỷ Đỏ" thành London</span><br style="color: rgb(51, 51, 51); font-family: Roboto; font-size: 15px; text-align: justify; background-color: rgb(250, 250, 250);"></p>', 0, N'ao-dau-chelsea.jpg', N'Adidas', 1, 0, 0, N'ao-dau-manchester-united', N'Đỏ', 100, 200, 100, 200, 600)
INSERT [dbo].[Products] ([ProductID], [ProductName], [Price], [Description], [Discount], [Thumb], [Brand], [Active], [BestSeller], [HomeFlag], [Alias], [Color], [SizeS], [SizeM], [SizeL], [SizeXL], [Stock]) VALUES (5, N'Áo thi đấu Arsernal', 2000000, N'<h1 class="pd-title font-size-large main-color-1" style="font-family: Roboto; line-height: 1.2; color: rgb(49, 39, 131); background-color: rgb(250, 250, 250);">Áo Bóng Đá CLB Arsenal đỏ mùa giải 2021-2022</h1>', 10, N'ao-thi-dau-arsernal.jpg', N'Adidas', 1, 0, 0, N'ao-thi-dau-arsernal', N'Đỏ', 100, 20, 10, 5, 135)
SET IDENTITY_INSERT [dbo].[Products] OFF
ALTER TABLE [dbo].[Posts] ADD  CONSTRAINT [DF_Posts_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[CategoryProducts]  WITH CHECK ADD  CONSTRAINT [FK_CatDetails_Categories] FOREIGN KEY([CatID])
REFERENCES [dbo].[Categories] ([CatID])
GO
ALTER TABLE [dbo].[CategoryProducts] CHECK CONSTRAINT [FK_CatDetails_Categories]
GO
ALTER TABLE [dbo].[CategoryProducts]  WITH CHECK ADD  CONSTRAINT [FK_CatDetails_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[CategoryProducts] CHECK CONSTRAINT [FK_CatDetails_Products]
GO
ALTER TABLE [dbo].[OrderProducts]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[OrderProducts] CHECK CONSTRAINT [FK_OrderDetails_Orders]
GO
ALTER TABLE [dbo].[OrderProducts]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[OrderProducts] CHECK CONSTRAINT [FK_OrderDetails_Products]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Accounts] FOREIGN KEY([AccountID])
REFERENCES [dbo].[Accounts] ([AccountID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Accounts]
GO
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD  CONSTRAINT [FK_Posts_Accounts] FOREIGN KEY([AccountID])
REFERENCES [dbo].[Accounts] ([AccountID])
GO
ALTER TABLE [dbo].[Posts] CHECK CONSTRAINT [FK_Posts_Accounts]
GO
USE [master]
GO
ALTER DATABASE [ShopOnline] SET  READ_WRITE 
GO
