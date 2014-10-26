namespace JumpKick.HttpLib.Provider
{
    using System;
    using System.Text;

    public class BasicAuthenticationProvider : AuthenticationProvider
    {
        private string username;
        private string password;

        public BasicAuthenticationProvider(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public Header GetAuthHeader()
        {
            return new Header("Authorization",  string.Format("Basic {0}",GenerateAuthString(username,password)));
        }

        public static string GenerateAuthString(string username, string password) 
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password)));
        }
    }
}
