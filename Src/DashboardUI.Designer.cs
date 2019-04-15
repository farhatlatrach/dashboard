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
        }

        private void AddTabs(System.Collections.Generic.Dictionary<string, Portfolio> portfolios)
        {
            tabControl_portfolios.Controls.Clear();

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
                
                dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dataGridView.Location = new System.Drawing.Point(4, 4);
                dataGridView.Name = "dataGridView";
                dataGridView.Size = new System.Drawing.Size(907, 317);
                dataGridView.TabIndex = i;

               
                DataTable data_source = new DataTable();
                data_source.Columns.Add("Security");
                
                data_source.Columns.Add("Buy Quantity");
                data_source.Columns.Add("Buy Average Price");

                data_source.Columns.Add("Sell Quantity");
                data_source.Columns.Add("Sell Average Price");
                
                data_source.Columns.Add("Bid");
                data_source.Columns.Add("Ask");
                foreach (var pos in ptf.Value.Positions)
                {
                    data_source.Rows.Add(pos.Key,
                        pos.Value.BuyQuantity,
                        pos.Value.BuyAveragePrice, 
                        pos.Value.SellQuantity, 
                        pos.Value.SellAveragePrice,  
                        pos.Value.Underlying.Bid, pos.Value.Underlying.Ask);
                }
                dataGridView.DataSource = data_source;
                tab.Controls.Add(dataGridView);
                tabControl_portfolios.Controls.Add(tab);
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
                            if((string)row.Cells["Security"].Value == sec.Name)
                            {
                                row.Cells["Bid"].Value = sec.Bid;
                                row.Cells["Ask"].Value = sec.Ask;
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
                            if ((string)row.Cells["Security"].Value == pos.Underlying.Name)
                            {
                                row.Cells["Sell Average Price"].Value = pos.SellAveragePrice;
                                row.Cells["Buy Average Price"].Value = pos.BuyAveragePrice;
                                row.Cells["Sell Quantity"].Value = pos.SellQuantity;
                                row.Cells["Buy Quantity"].Value = pos.BuyQuantity;
                                found = true;
                                break;
                            }
                        }
                        if(false == found 
                            && (string)tab.Name == pos.PortfolioName)
                        {
                            DataTable data_source = (DataTable)view.DataSource;
                            data_source.Rows.Add(pos.Underlying.Name,
                        pos.BuyQuantity,
                        pos.BuyAveragePrice,
                        pos.SellQuantity,
                        pos.SellAveragePrice,
                        pos.Underlying.Bid, pos.Underlying.Ask);
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
            this.button_load_books = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.button_start_rt = new System.Windows.Forms.Button();
            this.button_cancel_RT = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl_portfolios);
            this.groupBox1.Location = new System.Drawing.Point(31, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(934, 372);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Portfolios";
            // 
            // tabControl_portfolios
            // 
            this.tabControl_portfolios.Location = new System.Drawing.Point(18, 20);
            this.tabControl_portfolios.Name = "tabControl_portfolios";
            this.tabControl_portfolios.SelectedIndex = 0;
            this.tabControl_portfolios.Size = new System.Drawing.Size(910, 346);
            this.tabControl_portfolios.TabIndex = 0;
            // 
            // button_load_books
            // 
            this.button_load_books.Location = new System.Drawing.Point(105, 29);
            this.button_load_books.Name = "button_load_books";
            this.button_load_books.Size = new System.Drawing.Size(118, 23);
            this.button_load_books.TabIndex = 1;
            this.button_load_books.Text = "Load books";
            this.button_load_books.UseVisualStyleBackColor = true;
            this.button_load_books.Click += new System.EventHandler(this.Button_load_books_Click);
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
            this.button_start_rt.Location = new System.Drawing.Point(335, 28);
            this.button_start_rt.Name = "button_start_rt";
            this.button_start_rt.Size = new System.Drawing.Size(108, 23);
            this.button_start_rt.TabIndex = 2;
            this.button_start_rt.Text = "Start RT";
            this.button_start_rt.UseVisualStyleBackColor = true;
            this.button_start_rt.Click += new System.EventHandler(this.Start_rt_Click);
            // 
            // button_cancel_RT
            // 
            this.button_cancel_RT.Location = new System.Drawing.Point(500, 32);
            this.button_cancel_RT.Name = "button_cancel_RT";
            this.button_cancel_RT.Size = new System.Drawing.Size(114, 20);
            this.button_cancel_RT.TabIndex = 3;
            this.button_cancel_RT.Text = "Cancel RT";
            this.button_cancel_RT.UseVisualStyleBackColor = true;
            this.button_cancel_RT.Click += new System.EventHandler(this.Button_cancel_RT_Click);
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 649);
            this.Controls.Add(this.button_cancel_RT);
            this.Controls.Add(this.button_start_rt);
            this.Controls.Add(this.button_load_books);
            this.Controls.Add(this.groupBox1);
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_load_books;
        private System.Windows.Forms.TabControl tabControl_portfolios;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private Button button_start_rt;
        private Button button_cancel_RT;
    }
}

