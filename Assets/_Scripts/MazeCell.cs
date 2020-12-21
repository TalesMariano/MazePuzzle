using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [Header("References")]
    [Tooltip("0 - North, 1- East, 2- South, 3-West")]
    public SpriteRenderer[] walls;
    public SpriteRenderer floor;


    [Header("Info")]
    public int x, y;

    [Tooltip("0 - North, 1- East, 2- South, 3-West")]
    public bool[] paths = new bool[4];


   


    public void SetPathOpen(Vector2 dir)
    {
        switch (dir)
        {
            case Vector2 v when v.Equals(Vector3.up):
                paths[0] = true;
                break;
            case Vector2 v when v.Equals(Vector3.down):
                paths[2] = true;
                break;
            case Vector2 v when v.Equals(Vector3.left):
                paths[3] = true;
                break;
            case Vector2 v when v.Equals(Vector3.right):
                paths[1] = true;
                break;

        }

        UpdVisualWall();
    }


    public bool IsPathOpen(Vector2 dir)
    {
        switch (dir)
        {
            case Vector2 v when v.Equals(Vector3.up):
                return paths[0];
                
            case Vector2 v when v.Equals(Vector3.down):
                return paths[2];
                
            case Vector2 v when v.Equals(Vector3.left):
                return paths[3];
                
            case Vector2 v when v.Equals(Vector3.right):
                return paths[1];

            default:
                return false;

        }
    }


    void UpdVisualWall()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].enabled = !paths[i];
        }
    }


    public void PaintGreen()
    {
        floor.color = Color.green;
    }

    public void PaintRed()
    {
        floor.color = Color.red;
    }

    public void PaintBlue()
    {
        floor.color = Color.blue;
    }


    private void OnMouseDown()
    {
        print(x + " " + y);
        //PaintGreen();

        PathFinder.instance.FindThePath(x, y);
    }
}

enum Directions
{
    North = 1,
    East = 2,
    South = 4,
    West= 8
}
