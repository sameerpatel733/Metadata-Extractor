using System;
using System.Collections.Generic;
using System.Text;
using iText.StyledXmlParser.Jsoup.Nodes;
using TagLib.Flac;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace MetadataExtractor.Models
{
    public class FileMetadata
    {
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public string Extension { get; set; }
        public long SizeKB { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime LastAccessDate { get; set; }
        public string Resolution { get; set; }
        public string Duration { get; set; }
        public string Author { get; set; }

        public string Title { get; set; }
        public string Subject { get; set; }
        public string Keywords { get; set; }
        public string Creator { get; set; }
        public string Producer { get; set; }
        public string PageCount { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Year { get; set; }
        public string SampleRate { get; set; }
        public string Channel { get; set; }
        public string ProductName { get; set; }
        public string FileDescription { get; set; }
        public string CompanyName { get; set; }
        public string FileVersion { get; set; }
        public string ProductVersion { get; set; }
        public string OriginalFileName { get; set; }
        public string CopyRight { get; set; }
        public string RowCount { get; set; }
        public string ColumnCount { get; set; }
        public string Headers { get; set; }
        public string Delimiter { get; set; }
        public string Encoding { get; set; }
        public string SheetCount { get; set; }
        public string SheetNames { get; set; }
        public string SlideCount { get; set; }
        public string WordCount { get; set; }
        public string CharcterCount { get; set; }
    }
}
