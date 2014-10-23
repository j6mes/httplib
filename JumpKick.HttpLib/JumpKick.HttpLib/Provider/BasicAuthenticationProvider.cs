namespace JumpKick.HttpLib.Provider
{
    using System;
    using System.Text;

    public class BasicAuthenticationProvider : AuthenticationProvider
    {
        private string username;
        private string password;

        public Header GetAuthHeader()
        {
            return new Header("Authorization", GenerateAuthString(username,password));
        }

        public static string GenerateAuthString(string username, string password) 
        {
            string authstring = string.Format("{0}:{1}", username, password);
            return string.Format("Basic {0}", Convert.ToBase64String(Encoding.ASCII.GetBytes(authstring)));
        }
    }
}
