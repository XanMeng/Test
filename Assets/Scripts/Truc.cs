using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truc : MonoBehaviour , ISelectable {

    private bool selected = false;

    public void Deselect()
    {
        selected = false;
    }

    public void Select()
    {
        selected = true;
    }

    public void Target()
    {
        transform.localScale *= 1.2f;
    }

    public void Untarget()
    {

        transform.localScale /= 1.2f;
    }

    private void Update()
    {
        if (selected)
        {
            transform.Rotate(Vector3.up, 360.0f * Time.deltaTime);
        }
    }

}
