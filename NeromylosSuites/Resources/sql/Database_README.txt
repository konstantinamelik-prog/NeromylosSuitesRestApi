# NeromylosSuites — Backend (REST API)

ASP.NET Core Web API για σύστημα κρατήσεων boutique ξενοδοχείου. Layered
architecture (Repository / Service / Controller), JWT authentication με
role-based authorization, soft delete pattern, seasonal pricing.

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 10 SDK](https://dotnet.microsoft.com/download) (χρειάζεται για τα EF Core migrations)
- Ένα SQL client για να τρέξεις τα setup scripts (π.χ. [DBeaver](https://dbeaver.io/), Azure Data Studio, SSMS)

## Build & Deploy

### 1. Clone το repository

```bash
git clone https://github.com/konstantinamelik-prog/NeromylosSuitesRestApi.git
cd NeromylosSuitesRestApi
```

### 2. Δημιούργησε το `.env` αρχείο

Αντίγραψε το `.env.example` σε `.env` και συμπλήρωσε τις τιμές:

```bash
cp .env.example .env
```

```env
JWT_ISSUER=https://localhost:8081
JWT_AUDIENCE=https://localhost:8081
JWT_SECRET=<βάλε ένα δικό σου, τυχαίο, μεγάλο secret string>
CORS_ORIGINS=http://localhost:5173,http://localhost:5174
SA_PASSWORD=<password για τον SQL Server sa χρήστη>
DB_HOST=sqlserver
DB_PORT=1438
DB_NAME=NeromylosSuites
DB_USER=neromylosadmin
DB_USER_PASSWORD=<password για τον neromylosadmin χρήστη>
ASPNETCORE_ENVIRONMENT=Development
APP_PORT=8081
```

> **Σημαντικό:** Το `SA_PASSWORD` πρέπει να πληροί τις απαιτήσεις
> πολυπλοκότητας του SQL Server (τουλάχιστον 8 χαρακτήρες, με κεφαλαία,
> πεζά, αριθμό και σύμβολο) — αλλιώς ο SQL Server container θα αποτύχει
> να ξεκινήσει.
>
> **Σημαντικό:** Το `DB_USER_PASSWORD` που βάζεις εδώ πρέπει να είναι
> **ακριβώς το ίδιο** με το password που χρησιμοποιείται στο SQL script
> `Resources/sql/Database_01_CreateDatabaseAndUser.sql` (γραμμή
> `CREATE LOGIN neromylosadmin WITH PASSWORD = '...'`). Αν διαφέρουν, η
> εφαρμογή δεν θα μπορεί να συνδεθεί στη βάση (`Login failed for user
> 'neromylosadmin'`). Είτε άλλαξε το password μέσα στο SQL script ώστε
> να ταιριάζει με το δικό σου `.env`, είτε αντίγραψε αυτό που ήδη υπάρχει
> στο script στο `.env` σου.

### 3. Ξεκίνα τα containers

```bash
docker compose up -d --build
```

Αυτό ξεκινά δύο containers: τον SQL Server (`neromylossuites-db`) και το
API (`neromyloswebmodelfirstrestapi`). Το API περιμένει αυτόματα να γίνει
healthy ο SQL Server πριν ξεκινήσει.

> Η πρώτη εκκίνηση του SQL Server container μπορεί να πάρει 1-2 λεπτά
> (αρχικοποίηση system databases). Αν το `docker compose up` αποτύχει με
> "dependency failed to start: container ... is unhealthy", περίμενε
> λίγα λεπτά και ξαναπροσπάθησε.

### 4. Δημιούργησε τη βάση και τον χρήστη

Συνδέσου στη βάση μέσω του SQL client σου (`localhost`, port από το
`DB_PORT` στο `.env`, χρήστης `sa`) και τρέξε:

- `Resources/sql/Database_01_CreateDatabaseAndUser.sql` — δημιουργεί τη
  βάση `NeromylosSuites` και τον χρήστη `neromylosadmin`

### 5. Τρέξε τα EF Core migrations

Από τον root φάκελο του repository, μπες πρώτα στον φάκελο του project:

```bash
cd NeromylosSuites
dotnet ef database update
```

Αυτό δημιουργεί όλο το schema (πίνακες, foreign keys, indexes) —
απαραίτητο **πριν** τρέξεις τα seed scripts στο επόμενο βήμα, αλλιώς οι
πίνακες δεν θα υπάρχουν ακόμα.

(Το connection string διαβάζεται από το `appsettings.Development.json`,
που ήδη δείχνει στο `localhost` με το port που όρισες στο `.env`.)

### 6. Γέμισε τη βάση με αρχικά δεδομένα

Πίσω στον SQL client σου, τρέξε τα δύο επόμενα scripts στο
`Resources/sql/`, **με αυτή τη σειρά**:

1. `Database_02_SeedRoles.sql` — δημιουργεί τους ρόλους (GUEST, ADMIN,
   RECEPTIONIST)
2. `Database_03_SeedRoomsAndPrices.sql` — δημιουργεί τα δωμάτια και τις
   εποχιακές τιμές

### 7. Δημιούργησε τον πρώτο σου ADMIN λογαριασμό

1. Άνοιξε το Swagger UI: `http://localhost:8081/swagger`
2. Κάνε `POST /api/v1/auth/register/member` με τα στοιχεία σου — αυτό
   δημιουργεί έναν λογαριασμό με ρόλο GUEST και προφίλ Member
3. Στη βάση, προήγαγε τον λογαριασμό σε ADMIN:
```sql
   UPDATE Users SET RoleId = 2 WHERE Username = '<το username σου>';
```
4. (Προαιρετικά) Αν ο admin λογαριασμός δεν χρειάζεται member-specific
   δεδομένα (τηλέφωνο, κωδικός χώρας), διέγραψε τη γραμμή του από τον
   πίνακα `Members`:
```sql
   DELETE FROM Members WHERE Id = <το member id σου>;
```

Επανάλαβε τα βήματα 1-3 για οποιονδήποτε RECEPTIONIST λογαριασμό
χρειάζεσαι, χρησιμοποιώντας `RoleId = 3`.

> **Σημείωση:** Δεν υπάρχει προς το παρόν αφιερωμένο endpoint για τη
> δημιουργία ADMIN/RECEPTIONIST λογαριασμών — αυτό το χειροκίνητο βήμα
> role promotion είναι γνωστός περιορισμός, σκόπιμα απλοποιημένος για
> το scope αυτού του project.

### 8. Πρόσβαση

- API: `http://localhost:8081`
- Swagger UI: `http://localhost:8081/swagger`
- Login: `POST /api/v1/auth/login`

## Καθημερινή χρήση κατά την ανάπτυξη

```bash
docker compose stop     # παύση χωρίς διαγραφή containers
docker compose up -d    # επανεκκίνηση
```

Μετά από αλλαγές στον κώδικα, χρειάζεται rebuild:

```bash
docker compose down
docker compose up -d --build
```

## Αρχιτεκτονική

- **Controllers** — HTTP endpoints, τεκμηριωμένα με XML comments + Swagger
- **Services** — επιχειρησιακή λογική, validation
- **Repositories + Unit of Work** — πρόσβαση σε δεδομένα μέσω EF Core
- **AutoMapper** — μετατροπές μεταξύ entities και DTOs
- JWT authentication με ρόλους: `GUEST`, `RECEPTIONIST`, `ADMIN`
- Soft delete pattern (`IsDeleted`/`DeletedAt`) σε όλα τα βασικά entities