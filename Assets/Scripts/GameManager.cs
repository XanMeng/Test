using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    List<ISelectable> currentSelection = new List<ISelectable>();
    ITargetable currentTarget = null;

    public delegate void selectionDelegate();
    public event selectionDelegate OnBeginRectSelection = () => {};
    public event selectionDelegate OnUpdateRectSelection = () => {};
    public event selectionDelegate OnEndRectSelection = () => {};
    private Vector3 initScreenPos = Vector3.zero;
    private Vector3 currentScreenPos = Vector3.zero;
    private Vector3 lastCameraPos;

    Vector3 GetSizeSelection()
    {
        Vector2 screenUpRight = new Vector2(currentScreenPos.x, initScreenPos.y);

        Vector3 worldUpLeft = Camera.main.ScreenToWorldPoint(initScreenPos);
        Vector3 worldBottomRight = Camera.main.ScreenToWorldPoint(currentScreenPos);
        Vector3 worldUpRight = Camera.main.ScreenToWorldPoint(screenUpRight);

        Vector3 size = new Vector3();
        size.x = Vector3.Distance(worldUpLeft, worldUpRight);
        size.y = Vector3.Distance(worldUpRight, worldBottomRight);
        size.z = 100.0f;
        return size;
    }

    Vector3 GetCenterSelection()
    {
        Vector3 worldUpLeft = Camera.main.ScreenToWorldPoint(initScreenPos);
        Vector3 worldBottomRight = Camera.main.ScreenToWorldPoint(currentScreenPos);

        return (worldUpLeft + worldBottomRight)/2.0f;
    }

    void Selection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                ISelectable selectable = hit.transform.GetComponent<ISelectable>();
                if (selectable != null)
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        if (currentSelection.Contains(selectable))
                        {
                            selectable.Deselect();
                            currentSelection.Remove(selectable);
                        }
                        //if (currentSelection == selectable)
                        //{
                        //    selectable.Deselect();
                        //    currentSelection = null;
                        //}
                        else
                        {
                            selectable.Select();
                            currentSelection.Add(selectable);
                            //if (currentSelection != null)
                            //{
                            //    currentSelection.Deselect();
                            //    currentSelection = null;
                            //}
                            //selectable.Select();
                            //currentSelection = selectable;
                        }
                    }
                    else
                    {
                        if (currentSelection.Count == 1 && currentSelection[0] == selectable)
                        {
                            selectable.Deselect();
                            currentSelection.Clear();
                        }
                        else
                        {
                            foreach (ISelectable s in currentSelection)
                            {
                                s.Deselect();
                            }
                            currentSelection.Clear();

                            selectable.Select();
                            currentSelection.Add(selectable);
                        }
                    }
                }
                else
                {
                    foreach (ISelectable s in currentSelection)
                    {
                        s.Deselect();
                    }
                    currentSelection.Clear();
                    //if (currentSelection != null)
                    //{
                    //    currentSelection.Deselect();
                    //    currentSelection = null;
                    //}
                }
            }
            else
            {
                foreach (ISelectable s in currentSelection)
                {
                    s.Deselect();
                }
                currentSelection.Clear();
                //if (currentSelection != null)
                //{
                //    currentSelection.Deselect();
                //    currentSelection = null;
                //}
            }
        }
    }

    void Target()
    {
        if (currentTarget as MonoBehaviour == null)
        {
            currentTarget = null;
        }
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            ITargetable targetable = hit.transform.GetComponent<ITargetable>();
            if (targetable != null)
            {
                if (currentTarget != targetable)
                {
                    if (currentTarget != null)
                    {
                        currentTarget.Untarget();
                        currentTarget = null;
                    }
                    targetable.Target();
                    currentTarget = targetable;
                }
            }
            else
            {
                if (currentTarget != null)
                {
                    currentTarget.Untarget();
                    currentTarget = null;
                }
            }
        }
        else
        {

            if (currentTarget != null)
            {
                currentTarget.Untarget();
                currentTarget = null;
            }
        }
    }

    void Move()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                foreach (var selectec in currentSelection)
                {
                    IMovable movable = selectec as IMovable;
                    if (movable == null)
                    {
                        return;
                    }
                    if (movable != null)
                    {
                        movable.Move(hit.point);
                    }
                }
            }
        }
    }

    void RectSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnBeginRectSelection();
            initScreenPos = Input.mousePosition;

            currentSelection.ForEach(x => x.Deselect());
            //foreach (ISelectable item in currentSelection)
            //{
            //    item.Deselect();
            //}
            currentSelection.Clear();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnEndRectSelection();
        }
        else if (Input.GetMouseButton(0))
        {
            OnUpdateRectSelection();
            currentScreenPos = Input.mousePosition;
            SelectInBox();
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
    //    Gizmos.matrix = Camera.main.cameraToWorldMatrix;
    //    Vector3 pos = Camera.main.worldToCameraMatrix * GetCenterSelection();
    //    Gizmos.DrawCube(pos, GetSizeSelection());
    //}

    void SelectInBox()
    {
        Collider[] colliders = Physics.OverlapBox(GetCenterSelection(), GetSizeSelection() / 2.0f,Camera.main.transform.rotation);
        foreach (Collider item in colliders)
        {
            ISelectable selectable = item.GetComponent<ISelectable>();
            if (selectable != null)
            {
                selectable.Select();
                currentSelection.Add(selectable);
            }
            //else 
            //{
            //    foreach (ISelectable select in currentSelection)
            //    {
            //    }
            //    //selectable.Deselect();
            //    currentSelection.Clear();
            //}
        }
    }



    void Update()
    {
        currentSelection.RemoveAll(x => x as MonoBehaviour == null);

        //Selection();
        RectSelection();
        Target();
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentSelection.ForEach(selection =>
            {
                IDamageable damageable = selection as IDamageable;
                if (damageable != null)
                {
                    damageable.Damage(4);
                }
            });
        }
    }

}
