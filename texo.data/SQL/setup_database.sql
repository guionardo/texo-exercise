CREATE TABLE IF NOT EXISTS movies
(
    id      INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    release INTEGER NOT NULL,
    title   TEXT    NOT NULL,
    winner  INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS producers
(
    id   INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    name TEXT    NOT NULL
);

CREATE TABLE IF NOT EXISTS studios
(
    id   INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    name TEXT    NOT NULL
);

CREATE TABLE IF NOT EXISTS movie_producers
(
    id_movie    INTEGER,
    id_producer INTEGER,
    PRIMARY KEY (id_movie, id_producer),
    CONSTRAINT fk_movie FOREIGN KEY (id_movie) REFERENCES movies (id),
    CONSTRAINT fk_producer FOREIGN KEY (id_producer) REFERENCES producers (id)
);


CREATE TABLE IF NOT EXISTS movie_studios
(
    id_movie  INTEGER,
    id_studio INTEGER,
    PRIMARY KEY (id_movie, id_studio),
    CONSTRAINT fk_movie FOREIGN KEY (id_movie) REFERENCES movies (id),
    CONSTRAINT fk_producer FOREIGN KEY (id_studio) REFERENCES studios (id)
); 