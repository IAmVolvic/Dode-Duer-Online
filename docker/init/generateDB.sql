CREATE TABLE Users (
                       Id UUID PRIMARY KEY,
                       Name VARCHAR(255) NOT NULL,
                       Email VARCHAR(255) UNIQUE NOT NULL,
                       PhoneNumber VARCHAR(255) NOT NULL,
                       PasswordHash VARCHAR(255) NOT NULL,
                       Enrolled VARCHAR(255) DEFAULT 'False',
                       Balance DECIMAL(10, 2) DEFAULT 0,
                       Role VARCHAR(255) DEFAULT 'User',
                       Status VARCHAR(255)  DEFAULT 'Active'
);

CREATE TABLE Prices (
                        Id UUID PRIMARY KEY ,
                        Price DECIMAL(10, 2) NOT NULL,
                        Numbers DECIMAL(10, 2) NOT NULL
);

create table game
(
    id                uuid not null
        primary key,
    prizepool         numeric(10, 2),
    date              date not null,
    status            varchar(255) default 'Active'::character varying,
    enddate           timestamp,
    startingprizepool numeric(10, 2)
);
CREATE TABLE Board (
                       Id UUID PRIMARY KEY,
                       UserId UUID NOT NULL,
                       GameId UUID NOT NULL,
                       PriceId UUID NOT NULL,
                       DateOfPurchase DATE NOT NULL,
                       FOREIGN KEY (UserId) REFERENCES Users(Id),
                       FOREIGN KEY (GameId) REFERENCES Game(Id),
                       FOREIGN KEY (PriceId) REFERENCES Prices(Id)
);

CREATE TABLE Transactions (
                              Id UUID PRIMARY KEY,
                              UserId UUID NOT NULL,
                              TransactionNumber VARCHAR(255) NOT NULL, /*MobilePay transaction number*/
                              TransactionStatus VARCHAR(255) DEFAULT 'Pending',
                              FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE ChosenNumbers (
                               Id UUID PRIMARY KEY,
                               BoardId UUID NOT NULL,
                               Number INT DEFAULT 0,
                               FOREIGN KEY (BoardId) REFERENCES Board(Id)
);

CREATE TABLE Winners (
                         Id UUID PRIMARY KEY,
                         GameId UUID NOT NULL,
                         UserId UUID NOT NULL,
                         WonAmount DECIMAL(10, 2) NOT NULL,
                         FOREIGN KEY (GameId) REFERENCES Game(Id),
                         FOREIGN KEY (UserId) REFERENCES Users(Id)
);
create table WinningNumbers (
                                Id uuid primary key not null,
                                GameId uuid not null,
                                Number integer default 0,
                                foreign key (GameId) references Game(id)
);

CREATE TABLE BoardAutoplay (
                               Id UUID PRIMARY KEY,
                               UserId UUID NOT NULL,
                               LeftToPlay INT NOT NULL,
                               FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE ChosenNumbersAutoplay (
                               Id UUID PRIMARY KEY,
                               BoardId UUID NOT NULL,
                               Number INT DEFAULT 0,
                               FOREIGN KEY (BoardId) REFERENCES BoardAutoplay(Id) ON DELETE CASCADE 
);

