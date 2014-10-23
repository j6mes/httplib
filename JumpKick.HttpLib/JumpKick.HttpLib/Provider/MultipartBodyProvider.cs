
namespace JumpKick.HttpLib.Provider
{
    using System;
    using System.IO;

    public class MultipartBodyProvider : BodyProvider
    {
        public string GetContentType()
        {
            return "multipart/form-data";
        }

        public Stream GetBody()
        {
            throw new NotImplementedException();
        }
    }
}
