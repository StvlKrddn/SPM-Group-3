using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingEffect : MonoBehaviour
{
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

    public IEnumerator PlayEffect()
    {
        bool finished = false;
        while(!finished)
        {
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
                else
                {
                    finished = true;


                    Destroy(gameObject);

                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void SetCutoff(float height)
    {
        material.SetFloat("CutoffHeight", height);
    }
}
