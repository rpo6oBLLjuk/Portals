using CustomInspector;
using UnityEngine;

public class EffectService : MonoBehaviour
{
    [SerializeField] private GameObject lightningObj;
    [SerializeField, ShowIf(ComparisonOp.NotNull, nameof(lightningObj))] private Transform lightningStartPoint;
    [SerializeField, ShowIf(ComparisonOp.NotNull, nameof(lightningObj))] private Transform lightningEndPoint;


    public void EnablePortalGunLightning()
    {
        lightningObj.SetActive(true);
    }

    public void DisablePortalGunLightning()
    {
        lightningObj.SetActive(false);
    }

    public void SetPortalGunLightningPoints(Vector3 startPoint, Vector3 endPoint)
    {
        lightningStartPoint.position = startPoint;
        lightningEndPoint.position = endPoint;
    }
}
