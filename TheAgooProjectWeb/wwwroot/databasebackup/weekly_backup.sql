-- MySqlBackup.NET 2.6.5.0
-- Dump Time: 2026-01-29 20:12:15
-- --------------------------------------
-- Server version 11.8.2-MariaDB mariadb.org binary distribution


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 
-- Definition of __efmigrationshistory
-- 

DROP TABLE IF EXISTS `__efmigrationshistory`;
CREATE TABLE IF NOT EXISTS `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table __efmigrationshistory
-- 

/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory`(`MigrationId`,`ProductVersion`) VALUES('20241030140049_FirstDataPush','8.0.6'),('20241031120757_PrincipalName','8.0.6'),('20241125120200_SubjectUpdate','8.0.6');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;

-- 
-- Definition of aspnetroles
-- 

DROP TABLE IF EXISTS `aspnetroles`;
CREATE TABLE IF NOT EXISTS `aspnetroles` (
  `Id` varchar(255) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table aspnetroles
-- 

/*!40000 ALTER TABLE `aspnetroles` DISABLE KEYS */;
INSERT INTO `aspnetroles`(`Id`,`Name`,`NormalizedName`,`ConcurrencyStamp`) VALUES('41327a1a-454c-4387-b02d-340a4e1b20fe','Admin','ADMIN',NULL),('9ad23da2-8203-4b54-933e-43901a2eeb9c','Exam Officer','EXAM OFFICER',NULL),('a4f17842-7795-4a6d-b92e-b33b0b147af8','Accountant','ACCOUNTANT',NULL),('b5e36276-80ec-4b6a-9791-ba8f3908d32d','IsStudent','ISSTUDENT',NULL);
/*!40000 ALTER TABLE `aspnetroles` ENABLE KEYS */;

-- 
-- Definition of aspnetroleclaims
-- 

DROP TABLE IF EXISTS `aspnetroleclaims`;
CREATE TABLE IF NOT EXISTS `aspnetroleclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RoleId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table aspnetroleclaims
-- 

/*!40000 ALTER TABLE `aspnetroleclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetroleclaims` ENABLE KEYS */;

-- 
-- Definition of aspnetusers
-- 

DROP TABLE IF EXISTS `aspnetusers`;
CREATE TABLE IF NOT EXISTS `aspnetusers` (
  `Id` varchar(255) NOT NULL,
  `Discriminator` varchar(21) NOT NULL,
  `Fullname` varchar(150) DEFAULT NULL,
  `Position` varchar(80) DEFAULT NULL,
  `Active` tinyint(1) DEFAULT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext DEFAULT NULL,
  `SecurityStamp` longtext DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL,
  `PhoneNumber` longtext DEFAULT NULL,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table aspnetusers
-- 

/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers`(`Id`,`Discriminator`,`Fullname`,`Position`,`Active`,`UserName`,`NormalizedUserName`,`Email`,`NormalizedEmail`,`EmailConfirmed`,`PasswordHash`,`SecurityStamp`,`ConcurrencyStamp`,`PhoneNumber`,`PhoneNumberConfirmed`,`TwoFactorEnabled`,`LockoutEnd`,`LockoutEnabled`,`AccessFailedCount`) VALUES('ede38d91-7e31-4a2e-846e-8dff41b077e2','ApplicationUser','Admin Officer','Admin',1,'AG/ADMIN/01','AG/ADMIN/01',NULL,NULL,0,'AQAAAAIAAYagAAAAEDT3QoDwIbgb/KapzM8ivxTp4jrEmEyhlezoGe/vibnJXkR6bVAfnv1Z60Uj2YGr1Q==','WNEZFMLMH5QGSMAGY3KZIRBAGDD2QBI7','31cec059-3fbf-46ef-b991-1d4162a3ea5b',NULL,0,0,NULL,1,0);
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;

-- 
-- Definition of aspnetuserclaims
-- 

DROP TABLE IF EXISTS `aspnetuserclaims`;
CREATE TABLE IF NOT EXISTS `aspnetuserclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table aspnetuserclaims
-- 

/*!40000 ALTER TABLE `aspnetuserclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserclaims` ENABLE KEYS */;

-- 
-- Definition of aspnetuserlogins
-- 

DROP TABLE IF EXISTS `aspnetuserlogins`;
CREATE TABLE IF NOT EXISTS `aspnetuserlogins` (
  `LoginProvider` varchar(255) NOT NULL,
  `ProviderKey` varchar(255) NOT NULL,
  `ProviderDisplayName` longtext DEFAULT NULL,
  `UserId` varchar(255) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table aspnetuserlogins
-- 

/*!40000 ALTER TABLE `aspnetuserlogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserlogins` ENABLE KEYS */;

-- 
-- Definition of aspnetuserroles
-- 

DROP TABLE IF EXISTS `aspnetuserroles`;
CREATE TABLE IF NOT EXISTS `aspnetuserroles` (
  `UserId` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table aspnetuserroles
-- 

/*!40000 ALTER TABLE `aspnetuserroles` DISABLE KEYS */;
INSERT INTO `aspnetuserroles`(`UserId`,`RoleId`) VALUES('ede38d91-7e31-4a2e-846e-8dff41b077e2','41327a1a-454c-4387-b02d-340a4e1b20fe');
/*!40000 ALTER TABLE `aspnetuserroles` ENABLE KEYS */;

-- 
-- Definition of aspnetusertokens
-- 

DROP TABLE IF EXISTS `aspnetusertokens`;
CREATE TABLE IF NOT EXISTS `aspnetusertokens` (
  `UserId` varchar(255) NOT NULL,
  `LoginProvider` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Value` longtext DEFAULT NULL,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table aspnetusertokens
-- 

/*!40000 ALTER TABLE `aspnetusertokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetusertokens` ENABLE KEYS */;

-- 
-- Definition of expendituretype
-- 

DROP TABLE IF EXISTS `expendituretype`;
CREATE TABLE IF NOT EXISTS `expendituretype` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` longtext NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table expendituretype
-- 

/*!40000 ALTER TABLE `expendituretype` DISABLE KEYS */;
/*!40000 ALTER TABLE `expendituretype` ENABLE KEYS */;

-- 
-- Definition of feedbacks
-- 

DROP TABLE IF EXISTS `feedbacks`;
CREATE TABLE IF NOT EXISTS `feedbacks` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  `Subject` varchar(200) NOT NULL,
  `Email` varchar(550) NOT NULL,
  `Message` longtext NOT NULL,
  `IsRead` tinyint(1) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table feedbacks
-- 

/*!40000 ALTER TABLE `feedbacks` DISABLE KEYS */;
/*!40000 ALTER TABLE `feedbacks` ENABLE KEYS */;

-- 
-- Definition of gallerytables
-- 

DROP TABLE IF EXISTS `gallerytables`;
CREATE TABLE IF NOT EXISTS `gallerytables` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Message` varchar(500) NOT NULL,
  `PostedBy` varchar(100) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table gallerytables
-- 

/*!40000 ALTER TABLE `gallerytables` DISABLE KEYS */;
/*!40000 ALTER TABLE `gallerytables` ENABLE KEYS */;

-- 
-- Definition of imagestables
-- 

DROP TABLE IF EXISTS `imagestables`;
CREATE TABLE IF NOT EXISTS `imagestables` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(700) NOT NULL,
  `GalleryId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ImagesTables_GalleryId` (`GalleryId`),
  CONSTRAINT `FK_ImagesTables_GalleryTables_GalleryId` FOREIGN KEY (`GalleryId`) REFERENCES `gallerytables` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table imagestables
-- 

/*!40000 ALTER TABLE `imagestables` DISABLE KEYS */;
/*!40000 ALTER TABLE `imagestables` ENABLE KEYS */;

-- 
-- Definition of newstables
-- 

DROP TABLE IF EXISTS `newstables`;
CREATE TABLE IF NOT EXISTS `newstables` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Reporter` varchar(150) NOT NULL,
  `Headline` varchar(250) NOT NULL,
  `NewBody` longtext NOT NULL,
  `ImagePath` varchar(1500) NOT NULL,
  `Read` int(11) NOT NULL,
  `Category` varchar(150) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table newstables
-- 

/*!40000 ALTER TABLE `newstables` DISABLE KEYS */;
/*!40000 ALTER TABLE `newstables` ENABLE KEYS */;

-- 
-- Definition of otherpaytables
-- 

DROP TABLE IF EXISTS `otherpaytables`;
CREATE TABLE IF NOT EXISTS `otherpaytables` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` longtext NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table otherpaytables
-- 

/*!40000 ALTER TABLE `otherpaytables` DISABLE KEYS */;
/*!40000 ALTER TABLE `otherpaytables` ENABLE KEYS */;

-- 
-- Definition of parenttables
-- 

DROP TABLE IF EXISTS `parenttables`;
CREATE TABLE IF NOT EXISTS `parenttables` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `StudentRegNo` varchar(30) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `Occupation` varchar(50) NOT NULL,
  `Relationship` varchar(30) NOT NULL,
  `PhoneNumber` varchar(11) NOT NULL,
  `Address` varchar(250) NOT NULL,
  `StaffId` varchar(250) DEFAULT NULL,
  `CreateDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_ParentTables_PhoneNumber` (`PhoneNumber`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table parenttables
-- 

/*!40000 ALTER TABLE `parenttables` DISABLE KEYS */;
/*!40000 ALTER TABLE `parenttables` ENABLE KEYS */;

-- 
-- Definition of schoolclasses
-- 

DROP TABLE IF EXISTS `schoolclasses`;
CREATE TABLE IF NOT EXISTS `schoolclasses` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` longtext NOT NULL,
  `Elective` tinyint(1) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table schoolclasses
-- 

/*!40000 ALTER TABLE `schoolclasses` DISABLE KEYS */;
/*!40000 ALTER TABLE `schoolclasses` ENABLE KEYS */;

-- 
-- Definition of sessionyears
-- 

DROP TABLE IF EXISTS `sessionyears`;
CREATE TABLE IF NOT EXISTS `sessionyears` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` longtext NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table sessionyears
-- 

/*!40000 ALTER TABLE `sessionyears` DISABLE KEYS */;
/*!40000 ALTER TABLE `sessionyears` ENABLE KEYS */;

-- 
-- Definition of expendituretables
-- 

DROP TABLE IF EXISTS `expendituretables`;
CREATE TABLE IF NOT EXISTS `expendituretables` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Description` longtext DEFAULT NULL,
  `Amount` double NOT NULL,
  `TransactionDate` datetime(6) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  `ApplicationUserId` varchar(255) NOT NULL,
  `SessionYearId` int(11) NOT NULL,
  `Term` longtext NOT NULL,
  `AccountClose` tinyint(1) NOT NULL,
  `ExpendId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ExpenditureTables_ApplicationUserId` (`ApplicationUserId`),
  KEY `IX_ExpenditureTables_ExpendId` (`ExpendId`),
  KEY `IX_ExpenditureTables_SessionYearId` (`SessionYearId`),
  CONSTRAINT `FK_ExpenditureTables_AspNetUsers_ApplicationUserId` FOREIGN KEY (`ApplicationUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ExpenditureTables_ExpenditureType_ExpendId` FOREIGN KEY (`ExpendId`) REFERENCES `expendituretype` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ExpenditureTables_SessionYears_SessionYearId` FOREIGN KEY (`SessionYearId`) REFERENCES `sessionyears` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table expendituretables
-- 

/*!40000 ALTER TABLE `expendituretables` DISABLE KEYS */;
/*!40000 ALTER TABLE `expendituretables` ENABLE KEYS */;

-- 
-- Definition of expenditurefiles
-- 

DROP TABLE IF EXISTS `expenditurefiles`;
CREATE TABLE IF NOT EXISTS `expenditurefiles` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(250) DEFAULT NULL,
  `filePath` varchar(250) NOT NULL,
  `ExpenditureId` int(11) NOT NULL,
  `AccountClose` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ExpenditureFiles_ExpenditureId` (`ExpenditureId`),
  CONSTRAINT `FK_ExpenditureFiles_ExpenditureTables_ExpenditureId` FOREIGN KEY (`ExpenditureId`) REFERENCES `expendituretables` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table expenditurefiles
-- 

/*!40000 ALTER TABLE `expenditurefiles` DISABLE KEYS */;
/*!40000 ALTER TABLE `expenditurefiles` ENABLE KEYS */;

-- 
-- Definition of ptaexpendituretables
-- 

DROP TABLE IF EXISTS `ptaexpendituretables`;
CREATE TABLE IF NOT EXISTS `ptaexpendituretables` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Description` longtext DEFAULT NULL,
  `Amount` double NOT NULL,
  `TransactionDate` datetime(6) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  `ApplicationUserId` varchar(255) NOT NULL,
  `SessionYearId` int(11) NOT NULL,
  `Term` longtext NOT NULL,
  `ExpedId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_PTAExpenditureTables_ApplicationUserId` (`ApplicationUserId`),
  KEY `IX_PTAExpenditureTables_ExpedId` (`ExpedId`),
  KEY `IX_PTAExpenditureTables_SessionYearId` (`SessionYearId`),
  CONSTRAINT `FK_PTAExpenditureTables_AspNetUsers_ApplicationUserId` FOREIGN KEY (`ApplicationUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_PTAExpenditureTables_ExpenditureType_ExpedId` FOREIGN KEY (`ExpedId`) REFERENCES `expendituretype` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_PTAExpenditureTables_SessionYears_SessionYearId` FOREIGN KEY (`SessionYearId`) REFERENCES `sessionyears` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table ptaexpendituretables
-- 

/*!40000 ALTER TABLE `ptaexpendituretables` DISABLE KEYS */;
/*!40000 ALTER TABLE `ptaexpendituretables` ENABLE KEYS */;

-- 
-- Definition of ptafees
-- 

DROP TABLE IF EXISTS `ptafees`;
CREATE TABLE IF NOT EXISTS `ptafees` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Fees` double NOT NULL,
  `SessionYearId` int(11) NOT NULL,
  `ApplicationUserId` varchar(255) DEFAULT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  `Term` varchar(20) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_PTAfees_ApplicationUserId` (`ApplicationUserId`),
  KEY `IX_PTAfees_SessionYearId` (`SessionYearId`),
  CONSTRAINT `FK_PTAfees_AspNetUsers_ApplicationUserId` FOREIGN KEY (`ApplicationUserId`) REFERENCES `aspnetusers` (`Id`),
  CONSTRAINT `FK_PTAfees_SessionYears_SessionYearId` FOREIGN KEY (`SessionYearId`) REFERENCES `sessionyears` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table ptafees
-- 

/*!40000 ALTER TABLE `ptafees` DISABLE KEYS */;
/*!40000 ALTER TABLE `ptafees` ENABLE KEYS */;

-- 
-- Definition of settings
-- 

DROP TABLE IF EXISTS `settings`;
CREATE TABLE IF NOT EXISTS `settings` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `sessionYear` int(11) DEFAULT NULL,
  `Term` varchar(20) DEFAULT NULL,
  `Classes` int(11) DEFAULT NULL,
  `subclass` int(11) DEFAULT NULL,
  `ApplicationUserId` varchar(452) DEFAULT NULL,
  `CashierSignature` longtext DEFAULT NULL,
  `PrincipalSignature` longtext DEFAULT NULL,
  `PrincipalName` longtext DEFAULT NULL,
  `CanPrintResult` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Settings_ApplicationUserId` (`ApplicationUserId`),
  CONSTRAINT `FK_Settings_AspNetUsers_ApplicationUserId` FOREIGN KEY (`ApplicationUserId`) REFERENCES `aspnetusers` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table settings
-- 

/*!40000 ALTER TABLE `settings` DISABLE KEYS */;
/*!40000 ALTER TABLE `settings` ENABLE KEYS */;

-- 
-- Definition of smstable
-- 

DROP TABLE IF EXISTS `smstable`;
CREATE TABLE IF NOT EXISTS `smstable` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ApplicationUserId` varchar(150) NOT NULL,
  `Sender` varchar(12) NOT NULL,
  `Subject` varchar(250) NOT NULL,
  `Message` varchar(1000) NOT NULL,
  `SmsCount` int(11) NOT NULL,
  `Cost` double NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_SmsTable_ApplicationUserId` (`ApplicationUserId`),
  CONSTRAINT `FK_SmsTable_AspNetUsers_ApplicationUserId` FOREIGN KEY (`ApplicationUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table smstable
-- 

/*!40000 ALTER TABLE `smstable` DISABLE KEYS */;
/*!40000 ALTER TABLE `smstable` ENABLE KEYS */;

-- 
-- Definition of specialpaysetups
-- 

DROP TABLE IF EXISTS `specialpaysetups`;
CREATE TABLE IF NOT EXISTS `specialpaysetups` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SessionYearId` int(11) NOT NULL,
  `ClassesInSchoolId` int(11) NOT NULL,
  `Term` varchar(20) NOT NULL,
  `Amount` double NOT NULL,
  `OtherPayId` int(11) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_SpecialPaySetUps_ClassesInSchoolId` (`ClassesInSchoolId`),
  KEY `IX_SpecialPaySetUps_OtherPayId` (`OtherPayId`),
  KEY `IX_SpecialPaySetUps_SessionYearId` (`SessionYearId`),
  CONSTRAINT `FK_SpecialPaySetUps_OtherPayTables_OtherPayId` FOREIGN KEY (`OtherPayId`) REFERENCES `otherpaytables` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_SpecialPaySetUps_SchoolClasses_ClassesInSchoolId` FOREIGN KEY (`ClassesInSchoolId`) REFERENCES `schoolclasses` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_SpecialPaySetUps_SessionYears_SessionYearId` FOREIGN KEY (`SessionYearId`) REFERENCES `sessionyears` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table specialpaysetups
-- 

/*!40000 ALTER TABLE `specialpaysetups` DISABLE KEYS */;
/*!40000 ALTER TABLE `specialpaysetups` ENABLE KEYS */;

-- 
-- Definition of studenttable
-- 

DROP TABLE IF EXISTS `studenttable`;
CREATE TABLE IF NOT EXISTS `studenttable` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `StudentRegNo` varchar(50) DEFAULT NULL,
  `FirstName` varchar(50) NOT NULL,
  `OtherName` varchar(50) DEFAULT NULL,
  `SurName` varchar(50) NOT NULL,
  `Gender` varchar(10) NOT NULL,
  `Address` varchar(150) NOT NULL,
  `DateOfBirth` datetime(6) NOT NULL,
  `State` varchar(50) NOT NULL,
  `LocalGov` varchar(50) NOT NULL,
  `Passport` longtext DEFAULT NULL,
  `UserID` varchar(150) NOT NULL,
  `ApplicationUserId` varchar(255) NOT NULL,
  `SessionYearId` int(11) DEFAULT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_StudentTable_ApplicationUserId` (`ApplicationUserId`),
  KEY `IX_StudentTable_SessionYearId` (`SessionYearId`),
  CONSTRAINT `FK_StudentTable_AspNetUsers_ApplicationUserId` FOREIGN KEY (`ApplicationUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_StudentTable_SessionYears_SessionYearId` FOREIGN KEY (`SessionYearId`) REFERENCES `sessionyears` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table studenttable
-- 

/*!40000 ALTER TABLE `studenttable` DISABLE KEYS */;
/*!40000 ALTER TABLE `studenttable` ENABLE KEYS */;

-- 
-- Definition of parentstudents
-- 

DROP TABLE IF EXISTS `parentstudents`;
CREATE TABLE IF NOT EXISTS `parentstudents` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `StudentsdataId` int(11) NOT NULL,
  `ParentTableId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_ParentStudents_ParentTableId` (`ParentTableId`),
  KEY `IX_ParentStudents_StudentsdataId` (`StudentsdataId`),
  CONSTRAINT `FK_ParentStudents_ParentTables_ParentTableId` FOREIGN KEY (`ParentTableId`) REFERENCES `parenttables` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ParentStudents_StudentTable_StudentsdataId` FOREIGN KEY (`StudentsdataId`) REFERENCES `studenttable` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table parentstudents
-- 

/*!40000 ALTER TABLE `parentstudents` DISABLE KEYS */;
/*!40000 ALTER TABLE `parentstudents` ENABLE KEYS */;

-- 
-- Definition of subclasses
-- 

DROP TABLE IF EXISTS `subclasses`;
CREATE TABLE IF NOT EXISTS `subclasses` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` longtext NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table subclasses
-- 

/*!40000 ALTER TABLE `subclasses` DISABLE KEYS */;
/*!40000 ALTER TABLE `subclasses` ENABLE KEYS */;

-- 
-- Definition of generalclasstables
-- 

DROP TABLE IF EXISTS `generalclasstables`;
CREATE TABLE IF NOT EXISTS `generalclasstables` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Term` longtext NOT NULL,
  `SessionYearId` int(11) NOT NULL,
  `ClassId` int(11) NOT NULL,
  `SubClassId` int(11) NOT NULL,
  `Next_Term_Fees` double NOT NULL,
  `TermEnd` datetime(6) NOT NULL,
  `NextTermStart` datetime(6) NOT NULL,
  `ClassTeacher` varchar(60) NOT NULL,
  `TotalAttendance` double NOT NULL,
  `ExamOfficerID` longtext DEFAULT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `PrincipalName` varchar(60) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_GeneralClassTables_ClassId` (`ClassId`),
  KEY `IX_GeneralClassTables_SessionYearId` (`SessionYearId`),
  KEY `IX_GeneralClassTables_SubClassId` (`SubClassId`),
  CONSTRAINT `FK_GeneralClassTables_SchoolClasses_ClassId` FOREIGN KEY (`ClassId`) REFERENCES `schoolclasses` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_GeneralClassTables_SessionYears_SessionYearId` FOREIGN KEY (`SessionYearId`) REFERENCES `sessionyears` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_GeneralClassTables_SubClasses_SubClassId` FOREIGN KEY (`SubClassId`) REFERENCES `subclasses` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table generalclasstables
-- 

/*!40000 ALTER TABLE `generalclasstables` DISABLE KEYS */;
/*!40000 ALTER TABLE `generalclasstables` ENABLE KEYS */;

-- 
-- Definition of subjects
-- 

DROP TABLE IF EXISTS `subjects`;
CREATE TABLE IF NOT EXISTS `subjects` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(150) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  `Register_State` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table subjects
-- 

/*!40000 ALTER TABLE `subjects` DISABLE KEYS */;
/*!40000 ALTER TABLE `subjects` ENABLE KEYS */;

-- 
-- Definition of teammanagements
-- 

DROP TABLE IF EXISTS `teammanagements`;
CREATE TABLE IF NOT EXISTS `teammanagements` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Fullname` varchar(100) NOT NULL,
  `Position` varchar(60) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Phone` varchar(11) NOT NULL,
  `Note` longtext NOT NULL,
  `ImagePath` varchar(1000) NOT NULL,
  `Hierarchy` int(11) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table teammanagements
-- 

/*!40000 ALTER TABLE `teammanagements` DISABLE KEYS */;
/*!40000 ALTER TABLE `teammanagements` ENABLE KEYS */;

-- 
-- Definition of termlyfeespays
-- 

DROP TABLE IF EXISTS `termlyfeespays`;
CREATE TABLE IF NOT EXISTS `termlyfeespays` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Amount` double NOT NULL,
  `SessionYearId` int(11) NOT NULL,
  `ClassesInSchoolId` int(11) NOT NULL,
  `Term` varchar(20) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_TermLyFeesPays_ClassesInSchoolId` (`ClassesInSchoolId`),
  KEY `IX_TermLyFeesPays_SessionYearId` (`SessionYearId`),
  CONSTRAINT `FK_TermLyFeesPays_SchoolClasses_ClassesInSchoolId` FOREIGN KEY (`ClassesInSchoolId`) REFERENCES `schoolclasses` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_TermLyFeesPays_SessionYears_SessionYearId` FOREIGN KEY (`SessionYearId`) REFERENCES `sessionyears` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table termlyfeespays
-- 

/*!40000 ALTER TABLE `termlyfeespays` DISABLE KEYS */;
/*!40000 ALTER TABLE `termlyfeespays` ENABLE KEYS */;

-- 
-- Definition of termregistrations
-- 

DROP TABLE IF EXISTS `termregistrations`;
CREATE TABLE IF NOT EXISTS `termregistrations` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `StudentId` int(11) NOT NULL,
  `SessionYearId` int(11) NOT NULL,
  `ClassesInSchoolId` int(11) NOT NULL,
  `SubClassId` int(11) NOT NULL,
  `Term` varchar(20) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  `ResultIsReady` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_TermRegistrations_ClassesInSchoolId` (`ClassesInSchoolId`),
  KEY `IX_TermRegistrations_SessionYearId` (`SessionYearId`),
  KEY `IX_TermRegistrations_StudentId` (`StudentId`),
  KEY `IX_TermRegistrations_SubClassId` (`SubClassId`),
  CONSTRAINT `FK_TermRegistrations_SchoolClasses_ClassesInSchoolId` FOREIGN KEY (`ClassesInSchoolId`) REFERENCES `schoolclasses` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_TermRegistrations_SessionYears_SessionYearId` FOREIGN KEY (`SessionYearId`) REFERENCES `sessionyears` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_TermRegistrations_StudentTable_StudentId` FOREIGN KEY (`StudentId`) REFERENCES `studenttable` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_TermRegistrations_SubClasses_SubClassId` FOREIGN KEY (`SubClassId`) REFERENCES `subclasses` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table termregistrations
-- 

/*!40000 ALTER TABLE `termregistrations` DISABLE KEYS */;
/*!40000 ALTER TABLE `termregistrations` ENABLE KEYS */;

-- 
-- Definition of additionalpayments
-- 

DROP TABLE IF EXISTS `additionalpayments`;
CREATE TABLE IF NOT EXISTS `additionalpayments` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Amount` double NOT NULL,
  `TermRegId` int(11) NOT NULL,
  `CasheirId` varchar(450) DEFAULT NULL,
  `SpecialpayId` int(11) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AdditionalPayments_CasheirId` (`CasheirId`),
  KEY `IX_AdditionalPayments_SpecialpayId` (`SpecialpayId`),
  KEY `IX_AdditionalPayments_TermRegId` (`TermRegId`),
  CONSTRAINT `FK_AdditionalPayments_AspNetUsers_CasheirId` FOREIGN KEY (`CasheirId`) REFERENCES `aspnetusers` (`Id`),
  CONSTRAINT `FK_AdditionalPayments_SpecialPaySetUps_SpecialpayId` FOREIGN KEY (`SpecialpayId`) REFERENCES `specialpaysetups` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AdditionalPayments_TermRegistrations_TermRegId` FOREIGN KEY (`TermRegId`) REFERENCES `termregistrations` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table additionalpayments
-- 

/*!40000 ALTER TABLE `additionalpayments` DISABLE KEYS */;
/*!40000 ALTER TABLE `additionalpayments` ENABLE KEYS */;

-- 
-- Definition of paymentreports
-- 

DROP TABLE IF EXISTS `paymentreports`;
CREATE TABLE IF NOT EXISTS `paymentreports` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AccummulatedPay` double NOT NULL,
  `Status` longtext DEFAULT NULL,
  `TermRegId` int(11) NOT NULL,
  `TermlyFeesId` int(11) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_PaymentReports_TermlyFeesId` (`TermlyFeesId`),
  KEY `IX_PaymentReports_TermRegId` (`TermRegId`),
  CONSTRAINT `FK_PaymentReports_TermLyFeesPays_TermlyFeesId` FOREIGN KEY (`TermlyFeesId`) REFERENCES `termlyfeespays` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_PaymentReports_TermRegistrations_TermRegId` FOREIGN KEY (`TermRegId`) REFERENCES `termregistrations` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table paymentreports
-- 

/*!40000 ALTER TABLE `paymentreports` DISABLE KEYS */;
/*!40000 ALTER TABLE `paymentreports` ENABLE KEYS */;

-- 
-- Definition of payments
-- 

DROP TABLE IF EXISTS `payments`;
CREATE TABLE IF NOT EXISTS `payments` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Paid` double NOT NULL,
  `Balance` double NOT NULL,
  `Description` varchar(200) DEFAULT NULL,
  `Status` varchar(20) DEFAULT NULL,
  `TermlyFeesId` int(11) NOT NULL,
  `TermRegId` int(11) NOT NULL,
  `ReceiptNo` varchar(100) NOT NULL,
  `Cashier` varchar(100) DEFAULT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `UpdatedDate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Payments_TermlyFeesId` (`TermlyFeesId`),
  KEY `IX_Payments_TermRegId` (`TermRegId`),
  CONSTRAINT `FK_Payments_TermLyFeesPays_TermlyFeesId` FOREIGN KEY (`TermlyFeesId`) REFERENCES `termlyfeespays` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Payments_TermRegistrations_TermRegId` FOREIGN KEY (`TermRegId`) REFERENCES `termregistrations` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table payments
-- 

/*!40000 ALTER TABLE `payments` DISABLE KEYS */;
/*!40000 ALTER TABLE `payments` ENABLE KEYS */;

-- 
-- Definition of ptafeesreports
-- 

DROP TABLE IF EXISTS `ptafeesreports`;
CREATE TABLE IF NOT EXISTS `ptafeesreports` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CulmulativePay` double NOT NULL,
  `Status` longtext NOT NULL,
  `TermRegId` int(11) NOT NULL,
  `PTAfeesId` int(11) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_PTAFeesReports_PTAfeesId` (`PTAfeesId`),
  KEY `IX_PTAFeesReports_TermRegId` (`TermRegId`),
  CONSTRAINT `FK_PTAFeesReports_PTAfees_PTAfeesId` FOREIGN KEY (`PTAfeesId`) REFERENCES `ptafees` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_PTAFeesReports_TermRegistrations_TermRegId` FOREIGN KEY (`TermRegId`) REFERENCES `termregistrations` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table ptafeesreports
-- 

/*!40000 ALTER TABLE `ptafeesreports` DISABLE KEYS */;
/*!40000 ALTER TABLE `ptafeesreports` ENABLE KEYS */;

-- 
-- Definition of ptatables
-- 

DROP TABLE IF EXISTS `ptatables`;
CREATE TABLE IF NOT EXISTS `ptatables` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Amount` double NOT NULL,
  `Balance` double NOT NULL,
  `Status` longtext NOT NULL,
  `PaymentDate` datetime(6) NOT NULL,
  `TermRegId` int(11) NOT NULL,
  `PTAFeesId` int(11) NOT NULL,
  `ReceiptNo` longtext NOT NULL,
  `Description` varchar(200) DEFAULT NULL,
  `ApplicationUserId` varchar(255) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_PTATables_ApplicationUserId` (`ApplicationUserId`),
  KEY `IX_PTATables_PTAFeesId` (`PTAFeesId`),
  KEY `IX_PTATables_TermRegId` (`TermRegId`),
  CONSTRAINT `FK_PTATables_AspNetUsers_ApplicationUserId` FOREIGN KEY (`ApplicationUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_PTATables_PTAfees_PTAFeesId` FOREIGN KEY (`PTAFeesId`) REFERENCES `ptafees` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_PTATables_TermRegistrations_TermRegId` FOREIGN KEY (`TermRegId`) REFERENCES `termregistrations` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table ptatables
-- 

/*!40000 ALTER TABLE `ptatables` DISABLE KEYS */;
/*!40000 ALTER TABLE `ptatables` ENABLE KEYS */;

-- 
-- Definition of remarkpositions
-- 

DROP TABLE IF EXISTS `remarkpositions`;
CREATE TABLE IF NOT EXISTS `remarkpositions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Student_Attendance` double DEFAULT NULL,
  `Absent` varchar(10) DEFAULT NULL,
  `Total_Marks_Obtain` double DEFAULT NULL,
  `Position_In_Class` int(11) DEFAULT NULL,
  `General_Remark` varchar(250) DEFAULT NULL,
  `Principal_Remark` varchar(250) DEFAULT NULL,
  `AddedBy` varchar(250) DEFAULT NULL,
  `P_Status` tinyint(1) NOT NULL,
  `R_Status` tinyint(1) NOT NULL,
  `TermRegId` int(11) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_RemarkPositions_TermRegId` (`TermRegId`),
  CONSTRAINT `FK_RemarkPositions_TermRegistrations_TermRegId` FOREIGN KEY (`TermRegId`) REFERENCES `termregistrations` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table remarkpositions
-- 

/*!40000 ALTER TABLE `remarkpositions` DISABLE KEYS */;
/*!40000 ALTER TABLE `remarkpositions` ENABLE KEYS */;

-- 
-- Definition of resulttable
-- 

DROP TABLE IF EXISTS `resulttable`;
CREATE TABLE IF NOT EXISTS `resulttable` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Assignment` double DEFAULT NULL,
  `Test` double DEFAULT NULL,
  `Project` double DEFAULT NULL,
  `ClassWork` double DEFAULT NULL,
  `Examination` double DEFAULT NULL,
  `Total` double DEFAULT NULL,
  `Position` int(11) DEFAULT NULL,
  `Grade` varchar(4) DEFAULT NULL,
  `Remark` varchar(20) DEFAULT NULL,
  `SubjectId` int(11) NOT NULL,
  `TermRegId` int(11) NOT NULL,
  `ExamsOfficer` longtext DEFAULT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  `Status` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ResultTable_SubjectId` (`SubjectId`),
  KEY `IX_ResultTable_TermRegId` (`TermRegId`),
  CONSTRAINT `FK_ResultTable_Subjects_SubjectId` FOREIGN KEY (`SubjectId`) REFERENCES `subjects` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ResultTable_TermRegistrations_TermRegId` FOREIGN KEY (`TermRegId`) REFERENCES `termregistrations` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table resulttable
-- 

/*!40000 ALTER TABLE `resulttable` DISABLE KEYS */;
/*!40000 ALTER TABLE `resulttable` ENABLE KEYS */;

-- 
-- Definition of studentratings
-- 

DROP TABLE IF EXISTS `studentratings`;
CREATE TABLE IF NOT EXISTS `studentratings` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Attentiveness` tinyint(3) unsigned DEFAULT NULL,
  `Attendance` tinyint(3) unsigned DEFAULT NULL,
  `Reliability` tinyint(3) unsigned DEFAULT NULL,
  `Punctuality` tinyint(3) unsigned NOT NULL,
  `Perseverance` tinyint(3) unsigned DEFAULT NULL,
  `Neatness` tinyint(3) unsigned DEFAULT NULL,
  `Sense_of_Responsibility` tinyint(3) unsigned DEFAULT NULL,
  `Politeness` tinyint(3) unsigned DEFAULT NULL,
  `Spirit_of_Cooperation` tinyint(3) unsigned DEFAULT NULL,
  `SelfControl` tinyint(3) unsigned DEFAULT NULL,
  `Relationship_With_Student` tinyint(3) unsigned DEFAULT NULL,
  `Relation_With_Staff` tinyint(3) unsigned DEFAULT NULL,
  `Curiosity` tinyint(3) unsigned DEFAULT NULL,
  `Initiative` tinyint(3) unsigned DEFAULT NULL,
  `Honesty` tinyint(3) unsigned DEFAULT NULL,
  `Industry` tinyint(3) unsigned DEFAULT NULL,
  `Humility` tinyint(3) unsigned DEFAULT NULL,
  `Organisational_Ability` tinyint(3) unsigned DEFAULT NULL,
  `Tolanrance` tinyint(3) unsigned DEFAULT NULL,
  `Leadership` tinyint(3) unsigned DEFAULT NULL,
  `Respect_For_Other` tinyint(3) unsigned DEFAULT NULL,
  `Courage` tinyint(3) unsigned DEFAULT NULL,
  `Handwriting` tinyint(3) unsigned DEFAULT NULL,
  `Fluecy` tinyint(3) unsigned DEFAULT NULL,
  `Drawing_Painting` tinyint(3) unsigned DEFAULT NULL,
  `Handing_WShop_Tool` tinyint(3) unsigned DEFAULT NULL,
  `Games_Sport` tinyint(3) unsigned DEFAULT NULL,
  `Musical_Skill` tinyint(3) unsigned DEFAULT NULL,
  `Constrution` tinyint(3) unsigned DEFAULT NULL,
  `TermRegId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_StudentRatings_TermRegId` (`TermRegId`),
  CONSTRAINT `FK_StudentRatings_TermRegistrations_TermRegId` FOREIGN KEY (`TermRegId`) REFERENCES `termregistrations` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table studentratings
-- 

/*!40000 ALTER TABLE `studentratings` DISABLE KEYS */;
/*!40000 ALTER TABLE `studentratings` ENABLE KEYS */;

-- 
-- Definition of visitors
-- 

DROP TABLE IF EXISTS `visitors`;
CREATE TABLE IF NOT EXISTS `visitors` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Website` int(11) NOT NULL,
  `Portal` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- 
-- Dumping data for table visitors
-- 

/*!40000 ALTER TABLE `visitors` DISABLE KEYS */;
/*!40000 ALTER TABLE `visitors` ENABLE KEYS */;


/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2026-01-29 20:12:18
-- Total time: 0:0:0:3:249 (d:h:m:s:ms)
