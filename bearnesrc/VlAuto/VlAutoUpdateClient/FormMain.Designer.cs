namespace VlAutoUpdateClient
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            buttonRetry = new Button();
            buttonOpenGame = new Button();
            buttonOpenAuto = new Button();
            buttonExit = new Button();
            progressBarAll = new ProgressBar();
            progressBarCurent = new ProgressBar();
            label1 = new Label();
            label2 = new Label();
            labelStatus = new Label();
            SuspendLayout();
            // 
            // buttonRetry
            // 
            buttonRetry.BackColor = Color.Transparent;
            buttonRetry.FlatAppearance.BorderColor = Color.MistyRose;
            buttonRetry.FlatAppearance.MouseDownBackColor = Color.Snow;
            buttonRetry.FlatAppearance.MouseOverBackColor = Color.Snow;
            buttonRetry.ForeColor = SystemColors.ControlText;
            buttonRetry.Location = new Point(73, 575);
            buttonRetry.Name = "buttonRetry";
            buttonRetry.Size = new Size(75, 35);
            buttonRetry.TabIndex = 0;
            buttonRetry.Text = "Thử lại";
            buttonRetry.UseVisualStyleBackColor = false;
            buttonRetry.Click += buttonRetry_Click;
            // 
            // buttonOpenGame
            // 
            buttonOpenGame.Location = new Point(154, 575);
            buttonOpenGame.Name = "buttonOpenGame";
            buttonOpenGame.Size = new Size(75, 35);
            buttonOpenGame.TabIndex = 1;
            buttonOpenGame.Text = "Mở game";
            buttonOpenGame.UseVisualStyleBackColor = true;
            buttonOpenGame.Click += buttonOpenGame_Click;
            // 
            // buttonOpenAuto
            // 
            buttonOpenAuto.Location = new Point(235, 575);
            buttonOpenAuto.Name = "buttonOpenAuto";
            buttonOpenAuto.Size = new Size(75, 35);
            buttonOpenAuto.TabIndex = 2;
            buttonOpenAuto.Text = "Thiết lập";
            buttonOpenAuto.UseVisualStyleBackColor = true;
            buttonOpenAuto.Click += buttonAutoGame_Click;
            // 
            // buttonExit
            // 
            buttonExit.Location = new Point(318, 575);
            buttonExit.Name = "buttonExit";
            buttonExit.Size = new Size(75, 35);
            buttonExit.TabIndex = 3;
            buttonExit.Text = "Thoát";
            buttonExit.UseVisualStyleBackColor = true;
            buttonExit.Click += buttonExit_Click;
            // 
            // progressBarAll
            // 
            progressBarAll.Location = new Point(131, 554);
            progressBarAll.Name = "progressBarAll";
            progressBarAll.Size = new Size(262, 12);
            progressBarAll.TabIndex = 4;
            // 
            // progressBarCurent
            // 
            progressBarCurent.Location = new Point(131, 534);
            progressBarCurent.Name = "progressBarCurent";
            progressBarCurent.Size = new Size(262, 12);
            progressBarCurent.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(73, 533);
            label1.Name = "label1";
            label1.Size = new Size(51, 15);
            label1.TabIndex = 6;
            label1.Text = "Hiện tại:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(73, 554);
            label2.Name = "label2";
            label2.Size = new Size(52, 15);
            label2.TabIndex = 7;
            label2.Text = "Toàn bộ:";
            // 
            // labelStatus
            // 
            labelStatus.AutoEllipsis = true;
            labelStatus.BorderStyle = BorderStyle.FixedSingle;
            labelStatus.Location = new Point(131, 471);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(262, 60);
            labelStatus.TabIndex = 8;
            labelStatus.Text = "Hãy nhấp *Mở game* để bôn tẩu giang hồ";
            labelStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(480, 657);
            Controls.Add(labelStatus);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(progressBarCurent);
            Controls.Add(progressBarAll);
            Controls.Add(buttonExit);
            Controls.Add(buttonOpenAuto);
            Controls.Add(buttonOpenGame);
            Controls.Add(buttonRetry);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bearne Princess Version: 1.0.1";
            Load += FormMain_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonRetry;
        private Button buttonOpenGame;
        private Button buttonOpenAuto;
        private Button buttonExit;
        private ProgressBar progressBarAll;
        private ProgressBar progressBarCurent;
        private Label label1;
        private Label label2;
        private Label labelStatus;
    }
}