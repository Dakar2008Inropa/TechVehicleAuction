using AuctionData.Models.Database;
using Avalonia.Media.Imaging;
using Microsoft.Data.SqlClient;
using System;
using System.IO;

namespace TechAuction.Utilities
{
    public static class Helper
    {
        public static class Vehicle
        {
            public static Bitmap? DownloadImageFromDB(int imageId)
            {
                Database.Instance.OpenConnection();
                var cmd = new SqlCommand($"SELECT Image FROM {DatabaseTables.VehicleImages} WHERE Id = @Id", Database.Instance.GetConnection());
                cmd.Parameters.AddWithValue("@Id", imageId);

                byte[] imageData = (byte[])cmd.ExecuteScalar();

                Database.Instance.CloseConnection();

                if (imageData == null)
                    return GetPlaceholderBitmap();

                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    return new Bitmap(ms);
                }
            }

            public static void UploadImageToDatabase(string imagePath, string desc)
            {
                Database.Instance.OpenConnection();

                byte[] image = File.ReadAllBytes(imagePath);

                int imageWidth = 0;
                int imageHeight = 0;

                using(MemoryStream ms = new MemoryStream(image))
                {
                    Bitmap bitmapImage = new Bitmap(ms);
                    imageWidth = bitmapImage.PixelSize.Width;
                    imageHeight = bitmapImage.PixelSize.Height;
                }

                string query = $"INSERT INTO {DatabaseTables.VehicleImages} (Image, Description, ImageWidth, ImageHeight) VALUES (@Image, @Description, @ImageWidth, @ImageHeight)";
                using (SqlCommand cmd = new SqlCommand(query, Database.Instance.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Image", image);
                    cmd.Parameters.AddWithValue("@Description", desc);
                    cmd.Parameters.AddWithValue("@ImageWidth", imageWidth);
                    cmd.Parameters.AddWithValue("@ImageHeight", imageHeight);
                    cmd.ExecuteNonQuery();
                }

                Database.Instance.CloseConnection();
            }

            private static Bitmap GetPlaceholderBitmap()
            {
                var uri = new Uri("avares://TechAuction/Assets/vehiclePlaceholder.png");
                return new Bitmap(Avalonia.Platform.AssetLoader.Open(uri));
            }
        }
    }
}