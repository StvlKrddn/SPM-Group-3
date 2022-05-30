using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialHolder : MonoBehaviour
{
	private List<GameObject> materials = new List<GameObject>();
    [SerializeField] private GameObject material;

    public GameObject GiveMaterial(Vector3 position, Quaternion rotation)
    {
        int materialIndex = FindEmptyPool();
        if (materialIndex < 0)
        {
            GameObject tempMaterial = Instantiate(material, position, rotation, transform);
            materials.Add(tempMaterial);
            return tempMaterial;
        }
        else
        {
            materials[materialIndex].transform.position = position;
            materials[materialIndex].transform.rotation = rotation;
            materials[materialIndex].SetActive(true);
            return materials[materialIndex];
        }
    }

    private int FindEmptyPool()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            if (materials.Count > i && materials[i].activeSelf == false)
            {
                return i;
            }
        }
        return -1;
    }
}
