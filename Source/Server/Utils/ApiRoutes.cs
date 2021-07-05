namespace MultiplayerSnake.Server
{
    public static class ApiRoutes
    {
        /// <summary>
        /// Register action in <see cref="AccountController"/>
        /// </summary>
        /// <remarks>
        /// Pass UserRegisterDto as the post parameters
        /// </remarks>
        public const string Register = "register";

        /// <summary>
        /// Login action in <see cref="AccountController"/>
        /// </summary>
        /// <remarks>
        /// Pass UserLoginDto as the post parameters
        /// </remarks>
        public const string Login = "login";
    }
}
