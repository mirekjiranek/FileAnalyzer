using System;

namespace FileAnalyzer
{
    [Serializable]
    public class FileDetails
    {
        public string Path { get; set; }
        public string Hash { get; set; }
        public int Version { get; set; }
    }
}