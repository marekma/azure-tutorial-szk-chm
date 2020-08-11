using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace Cybercom.FunApps
{
    public static class OnCsvUploaded
    {
        [FunctionName("OnCsvUploaded")]
        public static void Run(
            [BlobTrigger("csvs/{name}", Connection = "json2excel0formatter_STORAGE")] Stream inBlob,
            [Blob("excels/output.xlsx", FileAccess.Write)] Stream outBlob,
            string name,
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {inBlob.Length} Bytes");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (StreamReader sr = new StreamReader(inBlob))
            {
                var csv = sr.ReadToEnd();
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
                    worksheet.Cells["A1"].LoadFromText(csv);

                    excelPackage.SaveAs(outBlob);
                }
            }
        }
    }
}
