namespace oksei_fsot_api.src.Utility
{
    public class FileUploader
    {
        public static async Task<string?> UploadImageAsync(string directoryPath, Stream stream)
        {
            string filename = Guid.NewGuid().ToString() + ".jpeg";
            string fullPathToFile = Path.Combine(directoryPath, filename);

            using var file = File.Create(fullPathToFile);
            if (file == null)
                return null;

            await stream.CopyToAsync(file);
            return filename;
        }

        public static async Task<byte[]?> GetStreamImageAsync(string directoryPath, string filename)
        {
            string fullPathToFile = Path.Combine(directoryPath, filename);
            if (!File.Exists(fullPathToFile))
                return null;

            using Stream fileStream = File.OpenRead(fullPathToFile);
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}