-- Δημιουργεί τη βάση με Greek collation (CI_AI: case-insensitive, accent-insensitive)
-- Απαραίτητο για σωστή λειτουργία του lastname search (.Contains()) σε Bookings/Members
CREATE DATABASE NeromylosSuites COLLATE Greek_100_CI_AI;
GO

CREATE LOGIN neromylosadmin WITH PASSWORD = 'N3r0myl0sSu!t3s';
GO

USE NeromylosSuites;
GO

CREATE USER neromylosadmin FOR LOGIN neromylosadmin;
ALTER ROLE db_owner ADD MEMBER neromylosadmin;
GO