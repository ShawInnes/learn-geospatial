FROM postgis/postgis

RUN apt update && \
  apt install -y python pip git cmake postgresql-server-dev-13 && \
  pip install pgxnclient

RUN pgxn install h3

RUN mkdir -p /docker-entrypoint-initdb.d

COPY ./initdb-extensions.sh /docker-entrypoint-initdb.d/extensions.sh
