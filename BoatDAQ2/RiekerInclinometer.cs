using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

namespace BoatDAQ2{
    class RiekerInclinometer : Device{
        private SerialPort angleReader;
        private int errors;

        public override void connectDevice(string port, DataGridView deviceTable, TextBox debugText, int inputDeviceType) {
            angleReader = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
            this.setPort(port);
            angleReader.Open();
            string[] angleReaderProperties = { port, "Inclinometer", "unknown", "degrees", "unknown" };
            deviceTable.Rows.Add(angleReaderProperties);
            debugText.AppendText("Inclinometer connected on port " + angleReader.PortName + ".\n");
            deviceType = inputDeviceType;
            watch = new Stopwatch();
            watch.Start();
        }

        public override void readData(DataGridView deviceTable, int rowNumber) {
            //reads an instant of data
            try {
                if (watch.ElapsedMilliseconds % 10 <= 5) { //H4S output data protocol: every 25 ms, mod10 to slow it down                   
                    string result = angleReader.ReadLine();
                    long time = watch.ElapsedMilliseconds;
                    double angleReading = double.Parse(result.Substring(1));
                    if (result[0] == '-') { //for negative values
                        angleReading = angleReading * -1;
                    }
                    dataChart.Invoke((MethodInvoker)delegate {
                        //get current reading, plot it, save the data                        
                        // Running on the UI thread
                        dataChart.Series[0].Points.AddXY(time, angleReading);                       
                    });
                    deviceTimeStamps.Add(time);
                    deviceValues.Add(angleReading);
                    deviceTable[2, rowNumber].Value = angleReading.ToString();
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
           string pathName = System.IO.Path.Combine(directoryName, "BoatDAQ2Data_Inclinometer.txt");
            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(pathName, true)) {                           
                for (int i = 0; i<deviceTimeStamps.Count; i++) {
                    if(deviceValues[i] >= 0) {
                        fs.WriteLine("Inclinometer" + "\t" + deviceTimeStamps[i].ToString() + "\t" + "+" + deviceValues[i].ToString()); //for pos values, to work with TwoIntervalStatsCalculator
                    }
                    else {
                        fs.WriteLine("Inclinometer" + "\t" + deviceTimeStamps[i].ToString() + "\t" + deviceValues[i].ToString());
                    }
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
            excelWorksheet.Cells[1, "C"] = "Angle (degrees)";
            excelWorksheet.Name = "Inclinometer";
            for (int i = 0; i<deviceTimeStamps.Count; i++) {
                excelWorksheet.Cells[i+2, "A"] = "Inclinometer";
                excelWorksheet.Cells[i+2, "B"] = deviceTimeStamps[i].ToString();
                excelWorksheet.Cells[i+2, "C"] = deviceValues[i].ToString();       
            }
        }

        public override void resetDevice() {
            errors = 0;
            angleReader.DiscardOutBuffer(); //don't know which one so do both
            angleReader.DiscardInBuffer();
            watch.Restart();
            deviceTimeStamps.Clear();
            deviceValues.Clear();
        }
        public void closeAngleReader(DataGridView deviceTable) {
            int row = 0;
            for (int i = 0; i < deviceTable.RowCount; i++) {
                if (deviceTable[0, i].Value.ToString() == angleReader.PortName) {
                    row = i;
                    break;
                }
            }
            deviceTable.Rows.RemoveAt(row);
            angleReader.Dispose();
            angleReader.Close();
            deviceTimeStamps.Clear();
            deviceValues.Clear();
            watch.Stop();
            dataChart.Dispose();
        }
    }
}
