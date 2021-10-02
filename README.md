# Geospatial Learning

## Server-side Geospatial Clustering using H3

This repo is a companion to a complete blog post [here](https://shawinnes.com/server-side-spatial-clustering/)

## Getting Started

1. Build the custom PostGIS docker image and start it using docker-compose

```
docker-compose up -d --build
```

2. Run the API

```
cd api
dotnet run
```

3. Start the Front-end App

```
cd app
yarn dev
```

4. Go to the page served by the Front-end App
