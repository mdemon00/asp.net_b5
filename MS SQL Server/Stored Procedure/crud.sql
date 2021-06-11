USE [aspnetB5]
GO
/****** Object:  StoredProcedure [dbo].[stpGetAllStudents]    Script Date: 6/5/2021 11:18:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[stpGetAllStudents]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Students;
END



USE [aspnetB5]
GO
/****** Object:  StoredProcedure [dbo].[stpGetStudent]    Script Date: 6/5/2021 11:19:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[stpGetStudent]
	@NAME nvarchar(200) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Students WHERE [Name] = @NAME;
END



USE [aspnetB5]
GO
/****** Object:  StoredProcedure [dbo].[stpInsertStudent]    Script Date: 6/5/2021 11:19:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[stpInsertStudent]
	@Name nvarchar(200) = null,
	@Weight decimal(18,2) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Insert into Students (Name,Weight) Values(@Name, @Weight)
END



USE [aspnetB5]
GO
/****** Object:  StoredProcedure [dbo].[stpUpdateStudent]    Script Date: 6/5/2021 11:19:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[stpUpdateStudent]
	@Id int = null,
	@Name nvarchar(200) = null,
	@Weight decimal(18,0) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Update Students
	Set 
	[Name] = @Name,
	[Weight] = @Weight
	WHERE 
	[Id] = @Id
END



USE [aspnetB5]
GO
/****** Object:  StoredProcedure [dbo].[DeleteStudent]    Script Date: 6/5/2021 11:19:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[DeleteStudent]
	@Id int = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Delete from Students
	Where 
	[Id] = @Id;
	
END
