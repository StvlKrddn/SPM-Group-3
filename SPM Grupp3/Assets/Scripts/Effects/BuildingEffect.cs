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

    void Awake()
    {
        material = GetComponent<Renderer>().material;
        height = material.GetFloat("CutoffHeight");
        tower.SetActive(false);
    }

    private void Update()
    {
        if (transform.localPosition.y > 0)
        {
            transform.localPosition -= Vector3.up * sinkSpeed * Time.deltaTime;
        }
        else 
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
            if (height < objectHeight)
            {
                height += sinkSpeed * Time.deltaTime;
                SetCutoff(height);
            }
            else
            {
                tower.SetActive(true);
                transform.parent.gameObject.SetActive(false);
            }
        }
    }

    void SetCutoff(float height)
    {
        material.SetFloat("CutoffHeight", height);
    }
}
