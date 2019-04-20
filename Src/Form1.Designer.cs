namespace Dashboard
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.add_column = new System.Windows.Forms.Button();
            this.hide_column = new System.Windows.Forms.Button();
            this.move_up = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(24, 31);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(161, 478);
            this.dataGridView1.TabIndex = 0;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(283, 31);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(165, 478);
            this.dataGridView2.TabIndex = 1;
            // 
            // add_column
            // 
            this.add_column.Location = new System.Drawing.Point(193, 190);
            this.add_column.Name = "add_column";
            this.add_column.Size = new System.Drawing.Size(75, 23);
            this.add_column.TabIndex = 2;
            this.add_column.Text = "Add ==>";
            this.add_column.UseVisualStyleBackColor = true;
            // 
            // hide_column
            // 
            this.hide_column.Location = new System.Drawing.Point(193, 227);
            this.hide_column.Name = "hide_column";
            this.hide_column.Size = new System.Drawing.Size(75, 23);
            this.hide_column.TabIndex = 3;
            this.hide_column.Text = "<== Hide";
            this.hide_column.UseVisualStyleBackColor = true;
            // 
            // move_up
            // 
            this.move_up.Location = new System.Drawing.Point(467, 176);
            this.move_up.Name = "move_up";
            this.move_up.Size = new System.Drawing.Size(53, 26);
            this.move_up.TabIndex = 4;
            this.move_up.Text = "UP";
            this.move_up.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(467, 217);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(53, 24);
            this.button2.TabIndex = 5;
            this.button2.Text = "DOWN";
            this.button2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 540);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.move_up);
            this.Controls.Add(this.hide_column);
            this.Controls.Add(this.add_column);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button add_column;
        private System.Windows.Forms.Button hide_column;
        private System.Windows.Forms.Button move_up;
        private System.Windows.Forms.Button button2;
    }
}