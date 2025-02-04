using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private Vector3 borderMax;
    [SerializeField] private Vector3 borderMin;
    
    [SerializeField] private Texture2D cursorTextureHold;
    [SerializeField] private Vector3 leftBorder;
    [SerializeField] private Vector3 topBorder;
    [SerializeField] private Vector3 rightBorder;
    [SerializeField] private Vector3 bottomBorder;

    private float _currentX, _currentY;

    private float _maxX, _minX, _x;
    private float _maxY, _minY, _y;

    private Plane plane;
    private Camera _camera;

    private Vector3 newPosition;
    private Vector3 dragStart;
    private Vector3 dragCurrent;

    private Vector2 _vectorHotspot;

    public static bool IsMove;
    private bool _isLimitDrag;

    private void Start()
    {
        // ZoomCamera.Zooming += UpdateBorder;

        plane = new Plane(Vector3.up, Vector3.zero);
        _camera = Camera.main;

        newPosition = transform.position;

        _vectorHotspot = new Vector2(cursorTextureHold.width * 0.5f, cursorTextureHold.height * 0.5f);
    }

    void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Cursor.SetCursor(cursorTextureHold, _vectorHotspot, CursorMode.ForceSoftware);
            IsMove = true;
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out var enter))
            {
                dragStart = ray.GetPoint(enter);
                // Debug.Log(dragStart);
            }
        }
        
        if (Input.GetMouseButton(2))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out var enter))
            {
                dragCurrent = ray.GetPoint(enter);
                // Debug.Log(dragCurrent);

                newPosition = transform.position + (dragStart - dragCurrent);
                
                // Debug.Log(newPosition);
            }

            
        }

        var borderDrag = MinMaxDrag(
            newPosition, 
            leftBorder, 
            topBorder,
            rightBorder,
            bottomBorder
            );

        if (Input.GetMouseButtonUp(2))
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
            IsMove = false;
        }

        if (!_isLimitDrag)
        {
            transform.DOMove(borderDrag, speed);
        }
        else
        {
            newPosition = borderDrag;
        }
    }

    private void HandleMouseOld()
    {
        // //Detect if the middle mouse button is pressed
        // if (Input.GetKeyDown(KeyCode.Mouse2))
        // {
        //     Vector3 mousePos = Input.mousePosition;
        //     
        //     _maxX = Screen.width - (mousePos.x + mousePos.x * 0.5f);
        //     _minX = 0 - (mousePos.x + mousePos.x * 0.5f);
        //     _x = mousePos.x;
        //     
        //     _maxY = Screen.height - (mousePos.y + mousePos.y * 0.5f);
        //     _minY = 0 - (mousePos.y + mousePos.y * 0.5f);
        //     _y = mousePos.y;
        // }
        //
        // if (Input.GetKey(KeyCode.Mouse2))
        // {
        //     
        //     Vector3 mousePos = Input.mousePosition;
        //     // var x = (mousePos.x - Screen.width * 0.5f) / (Screen.width * 0.5f) * scale;
        //     // var y = (mousePos.y - Screen.height * 0.5f) / (Screen.height * 0.5f) * scale;
        //
        //     var x = (mousePos.x - _x) / (_maxX-_minX);
        //     var y = (mousePos.y - _y) / (_maxY-_minY);
        //     
        //     {
        //         Debug.Log(new Vector2(x, y));
        //     }
        //     
        //     transform.Translate(Vector3.right * (x * Time.deltaTime * speed), Space.World);
        //     transform.Translate(Vector3.forward * (y * Time.deltaTime * speed), Space.World);
        //
        //     var position = transform.position;
        //     if (position.x < borderMin.x)
        //     {
        //         position.x = borderMin.x;
        //     }
        //
        //     if (position.z < borderMin.z)
        //     {
        //         position.z = borderMin.z;
        //     }
        //
        //     if (position.x > borderMax.x)
        //     {
        //         position.x = borderMax.x;
        //     }
        //
        //     if (position.z > borderMax.z)
        //     {
        //         position.z = borderMax.z;
        //     }
        //
        //     transform.position = position;
        // }
    }

    private Vector3 MinMaxDrag(Vector3 current, Vector3 left, Vector3 top, Vector3 right, Vector3 bottom)
    {
        // var result = Mathf.cl
        
        return current;
    }

    private void UpdateBorder(float scroll)
    {
        scroll *= 0.5f;
        
        borderMax.x += scroll;
        borderMax.z += scroll;
        
        borderMin.x -= scroll;
        borderMin.z -= scroll;
    }
}
