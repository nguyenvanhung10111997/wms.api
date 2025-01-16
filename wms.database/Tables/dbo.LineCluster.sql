CREATE TABLE dbo.LineCluster 
(
	LineClusterID INT NOT NULL IDENTITY(1, 1),
	LineID INT NOT NULL,
	ClusterID INT NOT NULL,
	CreatedUser INT NOT NULL,
	CreatedDate DATETIME NOT NULL DEFAULT(GETDATE()),
	UpdatedUser INT NOT NULL,
	UpdatedDate DATETIME NOT NULL DEFAULT(GETDATE()),
	IsDeleted BIT NOT NULL DEFAULT(0),
	CONSTRAINT PK_LineCluster PRIMARY KEY CLUSTERED (LineClusterID)
);