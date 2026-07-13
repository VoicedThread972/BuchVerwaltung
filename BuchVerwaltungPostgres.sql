--CREATE DATABASE IF NOT EXISTS buchdb;

DROP TABLE IF EXISTS aktuelle_buecher;
CREATE TABLE aktuelle_buecher (
	id SERIAL NOT NULL,
	titel VARCHAR(100) NOT NULL,
	autor VARCHAR(100) NOT NULL,
	PRIMARY KEY ( id )
	);

DROP TABLE IF EXISTS archivierte_buecher;
CREATE TABLE archivierte_buecher (
	id SERIAL NOT NULL,
	titel VARCHAR(100) NOT NULL,
	autor VARCHAR(100) NOT NULL,
	PRIMARY KEY ( id )
	);


INSERT INTO aktuelle_buecher (titel, autor) VALUES ('C# 8 mit Visual Studio 2019', 'Andreas Kühnel');
INSERT INTO aktuelle_buecher (titel, autor) VALUES ('C# Programmieren: für Einsteiger', 'Michael Bonacina');
INSERT INTO aktuelle_buecher (titel, autor) VALUES ('Einstieg in C# mit Visual Studio 2022', 'Thomas Theis');
INSERT INTO archivierte_buecher (titel, autor) VALUES ('C# Programmieren für Einsteiger', 'Adrian Rechsteiner');
INSERT INTO archivierte_buecher (titel, autor) VALUES ('Learning C# by Developing Games with Unity 2021', 'Harrison Ferrone');


