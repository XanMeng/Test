using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour, ISelectable, ITargetable , IMovable
{
    [SerializeField] GameObject prefabEntity;

    [SerializeField] GameObject visual;

    [SerializeField] Transform spawPosition;

    [SerializeField] GameObject rallyPoint;

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            
            GameObject entity = Instantiate(prefabEntity, spawPosition.position, Quaternion.identity);

            entity.GetComponent<IMovable>().Move(rallyPoint.transform.position);
        }
    }

    void ISelectable.Deselect()
    {
        visual.SetActive(false);
    }

    void ISelectable.Select()
    {
        visual.SetActive(true);
    }

    void ITargetable.Target()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.red;
    }

    void ITargetable.Untarget()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.white;
    }

    void IMovable.Move(Vector3 _desination)
    {
        rallyPoint.transform.position = _desination;
    }
}
