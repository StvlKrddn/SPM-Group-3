using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingEffect : MonoBehaviour
{
    [SerializeField] private float objectHeight = 1f;
    [SerializeField] private float sinkSpeed = 2f;
    [Space]
    [SerializeField] private GameObject tower;

    private Material material;
    private float height;
    private bool isHeightSet = false;

    void Awake()
    {
        material = GetComponent<Renderer>().material;
        height = material.GetFloat("CutoffHeight");
        tower.GetComponent<Renderer>().enabled = false;
    }

    private void Update()
    {
        if (transform.position.y > tower.transform.position.y)
        {
            transform.position -= Vector3.up * sinkSpeed * Time.deltaTime;
        }
        else 
        {
            transform.position = new Vector3(transform.position.x, tower.transform.position.y, transform.position.z);
            if (height < objectHeight)
            {
                height += sinkSpeed * Time.deltaTime;
                SetCutoff(height);
            }
            else
            {
                tower.GetComponent<Renderer>().enabled = true;
                transform.parent.gameObject.SetActive(false);
            }
        }
    }

    void SetCutoff(float height)
    {
        material.SetFloat("CutoffHeight", height);
    }
}
