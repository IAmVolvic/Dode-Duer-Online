

CREATE TABLE Users (
                       Id UUID PRIMARY KEY,
                       Name VARCHAR(255) NOT NULL,
                       Email VARCHAR(255) UNIQUE NOT NULL,
                       PasswordHash VARCHAR(255) NOT NULL,
                       Balance DECIMAL(10, 2) DEFAULT 0,
                       Role VARCHAR(255) DEFAULT 'User',
                       Status VARCHAR(255)  DEFAULT 'Active'
);

CREATE TABLE Game (
                      Id UUID PRIMARY KEY,
                      PrizePool DECIMAL(10, 2),
                      Date DATE NOT NULL,
                      WinningNumbers VARCHAR(50),
                      Status VARCHAR(255)   DEFAULT 'Active'
);

CREATE TABLE Board (
                       Id UUID PRIMARY KEY,
                       UserId UUID NOT NULL,
                       GameId UUID NOT NULL,
                       Price DECIMAL(10, 2) NOT NULL,
                       DateOfPurchase DATE NOT NULL,
                       FOREIGN KEY (UserId) REFERENCES Users(Id),
                       FOREIGN KEY (GameId) REFERENCES Game(Id)
);

CREATE TABLE Transactions (
                              Id UUID PRIMARY KEY,
                              UserId UUID NOT NULL,
                              Amount DECIMAL(10, 2) NOT NULL,
                              MobilePayID VARCHAR(255) NOT NULL,
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
