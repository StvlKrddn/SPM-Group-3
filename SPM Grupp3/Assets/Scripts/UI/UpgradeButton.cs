using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update

    public Button buttonReference;
    public Text textToModify;


    public void OnPointerEnter(PointerEventData eventData)
    {
        ((IPointerEnterHandler)buttonReference).OnPointerEnter(eventData);
        textToModify.text = "Damage: 20";
        textToModify.color = Color.green; 

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ((IPointerExitHandler)buttonReference).OnPointerExit(eventData);
        textToModify.text = "Damage: 10";
        textToModify.color = Color.black;
    }

    void Start()
    {
        
    }

   

    // Update is called once per frame
    void Update()
    {


    }
}
