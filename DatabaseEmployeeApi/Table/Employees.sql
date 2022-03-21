IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('Employees'))
BEGIN
	CREATE TABLE [dbo].Employees(
		Id INT IDENTITY(1,1) NOT NULL,
		FirstName VARCHAR(256) NULL,
		LastName VARCHAR(256) NULL,
		Salary INT NULL,
		Email VARCHAR(256) NULL,
		Mobile VARCHAR(256) NULL,
	)
END
GO