using System.Collections.Generic;
using UnityEngine;

namespace CamerasController
{
    public interface ICamerasController
    {
        void FitToPointsCamera(CameraType cameraType, List<Transform> points);
        void SetCameraEnabled(CameraType cameraType, bool enabled);
    }
}