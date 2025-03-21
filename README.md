# Logiciel de Gestion des Étudiants (LGE)

## Aperçu

Le **Logiciel de Gestion des Étudiants (LGE)** est une application de bureau conçue pour simplifier la gestion des étudiants, des enseignants et des tâches administratives dans les établissements éducatifs. Développé dans le cadre d’un projet pour Webitech (Paris), ce logiciel répond aux limites des outils traditionnels tels que les registres papier et les fichiers Excel en offrant un système centralisé, sécurisé et convivial. Il automatise des processus comme la gestion des notes, l’inscription des étudiants et la consultation des emplois du temps tout en garantissant la sécurité des données et la conformité aux réglementations comme le RGPD.

Ce projet a été réalisé par **Samia Boutezrout** dans le cadre d’une initiative académique visant à améliorer la gestion des affaires étudiantes et de l’éducation.

---

## Fonctionnalités

### Objectifs Généraux
- Centraliser les informations sur les étudiants, les enseignants et les cours.
- Automatiser les processus administratifs.
- Assurer la sécurité et la confidentialité des données grâce à un contrôle d’accès et des sauvegardes régulières.

### Fonctionnalités Spécifiques par Utilisateur
#### Administrateurs
- Gérer les inscriptions des étudiants et des enseignants.
- Attribuer des cours aux enseignants.
- Modifier des documents académiques (listes d’étudiants, listes d’enseignants, relevés de notes, etc.).

#### Enseignants
- Gérer et saisir les notes des étudiants.
- Consulter les listes de classes et les détails des étudiants.
- Accéder aux emplois du temps.

#### Étudiants
- Consulter leurs notes et emplois du temps.
- Accéder aux informations relatives aux cours.

---

## Technologies Utilisées
- **IDE** : Visual Studio 2022
- **Langage de Programmation** : C#
- **Base de Données** : MariaDB / MySQL
- **Outils de Documentation** : Microsoft Word, Excel

---

## Installation

### Prérequis
- **Visual Studio 2022** installé sur votre machine.
- Serveur **MariaDB** ou **MySQL** installé et opérationnel.
- Connaissances de base en C# et en configuration de bases de données.

### Étapes
1. **Cloner le Dépôt**
   ```bash
   git clone https://github.com/<votre-nom-utilisateur>/logiciel-gestion-etudiants.git

   # Guide d'Installation et d'Utilisation

## Ouvrir le Projet
1. Lancez **Visual Studio 2022**.
2. Ouvrez le fichier `.sln` situé dans le répertoire du projet.

## Configurer la Base de Données
1. Créez une nouvelle base de données dans **MariaDB/MySQL** (par exemple, `gestion_etudiants_db`).
2. Mettez à jour la chaîne de connexion dans le fichier de configuration du projet (par exemple, `appsettings.json`) avec vos identifiants de base de données :

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=gestion_etudiants_db;User=root;Password=votre_mot_de_passe;"
}
```

3. Exécutez les scripts SQL fournis (si inclus) pour configurer les tables et les données initiales.

## Compiler et Lancer
1. Compilez la solution dans **Visual Studio** (`Ctrl+Shift+B`).
2. Exécutez l’application (`F5`).

## Utilisation
### Cas 1 : L’Administrateur Crée un Compte Étudiant
1. Connectez-vous en tant qu’administrateur.
2. Accédez à la section **"Gestion des Étudiants"**.
3. Cliquez sur **"Ajouter un Étudiant"**.
4. Remplissez les champs requis (ex : Nom, Prénom, ID Étudiant, Classe).
5. Validez pour créer le compte.

### Cas 2 : L’Enseignant Gère les Notes
1. Connectez-vous en tant qu’enseignant.
2. Rendez-vous dans la section **"Gestion des Classes"**.
3. Sélectionnez une classe (ex : 1ère Année).
4. Choisissez un étudiant et saisissez sa note pour une évaluation ou un examen.
5. Enregistrez les modifications.

## Captures d’Écran
Voici quelques écrans clés de l’application :
- **Écran de Connexion**
- **Tableau de Bord Administrateur**
- **Vue des Classes Enseignant**
- **Vue des Notes Étudiant**

*(Remarque : Remplacez les chemins placeholders ci-dessus par les noms réels des fichiers de captures d’écran une fois disponibles.)*

## Structure du Projet
```
/src        -> Contient le code source en C#
/database   -> Scripts SQL pour la configuration de la base de données
/docs       -> Documentation du projet (ex : fichiers Word/Excel)
/screenshots -> Dossier pour les captures d’écran de l’application
```

## Sécurité & Conformité
- **Contrôle d’Accès** : Authentification basée sur les rôles pour les administrateurs, enseignants et étudiants.
- **Sauvegarde des Données** : Sauvegardes automatisées régulières pour éviter toute perte de données.
- **Conformité RGPD** : Garantit la protection des données personnelles conformément aux réglementations européennes.

## Améliorations Futures
- Ajouter la prise en charge de plusieurs langues.
- Développer une version web pour une accessibilité accrue.
- Intégrer des notifications par email pour les mises à jour des notes et des emplois du temps.

## Contact
Pour toute question ou assistance, contactez :

**Samia Boutezrout**
- **Email** : samaholiccs@gmail.com
- **Établissement** : Webitech, 6-8 Rue Firmin Gillot, 75015 Paris

**Dernière mise à jour : 21 mars 2025**

