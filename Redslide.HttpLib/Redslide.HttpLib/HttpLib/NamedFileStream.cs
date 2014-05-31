using System.IO;

namespace Redslide.HttpLib
{
    /// <summary>
    /// NamedFileStream is a simple data structre that holds a file name, and stream
    /// </summary>
    public sealed class NamedFileStream
    {
        public string Name;
        public string Filename;
        public string ContentType;
        public Stream Stream;

        public NamedFileStream() { }

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
