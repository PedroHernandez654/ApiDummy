using ApiDummy.Model;
using ApiDummy.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ApiDummy
{
    public partial class ListProduct : Form
    {
        private settings setting;

        public ListProduct()
        {
            InitializeComponent();
            setting = new settings();
        }

        private async void ListProduct_Load(object sender, EventArgs e)
        {
            var productos = await Products();

            dgvProducts.DataSource = productos;

        }

        public async Task<List<Product>> Products()
        {
            List<Product> products = new List<Product>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(setting.URL);
                HttpResponseMessage response = await client.GetAsync("products");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();

                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonResult);

                    products = apiResponse.Products;
                    var total = apiResponse.Total;
                    var skip = apiResponse.Skip;
                    var limit = apiResponse.Limit;
                }
                else
                {
                    var errorResult = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {

            }
            return products;
        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            AddProduct form = new AddProduct();
            form.Show();
        }

        private void dgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                int rowIndex = dgvProducts.SelectedRows[0].Index;

                object idCellValue = dgvProducts["ID", rowIndex].Value;

                AddProduct nuevaVistaForm = new AddProduct();

                nuevaVistaForm.SetValor(Convert.ToInt32(idCellValue));

                nuevaVistaForm.Show();
;
            }
        }
    }
}
