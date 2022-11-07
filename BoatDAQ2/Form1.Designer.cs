namespace BoatDAQ2
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
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.deviceTypeBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.exportFileTypeBox = new System.Windows.Forms.ComboBox();
            this.outputText = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.saveFilePath = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.saveFilePathText = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.refreshPortsButton = new System.Windows.Forms.Button();
            this.closePortButton = new System.Windows.Forms.Button();
            this.connectButton = new System.Windows.Forms.Button();
            this.portOptionsBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.maxCountUpDown = new System.Windows.Forms.NumericUpDown();
            this.button3 = new System.Windows.Forms.Button();
            this.zeroEncoderButton = new System.Windows.Forms.Button();
            this.deviceTable = new System.Windows.Forms.DataGridView();
            this.colCOMPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeviceType2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReading2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnits2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTime2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startRecordingButton = new System.Windows.Forms.Button();
            this.stopRecodingButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listDiagButton = new System.Windows.Forms.Button();
            this.dataReader = new System.ComponentModel.BackgroundWorker();
            this.chooseFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.plotDataCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.maxCountUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviceTable)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // deviceTypeBox
            // 
            this.deviceTypeBox.FormattingEnabled = true;
            this.deviceTypeBox.Items.AddRange(new object[] {
            "QSB/Mercury 2000 Encoder",
            "Prolific/Rieker Inclinometer",
            "Arduino CH340/Ultrasonic Sensor",
            "Arduino Leonardo/Speedometer",
            "Arduino Uno/Transducer",
            "Other"});
            this.deviceTypeBox.Location = new System.Drawing.Point(82, 21);
            this.deviceTypeBox.Margin = new System.Windows.Forms.Padding(4);
            this.deviceTypeBox.Name = "deviceTypeBox";
            this.deviceTypeBox.Size = new System.Drawing.Size(198, 21);
            this.deviceTypeBox.TabIndex = 1;
            this.deviceTypeBox.SelectedIndexChanged += new System.EventHandler(this.deviceTypeBox_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 13);
            this.label8.TabIndex = 102;
            this.label8.Text = "Device Type";
            // 
            // exportFileTypeBox
            // 
            this.exportFileTypeBox.FormattingEnabled = true;
            this.exportFileTypeBox.Items.AddRange(new object[] {
            "Excel (.xlsx)",
            "Text (.zip of .txt)"});
            this.exportFileTypeBox.Location = new System.Drawing.Point(84, 584);
            this.exportFileTypeBox.Name = "exportFileTypeBox";
            this.exportFileTypeBox.Size = new System.Drawing.Size(103, 21);
            this.exportFileTypeBox.TabIndex = 12;
            // 
            // outputText
            // 
            this.outputText.Location = new System.Drawing.Point(12, 610);
            this.outputText.Multiline = true;
            this.outputText.Name = "outputText";
            this.outputText.ReadOnly = true;
            this.outputText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputText.Size = new System.Drawing.Size(551, 74);
            this.outputText.TabIndex = 99;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(569, 582);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 15;
            this.saveButton.Text = "Save Data";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // saveFilePath
            // 
            this.saveFilePath.Location = new System.Drawing.Point(537, 582);
            this.saveFilePath.Name = "saveFilePath";
            this.saveFilePath.Size = new System.Drawing.Size(26, 23);
            this.saveFilePath.TabIndex = 14;
            this.saveFilePath.Text = "...";
            this.saveFilePath.UseVisualStyleBackColor = true;
            this.saveFilePath.Click += new System.EventHandler(this.saveFilePath_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 587);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 96;
            this.label7.Text = "Save data as:";
            // 
            // saveFilePathText
            // 
            this.saveFilePathText.Location = new System.Drawing.Point(193, 584);
            this.saveFilePathText.Name = "saveFilePathText";
            this.saveFilePathText.Size = new System.Drawing.Size(338, 20);
            this.saveFilePathText.TabIndex = 13;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(327, 175);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(117, 48);
            this.button2.TabIndex = 9;
            this.button2.Text = "Clear Plot and Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // refreshPortsButton
            // 
            this.refreshPortsButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.refreshPortsButton.Location = new System.Drawing.Point(569, 19);
            this.refreshPortsButton.Name = "refreshPortsButton";
            this.refreshPortsButton.Size = new System.Drawing.Size(80, 23);
            this.refreshPortsButton.TabIndex = 5;
            this.refreshPortsButton.Text = "Refresh Ports List]";
            this.refreshPortsButton.UseVisualStyleBackColor = false;
            this.refreshPortsButton.Click += new System.EventHandler(this.refreshPortsButton_Click);
            // 
            // closePortButton
            // 
            this.closePortButton.Location = new System.Drawing.Point(493, 19);
            this.closePortButton.Name = "closePortButton";
            this.closePortButton.Size = new System.Drawing.Size(70, 23);
            this.closePortButton.TabIndex = 4;
            this.closePortButton.Text = "Disconnect";
            this.closePortButton.UseVisualStyleBackColor = true;
            this.closePortButton.Click += new System.EventHandler(this.closePortButton_Click);
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(422, 19);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(65, 23);
            this.connectButton.TabIndex = 3;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // portOptionsBox
            // 
            this.portOptionsBox.FormattingEnabled = true;
            this.portOptionsBox.Location = new System.Drawing.Point(346, 21);
            this.portOptionsBox.Name = "portOptionsBox";
            this.portOptionsBox.Size = new System.Drawing.Size(70, 21);
            this.portOptionsBox.TabIndex = 2;
            this.portOptionsBox.SelectedIndexChanged += new System.EventHandler(this.portOptionsBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(282, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 87;
            this.label5.Text = "Serial Port:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(451, 205);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 13);
            this.label3.TabIndex = 86;
            this.label3.Text = "Max. Count Number (100X):";
            // 
            // maxCountUpDown
            // 
            this.maxCountUpDown.Location = new System.Drawing.Point(595, 201);
            this.maxCountUpDown.Name = "maxCountUpDown";
            this.maxCountUpDown.Size = new System.Drawing.Size(55, 20);
            this.maxCountUpDown.TabIndex = 11;
            this.maxCountUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.maxCountUpDown.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(217, 175);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(104, 48);
            this.button3.TabIndex = 8;
            this.button3.Text = "Zero Timestamps";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // zeroEncoderButton
            // 
            this.zeroEncoderButton.Location = new System.Drawing.Point(449, 175);
            this.zeroEncoderButton.Name = "zeroEncoderButton";
            this.zeroEncoderButton.Size = new System.Drawing.Size(200, 23);
            this.zeroEncoderButton.TabIndex = 10;
            this.zeroEncoderButton.Text = "Zero encoder values";
            this.zeroEncoderButton.UseVisualStyleBackColor = true;
            this.zeroEncoderButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // deviceTable
            // 
            this.deviceTable.AllowUserToAddRows = false;
            this.deviceTable.AllowUserToDeleteRows = false;
            this.deviceTable.AllowUserToResizeRows = false;
            this.deviceTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.deviceTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCOMPort,
            this.colDeviceType2,
            this.colReading2,
            this.colUnits2,
            this.colTime2});
            this.deviceTable.Location = new System.Drawing.Point(12, 48);
            this.deviceTable.Name = "deviceTable";
            this.deviceTable.ReadOnly = true;
            this.deviceTable.Size = new System.Drawing.Size(635, 121);
            this.deviceTable.TabIndex = 105;
            // 
            // colCOMPort
            // 
            this.colCOMPort.HeaderText = "COM Port";
            this.colCOMPort.Name = "colCOMPort";
            this.colCOMPort.ReadOnly = true;
            this.colCOMPort.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colDeviceType2
            // 
            this.colDeviceType2.HeaderText = "Device Type";
            this.colDeviceType2.Name = "colDeviceType2";
            this.colDeviceType2.ReadOnly = true;
            this.colDeviceType2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDeviceType2.Width = 150;
            // 
            // colReading2
            // 
            this.colReading2.HeaderText = "Reading";
            this.colReading2.Name = "colReading2";
            this.colReading2.ReadOnly = true;
            this.colReading2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colReading2.Width = 110;
            // 
            // colUnits2
            // 
            this.colUnits2.HeaderText = "Units";
            this.colUnits2.Name = "colUnits2";
            this.colUnits2.ReadOnly = true;
            this.colUnits2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colTime2
            // 
            this.colTime2.HeaderText = "Time";
            this.colTime2.Name = "colTime2";
            this.colTime2.ReadOnly = true;
            this.colTime2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colTime2.Width = 125;
            // 
            // startRecordingButton
            // 
            this.startRecordingButton.Location = new System.Drawing.Point(12, 175);
            this.startRecordingButton.Name = "startRecordingButton";
            this.startRecordingButton.Size = new System.Drawing.Size(103, 48);
            this.startRecordingButton.TabIndex = 6;
            this.startRecordingButton.Text = "Start Recording";
            this.startRecordingButton.UseVisualStyleBackColor = true;
            this.startRecordingButton.Click += new System.EventHandler(this.startRecordingButton_Click);
            // 
            // stopRecodingButton
            // 
            this.stopRecodingButton.Location = new System.Drawing.Point(121, 175);
            this.stopRecodingButton.Name = "stopRecodingButton";
            this.stopRecodingButton.Size = new System.Drawing.Size(90, 48);
            this.stopRecodingButton.TabIndex = 7;
            this.stopRecodingButton.Text = "Stop Recording";
            this.stopRecodingButton.UseVisualStyleBackColor = true;
            this.stopRecodingButton.Click += new System.EventHandler(this.stopRecodingButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(657, 24);
            this.menuStrip1.TabIndex = 112;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // listDiagButton
            // 
            this.listDiagButton.Location = new System.Drawing.Point(569, 608);
            this.listDiagButton.Name = "listDiagButton";
            this.listDiagButton.Size = new System.Drawing.Size(75, 52);
            this.listDiagButton.TabIndex = 16;
            this.listDiagButton.Text = "List Diagnostics (Advanced)";
            this.listDiagButton.UseVisualStyleBackColor = true;
            this.listDiagButton.Click += new System.EventHandler(this.listDiagButton_Click);
            // 
            // plotDataCheckBox
            // 
            this.plotDataCheckBox.AutoSize = true;
            this.plotDataCheckBox.Enabled = false;
            this.plotDataCheckBox.Location = new System.Drawing.Point(574, 666);
            this.plotDataCheckBox.Name = "plotDataCheckBox";
            this.plotDataCheckBox.Size = new System.Drawing.Size(70, 17);
            this.plotDataCheckBox.TabIndex = 0;
            this.plotDataCheckBox.Text = "Plot Data";
            this.plotDataCheckBox.UseVisualStyleBackColor = true;
            this.plotDataCheckBox.CheckedChanged += new System.EventHandler(this.plotDataCheckBox_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 701);
            this.Controls.Add(this.plotDataCheckBox);
            this.Controls.Add(this.listDiagButton);
            this.Controls.Add(this.stopRecodingButton);
            this.Controls.Add(this.startRecordingButton);
            this.Controls.Add(this.deviceTable);
            this.Controls.Add(this.deviceTypeBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.exportFileTypeBox);
            this.Controls.Add(this.outputText);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.saveFilePath);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.saveFilePathText);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.refreshPortsButton);
            this.Controls.Add(this.closePortButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.portOptionsBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.maxCountUpDown);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.zeroEncoderButton);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.maxCountUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviceTable)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox deviceTypeBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox exportFileTypeBox;
        private System.Windows.Forms.TextBox outputText;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button saveFilePath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox saveFilePathText;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button refreshPortsButton;
        private System.Windows.Forms.Button closePortButton;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.ComboBox portOptionsBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown maxCountUpDown;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button zeroEncoderButton;
        private System.Windows.Forms.DataGridView deviceTable;
        private System.Windows.Forms.Button startRecordingButton;
        private System.Windows.Forms.Button stopRecodingButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button listDiagButton;
        private System.ComponentModel.BackgroundWorker dataReader;
        private System.Windows.Forms.FolderBrowserDialog chooseFolderDialog;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCOMPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeviceType2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReading2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnits2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime2;
        private System.Windows.Forms.CheckBox plotDataCheckBox;
    }
}

