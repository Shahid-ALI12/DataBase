
CREATE DATABASE AIRLINE;
GO
USE AIRLINE;
GO


IF OBJECT_ID('NOTIFICATION',  'U') IS NOT NULL DROP TABLE NOTIFICATION;
IF OBJECT_ID('CANCELLATION',  'U') IS NOT NULL DROP TABLE CANCELLATION;
IF OBJECT_ID('TICKET',        'U') IS NOT NULL DROP TABLE TICKET;
IF OBJECT_ID('PAYMENT',       'U') IS NOT NULL DROP TABLE PAYMENT;
IF OBJECT_ID('BOOKING',       'U') IS NOT NULL DROP TABLE BOOKING;
IF OBJECT_ID('SEAT',          'U') IS NOT NULL DROP TABLE SEAT;
IF OBJECT_ID('FLIGHT',        'U') IS NOT NULL DROP TABLE FLIGHT;
IF OBJECT_ID('STAFF',         'U') IS NOT NULL DROP TABLE STAFF;
IF OBJECT_ID('ROLE',          'U') IS NOT NULL DROP TABLE ROLE;
IF OBJECT_ID('ADMIN',         'U') IS NOT NULL DROP TABLE ADMIN;
IF OBJECT_ID('PASSENGER',     'U') IS NOT NULL DROP TABLE PASSENGER;
IF OBJECT_ID('PAYMENT_METHOD','U') IS NOT NULL DROP TABLE PAYMENT_METHOD;
IF OBJECT_ID('USERS',         'U') IS NOT NULL DROP TABLE USERS;


CREATE TABLE USERS (
    UserID    VARCHAR(10)  NOT NULL,
    Name      VARCHAR(100) NOT NULL,
    Email     VARCHAR(150) NOT NULL,
    CONSTRAINT PK_USERS        PRIMARY KEY (UserID),
    CONSTRAINT UQ_USERS_EMAIL  UNIQUE      (Email)
);

-- ============================================================
-- 2. PASSENGER
-- ============================================================
CREATE TABLE PASSENGER (
    PassengerID VARCHAR(10) NOT NULL,
    UserID      VARCHAR(10) NOT NULL,
    CNIC        VARCHAR(15) NOT NULL,
    Phone       VARCHAR(15) NOT NULL,
    CONSTRAINT PK_PASSENGER      PRIMARY KEY (PassengerID),
    CONSTRAINT UQ_PASSENGER_CNIC UNIQUE      (CNIC),
    CONSTRAINT FK_PASSENGER_USER FOREIGN KEY (UserID)
        REFERENCES USERS(UserID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

-- ============================================================
-- 3. ADMIN
-- ============================================================
CREATE TABLE ADMIN (
    AdminID  VARCHAR(10)  NOT NULL,
    UserID   VARCHAR(10)  NOT NULL,
    Username VARCHAR(50)  NOT NULL,
    Password VARCHAR(255) NOT NULL,
    CONSTRAINT PK_ADMIN          PRIMARY KEY (AdminID),
    CONSTRAINT UQ_ADMIN_USERNAME UNIQUE      (Username),
    CONSTRAINT FK_ADMIN_USER     FOREIGN KEY (UserID)
        REFERENCES USERS(UserID)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
);

-- ============================================================
-- 4. FLIGHT
-- ============================================================
CREATE TABLE FLIGHT (
    FlightID      VARCHAR(10)  NOT NULL,
    FlightName    VARCHAR(100) NOT NULL,
    DepartureCity VARCHAR(100) NOT NULL,
    ArrivalCity   VARCHAR(100) NOT NULL,
    CONSTRAINT PK_FLIGHT PRIMARY KEY (FlightID)
);

-- ============================================================
-- 5. SEAT
-- ============================================================
CREATE TABLE SEAT (
    SeatID     VARCHAR(10) NOT NULL,
    FlightID   VARCHAR(10) NOT NULL,
    SeatNo     VARCHAR(5)  NOT NULL,
    SeatStatus VARCHAR(10) NOT NULL DEFAULT 'Available',
    CONSTRAINT PK_SEAT        PRIMARY KEY (SeatID),
    CONSTRAINT UQ_SEAT_FLIGHT UNIQUE      (FlightID, SeatNo),
    CONSTRAINT FK_SEAT_FLIGHT FOREIGN KEY (FlightID)
        REFERENCES FLIGHT(FlightID)
        ON DELETE CASCADE
        ON UPDATE NO ACTION
);

-- ============================================================
-- 6. BOOKING
-- ============================================================
CREATE TABLE BOOKING (
    BookingID     VARCHAR(10) NOT NULL,
    PassengerID   VARCHAR(10) NOT NULL,
    FlightID      VARCHAR(10) NOT NULL,
    SeatID        VARCHAR(10) NOT NULL,
    BookingDate   DATE        NOT NULL DEFAULT GETDATE(),
    BookingStatus VARCHAR(10) NOT NULL DEFAULT 'Pending',
    CONSTRAINT PK_BOOKING          PRIMARY KEY (BookingID),
    CONSTRAINT FK_BOOKING_PASSENGER FOREIGN KEY (PassengerID)
        REFERENCES PASSENGER(PassengerID)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    CONSTRAINT FK_BOOKING_FLIGHT   FOREIGN KEY (FlightID)
        REFERENCES FLIGHT(FlightID)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    CONSTRAINT FK_BOOKING_SEAT     FOREIGN KEY (SeatID)
        REFERENCES SEAT(SeatID)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
);

-- ============================================================
-- 7. PAYMENT_METHOD
-- ============================================================
CREATE TABLE PAYMENT_METHOD (
    PaymentMethodID VARCHAR(10) NOT NULL,
    MethodName      VARCHAR(50) NOT NULL,
    CONSTRAINT PK_PAYMENT_METHOD PRIMARY KEY (PaymentMethodID),
    CONSTRAINT UQ_PAYMENT_METHOD UNIQUE      (MethodName)
);

-- ============================================================
-- 8. PAYMENT
-- ============================================================
CREATE TABLE PAYMENT (
    PaymentID       VARCHAR(10)   NOT NULL,
    BookingID       VARCHAR(10)   NOT NULL,
    Amount          DECIMAL(10,2) NOT NULL,
    PaymentMethodID VARCHAR(10)   NOT NULL,
    PaymentDate     DATE          NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_PAYMENT        PRIMARY KEY (PaymentID),
    CONSTRAINT UQ_PAYMENT_BOOKING UNIQUE     (BookingID),
    CONSTRAINT FK_PAYMENT_BOOKING FOREIGN KEY (BookingID)
        REFERENCES BOOKING(BookingID)
        ON DELETE CASCADE
        ON UPDATE NO ACTION,
    CONSTRAINT FK_PAYMENT_METHOD  FOREIGN KEY (PaymentMethodID)
        REFERENCES PAYMENT_METHOD(PaymentMethodID)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
);

-- ============================================================
-- 9. TICKET
-- ============================================================
CREATE TABLE TICKET (
    TicketID     VARCHAR(10) NOT NULL,
    BookingID    VARCHAR(10) NOT NULL,
    TicketNumber VARCHAR(20) NOT NULL,
    IssueDate    DATE        NOT NULL,
    CONSTRAINT PK_TICKET         PRIMARY KEY (TicketID),
    CONSTRAINT UQ_TICKET_BOOKING UNIQUE      (BookingID),
    CONSTRAINT UQ_TICKET_NUMBER  UNIQUE      (TicketNumber),
    CONSTRAINT FK_TICKET_BOOKING FOREIGN KEY (BookingID)
        REFERENCES BOOKING(BookingID)
        ON DELETE CASCADE
        ON UPDATE NO ACTION
);

-- ============================================================
-- 10. CANCELLATION
-- ============================================================
CREATE TABLE CANCELLATION (
    CancellationID   VARCHAR(10)   NOT NULL,
    BookingID        VARCHAR(10)   NOT NULL,
    CancellationDate DATE          NOT NULL,
    RefundAmount     DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    CONSTRAINT PK_CANCELLATION         PRIMARY KEY (CancellationID),
    CONSTRAINT UQ_CANCELLATION_BOOKING UNIQUE      (BookingID),
    CONSTRAINT FK_CANCELLATION_BOOKING FOREIGN KEY (BookingID)
        REFERENCES BOOKING(BookingID)
        ON DELETE CASCADE
        ON UPDATE NO ACTION
);

-- ============================================================
-- 11. NOTIFICATION
-- ============================================================
CREATE TABLE NOTIFICATION (
    NotificationID   VARCHAR(10) NOT NULL,
    PassengerID      VARCHAR(10) NOT NULL,
    Message          VARCHAR(MAX) NOT NULL,
    NotificationDate DATE         NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_NOTIFICATION          PRIMARY KEY (NotificationID),
    CONSTRAINT FK_NOTIFICATION_PASSENGER FOREIGN KEY (PassengerID)
        REFERENCES PASSENGER(PassengerID)
        ON DELETE CASCADE
        ON UPDATE NO ACTION
);

-- ============================================================
-- 12. ROLE
-- ============================================================
CREATE TABLE ROLE (
    RoleID   VARCHAR(10)   NOT NULL,
    RoleName VARCHAR(50)   NOT NULL,
    Salary   DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    CONSTRAINT PK_ROLE      PRIMARY KEY (RoleID),
    CONSTRAINT UQ_ROLE_NAME UNIQUE      (RoleName)
);

-- ============================================================
-- 13. STAFF
-- ============================================================
CREATE TABLE STAFF (
    StaffID VARCHAR(10)  NOT NULL,
    Name    VARCHAR(100) NOT NULL,
    RoleID  VARCHAR(10)  NOT NULL,
    CONSTRAINT PK_STAFF      PRIMARY KEY (StaffID),
    CONSTRAINT FK_STAFF_ROLE FOREIGN KEY (RoleID)
        REFERENCES ROLE(RoleID)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
);

select * From Users;