use pubs;
go

select * from publishers;

select * from titles;

select * from sales;

select * from authors;

select * from titleauthor;

select * from stores;

-- Joins --


select publishers.pub_name Pub_Name, titles.title Book_Name, sales.ord_date Order_Date from sales
join titles on sales.title_id = titles.title_id
join publishers on titles.pub_id = publishers.pub_id


select distinct pub_name from publishers
left join titles on publishers.pub_id = titles.pub_id;



select titleauthor.au_id, concat(au_lname, ' ',au_fname) as au_name, title from titleauthor
join authors on authors.au_id = titleauthor.au_id
join titles on  titles.title_id = titleauthor.title_id;


select pub_name, min(ord_date) from publishers
left outer join titles on titles.pub_id = publishers.pub_id
left outer join sales on sales.title_id = titles.title_id
group by pub_name

select p.pub_name, min(s.ord_date) as FirstOrder from titles t 
right join publishers p on t.pub_id = p.pub_id 
left join sales s on t.title_id = s.title_id 
group by p.pub_name 
order by FirstOrder desc;


select title Book_name, stor_address from titles
join sales on sales.title_id = titles.title_id
join stores on sales.stor_id = stores.stor_id


-- Procedures --


CREATE PROC proc_FirstProcedure
AS
BEGIN
	print 'Hello Wrold!!'
END


EXEC proc_FirstProcedure


CREATE TABLE Products (
	id int identity(1,1) constraint pk_products primary key,
	name varchar(100) not null,
	details nvarchar(max)
)

CREATE PROC proc_InsertProduct(@prodname varchar(100), @proddetails nvarchar(max))
AS
BEGIN
	insert into Products (name, details) values (@prodname, @proddetails)
END

EXEC proc_InsertProduct 'Laptop', '{"Brand":"DELL" ,"Spec":{"Ram":"16GB", "Cpu":"i7"}}' 

select * from Products;

SELECT JSON_QUERY(details, '$.Spec') as prod_spec from products;

CREATE OR ALTER PROC proc_UpdateProductSpecRam(@pid int, @new_ram varchar(50))
AS
BEGIN
	update products set details = JSON_MODIFY(details, '$.Spec.Ram', @new_ram) where id = @pid
END

EXEC proc_UpdateProductSpecRam 1, '24GB'

select * from Products;

create table todos (
	userId int,
	id int primary key,
	title varchar(100),
	completed varchar(50)
)


 CREATE OR ALTER PROC proc_BulkInsertTodos(@jsonData varchar(max))
 AS
 BEGIN
	insert into todos (userId,id, title, completed)
	select  userId,id, title, completed from openjson(@jsonData)
	with ( userId int, id int, title varchar(100), completed varchar(50))
 END

 proc_BulkInsertTodos '[
  {
    "userId": 1,
    "id": 1,
    "title": "delectus aut autem",
    "completed": false
  },
  {
    "userId": 1,
    "id": 2,
    "title": "quis ut nam facilis et officia qui",
    "completed": false
  },
  {
    "userId": 1,
    "id": 3,
    "title": "fugiat veniam minus",
    "completed": false
  },
  {
    "userId": 1,
    "id": 4,
    "title": "et porro tempora",
    "completed": true
  },
  {
    "userId": 1,
    "id": 5,
    "title": "laboriosam mollitia et enim quasi adipisci quia provident illum",
    "completed": false
  }]'

select * from todos;

select * from products where
try_cast(json_value(details, '$.Spec.Cpu') as varchar(50)) = 'i7';

CREATE PROC proc_SelectTodoByUserId(@uid int)
AS
BEGIN
	select * from todos
	where userId = @uid;
END

EXEC proc_SelectTodoByUserId 1;
