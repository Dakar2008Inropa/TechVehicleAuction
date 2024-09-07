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
                var cmd = new SqlCommand("SELECT Image FROM VehicleImages WHERE Id = @Id", Database.Instance.GetConnection());
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

            public static void UploadImageToDatabase(string imagePath)
            {
                Database.Instance.OpenConnection();

                byte[] imageData = File.ReadAllBytes(imagePath);

                string query = "INSERT INTO ImagesTable (ImageData) VALUES (@ImageData)";
                using (SqlCommand cmd = new SqlCommand(query, Database.Instance.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@ImageData", imageData);
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