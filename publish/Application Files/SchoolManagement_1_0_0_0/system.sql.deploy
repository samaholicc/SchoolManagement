-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 03, 2025 at 03:02 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `system`
--
DELIMITER $$

CREATE DEFINER=`root`@`localhost` PROCEDURE `generate_next_class_id` ()
BEGIN
    DECLARE next_class_id INT;

    -- Récupérer le dernier numéro existant 
    SELECT IFNULL(MAX(CAST(SUBSTRING(CLASS_ID, 2) AS UNSIGNED)), 0) + 1 INTO next_class_id
    FROM studentstable;

    -- Retourner l'ID généré sous le format "C001", "C002", etc.
    SELECT CONCAT('C', LPAD(next_class_id, 3, '0')) AS next_class_id;
END$$

DELIMITER ;


CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_ACCOUNT_PASSWORD` (IN `user` VARCHAR(50), IN `password` VARCHAR(50))   BEGIN
    -- Declare a variable to hold the hashed password
    DECLARE hashed_password VARCHAR(64);
    
    -- Hash the password using SHA-256
    SET hashed_password = SHA2(password, 256);

    -- Update the ACCOUNT table with the hashed password
    UPDATE ACCOUNT 
    SET PASSWORD = hashed_password 
    WHERE USER = user;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_CLASSSTUDENT_DELETE` (IN `p_STUDENT_ID` INT, IN `p_CLASS_ID` INT)   BEGIN
    DELETE FROM student_classes
    WHERE student_id = p_STUDENT_ID AND class_id = p_CLASS_ID;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_CLASS_ADD` (IN `p_SUB_ID` VARCHAR(50), IN `p_TEACHER_ID` VARCHAR(50), IN `p_START_DATE` DATE, IN `p_FINISH_DATE` DATE, IN `p_SCHEDULE` VARCHAR(255), IN `p_NB_S` INT)   BEGIN
    INSERT INTO CLASS (SUB_ID, TEACHER_ID, START_DATE, FINISH_DATE, SCHEDULE, NB_S) 
    VALUES (p_SUB_ID, p_TEACHER_ID, p_START_DATE, p_FINISH_DATE, p_SCHEDULE, p_NB_S);
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_CLASS_STUDENT_ADD` (IN `p_CLASS_ID` VARCHAR(50), IN `p_STUDENT_ID` VARCHAR(11))   BEGIN
    -- Declare a variable to check if the student exists
    DECLARE student_exists INT;

    -- Step 1: Check if the student exists in the STUDENTSTABLE
    SELECT COUNT(*) 
    INTO student_exists
    FROM STUDENTSTABLE 
    WHERE STUDENT_ID = p_STUDENT_ID;

    -- Step 2: If the student exists, proceed with the insertion
    IF student_exists > 0 THEN
        -- Insert the student into the student_classes table (assuming this table holds class enrollments)
        INSERT INTO student_classes (student_id, class_id)
        VALUES (p_STUDENT_ID, p_CLASS_ID);

    ELSE
        -- If the student doesn't exist, throw an error (or handle it differently)
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Student not found in the system!';
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_CLASS_UPDATE` (IN `p_CLASS_ID` VARCHAR(50), IN `p_SUB_ID` VARCHAR(50), IN `p_TEACHER_ID` VARCHAR(50), IN `p_START_DATE` DATE, IN `p_FINISH_DATE` DATE, IN `p_SCHEDULE` VARCHAR(255), IN `p_NB_S` INT)   BEGIN
    UPDATE CLASS 
    SET 
        SUB_ID = p_SUB_ID,
        TEACHER_ID = p_TEACHER_ID,
        START_DATE = p_START_DATE, 
        FINISH_DATE = p_FINISH_DATE,
        SCHEDULE = p_SCHEDULE,
        NB_S = p_NB_S
    WHERE CLASS_ID = p_CLASS_ID;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_SUBJECT_ADD` (IN `p_SUB_NAME` VARCHAR(255), IN `p_CREDITS` INT)   BEGIN
    -- Logic for adding a subject into the SUBJECT table
    INSERT INTO SUBJECT (SUB_NAME, CREDITS) 
    VALUES (p_SUB_NAME, p_CREDITS);
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_SUBJECT_DELETE` (IN `p_SUB_ID` INT)   BEGIN
    -- Delete the subject record with the given SUB_ID
    DELETE FROM SUBJECT
    WHERE SUB_ID = p_SUB_ID;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_SUBJECT_UPDATE` (IN `p_SUB_ID` INT, IN `p_SUB_NAME` VARCHAR(255), IN `p_CREDITS` INT)   BEGIN
    -- Update the SUBJECT table with the provided parameters
    UPDATE SUBJECT
    SET
        SUB_NAME = p_SUB_NAME,   -- Set the new subject name
        CREDITS = p_CREDITS      -- Set the new credits
    WHERE SUB_ID = p_SUB_ID;    -- Ensure we're updating the correct record
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_TEACHER_DELETE` (IN `p_TEACHER_ID` VARCHAR(50))   BEGIN
    -- Start a transaction to ensure data consistency
    START TRANSACTION;

    -- Delete the teacher record from the TEACHER table
    DELETE FROM teacher WHERE TEACHER_ID = p_TEACHER_ID;

    -- Delete the corresponding account from the account table
    DELETE FROM account WHERE ID = p_TEACHER_ID;

    -- Commit the transaction
    COMMIT;

END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `account`
--

CREATE TABLE `account` (
  `ID` varchar(11) NOT NULL,
  `FULL_NAME` varchar(50) NOT NULL,
  `PASSWORD` varchar(100) NOT NULL,
  `ROLE` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `account`
--

INSERT INTO `account` (`ID`, `FULL_NAME`, `PASSWORD`, `ROLE`) VALUES
('AD006', 'Admin', '4DDCB75C8CBAA5AA13D299E34D40CC3553A00D63936AAE42DB2CCAD252960F5E', 'Admin'),
('T012', 'Charlotte Clark', '5DDE896887F6754C9B15BFE3A441AE4806DF2FDE94001311E08BF110622E0BBE', 'Teacher'),
('T003', 'John Smith', 'A5CD77C776E9E54D7104D9999380DF48CD2D4DDC08999DD428774D5B6C437943', 'Teacher'),
('T001', 'saXx', 'A09965720F3C4648EB2D465641BEF0A60A0A81EC1308B2CEF0596D4D252BA023', 'Teacher'),
('S001', 'student1', '2FE7BF7C2E31BCE63D1CE77A05BCC976137A156534526E261196A0E5E1D0E6D3', 'Student'),
('S002', 'student2', '2A6F9DA3B03885004BE5C91FBCB33F655764462068BB014A630903A5C84064BB', 'Student'),
('S003', 'student3', 'DA3CB08BD0B4B825839E52A827D07E6F8496D1A954FA430B6B7036F691BE5E45', 'Student'),
('S004', 'student4', '10ED43C2101C036D9F2355B6087B19B580367635D4FEDEFF968CD3738381BB92', 'Student'),
('T007', 'William Wilson', '5DDE896887F6754C9B15BFE3A441AE4806DF2FDE94001311E08BF110622E0BBE', 'Teacher'),
('T006', 'XXsa', 'A5CD77C776E9E54D7104D9999380DF48CD2D4DDC08999DD428774D5B6C437943', 'Teacher'),
('T002', 'XXt', '1057A9604E04B274DA5A4DE0C8F4B4868D9B230989F8C8C6A28221143CC5A755', 'Teacher'),
('T024', 'xxx', '2D711642B726B04401627CA9FBAC32F5C8530FB1903CC4DB02258717921A4881', 'Teacher'),
('T023', 'xxxxxx', '2D711642B726B04401627CA9FBAC32F5C8530FB1903CC4DB02258717921A4881', 'Teacher');

-- --------------------------------------------------------

--
-- Table structure for table `class`
--

CREATE TABLE `class` (
  `CLASS_ID` varchar(50) NOT NULL,
  `NB_S` int(11) DEFAULT NULL,
  `SUB_ID` int(50) NOT NULL,
  `TEACHER_ID` varchar(11) DEFAULT NULL,
  `SCHEDULE` varchar(255) DEFAULT NULL,
  `START_DATE` date DEFAULT NULL,
  `FINISH_DATE` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `class`
--

INSERT INTO `class` (`CLASS_ID`, `NB_S`, `SUB_ID`, `TEACHER_ID`, `SCHEDULE`, `START_DATE`, `FINISH_DATE`) VALUES
('C001', 21, 31, 'T002', 'Sunday, 16 March 2025 07:00 - 09:00', '2025-02-28', '2025-04-26');

--
-- Triggers `class`
--
DELIMITER $$
CREATE TRIGGER `before_insert_class` BEFORE INSERT ON `class` FOR EACH ROW BEGIN 
    DECLARE next_class_id INT;

    -- Récupérer le dernier numéro existant 
    SELECT IFNULL(MAX(CAST(SUBSTRING(CLASS_ID, 2) AS UNSIGNED)), 0) + 1 INTO next_class_id 
    FROM class;

    -- Assigner l'ID généré sous le format "C001", "C002"... 
    SET NEW.class_ID = CONCAT('C', LPAD(next_class_id, 3, '0'));
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `dep`
--

CREATE TABLE `dep` (
  `DEP_ID` varchar(50) NOT NULL,
  `DEP_NAME` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `dep`
--

INSERT INTO `dep` (`DEP_ID`, `DEP_NAME`) VALUES
('DEP001', 'Mathematics'),
('DEP002', 'Physics'),
('DEP003', 'Chemistry'),
('DEP004', 'Biology'),
('DEP005', 'Computer Science'),
('DEP006', 'History'),
('DEP007', 'Literature'),
('DEP008', 'Art'),
('DEP009', 'Music'),
('DEP010', 'Physical Education'),
('DEP011', 'Business Studies'),
('DEP013', 'Psychology'),
('DEP014', 'Sociology'),
('DEP015', 'Economics'),
('DEP016', 'Environmental Science'),
('DEP017', 'Political Science'),
('DEP018', 'Statistics'),
('DEP019', 'Film Studies'),
('DEP021', 'Politics'),
('DEP022', 'HISTORICA');

--
-- Triggers `dep`
--
DELIMITER $$
CREATE TRIGGER `before_insert_dep` BEFORE INSERT ON `dep` FOR EACH ROW BEGIN
    DECLARE last_id VARCHAR(7);
    DECLARE next_id INT;
    
    -- Get the last ID inserted into the table, based on the 'DEP' prefix
    SELECT MAX(SUBSTRING(DEP_ID, 4, 3)) INTO last_id FROM DEP WHERE DEP_ID LIKE 'DEP%';
    
    -- If no IDs are present (i.e., first insert), start with 1
    IF last_id IS NULL THEN
        SET next_id = 1;
    ELSE
        -- Increment the last ID by 1 (assuming the last number was '001', '002', etc.)
        SET next_id = CAST(last_id AS UNSIGNED) + 1;
    END IF;
    
    -- Generate the new ID by concatenating 'DEP' with the incremented number, zero-padded to 3 digits
    SET NEW.DEP_ID = CONCAT('DEP', LPAD(next_id, 3, '0'));
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `results`
--

CREATE TABLE `results` (
  `CLASS_ID` varchar(50) NOT NULL,
  `STUDENT_ID` varchar(11) NOT NULL,
  `Mid_Term` int(11) NOT NULL,
  `Final_term` int(11) NOT NULL,
  `Average` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `studentstable`
--

CREATE TABLE `studentstable` (
  `STUDENT_ID` varchar(10) NOT NULL,
  `FULL_NAME` varchar(100) NOT NULL,
  `DATE_OF_BIRTH` date DEFAULT NULL,
  `ADRESS` varchar(255) DEFAULT NULL,
  `GENDER` varchar(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `studentstable`
--

INSERT INTO `studentstable` (`STUDENT_ID`, `FULL_NAME`, `DATE_OF_BIRTH`, `ADRESS`, `GENDER`) VALUES
('S001', 'student1', '2025-02-26', 'xx', 'Femme'),
('S002', 'student2', '2025-02-26', 'xx', 'Homme'),
('S003', 'student3', '2025-02-26', 'sssxxx', 'Femme'),
('S004', 'student4', '2025-02-28', 'xx', 'Femme');

--
-- Triggers `studentstable`
--
DELIMITER $$
CREATE TRIGGER `before_insert_student` BEFORE INSERT ON `studentstable` FOR EACH ROW BEGIN
    DECLARE next_id INT;

    -- Récupérer le dernier numéro existant
    SELECT IFNULL(MAX(CAST(SUBSTRING(STUDENT_ID, 2) AS UNSIGNED)), 0) + 1 INTO next_id 
    FROM studentstable;

    -- Assigner l'ID généré sous le format "S001", "S002"...
    SET NEW.STUDENT_ID = CONCAT('S', LPAD(next_id, 3, '0'));
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `student_classes`
--

CREATE TABLE `student_classes` (
  `student_id` varchar(11) NOT NULL,
  `class_id` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `student_classes`
--

INSERT INTO `student_classes` (`student_id`, `class_id`) VALUES
('S001', 'C001'),
('S001', 'C002'),
('S001', 'C003'),
('S002', 'C001'),
('S002', 'C002'),
('S002', 'C003'),
('S002', 'C039'),
('S003', 'C003'),
('S004', 'C001'),
('S004', 'C002'),
('S004', 'C003');

-- --------------------------------------------------------

--
-- Table structure for table `subject`
--

CREATE TABLE `subject` (
  `SUB_ID` int(50) NOT NULL,
  `SUB_NAME` varchar(100) NOT NULL,
  `CREDITS` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `subject`
--

INSERT INTO `subject` (`SUB_ID`, `SUB_NAME`, `CREDITS`) VALUES
(31, 'maths', 5);

-- --------------------------------------------------------

--
-- Table structure for table `teacher`
--

CREATE TABLE `teacher` (
  `TEACHER_ID` varchar(50) NOT NULL,
  `FULL_NAME` varchar(100) NOT NULL,
  `ADRESS` varchar(255) DEFAULT NULL,
  `GENDER` varchar(10) DEFAULT NULL,
  `DATE_OF_BIRTH` date DEFAULT NULL,
  `DEP_ID` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `teacher`
--

INSERT INTO `teacher` (`TEACHER_ID`, `FULL_NAME`, `ADRESS`, `GENDER`, `DATE_OF_BIRTH`, `DEP_ID`) VALUES
('T001', 'samia', 'xx', 'Femme', '2025-02-04', 'DEP003'),
('T002', 'XXt', 'XX', 'Femme', '2025-02-05', 'DEP005'),
('T003', 'John Smith', '123 Main St, Springfield', 'Homme', '1985-06-15', 'DEP001'),
('T006', 'XXsa', '101 Maple St, Evergreen', 'Femme', '1988-12-05', 'DEP001'),
('T007', 'William Wilson', '202 Birch Blvd, Riverton', 'Homme', '1979-04-18', 'DEP004'),
('T012', 'Charlotte Clark', '707 Aspen Ave, Riverton', 'Femme', '1993-08-17', 'DEP001'),
('T020', 'xx', 'xx', 'Femme', '1985-06-15', 'DEP001'),
('T021', 'xxxxx', 'xx', 'Femme', '2025-02-06', 'DEP007'),
('T023', 'xxxxxx', 'xx', 'Femme', '2025-02-22', 'DEP005'),
('T024', 'xxx', 'xx', 'Homme', '2025-02-22', 'DEP002');

--
-- Triggers `teacher`
--
DELIMITER $$
CREATE TRIGGER `before_insert_TEACHER` BEFORE INSERT ON `teacher` FOR EACH ROW BEGIN
    DECLARE next_id INT;

    -- Récupérer le dernier numéro existant
    SELECT IFNULL(MAX(CAST(SUBSTRING(TEACHER_ID, 2) AS UNSIGNED)), 0) + 1 INTO next_id 
    FROM teacher;

    -- Assigner l'ID généré sous le format "S001", "S002"...
    SET NEW.TEACHER_ID = CONCAT('T', LPAD(next_id, 3, '0'));
END
$$
DELIMITER ;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `account`
--
ALTER TABLE `account`
  ADD PRIMARY KEY (`FULL_NAME`),
  ADD UNIQUE KEY `TENDN` (`FULL_NAME`);

--
-- Indexes for table `class`
--
ALTER TABLE `class`
  ADD PRIMARY KEY (`CLASS_ID`),
  ADD KEY `SUB_ID` (`SUB_ID`),
  ADD KEY `TEACHER_ID` (`TEACHER_ID`);

--
-- Indexes for table `dep`
--
ALTER TABLE `dep`
  ADD PRIMARY KEY (`DEP_ID`);

--
-- Indexes for table `results`
--
ALTER TABLE `results`
  ADD PRIMARY KEY (`STUDENT_ID`,`CLASS_ID`),
  ADD UNIQUE KEY `UC_student_class` (`STUDENT_ID`,`CLASS_ID`),
  ADD KEY `CLASS_ID` (`CLASS_ID`);

--
-- Indexes for table `studentstable`
--
ALTER TABLE `studentstable`
  ADD PRIMARY KEY (`STUDENT_ID`),
  ADD KEY `STUDENT_ID_2` (`STUDENT_ID`);

--
-- Indexes for table `student_classes`
--
ALTER TABLE `student_classes`
  ADD PRIMARY KEY (`student_id`,`class_id`),
  ADD KEY `class_id` (`class_id`);

--
-- Indexes for table `subject`
--
ALTER TABLE `subject`
  ADD PRIMARY KEY (`SUB_ID`),
  ADD UNIQUE KEY `SUB_ID` (`SUB_ID`),
  ADD KEY `SUB_ID_2` (`SUB_ID`);

--
-- Indexes for table `teacher`
--
ALTER TABLE `teacher`
  ADD PRIMARY KEY (`TEACHER_ID`),
  ADD UNIQUE KEY `TEACHER_ID` (`TEACHER_ID`),
  ADD KEY `TEACHER_ID_2` (`TEACHER_ID`),
  ADD KEY `DEP_ID` (`DEP_ID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `subject`
--
ALTER TABLE `subject`
  MODIFY `SUB_ID` int(50) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=33;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `class`
--
ALTER TABLE `class`
  ADD CONSTRAINT `class_ibfk_1` FOREIGN KEY (`TEACHER_ID`) REFERENCES `teacher` (`TEACHER_ID`);

--
-- Constraints for table `results`
--
ALTER TABLE `results`
  ADD CONSTRAINT `results_ibfk_1` FOREIGN KEY (`STUDENT_ID`) REFERENCES `studentstable` (`STUDENT_ID`) ON DELETE CASCADE,
  ADD CONSTRAINT `results_ibfk_2` FOREIGN KEY (`CLASS_ID`) REFERENCES `class` (`CLASS_ID`) ON DELETE CASCADE;

--
-- Constraints for table `studentstable`
--
ALTER TABLE `studentstable`
  ADD CONSTRAINT `studentstable_ibfk_1` FOREIGN KEY (`STUDENT_ID`) REFERENCES `studentstable` (`STUDENT_ID`) ON DELETE CASCADE;

--
-- Constraints for table `student_classes`
--
ALTER TABLE `student_classes`
  ADD CONSTRAINT `student_classes_ibfk_1` FOREIGN KEY (`student_id`) REFERENCES `studentstable` (`STUDENT_ID`);

--
-- Constraints for table `teacher`
--
ALTER TABLE `teacher`
  ADD CONSTRAINT `teacher_ibfk_1` FOREIGN KEY (`DEP_ID`) REFERENCES `dep` (`DEP_ID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
