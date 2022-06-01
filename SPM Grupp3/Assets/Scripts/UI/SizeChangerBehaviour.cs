using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeChangerBehaviour : MonoBehaviour
{

    public float duration = 0.4f;

    public float sizeMultiplier = 5;
    // Start is called before the first frame update

    public IEnumerator DoIncrease()
    {
        Vector3 originalScale = gameObject.transform.localScale;
        Vector3 destinationScale = originalScale * sizeMultiplier;

        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            gameObject.transform.localScale = Vector3.Lerp(originalScale, destinationScale, counter / duration);

            yield return null;
        }
    }
}
