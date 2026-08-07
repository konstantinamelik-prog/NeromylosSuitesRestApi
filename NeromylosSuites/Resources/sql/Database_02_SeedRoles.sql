USE NeromylosSuites;
GO

INSERT INTO Roles (Name, Description)
VALUES 
    ('GUEST', 'Guest user (default role for new member signups)'),
    ('ADMIN', 'Full system access'),
    ('RECEPTIONIST', 'Front-desk staff access');
GO