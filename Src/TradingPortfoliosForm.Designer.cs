using System.Data;
using System.Windows.Forms;

namespace Dashboard
{
    partial class Dashboard
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            instances--;
        }

        private void AddTabs(System.Collections.Generic.Dictionary<string, Portfolio> portfolios)
        {
            tabControl_portfolios.Controls.Clear();
            DataTable data_source_aggregate_folios = new DataTable();
            data_source_aggregate_folios.Columns.Add("Portfolio");
            data_source_aggregate_folios.Columns.Add("Today PnL");
            data_source_aggregate_folios.Columns.Add("Net");
            data_source_aggregate_folios.Columns.Add("Long");
            data_source_aggregate_folios.Columns.Add("Short");
            
            data_source_aggregate_folios.Columns.Add("BOD PnL");
            data_source_aggregate_folios.Columns.Add("Trading PnL");
            data_source_aggregate_folios.Columns.Add("Div PnL");
            data_source_aggregate_folios.Columns.Add("YTD PnL");
            data_source_aggregate_folios.Columns.Add("MTD PnL");
            data_source_aggregate_folios.Columns.Add("WTD PnL");
            data_source_aggregate_folios.Rows.Add("TOTAL", null, null, null, null, null, null, null, null, null, null);

            int i = 0;
            foreach (var ptf in portfolios)
            {
                TabPage tab = new TabPage();
                tab.Location = new System.Drawing.Point(18, 20);
                tab.Name =ptf.Key;
                tab.Padding = new System.Windows.Forms.Padding(3);
                tab.Size = new System.Drawing.Size(914,321);
                tab.TabIndex = i;
                tab.Text = "   " + ptf.Key +"   ";
                tab.UseVisualStyleBackColor = true;
                // data grid
                DataGridView dataGridView = new DataGridView();

                dataGridView.AllowUserToAddRows = false;
                dataGridView.AllowUserToDeleteRows = false;
                dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dataGridView.Location = new System.Drawing.Point(4, 4);
                dataGridView.Name = "dataGridView";
                dataGridView.ReadOnly = true;
                dataGridView.Size = new System.Drawing.Size(1110, 140);
                dataGridView.TabIndex = i;

               
                DataTable data_source = new DataTable();
                data_source.Columns.Add("Ticker");
                data_source.Columns.Add("Delta");
                data_source.Columns.Add("Today PnL");
                data_source.Columns.Add("BOD PnL");
                data_source.Columns.Add("Trading PnL");
                data_source.Columns.Add("Div PnL");
                data_source.Columns.Add("Last Price");
                data_source.Columns.Add("Previous Close");
                data_source.Columns.Add("Position");
                data_source.Columns.Add("BOD Position");
                data_source.Columns.Add("Bought Quantity");
                data_source.Columns.Add("Average Bought Price");
                data_source.Columns.Add("Sold Quantity");
                data_source.Columns.Add("Average Sold Price");
                data_source.Columns.Add("Multiplier");
                data_source.Columns.Add("Security Type");
                data_source.Columns.Add("YTD PnL");
                data_source.Columns.Add("MTD PnL");
                data_source.Columns.Add("WTD PnL");
                foreach (var pos in ptf.Value.Positions)
                {
                    data_source.Rows.Add(pos.Key,
                        null, null, null, null, null, pos.Value.Underlying.Last, pos.Value.Underlying.PreviousClose, null, pos.Value.BeginOfDayQuantity,
                        pos.Value.BoughtQuantity, pos.Value.BoughtAveragePrice, pos.Value.SoldQuantity, pos.Value.SoldAveragePrice, 1,
                        pos.Value.Underlying is Future ? "FUTURE" : "EQUITY", null, null, null);
                        
                }
                dataGridView.DataSource = data_source;
                tab.Controls.Add(dataGridView);
                tabControl_portfolios.Controls.Add(tab);

                data_source_aggregate_folios.Rows.Add(ptf.Key, null, null, null, null, null, null, null, null, null, null);
               
                

                i++;
            }
            
        }
        private void UpdatePricesInDataGrids(Security sec)
        {
            foreach(TabPage tab in tabControl_portfolios.Controls)
            {
                foreach (Control control in tab.Controls)
                {
                    if (control.Name == "dataGridView")
                    {
                        DataGridView view = (DataGridView)control;
                        foreach(DataGridViewRow row in view.Rows)
                        {
                            if((string)row.Cells["Ticker"].Value == sec.Name)
                            {
                                row.Cells["Last Price"].Value = sec.Last;
                                
                            }
                        }
                        this.BindingContext[view.DataSource].EndCurrentEdit();

                    }

                }
            }

        }
        private void UpdatePositionInDataGrids(Position pos)
        {
            foreach (TabPage tab in tabControl_portfolios.Controls)
            {
                foreach (Control control in tab.Controls)
                {
                    if (control.Name == "dataGridView")
                    {
                        DataGridView view = (DataGridView)control;
                        bool found = false;
                        foreach (DataGridViewRow row in view.Rows)
                        {
                            if ((string)row.Cells["Ticker"].Value == pos.Underlying.Name)
                            {
                                row.Cells["Average Sold Price"].Value = pos.SoldAveragePrice;
                                row.Cells["Average Bought Price"].Value = pos.BoughtAveragePrice;
                                row.Cells["Sold Quantity"].Value = pos.SoldQuantity;
                                row.Cells["Bought Quantity"].Value = pos.BoughtQuantity;

                                
                                found = true;
                                break;
                            }
                        }
                        if(false == found 
                            && (string)tab.Name == pos.PortfolioName)
                        {
                            DataTable data_source = (DataTable)view.DataSource;
                            
                            data_source.Rows.Add(pos.Underlying.Name,
                        null, null, null, null, null, pos.Underlying.Last, pos.Underlying.PreviousClose, null, pos.BeginOfDayQuantity,
                        pos.BoughtQuantity, pos.BoughtAveragePrice, pos.SoldQuantity, pos.SoldAveragePrice, 1,
                        pos.Underlying is Future ? "FUTURE" : "EQUITY", null, null, null);

                            view.DataSource = data_source;
                        }

                        this.BindingContext[view.DataSource].EndCurrentEdit();

                    }

                }
            }
        }
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl_portfolios = new System.Windows.Forms.TabControl();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.button_start_rt = new System.Windows.Forms.Button();
            this.button_cancel_RT = new System.Windows.Forms.Button();
            this.data_source = new System.Data.DataTable();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.data_source)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl_portfolios);
            this.groupBox1.Location = new System.Drawing.Point(12, 78);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1124, 551);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Portfolios";
            // 
            // tabControl_portfolios
            // 
            this.tabControl_portfolios.Location = new System.Drawing.Point(6, 20);
            this.tabControl_portfolios.Name = "tabControl_portfolios";
            this.tabControl_portfolios.SelectedIndex = 0;
            this.tabControl_portfolios.Size = new System.Drawing.Size(1112, 346);
            this.tabControl_portfolios.TabIndex = 0;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
            // 
            // button_start_rt
            // 
            this.button_start_rt.Location = new System.Drawing.Point(1003, 12);
            this.button_start_rt.Name = "button_start_rt";
            this.button_start_rt.Size = new System.Drawing.Size(108, 23);
            this.button_start_rt.TabIndex = 2;
            this.button_start_rt.Text = "Start RT";
            this.button_start_rt.UseVisualStyleBackColor = true;
            this.button_start_rt.Click += new System.EventHandler(this.Start_rt_Click);
            // 
            // button_cancel_RT
            // 
            this.button_cancel_RT.Location = new System.Drawing.Point(999, 52);
            this.button_cancel_RT.Name = "button_cancel_RT";
            this.button_cancel_RT.Size = new System.Drawing.Size(114, 20);
            this.button_cancel_RT.TabIndex = 3;
            this.button_cancel_RT.Text = "Stop RT";
            this.button_cancel_RT.UseVisualStyleBackColor = true;
            this.button_cancel_RT.Click += new System.EventHandler(this.Button_cancel_RT_Click);
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1245, 649);
            this.Controls.Add(this.button_cancel_RT);
            this.Controls.Add(this.button_start_rt);
            this.Controls.Add(this.groupBox1);
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.data_source)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl_portfolios;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private Button button_start_rt;
        private Button button_cancel_RT;
        
        private DataTable data_source;
    }
}

