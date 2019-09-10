using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace NUSMed_WebApp.Classes.Entity
{
    [Serializable]
    public abstract class RecordType
    {

        public abstract string name { get; }
        public abstract short permissionFlag { get; }
        public abstract bool isContent { get; }        // true = record contains content not file
        public virtual bool IsContentValid(string content)
        {
            return false;
        }
        public virtual bool IsFileValid(string content)
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
        public override short permissionFlag
        {
            get { return 1; }
        }
        public override bool isContent { get { return true; } }
        public override bool IsContentValid(string content)
        {
            return false;
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
            return false;
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
            return false;
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
            return false;
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
        public override bool IsFileValid(string extension)
        {
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
        public override short permissionFlag
        {
            get { return 32; }
        }
        public override bool isContent { get { return false; } }
        public override bool IsFileValid(string extension)
        {
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
        public override bool IsFileValid(string extension)
        {
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
        public override bool IsFileValid(string extension)
        {
            return false;
        }
    }
}