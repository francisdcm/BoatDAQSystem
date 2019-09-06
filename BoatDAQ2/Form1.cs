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

namespace BoatDAQ2
{
    public partial class Form1 : Form
    {

        List<string> ports = new List<string>();
        List<Device> devices = new List<Device>();
        List<BackgroundWorker> backgroundWorkers = new List<BackgroundWorker>(); //initialize
        //or maybe 1 background worker for all data collection? test both implementations
        List<Chart> charts = new List<Chart>();
        string pathName = @"C:\Users\Public\BoatDAQ2Data.txt";

        public Form1() {
            InitializeComponent();
            ports = SerialPort.GetPortNames().Cast<string>().ToList(); //get current ports upon startup
            for (int i = 0; i < ports.Count; i++) {
                portOptionsBox.Items.Add(ports[i]);
            }
            stopRecodingButton.Enabled = false;
            saveFilePathText.Text = pathName;
            zeroEncoderButton.Enabled = false;
            maxCountUpDown.Enabled = false;
            Text = Application.ProductName + @" - Version " + Application.ProductVersion;
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
                //2 = speedometer
                case 0:
                    QSBDevices tempQSB = new QSBDevices();
                    tempQSB.connectDevice(ports[portOptionsBox.SelectedIndex], deviceTable, outputText, 0);
                    devices.Add(tempQSB);
                    tempQSB.initializeChart("Encoder Count", "Count", tempQSB.getNumDevices());
                    tempQSB.setChartOrigin(12, 230 + 200 * (devices.Count - 1));
                    Controls.Add(tempQSB.getChart());
                    zeroEncoderButton.Enabled = true;
                    maxCountUpDown.Enabled = true;
                    break;
                case 1:
                    RiekerInclinometer tempAngleReader = new RiekerInclinometer();
                    tempAngleReader.connectDevice(ports[portOptionsBox.SelectedIndex], deviceTable, outputText, 1);
                    devices.Add(tempAngleReader);
                    tempAngleReader.initializeChart("Angle", "Angle (degrees)", 1);
                    tempAngleReader.setChartOrigin(12, 230 + 200 * (devices.Count - 1));
                    Controls.Add(tempAngleReader.getChart());
                    break;
                default:
                    MessageBox.Show("to be implemented");
                    return;
            }
            addBackgroundWorker();
            resizeCharts();
        }

        private void resizeCharts() {
            int count = devices.Count;
            int individualChartHeight = 357 / devices.Count;
            for (int i = 0; i < count; i++) {
                devices[i].getChart().Size = new Size(635, individualChartHeight);
                devices[i].getChart().Location = new Point(12, 230 + i * (individualChartHeight + 20));
            }
        }

        private void addBackgroundWorker() {
            BackgroundWorker bgw = new BackgroundWorker();
            bgw.DoWork += new DoWorkEventHandler(workHandler);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workHandlerCompleted);
            bgw.WorkerSupportsCancellation = true;
            backgroundWorkers.Add(bgw);
            outputText.AppendText("Background worker added.\n");
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
                //  try {
                if (worker.CancellationPending == true) { //cancellation requested
                    e.Cancel = true;
                    break;
                }
                dev.readData(deviceTable, rowNumber);
                // }
                // catch (Exception ex) {
                //    outputText.AppendText(ex.Message + "ERROR\n");
                // }
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
            outputText.AppendText("Data points collected:  " + devices[0].getDeviceValues().Count + "\n");
        }

        private void startRecordingButton_Click(object sender, EventArgs e) {
            stopRecodingButton.Enabled = true;
            startRecordingButton.Enabled = false;
            for (int i = 0; i < devices.Count; i++) {
                backgroundWorkers[i].RunWorkerAsync(i);
            }
        }

        private void stopRecodingButton_Click(object sender, EventArgs e) {
            for (int i = backgroundWorkers.Count - 1; i >= 0; --i) {
                backgroundWorkers[i].CancelAsync();
            }
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
            outputText.AppendText("number of devices: " + devices.Count + " number of " +
                "background workers: " + backgroundWorkers.Count + "\n");
        }

        private void showGraphsButton_Click(object sender, EventArgs e) {
            for (int i = 0; i < devices.Count; i++) {
                // Controls.Add(devices[i].getChart());
                devices[i].makeChartVisible(this);
            }
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
                default:
                    MessageBox.Show("To be implemented...");
                    break;
            }
        }

        private void saveButton_Click(object sender, EventArgs e) {
            for (int i = 0; i < devices.Count; i++) {
                devices[i].exportData(saveFilePathText.Text);
            }
        }

        private void saveFilePath_Click(object sender, EventArgs e) {
            if (chooseFolderDialog.ShowDialog() == DialogResult.OK) {
                pathName = chooseFolderDialog.SelectedPath;      //get the chosen path  
            }
            string filename = "BoatDAQ2Data.txt";
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
    }
}