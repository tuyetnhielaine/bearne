namespace VlAutoUpdateTool
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
            label1 = new Label();
            textBoxFolderPath = new TextBox();
            buttonFileSelect = new Button();
            button1 = new Button();
            buttonSaveConfig = new Button();
            treeViewPath = new TreeView();
            buttonCreateChecksum = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Location = new Point(12, 6);
            label1.Name = "label1";
            label1.Size = new Size(57, 23);
            label1.TabIndex = 0;
            label1.Text = "Thư mục:";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBoxFolderPath
            // 
            textBoxFolderPath.ImeMode = ImeMode.NoControl;
            textBoxFolderPath.Location = new Point(75, 6);
            textBoxFolderPath.MaxLength = 120;
            textBoxFolderPath.Name = "textBoxFolderPath";
            textBoxFolderPath.ScrollBars = ScrollBars.Both;
            textBoxFolderPath.Size = new Size(599, 23);
            textBoxFolderPath.TabIndex = 1;
            // 
            // buttonFileSelect
            // 
            buttonFileSelect.Location = new Point(680, 6);
            buttonFileSelect.Name = "buttonFileSelect";
            buttonFileSelect.Size = new Size(92, 23);
            buttonFileSelect.TabIndex = 2;
            buttonFileSelect.Text = "Chọn thư mục";
            buttonFileSelect.UseVisualStyleBackColor = true;
            buttonFileSelect.Click += buttonFileSelect_Click;
            // 
            // button1
            // 
            button1.Location = new Point(75, 39);
            button1.Name = "button1";
            button1.Size = new Size(75, 30);
            button1.TabIndex = 1;
            button1.Text = "Chọn file";
            button1.UseVisualStyleBackColor = true;
            button1.Click += buttonOpenFile_Click;
            // 
            // buttonSaveConfig
            // 
            buttonSaveConfig.Location = new Point(168, 39);
            buttonSaveConfig.Name = "buttonSaveConfig";
            buttonSaveConfig.Size = new Size(75, 30);
            buttonSaveConfig.TabIndex = 2;
            buttonSaveConfig.Text = "Đánh dấu";
            buttonSaveConfig.UseVisualStyleBackColor = true;
            buttonSaveConfig.Click += buttonSaveConfig_Click;
            // 
            // treeViewPath
            // 
            treeViewPath.BackColor = Color.Snow;
            treeViewPath.CheckBoxes = true;
            treeViewPath.Location = new Point(12, 79);
            treeViewPath.Name = "treeViewPath";
            treeViewPath.Size = new Size(760, 470);
            treeViewPath.TabIndex = 3;
            treeViewPath.AfterCheck += treeViewPath_AfterCheck;
            // 
            // buttonCreateChecksum
            // 
            buttonCreateChecksum.Location = new Point(265, 39);
            buttonCreateChecksum.Name = "buttonCreateChecksum";
            buttonCreateChecksum.Size = new Size(130, 30);
            buttonCreateChecksum.TabIndex = 4;
            buttonCreateChecksum.Text = "Tạo file checksum";
            buttonCreateChecksum.UseVisualStyleBackColor = true;
            buttonCreateChecksum.Click += buttonCreateChecksum_Click;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1004, 657);
            Controls.Add(buttonCreateChecksum);
            Controls.Add(treeViewPath);
            Controls.Add(buttonSaveConfig);
            Controls.Add(button1);
            Controls.Add(buttonFileSelect);
            Controls.Add(textBoxFolderPath);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tạo danh sách updatefiles Version: 1.0.1";
            Load += FormMain_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBoxFolderPath;
        private Button buttonFileSelect;
        private Button buttonSaveConfig;
        private Button button1;
        private TreeView treeViewPath;
        private Button buttonCreateChecksum;
    }
}
