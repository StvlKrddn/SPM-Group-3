using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingEffect : MonoBehaviour
{
    [SerializeField] private float noiseStrength = 0.5f;
    [SerializeField] private float objectHeight = 1f;
    [SerializeField] private float sinkSpeed = 2f;

    private Material material;
    private float height;
    private bool isHeightSet = false;

    void Awake()
    {
        material = GetComponent<Renderer>().material;
        height = material.GetFloat("CutoffHeight");
    }

    void Update()
    {
        material.SetFloat("NoiseStrength", noiseStrength);

        if (transform.position.y > 0)
            {
                transform.position -= Vector3.up * sinkSpeed * Time.deltaTime;
            }
            else 
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                if (height < objectHeight)
                {
                    height += sinkSpeed * Time.deltaTime;
                    SetCutoff(height);
                }
            }

    }

    void SetCutoff(float height)
    {
        material.SetFloat("CutoffHeight", height);
    }
}
