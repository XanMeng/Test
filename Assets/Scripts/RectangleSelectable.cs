using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleSelectable : MonoBehaviour
{
    private bool isSelect = false;
    private Vector3 mousePos;
    static Texture2D texture2D;

    void Start()
    {
        texture2D = new Texture2D(1, 1);
        texture2D.SetPixel(0, 0, Color.white);
        texture2D.Apply();
    }

    public static void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, texture2D);
        GUI.color = Color.white;
    }

    public static Rect GetScreenPoint(Vector3 pos1,Vector3 pos2)
    {
        pos1.y = Screen.height - pos1.y;
        pos2.y = Screen.height - pos2.y;

        Vector3 topLeft = Vector3.Min(pos1, pos2);
        Vector3 bottomRight = Vector3.Max(pos1, pos2);

        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isSelect = true;
            mousePos = Input.mousePosition;
        }


        if (Input.GetMouseButtonUp(0))
        {
            isSelect = false;
        }
    }
    void OnGUI()
    {
        if (isSelect)
        {
            Rect rect = GetScreenPoint(mousePos, Input.mousePosition);
            DrawScreenRect(rect, new Color(1.0f, 1.0f, 1.0f, 0.25f));
        }
        //GUI.Box(new Rect(0, 0, 100, 20),Color.white);
    }

}
