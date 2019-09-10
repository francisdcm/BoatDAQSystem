using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
using System.Data;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

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
            debugText.AppendText("Connected on port " + ultrasonicReader.PortName + ".\n");
            deviceType = inputDeviceType;
            watch = new Stopwatch();
            watch.Start();
        }

        public override void readData(DataGridView deviceTable, int rowNumber) {
            try {
                //if (watch.ElapsedMilliseconds % 10 <= 5) {         
                    dataChart.Invoke((MethodInvoker)delegate {
                        //get current reading, plot it, save the data
                        string result = ultrasonicReader.ReadLine();
                        long time = watch.ElapsedMilliseconds;
                        double distance = double.Parse(result.Substring(1));
                        // Running on the UI thread
                        dataChart.Series[0].Points.AddXY(time, distance);
                        deviceTimeStamps.Add(time);
                        deviceValues.Add(distance);
                        deviceTable[2, rowNumber].Value = distance.ToString();
                        deviceTable[4, rowNumber].Value = time.ToString();
                    });
               // }
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
           string pathName = System.IO.Path.Combine(directoryName, "BoatDAQ2Data_Ultrasonic.txt");
            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(pathName, true)) {
                for (int i = 0; i < deviceTimeStamps.Count; i++) {
                        fs.WriteLine("Ultrasonic Sensor" + "\t" + deviceTimeStamps[i].ToString() + "\t" + deviceValues[i].ToString());
                }
                fs.Dispose();
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