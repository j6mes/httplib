namespace JumpKick.HttpLib.Provider
{
    using System.IO;

    interface BodyProvider
    {
        public string getContentType();

        public MemoryStream getBody();
    }
}
