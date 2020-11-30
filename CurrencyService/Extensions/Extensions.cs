using CsvHelper;
using CurrencyService.Enums;
using CurrencyService.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace CurrencyService.Extensions
{
    public static class Extensions
    {
        public static T ConvertTo<T>(this IConvertible obj)
        {
            Type t = typeof(T);

            Type u = Nullable.GetUnderlyingType(t);

            if (u != null)
                return (obj is string ? string.IsNullOrEmpty((string)obj) : obj == null) ? default : (T)Convert.ChangeType(obj, u, CultureInfo.InvariantCulture);

            return (T)Convert.ChangeType(obj, t, CultureInfo.InvariantCulture);
        }

        public static FileInfoModel GenerateExcelFile<T>(this IEnumerable<T> paramList)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            var memoryStream = new MemoryStream();
            using var package = new ExcelPackage(memoryStream);

            var ws = package.Workbook.Worksheets.Add("Sheet1");
            ws.Cells.LoadFromCollection(paramList, true);
            package.Save();

            return new FileInfoModel { FileStream = new MemoryStream(memoryStream.ToArray()), FileName = $"{Guid.NewGuid()}.{Enum.GetName(typeof(FileTypeEnum), FileTypeEnum.xlsx)}" };
        }

        public static FileInfoModel GenerateCsvFile<T>(this IEnumerable<T> paramList)
        {
            var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(paramList);
            streamWriter.Flush();

            return new FileInfoModel { FileStream = new MemoryStream(memoryStream.ToArray()), FileName = $"{Guid.NewGuid()}.{Enum.GetName(typeof(FileTypeEnum), FileTypeEnum.csv)}" };
        }

        public static FileInfoModel GenerateJsonFile<T>(this IEnumerable<T> paramList)
        {
            var json = JsonSerializer.Serialize(paramList, options: new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All), WriteIndented = true });

            return new FileInfoModel { FileStream = new MemoryStream(Encoding.UTF8.GetBytes(json)), FileName = $"{Guid.NewGuid()}.{Enum.GetName(typeof(FileTypeEnum), FileTypeEnum.json)}" };
        }

        public static ResultModel SaveFile(this FileInfoModel fileInfoModel, string path = "")
        {
            var resultModel = new ResultModel { IsSuccessfull = true };
            try
            {
                if (fileInfoModel == null || !CheckStream(fileInfoModel.FileStream) || string.IsNullOrEmpty(fileInfoModel.FileName))
                    throw new Exception("Incorrect file content");

                path = string.IsNullOrEmpty(path) ? $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\{fileInfoModel.FileName}" : $"{path}\\{fileInfoModel.FileName}";
                resultModel.Message = path;

                using FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);
                fileInfoModel.FileStream.CopyTo(file);

            }
            catch (Exception ex)
            {
                resultModel.IsSuccessfull = false;
                resultModel.Message = ex.Message;
            }

            return resultModel;
        }

        private static bool CheckStream(Stream stream)
        {
            return stream != null && stream.Length > 0;
        }

    }
}
