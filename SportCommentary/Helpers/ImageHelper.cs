using Microsoft.AspNetCore.Components.Forms;

namespace SportCommentary.Helpers
{
    public static class ImageHelper
    {
        public static async Task<string> InputFileToBase64(InputFileChangeEventArgs args)
        {
            string base64String = "";

                var files = args.GetMultipleFiles();
                foreach (var file in files)
                {
                    await using MemoryStream fs = new MemoryStream();
                    await file.OpenReadStream().CopyToAsync(fs);
                    byte[] somBytes = GetBytes(fs);
                    base64String = Convert.ToBase64String(somBytes, 0, somBytes.Length);
                    base64String = "data:" + file.ContentType + ";base64," + base64String;
                }

            return base64String;
        }

        private static byte[] GetBytes(Stream stream)
        {
            var bytes = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.ReadAsync(bytes, 0, bytes.Length);
            stream.Dispose();
            return bytes;
        }
    }
}
