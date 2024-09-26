using Avalonia.Media.Imaging;
using System;
using System.IO;

namespace TechAuction.Utilities
{
    public static class Helper
    {
        public static class Vehicle
        {
            public static Bitmap Base64ToBitmap(string base64String)
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using (var stream = new MemoryStream(imageBytes))
                {
                    return new Bitmap(stream);
                }
            }
        }
    }
}