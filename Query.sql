Use master 
Go
Create Database Kanban
Go
Use Kanban
Go
Create Table [User] (
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Email nvarchar(256) unique not null,
	Username nvarchar(256) unique not null,
	Password nvarchar(256) not null,
	Status bit not null,
)
Go
Create Table [Role] (
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Description nvarchar(max),
)
Go
Create Table UserRole (
	UserId uniqueidentifier foreign key references [User](Id),
	RoleId uniqueidentifier foreign key references [Role](Id),
	Description nvarchar(max),
	Primary key (UserId, RoleId)
)
Go
Create Table Project(
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Description nvarchar(max),
	LeaderId uniqueidentifier foreign key references [User](Id) not null,
	DefaultAssigneeId uniqueidentifier foreign key references [User](Id),
	CreateAt datetime not null default getdate(),
	UpdateAt datetime,
	IsClose bit not null default 0
)
Go
Create Table [Type] (
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Description nvarchar(max),
	IsDefault bit not null default 0
)
Go
Create Table [Status] (
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Description nvarchar(max),
	IsDefault bit not null default 0
)
Go
Create Table [Priority] (
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Description nvarchar(max),
	IsDefault bit not null default 0
)
Go
Create Table [Issue] (
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Description nvarchar(max),
	AssigneeId uniqueidentifier foreign key references [User](Id) not null,
	EstimateTime int not null default 0,
	PriorityId uniqueidentifier foreign key references [Priority](Id) not null,
	StatusId uniqueidentifier foreign key references [Status](Id) not null,
	TypeId uniqueidentifier foreign key references [Type](Id) not null,
	DueDate datetime not null,
	ReporterId uniqueidentifier foreign key references [User](Id) not null,
	CreateAt datetime not null default getdate(),
	UpdateAt datetime,
	IsClose bit not null default 0
)
Go
Create Table [ChildIssue] (
	IssueId uniqueidentifier foreign key references [Issue](Id) not null,
	ChildId uniqueidentifier foreign key references [Issue](Id) unique not null,
	Description nvarchar(max),
	Primary key (IssueId, ChildId)
)
Go
Create Table [Attachment] (
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	Url nvarchar(max) not null,
	IssueId uniqueidentifier foreign key references [Issue](Id) not null,
)
Go
Create Table [Link] (
	Id uniqueidentifier primary key,
	Url nvarchar(max) not null,
	Description nvarchar(max),
	IssueId uniqueidentifier foreign key references [Issue](Id) not null,
)
Go
Create Table [Label] (
	Id uniqueidentifier primary key,
	Name nvarchar(256) not null,
	IssueId uniqueidentifier foreign key references [Issue](Id) not null,
)
Go
Create Table [Comment] (
	Id uniqueidentifier primary key,
	IssueId uniqueidentifier foreign key references [Issue](Id) not null,
	UserId uniqueidentifier foreign key references [User](Id) not null,
	Content nvarchar(256) not null,
	CreateAt datetime not null default getdate()
)
Go
Create Table [LogWork] (
	Id uniqueidentifier primary key,
	IssueId uniqueidentifier foreign key references [Issue](Id) not null,
	UserId uniqueidentifier foreign key references [User](Id) not null,
	SpentTime int not null,
	RemainingTime int not null
)
