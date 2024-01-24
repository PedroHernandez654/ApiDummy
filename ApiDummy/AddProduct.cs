using ApiDummy.Model;
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
    public partial class AddProduct : Form
    {
        private settings setting;
        private int valorSeleccionado;

        public AddProduct()
        {
            InitializeComponent();
            setting = new settings();
        }

        public async void SetValor(int valor)
        {
            valorSeleccionado = valor;
            Product editProduct = await GetProduct(valorSeleccionado);

            if (editProduct != null)
            {
                txtTitle.Text = editProduct.Title;
                txtDescription.Text = editProduct.Description;
                txtBrand.Text = editProduct.Brand;
                txtCategory.Text = editProduct.Category;
                txtDiscountPercentage.Text = editProduct.DiscountPercentage.ToString();
                txtPrice.Text = editProduct.Price.ToString();
                txtRating.Text = editProduct.Rating.ToString();
                txtStock.Text = editProduct.Stock.ToString();
                txtThumbnail.Text = editProduct.Thumbnail;

                btnAddProduct.Enabled = false;
                btnAddProduct.Visible= false;
                btnEditar.Enabled = true;
                btnEditar.Visible = true;
            }

        }

        private async void btnAddProduct_Click(object sender, EventArgs e)
        {
            bool validacion = Validaciones();
            if (validacion)
            {
                Product product = new Product();

                product.Title = txtTitle.Text;
                product.Description = txtDescription.Text;
                product.Category = txtCategory.Text;
                product.Price = Convert.ToDecimal(txtPrice.Text);
                product.Brand = txtBrand.Text;
                product.Thumbnail = txtThumbnail.Text;
                product.DiscountPercentage = Convert.ToDouble(txtDiscountPercentage.Text);
                product.Stock = Convert.ToInt32(txtStock.Text);

                (string complete, bool responseBool) = await AddNewProduct(product);

                if (responseBool)
                {
                    MessageBox.Show("Producto Añadido", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(complete, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }          
        }

        public async Task<(string,bool)> AddNewProduct(Product newProduct)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(setting.URL);

                    var jsonProduct = JsonConvert.SerializeObject(newProduct);
                    var request = new StringContent(jsonProduct, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("products/add", request);

                    if (response.IsSuccessStatusCode)
                    {
                        
                        var resultado = await response.Content.ReadAsStringAsync();
                        var algo = JsonConvert.DeserializeObject(resultado);
                        return (algo.ToString(),true);
                    }
                    else
                    {
                        var errorResult = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Error en la solicitud: {response.StatusCode}, {errorResult}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<Product> GetProduct(int idProduct)
        {
            Product products = new Product();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(setting.URL);
                HttpResponseMessage response = await client.GetAsync($"products/{idProduct}");

                if (response.IsSuccessStatusCode)
                {
                    products = await response.Content.ReadFromJsonAsync<Product>();
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

        public async Task<(string, bool)> UpdateProduct(Product updateProduct, int idProduct)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(setting.URL);

                    var jsonProduct = JsonConvert.SerializeObject(updateProduct);
                    var request = new StringContent(jsonProduct, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"products/{idProduct}", request);

                    if (response.IsSuccessStatusCode)
                    {

                        var resultado = await response.Content.ReadAsStringAsync();
                        var algo = JsonConvert.DeserializeObject(resultado);
                        return (algo.ToString(), true);
                    }
                    else
                    {
                        var errorResult = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Error en la solicitud: {response.StatusCode}, {errorResult}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private  async void btnEditar_Click(object sender, EventArgs e)
        {
            bool validaciones = Validaciones();

            if (validaciones)
            {
                Product product = new Product();

                product.Title = txtTitle.Text;
                product.Description = txtDescription.Text;
                product.Category = txtCategory.Text;
                product.Price = Convert.ToDecimal(txtPrice.Text);
                product.Brand = txtBrand.Text;
                product.Thumbnail = txtThumbnail.Text;
                product.DiscountPercentage = Convert.ToDouble(txtDiscountPercentage.Text);
                product.Stock = Convert.ToInt32(txtStock.Text);

                (string complete, bool responseBool) = await UpdateProduct(product, valorSeleccionado);

                if (responseBool)
                {
                    MessageBox.Show("Producto Actualizado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(complete, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public bool Validaciones()
        {
            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                MessageBox.Show("Titulo requerido", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(txtDescription.Text))
            {
                MessageBox.Show("Descripción requerido", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(txtCategory.Text))
            {
                MessageBox.Show("Categoría requerido", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(txtPrice.Text) || !decimal.TryParse(txtPrice.Text, out decimal priceValue))
            {
                MessageBox.Show("Precio requerido", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(txtBrand.Text))
            {
                MessageBox.Show("Marca requerido", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(txtThumbnail.Text))
            {
                MessageBox.Show("Miniatura requerido", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(txtDiscountPercentage.Text) || !double.TryParse(txtDiscountPercentage.Text, out double discount))
            {
                MessageBox.Show("Descuento requerido", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (string.IsNullOrEmpty(txtStock.Text) || !int.TryParse(txtStock.Text, out int stockInt))
            {
                MessageBox.Show("Cantidad requerido", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
