using InventoryManagementSystemUI.ApiServices;
using InventoryShipmentMgmtUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryShipmentMgmtUI
{
    public partial class frmNewProduct : Form
    {
        private readonly ProductService prdServices;
        private int prdtId;
        public frmNewProduct(int prdtIds)
        {
            prdServices = new ProductService();
            InitializeComponent();
            prdtId = prdtIds;
            if (prdtId > 0)
            {
                _ = GetProductById(prdtId);
                btnUpdate.Visible = true;
                btnSave.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
                btnUpdate.Visible = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtProduct.Text))
                {
                    MessageBox.Show($"Error: Please enter the product name!");
                }
                else if (txQuantity.Text == "" || txQuantity.Text == "0")
                {
                    MessageBox.Show($"Error: Please enter the valid quantity!");
                }
                else if (txtPrice.Text == "" || txtPrice.Text == "0")
                {
                    MessageBox.Show($"Error: Please enter the valid price!");
                }
                else
                {
                    _ = AddNewProductAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public async Task AddNewProductAsync()
        {
            try
            {
                ProductRequest productRequest = new ProductRequest();
                productRequest.ProductName = txtProduct.Text;
                productRequest.Quantity = Convert.ToInt32(txQuantity.Text);
                productRequest.Price = Convert.ToInt32(txtPrice.Text);

                var result = await prdServices.CreateProductAsync(productRequest);
                if (result.statusCode == 200 && result.status)
                {
                    MessageBox.Show($"Success: {result.responseMessage}");
                    this.Close();
                    // Optionally, if the MainForm is not yet open, create a new instance of it
                    frmProductList product = new frmProductList();
                    product.Show();
                }
                else
                {
                    MessageBox.Show($"Error: {result.responseMessage}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtProduct.Text))
                {
                    MessageBox.Show($"Error: Please enter the product name!");
                }
                else if (txQuantity.Text == "" || txQuantity.Text == "0")
                {
                    MessageBox.Show($"Error: Please enter the valid quantity!");
                }
                else if (txtPrice.Text == "" || txtPrice.Text == "0")
                {
                    MessageBox.Show($"Error: Please enter the valid price!");
                }
                else
                {
                    _ = UpdateProductAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public async Task UpdateProductAsync()
        {
            try
            {
                ProductRequest productRequest = new ProductRequest();
                productRequest.ProductId = prdtId; // need to pass the product Id dynamically
                productRequest.ProductName = txtProduct.Text;
                productRequest.Quantity = Convert.ToInt32(txQuantity.Text);
                productRequest.Price = Convert.ToDecimal(txtPrice.Text);

                var result = await prdServices.UpdateProductAsync(productRequest);
                if (result.statusCode == 200 && result.status)
                {
                    MessageBox.Show($"Success: {result.responseMessage}");
                    this.Close();
                    // Optionally, if the MainForm is not yet open, create a new instance of it

                    frmProductList product = new frmProductList();
                    product.Show();
                }
                else
                {
                    MessageBox.Show($"Error: {result.responseMessage}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public async Task GetProductById(int productId)
        {
            try
            {
                _ = new ProductModel();
                ProductModel response = await prdServices.GetProductById(productId);
                if (response.status && response.statusCode == 200)
                {
                    txtProduct.Text = response.data.productName;
                    txQuantity.Text = response.data.quantity.ToString();
                    txtPrice.Text = response.data.price.ToString();
                }
                else
                {
                    MessageBox.Show($"Error: {response.responseMessage}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        private void txQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                // Allow only digits, control keys (e.g., Backspace), and a single decimal point
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true; // Reject the input
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                // Allow only digits, control keys (e.g., Backspace), and a single decimal point
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                {
                    e.Handled = true; // Reject the input
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
