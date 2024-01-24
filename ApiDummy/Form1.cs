using ApiDummy.Model;
using Newtonsoft.Json;
using System.Net.Http.Json;
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
using System.Numerics;

namespace ApiDummy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            
        }

        public async Task<(LoginResponse loginReponse, string error)> Login(LoginRequest request, string URL)
        {
            LoginResponse result = new LoginResponse();
            string errorResult = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URL);

                    var jsonLogin = JsonConvert.SerializeObject(request);
                    var loginRequest = new StringContent(jsonLogin, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("auth/login", loginRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                        /*var resultado = await response.Content.ReadAsStringAsync();
                        var algo = JsonConvert.DeserializeObject(resultado);*/
                    }
                    else
                    {
                        errorResult = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                errorResult = ex.Message;
            }
            return (result, errorResult);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string URL = "https://dummyjson.com/";

            LoginRequest loginRequest = new LoginRequest();
            loginRequest.username = username;
            loginRequest.password = password;

            (LoginResponse response, string error) = await Login(loginRequest, URL);

            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!string.IsNullOrEmpty(response.token))
            {
                this.Hide();
                ListProduct list = new ListProduct();
                list.Show();
            }
        }
    }
}
