
create table public_art
(
    gid         serial
        constraint public_art_pkey
            primary key,
    title       varchar(150),
    artist      varchar(50),
    location    varchar(100),
    material    varchar(150),
    description varchar(600),
    year        integer,
    latitude    double precision,
    longitude   double precision,
    the_geom    geometry
        constraint enforce_geotype_geom
            check ((geometrytype(the_geom) = 'POINT'::text) OR (the_geom IS NULL))
        constraint enforce_dims_the_geom
            check (st_ndims(the_geom) = 2)
        constraint enforce_srid_the_geom
            check (st_srid(the_geom) = 4326)
);

CREATE INDEX public_art_the_geom_gist
  ON public_art
  USING gist
  (the_geom );


