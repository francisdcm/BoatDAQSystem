using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;

namespace BoatDAQ2{
    class Device {
        protected List<long> deviceTimeStamps = new List<long>(10000);
        protected List<double> deviceValues = new List<double>(10000);
        private string port;
        protected Stopwatch watch;
        protected Chart dataChart;
        protected int deviceType; //0 for encoder, 1 for inclinometer, 2 for ultrasonic sensor, 99 for other

        public int getDeviceType() {
            return deviceType;
        }

        public virtual void resetWatch() {
            watch.Restart();
        }

        public List<long> getDeviceTimeStamps() {
            return deviceTimeStamps;
        }

        public List<double> getDeviceValues() {
            return deviceValues;
        }
        public string getPort() {
            return port;
        }

        public virtual void initializeChart(string seriesName, string yAxisName) {
            dataChart = new Chart();
            dataChart.ChartAreas.Add("ChartArea1");
            dataChart.ChartAreas[0].AxisX.Title = "Time (ms)";
            dataChart.ChartAreas[0].AxisY.Title = yAxisName;
            dataChart.Series.Add(seriesName);
            dataChart.Series[0].ChartType = SeriesChartType.FastPoint;
            dataChart.Size = new Size(623, 315);
            // dataChart.Visible = false;
        }

        public Chart getChart() {
            return dataChart;
        }

        public void clearChart() {
            for (int j = 0; j < dataChart.Series.Count; j++) {
                dataChart.Series[j].Points.Clear();
            }
        }

        public void setPort(string portName) {
            port = portName;
        }

        public virtual void connectDevice(string port, DataGridView deviceTable, TextBox debugText, int inputDeviceType) {
        }

        public virtual void readData(DataGridView deviceTable, int rowNumber) {
        }

        public virtual void exportData(string directoryName) {

        }

        public virtual void exportData(ref Excel.Worksheet excelWorksheet, string filePath) {

        }

        public virtual void resetDevice() {
            //to be called before data collection starts
            resetWatch();
            deviceTimeStamps.Clear();
            deviceValues.Clear();
        }

    }
}
