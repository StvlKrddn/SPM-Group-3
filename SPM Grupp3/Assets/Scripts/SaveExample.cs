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
        dataManager = DataManager.Instance;
        TestStruct test = new TestStruct();
        test.Text = "Hello World";
        test.Integer = 42;

        dataManager.Save(test, fileName);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            TestStruct test = (TestStruct) dataManager.Load(fileName);

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
