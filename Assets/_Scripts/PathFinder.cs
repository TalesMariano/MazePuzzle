// Pathfinder based on A*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFinder : MonoBehaviour
{
   
    public int endX,endY;
    //public Vector2 startPos;


    // Singleton

    public static PathFinder instance;

    public MazeCell[,] cellsArray;
    Node[,] nodesArray;



    List<Node> open;
    List<Node> closed;


    Vector2[] direction = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

    private void Awake()
    {
        instance = this;
    }



    public void FindThePath(int x, int y)
    {
        

        open = new List<Node>();
        closed = new List<Node>();


        //Start nodes
        nodesArray = new Node[cellsArray.GetLength(0), cellsArray.GetLength(1)];

        for (int yy = 0; yy < nodesArray.GetLength(1); yy++)
        {
            for (int xx = 0; xx < nodesArray.GetLength(0); xx++)
            {

                nodesArray[xx, yy] = new Node(xx, yy);
                
            }
        }



        //cellValues = new int[cellsArray.GetLength(0), cellsArray.GetLength(1)];
        //closed = new bool[cellsArray.GetLength(0), cellsArray.GetLength(1)];

        nodesArray[x, y].CalculateCosts(endX, endY, x,y);

        open.Add(nodesArray[x,y]);

        print(open.Any());

        while (open.Any())
        {
            Node current = open.OrderBy(a => a.fCost).First();
            closed.Add(current);
            //cellsArray[current.x, current.y].PaintRed();

            open.Remove(current);

            //check if end
            if(current.x == endX && current.y == endY)
            {
                //end here

                PaintPath(current);

                return;
            }



            //For each neighbour
            for (int i = 0; i < 4; i++)
            {
                //check if traversable
                if (!cellsArray[current.x, current.y].IsPathOpen(direction[i]))
                    continue;

                print(nodesArray[1, 1]);

                Node neighbour = nodesArray[current.x + (int)direction[i].x, current.y + (int)direction[i].y];

                //check if neighbour is closed
                if (closed.Contains(neighbour))
                    continue;

                int tentative_gCost = current.gCost + 1;    

                if(tentative_gCost < neighbour.gCost)
                {
                    neighbour.previous = current;
                    neighbour.CalculateHCost(endX, endY);
                    neighbour.gCost = tentative_gCost;
                    neighbour.fCost = neighbour.hCost + current.gCost;

                    if (!open.Contains(neighbour))
                    {
                        open.Add(neighbour);
                        //cellsArray[neighbour.x, neighbour.y].PaintGreen();
                    }
                        
                }
            }

        }


    }


    void PaintPath(Node endNode)
    {
        Node current = endNode;

        while(current.previous != null)
        {
            cellsArray[current.x, current.y].PaintBlue();

            current = current.previous;

        }


    }



}

[System.Serializable]
class Node
{
    public Node(int _x, int _y)
    {
        x = _x;
        y = _y;
        gCost = int.MaxValue;
        hCost = int.MaxValue;
        fCost = int.MaxValue;
    }

    public int x, y;

    public Node previous;

    public int gCost, hCost, fCost;

    public void CalculateHCost(int endX, int endY)
    {
        hCost = Mathf.Abs(x - endX) + Mathf.Abs(y - endY);
    }

    public void CalculateCosts(int endX, int endY, int startX, int startY )
    {
        gCost = Mathf.Abs(x - startX) + Mathf.Abs(y - startY);
        hCost = Mathf.Abs(x - endX) + Mathf.Abs(y - endY);
        fCost = gCost + hCost;
    }

}
