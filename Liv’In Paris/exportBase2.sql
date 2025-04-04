CREATE DATABASE  IF NOT EXISTS `liv_in_paris` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `liv_in_paris`;
-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: liv_in_paris
-- ------------------------------------------------------
-- Server version	8.0.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `clients`
--

DROP TABLE IF EXISTS `clients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `clients` (
  `Email` varchar(50) NOT NULL,
  `Nom` varchar(50) NOT NULL,
  `Prenom` varchar(50) NOT NULL,
  `Rue` varchar(50) DEFAULT NULL,
  `Numero_rue` varchar(50) DEFAULT NULL,
  `CodePostal` varchar(50) DEFAULT NULL,
  `Ville` varchar(50) DEFAULT NULL,
  `Tel` varchar(50) NOT NULL,
  `Metro_le_plus_proche` varchar(50) NOT NULL,
  `ID` int NOT NULL,
  PRIMARY KEY (`Email`),
  UNIQUE KEY `Clients_Compte_AK` (`ID`),
  CONSTRAINT `Clients_Compte_FK` FOREIGN KEY (`ID`) REFERENCES `compte` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clients`
--

LOCK TABLES `clients` WRITE;
/*!40000 ALTER TABLE `clients` DISABLE KEYS */;
INSERT INTO `clients` VALUES ('alex@example.com','alex','fath',NULL,NULL,NULL,NULL,'0768243263','chatelet',9),('clara.legrand@gmail.com','Legrand','Clara','Rue du Marché','10','75006','Paris','0606060708','Les Halles',7),('emma.jacques@gmail.com','Jacques','Emma','Place des Vosges','5','75004','Paris','0604040506','Bastille',6),('lucas.benoit@gmail.com','Benoit','Lucas','Boulevard de la République','78','75003','Paris','0603040506','Nation',3),('matt@example.com','Matthieu','fecamp',NULL,NULL,NULL,NULL,'0768243263','la defense',10),('michael.brown@gmail.com','Brown','Michael','Chemin du Gourmet','22','75005','Paris','0605050607','Saint-Germain',5),('sophie.martin@gmail.com','Martin','Sophie','Avenue du Chef','34','75002','Paris','0602030405','République',2),('thomas.dupont@gmail.com','Dupont','Thomas','Rue des Fleurs','12','75001','Paris','0601020304','Châtelet',1);
/*!40000 ALTER TABLE `clients` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `commandes`
--

DROP TABLE IF EXISTS `commandes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `commandes` (
  `id_commande` int NOT NULL AUTO_INCREMENT,
  `Date_Fabrication` date NOT NULL,
  `Date_Peremption` date NOT NULL,
  `id_consommateur` int NOT NULL,
  `id_cuisinier` int NOT NULL,
  PRIMARY KEY (`id_commande`),
  KEY `Commandes_Consommateur_FK` (`id_consommateur`),
  KEY `Commandes_cuisinier_FK` (`id_cuisinier`),
  CONSTRAINT `Commandes_Consommateur_FK` FOREIGN KEY (`id_consommateur`) REFERENCES `consommateur` (`id_consommateur`),
  CONSTRAINT `Commandes_cuisinier_FK` FOREIGN KEY (`id_cuisinier`) REFERENCES `cuisinier` (`id_cuisinier`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `commandes`
--

LOCK TABLES `commandes` WRITE;
/*!40000 ALTER TABLE `commandes` DISABLE KEYS */;
INSERT INTO `commandes` VALUES (1,'2025-02-25','2025-03-02',1,1),(2,'2025-02-26','2025-03-03',2,2),(3,'2025-02-27','2025-03-05',3,3),(4,'2025-02-25','2025-03-02',4,2),(5,'2025-02-28','2025-03-05',5,6),(6,'2025-03-01','2025-03-08',2,5),(7,'2025-03-01','2025-03-08',3,3),(8,'2025-03-02','2025-03-09',6,7),(9,'2025-04-04','2025-04-09',1,1);
/*!40000 ALTER TABLE `commandes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `compose`
--

DROP TABLE IF EXISTS `compose`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `compose` (
  `Id_met` int NOT NULL AUTO_INCREMENT,
  `ingredient` varchar(50) NOT NULL,
  PRIMARY KEY (`Id_met`,`ingredient`),
  KEY `compose_ingredient_FK` (`ingredient`),
  CONSTRAINT `compose_ingredient_FK` FOREIGN KEY (`ingredient`) REFERENCES `ingredient` (`ingredient`),
  CONSTRAINT `compose_mets_FK` FOREIGN KEY (`Id_met`) REFERENCES `mets` (`Id_met`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `compose`
--

LOCK TABLES `compose` WRITE;
/*!40000 ALTER TABLE `compose` DISABLE KEYS */;
INSERT INTO `compose` VALUES (1,'Tomate');
/*!40000 ALTER TABLE `compose` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `compose_commande`
--

DROP TABLE IF EXISTS `compose_commande`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `compose_commande` (
  `id_commande` int NOT NULL,
  `Id_met` int NOT NULL,
  `Quantite` int NOT NULL DEFAULT '1',
  PRIMARY KEY (`id_commande`,`Id_met`),
  KEY `compose_commande_mets_FK` (`Id_met`),
  CONSTRAINT `compose_commande_Commandes_FK` FOREIGN KEY (`id_commande`) REFERENCES `commandes` (`id_commande`) ON DELETE CASCADE,
  CONSTRAINT `compose_commande_mets_FK` FOREIGN KEY (`Id_met`) REFERENCES `mets` (`Id_met`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `compose_commande`
--

LOCK TABLES `compose_commande` WRITE;
/*!40000 ALTER TABLE `compose_commande` DISABLE KEYS */;
INSERT INTO `compose_commande` VALUES (1,1,1),(2,2,1),(3,3,2),(4,4,1),(5,5,1),(6,6,1),(7,7,1),(8,8,1),(9,1,2);
/*!40000 ALTER TABLE `compose_commande` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `compte`
--

DROP TABLE IF EXISTS `compte`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `compte` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Mdp` varchar(50) NOT NULL,
  `Ban` tinyint(1) DEFAULT '0',
  `est_utilisateur` tinyint(1) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `compte`
--

LOCK TABLES `compte` WRITE;
/*!40000 ALTER TABLE `compte` DISABLE KEYS */;
INSERT INTO `compte` VALUES (1,'mdp123',0,1),(2,'securepass',0,1),(3,'chefpass',0,1),(4,'businesspass',0,0),(5,'foodlover',0,1),(6,'chef2023',0,1),(7,'gourmetchef',0,1),(8,'urbanrestaurant',0,0),(9,'securepassword',0,1),(10,'azerty',0,1);
/*!40000 ALTER TABLE `compte` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `consommateur`
--

DROP TABLE IF EXISTS `consommateur`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `consommateur` (
  `id_consommateur` int NOT NULL AUTO_INCREMENT,
  `Note` float DEFAULT NULL,
  `nombre_notes` int DEFAULT '0',
  `ID` int NOT NULL,
  PRIMARY KEY (`id_consommateur`),
  UNIQUE KEY `Consommateur_Compte_AK` (`ID`),
  CONSTRAINT `Consommateur_Compte_FK` FOREIGN KEY (`ID`) REFERENCES `compte` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `consommateur`
--

LOCK TABLES `consommateur` WRITE;
/*!40000 ALTER TABLE `consommateur` DISABLE KEYS */;
INSERT INTO `consommateur` VALUES (1,3.5,2,1),(2,4,2,2),(3,5,2,3),(4,3,1,4),(5,5,1,5),(6,5,1,7),(7,NULL,0,9);
/*!40000 ALTER TABLE `consommateur` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cuisinier`
--

DROP TABLE IF EXISTS `cuisinier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cuisinier` (
  `id_cuisinier` int NOT NULL AUTO_INCREMENT,
  `nom_cuisinier` varchar(50) NOT NULL,
  `Note` float DEFAULT NULL,
  `nombre_notes` int DEFAULT '0',
  `ID` int NOT NULL,
  PRIMARY KEY (`id_cuisinier`),
  UNIQUE KEY `cuisinier_Compte_AK` (`ID`),
  CONSTRAINT `cuisinier_Compte_FK` FOREIGN KEY (`ID`) REFERENCES `compte` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cuisinier`
--

LOCK TABLES `cuisinier` WRITE;
/*!40000 ALTER TABLE `cuisinier` DISABLE KEYS */;
INSERT INTO `cuisinier` VALUES (1,'Le Chef Thomas',4,2,1),(2,'Sophie Cuisinière',4.5,2,2),(3,'Cuisine de Lucas',5,2,3),(4,'Emma la Gourmet',NULL,0,6),(5,'Michael le Gourmet',4,1,5),(6,'Urban Restaurant',5,1,8),(7,'Clara la Cuisine',5,1,7),(8,'Chef A',NULL,0,10);
/*!40000 ALTER TABLE `cuisinier` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `entreprise`
--

DROP TABLE IF EXISTS `entreprise`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `entreprise` (
  `nom_entreprise` varchar(50) NOT NULL,
  `nom_referent` varchar(50) NOT NULL,
  `ID` int NOT NULL,
  `Metro_le_plus_proche` varchar(50) NOT NULL,
  PRIMARY KEY (`nom_entreprise`),
  UNIQUE KEY `Entreprise_Compte_AK` (`ID`),
  CONSTRAINT `Entreprise_Compte_FK` FOREIGN KEY (`ID`) REFERENCES `compte` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `entreprise`
--

LOCK TABLES `entreprise` WRITE;
/*!40000 ALTER TABLE `entreprise` DISABLE KEYS */;
INSERT INTO `entreprise` VALUES ('La Bonne Table','Paul Lambert',4,'Opéra'),('Urban Bites','Julia Martin',8,'Gare de Lyon');
/*!40000 ALTER TABLE `entreprise` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ingredient`
--

DROP TABLE IF EXISTS `ingredient`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ingredient` (
  `ingredient` varchar(50) NOT NULL,
  PRIMARY KEY (`ingredient`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ingredient`
--

LOCK TABLES `ingredient` WRITE;
/*!40000 ALTER TABLE `ingredient` DISABLE KEYS */;
INSERT INTO `ingredient` VALUES ('Tomate');
/*!40000 ALTER TABLE `ingredient` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mets`
--

DROP TABLE IF EXISTS `mets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mets` (
  `Id_met` int NOT NULL AUTO_INCREMENT,
  `Nom_plat` varchar(50) NOT NULL,
  `prix` decimal(10,2) NOT NULL,
  `type_de_plat` varchar(50) DEFAULT 'plat',
  `image` blob,
  `Régime_Alimentaire` varchar(50) DEFAULT NULL,
  `origine_plat` varchar(50) DEFAULT NULL,
  `pour_combien` int DEFAULT '1',
  `id_cuisinier` int NOT NULL,
  PRIMARY KEY (`Id_met`),
  KEY `mets_cuisinier_FK` (`id_cuisinier`),
  CONSTRAINT `mets_cuisinier_FK` FOREIGN KEY (`id_cuisinier`) REFERENCES `cuisinier` (`id_cuisinier`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mets`
--

LOCK TABLES `mets` WRITE;
/*!40000 ALTER TABLE `mets` DISABLE KEYS */;
INSERT INTO `mets` VALUES (1,'Lasagnes',12.00,'plat',NULL,NULL,'Italien',1,1),(2,'Sushi',15.00,'plat',NULL,NULL,'Japonais',1,2),(3,'Boeuf Bourguignon',18.00,'plat',NULL,NULL,'Français',1,3),(4,'Tacos',10.00,'plat',NULL,NULL,'Mexicain',1,2),(5,'Coq au Vin',20.00,'plat',NULL,NULL,'Français',1,6),(6,'Ramen',14.00,'plat',NULL,NULL,'Japonais',1,5),(7,'Pizza Margherita',11.00,'plat',NULL,NULL,'Italien',1,3),(8,'Burgers Maison',13.00,'plat',NULL,NULL,'Américain',1,7),(9,'Pizza',10.00,'Plat principal',NULL,'Végétarien','Italienne',1,1);
/*!40000 ALTER TABLE `mets` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rating`
--

DROP TABLE IF EXISTS `rating`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rating` (
  `id_rating` int NOT NULL AUTO_INCREMENT,
  `note_client` float DEFAULT NULL,
  `commentaire_client` longtext,
  `note_cuisinier` float DEFAULT NULL,
  `commentaire_cuisinier` longtext,
  `id_cuisinier` int NOT NULL,
  `id_consommateur` int NOT NULL,
  `id_commande` int NOT NULL,
  PRIMARY KEY (`id_rating`),
  KEY `rating_cuisinier_FK` (`id_cuisinier`),
  KEY `rating_Consommateur_FK` (`id_consommateur`),
  KEY `rating_Commandes_FK` (`id_commande`),
  CONSTRAINT `rating_Commandes_FK` FOREIGN KEY (`id_commande`) REFERENCES `commandes` (`id_commande`),
  CONSTRAINT `rating_Consommateur_FK` FOREIGN KEY (`id_consommateur`) REFERENCES `consommateur` (`id_consommateur`),
  CONSTRAINT `rating_cuisinier_FK` FOREIGN KEY (`id_cuisinier`) REFERENCES `cuisinier` (`id_cuisinier`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rating`
--

LOCK TABLES `rating` WRITE;
/*!40000 ALTER TABLE `rating` DISABLE KEYS */;
INSERT INTO `rating` VALUES (1,5,'Lasagnes excellentes !',4,'Client poli.',1,1,1),(2,4,'Sushi frais.',5,'Commande rapide.',2,2,2),(3,5,'Boeuf Bourguignon incroyable !',5,'Merci !',3,3,3),(4,3,'Tacos bons mais pas assez épicés.',4,'Merci pour le retour.',2,4,4),(5,5,'Un Coq au Vin divin !',5,'Plaisir de servir.',6,5,5),(6,4,'Ramen savoureux.',4,'Merci.',5,2,6),(7,5,'Pizza délicieuse !',5,'Client fidèle.',3,3,7),(8,5,'Burgers excellents.',5,'Merci.',7,6,8),(9,2,'manque dingredient cest un peu simple non ?',4,'Merci',1,1,9);
/*!40000 ALTER TABLE `rating` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-04 17:27:04
