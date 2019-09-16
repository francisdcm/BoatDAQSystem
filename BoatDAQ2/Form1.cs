using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using USDigital;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO.Ports;
using System.Diagnostics;
using System.Collections.Concurrent;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO.Compression;

namespace BoatDAQ2{
    public partial class Form1 : Form{
        List<string> ports = new List<string>();
        List<Device> devices = new List<Device>();
        List<BackgroundWorker> backgroundWorkers = new List<BackgroundWorker>(); //initialize
        //or maybe 1 background worker for all data collection? test both implementations
        List<Chart> charts = new List<Chart>();
        string pathName = @"C:\Users\Public\BoatDAQ2Data";
        Excel.Application excelFile = new Excel.Application();
        Stopwatch watch = new Stopwatch();

        public Form1() {
            InitializeComponent();
            ports = SerialPort.GetPortNames().Cast<string>().ToList(); //get current ports upon startup
            for (int i = 0; i < ports.Count; i++) {
                portOptionsBox.Items.Add(ports[i]);
            }
            stopRecodingButton.Enabled = false;
            startRecordingButton.Enabled = false;
            saveFilePathText.Text = pathName;
            zeroEncoderButton.Enabled = false;
            maxCountUpDown.Enabled = false;
            Text = Application.ProductName + @" - Version " + Application.ProductVersion;
            outputText.AppendText("Note: The file extension is purposefully missing on the save path. Use the last subdirectory as the name of the file" +
                "you wish to create\n");
        }

        private void refreshPortsButton_Click(object sender, EventArgs e) {
            ports.Clear(); //clear out old ports
            portOptionsBox.Items.Clear(); //and the options in the selection box
            ports = SerialPort.GetPortNames().Cast<string>().ToList(); //get current ports
            for (int i = 0; i < ports.Count; i++) {
                portOptionsBox.Items.Add(ports[i]);
            }
        }

        private void connectButton_Click(object sender, EventArgs e) {
            if (deviceTypeBox.SelectedIndex == -1 || portOptionsBox.SelectedIndex == -1) {
                MessageBox.Show("Error: Select device type and/or port.");
                return;
            }
            for (int i = 0; i < devices.Count; i++) {
                if (devices[i].getPort() == ports[portOptionsBox.SelectedIndex]) {
                    MessageBox.Show("Error: Port is already addded as a device.");
                    return;
                }
            }
            //add the device
            switch (deviceTypeBox.SelectedIndex) {
                //0 = encoder
                //1 = Rieker angle reader
                //2 = ultrasonic sensor
                //3 = speedometer
                case 0:
                    QSBDevices tempQSB = new QSBDevices();
                    tempQSB.connectDevice(ports[portOptionsBox.SelectedIndex], deviceTable, outputText, 0);
                    devices.Add(tempQSB);
                    tempQSB.initializeChart("Encoder Count", "Count");
                    //tempQSB.setChartOrigin(12, 230 + 200 * (devices.Count - 1));
                    Controls.Add(tempQSB.getChart());
                    zeroEncoderButton.Enabled = true;
                    maxCountUpDown.Enabled = true;
                    break;
                case 1:
                    RiekerInclinometer tempAngleReader = new RiekerInclinometer();
                    tempAngleReader.connectDevice(ports[portOptionsBox.SelectedIndex], deviceTable, outputText, 1);
                    devices.Add(tempAngleReader);
                    tempAngleReader.initializeChart("Inclinometer " + tempAngleReader.getPort(), "Angle (degrees)");
                    // tempAngleReader.setChartOrigin(12, 230 + 200 * (devices.Count - 1));
                    Controls.Add(tempAngleReader.getChart());
                    break;
                case 2:
                    UltrasonicSensor tempUltrasonic = new UltrasonicSensor();
                    tempUltrasonic.connectDevice(ports[portOptionsBox.SelectedIndex], deviceTable, outputText, 2);
                    devices.Add(tempUltrasonic);
                    tempUltrasonic.initializeChart("Ultrasonic Sensor " + tempUltrasonic.getPort(), "Distance (cm)");
                    Controls.Add(tempUltrasonic.getChart());
                    break;
                case 3:
                    Speedometer tempSpeedometer = new Speedometer();
                    tempSpeedometer.connectDevice(ports[portOptionsBox.SelectedIndex], deviceTable, outputText, 3);
                    devices.Add(tempSpeedometer);
                    tempSpeedometer.initializeChart("Speedometer Sensor " + tempSpeedometer.getPort(), "Speed (knots)");
                    tempSpeedometer.getChart().Series[0].ChartType = SeriesChartType.FastLine;
                    tempSpeedometer.getChart().Series[0].BorderWidth = 5;
                    Controls.Add(tempSpeedometer.getChart());
                    break;
                default:
                    MessageBox.Show("To be implemented");
                    return;
            }
            addBackgroundWorker();
            resizeCharts();
            startRecordingButton.Enabled = true;
        }

        private void resizeCharts() {
            int count = devices.Count;
            int individualChartHeight = 350 / devices.Count;
            for (int i = 0; i < count; i++) {
                devices[i].getChart().Size = new Size(635, individualChartHeight);
                devices[i].getChart().Location = new Point(12, 230 + i * (individualChartHeight));
            }
        }

        private void addBackgroundWorker() {
            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += new DoWorkEventHandler(workHandler);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workHandlerCompleted);
            bgw.WorkerSupportsCancellation = true;
            backgroundWorkers.Add(bgw);
        }

        private void workHandler(object sender, DoWorkEventArgs e) {
            BackgroundWorker worker = sender as BackgroundWorker;
            //collect the data here, and time consuming part at the end
            int index = (int)e.Argument;
            Device dev = devices[index];
            int rowNumber = 0;
            for (int i = 0; i < deviceTable.RowCount; i++) {
                if (deviceTable[0, i].Value.ToString() == dev.getPort()) { //find the corresponding row number
                    rowNumber = i;
                    break;
                }
            }
            dev.resetDevice();
            while (true) {
                if (worker.CancellationPending == true) { //cancellation requested
                    e.Cancel = true;
                    break;
                }
                dev.readData(deviceTable, rowNumber);
            }
        }

        private void workHandlerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Error != null) { //if exception occurs
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled) {
                outputText.AppendText("Data collection stopped.\n");
            }
            else {
                outputText.AppendText("Data collection ended.\n");
            }
            for (int i = 0; i < devices.Count; i++) {
                if (devices[i].getDeviceType() == 0) {
                    outputText.AppendText("Encoder data points collected:  " + ((QSBDevices)devices[i]).getQSBData().Count + "\n");
                }
                else if (devices[i].getDeviceType() == 1) {
                    outputText.AppendText("Inclinometer data points collected:  " + devices[i].getDeviceValues().Count + "\n");
                }
                else {
                    outputText.AppendText("Data points collected:  " + devices[i].getDeviceValues().Count + "\n");
                }
            }
        }

        private void startRecordingButton_Click(object sender, EventArgs e) {
            stopRecodingButton.Enabled = true;
            startRecordingButton.Enabled = false;
            plotDataCheckBox.Checked = true;
            for (int i = 0; i < devices.Count; i++) {
                backgroundWorkers[i].RunWorkerAsync(i);
            }
        }

        private void stopRecodingButton_Click(object sender, EventArgs e) {
            for (int i = backgroundWorkers.Count - 1; i >= 0; --i) {
                backgroundWorkers[i].CancelAsync();
            }
            plotDataCheckBox.Checked = false;
            stopRecodingButton.Enabled = false;
            startRecordingButton.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void button2_Click(object sender, EventArgs e) {
            for (int i = 0; i < devices.Count; i++) {
                devices[i].clearChart();
                devices[i].resetDevice();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Environment.Exit(0);
            Application.Exit();
        }

        private void listDiagButton_Click(object sender, EventArgs e) {
            outputText.AppendText("number of devices: " + devices.Count + " number of " + "background workers: " + backgroundWorkers.Count + "\n");
        }

        private void button3_Click(object sender, EventArgs e) {
            for (int i = 0; i < devices.Count; i++) {
                devices[i].resetWatch();
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            for (int i = 0; i < devices.Count; i++) {
                if (devices[i].getDeviceType() == 0) {
                    IList<IDevice> QSBDevices = ((QSBDevices)devices[i]).getQSBDeviceList();
                    for (int j = 0; j < QSBDevices.Count; j++) {
                        ((QSB_S)QSBDevices[j]).ResetCount();
                    }
                    break; //always only one qsbdevices (note plural!)
                }
            }
        }

        private void closePortButton_Click(object sender, EventArgs e) {
            if (deviceTypeBox.SelectedIndex == -1 || portOptionsBox.SelectedIndex == -1) {
                MessageBox.Show("Error: Select device type and/or port.");
                return;
            }
            switch (deviceTypeBox.SelectedIndex) {
                case 0:
                    MessageBox.Show("QSB cannot be disconnected. To disconnect, close application instead");
                    break;
                case 1:
                    for (int i = 0; i < devices.Count; i++) {
                        if (devices[i].getPort() == ports[portOptionsBox.SelectedIndex]) {
                            ((RiekerInclinometer)devices[i]).closeAngleReader(deviceTable);
                            devices.RemoveAt(i);
                        }
                    }
                    break;
                case 2: 
                    MessageBox.Show("Close ultrasonic sensor, to be implemented...");
                    break;
                case 3:
                    MessageBox.Show("Close speedometer, to be implemented...");
                    break;
                default:
                    MessageBox.Show("To be implemented...");
                    break;
            }
        }

        private void saveButton_Click(object sender, EventArgs e) {
            string directoryName = System.IO.Path.GetDirectoryName(saveFilePathText.Text);
            if (exportFileTypeBox.SelectedIndex == -1) {
                MessageBox.Show("Error: Please select the file export format.");
                return;
            }
            if (exportFileTypeBox.SelectedIndex == 0) {
                outputText.AppendText("Exporting data in .xlsx format. Please wait...");
                string filePath = saveFilePathText.Text + ".xlsx";
                saveFilePathText.Text = filePath;
                var excelWorkBook = excelFile.Workbooks.Add(Missing.Value);
                var excelSheets = excelWorkBook.Sheets as Excel.Sheets;
                for (int i = 0; i < devices.Count; i++) {
                    var xlNewSheet = (Excel.Worksheet)excelSheets.Add(excelSheets[1], Type.Missing, Type.Missing, Type.Missing);
                    devices[i].exportData(ref xlNewSheet, filePath);
                }
                excelWorkBook.SaveAs(filePath);
                excelWorkBook.Close();
                excelFile.Quit();
                outputText.AppendText("Finished exporting data in .xlsx format.\n");
            }
            else if (exportFileTypeBox.SelectedIndex == 1) {
                //make new directory, save text files in that directory, then zip that directory
                outputText.AppendText("Exporting data in .txt format. Please wait...");
                string newFolderPath = System.IO.Path.Combine(directoryName, "BoatDAQ2DataFolder");
                System.IO.Directory.CreateDirectory(newFolderPath);
                for (int i = 0; i < devices.Count; i++) {
                    devices[i].exportData(newFolderPath); //make a temporary new folder
                }
                string zipPath = System.IO.Path.Combine(directoryName, "BoatDAQ2Data.zip");
                ZipFile.CreateFromDirectory(newFolderPath, zipPath);
                System.IO.Directory.Delete(newFolderPath, true);
                outputText.AppendText("Finished exporting data, .zip file created.\n");
            }
        }

        private void saveFilePath_Click(object sender, EventArgs e) {
            if (chooseFolderDialog.ShowDialog() == DialogResult.OK) {
                pathName = chooseFolderDialog.SelectedPath;      //get the chosen path  
            }
            string filename = "BoatDAQ2Data";
            pathName = System.IO.Path.Combine(pathName, filename);
            saveFilePathText.Text = pathName; // show the path in SavePathTextbox
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) {
            for (int i = 0; i < devices.Count; i++) {
                if (devices[i].getDeviceType() == 0) {
                    ((QSBDevices)devices[i]).changeQSBResolution((int)maxCountUpDown.Value);
                    break;
                }
            }
        }

        private void plotDataCheckBox_CheckedChanged(object sender, EventArgs e) {
            if (plotDataCheckBox.Checked == true) {
                for (int i = 0; i < devices.Count; i++) {
                    if (devices[i].getDeviceType() == 0) {
                        ((QSBDevices)devices[i]).setRecordData(true);
                    }
                }
            }
            if (plotDataCheckBox.Checked == false) {
                for (int i = 0; i < devices.Count; i++) {
                    if (devices[i].getDeviceType() == 0) {
                        ((QSBDevices)devices[i]).setRecordData(false);
                    }
                }
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
    }
}