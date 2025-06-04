# AlumniConnect.API

Backend ASP.NET Core pour le portail des anciens étudiants (Alumni).

## Fonctionnalités principales

- Authentification avec confirmation d’email (OTP 6 chiffres)
- Connexion avec JWT
- Réinitialisation du mot de passe par OTP
- Gestion des profils alumni
- Publication d’événements, offres d’emploi, témoignages, messagerie (à compléter)

---

## Démarrage rapide

1. **Cloner le projet**
2. **Configurer la base de données**
   - Par défaut, SQLite (`alumni.db`)
3. **Configurer le SMTP dans `appsettings.json`**
   ```json
   "Smtp": {
     "Host": "smtp.example.com",
     "Port": 587,
     "EnableSsl": true,
     "User": "ton_email@example.com",
     "Pass": "mot_de_passe"
   }

4 **Lancer les migrations**

      dotnet ef database update

5. **Démarrer l’API**


        dotnet run --project AlumniConnect.API

6 **Accéder à Swagger**

   - [http://localhost:5000/swagger

## Endpoints d’authentification

1.inscription

- **POST** `/api/auth/register`

- **Body** :
  ```json
  {
    "email": "test@example.com",
    "password": "Test123!",
    "fullName": "Jean Dupont",
    "promotion": "2020",
    "profession": "Développeur"
  }

2. Confirmation d’email (OTP)

- **POST** `/api/auth/confirm-email`
- **Body** :
  ```json
  {
    "email": "test@example.com",
    "token": "123456"
  }
  ```
- **Réponse** : "Email confirmé !"

3. Renvoyer l’OTP

- **POST** `/api/auth/resend-otp`
- **Body** :
  ```json
  { "email": "test@example.com" }
  ```
- **Réponse** : "Un nouveau code OTP a été envoyé à votre email."

4. Connexion

- **POST** `/api/auth/login`
- **Body** :
  ```json
  {
    "email": "test@example.com",
    "password": "Test123!"
  }
  ```
- **Réponse** :
  ```json
  {
    "token": "jwt...",
    "user": {
      "id": "...",
      "email": "test@example.com",
      "fullName": "Jean Dupont",
      "promotion": "2020",
      "profession": "Développeur",
      "bio": "",
      "photoUrl": "",
      "phoneNumber": ""
    }
  }

5. Demander un OTP de réinitialisation de mot de passe

- **POST** `/api/auth/request-reset-password-otp`
- **Body** :
  ```json
  { "email": "test@example.com" }
  ```
- **Réponse** : "Un code OTP de réinitialisation a été envoyé à votre email."

---

### 6. Réinitialiser le mot de passe

- **POST** `/api/auth/reset-password`
- **Body** :
  ```json
  {
    "email": "test@example.com",
    "otp": "123456",
    "newPassword": "NouveauMotDePasse123!"
  }
  ```
- **Réponse** :
  - Succès : "Mot de passe réinitialisé avec succès."
  - Erreur : `{ "errors": [ "Passwords must have at least one uppercase ('A'-'Z').", ... ] }

Tests unitaires

- Les tests sont dans le projet `AlumniConnect.API.Tests`
- Lancer tous les tests :
  ```bash
  dotnet test AlumniConnect.API.Tests
  ```

---

## Swagger

- Documentation interactive disponible sur `/swagger` après lancement de l’API.
