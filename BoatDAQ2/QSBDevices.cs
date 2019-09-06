using System.Collections.Generic;
using System.Windows.Forms;
using USDigital;

namespace BoatDAQ2{
    class QSBDevices : Device{
        private DeviceManager mDeviceManager;
        private int devCount = 0;
        private List<int> rowNumbers = new List<int>();
        private List<List<uint>> encoderData = new List<List<uint>>(2); //nested list to store values - initialization?
        private List<List<long>> timeStamps = new List<List<long>>(2); //nested list for timestamps
        private List<QSB_S> QSBDeviceList;
        uint deviceTime = 0;

        public IList<IDevice> getQSBDeviceList() {
            return mDeviceManager.Devices;
        }

        public override void resetWatch() {
            for(int i = 0; i<QSBDeviceList.Count; i++) {
                QSBDeviceList[i].ResetTimeStamp();
            }
        }

        public override void connectDevice(string port, DataGridView deviceTable, TextBox debugText, int inputDeviceType) {
            if (mDeviceManager == null) {
                mDeviceManager = new DeviceManager();
                mDeviceManager.Initialize();
            }
            QSB_S aQSB = null;
           IList<IDevice> deviceManagerDevices =mDeviceManager.Devices;
            for(int i = 0; i<deviceManagerDevices.Count; i++) {
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
                            rowNumbers.Add(j);
                        }
                    }
                    devCount++;
                    var count = aQSB.StreamEncoderCount(0, 0);
                    aQSB.OnRegisterValueChanged += aQSB_OnRegisterValueChanged;
                    var count2 = aQSB.GetCount();
                    deviceTable[2, rowNumbers[i]].Value = count2.ToString();
                    // Set the QSB resolution
                    aQSB.SetResolution((uint)100 * 100);
                    encoderData.Add(new List<uint>(10000));
                    timeStamps.Add(new List<long>(10000));
                }
            }
            // Configure QSB to stream count on timer interval. Each interval equal to apx. 1.95 ms.
            debugText.AppendText(devCount.ToString() + " devices found\n");
            deviceType = inputDeviceType;
            this.setPort(port);
        }

        void aQSB_OnRegisterValueChanged(object sender, RegisterChangeEventArgs args) {
            QSB_S aQSB = (QSB_S)sender;

            //if (args.Register == (byte)QSB.Register.ReadEncoder) {
               // try {
                    dataChart.Invoke((MethodInvoker)delegate { // Running on the UI thread
                        dataChart.Series[0].Points.AddXY(args.TimeStamp, args.Value);
                    });
               // }
               // catch (Exception ex) {
               //     MessageBox.Show(ex.Message);
               // }
           // }
        }

        public void changeQSBResolution(int value) {
            foreach(IDevice QSBDevice in mDeviceManager.Devices) {
                QSB_S aQSB = (QSB_S)QSBDevice;
                aQSB.SetResolution((uint)value * 100);
            }
        }

        public override void readData(DataGridView deviceTable, int rowNumber) {
            for (int i = 0; i < QSBDeviceList.Count; i++) { //loop through all the devices
                QSB_S aQSB = QSBDeviceList[i];
                uint value= aQSB.GetCountWithTimeStamp(out deviceTime);
               // if (deviceTime % 16 == 0) {
                    dataChart.Invoke((MethodInvoker)delegate { // Running on the UI thread
                        dataChart.Series[i].Points.AddXY(deviceTime, value);
                    });
               // }               
                encoderData[i].Add(value);
                timeStamps[i].Add(deviceTime);
                deviceTable[2, rowNumbers[i]].Value = value.ToString();
                deviceTable[4, rowNumbers[i]].Value = deviceTime.ToString();
            }
        }

        public int getNumDevices() {
            return devCount;
        }

        public override void exportData(string pathName) {
            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(pathName, true)) {
                for(int i = 0; i<encoderData.Count; i++) {
                    string QSBPort = QSBDeviceList[i].Connection;
                    for(int j = 0; j<encoderData[i].Count; j++) {
                        fs.WriteLine("QSB-D on " + QSBPort + "\t" + timeStamps[i][j].ToString() + "\t" + encoderData[i][j].ToString());
                    }
                }
                fs.Dispose();
            }
        }

        public override void resetDevice() {
            encoderData.Clear();
            timeStamps.Clear();
            for(int i =0; i<QSBDeviceList.Count; i++) { //read in the lists
                encoderData.Add(new List<uint>(10000));
                timeStamps.Add(new List<long>(10000));
                QSBDeviceList[i].ResetTimeStamp();
            }            
        }
    }
}
