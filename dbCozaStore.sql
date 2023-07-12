-- scaffold-dbcontext -provider Microsoft.EntityFrameworkCore.Sqlserver -connection "Data Source=(local); Initial catalog=dbCozaStore; trusted_connection=yes" -outputdir "Models"

-- CREATE DATABASE dbCozaStore


-- 1. Tạo bảng tblRoles: Bảng phân quyền
CREATE TABLE tblRoles 
(
	RoleID INT PRIMARY KEY IDENTITY(1, 1),
	RoleName NVARCHAR(50),
	Description NVARCHAR(250)
);
GO
-- 2. Tạo bảng tblAccounts: Tài khoản
CREATE TABLE tblAccounts (
	AccountID INT PRIMARY KEY IDENTITY(1, 1),
	RoleID INT,
	Password NVARCHAR(150),
	FullName NVARCHAR(250),
	BirthDay DATETIME,
	Country NVARCHAR(150), -- quốc gia
	Education NVARCHAR(100), -- Học vấn - giáo dục
	Address NVARCHAR(200),
	Thumb NVARCHAR(250),
	Phone NCHAR(10),
	Email NVARCHAR(30),
	Salt NCHAR(6),
	Active BIT NOT NULL,
	LastLogin DATETIME,
	CreatedDate DATETIME,
	Description NVARCHAR(250),
	FOREIGN KEY (RoleID) REFERENCES tblRoles(RoleID)
);


-- 3. Tạo bảng tblAttributes: Thuộc tính
CREATE TABLE tblAttributes (
	AttributeID INT PRIMARY KEY IDENTITY(1,1),
	AttributeName NVARCHAR(255),
	Description NVARCHAR(250)
);

GO

-- 4. Tạo bảng tblMenus: Menu
CREATE TABLE tblMenus (
	MenuID INT PRIMARY KEY IDENTITY(1, 1),
	CategoryID INT,
	IsActive BIT NOT NULL,
	MenuName NVARCHAR(50),
	ControllerName NVARCHAR(50),
	ActionName NVARCHAR(50),
	Levels INT,
	ParentID INT,
	MenuOrder INT,
	Position INT,
	Link NVARCHAR(250),
	CreatedDate DATETIME,
	CreatedBy DATETIME,
	ModifiedDate DATETIME,
	ModifierBy DATETIME,
	Description NVARCHAR(250),
);
GO

-- 5. Tạo tblAdminMenus
CREATE TABLE tblAdminMenus 
(
	AdminMenuID BIGINT PRIMARY KEY IDENTITY(1,1),
	ItemName NVARCHAR(50),
	ItemLevel INT,
	ParentLevel INT,
	ItemOrder INT,
	IsActive BIT NOT NULL,
	ItemTarget NVARCHAR(20),
	AreaName NVARCHAR(20),
	ControllerName NVARCHAR(20),
	ActionName NVARCHAR(20),
	Icon NVARCHAR(50),
	IdName NVARCHAR(50)
)
GO

-- 6. Tạo bảng tblCategories: bảng Danh Mục
CREATE TABLE tblCategories
(
	CategoryID INT PRIMARY KEY IDENTITY(1, 1),
	CategoryName NVARCHAR(100),
	ShortName NVARCHAR(100),
	ParentID INT,
	Levels INT,
	IsActive BIT NOT NULL,
	CategoryOrder INT,
	Published BIT,
	Thumb NVARCHAR(250),
	HomeFlag BIT NOT NULL,
	Title NVARCHAR(250),
	Alias NVARCHAR(250),
	MetaDesc NVARCHAR(250),
	MetaKey NVARCHAR(250),
	Cover NVARCHAR(250),
	SchemaMarkup NVARCHAR(MAX),
	Description NVARCHAR(250)
);
GO

-- 7. Tạo bảng tblProducts: Bảng sản phẩm
CREATE TABLE tblProducts (
	ProductID INT PRIMARY KEY IDENTITY(1, 1),
	ProductName NVARCHAR(255),
	ShortDesc NVARCHAR(255),
	CategoryID INT,
	Price INT,
	Discount INT,
	Thumb NVARCHAR(250),
	Video NVARCHAR(250),
	CreatedDate DATETIME,
	ModifiedDate DATETIME,
	BestSellers BIT NOT NULL,
	HomeFlag BIT NOT NULL,
	MenuID INT,
	Active BIT NOT NULL,
	Tags NVARCHAR(MAX),
	Title NVARCHAR(255),
	Alias NVARCHAR(255),
	MetaDesc NVARCHAR(255),
	MetaKey NVARCHAR(255),
	UnitsInStock INT,
	Description NVARCHAR(250),
	FOREIGN KEY (CategoryID) REFERENCES tblCategories (CategoryID),
	FOREIGN KEY (MenuID) REFERENCES tblMenus (MenuID)
);
GO

-- 8. Tạo bảng tblImageProducts: Ảnh sản phẩm
CREATE TABLE tblImageProducts (
	ImageID INT PRIMARY KEY IDENTITY(1, 1),
	ProductID INT,
	ImagePath NVARCHAR(255),
	IsDefault BIT,
	IsActive BIT,
	SortOrder INT,
	Title NVARCHAR(255),
	Description NVARCHAR(250),
);
GO

-- 9. Tạo bảng tblFeedback: Đánh giá sản phẩm
CREATE TABLE tblFeedback(
	FeedbackID INT PRIMARY KEY IDENTITY(1, 1),
	ProductID INT,
	Content NTEXT,
	CreatedDate Datetime,
	IsActive Bit,
	Status Bit,
	CustomerID INT,
	SLike int,
	Sstart int
);
GO

-- 10. Tạo bảng tblAttributes: Thuộc tính giá
CREATE TABLE tblAttributesPrices (
	AttributesPriceID INT PRIMARY KEY IDENTITY(1,1),
	AttributeID INT,
	ProductID INT,
	Price INT,
	Active BIT NOT NULL,
	Description NVARCHAR(250),
	FOREIGN KEY (AttributeID) REFERENCES tblAttributes(AttributeID),
	FOREIGN KEY (ProductID) REFERENCES tblProducts(ProductID)
);
GO

-- 11. Tạo bảng tblLocation: Vị trí 
CREATE TABLE tblLocations
(
	LocationID INT PRIMARY KEY IDENTITY(1,1),
	CustomerID INT,
	FullName NVARCHAR(50),
	Phone NCHAR(10),
	Address nvarchar(150),
	IsDefault bit,
	Description NVARCHAR(250)
);
GO

-- 12. Tạo bảng tblCustomers: Khách hàng
CREATE TABLE tblCustomers (
	CustomerID INT PRIMARY KEY IDENTITY(1,1),
	Password NVARCHAR(50),
	FullName NVARCHAR(50),
	Birthday DATETIME,
	Avatar NVARCHAR(255),
	Address NVARCHAR(150),
	Phone NCHAR(10),
	Email NVARCHAR(30),
	--LocationID INT,
	--District INT, --Quận, huyện.
	--Ward INT, --Phường, xã.
	CreatedDate DATETIME,
	--Salt NCHAR(8),
	LastLogin DATETIME,
	Active BIT,
	Description NVARCHAR(250),
);
GO

-- 13. Tạo bảng tblTransactStatus: Trạng thái đơn hàng
CREATE TABLE tblTransactStatus
(
	TransactStatusID INT PRIMARY KEY IDENTITY(1,1),
	Status NVARCHAR(50),
	Description NVARCHAR(250)
);

GO

-- 14. Tạo bảng tblOrders: Đơn hàng
CREATE TABLE tblOrders (
	OrderID INT PRIMARY KEY IDENTITY(1,1),
	CustomerID INT,
	TransactStatusID INT,
	OrderDate DATETIME,
	ShipDate DATETIME,
	Deleted BIT,
	Paid BIT,
	PaymentDate DATETIME,
	PaymentID BIT,
	Description NVARCHAR(250),
	FOREIGN KEY (CustomerID) REFERENCES tblCustomers(CustomerID),
	FOREIGN KEY (TransactStatusID) REFERENCES tblTransactStatus(TransactStatusID),
);
GO

-- 15. Tạo bảng tblOrderDetails : Chi tiết Đơn hàng
CREATE TABLE tblOrderDetails (
	OrderDetailsID INT PRIMARY KEY IDENTITY(1,1),
	OrderID INT,
	ProductID INT,
	OrderNumber INT,
	Discount INT,
	Quantity INT,
	Total INT,
	ShipDate DATETIME,
	Description NVARCHAR(250),
	FOREIGN KEY (OrderID) REFERENCES tblOrders(OrderID),
	FOREIGN KEY (ProductID) REFERENCES tblProducts(ProductID)
);
GO

-- 16. Tạo bảng tblPages: Tin tức ít biến động
CREATE TABLE tblPages
(
	PageID INT PRIMARY KEY IDENTITY(1,1),
	PageName NVARCHAR(250),
	Contents NVARCHAR(MAX),
	Thumb NVARCHAR(250),
	Published BIT,
	Title NVARCHAR(250),
	MetaDesc NVARCHAR(250),
	MetaKey NVARCHAR(250),
	Alias NVARCHAR(250),
	CreatedDate DATETIME,
	PageOrder INT,
	Description NVARCHAR(250)
);
GO

-- xxx17. Tạo bảng shipper
CREATE TABLE tblShipper (
	ShipperID INT PRIMARY KEY IDENTITY(1,1),
	ShipperName NVARCHAR(150),
	Phone NCHAR(10),
	Company NVARCHAR(150),
	ShipDate DATETIME,
	Description NVARCHAR(250)
);

GO

-- 18. Tạo bảng tblPosts : Tin tức
CREATE TABLE tblPosts (
	PostID INT PRIMARY KEY IDENTITY(1,1),
	CategoryID INT,
	Title NVARCHAR(255),
	Abstract NVARCHAR(255),
	Contents NTEXT,
	Thumb NVARCHAR(255),
	Published BIT,
	IsActive BIT NOT NULL,
	PostOrder INT,
	Status INT,
	CreatedDate DATETIME,
	Author NVARCHAR(150),
	Tags NVARCHAR(MAX),
	isHot BIT NOT NULL,
	isNewfeed BIT NOT NULL,
	Alias NVARCHAR(255),
	MetaDesc NVARCHAR(255),
	MetaKey NVARCHAR(255),
	SView INT,
	Description NVARCHAR(250),
	FOREIGN KEY (CategoryID) REFERENCES tblCategories(CategoryID)
);
GO

-- 19. Tạo bảng comment : Bình luận
CREATE TABLE tblComments (
	CommentID INT PRIMARY KEY IDENTITY(1,1),
	PostID INT,
	CustomerID INT,
	ParentID int,
	Content NTEXT,
	CreatedDate Datetime,
	IsActive Bit,
	Thumb NVARCHAR(150),
	Author NVARCHAR(150),
	SLike int
);
GO

-- 20. Tạo bảng tblSliders
CREATE TABLE tblSliders (
	SliderID INT PRIMARY KEY IDENTITY(1,1),
	CategoryID INT,
	Title NVARCHAR(255),
	Abstract NVARCHAR(255),
	IsActive BIT,
	Images NVARCHAR(255),
	Position INT,
	SliderOrder INT,
	Description NVARCHAR(250),
);
GO

-- 20. Tạo bảng tblFavourites: Sản phẩm yêu thích
CREATE TABLE tblFavourites (
	FavouriteID INT PRIMARY KEY IDENTITY(1,1),
	CustomerID INT,
	ProductID INT,
	CreatedDate Datetime,
);
GO

-- 21. Tạo bảng tblTransactStatus: Trạng thái đơn hàng
CREATE TABLE tblPostStatus
(
	Status INT PRIMARY KEY IDENTITY(1,1),
	StatusName NVARCHAR(50),
	Description NVARCHAR(250)
);
GO

/*INSERT
INSERT INTO tblAdminMenus VALUES 
(N'Quản lý đơn hàng', 1, 0, 1, 1, N'components-nav', N'Admin', N'Home', N'Index', N'bi bi-menu-button-wide', N'components-nav'),
(N'Quản lý sản phẩm', 1, 0, 2, 1, N'forms-nav', N'Admin', N'Home', N'Index', N'bi bi-journal-text', N'forms-nav'),
(N'Quản lý khách hàng', 1, 0, 3, 1, N'tables-nav', N'Admin', N'Home', N'Index', N'bi bi-layout-text-window-reverse', N'tables-nav'),
(N'Quản lý hệ thống', 1, 0, 4, 1, N'charts-nav', N'Admin', N'Home', N'Index', N'bi bi-bar-chart', N'charts-nav'),
(N'Quản lý tin tức', 1, 0, 5, 1, N'chevron-nav', N'Admin', N'Home', N'Index', N'ri ri-add-circle-line', N'chevron-nav'),
(N'Doanh số', 1, 0, 6, 1, N'icons-nav', N'Admin', N'Home', N'Index', N'ri ri-funds-box-line', N'icons-nav'),
(N'Thông tin đơn hàng', 2, 1, 1, 1, NULL, N'Admin', N'Home', N'Index', NULL, NULL),
(N'Thông tin sản phẩm', 2, 2, 1, 1, NULL, N'Admin', N'AdminProducts', N'Index', NULL, NULL),
(N'Thông tin khách hàng', 2, 3, 1, 1, NULL, N'Admin', N'AdminCustomers', N'Index', NULL, NULL),
(N'Quản lý tài khoản', 2, 4, 1, 1, NULL, N'Admin', N'AdminAccounts', N'Index', NULL, NULL),
(N'Quản lý quyền truy cập', 2, 4, 2, 1, NULL, N'Admin', N'AdminRoles', N'Index', NULL, NULL),
(N'Quản lý danh mục', 2, 4, 3, 1, NULL, N'Admin', N'AdminCategories', N'Index', NULL, NULL),
(N'Quản lý menu', 2, 4, 4, 1, NULL, N'Admin', N'AdminMenus', N'Index', NULL, NULL),
(N'Danh sách tin tức', 2, 5, 1, 1, NULL, N'Admin', N'AdminPosts', N'Index', NULL, NULL),
(N'Thống kê', 2, 6, 1, 1, NULL, N'Admin', N'Home', N'Index', NULL, NULL),
(N'Báo cáo', 2, 6, 2, 1, NULL, N'Admin', N'Home', N'Index', NULL, NULL);;

GO

insert into tblMenus(CategoryID, IsActive, MenuName, Levels, ParentID, MenuOrder, Position, Link, ActionName) values
(NULL, 1,	N'Trang chủ'	     				, 	1,	0,	1,	1, 'null', 'null'),
(1, 1,	N'Áo'	             				, 	1,	0,	2,	1, 'null', 'null'),
(2, 1,	N'Quần'	             				, 	1,	0,	3,	1, 'null', 'null'),
(3, 1,	N'Phụ kiện'	         				, 	1,	0,	4,	1, 'null', 'null'),
(NULL, 1,	N'Giới thiệu'	     				, 	1,	0,	6,	1, 'null', 'about'),
(NULL, 1,	N'Liên hệ'	         				, 	1,	0,	7,	1, 'null', 'contact'),
(4, 1,	N'Áo thun'	         				, 	2,	2,	1,	1, 'null', 'null'),
(5, 1,	N'Áo khoác'	         				, 	2,	2,	2,	1, 'null', 'null'),
(6, 1,	N'Áo Polo'	         				, 	2,	2,	3,	1, 'null', 'null'),
(7, 1,	N'Áo nỉ'	         				, 	2,	2,	4,	1, 'null', 'null'),
(8, 1,	N'Áo sơ mi'	         				, 	2,	2,	5,	1, 'null', 'null'),
(9, 1,	N'Quần Short'	     				, 	2,	3,	1,	1, 'null', 'null'),
(10, 1,	N'Quần Jogger'	     				, 	2,	3,	2,	1, 'null', 'null'),
(11, 1,	N'Balo'	             				, 	2,	4,	1,	1, 'null', 'null'),
(12, 1,	N'Nón'	             				, 	2,	4,	2,	1, 'null', 'null'),
(13, 1,	N'Đồng hồ'	         				, 	2,	4,	3,	1, 'null', 'null'),
(14, 1,	N'Túi đeo chéo'	     				, 	2,	4,	4,	1, 'null', 'null'),
(15, 1,	N'Áo thun Basic'	 				, 	3,	7,	1,	1, 'null', 'null'),
(16, 1,	N'Áo thun ArtWork'	 				, 	3,	7,	2,	1, 'null', 'null'),
(17, 1,	N'Áo khoác Bomber'	 				, 	3,	8,	2,	1, 'null', 'null'),
(18, 1,	N'Áo khoác dù'	     				, 	3,	8,	1,	1, 'null', 'null'),
(19, 1,	N'Áo nỉ Hoodie'	     				, 	3,	10,	1,	1, 'null', 'null'),
(20, 1,	N'Áo nỉ Sweater'	 				, 	3,	10,	2,	1, 'null', 'null'),
(NULL, 1,	N'Blog'	             				, 	1,	0,	5,	1, 'null', 'blog'),
(NULL, 1,	N'Nổi bật'			 				, 	1,	0,	1,	2, 'null', 'null'),
(NULL, 1,	N'Mới nhất'			 				, 	1,	0,	2,	2, 'null', 'null'),
(NULL, 1,	N'Sale'				 				, 	1,	0,	3,	2, 'null', 'null'),
(NULL, 1,	N'Top tìm kiếm'		 				, 	1,	0,	4,	2, 'null', 'null'),
(NULL, 1,	N'LIÊN HỆ'							, 	1,	0,	1,	3, 'null', 'null'),
(NULL, 1,	N'LIÊN KẾT'							, 	1,	0,	2,	3, 'null', 'null'),
(NULL, 1,	N'HỖ TRỢ'							, 	1,	0,	3,	3, 'null', 'null'),
(NULL, 1,	N'Cs1: 184 Lê Duẩn, TP Vinh.'		, 	2,	29,	1,	3, 'bi-geo-alt', 'null'),
(NULL, 1,	N'Cs2: 114 Kim Đồng, TP Vinh.'		, 	2,	29,	2,	3, 'bi-geo-alt', 'null'),
(NULL, 1,	N'Cs3: 88 Trần Phú, TP Vinh.'		, 	2,	29,	3,	3, 'bi-geo-alt', 'null'),
(NULL, 1,	N'0868 886 886 - 0982 222 333'		, 	2,	29,	4,	3, 'bi-telephone', 'null'),
(NULL, 1,	N'CozaStore.vn@gmail.com'			, 	2,	29,	5,	3, 'bi-envelope-open', 'null'),
(NULL, 1,	N'Áo thun Basic'					, 	2,	30,	1,	3, 'bi-arrow-right', 'null'),
(NULL, 1,	N'Đồng hồ'							, 	2,	30,	2,	3, 'bi-arrow-right', 'null'),
(NULL, 1,	N'Áo khoác Bomber'					, 	2,	30,	3,	3, 'bi-arrow-right', 'null'),
(NULL, 1,	N'Quần Jogger'						, 	2,	30,	4,	3, 'bi-arrow-right', 'null'),
(NULL, 1,	N'Áo nỉ Hoodie'						, 	2,	30,	5,	3, 'bi-arrow-right', 'null'),
(NULL, 1,	N'Hướng dẫn đặt hàng'				, 	2,	31,	1,	3, 'bi-arrow-right', 'null'),
(NULL, 1,	N'Chính sách và quy định'			, 	2,	31,	2,	3, 'bi-arrow-right', 'null'),
(NULL, 1,	N'Chính sách bảo mật'				, 	2,	31,	3,	3, 'bi-arrow-right', 'null'),
(NULL, 1,	N'Thông tin sở hữu'					, 	2,	31,	4,	3, 'bi-arrow-right', 'null');;;
GO

insert into tblSliders(IsActive, Title, Abstract, Images, SliderOrder, Position, CategoryID) values
(1,	N'JECKETS & COATS'			, 	N'Men New-Season'		, N'slide-02.jpg',	1,	1, 17),
(1,	N'NEW SEASON'	            , 	N'Woman Collection 2022', N'slide-04.jpg',	2,	1, 15),
(1,	N'NEW ARRIVALS'	            , 	N'Men Collection 2022'	, N'slide-03.jpg',	3,	1, 19);;;
GO

insert into tblCategories(CategoryName, Levels, ParentID, CategoryOrder, IsActive, Thumb, HomeFlag, Title, ShortName ) values
(N'Áo'					, 	1,	0,	2,	1, N'shirt.png', 1, N'null', N'SHIRT'),
(N'Quần'				, 	1,	0,	3,	1, N'null', 0, N'null', N'null'),
(N'Phụ kiện'			, 	1,	0,	4,	1, N'null', 0, N'null', N'null'),
(N'Áo thun'				, 	2,	2,	1,	1, N'null', 0, N'null', N'null'),
(N'Áo khoác'	        , 	2,	2,	2,	1, N'null', 0, N'null', N'null'),
(N'Áo Polo'				, 	2,	2,	3,	1, N'null', 0, N'null', N'null'),
(N'Áo nỉ'				, 	2,	2,	4,	1, N'null', 0, N'null', N'null'),
(N'Áo sơ mi'	        , 	2,	2,	5,	1, N'null', 0, N'null', N'null'),
(N'Quần Short'			, 	2,	3,	1,	1, N'null', 0, N'null', N'null'),
(N'Quần Jogger'			, 	2,	3,	2,	1, N'null', 0, N'null', N'null'),
(N'Balo'	            , 	2,	4,	1,	1, N'null', 0, N'null', N'null'),
(N'Nón'					, 	2,	4,	2,	1, N'null', 0, N'null', N'null'),
(N'Đồng hồ'				, 	2,	4,	3,	1, N'null', 0, N'null', N'null'),
(N'Túi đeo chéo'	    , 	2,	4,	4,	1, N'null', 0, N'null', N'null'),
(N'Áo thun Basic'		, 	3,	7,	1,	1, N'basictee.png', 1, N'null', N'BASIC TEE'),
(N'Áo thun ArtWork'		, 	3,	7,	2,	1, N'null', 0, N'null', N'null'),
(N'Áo khoác Bomber'		, 	3,	8,	2,	1, N'bomber.png', 1, N'null', N'BOMBER'),
(N'Áo khoác dù'			, 	3,	8,	1,	1, N'null', 0, N'null', N'null'),
(N'Áo nỉ Hoodie'	    , 	3,	10,	1,	1, N'null', 0, N'null', N'null'),
(N'Áo nỉ Sweater'		, 	3,	10,	2,	1, N'null', 0, N'null', N'null');;;;
GO

insert into tblProducts(CategoryID, ProductName, Price,	 Discount,	Thumb,													HomeFlag,	MenuID, Active, BestSellers, UnitsInStock	) values
(9,		N'FRINGY LOGO SHORTS (Black)'	           , 130000	,0,			N'8ab6dc5708ddc88391cc.webp',	1,			25,		1,		1		, 0	),
(19,	N'DARKO HOODIE (Black)'					   , 290000	,0,			N'97ed-bf70bd2c954c.webp',	1,			25,		1,		1		, 0	),
(6,		N'ZIGZAG POLO SHIRT (Black)'	           , 390000	,0,			N'72-copy.png',				1,			25,		1,		1		, 0	),
(17,	N'NEUTRON CHUTE BOMBER'					   , 440000	,0,			N'neutron-chute-bomber.webp',	1,			25,		1,		1		, 0	),
(17,	N'SIGNATURE LOGO CHUTE JACKET'	           , 440000	,0,			N'8aa6-724fd71da575.webp',	1,			25,		1,		1		, 0	),
(17,	N'SQUIGGLE HOURGLASS BOMBER'	           , 590000	,0,			N'mat-truoc-0-jpeg.webp',		1,			25,		1,		1		, 0	),
(15,	N'Áo thun Paradox THE REVERIE TEE (Black)' , 350000	,10,		N'9c425ca0-96da-4ec6-b638-b09bc3e09768.webp',	1,	25,	1,	1		, 0	),
(12,	N'Nón Paradox GRINNING CORDURO'			   , 99000	,25,		N'415d-a5a2-0a67b59899fb.jpg',1,			25,		1,		1		, 0	),
(15,	N'KIDDO TEE (White)'					   , 199000	,0,			N'4ccb-a660-a0a3e4384138.webp',1,			25,		1,		1		, 0	),
(15,	N'Áo thun Paradox FLYING FAIRY TEE'		   , 199000	,20,		N'897b53bbcbf83a9f3bf07214fcdeb93d.webp',	1,25,	1,		1		, 0	),
(11,	N'Balo Paradox ALIEN ERA BACKPACK - BLACK POCKET', 320000,10,	N'b16b-47c7-bac4-46e96ce1e728.webp',1,	25,		1,		1		, 0	),
(10,	N'ATTENTION LOGO JOGGER PANTS (Black)'	   , 320000	,20,		N'782s7jmg-1-ct5t-hinh-mat-truoc-0.webp',	1,25,	1,		1		, 0	);;;;
GO

insert into tblImageProducts(ProductID, ImagePath, IsDefault, IsActive, SortOrder, Title) values
(1, 'f33f7ddea954690a3045.jpg'														, 1, 1, 1, N'/neutron-chute-bomber.webp'),
(1, '8ab6dc5708ddc88391cc.jpg'														, 0, 1, 2, N'/neutron-chute-bomber.webp'),
(1, 'bang-size-varsity.png'															, 0, 1, 3, N'/neutron-chute-bomber.webp'),
(2, 'dad32e19-0c71-4495-97ed-bf70bd2c954c.png'										, 1, 1, 1, N'/neutron-chute-bomber.webp'),
(2, 'cc0c1c47-a87a-4a8a-a19f-2e320c96f87e.png'										, 0, 1, 2, N'/neutron-chute-bomber.webp'),
(2, 'bang-size-varsity.png'															, 0, 1, 3, N'/neutron-chute-bomber.webp'),
(3, '72-copy.png'																		, 1, 1, 1, N'/neutron-chute-bomber.webp'),
(3, '71-copy.webp'																	, 0, 1, 2, N'/neutron-chute-bomber.webp'),
(3, 'bang-size-varsity.png'															, 0, 1, 3, N'/neutron-chute-bomber.webp'),
(5, '1-32c28533-6584-485d-8aa6-724fd71da575.webp'										, 1, 1, 1, N'/neutron-chute-bomber.webp'),
(5, '2-05d45d20-451e-48c7-9513-852dc4a063f6.webp'										, 0, 1, 2, N'/neutron-chute-bomber.webp'),
(5, 'f7512451-c825-488d-842d-260811159018.webp'										, 0, 1, 3, N'/neutron-chute-bomber.webp'),
(6, 'jhpwu5y2-1-psm9-hinh-mat-truoc-0-jpeg.webp'										, 1, 1, 1, N'/neutron-chute-bomber.webp'),
(6, 'jhpwu5y2-1-psm9-hinh-mat-sau-0-jpeg.webp'										, 0, 1, 2, N'/neutron-chute-bomber.webp'),
(6, 'bang-size-varsity.png'															, 0, 1, 3, N'/neutron-chute-bomber.webp'),
(7, '9c425ca0-96da-4ec6-b638-b09bc3e09768 (1).webp'									, 1, 1, 1, N'/neutron-chute-bomber.webp'),
(7, 'rev-bl1.webp'																	, 0, 1, 2, N'/neutron-chute-bomber.webp'),
(7, 'z2546025145913-fd6e0a25a7d59cd00e937c2aa2e986cc-optimized.jpg'					, 0, 1, 2, N'/neutron-chute-bomber.webp'),
(7, 'e6dd2381a541641f3d50-jpeg.webp'													, 0, 1, 4, N'/neutron-chute-bomber.webp'),
(8, '8f87ee42-de22-415d-a5a2-0a67b59899fb.webp'										, 1, 1, 1, N'/neutron-chute-bomber.webp'),
(8, '1e9d5446-7b37-4b0a-a275-dc1c648a6972.webp'										, 0, 1, 2, N'/neutron-chute-bomber.webp'),
(8, '78b0c9e6-f805-4422-9a5a-fbb9f8c879b0.webp'										, 0, 1, 3, N'/neutron-chute-bomber.webp'),
(9, '2014ffe2-a5d3-4ccb-a660-a0a3e4384138.webp'										, 1, 1, 1, N'/neutron-chute-bomber.webp'),
(9, 'b8a822d8-8731-490b-9d32-5f08ffa8663d.webp'										, 0, 1, 2, N'/neutron-chute-bomber.webp'),
(9, 'z2942788864680-0761cbf0003d60174b5003b93ac8ab7d-optimized.webp'					, 0, 1, 3, N'/neutron-chute-bomber.webp'),
(10, 'z2662962648071-897b53bbcbf83a9f3bf07214fcdeb93d.jpg'							, 1, 1, 1, N'/neutron-chute-bomber.webp'),
(10, '7c740972-8b7c-4b4a-a331-522826468556.webp'										, 0, 1, 2, N'/neutron-chute-bomber.webp'),
(10, 'e6dd2381a541641f3d50-jpeg.webp'													, 0, 1, 3, N'/neutron-chute-bomber.webp'),
(11, 'f2ff1c0b-7e40-4bd3-97ba-7253fecce5e5-64eb45b7-b16b-47c7-bac4-46e96ce1e728.webp'	, 1, 1, 1, N'/neutron-chute-bomber.webp'),
(11, '02503ba2-a821-4058-955b-b875ac6cd985-055306fa-6dbe-4cbb-ab7c-a2bf8ec351c0.webp'	, 0, 1, 2, N'/neutron-chute-bomber.webp'),
(11, 'ab033541-17b5-4877-86f2-7210a8806665-956d61f5-ed40-4a23-be2e-e8c95cae9778.webp'	, 0, 1, 3, N'/neutron-chute-bomber.webp'),
(12, '782s7jmg-1-ct5t-hinh-mat-truoc-0.jpg'                                           , 1, 1, 1, N'/neutron-chute-bomber.webp'),
(12, '782s7jmg-1-ct5t-hinh-mat-sau-0.webp'											, 0, 1, 2, N'/neutron-chute-bomber.webp'),
(12, '782s7jmg-1-ct5t-hinh-chi-tiet-goc-do-khac-0.jpg'								, 0, 1, 3, N'/neutron-chute-bomber.webp'),
(4, 'neutron-chute-bomber.webp', 1, 1, 1, N'/neutron-chute-bomber.webp'),
(4, 'stfzifr3-1-t7va-hinh-mat-sau-0.png', 0, 1, 2, N'/neutron-chute-bomber.webp'),
(4, 'bang-size-varsity.png', 0, 1, 3, N'/neutron-chute-bomber.webp');;;;
go

insert into tblPosts(CategoryID, Title, Abstract, Contents, Thumb, IsActive, isHot, isNewfeed, Published, PostOrder, Status, CreatedDate, Author) values
(14,  N'Làm thế nào để phối đồ Local Brand xịn như phong cách Unisex?', N'Làm thế nào để phối đồ Local Brand xịn như phong cách Unisex?', N'Bạn muốn diện đồ với phong cách Unisex nhưng trong tủ quần áo chỉ có đồ Local Brand? Với sự phổ biến của thời trang Unisex khi ranh giới giữa 2 giới tính đã dần bị xóa nhòa, mang lại làn sóng thời trang mới thật mạnh mẽ. Hôm nay Hidanz sẽ giúp bạn phối đồ Local Brand như thế nào để chuẩn với thời trang Unisex nhất.'
,'Ao-hoodie-quan-bermuda-cho-ban-thoa-suc-sang-tao.png', 1, 1, 1, 1, 1, 1, '10/30/2022', N'Hồ Hòa'),
(14,  N'Bật mí bí quyết mix & match với balo Local Brand chất lượng', N'Bật mí bí quyết mix & match với balo Local Brand chất lượng', N'Để tạo nên một bộ outfit hoàn hảo và trông bắt mắt nhất thì phụ kiện đi kèm là một món đồ không thể thiếu với các tín đồ thời trang. Kết hợp trang phục với những loại phụ kiện mũ, giày, trang sức,… và đặc biệt là một chiếc túi xinh xắn là một điều rất quen thuộc với mỗi người khi bước chân ra đường. Tuy nhiên, với những chiếc túi đeo chéo như thế chắc hẳn ai cũng đau đầu vì phải suy nghĩ xem nên lựa màu nào để phù hợp nhất với bộ trang phục mình sẽ mặc. Vì thế hôm nay Hidanz sẽ gợi ý cho bạn những cách để chọn màu túi phù hợp với trang phục nhất, bạn đừng bỏ lỡ những mẹo hay này nhé.'
, 'Balo-Local-Brand-REGODS-CLUB.png', 1, 1, 1, 1,1, 1, '10/30/2022', N'Hồ Hòa'),
(14,  N'LGBT là gì? Tìm hiểu phong cách độc lạ của thời trang LGBT', N'LGBT là gì? Tìm hiểu phong cách độc lạ của thời trang LGBT', N'Để tạo nên một bộ outfit hoàn hảo và trông bắt mắt nhất thì phụ kiện đi kèm là một món đồ không thể thiếu với các tín đồ thời trang. Kết hợp trang phục với những loại phụ kiện mũ, giày, trang sức,… và đặc biệt là một chiếc túi xinh xắn là một điều rất quen thuộc với mỗi người khi bước chân ra đường. Tuy nhiên, với những chiếc túi đeo chéo như thế chắc hẳn ai cũng đau đầu vì phải suy nghĩ xem nên lựa màu nào để phù hợp nhất với bộ trang phục mình sẽ mặc. Vì thế hôm nay Hidanz sẽ gợi ý cho bạn những cách để chọn màu túi phù hợp với trang phục nhất, bạn đừng bỏ lỡ những mẹo hay này nhé.'
, 'Co-cau-vong-luc-sac-Bieu-tuong-dep-cua-cong-dong-LGBT.jpg', 1, 1, 1, 1, 1, 1, '10/30/2022', N'Hồ Hòa'),
(14, N'Công thức diện đồ với quần dài Unisex nam và nữ siêu đẹp', N'Công thức diện đồ với quần dài Unisex nam và nữ siêu đẹp', N'Phong cách unisex đã in sâu trong tâm trí rất nhiều người, chúng ta rất dễ dàng bắt gặp chúng ở rất nhiều cửa hàng thời trang, một phong cách được ưa chuộng trong làng thời trang cao cấp. Vậy làm sao để có thể dễ dàng theo đuổi được phong cách unisex cá tính này? Cùng tìm hiểu qua những cách diện đồ với quần dài unisex nam và nữ trong bài viết dưới đây. Quần áo unisex là gì? Trong suốt nhiều thập kỷ, xã hội luôn có lệnh rằng nam giới phải ăn mặc như thế này, nữ giới phải ăn mặc theo cách khác. Vì thế thời trang unisex đã được sinh ra và là hàng may mặc được thiết kế không theo một giới tính cụ thể nào cả. Mặc dù kích thước cơ thể của phái nam và phái nữ khác nhau dẫn đến những đường cắt khác nhau với quần áo. Tuy nhiên rất nhiều chị em phụ nữ lại thích quần dài unisex nam hơn, hơn hết là khi họ không có nhiều đường cong hông. Thuật ngữ unisex xuất hiện, mang lại một cái gì đó mà cả hai giới tính đều có thể được sử dụng.'
, 'Quan-ong-suong-lich-lam-cuc-ki-de-phoi-do.jpg', 1, 1, 1, 1, 1, 1, '10/30/2022', N'Hồ Hòa'),
(14,  N'Cách chọn màu túi đeo chéo phù hợp với quần áo', N'Cách chọn màu túi đeo chéo phù hợp với quần áo', N'Để tạo nên một bộ outfit hoàn hảo và trông bắt mắt nhất thì phụ kiện đi kèm là một món đồ không thể thiếu với các tín đồ thời trang. Kết hợp trang phục với những loại phụ kiện mũ, giày, trang sức,… và đặc biệt là một chiếc túi xinh xắn là một điều rất quen thuộc với mỗi người khi bước chân ra đường. Tuy nhiên, với những chiếc túi đeo chéo như thế chắc hẳn ai cũng đau đầu vì phải suy nghĩ xem nên lựa màu nào để phù hợp nhất với bộ trang phục mình sẽ mặc. Vì thế hôm nay Hidanz sẽ gợi ý cho bạn những cách để chọn màu túi phù hợp với trang phục nhất, bạn đừng bỏ lỡ những mẹo hay này nhé.'
, 'Tui-deo-cheo-mau-nau-co-dien.jpg', 1, 1, 1, 1,1, 1, '10/20/2022', N'Hồ Hòa'),
(14,  N'Cách chọn màu túi đeo chéo phù hợp với quần áo', N'Cách chọn màu túi đeo chéo phù hợp với quần áo', N'Để tạo nên một bộ outfit hoàn hảo và trông bắt mắt nhất thì phụ kiện đi kèm là một món đồ không thể thiếu với các tín đồ thời trang. Kết hợp trang phục với những loại phụ kiện mũ, giày, trang sức,… và đặc biệt là một chiếc túi xinh xắn là một điều rất quen thuộc với mỗi người khi bước chân ra đường. Tuy nhiên, với những chiếc túi đeo chéo như thế chắc hẳn ai cũng đau đầu vì phải suy nghĩ xem nên lựa màu nào để phù hợp nhất với bộ trang phục mình sẽ mặc. Vì thế hôm nay Hidanz sẽ gợi ý cho bạn những cách để chọn màu túi phù hợp với trang phục nhất, bạn đừng bỏ lỡ những mẹo hay này nhé.'
, 'Tui-deo-cheo-mau-nau-co-dien.jpg', 1, 1, 1, 1, 1, 1, '10/20/2022', N'Hồ Hòa'),
(14, N'Cách chọn màu túi đeo chéo phù hợp với quần áo', N'Cách chọn màu túi đeo chéo phù hợp với quần áo', N'Để tạo nên một bộ outfit hoàn hảo và trông bắt mắt nhất thì phụ kiện đi kèm là một món đồ không thể thiếu với các tín đồ thời trang. Kết hợp trang phục với những loại phụ kiện mũ, giày, trang sức,… và đặc biệt là một chiếc túi xinh xắn là một điều rất quen thuộc với mỗi người khi bước chân ra đường. Tuy nhiên, với những chiếc túi đeo chéo như thế chắc hẳn ai cũng đau đầu vì phải suy nghĩ xem nên lựa màu nào để phù hợp nhất với bộ trang phục mình sẽ mặc. Vì thế hôm nay Hidanz sẽ gợi ý cho bạn những cách để chọn màu túi phù hợp với trang phục nhất, bạn đừng bỏ lỡ những mẹo hay này nhé.'
, 'Tui-deo-cheo-mau-nau-co-dien.jpg', 1, 1, 1, 1, 1, 1, '10/20/2022', N'Hồ Hòa');;;;
go

INSERT INTO tblTransactStatus VALUES
(N'Chờ xác nhận', N'Chờ xác nhận'),
(N'Chờ lấy hàng', N'Chờ lấy hàng'),
(N'Đang giao', N'Đang giao'),
(N'Đã giao', N'Đã giao'),
(N'Đã hủy', N'Đã hủy'),
(N'Trả hàng/Hoàn tiền', N'Trả hàng/Hoàn tiền');;;
go
*/

CREATE VIEW View_Post_Cat
AS
SELECT p.*, mn.CategoryName
FROM tblCategories mn join tblPosts p
on mn.CategoryID = p.CategoryID


CREATE VIEW ViewImageProduct
AS
SELECT p.*, ipp.ImagePath, ipp.IsActive, ipp.IsDefault, ipp.SortOrder, ipp.ImageID
FROM tblProducts p left join tblImageProducts ipp
on ipp.ProductID = p.ProductID

CREATE VIEW ViewOrderDetails
AS
select od.*, o.CustomerID, p.Thumb, p.ProductName, p.Price
from tblOrders o
join tblOrderDetails od on od.OrderID = o.OrderID
join tblProducts p on p.ProductID = od.ProductID

CREATE VIEW ViewFeedback
AS
select fb.*, c.Avatar, c.FullName
from tblFeedback fb
join tblCustomers c on c.CustomerID = fb.CustomerID

-- Tạo View chi tiết đơn đặt hàng
CREATE VIEW ViewOrder
AS
    SELECT P.ProductID, TS.TransactStatusID, O.OrderID, C.Address, C.CustomerID, P.ProductName, P.Price, C.FullName, C.Phone, P.Active, P.UnitsInStock, O.OrderDate, O.Deleted, O.PaymentDate, O.Paid, OD.Discount, OD.Quantity, OD.Total, OD.ShipDate, TS.Status  
    FROM tblProducts P
        INNER JOIN tblOrderDetails OD ON OD.ProductID = P.ProductID
        INNER JOIN tblOrders O ON O.OrderID = OD.OrderID
        INNER JOIN tblTransactStatus TS ON TS.TransactStatusID = O.TransactStatusID
        INNER JOIN tblCustomers C ON C.CustomerID = O.CustomerID

create view ViewFavourite
as
	select p.*, f.ProductName, f.Thumb, f.Discount, f.Price
	from  tblFavourites p
	join tblProducts f on p.ProductID = f.ProductID


--View Admin-------------------------------------------------------------------------------------
-- Tạo view - bán chạy nhất
CREATE VIEW ViewBestSellers
AS
	SELECT p.*, od.Quantity, od.Total FROM tblProducts p
		INNER JOIN tblOrderDetails od ON od.ProductID = p.ProductID


-- Tạo View bán gần đây
CREATE VIEW ViewRecentSales
AS
	SELECT P.ProductID, P.ProductName, P.Price, C.FullName, C.Phone, P.Active, P.UnitsInStock, O.OrderDate, O.PaymentDate, O.Paid, OD.Discount, OD.Quantity, OD.Total, OD.ShipDate, TS.Status  FROM tblProducts P
		INNER JOIN tblOrderDetails OD ON OD.ProductID = P.ProductID
		INNER JOIN tblOrders O ON O.OrderID = OD.OrderID
		INNER JOIN tblTransactStatus TS ON TS.TransactStatusID = O.TransactStatusID
		INNER JOIN tblCustomers C ON C.CustomerID = O.CustomerID

----------------------------------------------------
delete tblOrderDetails
delete tblOrders

select * from tblAdminMenus
where AdminMenuID = 51

select * from tblPosts

update tblPosts 
set SView = 999
where PostID between 49 and 55

