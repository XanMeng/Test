using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasIG : MonoBehaviour {

    [SerializeField] Image selectionBox;
    GameManager gm;
    Camera cam;

	// Use this for initialization
	void Start ()
    {
        gm = FindObjectOfType<GameManager>();
        gm.OnBeginRectSelection += BeginRectSelect;
        gm.OnUpdateRectSelection += UpdateRectSelect;
        gm.OnEndRectSelection += EndRectSelect;
        cam = Camera.main;
    }
	
    Vector2 GetCanvasPosition(Vector2 _screenPos)
    {
        Vector2 canvasPosition = Vector2.zero;
        canvasPosition.x = _screenPos.x / Screen.width *  GetComponent<RectTransform>().sizeDelta.x;
        canvasPosition.y = _screenPos.y / Screen.height * GetComponent<RectTransform>().sizeDelta.y;
        return canvasPosition;
    }

    Vector3 initWorldPos;

    void BeginRectSelect()
    {
        selectionBox.gameObject.SetActive(true);
        //Debug.Log("Begin : " + GetCanvasPosition());
        //selectionBox.rectTransform.anchoredPosition = GetCanvasPosition();
        //initAnchoredPos = GetCanvasPosition();
        //initWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        initWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void UpdateRectSelect()
    {
        Vector2 initCanvasPos = GetCanvasPosition(cam.WorldToScreenPoint(initWorldPos)); 
        //Debug.Log("Uptade : " + GetCanvasPosition());
        Vector2 currentPos = GetCanvasPosition(Input.mousePosition);
        //selectionBox.rectTransform.anchoredPosition = (currentPos + initAnchoredPos) / 2.0f;
        selectionBox.rectTransform.anchoredPosition = (currentPos + initCanvasPos) / 2.0f;
        //selectionBox.rectTransform.sizeDelta = GetCanvasPosition() - selectionBox.rectTransform.anchoredPosition;
        //Vector2 size = currentPos - initAnchoredPos;
        Vector2 size = currentPos - initCanvasPos;
        size.x = Mathf.Abs(size.x);
        size.y = Mathf.Abs(size.y);
        selectionBox.rectTransform.sizeDelta = size;
    }

    void EndRectSelect()
    {
        selectionBox.gameObject.SetActive(false);
        selectionBox.rectTransform.sizeDelta = Vector2.zero;
        //Debug.Log("End : " + GetCanvasPosition());
    }

	//// Update is called once per frame
	//void Update ()
 //   {
 //       BeginRectSelect();
 //       UpdateRectSelect();
 //       EndRectSelect();
 //   }
}
