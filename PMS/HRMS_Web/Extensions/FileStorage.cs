namespace HRMS_Web.Extensions
{
    public static class FileStorage
    {
        public static async Task<string> SaveBase64FileAsync(
            this string base64Content,
            string folderPath = null,
            string virtualPathPrefix = null)
        {
            if (base64Content == "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcThtjU0d_BQklzBkT7Hn7t48a5yaBVWIJa4i6PcFbFgt91JYcN-FPV0laysIBBD-VC-p-s&usqp=CAU")
                return base64Content;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            folderPath ??= configuration.GetValue<string>("Attachments:AttachmentsFolderPath");
            virtualPathPrefix ??= configuration.GetValue<string>("Attachments:VirtualPathPrefix");

            if (string.IsNullOrEmpty(base64Content))
                throw new ArgumentException("Base64 content cannot be null or empty.");

            var base64Parts = base64Content.Split(",");
            if (base64Parts.Length != 2)
                return base64Content;

            var header = base64Parts[0];
            var fileData = base64Parts[1];

            var fileExtension = ExtractFileExtension(header);
            if (string.IsNullOrEmpty(fileExtension))
                throw new InvalidOperationException("Could not determine file extension.");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            var fullPath = Path.Combine(folderPath, uniqueFileName);

            var fileBytes = Convert.FromBase64String(fileData);
            await File.WriteAllBytesAsync(fullPath, fileBytes);

            return Path.Combine(virtualPathPrefix, uniqueFileName).Replace("\\", "/");
        }

        private static string ExtractFileExtension(string base64Header)
        {
            if (base64Header.Contains("data:image/png"))
                return ".png";
            if (base64Header.Contains("data:image/jpeg"))
                return ".jpg";
            if (base64Header.Contains("data:image/gif"))
                return ".gif";
            if (base64Header.Contains("data:application/pdf"))
                return ".pdf";
            if (base64Header.Contains("data:video/mp4"))
                return ".mp4";
            if (base64Header.Contains("data:video/webm"))
                return ".webm";
            if (base64Header.Contains("data:video/ogg"))
                return ".ogg";

            return null;
        }

        public static void DeleteFile(this string filePath)
        {
            if (filePath.Contains("encrypted-tbn0.gstatic.com"))
                return;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var folderPath = configuration.GetValue<string>("Attachments:AttachmentsFolderPath");

            var fileName = Path.GetFileName(filePath);

            var fullFilePath = Path.Combine(folderPath, fileName);

            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
            }
        }
    }
}
