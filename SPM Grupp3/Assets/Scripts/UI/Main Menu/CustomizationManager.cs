using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationManager : MonoBehaviour
{
    [System.NonSerialized] public List<CustomizationData> CustomizationData = new List<CustomizationData>();

    public void SaveCustomization()
    {
        DataManager.WriteToFile(CustomizationData, DataManager.CustomizationData);
        print("Customization saved!");
    }
}
