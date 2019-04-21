using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dashboard
{
    public partial class ConfigColumnsForm : Form
    {
        DataGridViewColumnCollection columns_;
        public ConfigColumnsForm(DataGridViewColumnCollection columns)
        {
            columns_ = columns;
            InitializeComponent();

            RefreshGrid();
        }
        private void RefreshGrid()
            {
            DataTable data_source1 = new DataTable();// dataGridView1.DataSource;
            DataTable data_source2 = new DataTable();// (DataTable)dataGridView2.DataSource;
            data_source1.Columns.Add("Hidden Columns");
            data_source2.Columns.Add("Visible Columns");
            
            foreach (DataGridViewColumn col in columns_)
            {
                
                
                if (col.Visible == true)
                    data_source2.Rows.Add(col.Name);
                else
                    data_source1.Rows.Add(col.Name);
            }
            dataGridView1.DataSource = data_source1;
            dataGridView2.DataSource = data_source2;
        }

        private void Add_column_Click(object sender, EventArgs e)
        {
            Int32 selected_rows_count = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);
            for (int i = 0; i < selected_rows_count; ++i)
            {
                foreach (DataGridViewColumn col in columns_)
                {
                    if(col.Name == (string)dataGridView1.SelectedRows[i].Cells["Hidden Columns"].Value)
                    {
                        col.Visible = true;
                    }
                }
            }
            RefreshGrid();
        }

        private void Hide_column_Click(object sender, EventArgs e)
        {
            Int32 selected_rows_count = dataGridView2.Rows.GetRowCount(DataGridViewElementStates.Selected);
            for (int i=0; i < selected_rows_count; ++i )
            {
              
                foreach (DataGridViewColumn col in columns_)
                {
                    if (col.Name == (string)dataGridView2.SelectedRows[i].Cells["Visible Columns"].Value)
                    {
                        col.Visible = false;
                    }
                }
            }
            RefreshGrid();
        }
    }
}
