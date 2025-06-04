#  Système de gestion de projets étudiants

une plateforme de gestion de projets où les étudiants peuvent soumettre leurs livrables 
et les enseignants peuvent évaluer leur avancement. 

## Prérequis

Assurez-vous d'avoir installé le .NET SDK.

## Cloner le dépôt

```bash
git clone <URL_DU_DEPOT>
cd projet_apnet_dotnet
```

## Installation des dépendances (Backend)

Naviguez dans le dossier de l'API :

```bash
cd StudentProjectAPI
```

Installez les dépendances NuGet :

```bash
dotnet restore
```

## Exécution de l'API

Depuis le dossier `StudentProjectAPI` :

```bash
dotnet run
```

## Installation des dépendances (Frontend)

Naviguez dans le dossier du Frontend :

```bash
cd ../StudentProjectFront
```

Installez les dépendances NuGet :

```bash
dotnet restore
```

## Exécution du Frontend

Depuis le dossier `StudentProjectFront` :

```bash
dotnet run
``` 