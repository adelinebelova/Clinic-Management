CREATE DATABASE IF NOT EXISTS Clinic; 
USE Clinic;

DROP TABLE IF EXISTS Physician; 

CREATE TABLE Physician(
	Id INT PRIMARY KEY AUTO_INCREMENT, 
    Name VARCHAR(255) NOT NULL, 
    LicenseNumber VARCHAR(255),
    GradDate DateTime, 
    Specializations VARCHAR(255)
) ENGINE = InnoDB; 
