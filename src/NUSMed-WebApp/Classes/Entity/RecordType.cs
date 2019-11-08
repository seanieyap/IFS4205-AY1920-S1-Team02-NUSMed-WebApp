using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Security.AntiXss;

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
            if ((extension.Equals(".txt") || extension.Equals(".csv")) && size <= maxTextSize)
            {
                return true;
            }
            return false;
        }
        public override string GetTextPlotJS(string data)
        {
            if (data.Trim().Contains("\n"))
            {
                throw new Exception();
            }

            string[] dataArray = data.Split(',');
            List<string> timeList = new List<string>();

            double timeBuffer = 0;
            foreach (string value in dataArray)
            {
                if (!Regex.IsMatch(value, @"^[1-9]\d*(\.\d+)?$"))
                {
                    throw new Exception();
                }
                timeList.Add(timeBuffer.ToString());
                timeBuffer += 0.008;
            }

            string times = string.Join(", ", timeList.ToArray());

            return @"
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
                        y: [" + AntiXssEncoder.HtmlEncode(data, true) + @"],
                        x: [" + times + @"],
                        line: { color: '#17BECF' }
                };
                Plotly.newPlot('modalFileViewPanelText', [trace1], layout);";
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
                ((extension.Equals(".txt") || extension.Equals(".csv")) && size <= maxTextSize))
            {
                return true;
            }
            return false;
        }
        public override string GetTextPlotJS(string data)
        {
            List<string> time = new List<string>();
            List<string> ax = new List<string>();
            List<string> ay = new List<string>();
            List<string> az = new List<string>();
            List<string> gx = new List<string>();
            List<string> gy = new List<string>();
            List<string> gz = new List<string>();

            try
            {
                using (StringReader reader = new StringReader(data))
                {
                    string line = string.Empty;

                    do
                    {
                        line = reader.ReadLine();
                        if (line != null)
                        {
                            string[] tokens = line.Split(',');
                            time.Add((Convert.ToDouble(tokens[0]) / 1000).ToString());
                            ax.Add(tokens[1]);
                            ay.Add(tokens[2]);
                            az.Add(tokens[3]);
                            gx.Add(tokens[4]);
                            gy.Add(tokens[5]);
                            gz.Add(tokens[6]);
                        }

                    } while (line != null);
                }
            }
            catch
            {
                throw new Exception();
            }

            List<int> check = new List<int> { ax.Count, ay.Count, az.Count, gx.Count, gy.Count, gz.Count };
            if (!data.Trim().Contains("\n") || check[0] == 1 || check.Any(o => o != check[0]))
            {
                throw new Exception();
            }

            Regex regex = new Regex(@"^[1-9]\d*(\.\d+)?$");
            if (ax.Any(x => !regex.IsMatch(x)) || ay.Any(x => !regex.IsMatch(x)) || az.Any(x => !regex.IsMatch(x)) ||
                gx.Any(x => !regex.IsMatch(x)) || gy.Any(x => !regex.IsMatch(x)) || gz.Any(x => !regex.IsMatch(x)))
            {
                throw new Exception();
            }

            string seconds = string.Join(", ", time.ToArray());

            return @"
                var layout = {
                    title: 'Gait Timeseries',
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
                        name: 'ax',
                        y: [" + AntiXssEncoder.HtmlEncode(string.Join(", ", ax.ToArray()), true) + @"],
                        x: [" + seconds + @"],
                        line: { color: '#d53e4f' }
                }

                var trace2 = {
                        type: 'scatter',
                        mode: 'lines',
                        name: 'ay',
                        y: [" + AntiXssEncoder.HtmlEncode(string.Join(", ", ay.ToArray()), true) + @"],
                        x: [" + seconds + @"],
                        line: { color: '#fc8d59' }
                }

                var trace3 = {
                        type: 'scatter',
                        mode: 'lines',
                        name: 'az',
                        y: [" + AntiXssEncoder.HtmlEncode(string.Join(", ", az.ToArray()), true) + @"],
                        x: [" + seconds + @"],
                        line: { color: '#fee08b' }
                }

                var trace4 = {
                        type: 'scatter',
                        mode: 'lines',
                        name: 'gx',
                        y: [" + AntiXssEncoder.HtmlEncode(string.Join(", ", gx.ToArray()), true) + @"],
                        x: [" + seconds + @"],
                        line: { color: '#e6f598' }
                }

                var trace5 = {
                        type: 'scatter',
                        mode: 'lines',
                        name: 'gy',
                        y: [" + AntiXssEncoder.HtmlEncode(string.Join(", ", gy.ToArray()), true) + @"],
                        x: [" + seconds + @"],
                        line: { color: '#99d594' }
                }

                var trace6 = {
                        type: 'scatter',
                        mode: 'lines',
                        name: 'gz',
                        y: [" + AntiXssEncoder.HtmlEncode(string.Join(", ", gz.ToArray()), true) + @"],
                        x: [" + seconds + @"],
                        line: { color: '#3288bd' }
                }

                Plotly.newPlot('modalFileViewPanelText', [trace1, trace2, trace3, trace4, trace5, trace6], layout);";
        }
    }
}