## Database Setup

1. Run the SQL scripts in `Resources/sql/` in order:
   - `Database_01_CreateDatabaseAndUser.sql`
   - `Database_02_SeedRoles.sql`
   - `Database_03_SeedRoomsAndPrices.sql`

2. Start the application and register your first account through
   `POST /api/v1/auth/register/member`. This creates a GUEST-role
   user with an associated Member profile.

3. In the database, promote this account to ADMIN:
```sql
   UPDATE Users SET RoleId = 2 WHERE Username = '<your_username>';
```
   If this account doesn't need member-specific data (phone number,
   country code), you may also delete its row from the `Members` table.

4. Repeat steps 2–3 for any RECEPTIONIST accounts needed, using
   `RoleId = 3` instead.

> **Note:** There is currently no dedicated endpoint for creating
> ADMIN or RECEPTIONIST accounts directly — this manual role
> promotion step is a known limitation, kept simple for the scope
> of this project.