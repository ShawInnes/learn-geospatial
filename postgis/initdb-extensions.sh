#!/bin/sh

set -e

# Perform all actions as $POSTGRES_USER
export PGUSER="$POSTGRES_USER"

echo "Loading H3 extension"
pgxn load h3 

echo "Reload PG configuration"
psql -c 'SELECT pg_reload_conf();'

echo "Create Table"
psql < /import/public-art.sql

echo "Import Data"
psql -c "\copy public_art(title,artist,location,material,description,year,latitude,longitude) FROM '/import/public-art-open-data-2021-03-17.csv' DELIMITERS ',' CSV HEADER;"

echo "Create Spatial Types"
psql -c "UPDATE public_art SET the_geom = ST_GeomFromText('POINT(' || longitude || ' ' || latitude || ')',4326);"