-- Create Database
CREATE DATABASE DB_Ordering_Toylo_IT13;
GO

USE DB_Ordering_Toylo_IT13;
GO

-- Table 1: Menu Items
CREATE TABLE MenuItems (
    ItemID INT PRIMARY KEY IDENTITY(1,1),
    ItemName NVARCHAR(100) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    IsAvailable BIT DEFAULT 1,
    DateAdded DATETIME DEFAULT GETDATE()
);

-- Table 2: Orders
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    CustomerName NVARCHAR(100) NOT NULL,
    TableNumber INT NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(10,2) NOT NULL,
    OrderStatus NVARCHAR(50) DEFAULT 'Pending'
);

-- Table 3: Order Details
CREATE TABLE OrderDetails (
    OrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID) ON DELETE CASCADE,
    ItemID INT FOREIGN KEY REFERENCES MenuItems(ItemID),
    ItemName NVARCHAR(100),
    Quantity INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Subtotal DECIMAL(10,2) NOT NULL
);

-- Insert Sample Menu Items
INSERT INTO MenuItems (ItemName, Category, Price, IsAvailable) VALUES
('Cheeseburger', 'Main Course', 150.00, 1),
('Fried Chicken', 'Main Course', 180.00, 1),
('Caesar Salad', 'Appetizer', 120.00, 1),
('Spaghetti Carbonara', 'Main Course', 200.00, 1),
('French Fries', 'Sides', 80.00, 1),
('Coca Cola', 'Beverages', 50.00, 1),
('Iced Tea', 'Beverages', 45.00, 1),
('Chocolate Cake', 'Dessert', 100.00, 1);

-- Sample Orders
INSERT INTO Orders (CustomerName, TableNumber, TotalAmount, OrderStatus) VALUES
('Juan Dela Cruz', 5, 430.00, 'Completed'),
('Maria Santos', 3, 280.00, 'Pending');

INSERT INTO OrderDetails (OrderID, ItemID, ItemName, Quantity, Price, Subtotal) VALUES
(1, 1, 'Cheeseburger', 2, 150.00, 300.00),
(1, 6, 'Coca Cola', 2, 50.00, 100.00),
(1, 5, 'French Fries', 1, 80.00, 80.00);