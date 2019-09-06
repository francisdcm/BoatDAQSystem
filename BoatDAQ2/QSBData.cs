using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoatDAQ2{
    class QSBData{
        public uint m_countValue { get; set; }
        public long m_timeValue { get; set; }
        public string m_COMPort { get; set; }
        public QSBData(uint countValue, long timeValue, string COMPort) {
            m_countValue = countValue;
            m_timeValue = timeValue;
            m_COMPort = COMPort;
        }
    }
}
