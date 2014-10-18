namespace JumpKick.HttpLib
{
    using JumpKick.HttpLib.Provider;

    public class Request
    {
        private string url;
        private Verb httpVerb;
        private HeaderProvider headers;
        private AuthenticationProvider auth;
        private BodyProvider body;



    }
}
