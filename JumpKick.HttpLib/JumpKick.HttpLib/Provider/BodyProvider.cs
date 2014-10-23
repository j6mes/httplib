namespace JumpKick.HttpLib.Provider
{
    using System.IO;

    public interface BodyProvider
    {
        string GetContentType();

        Stream GetBody();
    }
}
