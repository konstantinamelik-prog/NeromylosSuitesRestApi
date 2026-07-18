# ============================================================
# Stage 1: build — χρησιμοποιούμε SDK για compile
# Αυτό το stage πετιέται μετά — δεν μπαίνει στο τελικό image
# Ότι ξεκινάει με FROM ορίζει ένα image / container / stage
# Στο τελικό image μπαίνει μόνο το τελευταίο stage
# ============================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# mkdir /src + cd /src — οι εντολές εκτελούνται στον container
# όλες οι εντολές από εδώ και κάτω εκτελούνται σχετικά με τον /src
WORKDIR /src

# Αντιγράφουμε ΜΟΝΟ τα αρχεία που χρειάζεται το restore
# slnx → δομή solution, csproj → NuGet dependencies
# Το dotnet restore διαβάζει τα .csproj, βρίσκει τα NuGet packages που χρειάζεται, 
# τα κατεβάζει από το nuget.org, και τα αποθηκεύει μέσα στο container.
# To 2o NeromylosSuites/ δημιουργεί τον φάκελο αν δεν υπάρχει. Προσοχή το / στο τέλος
# εννοεί φάκελο. Επίσης το πρώτο *.slnx και τo NeromylosSuites/*.csproj αναφέρονται
# έμμεσα στον τρέχον φάκελο που είναι το Dockerfile, δηλαδή στον φάκελο του Solution
# COPY *.slnx .
COPY NeromylosSuites/*.csproj NeromylosSuites/
RUN dotnet restore NeromylosSuites/NeromylosSuites.csproj

# Τώρα αντίγραψε τον υπόλοιπο κώδικα (.cs, wwwroot, appsettings κ.λπ.)
# Αντιγράφει τα πάντα από τον φάκελο NeromylosSuites του host μέσα στον φάκελο NeromylosSuites/ του container
# Ξεχωριστά από τα csproj για να μην σπάμε το cache του restore - αν αλλάξει ένα .cs μην ξανακάνει restore
COPY NeromylosSuites/ NeromylosSuites/

# Μπες στο project folder και κάνε publish
# compile σε Release mode, output στο /app
# -c Release → optimized build, όχι Debug
WORKDIR /src/NeromylosSuites
RUN dotnet publish -c Release -o /app

# ============================================================
# Stage 2: RUN — μόνο runtime, χωρίς SDK, χωρίς source code
# Αυτό είναι το τελικό image (~200MB αντί ~900MB)
# ============================================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Πάρε ΜΟΝΟ τα compiled DLLs από το build stage
COPY --from=build /app .

# Είναι documentation — λέει σε όποιον διαβάζει το Dockerfile "αυτό το container χρησιμοποιεί port 8080". 
# Δεν ανοίγει πραγματικά port, αυτό γίνεται στο docker run -p ή στο docker-compose
# στο .NET 8+ ο Kestrel ακούει σε port 8080 by default. Μέσα σε Docker container: Δεν υπάρχει launchSettings — αγνοείται εντελώς
EXPOSE 8080

# Όρισε τι τρέχει ο container: dotnet NeromylosSuites.dll
# Η μορφή με τα brackets ["dotnet", "NeromylosSuites.dll"] λέγεται exec form — 
# κάθε στοιχείο του array είναι ξεχωριστό argument. 
# Τρέχει: dotnet NeromylosSuites.dll — απευθείας ως process
ENTRYPOINT ["dotnet", "NeromylosSuites.dll"]