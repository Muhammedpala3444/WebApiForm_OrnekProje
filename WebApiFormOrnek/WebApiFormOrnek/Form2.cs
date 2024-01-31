using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebApiFormOrnek
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        SqlConnection baglan = new SqlConnection("Data Source=DOGANS\\MSSQLSERVER02;Initial Catalog=Kütüphane;Integrated Security=True");
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                baglan.Open();

                SqlCommand komut = new SqlCommand("SELECT COUNT(*) FROM Kayit WHERE KullaniciAdi = @KullaniciAdi AND Sifre = @Sifre",baglan);
                komut.Parameters.AddWithValue("@KullaniciAdi", textBox1.Text);
                komut.Parameters.AddWithValue("@Sifre", textBox2.Text);

                int result = (int)komut.ExecuteScalar();

                if (result > 0)
                {
                    // Credentials are correct
                    Form1 yeni = new Form1();
                    yeni.Show();
                    this.Hide();
                }
                else
                {
                    // Credentials are incorrect
                    MessageBox.Show("Hatalı Giriş...\n Tekrar Deneyiniz");
                    textBox1.Clear();
                    textBox2.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                baglan.Close();
            }
        }
    }
    }

