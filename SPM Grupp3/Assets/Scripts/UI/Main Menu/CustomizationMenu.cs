using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationMenu : MonoBehaviour
{
    [SerializeField] private Slider[] colorSliders;
    [SerializeField] private Text classText;
    [SerializeField] private GameObject tankBody;
    [SerializeField] private GameObject sniperTurret;
    [SerializeField] private GameObject fireTurret;
    [SerializeField] private GameObject hat;
    [SerializeField] private CustomizationManager customizationManager;

    private Material defaultTankMaterial;
    private Material defaultSniperTurretMaterial;
    private Material defaultFireTurretMaterial;
    private Material defaultHatMaterial;
    private Material newTankMaterial;
    private Material newSniperTurretMaterial;
    private Material newFireTurretMaterial;
    private Material newHatMaterial;
    private Renderer tankRenderer;
    private Renderer sniperTurretRenderer;
    private Renderer fireTurretRenderer;
    private Renderer hatRenderer;
    private Slider currentSlider;

    [NonSerialized] public int TankClass;

    [NonSerialized] public Color newColor;

    void Awake()
    {
        // Cache default materials and create new copies for preview

        tankRenderer = tankBody.GetComponent<Renderer>();
        defaultTankMaterial = tankRenderer.material;
        newTankMaterial = new Material(defaultTankMaterial.shader);
        newTankMaterial.CopyPropertiesFromMaterial(defaultTankMaterial);
        tankRenderer.material = newTankMaterial;

        sniperTurretRenderer = sniperTurret.GetComponent<Renderer>();
        defaultSniperTurretMaterial = sniperTurretRenderer.material;
        newSniperTurretMaterial = new Material(defaultSniperTurretMaterial.shader);
        newSniperTurretMaterial.CopyPropertiesFromMaterial(defaultSniperTurretMaterial);
        sniperTurretRenderer.material = newSniperTurretMaterial;

        fireTurretRenderer = fireTurret.GetComponent<Renderer>();
        defaultFireTurretMaterial = fireTurretRenderer.material;
        newFireTurretMaterial = new Material(defaultFireTurretMaterial.shader);
        newFireTurretMaterial.CopyPropertiesFromMaterial(defaultFireTurretMaterial);
        fireTurretRenderer.material = newFireTurretMaterial;

        hatRenderer = hat.GetComponent<Renderer>();
        defaultHatMaterial = hatRenderer.material;
        newHatMaterial = new Material(defaultHatMaterial.shader);
        newHatMaterial.CopyPropertiesFromMaterial(defaultHatMaterial);
        hatRenderer.material = newHatMaterial;

        if (AchievementTracker.Instance.IsAchievementCompleted(Achievement.CompleteStageThree))
        {
            EnableHatPanel();
        }
    }

    void EnableHatPanel()
    {

        GameObject hatPanel = transform.Find("HatPanel").gameObject;
        hatPanel.SetActive(true);

        GameObject colorPanel = transform.Find("ColorPanel").gameObject;
        Vector2 panelPosition = new Vector3(-210, 0);
        colorPanel.GetComponent<RectTransform>().anchoredPosition = panelPosition;
    }

    void OnDestroy()
    {
        if (tankRenderer != null)
        {
            tankRenderer.material = defaultTankMaterial;
            sniperTurretRenderer.material = defaultSniperTurretMaterial;
            hatRenderer.material = defaultHatMaterial;
        }
    }

    void Update()
    {
        newColor = new Color(colorSliders[0].value, colorSliders[1].value, colorSliders[2].value);
        newTankMaterial.color = newColor;
        newSniperTurretMaterial.color = newColor;
        newFireTurretMaterial.color = newColor;
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
            TankClass
        );
        customizationManager.Customizations.Add(customData);
    }

    public void SetPlayerClass(float classIndex)
    {
        switch (classIndex)
        {
            case 0:
                SetSniper();
                break;
            case 1:
                SetFire();
                break;
            default:
                SetSniper();
                break;
        }

        void SetSniper()
        {
            classText.text = "Sniper";
            TankClass = 0;
            fireTurret.SetActive(false);
            sniperTurret.SetActive(true);
        }

        void SetFire()
        {
            classText.text = "Fire";
            TankClass = 1;
            fireTurret.SetActive(true);
            sniperTurret.SetActive(false);
        }
    }
}
