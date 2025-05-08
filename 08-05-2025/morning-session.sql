
-- BULK INSERT WITH STORED PROCEDURE


CREATE TABLE people (
id int primary key,
name varchar(100), 
age int
)

CREATE OR ALTER PROC proc_BulkInsert(@filepath nvarchar(100))
AS
BEGIN
	declare @insertQuery nvarchar(max)

	set @insertQuery = 'BULK INSERT people FROM '''+@filepath+'''
	WITH (
		FIRSTROW = 2,
		FIELDTERMINATOR = '','',
		ROWTERMINATOR = ''\n''
	)'

	EXEC sp_executesql @insertquery
END

EXEC proc_BulkInsert 'C:\sai\genspark_training\08-05-2025\data.csv'

select * from people

drop proc proc_BulkInsert


-- BULK INSERT WITH STORED PROCEDURE AND EXCEPTION HANDLING

create table BulkInsertLog(
	id int identity(1, 1) primary key,
	filepath nvarchar(100),
	status nvarchar(50) constraint chk_sts Check(status in ('Success', 'Failed')),
	message nvarchar(200),
	insertedat Datetime default GetDate()
)


CREATE OR ALTER PROC proc_BulkInsert(@filepath nvarchar(100))
AS
BEGIN
	BEGIN TRY
		declare @insertQuery nvarchar(max)

		set @insertQuery = 'BULK INSERT people FROM '''+@filepath+'''
		WITH (
			FIRSTROW = 2,
			FIELDTERMINATOR = '','',
			ROWTERMINATOR = ''\n''
		)'

		EXEC sp_executesql @insertquery

		insert into BulkInsertLog (filepath, status, message)
		values (@filepath, 'Success', 'Inserted Successfully')

	END TRY
	BEGIN CATCH
		insert into BulkInsertLog(filepath, status, message)
		values (@filepath, 'Failed', ERROR_MESSAGE())
	END CATCH
END


truncate table people;

EXEC proc_BulkInsert 'C:\sai\genspark_training\08-05-2025\daa.csv' -- File path error

select * from BulkInsertLog -- Exception handled

EXEC proc_BulkInsert 'C:\sai\genspark_training\08-05-2025\data.csv' -- Correct Path

select * from people
select * from BulkInsertLog

-- CTE COMMON TABLE EXPRESSIONS

select * from authors;

with cte_Authors
as
(
	select au_id, au_fname, au_lname from authors
)
select * from cte_Authors


-- CTE ROW NUMBER WAY OF PAGINATION

CREATE OR ALTER PROC proc_GetTitilesByPage(@pno int, @psize int)
AS
BEGIN
	with cte_titles_paginated as
	(select title_id, title, type, price, ROW_NUMBER() over (order by price) as row from titles) 

	select * from cte_titles_paginated where row between ((@pno-1) * @psize+1) and (@pno * @psize)
END

exec proc_GetTitilesByPage 1, 10

-- OFFSET WAY OF PAGINATION (NEW)

CREATE OR ALTER PROC proc_GetTitilesByPage(@pno int, @psize int)
AS
BEGIN
	select * from titles 
	order by price
	offset (@pno - 1) * @psize rows fetch next @psize rows only
END

exec proc_GetTitilesByPage 2, 10


-- FUNCTIONS

create function fn_calculateTax(@baseprice float, @tax float)
returns float
as 
begin
	return (@baseprice + (@baseprice * @tax /100))
end

select title, price, dbo.fn_calculateTax(price, 10) as taxed_price from titles order by taxed_price desc

-- TABLE VALUED FUNCTION

create function fn_tableSelect(@price float)
returns table
as
	return select * from titles where price >= @price

select * from dbo.fn_tableSelect(10);

-- EXPERIMENTING CURSOR

DECLARE @authid varchar(50), @authname varchar(100);
DECLARE @title_id varchar(20), @title varchar(200), @price float;

DECLARE auth_cursor CURSOR
FOR
    SELECT au_id, CONCAT(au_fname, ' ', au_lname) AS authname
    FROM authors;

OPEN auth_cursor;
FETCH NEXT FROM auth_cursor INTO @authid, @authname;

WHILE @@FETCH_STATUS = 0
BEGIN
    DECLARE title_cursor CURSOR
    FOR
        SELECT t.title_id, t.title, t.price
        FROM titleauthor ta
        JOIN titles t ON ta.title_id = t.title_id
        WHERE ta.au_id = @authid;

    OPEN title_cursor;
    FETCH NEXT FROM title_cursor INTO @title_id, @title, @price;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT 'Author: ' + @authname + ' | Title: ' + @title + ' | Price: ' + CAST(@price AS VARCHAR);
        FETCH NEXT FROM title_cursor INTO @title_id, @title, @price;
    END

    CLOSE title_cursor;
    DEALLOCATE title_cursor;

    FETCH NEXT FROM auth_cursor INTO @authid, @authname;
END

CLOSE auth_cursor;
DEALLOCATE auth_cursor;
