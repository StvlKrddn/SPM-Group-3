using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveExample : MonoBehaviour
{
    private DataManager dataManager;
    private string fileName = "TestFile";

    void Awake()
    {
        TestStruct test = new TestStruct();
        test.Text = "Hello World";
        test.Integer = 42;

        //DataManager.WriteToFile(test, fileName);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && DataManager.FileExists(fileName))
        {
            TestStruct test = (TestStruct) DataManager.ReadFromFile(fileName);

            print("Text: " + test.Text);
            print("Integer: " + test.Integer);
        }
    }
}

[Serializable]
public struct TestStruct
{
    public string Text;
    public int Integer;

    // NOTE(August): Vektorer är inte Serializable i Unity och kan därför inte sparas i binära filer, ska hitta en annan lösning
    //public Vector3 Position;
}
