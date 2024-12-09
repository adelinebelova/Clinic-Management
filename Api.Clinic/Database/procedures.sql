##############
# PROCEDURES #
##############

USE Clinic; 

DROP PROCEDURE IF EXISTS PhysicianSearch;
DROP PROCEDURE IF EXISTS PhysicianAdd;
DROP PROCEDURE IF EXISTS PhysicianUpdate;
DROP PROCEDURE IF EXISTS PhysicianDelete;

DELIMITER //

##################
# Physician CRUD #
##################

CREATE PROCEDURE PhysicianSearch 
(
	IN query VARCHAR(50)
)
BEGIN
	SELECT * FROM Physician WHERE Name LIKE CONCAT('%', query, '%');
END//

CREATE PROCEDURE PhysicianAdd
(
	IN PhysicianName VARCHAR(255), 
    IN LicenseNumber VARCHAR(255),
    IN GradDate DateTime, 
    IN Specializations VARCHAR(255),
    OUT PhysicianId INT
)
BEGIN
	INSERT INTO Physician
    VALUES (DEFAULT, PhysicianName, LicenseNumber, GradDate, Specializations);
    
    #LAST_INSERT_ID() is MySQL's version of SCOPE_IDENTITY(). 
    SET PhysicianId = LAST_INSERT_ID();
END//

CREATE PROCEDURE PhysicianUpdate
(
	IN PhysicianId INT,
	IN NewName VARCHAR(255), 
    IN NewLicenseNumber VARCHAR(255),
    IN NewGradDate DateTime, 
    IN NewSpecializations VARCHAR(255)
)
BEGIN
	UPDATE Physician
    SET PhysicianName = NewName, 
		LicenseNumber = NewLicenseNumber, 
        GradDate = NewGradDate,
        Specializations = NewSpecializations
	WHERE Id = PhysicianId; 
END//

CREATE PROCEDURE PhysicianDelete
(
	IN PhysicianId INT
)
BEGIN
	DELETE FROM Physician WHERE Id = PhysicianId;
END//

DELIMITER ;



