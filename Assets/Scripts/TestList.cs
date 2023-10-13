using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestList : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BadGuy bg1 = new BadGuy("bob",10);
        BadGuy bg2 = new BadGuy("sue", 20);
        BadGuy bg3 = new BadGuy("raj",30 );

        List<BadGuy> badguys = new List<BadGuy>();

        badguys.Add(bg1);
        badguys.Add(bg2);
        badguys.Add(bg3);

        print("List Size:" + badguys.Count);

        foreach(BadGuy guy in badguys)
        {
            print(guy.name + " " + guy.power);
        }

        badguys.Clear();
        print("List Size:" + badguys.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
