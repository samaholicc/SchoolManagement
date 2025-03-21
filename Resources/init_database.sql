-- Création de la base de données
CREATE DATABASE IF NOT EXISTS `system`;
USE `system`;

-- Configuration initiale
SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";
SET NAMES utf8mb4;

-- Création des tables

CREATE TABLE `account` (
  `ID` varchar(11) NOT NULL,
  `FULL_NAME` varchar(50) NOT NULL,
  `PASSWORD` varchar(100) NOT NULL,
  `ROLE` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`FULL_NAME`),
  UNIQUE KEY `TENDN` (`FULL_NAME`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `dep` (
  `DEP_ID` varchar(50) NOT NULL,
  `DEP_NAME` varchar(100) NOT NULL,
  PRIMARY KEY (`DEP_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `subject` (
  `SUB_ID` int(50) NOT NULL AUTO_INCREMENT,
  `SUB_NAME` varchar(100) NOT NULL,
  `CREDITS` int(11) DEFAULT NULL,
  PRIMARY KEY (`SUB_ID`),
  UNIQUE KEY `SUB_ID` (`SUB_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `teacher` (
  `TEACHER_ID` varchar(50) NOT NULL,
  `FULL_NAME` varchar(100) NOT NULL,
  `ADRESS` varchar(255) DEFAULT NULL,
  `GENDER` varchar(10) DEFAULT NULL,
  `DATE_OF_BIRTH` date DEFAULT NULL,
  `DEP_ID` varchar(50) NOT NULL,
  PRIMARY KEY (`TEACHER_ID`),
  UNIQUE KEY `TEACHER_ID` (`TEACHER_ID`),
  KEY `DEP_ID` (`DEP_ID`),
  CONSTRAINT `teacher_ibfk_1` FOREIGN KEY (`DEP_ID`) REFERENCES `dep` (`DEP_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `class` (
  `CLASS_ID` varchar(50) NOT NULL,
  `NB_S` int(11) DEFAULT NULL,
  `SUB_ID` int(50) NOT NULL,
  `TEACHER_ID` varchar(11) DEFAULT NULL,
  `SCHEDULE` varchar(255) DEFAULT NULL,
  `START_DATE` date DEFAULT NULL,
  `FINISH_DATE` date DEFAULT NULL,
  PRIMARY KEY (`CLASS_ID`),
  KEY `SUB_ID` (`SUB_ID`),
  KEY `TEACHER_ID` (`TEACHER_ID`),
  CONSTRAINT `class_ibfk_1` FOREIGN KEY (`TEACHER_ID`) REFERENCES `teacher` (`TEACHER_ID`) ON UPDATE CASCADE,
  CONSTRAINT `class_ibfk_2` FOREIGN KEY (`SUB_ID`) REFERENCES `subject` (`SUB_ID`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `studentstable` (
  `STUDENT_ID` varchar(10) NOT NULL,
  `FULL_NAME` varchar(100) NOT NULL,
  `DATE_OF_BIRTH` date NOT NULL,
  `ADRESS` varchar(255) NOT NULL,
  `GENDER` varchar(10) NOT NULL,
  PRIMARY KEY (`STUDENT_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `student_classes` (
  `student_id` varchar(11) NOT NULL,
  `class_id` varchar(50) NOT NULL,
  PRIMARY KEY (`student_id`,`class_id`),
  KEY `class_id` (`class_id`),
  CONSTRAINT `student_classes_ibfk_1` FOREIGN KEY (`student_id`) REFERENCES `studentstable` (`STUDENT_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `results` (
  `CLASS_ID` varchar(50) NOT NULL,
  `STUDENT_ID` varchar(11) NOT NULL,
  `Mid_Term` int(11) NOT NULL,
  `Final_term` int(11) NOT NULL,
  `Average` int(11) NOT NULL,
  PRIMARY KEY (`STUDENT_ID`,`CLASS_ID`),
  UNIQUE KEY `UC_student_class` (`STUDENT_ID`,`CLASS_ID`),
  KEY `results_ibfk_2` (`CLASS_ID`),
  CONSTRAINT `results_ibfk_1` FOREIGN KEY (`STUDENT_ID`) REFERENCES `studentstable` (`STUDENT_ID`),
  CONSTRAINT `results_ibfk_2` FOREIGN KEY (`CLASS_ID`) REFERENCES `class` (`CLASS_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Création des triggers

DELIMITER $$

CREATE TRIGGER `before_insert_dep` BEFORE INSERT ON `dep` FOR EACH ROW 
BEGIN
    DECLARE last_id VARCHAR(7);
    DECLARE next_id INT;
    SELECT MAX(SUBSTRING(DEP_ID, 4, 3)) INTO last_id FROM DEP WHERE DEP_ID LIKE 'DEP%';
    IF last_id IS NULL THEN
        SET next_id = 1;
    ELSE
        SET next_id = CAST(last_id AS UNSIGNED) + 1;
    END IF;
    SET NEW.DEP_ID = CONCAT('DEP', LPAD(next_id, 3, '0'));
END$$

CREATE TRIGGER `before_insert_teacher` BEFORE INSERT ON `teacher` FOR EACH ROW 
BEGIN
    DECLARE next_id INT;
    SELECT IFNULL(MAX(CAST(SUBSTRING(TEACHER_ID, 2) AS UNSIGNED)), 0) + 1 INTO next_id 
    FROM teacher;
    SET NEW.TEACHER_ID = CONCAT('T', LPAD(next_id, 3, '0'));
END$$

CREATE TRIGGER `before_insert_class` BEFORE INSERT ON `class` FOR EACH ROW 
BEGIN 
    DECLARE next_class_id INT;
    SELECT IFNULL(MAX(CAST(SUBSTRING(CLASS_ID, 2) AS UNSIGNED)), 0) + 1 INTO next_class_id 
    FROM class;
    SET NEW.class_ID = CONCAT('C', LPAD(next_class_id, 3, '0'));
END$$

CREATE TRIGGER `before_insert_student` BEFORE INSERT ON `studentstable` FOR EACH ROW 
BEGIN
    DECLARE next_id INT;
    SELECT IFNULL(MAX(CAST(SUBSTRING(STUDENT_ID, 2) AS UNSIGNED)), 0) + 1 INTO next_id 
    FROM studentstable;
    SET NEW.STUDENT_ID = CONCAT('S', LPAD(next_id, 3, '0'));
END$$

DELIMITER ;

-- Création des procédures stockées

DELIMITER $$

CREATE PROCEDURE `generate_next_class_id` ()
BEGIN
    DECLARE next_class_id INT;
    SELECT IFNULL(MAX(CAST(SUBSTRING(CLASS_ID, 2) AS UNSIGNED)), 0) + 1 INTO next_class_id
    FROM studentstable;
    SELECT CONCAT('C', LPAD(next_class_id, 3, '0')) AS next_class_id;
END$$

CREATE PROCEDURE `SP_ACCOUNT_PASSWORD` (IN `user` VARCHAR(50), IN `password` VARCHAR(50))
BEGIN
    DECLARE hashed_password VARCHAR(64);
    SET hashed_password = SHA2(password, 256);
    UPDATE ACCOUNT 
    SET PASSWORD = hashed_password 
    WHERE USER = user;
END$$

CREATE PROCEDURE `SP_CLASSSTUDENT_DELETE` (IN `p_STUDENT_ID` INT, IN `p_CLASS_ID` INT)
BEGIN
    DELETE FROM student_classes
    WHERE student_id = p_STUDENT_ID AND class_id = p_CLASS_ID;
END$$

CREATE PROCEDURE `SP_CLASS_ADD` (IN `p_SUB_ID` VARCHAR(50), IN `p_TEACHER_ID` VARCHAR(50), IN `p_START_DATE` DATE, IN `p_FINISH_DATE` DATE, IN `p_SCHEDULE` VARCHAR(255), IN `p_NB_S` INT)
BEGIN
    INSERT INTO CLASS (SUB_ID, TEACHER_ID, START_DATE, FINISH_DATE, SCHEDULE, NB_S) 
    VALUES (p_SUB_ID, p_TEACHER_ID, p_START_DATE, p_FINISH_DATE, p_SCHEDULE, p_NB_S);
END$$

CREATE PROCEDURE `SP_CLASS_STUDENT_ADD` (IN `p_CLASS_ID` VARCHAR(50), IN `p_STUDENT_ID` VARCHAR(11))
BEGIN
    DECLARE student_exists INT;
    SELECT COUNT(*) INTO student_exists
    FROM STUDENTSTABLE 
    WHERE STUDENT_ID = p_STUDENT_ID;
    IF student_exists > 0 THEN
        INSERT INTO student_classes (student_id, class_id)
        VALUES (p_STUDENT_ID, p_CLASS_ID);
    ELSE
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Student not found in the system!';
    END IF;
END$$

CREATE PROCEDURE `SP_CLASS_UPDATE` (IN `p_CLASS_ID` VARCHAR(50), IN `p_SUB_ID` VARCHAR(50), IN `p_TEACHER_ID` VARCHAR(50), IN `p_START_DATE` DATE, IN `p_FINISH_DATE` DATE, IN `p_SCHEDULE` VARCHAR(255), IN `p_NB_S` INT)
BEGIN
    UPDATE CLASS 
    SET SUB_ID = p_SUB_ID,
        TEACHER_ID = p_TEACHER_ID,
        START_DATE = p_START_DATE, 
        FINISH_DATE = p_FINISH_DATE,
        SCHEDULE = p_SCHEDULE,
        NB_S = p_NB_S
    WHERE CLASS_ID = p_CLASS_ID;
END$$

CREATE PROCEDURE `SP_SUBJECT_ADD` (IN `p_SUB_NAME` VARCHAR(255), IN `p_CREDITS` INT)
BEGIN
    INSERT INTO SUBJECT (SUB_NAME, CREDITS) 
    VALUES (p_SUB_NAME, p_CREDITS);
END$$

CREATE PROCEDURE `SP_SUBJECT_DELETE` (IN `p_SUB_ID` INT)
BEGIN
    DELETE FROM SUBJECT
    WHERE SUB_ID = p_SUB_ID;
END$$

CREATE PROCEDURE `SP_SUBJECT_UPDATE` (IN `p_SUB_ID` INT, IN `p_SUB_NAME` VARCHAR(255), IN `p_CREDITS` INT)
BEGIN
    UPDATE SUBJECT
    SET SUB_NAME = p_SUB_NAME,
        CREDITS = p_CREDITS
    WHERE SUB_ID = p_SUB_ID;
END$$

CREATE PROCEDURE `SP_TEACHER_DELETE` (IN `p_TEACHER_ID` VARCHAR(50))
BEGIN
    START TRANSACTION;
    DELETE FROM teacher WHERE TEACHER_ID = p_TEACHER_ID;
    DELETE FROM account WHERE ID = p_TEACHER_ID;
    COMMIT;
END$$

DELIMITER ;

-- Insert departments
INSERT INTO `dep` (`DEP_ID`, `DEP_NAME`) VALUES
('DEP001', 'Mathematics'),
('DEP002', 'Physics'),
('DEP003', 'Chemistry'),
('DEP004', 'Biology'),
('DEP005', 'Computer Science');

-- Insert subjects
INSERT INTO `subject` (`SUB_ID`, `SUB_NAME`, `CREDITS`) VALUES
(33, 'Mathematics', 4),
(34, 'Physics', 4),
(35, 'Chemistry', 3),
(36, 'Biology', 3),
(37, 'Computer Science', 5);

-- Insert admin account (AD006)
INSERT INTO `account` (`ID`, `FULL_NAME`, `PASSWORD`, `ROLE`) VALUES
('AD006', 'Admin', '4DDCB75C8CBAA5AA13D299E34D40CC3553A00D63936AAE42DB2CCAD252960F5E', 'Admin');