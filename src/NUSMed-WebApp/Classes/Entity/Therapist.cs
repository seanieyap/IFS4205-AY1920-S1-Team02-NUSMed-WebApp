using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public DateTime PermissionCreateTime { get; set; }

        #region permission        
        public bool hasHeightMeasurementPermissionsApproved
        {
            get
            {
                if ((permissionApproved & HeightMeasurement.permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasWeightMeasurementPermissionsApproved
        {
            get
            {
                if ((permissionApproved & WeightMeasurement.permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasTemperatureReadingPermissionsApproved
        {
            get
            {
                if ((permissionApproved & TemperatureReading.permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasBloodPressureReadingPermissionsApproved
        {
            get
            {
                if ((permissionApproved & BloodPressureReading.permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasECGReadingPermissionsApproved
        {
            get
            {
                if ((permissionApproved & ECGReading.permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasMRIPermissionsApproved
        {
            get
            {
                if ((permissionApproved & MRI.permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasXRayPermissionsApproved
        {
            get
            {
                if ((permissionApproved & XRay.permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasGaitPermissionsApproved
        {
            get
            {
                if ((permissionApproved & Gait.permissionFlag) != 0)
                    return true;
                return false;
            }
        }

        public bool hasHeightMeasurementPermissions
        {
            get
            {
                if ((permissionUnapproved & HeightMeasurement.permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasWeightMeasurementPermissions
        {
            get
            {
                if ((permissionUnapproved & WeightMeasurement.permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasTemperatureReadingPermissions
        {
            get
            {
                if ((permissionUnapproved & TemperatureReading.permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasBloodPressureReadingPermissions
        {
            get
            {
                if ((permissionUnapproved & BloodPressureReading.permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasECGReadingPermissions
        {
            get
            {
                if ((permissionUnapproved & ECGReading.permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasMRIPermissions
        {
            get
            {
                if ((permissionUnapproved & MRI.permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasXRayPermissions
        {
            get
            {
                if ((permissionUnapproved & XRay.permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        public bool hasGaitPermissions
        {
            get
            {
                if ((permissionUnapproved & Gait.permissionFlag) != 0)
                    return true;
                return false;
            }
        }
        #endregion

        #endregion
    }
}