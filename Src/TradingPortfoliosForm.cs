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
        private static int instances = 0;
       
        
        public static int InstancesCount()
        {
            return instances;
        }
        public PortfoliosForm()
        {
            InitializeComponent();
            button_cancel_RT.Enabled = false;
           
            DataSource.StartRTWatch();
            LoadBooks();
            instances++;
        }
       
        private void LoadBooks()
        {
           
            AddTabs(Model.Instance.Portfolios);
        }
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(false == backgroundWorker.CancellationPending)
            {
                if(false == DataSource.RTUpdatesQueue.IsEmpty)
                {
                    Object obj = new Object();
                    if(DataSource.RTUpdatesQueue.TryDequeue(out obj))
                    {
                        BackgroundWorker worker = (BackgroundWorker)sender;
                        worker.ReportProgress(0,obj);
                    }
                }
            }
            e.Result = "cancelled";
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(e.UserState is Security)
            {
                Security sec = (Security)e.UserState;
                UpdatePricesInDataGrids(sec);
            }
            else if(e.UserState is Position)
            {
                Position pos = (Position)e.UserState;
                UpdatePositionInDataGrids(pos);
            }else
            {
                //issue with data type in queue
            }
        }
        private void Start_rt_Click(object sender, EventArgs e)
        {
            if(false == backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync();
                button_start_rt.Enabled = false;
                button_cancel_RT.Enabled = true;
            }
        }

        private void Button_cancel_RT_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();
            button_cancel_RT.Enabled = false;
            button_start_rt.Enabled = true;
        }

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
    }
}