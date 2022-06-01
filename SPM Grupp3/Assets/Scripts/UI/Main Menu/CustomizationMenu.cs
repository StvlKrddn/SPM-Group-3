using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationMenu : MonoBehaviour
{
    [SerializeField] private Slider[] colorSliders;
    [SerializeField] private GameObject tankBody;
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject hat;
    [SerializeField] private CustomizationManager customizationManager;
    
    private Material defaultTankMaterial;
    private Material defaultTurretMaterial;
    private Material defaultHatMaterial;
    private Material newTankMaterial;
    private Material newTurretMaterial;
    private Material newHatMaterial;
    private Renderer tankRenderer;
    private Renderer turretRenderer;
    private Renderer hatRenderer;
    private Slider currentSlider;
    
    [System.NonSerialized] public Color newColor;

    void Awake()
    {
        // Cache default materials and create new copies for preview

        tankRenderer = tankBody.GetComponent<Renderer>();
        defaultTankMaterial = tankRenderer.material;
        newTankMaterial = new Material(defaultTankMaterial.shader);
        newTankMaterial.CopyPropertiesFromMaterial(defaultTankMaterial);
        tankRenderer.material = newTankMaterial;

        turretRenderer = turret.GetComponent<Renderer>();
        defaultTurretMaterial = turretRenderer.material;
        newTurretMaterial = new Material(defaultTurretMaterial.shader);
        newTurretMaterial.CopyPropertiesFromMaterial(defaultTurretMaterial);
        turretRenderer.material = newTurretMaterial;

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
            turretRenderer.material = defaultTurretMaterial;
            hatRenderer.material = defaultHatMaterial;
        }
    }

    void Update()
    {
        newColor = new Color(colorSliders[0].value, colorSliders[1].value, colorSliders[2].value);
        newTankMaterial.color = newColor;
        newTurretMaterial.color = newColor;
        newHatMaterial.color = newColor;
    }

    public void ChangeValue(Text valueText)
    {
        // Slider value is a percentage and must be converted

        float value = Mathf.Round(255 * (currentSlider.value / 2));
        
        valueText.text = value.ToString();
    }

    public void SetSlider(Slider slider)
    {
        currentSlider = slider;
    }

    public void SavePlayerCustomization()
    {
        CustomizationData customData = new CustomizationData(
            newColor,
            false
        );
        customizationManager.CustomizationData.Add(customData);
    }
}
