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
       
        private PortfoliosForm portfolios_form;
        private RiskAnalyticsForm risk_analytics_form;
        private MktDataService mktDataService;

        public TradingDashboard()
        {
            InitializeComponent();
            // creer une instance de MktDataSubscription
            // run it

            MktDataService.startBloomberg();

        }

        private void ShowPortfoliosForm(object sender, EventArgs e)
        {
            if (PortfoliosForm.InstancesCount() < 1)
            {
                portfolios_form = new PortfoliosForm();
                portfolios_form.MdiParent = this;
                portfolios_form.Text = "Portfolios";
                portfolios_form.Show();
            }
            else
                portfolios_form.WindowState = FormWindowState.Normal;
        }

        private void ShowRiskAnalyticsForm(object sender, EventArgs e)
        {
            if (RiskAnalyticsForm.InstancesCount() < 1)
            {
                risk_analytics_form = new RiskAnalyticsForm();
                risk_analytics_form.MdiParent = this;
                risk_analytics_form.Text = "Risk Analytics";
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
    }
}
