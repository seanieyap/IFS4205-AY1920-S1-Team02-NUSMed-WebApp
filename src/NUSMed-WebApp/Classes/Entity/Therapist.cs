﻿using System;

namespace NUSMed_WebApp.Classes.Entity
{
    [Serializable]
    public class Therapist : Account
    {
        #region For Patients
        public bool acceptNewRequest { get; set; } = false;
        public short permissionUnapproved { get; set; } = 0;
        public DateTime? requestTime { get; set; }
        public short permissionApproved { get; set; } = 0;
        public DateTime? approvedTime { get; set; }
        public bool isEmergency { get; set; } = false;

        #region permission   
        public short? recordPermissionStatus { get; set; }

        public bool hasHeightMeasurementPermissionsApproved
        {
            get
            {
                if ((permissionApproved & new HeightMeasurement().permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasWeightMeasurementPermissionsApproved
        {
            get
            {
                if ((permissionApproved & new WeightMeasurement().permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasTemperatureReadingPermissionsApproved
        {
            get
            {
                if ((permissionApproved & new TemperatureReading().permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasBloodPressureReadingPermissionsApproved
        {
            get
            {
                if ((permissionApproved & new BloodPressureReading().permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasECGReadingPermissionsApproved
        {
            get
            {
                if ((permissionApproved & new ECGReading().permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasMRIPermissionsApproved
        {
            get
            {
                if ((permissionApproved & new MRI().permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasXRayPermissionsApproved
        {
            get
            {
                if ((permissionApproved & new XRay().permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasGaitPermissionsApproved
        {
            get
            {
                if ((permissionApproved & new Gait().permissionFlag) != 0)
                    return true;
                return false;
            }
        }

        public bool hasHeightMeasurementPermissions
        {
            get
            {
                if ((permissionUnapproved & new HeightMeasurement().permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasWeightMeasurementPermissions
        {
            get
            {
                if ((permissionUnapproved & new WeightMeasurement().permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasTemperatureReadingPermissions
        {
            get
            {
                if ((permissionUnapproved & new TemperatureReading().permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasBloodPressureReadingPermissions
        {
            get
            {
                if ((permissionUnapproved & new BloodPressureReading().permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasECGReadingPermissions
        {
            get
            {
                if ((permissionUnapproved & new ECGReading().permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasMRIPermissions
        {
            get
            {
                if ((permissionUnapproved & new MRI().permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasXRayPermissions
        {
            get
            {
                if ((permissionUnapproved & new XRay().permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasGaitPermissions
        {
            get
            {
                if ((permissionUnapproved & new Gait().permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        #endregion


        #endregion
    }
}