using CustomInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "BootstrapData", menuName = "Data/BootstrapData")]
public class BootstrapData : SingletonScriptableObject<BootstrapData>
{
    [FixedValues(0, 1, 2, 3, 4)] public int vSyncCount = 1;
}
