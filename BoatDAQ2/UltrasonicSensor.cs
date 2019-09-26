using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

namespace BoatDAQ2{
    class UltrasonicSensor : Device{
        private SerialPort ultrasonicReader;
        int errors = 0;

        public override void connectDevice(string port, DataGridView deviceTable, TextBox debugText, int inputDeviceType) {
            ultrasonicReader = new SerialPort(port, 9600, Parity.None);
            this.setPort(port);
            ultrasonicReader.Open();
            string[] ultrasonicProperties = { port, "Ultrasonic Sensor", "unknown", "cm", "unknown" };
            deviceTable.Rows.Add(ultrasonicProperties);
            debugText.AppendText("Ultrasonic sensor connected on port " + ultrasonicReader.PortName + ".\n");
            deviceType = inputDeviceType;
            watch = new Stopwatch();
            watch.Start();
        }

        public override void readData(DataGridView deviceTable, int rowNumber) {
            try {
                if (watch.ElapsedMilliseconds % 75 <= 5) { //get current reading, plot it, save the data
                    string result = (ultrasonicReader.ReadLine().Split('\t'))[1];
                    long time = watch.ElapsedMilliseconds;
                    double distance = double.Parse(result);
                    if (distance > 300) {
                        errors++;
                        return;
                    }
                    dataChart.Invoke((MethodInvoker)delegate { // Running on the UI thread
                        dataChart.Series[0].Points.AddXY(time, distance);                       
                    });
                    deviceTimeStamps.Add(time);
                    deviceValues.Add(distance);
                    deviceTable[2, rowNumber].Value = distance.ToString();
                    deviceTable[4, rowNumber].Value = time.ToString();
                }
            }
            catch {
                errors++; //for the occassional bad line of data, like "+75.00+75.00"
            }
        }

        public int getErrors() {
            return errors;
        }

        public override void exportData(string directoryName) {
            if (deviceTimeStamps.Count == 0 || deviceValues.Count == 0) {
                MessageBox.Show("ERROR: No data to save.");
                return;
            }
           string pathName = System.IO.Path.Combine(directoryName, "BoatDAQ2Data_Ultrasonic.txt");
            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(pathName, true)) {
                for (int i = 0; i < deviceTimeStamps.Count; i++) {
                        fs.WriteLine("Ultrasonic Sensor" + "\t" + deviceTimeStamps[i].ToString() + "\t" + deviceValues[i].ToString());
                }
                fs.Dispose();
            }
        }

        public override void exportData(ref Excel.Worksheet excelWorksheet, string filePath) {
            if (deviceTimeStamps.Count == 0 || deviceValues.Count == 0) {
                MessageBox.Show("ERROR: No data to save.");
                return;
            }
            excelWorksheet.Cells[1, "A"] = "Device Type";
            excelWorksheet.Cells[1, "B"] = "Time (ms)";
            excelWorksheet.Cells[1, "C"] = "Distance (cm)";
            excelWorksheet.Name = "Ultrasonic";
            for (int i = 0; i < deviceTimeStamps.Count; i++) {
                excelWorksheet.Cells[i + 2, "A"] = "Ultrasonic";
                excelWorksheet.Cells[i + 2, "B"] = deviceTimeStamps[i].ToString();
                excelWorksheet.Cells[i + 2, "C"] = deviceValues[i].ToString();
            }
        }

        public override void resetDevice() {
            errors = 0;
            ultrasonicReader.DiscardOutBuffer(); //don't know which one so do both
            ultrasonicReader.DiscardInBuffer();
            watch.Restart();
            deviceTimeStamps.Clear();
            deviceValues.Clear();
        }
        public void closeAngleReader(DataGridView deviceTable) {
            int row = 0;
            for (int i = 0; i < deviceTable.RowCount; i++) {
                if (deviceTable[0, i].Value.ToString() == ultrasonicReader.PortName) {
                    row = i;
                    break;
                }
            }
            deviceTable.Rows.RemoveAt(row);
            ultrasonicReader.Dispose();
            ultrasonicReader.Close();
            deviceTimeStamps.Clear();
            deviceValues.Clear();
            watch.Stop();
            dataChart.Dispose();
        }
    }
}