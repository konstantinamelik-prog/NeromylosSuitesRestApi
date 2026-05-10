USE [NeromylosSuitesDockerDB];
GO

INSERT INTO [dbo].[Roles]([Name], [Description])
VALUES 
	('ADMIN', 'Owns or manages the hotel. Full access to everything.'),
	('GUEST', 'Whoever created an account as a Member. Can have access to his/her own bookings.');
GO