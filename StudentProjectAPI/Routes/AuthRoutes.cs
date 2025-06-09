namespace StudentProjectAPI.Routes
{
    /// <summary>
    /// Documentation des routes d'authentification
    /// </summary>
    public static class AuthRoutes
    {
        /// <summary>
        /// Routes d'authentification
        /// </summary>
        public static class Auth
        {
            /// <summary>
            /// Inscription d'un nouvel utilisateur
            /// </summary>
            /// <remarks>
            /// POST /api/auth/register
            /// 
            /// Body:
            /// {
            ///     "email": "string",
            ///     "password": "string",
            ///     "firstName": "string",
            ///     "lastName": "string",
            ///     "role": "Student|Teacher"
            /// }
            /// </remarks>
            public const string Register = "register";

            /// <summary>
            /// Connexion d'un utilisateur
            /// </summary>
            /// <remarks>
            /// POST /api/auth/login
            /// 
            /// Body:
            /// {
            ///     "email": "string",
            ///     "password": "string"
            /// }
            /// </remarks>
            public const string Login = "login";

            /// <summary>
            /// Changement de mot de passe
            /// </summary>
            /// <remarks>
            /// POST /api/auth/change-password
            /// 
            /// Headers:
            /// Authorization: Bearer {token}
            /// 
            /// Body:
            /// {
            ///     "currentPassword": "string",
            ///     "newPassword": "string"
            /// }
            /// </remarks>
            public const string ChangePassword = "change-password";

            /// <summary>
            /// Suppression d'un utilisateur
            /// </summary>
            /// <remarks>
            /// DELETE /api/auth/delete-user/{id}
            /// 
            /// Headers:
            /// Authorization: Bearer {token}
            /// </remarks>
            public const string DeleteUser = "delete-user";
        }
    }
}
