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
    public partial class PortfoliosForm : Form
    {
       
       
        public bool IsFormOpen { get; set; }

        public PortfoliosForm()
        {
            InitializeComponent();
            IsFormOpen = false;
        }
       
        
       
        //private void LoadFolioPositionsData(DataGridView dataGridView,string folio_name)
        //{

        //    DataTable data_table = NewPositionViewDataTable(dataGridView);
        //    dataGridView.DataSource = data_table;

           
        //}
       
        private void Config_columns_Click(object sender, EventArgs e)
        {
            TabPage tab = tabControl_portfolios.SelectedTab;
            foreach (Control control in tab.Controls)
            {
                if (control.Name == "dataGridView")
                {
                    DataGridView view = (DataGridView)control;
                  
                    ConfigColumnsForm config_form = new ConfigColumnsForm(view.Columns);
                   // portfolios_form.MdiParent = this;
                    config_form.Text = "Configuration";
                    config_form.ShowDialog(this);
                }
            }
        }

        private void PortfoliosForm_Shown(object sender, EventArgs e)
        {
            IsFormOpen = true;
        }

        private void PortfoliosForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsFormOpen = false;
        }
    }
}