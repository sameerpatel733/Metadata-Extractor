using System.Diagnostics;
using iText;
using iText.Kernel.Pdf;
using MetadataExtractor.Models;
using OfficeOpenXml;
using TagLib;
using TagLib.Audible;
using NPOI;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var data = GetMetaData(ofd.FileName);
                dgvDataList.DataSource = data;
                HideEmptyColumns(dgvDataList);
            }
        }

        public List<FileMetadata> GetMetaData(string path)
        {
            List<FileMetadata> list = new List<FileMetadata>();
            FileInfo fileInfo = new FileInfo(path);

            FileMetadata model = new FileMetadata
            {
                FileName = fileInfo.Name,
                FullPath = fileInfo.FullName,
                Extension = fileInfo.Extension,
                SizeKB = (fileInfo.Length) / 1024,
                Created = fileInfo.CreationTime,
                Modified = fileInfo.LastWriteTime,
                LastAccessDate = fileInfo.LastAccessTime
            };
            string ext = fileInfo.Extension;

            if (ext == ".jpg" || ext == ".png" || ext == ".bmp")
            {
                AddImageMetadata(path, model);
            }
            else if (ext == ".mp3" || ext ==".weba")
            {
                AddMp3Metadata(path, model);
            }
            else if (ext == ".pdf")
            {
                AddPdfMeta(path, model);
            }else if(ext == ".dll" || ext == ".exe") 
            {
                addDLLMetadata(path, model);
            }
            else if(ext == ".csv" || ext == ".xlxs" || ext == ".xls") 
            {
                addExtractCSVandXLXSMetadata(path, model);
            }

            list.Add(model);
            return list;
        }

        public void AddImageMetadata(string inputpath, FileMetadata metadata)
        {
            var path = inputpath.Trim('"');
            Image image = Image.FromFile(path);
            metadata.Resolution = $"{image.Width} * {image.Height}";
        }

        public void AddMp3Metadata(string inputpath , FileMetadata metadata)
        {
            var path = inputpath.Trim('"');
            var file = TagLib.File.Create(path);
            metadata.Duration = file.Properties.Duration.ToString(@"mm\:ss");
            metadata.Artist = string.Join(", ", file.Tag.Performers);
            metadata.Album = file.Tag?.Album ?? null;
            metadata.Year = file.Tag?.Year.ToString();
            metadata.SampleRate = file.Properties.AudioSampleRate.ToString();
            metadata.Channel = file.Properties.AudioChannels.ToString();
        }

        public void AddPdfMeta(string inputpath, FileMetadata metadata)
        {
            var path = inputpath.Trim('"');
            PdfReader pdfReader = new PdfReader(path);
            PdfDocument pdfDocument = new PdfDocument(pdfReader);
            var info = pdfDocument.GetDocumentInfo();
            metadata.Author = info.GetAuthor();
            metadata.Title= info.GetTitle();
            metadata.Subject= info.GetSubject();
            metadata.Keywords= info.GetKeywords();
            metadata.Creator= info.GetCreator();
            metadata.Producer= info.GetProducer();
            metadata.PageCount = pdfDocument.GetNumberOfPages().ToString();
        }

        public void addExtractCSVandXLXSMetadata(string inputpath, FileMetadata metadata)
        {
            var reader = new StreamReader(inputpath);
            var firstLine = reader.ReadLine();

            if (firstLine != null)
            {
                var header = firstLine.Split(',');
                metadata.ColumnCount = header.Length;
                metadata.Headers = string.Join(", ", header);
                metadata.HasHeader = true;
                metadata.Delimiter = ",";
            }

            int rowCount = 1;

            while (!reader.EndOfStream)
            {
                reader.ReadLine();
                rowCount++;
            }

            metadata.RowCount = rowCount;

            using var package = new ExcelPackage(new FileInfo(inputpath));

            metadata.SheetCount = package.Workbook.Worksheets.Count;

            var sheetNames = package.Workbook.Worksheets
                                .Select(s => s.Name);

            metadata.SheetNames = string.Join(", ", sheetNames);

            var firstSheet = package.Workbook.Worksheets.FirstOrDefault();

            if (firstSheet?.Dimension != null)
            {
                metadata.RowCount = firstSheet.Dimension.Rows;
                metadata.ColumnCount = firstSheet.Dimension.Columns;

                var headers = new List<string>();

                for (int col = 1; col <= metadata.ColumnCount; col++)
                {
                    headers.Add(firstSheet.Cells[1, col].Text);
                }

                metadata.Headers = string.Join(", ", headers);
                metadata.HasHeader = true;
            }
        }

        private void addDLLMetadata(string inputpath, FileMetadata metadata)
        {
            var path = inputpath.Trim('"');
            var versionInfo = FileVersionInfo.GetVersionInfo(path);

            metadata.ProductName = versionInfo.ProductName;
            metadata.FileDescription = versionInfo.FileDescription;
            metadata.CompanyName = versionInfo.CompanyName;
            metadata.FileVersion = versionInfo.FileVersion;
            metadata.ProductVersion = versionInfo.ProductVersion;
            metadata.OriginalFileName = versionInfo.OriginalFilename;
            metadata.CopyRight = versionInfo.LegalCopyright;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(tbxFilepath.Text != null) 
            {
                var row = tbxFilepath.Text;
                var path = row.Trim('"');

                var metadata = GetMetaData(path);
                dgvDataList.DataSource = metadata;
                HideEmptyColumns(dgvDataList);
            }
            else 
            {
                MessageBox.Show("Please Enter valid path ");
            }
        }

        private void HideEmptyColumns(DataGridView dgv) 
        {
            foreach(DataGridViewColumn column in dgv.Columns) 
            {
                bool hasData= false;
                foreach(DataGridViewRow row in dgv.Rows) 
                {
                    if(!row.IsNewRow && row.Cells[column.Index].Value != null &&
                        !string.IsNullOrWhiteSpace(row.Cells[column.Index].Value.ToString()))
                    {
                        hasData = true;
                        break;
                    }
                }
                column.Visible = hasData;
            }
        }
    }
} 