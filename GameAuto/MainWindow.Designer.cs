namespace GameAuto
{
    partial class MainWindow
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
            this.btnSetROI = new System.Windows.Forms.Button();
            this.lbconfig = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.timer_process = new System.Windows.Forms.Timer(this.components);
            this.lbProcessTime = new System.Windows.Forms.Label();
            this.picScr = new System.Windows.Forms.PictureBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.lstBox = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.picScr)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSetROI
            // 
            this.btnSetROI.Enabled = false;
            this.btnSetROI.Location = new System.Drawing.Point(12, 12);
            this.btnSetROI.Name = "btnSetROI";
            this.btnSetROI.Size = new System.Drawing.Size(100, 32);
            this.btnSetROI.TabIndex = 0;
            this.btnSetROI.Text = "Set ROI";
            this.btnSetROI.UseVisualStyleBackColor = true;
            this.btnSetROI.Click += new System.EventHandler(this.btnSetROI_Click);
            // 
            // lbconfig
            // 
            this.lbconfig.AutoSize = true;
            this.lbconfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbconfig.Location = new System.Drawing.Point(118, 20);
            this.lbconfig.Name = "lbconfig";
            this.lbconfig.Size = new System.Drawing.Size(0, 18);
            this.lbconfig.TabIndex = 1;
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(12, 50);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(129, 83);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start Automation";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // timer_process
            // 
            this.timer_process.Interval = 1000;
            this.timer_process.Tick += new System.EventHandler(this.timer_process_Tick);
            // 
            // lbProcessTime
            // 
            this.lbProcessTime.AutoSize = true;
            this.lbProcessTime.Location = new System.Drawing.Point(49, 147);
            this.lbProcessTime.Name = "lbProcessTime";
            this.lbProcessTime.Size = new System.Drawing.Size(35, 13);
            this.lbProcessTime.TabIndex = 3;
            this.lbProcessTime.Text = "label1";
            // 
            // picScr
            // 
            this.picScr.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.picScr.Location = new System.Drawing.Point(12, 181);
            this.picScr.Name = "picScr";
            this.picScr.Size = new System.Drawing.Size(1061, 573);
            this.picScr.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picScr.TabIndex = 2;
            this.picScr.TabStop = false;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(257, 12);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 32);
            this.btnExit.TabIndex = 0;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lstBox
            // 
            this.lstBox.FormattingEnabled = true;
            this.lstBox.Location = new System.Drawing.Point(1105, 20);
            this.lstBox.Name = "lstBox";
            this.lstBox.Size = new System.Drawing.Size(212, 186);
            this.lstBox.TabIndex = 4;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1346, 766);
            this.Controls.Add(this.lstBox);
            this.Controls.Add(this.lbProcessTime);
            this.Controls.Add(this.picScr);
            this.Controls.Add(this.lbconfig);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnSetROI);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainWindow";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picScr)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSetROI;
        private System.Windows.Forms.Label lbconfig;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Timer timer_process;
        private System.Windows.Forms.Label lbProcessTime;
        private System.Windows.Forms.PictureBox picScr;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ListBox lstBox;
    }
}

