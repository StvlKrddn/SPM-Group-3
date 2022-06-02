using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationManager : MonoBehaviour
{
    [System.NonSerialized] public List<CustomizationData> Customizations = new List<CustomizationData>();

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    private CustomizationMenu player1Customizations;
    private CustomizationMenu player2Customizations;

    void Awake()
    {
        player1Customizations = player1.GetComponent<CustomizationMenu>();
        player2Customizations = player2.GetComponent<CustomizationMenu>();
    }

    public void SaveCustomization()
    {
        DataManager.WriteToFile(Customizations, DataManager.CustomizationData);
        print("Customization saved!");
    }

    void Update()
    {
        switch (player1Customizations.TankClass)
        {
            case 0:
                player2Customizations.SetPlayerClass(1);
                break;
            case 1:
                player2Customizations.SetPlayerClass(0);
                break;
        }
    }
}
