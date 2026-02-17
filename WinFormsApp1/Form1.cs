using System.Diagnostics;
using System.IO;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using iText;
using iText.Kernel.Pdf;
using MetadataExtractor.Models;
using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel; 
using OfficeOpenXml;
using TagLib;
using TagLib.Audible;

namespace WinFormsApp1
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

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
			if (!System.IO.File.Exists(path))
			{
				MessageBox.Show(
				"The selected file does not exist.",
				"File Error",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error
				);
				return null;
			}

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

			switch (ext) 
			{
				case ".jpg":
				case ".png":
				case ".bmp":
					AddImageMetadata(path, model);
					break;

				case ".mp3":
				case ".weba":
					AddMp3Metadata(path, model);
					break;

				case ".pdf":
					AddPdfMeta(path, model);
					break;

				case ".dll":
				case ".exe":
					addDLLMetadata(path, model); 
					break;

				case ".csv":
					ReadCsv(path,model); 
					break;

				case ".xlsx":
					ReadXlsx(path,model);
					break;

				case ".xls":
					ReadXls(path, model);
					break;

				case ".ppt":
					Readpptx(path,model);
					break;

				case ".docx":
					ReadDocx(path,model);
					break;
				
				default:
					break;
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

		public void AddMp3Metadata(string inputpath, FileMetadata metadata)
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
			metadata.Title = info.GetTitle();
			metadata.Subject = info.GetSubject();
			metadata.Keywords = info.GetKeywords();
			metadata.Creator = info.GetCreator();
			metadata.Producer = info.GetProducer();
			metadata.PageCount = pdfDocument.GetNumberOfPages().ToString();
		}
		public void ReadCsv(string path, FileMetadata metadata)
		{
			var lines = System.IO.File.ReadAllLines(path);

			if (lines.Length == 0) return;

			var header = lines[0].Split(',');

			metadata.RowCount = lines.Length.ToString();
			metadata.ColumnCount = header.Length.ToString();
			metadata.Headers = string.Join(", ", header);
			metadata.Delimiter = ",";
		}

		private void ReadXlsx(string path, FileMetadata metadata)
		{
			using var package = new ExcelPackage(new FileInfo(path));

			metadata.SheetCount = package.Workbook.Worksheets.Count.ToString();
			metadata.SheetNames = string.Join(", ",
								package.Workbook.Worksheets.Select(s => s.Name));

			var sheet = package.Workbook.Worksheets.FirstOrDefault();
			int columncount = 0;
			if (sheet?.Dimension != null)
			{
				metadata.RowCount = sheet.Dimension.Rows.ToString();
				metadata.ColumnCount = sheet.Dimension.Columns.ToString();
				columncount = Convert.ToInt32(metadata.ColumnCount);
				var headers = new List<string>();

				for (int col = 1; col <= columncount; col++)
				{
					headers.Add(sheet.Cells[1, col].Text);
				}

				metadata.Headers = string.Join(", ", headers);
			}
		}

		private void ReadXls(string path, FileMetadata metadata)
		{
			using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

			var workbook = new HSSFWorkbook(fs); // .xls

			metadata.SheetCount = workbook.NumberOfSheets.ToString();

			var sheetNames = Enumerable.Range(0, workbook.NumberOfSheets)
									   .Select(i => workbook.GetSheetName(i));

			metadata.SheetNames = string.Join(", ", sheetNames);
			
			var sheet = workbook.GetSheetAt(0);

			if (sheet != null)
			{
				metadata.RowCount = sheet.LastRowNum.ToString();

				var headerRow = sheet.GetRow(0);

				if (headerRow != null)
				{
					metadata.ColumnCount = headerRow.LastCellNum.ToString();

					var headers = new List<string>();

					for (int i = 0; i < headerRow.LastCellNum; i++)
					{
						headers.Add(headerRow.GetCell(i)?.ToString());
					}

					metadata.Headers = string.Join(", ", headers);

				}
			}
		}

		public void Readpptx(string path, FileMetadata metadata)
		{
			using var prestentation = PresentationDocument.Open(path, false);

			var presentationPart = prestentation.PresentationPart;

			if (presentationPart != null)
			{
				metadata.SlideCount = presentationPart.SlideParts.Count().ToString();
			}
		}

		public void ReadDocx(string path, FileMetadata metadata)
		{
			using var document = WordprocessingDocument.Open(path, false);

			var body = document.MainDocumentPart?.Document?.Body;

			if (body != null)
			{
				metadata.PageCount = body.Descendants<Paragraph>().Count().ToString();
				metadata.WordCount = body.InnerText.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length.ToString();
				metadata.CharcterCount = body.InnerText.Length.ToString();
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
			if (!string.IsNullOrWhiteSpace(tbxFilepath.Text))
			{
				var row = tbxFilepath.Text;
				var path = row.Trim('"');

				var metadata = GetMetaData(path);
				dgvDataList.DataSource = metadata;
				HideEmptyColumns(dgvDataList);
			}
			else
			{
				MessageBox.Show(
				"Please enter valid path !!",
				"Input Required",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error
				);
			}
		}

		private void HideEmptyColumns(DataGridView dgv)
		{
			foreach (DataGridViewColumn column in dgv.Columns)
			{
				bool hasData = false;
				foreach (DataGridViewRow row in dgv.Rows)
				{
					if (!row.IsNewRow && row.Cells[column.Index].Value != null && !string.IsNullOrWhiteSpace(row.Cells[column.Index].Value.ToString()))
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