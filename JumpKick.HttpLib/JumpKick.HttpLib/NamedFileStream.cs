namespace JumpKick.HttpLib
{
    using System.IO;

    /// <summary>
    /// NamedFileStream is a simple data structre that holds a file name, and stream
    /// </summary>
    public sealed class NamedFileStream
    {
        public string Name { get; private set; }
        public string Filename { get; private set; }
        public string ContentType { get; private set; }
        public Stream Stream { get; private set; }

        /// <summary>
        /// Create a new NamedFileStream
        /// </summary>
        /// <param name="name">Form name for file</param>
        /// <param name="filename">Name of file</param>
        /// <param name="contentType">Content type of file</param>
        /// <param name="stream">File Stream</param>
        public NamedFileStream(string name, string filename, string contentType, Stream stream)
        {
            this.Name = name;
            this.Filename = filename;
            this.ContentType = contentType;
            this.Stream = stream;
        }
    }
}
