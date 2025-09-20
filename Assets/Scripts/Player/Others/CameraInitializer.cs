using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CameraInitializer : MonoBehaviour
{
    private void OnEnable()
    {
        // Invalidate the confiner cache to ensure it's up to date
        CinemachineConfiner2D confiner = GetComponent<Unity.Cinemachine.CinemachineConfiner2D>();
        if (confiner != null)
        {
            StartCoroutine(InvalidateConfinerCache(confiner));
        }
    }

    private IEnumerator InvalidateConfinerCache(Unity.Cinemachine.CinemachineConfiner2D confiner)
    {
        yield return new WaitForEndOfFrame();
        confiner.InvalidateBoundingShapeCache();
    }
}