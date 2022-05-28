using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationMenu : MonoBehaviour
{
    [SerializeField] private Slider[] colorSliders;
    [SerializeField] private GameObject tankBody;

    private Material defaultTankMaterial;
    private Color newColor;
    private Slider currentSlider;
    private Material newTankMaterial;
    private Renderer tankRenderer;

    void Start()
    {
        tankRenderer = tankBody.GetComponent<Renderer>();
        defaultTankMaterial = tankRenderer.material;
        newTankMaterial = new Material(defaultTankMaterial.shader);
        newTankMaterial.CopyPropertiesFromMaterial(defaultTankMaterial);
        tankRenderer.material = newTankMaterial;
    }

    void OnDestroy() 
    {
        if (tankRenderer != null)
        {
            tankRenderer.material = defaultTankMaterial;
        }
    }

    void Update()
    {
        newColor = new Color(colorSliders[0].value, colorSliders[1].value, colorSliders[2].value);
        newTankMaterial.color = newColor;
    }

    public void ChangeValue(Text valueText)
    {
        float value = Mathf.Round(255 * (currentSlider.value / 2));
        
        valueText.text = value.ToString();
    }

    public void SetSlider(Slider slider)
    {
        currentSlider = slider;
    }

    public void SaveCustomization()
    {
        CustomizationData customData = new CustomizationData(
            newColor,
            newColor
        );
        DataManager.WriteToFile(customData, DataManager.CustomizationData);
        print("Customization saved!");
    }
}
