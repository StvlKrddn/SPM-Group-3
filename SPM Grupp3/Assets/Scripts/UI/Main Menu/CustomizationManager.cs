using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationManager : MonoBehaviour
{
    [SerializeField] private CustomizationMenu player1Panel;
    [SerializeField] private CustomizationMenu player2Panel;

    public List<CustomizationData> CustomizationData = new List<CustomizationData>();

    public void SaveCustomization()
    {
        DataManager.WriteToFile(CustomizationData, DataManager.CustomizationData);
        print("Customization saved!");
    }
}
