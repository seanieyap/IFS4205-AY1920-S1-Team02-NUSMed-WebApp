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
        public abstract bool isContent { get; }        // true = record contains content not file
        public virtual string prefix { get; }
        public virtual bool IsContentValid(string content)
        {
            return false;
        }
        public virtual bool IsFileValid(string fileExtension, int fileSize)
        {
            return false;
        }
        public static RecordType Get(string type)
        {
            if (type == "Height Measurement")
            {
                return new HeightMeasurement();
            }
            else if (type == "Weight Measurement")
            {
                return new WeightMeasurement();
            }
            else if (type == "Temperature Reading")
            {
                return new TemperatureReading();
            }
            else if (type == "Blood Pressure Reading")
            {
                return new BloodPressureReading();
            }
            else if (type == "ECG Reading")
            {
                return new ECGReading();
            }
            else if (type == "MRI")
            {
                return new MRI();
            }
            else if (type == "X-ray")
            {
                return new XRay();
            }

            return new Gait();
        }
    }

    [Serializable]
    public class HeightMeasurement : RecordType
    {
        public override string name
        {
            get { return "Height Measurement"; }
        }
        public static short permissionFlag
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
                    if (contentDecimal > 0 && contentDecimal <= 280)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override string prefix
        {
            get { return "m"; }
        }
    }
    [Serializable]
    public class WeightMeasurement : RecordType
    {
        public override string name
        {
            get { return "Weight Measurement"; }
        }
        public static short permissionFlag
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
        public static short permissionFlag
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
        public static short permissionFlag
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
        public static short permissionFlag
        {
            get { return 16; }
        }
        public override bool isContent { get { return false; } }
        public override bool IsFileValid(string extension, int size)
        {
            if (extension.Equals(".txt") && size < maxTextSize)
            {
                return true;
            }
            return false;
        }
    }
    [Serializable]
    public class MRI : RecordType
    {
        public override string name
        {
            get { return "MRI"; }
        }
        public static short permissionFlag
        {
            get { return 32; }
        }
        public override bool isContent { get { return false; } }
        public override bool IsFileValid(string extension, int size)
        {
            if ((extension.Equals(".jpg") || extension.Equals(".jpeg") || extension.Equals(".png")) && size < maxImageSize)
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
        public static short permissionFlag
        {
            get { return 64; }
        }
        public override bool isContent { get { return false; } }
        public override bool IsFileValid(string extension, int size)
        {
            if ((extension.Equals(".jpg") || extension.Equals(".jpeg") || extension.Equals(".png")) && size < maxImageSize)
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
        public static short permissionFlag
        {
            get { return 128; }
        }
        public override bool isContent { get { return false; } }
        public override bool IsFileValid(string extension, int size)
        {
            if (((extension.Equals(".mp4")) && size < maxVideoSize) ||
                ((extension.Equals(".txt")) && size < maxTextSize))
            {
                return true;
            }
            return false;
        }
    }
}