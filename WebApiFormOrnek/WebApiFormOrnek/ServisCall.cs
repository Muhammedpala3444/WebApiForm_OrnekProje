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
using static WebApiFormOrnek.Form1;

namespace WebApiFormOrnek
{

    public static class ConstDefinition
    {
        public  const string ViewData = "http://localhost:8080/api/Languages";
        public  const string Save = "http://localhost:8080/api/Languages/Post";
        public  const string Delete = "http://localhost:8080/api/Languages/DeleteData";
        public  const string Update = "http://localhost:8080/api/Languages/UpdateData";
    }
   public  class ServisCall
    {
        public enum Cagir
        {  
            ViewData = 1,
            Save = 2,
            Delete = 3,
            Update = 4
        };

  
        public async Task<bool> Cagirim(Cagir serviceType, Language language)
        {
            string jSondata =String.Empty;
            HttpResponseMessage response = null;
            if (language != null)
            {
                jSondata = JsonConvert.SerializeObject(language);

            }
            using (HttpClient client = new HttpClient())
            {

                switch (serviceType)
                {
                    case Cagir.ViewData:
                        response = await client.GetAsync(ConstDefinition.ViewData);
                        MessageBox.Show("ViewData method Çağırıldı");
                        break;
                    case Cagir.Save:
                        response = await client.PostAsync(ConstDefinition.Save, new StringContent(jSondata, Encoding.UTF8, "application/json"));
                        MessageBox.Show("Save method çağırıldı");
                        break;
                    case Cagir.Delete:
                        response = await client.PostAsync(ConstDefinition.Delete, new StringContent(jSondata, Encoding.UTF8, "application/json"));
                        MessageBox.Show("Delete method çağırıldı");
                        break;
                    case Cagir.Update:
                        response = await client.PostAsync(ConstDefinition.Update, new StringContent(jSondata, Encoding.UTF8, "application/json"));
                        MessageBox.Show("Update method çağırıldı");
                        break;
                    default:

                        return false;
                       
                }
                return response.IsSuccessStatusCode;
            }


         }

      
    }
}
