using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyIuserInput : IUserInput
{
    public GameObject player;
    public GameObject model;

    // Start is called before the first frame update
    void Start()
    {
        dup = 0;
        dright = 0;
        jup = 0;
        jright = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, model.transform.position);
        if (distance > 3.1)
        {
            run = true;
            attack = false;
            targetDmag = 1;
        }
        else if (distance <= 3)
        {
            run = false;
            attack = true;
            targetDmag = 0;
        }
    }
}
