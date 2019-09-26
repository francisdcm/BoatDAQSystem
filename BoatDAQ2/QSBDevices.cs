using System.Collections.Generic;
using System.Windows.Forms;
using USDigital;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;

namespace BoatDAQ2{
    class QSBDevices : Device{
        private DeviceManager mDeviceManager;
        private List<QSB_S> QSBDeviceList = new List<QSB_S>(2);
        private List<string> stringData = new List<string>(10000);
        private Dictionary<string, int> deviceRowInTable = new Dictionary<string, int>(2);
        private bool m_recordData = false;
        DataGridView deviceTableRef;
        private long watchFreq;


        public void setRecordData(bool recordData) {
            m_recordData = recordData;
        }

        public List<string> getQSBData() {
            return stringData;
        }

        public IList<IDevice> getQSBDeviceList() {
            return mDeviceManager.Devices;
        }

        public override void resetWatch() {
            for(int i = 0; i<QSBDeviceList.Count; i++) {
                QSBDeviceList[i].ResetTimeStamp();
            }
        }

        public override void initializeChart(string seriesName, string yAxisName) {
            dataChart = new Chart();
            dataChart.ChartAreas.Add("ChartArea1");
            dataChart.ChartAreas[0].AxisX.Title = "Time (ms)";
            dataChart.ChartAreas[0].AxisY.Title = yAxisName;
            for (int i = 0; i < QSBDeviceList.Count; i++) {
                dataChart.Series.Add("QSB " + QSBDeviceList[i].Connection);
                dataChart.Series[i].ChartType = SeriesChartType.FastPoint;
            }
            dataChart.Size = new Size(623, 315);
        }

        public override void connectDevice(string port,  DataGridView deviceTable, TextBox debugText, int inputDeviceType) {
            if (mDeviceManager == null) {
                mDeviceManager = new DeviceManager();
                mDeviceManager.Initialize();
            }
            QSB_S aQSB = null;
            IList<IDevice> deviceManagerDevices = mDeviceManager.Devices;
            for (int i = 0; i < deviceManagerDevices.Count; i++) {
                QSBDeviceList.Add((QSB_S)deviceManagerDevices[i]);
            }
            for (int i = 0; i < QSBDeviceList.Count; i++) {
                if (QSBDeviceList[i].GetType().FullName.Contains("QSB")) {
                    aQSB = QSBDeviceList[i];
                    // Updated 10/31/2016 sys: Set the response format to include the device timestamp.
                    aQSB.SetResponseFormat(false, false, true, false);
                    var itemX = deviceTable.Rows.Add(aQSB.Connection, "QSB " + aQSB.SerialNumber.ToString(), "unknown", "count", "unknown");
                    for (int j = 0; j < deviceTable.Rows.Count; j++) {
                        if (deviceTable[0, j].Value.ToString() == aQSB.Connection) {
                            deviceRowInTable.Add(aQSB.Connection, j); //associate port with row #
                        }
                    }
                    var count = aQSB.StreamEncoderCount(0, 0);
                    aQSB.OnRegisterValueChanged += aQSB_OnRegisterValueChanged;
                    aQSB.SetResolution((uint)100 * 100);
                    debugText.AppendText("QSB connected on port " + aQSB.Connection + ".\n");
                }
            }
            // Configure QSB to stream count on timer interval. Each interval equal to apx. 1.95 ms.
            debugText.AppendText(deviceManagerDevices.Count.ToString() + " devices found\n");
            deviceType = inputDeviceType;
            this.setPort(port);
            deviceTableRef = deviceTable;
        }

        void aQSB_OnRegisterValueChanged(object sender, RegisterChangeEventArgs args) {
            QSB_S aQSB = (QSB_S)sender;
            if (m_recordData) {
                dataChart.Invoke((MethodInvoker)delegate { // Running on the UI thread
                    dataChart.Series["QSB " + aQSB.Connection].Points.AddXY(1.95*args.TimeStamp, args.Value);
                    stringData.Add("QSB-D on " + aQSB.Connection + "\t" + (1.95*args.TimeStamp).ToString() + "\t" + args.Value.ToString());
                });
            }
            else {
                deviceTableRef[2, deviceRowInTable[aQSB.Connection]].Value = args.Value.ToString();
                deviceTableRef[4, deviceRowInTable[aQSB.Connection]].Value = (1.95 * args.TimeStamp).ToString();
            }
        }

        public void changeQSBResolution(int value) {
            foreach (IDevice QSBDevice in mDeviceManager.Devices) {
                QSB_S aQSB = (QSB_S)QSBDevice;
                aQSB.SetResolution((uint)value * 100);
            }
        }

        public override void exportData(string directoryName) {
            if(stringData.Count == 0) {
                MessageBox.Show("ERROR: No data to save.");
                return;
            }
            string pathName = System.IO.Path.Combine(directoryName, "BoatDAQ2Data_QSBDevice.txt");
            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(pathName, true)) {
                for (int i = 0; i<stringData.Count; i++) {
                    fs.WriteLine(stringData[i]);
                }
                fs.Dispose();
            }
        }

        public override void exportData(ref Excel.Worksheet excelWorksheet, string filePath) {
            if (stringData.Count == 0) {
                MessageBox.Show("ERROR: No data to save.");
                return;
            }
            excelWorksheet.Cells[1, "A"] = "Device Type";
            excelWorksheet.Cells[1, "B"] = "Time (ms)";
            excelWorksheet.Cells[1, "C"] = "Encoder Count";
            excelWorksheet.Name = "QSBData";
            string[] components = new string[3];
            for (int i = 0; i < stringData.Count; i++) {
                components = stringData[i].Split('\t');
                excelWorksheet.Cells[i + 2, "A"] = components[0];
                excelWorksheet.Cells[i + 2, "B"] = components[1];
                excelWorksheet.Cells[i + 2, "C"] = components[2];
            }
        }

        public override void resetDevice() {
            stringData.Clear();
            for(int i=0; i<QSBDeviceList.Count; i++) { //read in the lists
                QSBDeviceList[i].ResetTimeStamp();
            }
        }
    }
}