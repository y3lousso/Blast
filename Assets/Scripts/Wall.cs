using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : SpawnableObject
{
    public override void DestroyObject()
    {
        Destroy(gameObject);
    }

}
