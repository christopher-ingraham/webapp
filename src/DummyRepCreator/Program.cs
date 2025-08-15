using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace RepCreator
{
	class Program
	{
		static String[] titles = { "args[#]", "Name", "Value" };
		static IWorkbook workbook;
		static void Main(string[] args)
		{
			if (args.Length < 8)
			{
				PrintHelp();
				FatalExit();
			}

			// read command line
			string CreatorFolder = args[0];
			string ReportLanguage = args[1];
			string TemplateFolder = args[2];
			string OutputFolder = args[3];
			string OutputFileName = args[4];
			string ReportType = args[5];
			List<string> ReportParam = new List<string>();

			for (int paramIndex = 6; paramIndex < args.Length; paramIndex++)
			{
				ReportParam.Add(args[paramIndex]);
			}

			// Create the new dummy report
			workbook = new XSSFWorkbook();
			Dictionary<String, ICellStyle> styles = CreateStyles(workbook);
			ISheet sheet = workbook.CreateSheet("args");

			int rowCount = 0;

			//title row
			IRow titleRow = sheet.CreateRow(rowCount++);
			titleRow.HeightInPoints = (45);
			ICell titleCell = titleRow.CreateCell(0);
			titleCell.SetCellValue(String.Format("Execution {0}", DateTime.Now));
			titleCell.CellStyle = (styles["title"]);
			sheet.AddMergedRegion(CellRangeAddress.ValueOf("$A$1:$C$1"));

			//header row
			IRow headerRow = sheet.CreateRow(rowCount++);
			headerRow.HeightInPoints = (40);
			ICell headerCell;
			for (int i = 0; i < titles.Length; i++)
			{
				headerCell = headerRow.CreateCell(i);
				headerCell.SetCellValue(titles[i]);
				headerCell.CellStyle = (styles["header"]);
			}

			AddRow(sheet, rowCount++, 0, "CreatorFolder", CreatorFolder);
			AddRow(sheet, rowCount++, 1, "ReportLanguage", ReportLanguage);
			AddRow(sheet, rowCount++, 2, "TemplateFolder", TemplateFolder);
			AddRow(sheet, rowCount++, 3, "OutputFolder", OutputFolder);
			AddRow(sheet, rowCount++, 4, "OutputFileName", OutputFileName);
			AddRow(sheet, rowCount++, 5, "ReportType", ReportType);

			int paramCount = 1;
			int argsIndex = 6;
			foreach (string rp in ReportParam) {
				string paramName = string.Format("ReportParam{0}", paramCount++);
				AddRow(sheet, rowCount++, argsIndex++, paramName, rp);
			}

			sheet.AutoSizeColumn(1);
			sheet.AutoSizeColumn(2);

			// Try writing the report to *.xlsx
			try
			{
				Directory.SetCurrentDirectory(CreatorFolder);
				WriteToFile(OutputFolder, OutputFileName);
				SuccessExit();
			} 
			catch (Exception e) {
				Console.Error.WriteLine(e.Message);
				FatalExit();
			}
			
		}
		private static void AddRow(ISheet sheet, int rowNumber, int argsIndex, string name, string value) {
			ICellStyle style = sheet.Workbook.CreateCellStyle();
			style.Alignment = HorizontalAlignment.Center;
			style.VerticalAlignment = VerticalAlignment.Center;

			IRow row = sheet.CreateRow(rowNumber);

			ICell cell0 = row.CreateCell(0);
			cell0.SetCellValue(argsIndex.ToString());
			cell0.CellStyle = style;

			ICell cell1 = row.CreateCell(1);
			cell1.SetCellValue(name);

			ICell cell2 = row.CreateCell(2);
			cell2.SetCellValue(value);
		}
		/**
		 * Create a library of cell styles
		 */
		private static Dictionary<String, ICellStyle> CreateStyles(IWorkbook wb)
		{
			Dictionary<String, ICellStyle> styles = new Dictionary<String, ICellStyle>();
			ICellStyle style;
			IFont titleFont = wb.CreateFont();
			titleFont.FontHeightInPoints = 18;
			titleFont.IsBold = true;
			style = wb.CreateCellStyle();
			style.Alignment = HorizontalAlignment.Center;
			style.VerticalAlignment = VerticalAlignment.Center;
			style.SetFont(titleFont);
			styles.Add("title", style);

			IFont monthFont = wb.CreateFont();
			monthFont.FontHeightInPoints = 11;
			monthFont.Color = (IndexedColors.White.Index);
			style = wb.CreateCellStyle();
			style.Alignment = HorizontalAlignment.Center;
			style.VerticalAlignment = VerticalAlignment.Center;
			style.FillForegroundColor = (IndexedColors.Grey50Percent.Index);
			style.FillPattern = FillPattern.SolidForeground;
			style.SetFont(monthFont);
			style.WrapText = (true);
			styles.Add("header", style);

			style = wb.CreateCellStyle();
			style.Alignment = HorizontalAlignment.Center;
			style.WrapText = (true);
			style.BorderRight = BorderStyle.Thin;
			style.RightBorderColor = (IndexedColors.Black.Index);
			style.BorderLeft = BorderStyle.Thin;
			style.LeftBorderColor = (IndexedColors.Black.Index);
			style.BorderTop = BorderStyle.Thin;
			style.TopBorderColor = (IndexedColors.Black.Index);
			style.BorderBottom = BorderStyle.Thin;
			style.BottomBorderColor = (IndexedColors.Black.Index);
			styles.Add("cell", style);

			style = wb.CreateCellStyle();
			style.Alignment = HorizontalAlignment.Center;
			style.VerticalAlignment = VerticalAlignment.Center;
			style.FillForegroundColor = (IndexedColors.Grey25Percent.Index);
			style.FillPattern = FillPattern.SolidForeground;
			style.DataFormat = wb.CreateDataFormat().GetFormat("0.00");
			styles.Add("formula", style);

			style = wb.CreateCellStyle();
			style.Alignment = HorizontalAlignment.Center;
			style.VerticalAlignment = VerticalAlignment.Center;
			style.FillForegroundColor = (IndexedColors.Grey40Percent.Index);
			style.FillPattern = FillPattern.SolidForeground;
			style.DataFormat = wb.CreateDataFormat().GetFormat("0.00");
			styles.Add("formula_2", style);

			return styles;
		}
		static void WriteToFile(string outputDirectory, string fileName) {
			string filename = Path.Combine(outputDirectory, fileName);
			using (FileStream file = new FileStream(filename, FileMode.Create)) {
				workbook.Write(file);
				file.Close();
			}
		}
		static void FatalExit() {
			Environment.Exit(571); // FATAL
		}
		static void SuccessExit() {
			Environment.Exit(0);
		}
		static void PrintHelp() {
			StringBuilder message = new StringBuilder();
			message.AppendLine("Usage:");
			message.AppendLine("RepCreator.exe {CreatorFolder} {ReportLanguage} {TemplateFolder} {OutputFolder} {OutputFileName} {ReportType} {ReportParam1} [{ReportParam2} [...]]");
			message.AppendLine();
			message.AppendLine("\tCreatorFolder   directory");
			message.AppendLine("\tReportLanguage  English | Italian");
			message.AppendLine("\tTemplateFolder  directory");
			message.AppendLine("\tOutputFolder    directory");
			message.AppendLine("\tOutputFileName  fileName (in OutputFolder)");
			message.AppendLine("\tReportType      0 | 1 | 2 | 3 | 4 | 5");
			message.AppendLine("\tReportParam1    any");
			message.AppendLine("\tReportParam2    any");
			message.AppendLine("\t...");
			message.AppendLine("\tReportParamN    any");
			Console.Error.WriteLine(message.ToString());
		}
	}
}
