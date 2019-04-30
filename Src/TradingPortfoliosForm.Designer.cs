using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Dashboard
{
    partial class PortfoliosForm
    {
       
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
           //call explicit dispose
           
        }
        public  void ExplicitDispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);

        }
        private DataTable NewPositionViewDataTable( )
        {
            
            DataTable data_table = new DataTable();
            
            data_table.Columns.Add("Security Name", typeof(string));
            data_table.Columns.Add("Today PnL", typeof(double));
            data_table.Columns.Add("BOD PnL", typeof(double));
            data_table.Columns.Add("Trading PnL", typeof(double));
            data_table.Columns.Add("Div PnL", typeof(double));
            data_table.Columns.Add("Last Price", typeof(double));
            data_table.Columns.Add("Previous Close", typeof(double));
            data_table.Columns.Add("BOD Position", typeof(double));
            data_table.Columns.Add("Bought Quantity", typeof(double));
            data_table.Columns.Add("Average Bought Price", typeof(double));
            data_table.Columns.Add("Sold Quantity", typeof(double));
            data_table.Columns.Add("Average Sold Price", typeof(double));
            data_table.Columns.Add("Position", typeof(double), "`Bought Quantity` -  `Sold Quantity` ");
            data_table.Columns.Add("Forex Rate", typeof(double));
            data_table.Columns.Add("Delta", typeof(double), "`Last Price` * `Position` * `Forex Rate`");
           
            
            data_table.Columns.Add("Multiplier", typeof(double));
            data_table.Columns.Add("Security Type", typeof(string));
            data_table.Columns.Add("YTD PnL", typeof(double));
            data_table.Columns.Add("MTD PnL", typeof(double));
            data_table.Columns.Add("WTD PnL", typeof(double));

            data_table.Columns.Add("Security Quotation Factor", typeof(double));
            data_table.Columns.Add("Portfolio Currency", typeof(string));
            data_table.Columns.Add("Security Currency", typeof(string));
            data_table.Columns.Add("Forex", typeof(string));//pair = Portfolio ccy/security ccy
            
            data_table.Columns.Add("Security Sector", typeof(string));
            data_table.Columns.Add("Security Country", typeof(string));

            return data_table;
        }
        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView the_view = (DataGridView)sender;
           
            if (the_view.Columns[e.ColumnIndex].Name == "Delta")
            {
                if (e.Value != null)
                {
                    if ((double)e.Value > 0)
                        e.CellStyle.ForeColor = System.Drawing.Color.MediumVioletRed;
                    else
                        e.CellStyle.ForeColor = System.Drawing.Color.Green;
                   
                }
            }
            else if (the_view.Columns[e.ColumnIndex].Name == "Trading PnL")
            {
                if (e.Value != null)
                {
                    
                    if ((double)e.Value > 0)
                        e.CellStyle.ForeColor = System.Drawing.Color.MediumVioletRed;
                    else
                        e.CellStyle.ForeColor = System.Drawing.Color.Green;
                    
                }
            }
            else if (the_view.Columns[e.ColumnIndex].Name == "BOD PnL")
            {
                if (e.Value != null)
                {

                    if ((double)e.Value > 0)
                        e.CellStyle.ForeColor = System.Drawing.Color.MediumVioletRed;
                    else
                        e.CellStyle.ForeColor = System.Drawing.Color.Green;

                }
            }
            else if (e.Value != null)
            {
                e.CellStyle.ForeColor = System.Drawing.Color.Yellow;
            }
        }


        private void AddPortfolioTab(string folio_name)
        {
            
            int tab_index = tabControl_portfolios.Controls.Count;
           
            
            
                TabPage tab = new TabPage();
                tab.Location = new System.Drawing.Point(18, 20);
                tab.Name = folio_name;
                tab.Padding = new System.Windows.Forms.Padding(3);
                tab.Size = new System.Drawing.Size(1260,600);
                tab.TabIndex = tab_index;
                tab.Text = "   " + folio_name + "   (USD)";
                tab.UseVisualStyleBackColor = true;
                // data grid
                DataGridView dataGridView = new DataGridView();

                dataGridView.AllowUserToAddRows = false;
                dataGridView.AllowUserToDeleteRows = false;
               
                dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dataGridView.Location = new System.Drawing.Point(20, 22);
                dataGridView.Name = "dataGridView";
                dataGridView.ReadOnly = true;
                dataGridView.Size = new System.Drawing.Size(1210, 550);
                dataGridView.TabIndex = tab_index;
            dataGridView.CellFormatting +=
          new System.Windows.Forms.DataGridViewCellFormattingEventHandler(
          this.dataGridView_CellFormatting);

            dataGridView.RowsDefaultCellStyle.BackColor = System.Drawing.Color.Black;
            
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.DarkGray;
                
            DataTable data_table = NewPositionViewDataTable( );
            dataGridView.DataSource = data_table;
            tab.Controls.Add(dataGridView);
                this.tabControl_portfolios.Controls.Add(tab);

            
            
        }
        public void UpdatePricesInDataGrids(PriceUpdate price)
        {
            //var sec_row_to_update = TradingDashboard.the_data_set.Tables["securities_table"].AsEnumerable().Where(
            //   row =>
            //   row.Field<string>("Security Name") == sec.Name
            //   );
            //if (sec_row_to_update.Count<DataRow>() == 1)
            //{
               
            //    sec_row_to_update.ElementAt(0).SetField("LastPrice", price.Last);
            //    sec_row_to_update.ElementAt(0).SetField("PreviousClose", sec.PreviousClose);
                
               
            //    sec_row_to_update.ElementAt(0).SetField("Security Type", sec.SecurityType);
            //    sec_row_to_update.ElementAt(0).SetField("Currency", sec.Currency);
                
            //    sec_row_to_update.ElementAt(0).SetField("Sector", sec.Sector);
            //    sec_row_to_update.ElementAt(0).SetField("Country", sec.Country);
            //    TradingDashboard.the_data_set.Tables["securities_table"].AcceptChanges();

            //}//Securities is first inserted from position so always update
        

            foreach (TabPage tab in tabControl_portfolios.Controls)
            {
                string folio_name = tab.Name;
                    foreach (Control control in tab.Controls)
                    {
                        if (control.Name == "dataGridView")
                        {
                            DataGridView view = (DataGridView)control;
                         //   LoadFolioPositionsData(view, folio_name);
                        
                        foreach (DataGridViewRow row in view.Rows)
                        {
                            if ((string)row.Cells["Security Name"].Value.ToString() == price.Name)
                            {
                                row.Cells["Last Price"].Value = price.Price;

                            }else if ((string)row.Cells["Forex"].Value.ToString() == price.Name)
                            {
                                row.Cells["Forex Rate"].Value = price.Price;

                            }
                        }
                        this.BindingContext[view.DataSource].EndCurrentEdit();

                    }
                    }
            }
           

        }
        public void UpdateStaticDataInDataGrids(Security sec)
        {


           

            foreach (TabPage tab in tabControl_portfolios.Controls)
            {
             
                {
                    foreach (Control control in tab.Controls)
                    {
                        if (control.Name == "dataGridView")
                        {
                            DataGridView view = (DataGridView)control;
                            
                            foreach (DataGridViewRow row in view.Rows)
                            {
                               
                                if ((string)row.Cells["Security Name"].Value == sec.Name)
                                {
                                    row.Cells["Security Quotation Factor"].Value = sec.QuotationFactor;
                                    row.Cells["Portfolio Currency"].Value = "USD";
                                    row.Cells["Security Currency"].Value = sec.Currency;
                                    row.Cells["Security Country"].Value = sec.Country;
                                    row.Cells["Security Sector"].Value = sec.Sector;
                                    row.Cells["Forex"].Value = sec.Currency+ "/USD";
                                    if(sec.Currency=="USD")
                                        row.Cells["Forex Rate"].Value = 1;
                                }
                            }
                            this.BindingContext[view.DataSource].EndCurrentEdit();
                        }
                    }
                }
            }
           
        }
        public void UpdatePositionInDataGrids(Position pos)
        {
           

            bool found_folio = false;
           
            foreach (TabPage tab in tabControl_portfolios.Controls)
            {
                if (tab.Name == pos.PortfolioName)
                    found_folio = true;
                if(found_folio)
                {
                        foreach (Control control in tab.Controls)
                        {
                            if (control.Name == "dataGridView")
                            {
                                DataGridView view = (DataGridView)control;
                            bool found = false;
                            foreach (DataGridViewRow row in view.Rows)
                            {
                                if ((string)row.Cells["Security Name"].Value.ToString() == pos.Underlying)
                                {
                                    row.Cells["Average Sold Price"].Value = pos.SoldAveragePrice;
                                    row.Cells["Average Bought Price"].Value = pos.BoughtAveragePrice;
                                    row.Cells["Sold Quantity"].Value = pos.SoldQuantity;
                                    row.Cells["Bought Quantity"].Value = pos.BoughtQuantity;


                                    found = true;
                                    break;
                                }
                            }
                            if (false == found
                                && (string)tab.Name == pos.PortfolioName)
                            {
                                DataTable data_source = (DataTable)view.DataSource;

                                data_source.Rows.Add(pos.Underlying,
                            0, pos.BODPnL, 0, 0, 0, 0, pos.BeginOfDayQuantity,
                            pos.BoughtQuantity, pos.BoughtAveragePrice, pos.SoldQuantity, pos.SoldAveragePrice, 0, 0, 0,  1,
                            pos.UnderlyingType, 0, 0, 0);

                                view.DataSource = data_source;
                            }
                            this.BindingContext[view.DataSource].EndCurrentEdit();
                        }
                        }
                }
            }
            if (false==found_folio)
            {
                AddPortfolioTab(pos.PortfolioName);
                UpdatePositionInDataGrids(pos);
            }
        }
   
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.config_columns = new System.Windows.Forms.Button();
            this.tabControl_portfolios = new System.Windows.Forms.TabControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // config_columns
            // 
            this.config_columns.Location = new System.Drawing.Point(361, 33);
            this.config_columns.Name = "config_columns";
            this.config_columns.Size = new System.Drawing.Size(90, 19);
            this.config_columns.TabIndex = 4;
            this.config_columns.Text = "config. columns";
            this.config_columns.UseVisualStyleBackColor = true;
            this.config_columns.Click += new System.EventHandler(this.Config_columns_Click);
            // 
            // tabControl_portfolios
            // 
            this.tabControl_portfolios.Location = new System.Drawing.Point(6, 20);
            this.tabControl_portfolios.Name = "tabControl_portfolios";
            this.tabControl_portfolios.SelectedIndex = 0;
            this.tabControl_portfolios.Size = new System.Drawing.Size(1400, 700);
            this.tabControl_portfolios.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl_portfolios);
            this.groupBox1.Location = new System.Drawing.Point(12, 78);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1420, 720);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Portfolios";
            // 
            // PortfoliosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1245, 649);
            this.Controls.Add(this.config_columns);
            this.Controls.Add(this.groupBox1);
            this.Name = "PortfoliosForm";
            this.Text = "Dashboard";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PortfoliosForm_FormClosed);
            this.Shown += new System.EventHandler(this.PortfoliosForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        
        private DataSet data_set_;
        private Button config_columns;
        private TabControl tabControl_portfolios;
        private GroupBox groupBox1;
    }
}

