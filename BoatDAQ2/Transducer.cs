using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

namespace BoatDAQ2{
    class Transducer : Device{       
        private SerialPort pressureReader;
        private int errors;
        // Connecting the device to the computer
        public override void connectDevice(string port, DataGridView deviceTable, TextBox debugText, int inputDeviceType) {
            pressureReader = new SerialPort(port, 9600);
            pressureReader.StopBits = StopBits.One;
            pressureReader.Handshake = Handshake.None;
            pressureReader.DtrEnable = true;
            this.setPort(port);
            pressureReader.Open();
            string[] transducerProperties = { port, "Transducer", "unknown", "Pressure (Pa)", "unknown" };
            deviceTable.Rows.Add(transducerProperties);
            debugText.AppendText("Transducer connected on port " + pressureReader.PortName + ".\n");
            deviceType = inputDeviceType;
            watch = new Stopwatch();
            watch.Start();
        }
        // Gathering data from the transducer
        public override void readData(DataGridView deviceTable, int rowNumber) {
            //reads an instant of data
            try {
                if (watch.ElapsedMilliseconds % 100 <= 5) { //every 250 ms
                    string result = pressureReader.ReadLine();              // Reading the value (Volts) from the transducer
                    long time = watch.ElapsedMilliseconds;
                    double pressureReading = (double.Parse(result) * 0.5)* 6894.76; // Replace this line with the transducer specifications (slope of the fcn that relates pressure to voltage, and converting psi to Pa)
                    dataChart.Invoke((MethodInvoker)delegate {
                        //get current reading, plot it, save the data                                     
                        // Running on the UI thread
                        dataChart.Series[0].Points.AddXY(time, pressureReading);                      
                    });
                    deviceTimeStamps.Add(time);
                    deviceValues.Add(pressureReading);
                    deviceTable[2, rowNumber].Value = pressureReading.ToString();
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
            string pathName = System.IO.Path.Combine(directoryName, "BoatDAQ2Data_Transducer.txt");
            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(pathName, true)) {
                for (int i = 0; i < deviceTimeStamps.Count; i++) {
                    fs.WriteLine("Transducer" + "\t" + deviceTimeStamps[i].ToString() + "\t+" + deviceValues[i].ToString());
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
            excelWorksheet.Cells[1, "C"] = "Pressure (Pa)";
            excelWorksheet.Name = "Transducer";
            for (int i = 0; i < deviceTimeStamps.Count; i++) {
                excelWorksheet.Cells[i + 2, "A"] = "Transducer";
                excelWorksheet.Cells[i + 2, "B"] = deviceTimeStamps[i].ToString();
                excelWorksheet.Cells[i + 2, "C"] = deviceValues[i].ToString();
            }
        }

        public override void resetDevice() {
            errors = 0;
            pressureReader.DiscardOutBuffer(); //don't know which one so do both
            pressureReader.DiscardInBuffer();
            watch.Restart();
            deviceTimeStamps.Clear();
            deviceValues.Clear();
        }
        public void closeAngleReader(DataGridView deviceTable) {
            int row = 0;
            for (int i = 0; i < deviceTable.RowCount; i++) {
                if (deviceTable[0, i].Value.ToString() == pressureReader.PortName) {
                    row = i;
                    break;
                }
            }
            deviceTable.Rows.RemoveAt(row);
            pressureReader.Dispose();
            pressureReader.Close();
            deviceTimeStamps.Clear();
            deviceValues.Clear();
            watch.Stop();
            dataChart.Dispose();
        }
    }
}