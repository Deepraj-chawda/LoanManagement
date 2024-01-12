CREATE DATABASE LoanManagement;

USE LoanManagement; 


-- Customer table
CREATE TABLE Customer (
    customer_id INT PRIMARY KEY,
    name VARCHAR(255),
    email_address VARCHAR(255),
    phone_number VARCHAR(15),
    address VARCHAR(255),
    credit_score INT
);

-- Loan table (base class)
CREATE TABLE Loan (
    loan_id INT PRIMARY KEY,
    customer_id INT,
    principal_amount DECIMAL(10, 2),
    interest_rate DECIMAL(5, 2),
    loan_term_months INT,
    loan_type VARCHAR(50),
    loan_status VARCHAR(50),
    FOREIGN KEY (customer_id) REFERENCES Customer(customer_id)
);

-- HomeLoan table (subclass of Loan)
CREATE TABLE HomeLoan (
    loan_id INT PRIMARY KEY,
    property_address VARCHAR(255),
    property_value INT,
    FOREIGN KEY (loan_id) REFERENCES Loan(loan_id)
);

-- CarLoan table (subclass of Loan)
CREATE TABLE CarLoan (
    loan_id INT PRIMARY KEY,
    car_model VARCHAR(255),
    car_value INT,
    FOREIGN KEY (loan_id) REFERENCES Loan(loan_id)
);

-- Inserting data into Customer table
INSERT INTO Customer (customer_id, name, email_address, phone_number, address, credit_score)
VALUES
    (1, 'Rahul Sharma', 'rahul@gmail.com', '+91-9876543210', '123 Main Street, City', 750),
    (2, 'Priya Patel', 'priya@gmail.com', '+91-8765432109', '456 Park Avenue, Town', 800),
    (3, 'Raj Singh', 'raj@gmail.com', '+91-7654321098', '789 Street Road, Village', 700),
    (4, 'Ananya Gupta', 'ananya@gmail.com', '+91-6543210987', '321 Central Square, District', 820),
    (5, 'Amit Kumar', 'amit@gmail.com', '+91-5432109876', '654 Downtown Avenue, City', 680);

-- Inserting data into Loan table
INSERT INTO Loan (loan_id, customer_id, principal_amount, interest_rate, loan_term_months, loan_type, loan_status)
VALUES
    (101, 1, 500000, 8.5, 36, 'HomeLoan', 'Pending'),
    (102, 2, 300000, 7.2, 24, 'CarLoan', 'Approved'),
    (103, 3, 800000, 9.0, 48, 'HomeLoan', 'Pending'),
    (104, 4, 400000, 6.5, 36, 'CarLoan', 'Approved'),
    (105, 5, 600000, 8.0, 60, 'HomeLoan', 'Pending');

-- Inserting data into HomeLoan table
INSERT INTO HomeLoan (loan_id, property_address, property_value)
VALUES
    (101, '456 Green Gardens, City', 1200000),
    (103, '789 Serene Homes, Town', 2000000),
    (105, '654 Lakeside Villa, District', 1800000);

-- Inserting data into CarLoan table
INSERT INTO CarLoan (loan_id, car_model, car_value)
VALUES
    (102, 'Toyota Camry', 2500000),
    (104, 'Honda Civic', 1800000);


select * from Customer;
select * from Loan;
select * from HomeLoan;
select * from CarLoan;