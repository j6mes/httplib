namespace JumpKick.HttpLib.Provider
{
    using System;
    using System.Text;

    public class TextAuthenticationProvider : AuthenticationProvider
    {
        private string text;


        public TextAuthenticationProvider(string text)
        {
            this.text = text;
        }

        public Header GetAuthHeader()
        {
            return new Header("Authorization", text);
        }

    }
}
