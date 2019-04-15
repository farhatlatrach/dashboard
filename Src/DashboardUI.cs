﻿using System;
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
    public partial class Dashboard : Form
    {
        private Model model_ = new Model();
        private DataSource datasource_ = new DataSource();
        public Dashboard()
        {
            InitializeComponent();
            button_cancel_RT.Enabled = false;
            button_start_rt.Enabled = false;
            datasource_.StartRTWatch();
        }
        private void Button_load_books_Click(object sender, EventArgs e)
        {
            model_.LoadModel();
            AddTabs(model_.Portfolios);
            button_start_rt.Enabled = true;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(false == backgroundWorker.CancellationPending)
            {
                if(false == DataSource.RTUpdatesQueue.IsEmpty)
                {
                    Object obj = new Object();
                    if(DataSource.RTUpdatesQueue.TryDequeue(out obj))
                    {
                        BackgroundWorker worker = (BackgroundWorker)sender;
                        worker.ReportProgress(0,obj);
                    }
                }
            }
            e.Result = "cancelled";
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(e.UserState is Security)
            {
                Security sec = (Security)e.UserState;
                UpdatePricesInDataGrids(sec);
            }
            else if(e.UserState is Position)
            {
                Position pos = (Position)e.UserState;
                UpdatePositionInDataGrids(pos);
            }else
            {
                //issue with data type in queue
            }
        }
        private void Start_rt_Click(object sender, EventArgs e)
        {
            if(false == backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync();
                button_start_rt.Enabled = false;
                button_cancel_RT.Enabled = true;
            }
        }

        private void Button_cancel_RT_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();
            button_cancel_RT.Enabled = false;
            button_start_rt.Enabled = true;
        }
    }
}