namespace ELearningApp.Scripts
{
    public static class Ulitites
    {
        public static async Task<string> UploadFileAsync(IFormFile? file, string dirctory, string fileName)
        {
            var uploadDirctory = $"{Directory.GetCurrentDirectory()}\\wwwroot\\{dirctory}";

            if (!Directory.Exists(uploadDirctory))
                Directory.CreateDirectory(uploadDirctory);

            fileName = $"{fileName}.{file?.Name.Split('.').Last()}";

            var filePath = Path.Combine(uploadDirctory, fileName);

            using (FileStream stream = new(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }
    }
}
