-- Database: projekt

-- DROP DATABASE projekt;

CREATE DATABASE projekt
  WITH OWNER = postgres
       ENCODING = 'UTF8'
       TABLESPACE = pg_default
       LC_COLLATE = 'en_US.UTF-8'
       LC_CTYPE = 'en_US.UTF-8'
       CONNECTION LIMIT = -1;
	   
-- Table: movie

-- DROP TABLE movie;

CREATE TABLE movie
(
  movieid integer NOT NULL DEFAULT nextval('movie_movieid_seq'::regclass),
  title character varying(255) NOT NULL,
  categories character varying(255) NOT NULL,
  summary text NOT NULL,
  description text NOT NULL,
  vector tsvector NOT NULL,
  CONSTRAINT movie_pkey PRIMARY KEY (movieid)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE movie
  OWNER TO root;

-- Index: idxmoviegist

-- DROP INDEX idxmoviegist;

CREATE INDEX idxmoviegist
  ON movie
  USING gist
  (vector);
  
-- Table: search_history

-- DROP TABLE search_history;

CREATE TABLE search_history
(
  id integer NOT NULL DEFAULT nextval('search_history_id_seq'::regclass),
  searchinput character varying(200) NOT NULL,
  inputdatetime timestamp without time zone NOT NULL DEFAULT now(),
  CONSTRAINT search_history_pkey PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE search_history
  OWNER TO postgres;
  
-- Sequence: movie_movieid_seq

-- DROP SEQUENCE movie_movieid_seq;

CREATE SEQUENCE movie_movieid_seq
  INCREMENT 1
  MINVALUE 1
  MAXVALUE 9223372036854775807
  START 1018
  CACHE 1;
ALTER TABLE movie_movieid_seq
  OWNER TO postgres;
  
-- Sequence: search_history_id_seq

-- DROP SEQUENCE search_history_id_seq;

CREATE SEQUENCE search_history_id_seq
  INCREMENT 1
  MINVALUE 1
  MAXVALUE 9223372036854775807
  START 79
  CACHE 1;
ALTER TABLE search_history_id_seq
  OWNER TO postgres;
  
-- Extension: fuzzystrmatch

-- DROP EXTENSION fuzzystrmatch;

 CREATE EXTENSION fuzzystrmatch
  SCHEMA public
  VERSION "1.0";
  
-- Extension: pg_trgm

-- DROP EXTENSION pg_trgm;

 CREATE EXTENSION pg_trgm
  SCHEMA public
  VERSION "1.1";
  
-- Extension: plpgsql

-- DROP EXTENSION plpgsql;

 CREATE EXTENSION plpgsql
  SCHEMA pg_catalog
  VERSION "1.0";
  
-- Extension: tablefunc

-- DROP EXTENSION tablefunc;

 CREATE EXTENSION tablefunc
  SCHEMA public
  VERSION "1.0";



