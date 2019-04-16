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
    public partial class RiskAnalyticsForm : Form
    {
        private static int instances = 0;
        public static int InstancesCount()
        {
            return instances;
        }
        public RiskAnalyticsForm()
        {
            InitializeComponent();
            CreatGridView(Model.Instance.Portfolios);
            instances++;
        }
    }
}
