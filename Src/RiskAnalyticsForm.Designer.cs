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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            instances--;
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Portfolios)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_Portfolios
            // 
            this.dataGridView_Portfolios.AllowUserToAddRows = false;
            this.dataGridView_Portfolios.AllowUserToDeleteRows = false;
            this.dataGridView_Portfolios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Portfolios.Location = new System.Drawing.Point(12, 12);
            this.dataGridView_Portfolios.Name = "dataGridView_Portfolios";
            this.dataGridView_Portfolios.ReadOnly = true;
            this.dataGridView_Portfolios.Size = new System.Drawing.Size(1166, 311);
            this.dataGridView_Portfolios.TabIndex = 5;
            // 
            // RiskAnalyticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1190, 333);
            this.Controls.Add(this.dataGridView_Portfolios);
            this.Name = "RiskAnalyticsForm";
            this.Text = "Risk Analytics";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Portfolios)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_Portfolios;
    }
}