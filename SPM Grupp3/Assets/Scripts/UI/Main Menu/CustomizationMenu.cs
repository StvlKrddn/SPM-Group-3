using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationMenu : MonoBehaviour
{
    [SerializeField] private Slider[] colorSliders;
    [SerializeField] private GameObject tankBody;
    [SerializeField] private GameObject hat;
    [SerializeField] private CustomizationManager customizationManager;

    private Material defaultTankMaterial;
    private Material defaultHatMaterial;
    private Slider currentSlider;
    private Material newTankMaterial;
    private Material newHatMaterial;
    private Renderer tankRenderer;
    private Renderer hatRenderer;
    
    public Color newColor;

    void Start()
    {
        tankRenderer = tankBody.GetComponent<Renderer>();
        defaultTankMaterial = tankRenderer.material;
        newTankMaterial = new Material(defaultTankMaterial.shader);
        newTankMaterial.CopyPropertiesFromMaterial(defaultTankMaterial);
        tankRenderer.material = newTankMaterial;

        hatRenderer = hat.GetComponent<Renderer>();
        defaultHatMaterial = hatRenderer.material;
        newHatMaterial = new Material(defaultHatMaterial.shader);
        newHatMaterial.CopyPropertiesFromMaterial(defaultHatMaterial);
        hatRenderer.material = newHatMaterial;
    }

    void OnDestroy() 
    {
        if (tankRenderer != null)
        {
            tankRenderer.material = defaultTankMaterial;
        }
        if (hatRenderer != null)
        {
            hatRenderer.material = defaultHatMaterial;
        }
    }

    void Update()
    {
        newColor = new Color(colorSliders[0].value, colorSliders[1].value, colorSliders[2].value);
        newTankMaterial.color = newColor;
        newHatMaterial.color = newColor;
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
            false
        );
        customizationManager.CustomizationData.Add(customData);
    }
}
