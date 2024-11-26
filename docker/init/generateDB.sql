CREATE TABLE Users (
                       Id UUID PRIMARY KEY,
                       Name VARCHAR(255) NOT NULL,
                       Email VARCHAR(255) UNIQUE NOT NULL,
                       PhoneNumber VARCHAR(255) NOT NULL,
                       PasswordHash VARCHAR(255) NOT NULL,
                       Enrolled enrollment_status DEFAULT 'false',
                       Balance DECIMAL(10, 2) DEFAULT 0,
                       Role user_roles DEFAULT 'user',
                       Status user_status  DEFAULT 'active'
);
CREATE TYPE enrollment_status AS ENUM ('true', 'false');
CREATE TYPE user_status AS ENUM ('active', 'inactive');
CREATE TYPE user_roles AS ENUM ('user', 'admin');

CREATE TABLE Prices (
                        Id UUID PRIMARY KEY ,
                        Price DECIMAL(10, 2) NOT NULL,
                        Numbers DECIMAL(10, 2) NOT NULL
);

CREATE TABLE Game (
                      Id UUID PRIMARY KEY,
                      PrizePool DECIMAL(10, 2),
                      Date DATE NOT NULL,
                      WinningNumbers VARCHAR(50),
                      Status game_status   DEFAULT 'active'
);
CREATE TYPE game_status AS ENUM ('active', 'inactive');

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
                              TransactionStatus transaction_status DEFAULT 'pending',
                              FOREIGN KEY (UserId) REFERENCES Users(Id)
);
CREATE TYPE transaction_status AS ENUM ('pending', 'approved', 'rejected');

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


