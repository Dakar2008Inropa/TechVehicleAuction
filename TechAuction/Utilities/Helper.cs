using AuctionData.Models.Database;
using Avalonia.Media.Imaging;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace TechAuction.Utilities
{
    public static class Helper
    {
        public static class Vehicle
        {
            public static List<Bitmap?> DownloadImagesFromDB(int vehicleId)
            {
                Database.Instance.OpenConnection();
                string query = $"SELECT Image FROM {DatabaseTables.VehicleImages} WHERE VehicleId = {vehicleId}";

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, Database.Instance.GetConnection()))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    List<Bitmap?> bitmaps = new List<Bitmap?>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var row = dt.Rows[i];
                        byte[] imageData = (byte[])row["Image"];
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            bitmaps.Add(new Bitmap(ms));
                        }
                    }
                    Database.Instance.CloseConnection();

                    if (bitmaps.Count == 0)
                    {
                        bitmaps.Add(GetPlaceholderBitmap());
                    }

                    return bitmaps;
                }
            }

            public static void UploadImageToDatabase(string imagePath, string desc, int vehicleId)
            {
                Database.Instance.OpenConnection();

                byte[] image = File.ReadAllBytes(imagePath);

                int imageWidth = 0;
                int imageHeight = 0;

                using (MemoryStream ms = new MemoryStream(image))
                {
                    Bitmap bitmapImage = new Bitmap(ms);
                    imageWidth = bitmapImage.PixelSize.Width;
                    imageHeight = bitmapImage.PixelSize.Height;
                }

                string query = $"INSERT INTO {DatabaseTables.VehicleImages} (Image, Description, ImageWidth, ImageHeight, VehicleId) VALUES (@Image, @Description, @ImageWidth, @ImageHeight, @VehicleId)";

                using (SqlCommand cmd = new SqlCommand(query, Database.Instance.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Image", image);
                    cmd.Parameters.AddWithValue("@Description", desc);
                    cmd.Parameters.AddWithValue("@ImageWidth", imageWidth);
                    cmd.Parameters.AddWithValue("@ImageHeight", imageHeight);
                    cmd.Parameters.AddWithValue("@VehicleId", vehicleId);
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