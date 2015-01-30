namespace JumpKick.HttpLib.Provider
{
    using System.IO;

    public interface BodyProvider
    {
        string GetContentType();

        Stream GetBody();

        void OnProgressChange(long bytesSent, long? totalBytes);

        void OnCompleted(long totalBytes);
    }
}
