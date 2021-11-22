using System;
using MaFacture.BLL;
using MaFacture.BO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaFacture.WinForms
{
    public partial class FrmProductList : Form
    {
        private ProductBLO productBLO;

        public FrmProductList()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            productBLO = new ProductBLO(ConfigurationManager.AppSettings["DbFolder"]);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            Form f = new FrmProductEdit(loadData);
            f.Show();

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    Form f = new FrmProductEdit
                    (
                        dataGridView1.SelectedRows[i].DataBoundItem as Product,
                        loadData
                    );
                    f.ShowDialog();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void FrmProductList_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtSearch.Text))
                loadData();
            else
                txtSearch.Clear();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            List<ProductListPrint> items = new List<ProductListPrint>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                Product p = dataGridView1.Rows[i].DataBoundItem as Product;
                byte[] logo = null;
                if (!string.IsNullOrEmpty(p.Logo))
                {
                    logo = File.ReadAllBytes
                    (
                        Path.Combine
                        (
                            ConfigurationManager.AppSettings["DbFolder"],
                            "logo",
                            p.Logo
                        )
                    );
                }
                items.Add
                (
                   new ProductListPrint
                   (
                       p.Reference,
                       p.Name,
                       p.UnitPrice,
                       p.Picture,
                       logo
                    )
                );
            }
            Form f = new FrmPreview("ProductListRpt.rdlc", items);
            f.Show();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (
                    MessageBox.Show
                    (
                        "Do you really want to delete this product(s)?",
                        "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question
                    ) == DialogResult.Yes
                )
                {
                    for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                    {
                        productBLO.DeleteProduct(dataGridView1.SelectedRows[i].DataBoundItem as Product);
                    }
                    loadData();
                }
            }
        }

        private void loadData()
        {
            string value = txtSearch.Text.ToLower();
            var products = productBLO.GetBy
            (
                x =>
                x.Reference.ToLower().Contains(value) ||
                x.Name.ToLower().Contains(value)
            ).OrderBy(x => x.Reference).ToArray();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = products;
            lblRowCount.Text = $"{dataGridView1.RowCount} rows";
            dataGridView1.ClearSelection();
        }

            private void lblRowCount_Click(object sender, EventArgs e)
            {

            }

            private void Search_Click(object sender, EventArgs e)
            {

            }

            private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
            {

            }
        }
    }

