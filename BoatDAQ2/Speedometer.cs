using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

namespace BoatDAQ2{
    class Speedometer : Device{
        private SerialPort speedReader;
        private int errors;

        public override void connectDevice(string port, DataGridView deviceTable, TextBox debugText, int inputDeviceType) {            
            speedReader = new SerialPort(port, 9600);
            speedReader.StopBits = StopBits.One;
            speedReader.Handshake = Handshake.None;
            speedReader.DtrEnable = true;
            this.setPort(port);
            speedReader.Open();
            string[] speedometerProperties = { port, "Speedometer", "unknown", "knots", "unknown" };
            deviceTable.Rows.Add(speedometerProperties);
            debugText.AppendText("Speedometer connected on port " + speedReader.PortName + ".\n");
            deviceType = inputDeviceType;
            watch = new Stopwatch();
            watch.Start();
        }

        public override void readData(DataGridView deviceTable, int rowNumber) {
            //reads an instant of data
            try {
                if (watch.ElapsedMilliseconds % 100 <= 5) { //every 250 ms
                    string result = speedReader.ReadLine();
                    long time = watch.ElapsedMilliseconds;
                    double speedReading = double.Parse(result) * 0.44348;
                    dataChart.Invoke((MethodInvoker)delegate {
                        //get current reading, plot it, save the data                                     
                        // Running on the UI thread
                        dataChart.Series[0].Points.AddXY(time, speedReading);                      
                    });
                    deviceTimeStamps.Add(time);
                    deviceValues.Add(speedReading);
                    deviceTable[2, rowNumber].Value = speedReading.ToString();
                     deviceTable[4, rowNumber].Value = time.ToString();
                }
            }
            catch {
                errors++;
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
            string pathName = System.IO.Path.Combine(directoryName, "BoatDAQ2Data_Speedometer.txt");
            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(pathName, true)) {
                for (int i = 0; i < deviceTimeStamps.Count; i++) {
                    fs.WriteLine("Speedometer" + "\t" + deviceTimeStamps[i].ToString() + "\t+" + deviceValues[i].ToString());
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
            excelWorksheet.Cells[1, "C"] = "Speed (knots)";
            excelWorksheet.Name = "Speedometer";
            for (int i = 0; i < deviceTimeStamps.Count; i++) {
                excelWorksheet.Cells[i + 2, "A"] = "Speedometer";
                excelWorksheet.Cells[i + 2, "B"] = deviceTimeStamps[i].ToString();
                excelWorksheet.Cells[i + 2, "C"] = deviceValues[i].ToString();
            }
        }

        public override void resetDevice() {
            errors = 0;
            speedReader.DiscardOutBuffer(); //don't know which one so do both
            speedReader.DiscardInBuffer();
            watch.Restart();
            deviceTimeStamps.Clear();
            deviceValues.Clear();
        }
        public void closeAngleReader(DataGridView deviceTable) {
            int row = 0;
            for (int i = 0; i < deviceTable.RowCount; i++) {
                if (deviceTable[0, i].Value.ToString() == speedReader.PortName) {
                    row = i;
                    break;
                }
            }
            deviceTable.Rows.RemoveAt(row);
            speedReader.Dispose();
            speedReader.Close();
            deviceTimeStamps.Clear();
            deviceValues.Clear();
            watch.Stop();
            dataChart.Dispose();
        }
    }
}