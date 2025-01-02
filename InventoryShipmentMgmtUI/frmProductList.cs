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
    public partial class frmProductList : Form
    {
        private readonly ProductService prdService;
        private List<ProductResponse> productData;

        public frmProductList(bool isComingFromDelete)
        {
            prdService = new ProductService();
            productData = new List<ProductResponse>();
            InitializeComponent();
            var data = LoadAllProductsAsync(isComingFromDelete);

        }
        public async Task LoadAllProductsAsync(bool isComingFromDelete)
        {
            try
            {
                if (!isComingFromDelete)
                {
                    dgvProductList.DataSource = null;
                    dgvProductList.Columns.Clear();

                }
                var products = await prdService.GetProductsAsync();
                productData = products.data;
                if (products.statusCode == 200 && products.status)
                {
                    // Defineing the data Column for data grid
                    DataTable proudctTable = new DataTable();
                    proudctTable.Columns.Add("ProductId", typeof(int));
                    proudctTable.Columns.Add("ProductName", typeof(string));
                    proudctTable.Columns.Add("Quantity", typeof(int));
                    proudctTable.Columns.Add("Price", typeof(decimal));
                    proudctTable.Columns.Add("CreatedOn", typeof(DateTime));
                    proudctTable.Columns.Add("UpdatedOn", typeof(DateTime));

                    foreach (var prdItem in products.data)
                    {
                        proudctTable.Rows.Add(prdItem.productId, prdItem.productName, prdItem.Quantity, prdItem.price, prdItem.CreatedOn, prdItem.UpdatedOn);
                    }

                    dgvProductList.DataSource = proudctTable;
                    AddActionColumns();
                    dgvProductList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    //Define the HeaderText of Each column
                    dgvProductList.Columns[0].HeaderText = "Product Id";
                    dgvProductList.Columns[1].HeaderText = "Product Name";
                    dgvProductList.Columns[2].HeaderText = "Quantity";
                    dgvProductList.Columns[3].HeaderText = "Price";
                    dgvProductList.Columns[4].HeaderText = "Created On";
                    dgvProductList.Columns[5].HeaderText = "Updated On";

                    dgvProductList.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            // Create an instance of Form2          
            frmNewProduct newProduct = new frmNewProduct(0, this);
            // Show Form2
            newProduct.Show();
        }

        // Assuming you have a DataGridView named 'dataGridView1'
        private void AddActionColumns()
        {
            try
            {
                // Add 'Edit' button column
                DataGridViewButtonColumn editColumn = new DataGridViewButtonColumn
                {
                    Name = "Edit",
                    Text = "Edit",
                    HeaderText = "Actions",
                    UseColumnTextForButtonValue = true

                };
                dgvProductList.Columns.Add(editColumn);

                // Add 'Delete' button column
                DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn
                {
                    Name = "Delete",
                    Text = "Delete",
                    HeaderText = "Actions",
                    UseColumnTextForButtonValue = true
                };
                dgvProductList.Columns.Add(deleteColumn);
                // Add the DataGridView to the form
                this.Controls.Add(dgvProductList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void dgvProductList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ProductResponse productResponse = new ProductResponse();
                // Retrieve the Product object from the clicked row
                productResponse = productData[e.RowIndex];
                if (e.RowIndex >= 0)
                {
                    if (dgvProductList.Columns[e.ColumnIndex].Name == "Edit")
                    {
                        // Retrieve the Product object from the clicked row
                        //productResponse = productData[e.RowIndex];
                        // Create an instance of New Product Page          
                        frmNewProduct newProduct = new frmNewProduct(productResponse.productId, this);
                        // Show Form2
                        newProduct.Show();
                    }
                    else if (dgvProductList.Columns[e.ColumnIndex].Name == "Delete")
                    {
                        // Confirm the delete action
                        var confirmResult = MessageBox.Show("Are you sure you want to delete this product?",
                            "Confirm Delete", MessageBoxButtons.YesNo);
                        if (confirmResult == DialogResult.Yes)
                        {
                            // Remove the selected person from the list
                            //productResponse = productData[e.RowIndex];
                            _ = DeleteProductAsync(productResponse.productId);

                            // Refresh the DataGridView to reflect the changes
                            //dgvProductList.DataSource = null;  // Reset data binding
                            //dgvProductList.DataSource = productData;  // Rebind the updated data source
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        public async Task DeleteProductAsync(int productId)
        {
            try
            {
                if (productId > 0)
                {
                    ProductRequest productRequest = new ProductRequest();
                    var result = await prdService.DeleteProductAsync(productId);
                    if (result.statusCode == 200 && result.status)
                    {
                        MessageBox.Show($"Success: {result.responseMessage}");
                        //this.Close();
                        // Optionally, if the MainForm is not yet open, create a new instance of it
                        //frmProductList product = new frmProductList();
                        //product.Show();
                        _ = LoadAllProductsAsync(false);
                    }
                    else
                    {
                        MessageBox.Show($"Error: {result.responseMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

    }
}
