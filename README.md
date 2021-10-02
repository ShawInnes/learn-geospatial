# Geospatial Learning

## PostGIS

## Postgres Extension Network

https://pgxn.org/

## Getting Started

docker-compose up -d --build


dotnet new webapi --name api

dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite


dotnet ef dbcontext scaffold --no-onconfiguring --table public_art --output-dir Data --context PostGisDbContext  "Host=localhost;Database=postgis;Username=postgis;Password=postgis;" Npgsql.EntityFrameworkCore.PostgreSQL

dotnet add package pocketken.H3
