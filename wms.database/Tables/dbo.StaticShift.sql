CREATE TABLE dbo.StaticShift 
(
	StaticShiftID INT NOT NULL IDENTITY(1, 1),
	StaticShiftName NVARCHAR(200) NOT NULL,
	StartTime VARCHAR(10) NULL,
	EndTime VARCHAR(10) NULL,
	CreatedUser INT NOT NULL,
	CreatedDate DATETIME NOT NULL DEFAULT(GETDATE()),
	UpdatedUser INT NOT NULL,
	UpdatedDate DATETIME NOT NULL DEFAULT(GETDATE()),
	IsDeleted BIT NOT NULL DEFAULT(0),
	CONSTRAINT PK_StaticShift PRIMARY KEY CLUSTERED (StaticShiftID)
);