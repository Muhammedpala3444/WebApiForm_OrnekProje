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
using static WebApiFormOrnek.ServisCall;

namespace WebApiFormOrnek
{
    public partial class Form1 : Form
    { 
        ServisData servisData;
        ServisCall servisCall;
        public Form1()
        {
            InitializeComponent();
            servisData = new ServisData();
            servisCall = new ServisCall();
        }
        public class Language
        {
            public int ID { get; set; }
            public string lanquaqe { get; set; }
            public string Founder { get; set; }
            public int Year { get; set; }
            public bool IsPopular { get; set; }
        }
        public void ClearBox()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            comboBox1.Items.Clear();
        }
        private async void ViewData_Click(object sender, EventArgs e)
        {
            ServisCall.Cagir serviceType = ServisCall.Cagir.ViewData;
            Language language = new Language();

            bool ViewResponse = await servisCall.Cagirim(serviceType, language);
            string apiUrl = ConstDefinition.ViewData;
            List<Language> languages = await GetLanguage(apiUrl);
            dataGridView1.DataSource = languages;

        }
        private async Task<List<Language>> GetLanguage(string apiUrl) 
        {
            try
            {
                List<Language> languages = new List<Language>();

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);


                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        languages = JsonConvert.DeserializeObject<List<Language>>(jsonContent);
                        servisData.LogRequest(ConstDefinition.ViewData, requestMessage.ToString(), jsonContent, true, "");
                    }
                    else
                    {
                        MessageBox.Show("Web API'den veri çekerken hata oluştu. Durum Kodu: " + response.StatusCode);
                    }
                }
                return languages;
            }    
            catch (Exception ex)
            {
                servisData.LogRequest(ConstDefinition.ViewData, "RequestContent", "ResponseContent", false, ex.Message);
                MessageBox.Show(ex.Message);
                throw;
            }
        }
        private async void Save_Click(object sender, EventArgs e)
        {
            ServisCall.Cagir serviceType = ServisCall.Cagir.Save;

            Language language = new Language

            {
                ID = Convert.ToInt16(textBox1.Text),
                lanquaqe = Convert.ToString(textBox2.Text),
                Founder = Convert.ToString(textBox3.Text),
                Year = Convert.ToInt32(textBox4.Text),
                IsPopular = Convert.ToBoolean(comboBox1.SelectedItem),

            };

            string jsonContent = JsonConvert.SerializeObject(language);

            try
            {
                bool saveResponse = await servisCall.Cagirim(serviceType, language);

                if (saveResponse)
                {
                    MessageBox.Show("Dil başarılı bir şekilde eklendi.");
                    await RefreshDataGridView();
                }
                else
                {
                    MessageBox.Show("Dil eklenemedi.");
                }
                servisData.LogRequest(ConstDefinition.Save, jsonContent, "ResponseContent", true, "");
            }
            catch (Exception ex)
            {
                servisData.LogRequest(ConstDefinition.Save, jsonContent, "ResponseContent", false, ex.Message);
                MessageBox.Show(ex.Message);
            }
            ClearBox();
        }

        //burası düzeltilecek
        private async Task RefreshDataGridView()
        {
            // Web API adresi
            string apiUrl = "http://localhost:8080/api/Languages";

            // Web API'den veriyi çek
            var Language = await GetLanguage(apiUrl);

            // DataGridView'e veriyi bind et
            dataGridView1.DataSource = Language;
        }

        private async void Delete_Click(object sender, EventArgs e)
        {
            ServisCall.Cagir serviceType = ServisCall.Cagir.Delete;

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            int selectedId = (int)selectedRow.Cells["ID"].Value;
            string selectedLanquaqe = (string)selectedRow.Cells["lanquaqe"].Value;
            string selectedFounder = (string)selectedRow.Cells["Founder"].Value;
            int selectedYear = (int)selectedRow.Cells["Year"].Value;
            bool selectedIsPopular = (bool)selectedRow.Cells["IsPopular"].Value;

            Language language = new Language

            {
                ID = (int)selectedRow.Cells["ID"].Value,
                lanquaqe = (string)selectedRow.Cells["lanquaqe"].Value,
                Founder = (string)selectedRow.Cells["Founder"].Value,
                Year = (int)selectedRow.Cells["Year"].Value,
                IsPopular = (bool)selectedRow.Cells["IsPopular"].Value

            };
            string jsonContent = JsonConvert.SerializeObject(language);

            try
            {
                bool Deleteresponse = await servisCall.Cagirim(serviceType, language);
                if (Deleteresponse)
                {
                    MessageBox.Show("Dil başarılı bir şekilde silindi.");
                    await RefreshDataGridView();
                }
                else
                {
                    MessageBox.Show("Dil silinmedi.");
                }
                servisData.LogRequest(ConstDefinition.Delete, jsonContent, "ResponseContent", true, "");
            }

            catch (Exception ex)
            {
                servisData.LogRequest(ConstDefinition.Delete, jsonContent, "ResponseContent", true, "");
                MessageBox.Show(ex.Message);
            }

            ClearBox();
        }

        private async void Update_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    string id = selectedRow.Cells["ID"].Value.ToString();
                    string language = textBox2.Text;
                    string founder = textBox3.Text;
                    string year = textBox4.Text;
                    string isPopular = comboBox1.Text;

                    Language updatedData = new Language()
                    {
                        ID = int.Parse(id),
                        lanquaqe = language,
                        Founder = founder,
                        Year = int.Parse(year),
                        IsPopular = bool.Parse(isPopular)
  
                    };
                    await UpdateDataAsync(updatedData);
                }
                else
                {
                    MessageBox.Show("Lütfen güncellenecek bir kayıt seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }

        }
        private async Task UpdateDataAsync(Language updatedData)
        {
            ServisCall.Cagir serviceType = ServisCall.Cagir.Update;
            Language language = new Language();
            bool UpdateResponse = await servisCall.Cagirim(serviceType, language);
            string apiUrl = ConstDefinition.Update;
            string jsonContent = JsonConvert.SerializeObject(updatedData);
            try
            {
                using (HttpClient client = new HttpClient())
                {

                    HttpResponseMessage response = await client.PostAsync(apiUrl,
                        new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(updatedData), Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Veri güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await RefreshDataGridView();

                    }
                    else
                    {
                        MessageBox.Show("Veri güncelleme başarısız.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    servisData.LogRequest(apiUrl, jsonContent, "ResponseContent", true, "");
                }
            }
            catch (Exception ex)
            {
                servisData.LogRequest(apiUrl, jsonContent, "ResponseContent", false, ex.Message);
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ClearBox();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                textBox1.Text = selectedRow.Cells["ID"].Value.ToString();
                textBox2.Text = selectedRow.Cells["lanquaqe"].Value.ToString();
                textBox3.Text = selectedRow.Cells["Founder"].Value.ToString();
                textBox4.Text = selectedRow.Cells["Year"].Value.ToString();
                comboBox1.Text = selectedRow.Cells["IsPopular"].Value.ToString();
            }
        }
    }
 }










