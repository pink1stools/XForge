namespace XForge
{
    partial class Options
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.darkButton1 = new DarkUI.Controls.DarkButton();
            this.darkButton2 = new DarkUI.Controls.DarkButton();
            this.darkDataGridView1 = new DarkUI.Controls.DarkDataGridView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.darkDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Enabled = false;
            this.dataGridView1.Location = new System.Drawing.Point(12, 64);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(400, 307);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.Visible = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(400, 22);
            this.label1.TabIndex = 2;
            this.label1.Text = "Replace File Extensions";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(419, 48);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(125, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Show Splash Screen";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Visible = false;
            // 
            // darkButton1
            // 
            this.darkButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.darkButton1.Location = new System.Drawing.Point(12, 384);
            this.darkButton1.Name = "darkButton1";
            this.darkButton1.Padding = new System.Windows.Forms.Padding(5);
            this.darkButton1.Size = new System.Drawing.Size(130, 23);
            this.darkButton1.TabIndex = 5;
            this.darkButton1.Text = "Save Settings";
            this.darkButton1.Click += new System.EventHandler(this.button1_Click);
            // 
            // darkButton2
            // 
            this.darkButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.darkButton2.Location = new System.Drawing.Point(282, 384);
            this.darkButton2.Name = "darkButton2";
            this.darkButton2.Padding = new System.Windows.Forms.Padding(5);
            this.darkButton2.Size = new System.Drawing.Size(130, 23);
            this.darkButton2.TabIndex = 6;
            this.darkButton2.Text = "Reset File Extensions";
            this.darkButton2.Click += new System.EventHandler(this.button2_Click);
            // 
            // darkDataGridView1
            // 
            this.darkDataGridView1.AllowUserToAddRows = false;
            this.darkDataGridView1.AllowUserToDeleteRows = false;
            this.darkDataGridView1.AllowUserToDragDropRows = false;
            this.darkDataGridView1.AllowUserToResizeColumns = false;
            this.darkDataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkDataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.darkDataGridView1.ColumnHeadersHeight = 4;
            this.darkDataGridView1.Location = new System.Drawing.Point(12, 41);
            this.darkDataGridView1.MultiSelect = false;
            this.darkDataGridView1.Name = "darkDataGridView1";
            this.darkDataGridView1.RowHeadersWidth = 41;
            this.darkDataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.darkDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.darkDataGridView1.Size = new System.Drawing.Size(400, 330);
            this.darkDataGridView1.TabIndex = 7;
            this.darkDataGridView1.Load += new System.EventHandler(this.darkDataGridView1_Load);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "elf.png");
            this.imageList1.Images.SetKeyName(1, "bin.png");
            this.imageList1.Images.SetKeyName(2, "dump.png");
            this.imageList1.Images.SetKeyName(3, "fpo.png");
            this.imageList1.Images.SetKeyName(4, "vpo.png");
            this.imageList1.Images.SetKeyName(5, "mnu.png");
            this.imageList1.Images.SetKeyName(6, "txt.png");
            this.imageList1.Images.SetKeyName(7, "ini.png");
            this.imageList1.Images.SetKeyName(8, "path.png");
            this.imageList1.Images.SetKeyName(9, "bmp.png");
            this.imageList1.Images.SetKeyName(10, "jpg.png");
            this.imageList1.Images.SetKeyName(11, "tga.png");
            this.imageList1.Images.SetKeyName(12, "dds.png");
            this.imageList1.Images.SetKeyName(13, "gtf.png");
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(425, 466);
            this.Controls.Add(this.darkDataGridView1);
            this.Controls.Add(this.darkButton2);
            this.Controls.Add(this.darkButton1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Options_FormClosing);
            this.Controls.SetChildIndex(this.dataGridView1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.checkBox1, 0);
            this.Controls.SetChildIndex(this.darkButton1, 0);
            this.Controls.SetChildIndex(this.darkButton2, 0);
            this.Controls.SetChildIndex(this.darkDataGridView1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.darkDataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private DarkUI.Controls.DarkButton darkButton1;
        private DarkUI.Controls.DarkButton darkButton2;
        private DarkUI.Controls.DarkDataGridView darkDataGridView1;
        private System.Windows.Forms.ImageList imageList1;
    }
}