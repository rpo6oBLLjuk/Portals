using UnityEngine;
using UnityEngine.UI;

public class JatpackWidget : MonoBehaviour
{
    [SerializeField] private Slider enegrySlider;


    private void OnEnable()
    {
        PlayerJatpack.EnergyChanged += EnergyChanged;
    }

    private void OnDisable()
    {
        PlayerJatpack.EnergyChanged -= EnergyChanged;
    }

    private void EnergyChanged(float value)
    {
        enegrySlider.value = value;
    }
}
