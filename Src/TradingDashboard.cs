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
                    Currency = "USD",
                    UnderlyingType = values[17],
                    UnderlyingTicker = values[2],
                    Underlying = values[2],
                        RealizedPnL = Convert.ToDouble(values[4]),
                        BODPnL = Convert.ToDouble(values[5]),
                        BeginOfDayQuantity = Convert.ToDouble(values[11]),
                        BoughtQuantity = Convert.ToDouble(values[12]),
                        BoughtAveragePrice = Convert.ToDouble(values[13]),
                        SoldQuantity = Convert.ToDouble(values[14]),
                        SoldAveragePrice = Convert.ToDouble(values[15])
                    };
                Security sec = Security.GetNewSecurityFromOMSPosition(values[2], values[2], values[17]);
                  
                    DataSource.RTUpdatesQueue.Enqueue(pos);
                   // DataSource.WatchedTickers.Enqueue(sec);
                   
            }
         
        }

       private void UpdatePriceInDataSet(PriceUpdate update_price)
        {
            
        }
        private void UpdatePositionInDataSet(Position updated_position)
        {
            var pos_rows_to_update = TradingDashboard.the_data_set.Tables["positions_table"].AsEnumerable().Where(
               row =>
               row.Field<string>("Security Name") == updated_position.Underlying
               &&
               row.Field<string>("Portfolio") == updated_position.PortfolioName);
            if (pos_rows_to_update.Count<DataRow>() == 1)
            {

                pos_rows_to_update.ElementAt(0).SetField("Portfolio", updated_position.PortfolioName);
                pos_rows_to_update.ElementAt(0).SetField("Security Name", updated_position.Underlying);
                pos_rows_to_update.ElementAt(0).SetField("BOD PnL", updated_position.BODPnL);
                pos_rows_to_update.ElementAt(0).SetField("Realized PnL", updated_position.RealizedPnL);
                pos_rows_to_update.ElementAt(0).SetField("BOD Position", updated_position.BeginOfDayQuantity);
                pos_rows_to_update.ElementAt(0).SetField("Bought Quantity", updated_position.BoughtQuantity);
                pos_rows_to_update.ElementAt(0).SetField("Average Bought Price", updated_position.BoughtAveragePrice);
                pos_rows_to_update.ElementAt(0).SetField("Sold Quantity", updated_position.SoldQuantity);
                pos_rows_to_update.ElementAt(0).SetField("Average Sold Price", updated_position.SoldAveragePrice);
                TradingDashboard.the_data_set.Tables["positions_table"].AcceptChanges();

            }
            else
            {
                TradingDashboard.the_data_set.Tables["positions_table"].Rows.Add(updated_position.PortfolioName,
                    updated_position.Underlying, updated_position.BODPnL, updated_position.RealizedPnL,
                    updated_position.BoughtQuantity, updated_position.BoughtAveragePrice, updated_position.SoldQuantity, updated_position.SoldAveragePrice);
                TradingDashboard.the_data_set.Tables["positions_table"].AcceptChanges();
                var sec_rows_to_update = TradingDashboard.the_data_set.Tables["securities_table"].AsEnumerable().Where(
                row =>
                row.Field<string>("Security Name") == updated_position.Underlying
                );

                bool new_security = true;
                try
                {

                    TradingDashboard.the_data_set.Tables["securities_table"].Rows.Add(0, 0, 1, 1, updated_position.UnderlyingType, "", updated_position.Underlying,
                        updated_position.UnderlyingTicker, "Unknown Sector", "Unknown Country");
                    TradingDashboard.the_data_set.Tables["securities_table"].AcceptChanges();
                }
                catch (ConstraintException ex)
                {
                    new_security = false;
                    //this is ignored as it is normal to violate here 
                }
                if (new_security)//get static data
                    DataSource.WatchedTickers.Enqueue(Security.GetNewSecurityFromOMSPosition(updated_position.UnderlyingTicker, updated_position.UnderlyingTicker,updated_position.UnderlyingType));
            }
        }
        private void UpdateStaticDataInDataSet(Security updated_security)
        {
            if (updated_security.Currency != "USD")
            {
                Security forex_sec = Security.CreateForexSecurity("USD", updated_security.Currency);
                bool new_security = true;
                try
                {

                    TradingDashboard.the_data_set.Tables["securities_table"].Rows.Add(0, 0, 1, 1, forex_sec.Name, "", forex_sec.Name,
                       forex_sec.BloombergTicker, "Unknown Sector", "Unknown Country");
                    TradingDashboard.the_data_set.Tables["securities_table"].AcceptChanges();
                }
                catch (ConstraintException ex)
                {
                    new_security = false;
                    //this is ignored as it is normal to violate here 
                }
                if (new_security)//get static data
                    DataSource.WatchedTickers.Enqueue(forex_sec);
            }
          
        }
        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is PriceUpdate)
            {
                PriceUpdate price = (PriceUpdate)e.UserState;
                portfolios_form.UpdatePricesInDataGrids(price);
            }
            else if (e.UserState is Position)
            {
                Position pos = (Position)e.UserState;
                portfolios_form.UpdatePositionInDataGrids(pos);
                UpdatePositionInDataSet(pos);
            }
            else 
            if(e.UserState is Security)
            {
                Security sec = (Security)e.UserState;
                portfolios_form.UpdateStaticDataInDataGrids(sec);
                UpdateStaticDataInDataSet(sec);
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
