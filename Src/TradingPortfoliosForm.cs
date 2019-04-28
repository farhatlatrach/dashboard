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
       
        
       
        private void LoadFolioPositionsData(DataGridView dataGridView,string folio_name)
        {

            DataTable positions = TradingDashboard.the_data_set.Tables["positions_table"];
            DataTable securities = TradingDashboard.the_data_set.Tables["securities_table"];


            DataTable data_table = NewPositionViewDataTable(dataGridView);
            
            var query_folio_positions = from position_info in positions.AsEnumerable()
                                        from security_info in securities.AsEnumerable()
                                        where position_info.Field<string>("Portfolio") ==folio_name
                                            && position_info.Field<string>("Security Name") == security_info.Field<string>("Security Name")
                                        select new
                                        {
                                            sec_name = position_info.Field<string>("Security Name") ,


                                           
                                            bod_pnl = position_info.Field<double?>("BOD PnL") ?? 0,
                                           
                                            last_price = security_info.Field<double>("LastPrice"),
                                            previous_close = security_info.Field<double?>("PreviousClose") ?? 0,
                                           
                                           
                                            bod_position = position_info.Field<double?>("BOD Position") ?? 0,

                                            bought_quantity = position_info.Field<double?>("Bought Quantity")??0,
                                            average_buy_price = position_info.Field<double?>("Average Bought Price") ?? 0,
                                            sold_quantity = position_info.Field<double?>("Sold Quantity") ?? 0,
                                            average_sold_price = position_info.Field<double?>("Average Sold Price") ?? 0,
                                            multiplier = security_info.Field<double?>("Multiplier") ?? 0,
                                            sec_type = security_info.Field<string>("Security Type") ,
                                           

                                        };
            foreach (var row in query_folio_positions)
            {
                DataRow data_row = data_table.NewRow();
                data_row["Security Name"] = row.sec_name;
                data_row["Today PnL"] = 0;
                data_row["BOD PnL"] = row.bod_pnl;
                data_row["Trading PnL"] = 0;
                data_row["Div PnL"] = 0;
                data_row["Last Price"] = row.last_price;
                data_row["Previous Close"] = row.previous_close;
                data_row["Delta"] = 0;
                data_row["Position"] = 0;
                data_row["BOD Position"] = row.bod_position;
                data_row["Bought Quantity"] = row.bought_quantity;
                data_row["Average Bought Price"] = row.average_buy_price;
                data_row["Sold Quantity"] = row.sold_quantity;
                data_row["Average Sold Price"] = row.average_sold_price;
                data_row["Multiplier"] = row.multiplier;
                data_row["Security Type"] = row.sec_type;
                data_row["YTD PnL"] = 0;
                data_row["MTD PnL"] = 0;
                data_row["WTD PnL"] = 0;
                data_table.Rows.Add(data_row);
            }

            //put back view as it was before (scrolling)
            int current_view_row = 0;
            int current_view_col = 0;
            int h_offset = dataGridView.HorizontalScrollingOffset;
            if (dataGridView.Rows.Count > 0 && dataGridView.FirstDisplayedCell != null)
                current_view_row = dataGridView.FirstDisplayedCell.RowIndex;

            if (dataGridView.Columns.Count > 0 && dataGridView.FirstDisplayedCell != null)
                current_view_col = dataGridView.FirstDisplayedCell.ColumnIndex;

            dataGridView.DataSource = data_table;
            
            if (current_view_row != 0 && current_view_row < dataGridView.Rows.Count)
                dataGridView.FirstDisplayedScrollingRowIndex = current_view_row;

            if (current_view_col != 0 && current_view_col < dataGridView.Columns.Count)
                dataGridView.FirstDisplayedScrollingColumnIndex = current_view_col;
            dataGridView.HorizontalScrollingOffset = h_offset;
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