using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.IO;

  public  class ServisData
    {
        public  void LogRequest(string url, string request, string response, bool Controll, string Error)
        {
            try
            {
                // SQL bağlantısı
                SqlConnection connection = new SqlConnection("Data Source = DOGANS\\MSSQLSERVER02; Initial Catalog = Kütüphane; Integrated Security = True");

                connection.Open();

                // SQL komutu
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Durum (Url, Request, Response,Control,Error,Time) VALUES (@Url, @Request, @Response, @Control,@Error,@Time)", connection))
                {
                    cmd.Parameters.AddWithValue("@Time", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Url", url);
                    cmd.Parameters.AddWithValue("@Request", request);
                    cmd.Parameters.AddWithValue("@Response", response);
                    cmd.Parameters.AddWithValue("@Control", Controll);
                    cmd.Parameters.AddWithValue("@Error", Error);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
            }
        }
        
    }

