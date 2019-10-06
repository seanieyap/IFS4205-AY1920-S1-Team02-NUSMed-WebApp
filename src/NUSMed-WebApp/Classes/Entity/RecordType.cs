using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace NUSMed_WebApp.Classes.Entity
{
    [Serializable]
    public abstract class RecordType
    {
        protected const int maxTextSize = 524288;
        protected const int maxImageSize = 5242880;
        protected const int maxVideoSize = 52428800;

        public abstract string name { get; }
        public abstract short permissionFlag { get; }
        public abstract bool isContent { get; }        // true = record contains content not file
        public virtual string prefix { get; }
        public virtual bool IsContentValid(string content)
        {
            return false;
        }
        public virtual bool IsFileValid(string fileExtension, int? fileSize)
        {
            return false;
        }

        public virtual string GetTextPlotJS(string data)
        {
            return string.Empty;
        }

        public static RecordType Get(string type)
        {
            if (type.Equals(new HeightMeasurement().name))
            {
                return new HeightMeasurement();
            }
            else if (type.Equals(new WeightMeasurement().name))
            {
                return new WeightMeasurement();
            }
            else if (type.Equals(new TemperatureReading().name))
            {
                return new TemperatureReading();
            }
            else if (type.Equals(new BloodPressureReading().name))
            {
                return new BloodPressureReading();
            }
            else if (type.Equals(new ECGReading().name))
            {
                return new ECGReading();
            }
            else if (type.Equals(new MRI().name))
            {
                return new MRI();
            }
            else if (type.Equals(new XRay().name))
            {
                return new XRay();
            }
            else if (type.Equals(new Gait().name))
            {
                return new Gait();
            }

            return null;
        }
    }
    [Serializable]
    public class HeightMeasurement : RecordType
    {
        public override string name
        {
            get { return "Height Measurement"; }
        }
        public override short permissionFlag
        {
            get { return 1; }
        }
        public override bool isContent { get { return true; } }
        public override bool IsContentValid(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                decimal contentDecimal;
                if (decimal.TryParse(content, NumberStyles.Number, CultureInfo.InvariantCulture, out contentDecimal))
                {
                    if (contentDecimal >= 0 && contentDecimal <= 280)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override string prefix
        {
            get { return "cm"; }
        }
    }
    [Serializable]
    public class WeightMeasurement : RecordType
    {
        public override string name
        {
            get { return "Weight Measurement"; }
        }
        public override short permissionFlag
        {
            get { return 2; }
        }
        public override bool isContent { get { return true; } }
        public override bool IsContentValid(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                decimal contentDecimal;
                if (decimal.TryParse(content, NumberStyles.Number, CultureInfo.InvariantCulture, out contentDecimal))
                {
                    if (contentDecimal >= 0 && contentDecimal <= 650)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override string prefix
        {
            get { return "KG"; }
        }
    }
    [Serializable]
    public class TemperatureReading : RecordType
    {
        public override string name
        {
            get { return "Temperature Reading"; }
        }
        public override short permissionFlag
        {
            get { return 4; }
        }
        public override bool isContent { get { return true; } }
        public override bool IsContentValid(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                decimal contentDecimal;
                if (decimal.TryParse(content, NumberStyles.Number, CultureInfo.InvariantCulture, out contentDecimal))
                {
                    if (contentDecimal >= 0 && contentDecimal <= 100)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override string prefix
        {
            get { return "°C"; }
        }
    }
    [Serializable]
    public class BloodPressureReading : RecordType
    {
        public override string name
        {
            get { return "Blood Pressure Reading"; }
        }
        public override short permissionFlag
        {
            get { return 8; }
        }
        public override bool isContent { get { return true; } }
        public override bool IsContentValid(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                string[] contents = content.Split('/');

                int systolicPressure, distolicPressure;

                if (int.TryParse(contents[0], out systolicPressure) && int.TryParse(contents[1], out distolicPressure))
                {
                    if (systolicPressure >= 0 && systolicPressure <= 250 && distolicPressure >= 0 && distolicPressure <= 250)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override string prefix
        {
            get { return "mmHG"; }
        }
    }
    [Serializable]
    public class ECGReading : RecordType
    {
        public override string name
        {
            get { return "ECG Reading"; }
        }
        public override short permissionFlag
        {
            get { return 16; }
        }
        public override bool isContent { get { return false; } }
        public override bool IsFileValid(string extension, int? size)
        {
            if (extension.Equals(".txt") && size <= maxTextSize)
            {
                return true;
            }
            return false;
        }
        public override string GetTextPlotJS(string data)
        {
            string[] dataArray = data.Split(',');
            List<string> timeList = new List<string>();

            double timeBuffer = 0;
            foreach (string value in dataArray)
            {
                timeList.Add(timeBuffer.ToString());
                timeBuffer += 0.008;
            }

            string times = string.Join(", ", timeList.ToArray());

            string js = @"
                var layout = {
                    title: 'ECG Heartbeat',
                    yaxis: {
                        title: 'Magnitude',
                        autotick: true,
                        ticks: 'outside',
                      },
                      xaxis: {
                        title: 'Time (seconds)',
                        autotick: true,
                        ticks: 'outside',
                      },
                };

                var trace1 = {
                        type: 'scatter',
                        mode: 'lines',
                        name: 'ECG',
                        y: [" + data + @"],
                        x: [" + times + @"],
                        line: { color: '#17BECF' }
                };
                Plotly.newPlot('modalFileViewPanelText', [trace1], layout);";

            return js;
        }
    }
    [Serializable]
    public class MRI : RecordType
    {
        public override string name
        {
            get { return "MRI"; }
        }
        public override short permissionFlag
        {
            get { return 32; }
        }
        public override bool isContent { get { return false; } }
        public override bool IsFileValid(string extension, int? size)
        {
            if ((extension.Equals(".jpg") || extension.Equals(".jpeg") || extension.Equals(".png")) && size <= maxImageSize)
            {
                return true;
            }
            return false;
        }
    }
    [Serializable]
    public class XRay : RecordType
    {
        public override string name
        {
            get { return "X-ray"; }
        }
        public override short permissionFlag
        {
            get { return 64; }
        }
        public override bool isContent { get { return false; } }
        public override bool IsFileValid(string extension, int? size)
        {
            if ((extension.Equals(".jpg") || extension.Equals(".jpeg") || extension.Equals(".png")) && size <= maxImageSize)
            {
                return true;
            }
            return false;
        }
    }
    [Serializable]
    public class Gait : RecordType
    {
        public override string name
        {
            get { return "Gait"; }
        }
        public override short permissionFlag
        {
            get { return 128; }
        }
        public override bool isContent { get { return false; } }
        public override bool IsFileValid(string extension, int? size)
        {
            if (((extension.Equals(".mp4")) && size <= maxVideoSize) ||
                ((extension.Equals(".txt")) && size <= maxTextSize))
            {
                return true;
            }
            return false;
        }
        public override string GetTextPlotJS(string data)
        {
            return string.Empty;
        }

    }
}