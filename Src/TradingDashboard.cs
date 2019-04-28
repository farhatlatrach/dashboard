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
    public partial class TradingDashboard : Form
    {
       
        private PortfoliosForm portfolios_form= new PortfoliosForm();
        private RiskAnalyticsForm risk_analytics_form = new RiskAnalyticsForm();
       
        public static DataSet the_data_set = new DataSet();
      
        public TradingDashboard()
        {
            InitializeComponent();
          
            portfolios_form.MdiParent = this;
            portfolios_form.Text = "Portfolios";

            risk_analytics_form.MdiParent = this;
            risk_analytics_form.Text = "Risk Analytics";


            CretaeDataTables();
            // creer une instance de MktDataSubscription
            // run it
            backgroundWorker.RunWorkerAsync();
          
        }
        private void CretaeDataTables()
        {
            //portfolios table
            the_data_set.Tables.Add("portfolios_table");
            the_data_set.Tables["portfolios_table"].Columns.Add("Name", typeof(string));
            the_data_set.Tables["portfolios_table"].Columns.Add("Currency", typeof(string));

            //constraint unique portfolio
            UniqueConstraint unique_portfolio_name = new UniqueConstraint(
                new DataColumn[] 
                {
                    the_data_set.Tables["portfolios_table"].Columns["Name"]
                });
            the_data_set.Tables["portfolios_table"].Constraints.Add(unique_portfolio_name);

            //Positions table
            the_data_set.Tables.Add("positions_table");
            the_data_set.Tables["positions_table"].Columns.Add("Portfolio",typeof(string));
            the_data_set.Tables["positions_table"].Columns.Add("Security Name", typeof(string));
            the_data_set.Tables["positions_table"].Columns.Add("BOD PnL", typeof(double));
            the_data_set.Tables["positions_table"].Columns.Add("Realized PnL", typeof(double));
            the_data_set.Tables["positions_table"].Columns.Add("BOD Position", typeof(double));
            the_data_set.Tables["positions_table"].Columns.Add("Bought Quantity", typeof(double));
            the_data_set.Tables["positions_table"].Columns.Add("Average Bought Price", typeof(double));
            the_data_set.Tables["positions_table"].Columns.Add("Sold Quantity", typeof(double));
            the_data_set.Tables["positions_table"].Columns.Add("Average Sold Price", typeof(double));
           
           
            //constraint unique portfolio + security
            UniqueConstraint unique_position_constraint = new UniqueConstraint(
                new DataColumn[]
                {
                    the_data_set.Tables["positions_table"].Columns["Portfolio"],the_data_set.Tables["positions_table"].Columns["Security Name"]
                });
            the_data_set.Tables["positions_table"].Constraints.Add(unique_position_constraint);

            //Securities table
            the_data_set.Tables.Add("securities_table");
            the_data_set.Tables["securities_table"].Columns.Add("LastPrice", typeof(double));
            the_data_set.Tables["securities_table"].Columns.Add("PreviousClose", typeof(double));
            the_data_set.Tables["securities_table"].Columns.Add("Multiplier", typeof(double));
            the_data_set.Tables["securities_table"].Columns.Add("Quotation Factor", typeof(double));
            the_data_set.Tables["securities_table"].Columns.Add("Security Type", typeof(string));
            the_data_set.Tables["securities_table"].Columns.Add("Currency", typeof(string));
            the_data_set.Tables["securities_table"].Columns.Add("Security Name", typeof(string));
            the_data_set.Tables["securities_table"].Columns.Add("Security Ticker", typeof(string));
            the_data_set.Tables["securities_table"].Columns.Add("Sector", typeof(string));
            the_data_set.Tables["securities_table"].Columns.Add("Country", typeof(string));
            //constraint unique name + ticker
            UniqueConstraint unique_security_constraint = new UniqueConstraint(
                new DataColumn[]
                {
                    the_data_set.Tables["securities_table"].Columns["Security Ticker"], the_data_set.Tables["securities_table"].Columns["Security Name"]
                });
            the_data_set.Tables["securities_table"].Constraints.Add(unique_security_constraint);


        }
        private void ShowPortfoliosForm(object sender, EventArgs e)
        {
            
            if (portfolios_form.IsFormOpen ==false)
            {
               
                portfolios_form.Show();
                
            }
            else
                portfolios_form.WindowState = FormWindowState.Normal;
        }

        private void ShowRiskAnalyticsForm(object sender, EventArgs e)
        {
            if (risk_analytics_form.IsFormOpen == false)
            {
                
                
                risk_analytics_form.Show();
            }
            else
                risk_analytics_form.WindowState = FormWindowState.Normal;
        }

        
        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
       
        private void Loadbooksfromfile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    var filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                    {
                        LoadDataIntoTablesFromFile(reader);
                       
                        MessageBox.Show(String.Format("books loaded from file {0} ", openFileDialog.FileName),"Books Loaded");
                       
                    }
                }
            }
           
        }
        private void LoadDataIntoTablesFromFile(System.IO.StreamReader reader)
        {
            bool at_header = true;
           
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (at_header)
                {
                    at_header = false;
                    continue;//first line is the header
                }

                var values = line.Split('|');
                /*
              

             Date[0]|Book[1]Security[2]|Delta[3]|TdyPnL[4]|BODPnL[5]|TdingPnL[6]|DivPnL[7]|LastPx[8]|PrevCLS[9]
             |Position[10]|BODPos[11]|Bought[12]|BuyPx[13]|Sold[14]|SellPx[15]|Multiplier[16]|SecType[17]|Curncy[18]|YTDPnL[19]
             |MTDPnL[20]|WTDPnL[21]   
             */

                
                    Position pos = new Position()
                    {
                        PortfolioName = values[1],
                        Underlying = values[2],
                        RealizedPnL = Convert.ToDouble(values[4]),
                        BODPnL = Convert.ToDouble(values[5]),
                        BeginOfDayQuantity = Convert.ToDouble(values[11]),
                        BoughtQuantity = Convert.ToDouble(values[12]),
                        BoughtAveragePrice = Convert.ToDouble(values[13]),
                        SoldQuantity = Convert.ToDouble(values[14]),
                        SoldAveragePrice = Convert.ToDouble(values[15])
                    };
                    Security sec = new Security(values[2])
                    {
                       
                        SecurityType = values[17]
                      
                    };
                    DataSource.RTUpdatesQueue.Enqueue(pos);
                    DataSource.RTUpdatesQueue.Enqueue(sec);
                   
            }
         
        }

       
        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is Security)
            {
                Security sec = (Security)e.UserState;
                portfolios_form.UpdatePricesInDataGrids(sec);
            }
            else if (e.UserState is Position)
            {
                Position pos = (Position)e.UserState;
                portfolios_form.UpdatePositionInDataGrids(pos);
            }
            else
            {
                //issue with data type in queue
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //nothing
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (false == backgroundWorker.CancellationPending)
            {

                if (false == DataSource.RTUpdatesQueue.IsEmpty)
                {
                    Object obj = new Object();
                    if (DataSource.RTUpdatesQueue.TryDequeue(out obj))
                    {
                        BackgroundWorker worker = (BackgroundWorker)sender;
                        worker.ReportProgress(0, obj);
                    }
                }
            }
            e.Result = "cancelled";
        }
    }
}
