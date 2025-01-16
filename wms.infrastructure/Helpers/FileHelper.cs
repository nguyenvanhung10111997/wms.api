using wms.infrastructure.Models;

namespace wms.infrastructure.Helpers
{
    public static class FileHelper
    {
        public static async Task<FileUploadRes> Upload(byte[] bytes, string fileName)
        {
            var multiContent = new MultipartFormDataContent();
            var fileExtension = Path.GetExtension(fileName);

            var currentDate = DateTime.Now.ToString("yyyyMMdd");
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFile", currentDate);
            var filePath = Path.Combine(folderPath, $"{Guid.NewGuid()}{fileExtension}");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using (var fileStream = File.Create(filePath))
            {
                await fileStream.WriteAsync(bytes, 0, bytes.Length);
                await fileStream.FlushAsync();
            }

            return new FileUploadRes { FilePath = filePath };
        }

        public static async Task<byte[]> GetFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("File không tồn tại. Vui lòng kiểm tra lại");
            }

            var bytes = await File.ReadAllBytesAsync(filePath);

            return bytes;
        }
    }
}
