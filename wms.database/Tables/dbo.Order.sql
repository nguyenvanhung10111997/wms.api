CREATE TABLE dbo.[Order] 
(
	OrderID INT NOT NULL IDENTITY(1, 1),
	OrderCode VARCHAR(200),
	CustomerName NVARCHAR(200),
	BeginDate DATETIME,
	EndDate DATETIME,
	TotalTargetQuantity INT,
	TotalWorkers INT,
	TotalAmount DECIMAL(18,4),
	[StatusID] INT,
	CreatedUser INT NOT NULL,
	CreatedDate DATETIME NOT NULL DEFAULT(GETDATE()),
	UpdatedUser INT NOT NULL,
	UpdatedDate DATETIME NOT NULL DEFAULT(GETDATE()),
	IsDeleted BIT NOT NULL DEFAULT(0),
	CONSTRAINT PK_Order PRIMARY KEY CLUSTERED (OrderID)
);