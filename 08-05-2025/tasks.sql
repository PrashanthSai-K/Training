use sample;

-- List all orders with the customer name and the employee who handled the order.

select * from orders;

select * from Employees;

select * from Customers;

select o.OrderId, o.OrderDate, c.CompanyName as customer, concat(e.FirstName, ' ', e.LastName) as employee from orders o
join Customers c on c.CustomerID = o.CustomerID
join Employees e on e.EmployeeID = o.EmployeeID

-- Get a list of products along with their category and supplier name.

select * from products;

select * from categories;

select * from suppliers;

select p.ProductId, p.ProductName, p.UnitPrice, c.CategoryName, s.CompanyName as SupplierName 
from products p
join categories c on c.CategoryID = p.CategoryID
join suppliers s on s.SupplierID = p.SupplierID

-- Show all orders and the products included in each order with quantity and unit price.

select * from orders;

select * from [Order Details];

select * from products;

select o.OrderId,p.ProductName, od.quantity, od.UnitPrice  from [Order Details] od
join orders o on od.OrderID = o.OrderID
join products p on p.ProductID = od.ProductID

-- List employees who report to other employees (manager-subordinate relationship).

select * from Employees;

select e1.EmployeeId, concat(e1.FirstName, ' ', e1.LastName) as EmployeeName, concat(e2.FirstName, ' ', e2.LastName) as ReportingToName from employees e1
left outer join Employees e2 on e1.ReportsTo = e2.EmployeeID
order by ReportingToName

-- Display each customer and their total order count

select * from orders order by CustomerID;

select * from Customers;

select o.CustomerId, COUNT(o.CustomerId) TotalOrderCount, c.companyName from orders o
join customers c on c.CustomerID = o.CustomerID
group by o.CustomerID, c.companyName

-- Find the average unit price of products per category.

select * from Categories;

select p.CategoryId, c.CategoryName, avg(p.UnitPrice) AveragePrice from Products p
join Categories c on p.CategoryID = c.CategoryID
group by p.CategoryID, c.CategoryName;

-- List customers where the contact title starts with 'Owner'.

select * from customers 
where ContactTitle like 'Owner';

-- Show the top 5 most expensive products.

select top 5 * from products order by UnitPrice desc;

-- Return the total sales amount (quantity × unit price) per order.

select OrderID, SUM(UnitPrice * Quantity) as TotalOrderPrice from [Order Details]
group by OrderID

-- Create a stored procedure that returns all orders for a given customer ID.

select * from orders;

create or alter proc proc_GetOrderByCustomerId(@custid varchar(50))
as
begin
	select * from orders where CustomerID = @custid
end

exec proc_GetOrderByCustomerId 'TOMSP';

-- Write a stored procedure that inserts a new product.

select * from products;

create or alter proc proc_InsertNewProduct(@ProductName varchar(50), @SupplierId int, @CategoryId int, @QtyPerUnit varchar(100), @UnitPrice float, @UnitsInStock int, @UnitsOnOrder int, @ReorderLevel int, @Discontinued int)
as
begin
	insert into products (ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued)
	values (@ProductName, @SupplierId, @CategoryId, @QtyPerUnit, @UnitPrice, @UnitsInStock, @UnitsOnOrder, @ReorderLevel, @Discontinued)
end

exec proc_InsertNewProduct 'Mosambi Juice', 1, 1, '250 ml bottles', 95.00, 100, 0, 30, 0 

select * from products where ProductName = 'Mosambi Juice';


-- Create a stored procedure that returns total sales per employee.

create or alter proc proc_TotalSalesPerEmployee
as
begin
	select o.EmployeeID, concat(e.FirstName, ' ',e.LastName) EmployeeName, sum(od.Quantity * od.UnitPrice) as TotalSalesPerEmployee from orders o
	join [Order Details] od on o.OrderID = od.OrderID
	join employees e on e.EmployeeID = o.EmployeeID
	group by o.EmployeeID, e.FirstName, e.LastName
	order by TotalSalesPerEmployee Desc
end

exec proc_TotalSalesPerEmployee


--  Use a CTE to rank products by unit price within each category.

select * from products order by CategoryID, unitprice;

with cte_RankProducts as (
	select 
		p.ProductName, 
		p.CategoryID,
		c.CategoryName,
		ROW_NUMBER() OVER (PARTITION BY p.CategoryID ORDER BY p.UnitPrice ) Rank
	from Products p
	join Categories c on c.CategoryID = p.CategoryID
)

select * from cte_RankProducts;
 
 -- Create a CTE to calculate total revenue per product and filter products with revenue > 10,000.

 select * from products;

 select * from [Order Details];

 with cte_CalculateRevenueByProduct as
 (
	 select od.ProductID, p.ProductName, sum(od.UnitPrice * od.Quantity) as TotalRevenueByProduct from [Order Details] od
	 join Products p on p.ProductID = od.ProductID
	 group by od.ProductID, p.ProductName
 )

select * from cte_CalculateRevenueByProduct 
where TotalRevenueByProduct > 10000
order by TotalRevenueByProduct desc;

-- Use a CTE with recursion to display employee hierarchy

with cte_EmployeeHierarchy AS
(
	select
		e1.EmployeeID,
		concat(e1.FirstName, ' ',e1.LastName) as EmployeeName,
		e1.ReportsTo,
		0 as Level
	from Employees e1
	where e1.ReportsTo is null

	union all

	select
		e2.EmployeeID,
		concat(e2.FirstName, ' ',e2.LastName) as EmployeeName,
		e2.ReportsTo,
		e1.Level + 1
	from Employees e2
	inner join cte_EmployeeHierarchy e1 on e2.ReportsTo = e1.EmployeeID
)

select * from cte_EmployeeHierarchy;
