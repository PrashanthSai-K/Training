CREATE TABLE Categories (
	Id     int          PRIMARY KEY,
	Name   varchar(200),
	Status varchar(100)
);

CREATE TABLE Country (
	Id     int          PRIMARY KEY,
	Name   varchar(200),
);
CREATE TABLE State (
	Id 	int 		PRIMARY KEY,
	Name    varchar(200),
	Country_Id int,
	FOREIGN KEY (Country_Id) REFEENCES Country(Id)
);
CREATE TABLE City (
	Id	 int	PRIMARY KEY,
	Name	 varchar(200),
	State_Id int,
	FOREIGN KEY (State_id) REFERENCES State(Id)
);
CREATE TABLE Area (
	Zipcode int PRIMARY KEY,
	Name varchar(200),
	City_Id int,
	FOREIGN KEY (City_Id) REFERENCES City(Id)
);
CREATE TABLE Address (
	Id	int	PRIMARY KEY,
	DoorNumber varchar(50),
	AddressLine1	Varchar(200),
	Zipcode int,
	FOREIGN KEY (Zipcode) REFERENCES Area(Zipcode)
);
CREATE TABLE supplier (
    Id INT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Contact_person VARCHAR(100),
    Phone VARCHAR(20),
    Email VARCHAR(100),
    Address_id INT NOT NULL,
    Status VARCHAR(20) ,
    FOREIGN KEY (Address_id) REFERENCES address(Id)
);

CREATE TABLE Product (
    Id INT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Unit_price DECIMAL(10, 2) NOT NULL,
    Quantity INT NOT NULL,
    Description TEXT,
    Image VARCHAR(255)
);

CREATE TABLE Product_Supplier (
    Transaction_id INT PRIMARY KEY,
    Product_id INT NOT NULL,
    Supplier_id INT NOT NULL,
    Date_of_supply DATE NOT NULL,
    Quantity INT NOT NULL,
    FOREIGN KEY (Product_id) REFERENCES Product(Id),
    FOREIGN KEY (Supplier_id) REFERENCES Supplier(Id)
);

CREATE TABLE Customer (
    Id INT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Phone VARCHAR(20),
    Age INT,
    Address_id INT NOT NULL,
    FOREIGN KEY (Address_id) REFERENCES Address(Id)
);

CREATE TABLE orders (
    Order_number INT PRIMARY KEY,
    Customer_id INT NOT NULL,
    Date_of_order DATE NOT NULL,
    Amount DECIMAL(10, 2) NOT NULL,
    Order_status VARCHAR(20),
    FOREIGN KEY (Customer_id) REFERENCES Customer(Id)
);

CREATE TABLE Order_details (
    Id INT PRIMARY KEY,
    Order_number INT NOT NULL,
    Product_id INT NOT NULL,
    Quantity INT NOT NULL,
    Unit_price DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (Order_number) REFERENCES Orders(Order_number),
    FOREIGN KEY (Product_id) REFERENCES Product(Id)
);

