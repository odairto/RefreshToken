namespace RefreshToken.Api
{
    public static class RefreshTokenStore
    {
        public static Dictionary<string, string> Tokens = new();

        public static bool GetRefreshToken(string refreshToken, string userName)
        {
            // Exemplo: o store armazena os tokens por nome de usuário
            if (RefreshTokenStore.Tokens.TryGetValue(userName, out string storedToken))
            {
                return storedToken == refreshToken;
            }

            return false;
        }
    }

    
}
