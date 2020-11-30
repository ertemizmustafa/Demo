using System.IO;

namespace CurrencyService.Model
{
    public class FileInfoModel
    {
        public Stream FileStream { get; set; }
        public string FileName { get; set; }
    }
}
