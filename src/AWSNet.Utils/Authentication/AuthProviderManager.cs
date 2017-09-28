namespace AWSNet.Utils.Authentication
{
    public class AuthProviderManager
    {
        public static AuthProvider Microsoft
        {
            get
            {
                return new AuthProvider(AuthProviderType.Microsoft);
            }
        }

        public static AuthProvider Twitter
        {
            get
            {
                return new AuthProvider(AuthProviderType.Twitter);
            }
        }

        public static AuthProvider Facebook
        {
            get
            {
                return new AuthProvider(AuthProviderType.Facebook);
            }
        }

        public static AuthProvider Google
        {
            get
            {
                return new AuthProvider(AuthProviderType.Google);
            }
        }
    }
}
