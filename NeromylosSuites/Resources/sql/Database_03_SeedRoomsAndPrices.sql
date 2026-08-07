USE NeromylosSuites;
GO

-- Rooms
INSERT INTO Rooms (RoomNumber, Name, Description, MaxOccupancy, Status, ImageUrl)
VALUES 
    (1, 'Platanos', 'Cozy standard room with garden view', 2, 'AVAILABLE', '/images/rooms/standard-room.jpg'),
    (2, 'Krini', 'Spacious superior room with creek view', 2, 'AVAILABLE', '/images/rooms/superior-room.jpg'),
    (3, 'Mylos', 'Luxury suite with private garden', 3, 'AVAILABLE', '/images/rooms/suite.jpg');
GO

-- Seasonal Prices
INSERT INTO SeasonalPrices (SeasonName, DateFrom, DateTo, Price, RoomId)
VALUES
    ('Low Season 26', '2026-05-01', '2026-09-30', 100.00, 1),
    ('Low Season 26', '2026-05-01', '2026-09-30', 130.00, 2),
    ('Low Season 26', '2026-05-01', '2026-09-30', 180.00, 3),

    ('Mid-Season2 26', '2026-10-01', '2026-11-30', 110.00, 1),
    ('Mid-Season2 26', '2026-10-01', '2026-11-30', 140.00, 2),
    ('Mid-Season2 26', '2026-10-01', '2026-11-30', 200.00, 3),

    ('High Season2 26', '2026-12-01', '2026-12-19', 120.00, 1),
    ('High Season2 26', '2026-12-01', '2026-12-19', 170.00, 2),
    ('High Season2 26', '2026-12-01', '2026-12-19', 230.00, 3),

    ('Christmas Season 26', '2026-12-20', '2027-01-07', 140.00, 1),
    ('Christmas Season 26', '2026-12-20', '2027-01-07', 190.00, 2),
    ('Christmas Season 26', '2026-12-20', '2027-01-07', 250.00, 3),

    ('High Season1 27', '2027-01-08', '2027-02-28', 120.00, 1),
    ('High Season1 27', '2027-01-08', '2027-02-28', 170.00, 2),
    ('High Season1 27', '2027-01-08', '2027-02-28', 230.00, 3),

    ('Mid-Season1 27', '2027-03-01', '2027-04-30', 110.00, 1),
    ('Mid-Season1 27', '2027-03-01', '2027-04-30', 140.00, 2),
    ('Mid-Season1 27', '2027-03-01', '2027-04-30', 200.00, 3),

    ('Low Season 27', '2027-05-01', '2027-09-30', 100.00, 1),
    ('Low Season 27', '2027-05-01', '2027-09-30', 130.00, 2),
    ('Low Season 27', '2027-05-01', '2027-09-30', 180.00, 3);
GO