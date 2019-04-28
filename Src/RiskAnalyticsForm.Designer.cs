using System.Data;
using System.Windows.Forms;
namespace Dashboard
{
    partial class RiskAnalyticsForm
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
            
        }
        public  void ExplicitDispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            
        }
        private void CreatGridView(System.Collections.Generic.Dictionary<string, Portfolio> portfolios)
        {
            
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

        
            foreach (var ptf in portfolios)
            {
  
                data_source_aggregate_folios.Rows.Add(ptf.Key, null, null, null, null, null, null, null, null, null, null);

            }
            dataGridView_Portfolios.DataSource = data_source_aggregate_folios;
        }
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView_Portfolios = new System.Windows.Forms.DataGridView();
            this.config_columns = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Portfolios)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_Portfolios
            // 
            this.dataGridView_Portfolios.AllowUserToAddRows = false;
            this.dataGridView_Portfolios.AllowUserToDeleteRows = false;
            this.dataGridView_Portfolios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Portfolios.Location = new System.Drawing.Point(12, 36);
            this.dataGridView_Portfolios.Name = "dataGridView_Portfolios";
            this.dataGridView_Portfolios.ReadOnly = true;
            this.dataGridView_Portfolios.Size = new System.Drawing.Size(1166, 287);
            this.dataGridView_Portfolios.TabIndex = 5;
            // 
            // config_columns
            // 
            this.config_columns.Location = new System.Drawing.Point(489, 9);
            this.config_columns.Name = "config_columns";
            this.config_columns.Size = new System.Drawing.Size(93, 23);
            this.config_columns.TabIndex = 6;
            this.config_columns.Text = "config. columns";
            this.config_columns.UseVisualStyleBackColor = true;
            this.config_columns.Click += new System.EventHandler(this.Config_columns_Click);
            // 
            // RiskAnalyticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1190, 333);
            this.Controls.Add(this.config_columns);
            this.Controls.Add(this.dataGridView_Portfolios);
            this.Name = "RiskAnalyticsForm";
            this.Text = "Risk Analytics";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RiskAnalyticsForm_FormClosed);
            this.Shown += new System.EventHandler(this.RiskAnalyticsForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Portfolios)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_Portfolios;
        private Button config_columns;
    }
}