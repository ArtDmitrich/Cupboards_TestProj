using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace CamerasController
{
	public class CamerasController: MonoBehaviour, ICamerasController
    {
	    [SerializeField] private Camera _mainCamera;
	    [SerializeField] private Camera _miniMapCamera;
	    
	    public void FitToPointsCamera(CameraType cameraType, List<Transform> points)
	    {
		    switch (cameraType)
		    {
			    case CameraType.MainCamera:
				    _mainCamera.FitToPoints(points);
				    break;
			    case CameraType.MiniMapCamera:
				    _miniMapCamera.FitToPoints(points);
				    break;
		    }
	    }

	    public void SetCameraEnabled(CameraType cameraType, bool enabled)
	    {
		    switch (cameraType)
		    {
			    case CameraType.MainCamera:
				    _mainCamera.gameObject.SetActive(enabled);
				    break;
			    case CameraType.MiniMapCamera:
				    _miniMapCamera.gameObject.SetActive(enabled);
				    break;
		    }
	    }
    }
}