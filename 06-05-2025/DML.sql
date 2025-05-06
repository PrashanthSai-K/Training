CREATE TABLE emp  (
	empno int primary key ,
	empname varchar(100) not null,
	salary decimal(10, 2) not null,
	deptname varchar(50),
	bossno int,
	FOREIGN KEY (bossno) REFERENCES emp(empno)
);

CREATE TABLE department (
	deptname varchar(50) primary key,
	floor int, 
	phone varchar(50),
	mgrid int not null,
	FOREIGN KEY (mgrid) REFERENCES emp(empno)
);

ALTER TABLE emp
ADD FOREIGN KEY (deptname) REFERENCES department(deptname);

CREATE TABLE item (
	itemname varchar(50) primary key,
	itemtype varchar(50) not null,
	itemcolor varchar(50) not null
);

CREATE TABLE sales (
	salesno int primary key ,
	saleqty int not null,
	itemname varchar(50) not null,
	deptname varchar(50) not null,
	FOREIGN KEY (itemname) REFERENCES item(itemname),
	FOREIGN KEY (deptname) REFERENCES department
);

INSERT INTO emp (empno, empname, salary, deptname, bossno) VALUES
	('1', 'Alice', '75000', null, null),
	('2', 'Ned', '45000', null, '1'),
	('3', 'Andrew', '25000', null, '2'),
	('4', 'Clare', '22000', null, '2'),
	('5', 'Todd', '38000', null, '1'),
	('6', 'Nancy', '22000', null, '5'),
	('7', 'Brier', '43000', null, '1'),
	('8', 'Sarah', '56000', null, '7'),
	('9', 'Sophile', '35000', null, '1'),
	('10', 'Sanjay', '15000', null, '3'),
	('11', 'Rita', '15000', null, '4'),
	('12', 'Gigi', '16000', null, '4'),
	('13', 'Maggie', '11000', null, '4'),
	('14', 'Paul', '15000', null, '3'),
	('15', 'James', '15000', null, '3'),
	('16', 'Pat', '15000', null, '3'),
	('17', 'Mark', '15000', null, '3');

INSERT INTO department (deptname, floor, phone, mgrid) VALUES
	('Management', '5', '34', '1'),
	('Books', '1', '81', '4'),
	('Clothes', '2', '24', '4'),
	('Equipment', '3', '57', '3'),
	('Furniture', '4', '14', '3'),
	('Navigation', '1', '41', '3'),
	('Recreation', '2', '29', '4'),
	('Accounting', '5', '35', '5'),
	('Purchasing', '5', '36', '7'),
	('Personnel', '5', '37', '9'),
	('Marketing', '5', '38', '2');

UPDATE emp SET deptname = 'Management' WHERE empno IN (1);
UPDATE emp SET deptname = 'Marketing' WHERE empno IN (2, 3, 4);
UPDATE emp SET deptname = 'Accounting' WHERE empno IN (5, 6);
UPDATE emp SET deptname = 'Purchasing' WHERE empno IN (7, 8);
UPDATE emp SET deptname = 'Personnel' WHERE empno IN (9);
UPDATE emp SET deptname = 'Navigation' WHERE empno IN (10);
UPDATE emp SET deptname = 'Books' WHERE empno IN (11);
UPDATE emp SET deptname = 'Clothes' WHERE empno IN (12, 13);
UPDATE emp SET deptname = 'Equipment' WHERE empno IN (14, 15);
UPDATE emp SET deptname = 'Furniture' WHERE empno IN (16);
UPDATE emp SET deptname = 'Recreation' WHERE empno IN (17);

INSERT INTO item (itemname, itemtype, itemcolor) VALUES
	('Pocket Knife-Nile', 'E', 'Brown'),
	('Pocket Knife-Avon', 'E', 'Brown'),
	('Compass', 'N', ''),
	('Geo positioning system', 'N', ''),
	('Elephant Polo stick', 'R', 'Bamboo'),
	('Camel Saddle', 'R', 'Brown'),
	('Sextant', 'N', ''),
	('Map Measure', 'N', ''),
	('Boots-snake proof', 'C', 'Green'),
	('Pith Helmet', 'C', 'Khaki'),
	('Hat-polar Explorer', 'C', 'White'),
	('Exploring in 10 Easy Lessons', 'B', ''),
	('Hammock', 'F', 'Khaki'),
	('How to win Foreign Friends', 'B', ''),
	('Map case', 'E', 'Brown'),
	('Safari Chair', 'F', 'Khaki'),
	('Safari cooking kit', 'F', 'Khaki'),
	('Stetson', 'C', 'Black'),
	('Tent - 2 person', 'F', 'Khaki'),
	('Tent - 8 person', 'F', 'Khaki');

INSERT INTO sales (salesno, saleqty, itemname, deptname) VALUES
	('101', '2', 'Boots-snake proof', 'Clothes'),
	('102', '1', 'Pith Helmet', 'Clothes'),
	('103', '1', 'Sextant', 'Navigation'),
	('104', '3', 'Hat-polar Explorer', 'Clothes'),
	('105', '5', 'Pith Helmet', 'Equipment'),
	('106', '2', 'Pocket Knife-Nile', 'Clothes'),
	('107', '3', 'Pocket Knife-Nile', 'Recreation'),
	('108', '1', 'Compass', 'Navigation'),
	('109', '2', 'Geo positioning system', 'Navigation'),
	('110', '5', 'Map Measure', 'Navigation'),
	('111', '1', 'Geo positioning system', 'Books'),
	('112', '1', 'Sextant', 'Books'),
	('113', '3', 'Pocket Knife-Nile', 'Books'),
	('114', '1', 'Pocket Knife-Nile', 'Navigation'),
	('115', '1', 'Pocket Knife-Nile', 'Equipment'),
	('116', '1', 'Sextant', 'Clothes'),
	('117', '1', 'Sextant', 'Equipment'),
	('118', '1', 'Sextant', 'Recreation'),
	('119', '1', 'Sextant', 'Furniture'),
	('120', '1', 'Pocket Knife-Nile', 'Furniture'),
	('121', '1', 'Exploring in 10 easy lessons', 'Books'),
	('122', '1', 'How to win foreign friends', 'Books'),
	('123', '1', 'Compass', 'Books'),
	('124', '1', 'Pith Helmet', 'Books'),
	('125', '1', 'Elephant Polo stick', 'Recreation'),
	('126', '1', 'Camel Saddle', 'Recreation');


-- All data inserted and relationships are working perfectly. checked with following query,

select 
	sales.salesno, item.itemname, item.itemtype, item.itemcolor, department.deptname, department.floor, emp.empname 
from sales 
join item on sales.itemname = item.itemname
join department on sales.deptname = department.deptname
join emp on department.mgrid = emp.empno
