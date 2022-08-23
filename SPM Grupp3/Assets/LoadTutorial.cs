using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void loadTutorial()
    {
        SceneManager.LoadScene("Omgjord tutorial");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
