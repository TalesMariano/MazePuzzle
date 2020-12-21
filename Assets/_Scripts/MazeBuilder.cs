using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBuilder : MonoBehaviour
{
    [Header("References")]
    public GameObject cellPrefab;


    [Header("Settings")]
    public int mazeSide = 10;

    // Internal
    public MazeCell[,] cellsArray;

    private void Start()
    {
        CreateMaze();
    }


    [ContextMenu("CreateMaze")]
    void CreateMaze()
    {
        cellsArray = new MazeCell[mazeSide, mazeSide];

        InstantiatePrefabs();

        StartCoroutine(SetupMaze());

        // Prepare Pathfinder
        PathFinder.instance.cellsArray = cellsArray;

    }

    public Stack<Vector2> cellPath = new Stack<Vector2>();

    public Stack<Vector2> visited = new Stack<Vector2>();


    [ContextMenu("SetupMaze")]
    IEnumerator SetupMaze()
    {
        int totalCells = mazeSide * mazeSide;

        Vector2 startCell = Vector2.zero;   // Start cell = 0,0


        // End Cell = 10,10




        cellPath.Push(startCell);

        // Do loop while all cells are not visited yet
        while (visited.Count < totalCells)
        {
            
            Vector2 nextDir = NextDirectionCell((int)cellPath.Peek().x, (int)cellPath.Peek().y);

           
            if (nextDir != Vector2.zero)
            {
                //Vector2 currentCell = cellPath.Peek();

                // set path viable on Cell
                cellsArray[(int)cellPath.Peek().x, (int)cellPath.Peek().y].SetPathOpen(nextDir);


                // next cell
                cellPath.Push(cellPath.Peek() + nextDir);

                // set path viable on Cell
                cellsArray[(int)cellPath.Peek().x, (int)cellPath.Peek().y].SetPathOpen(nextDir*-1);

                //add cell to visited
                visited.Push(cellPath.Peek());

                //yield return new WaitForSeconds(0.03f);
            }
            else
            {
                cellPath.Pop();
            }

        }


        yield return null;
    }


    // Check and return direction 
    Vector2 NextDirectionCell(int x, int y) {

        List<Vector2> possibleDirections = new List<Vector2>();

        //check if north is avaliable
        if(y>0 && !visited.Contains(new Vector2(x, y - 1))){
            possibleDirections.Add(Vector2.down);
        }

        //check East
        if (x < mazeSide-1 && !visited.Contains(new Vector2(x+1, y))){
            possibleDirections.Add(Vector2.right);
        }

        //check south
        if (y < mazeSide - 1 && !visited.Contains(new Vector2(x, y +1))){
            possibleDirections.Add(Vector2.up);
        }

        //check west
        if (x > 0 && !visited.Contains(new Vector2(x - 1, y))){
            possibleDirections.Add(Vector2.left);
        }


        if(possibleDirections.Count > 0)
        {
            int random = Random.Range(0, possibleDirections.Count);
            return possibleDirections[random];  // return random direction avaliable
        }else
            return Vector2.zero;    // No direction avaliable
    }




    [ContextMenu("InstantiatePrefabs")]
    public void InstantiatePrefabs()
    {
        for (int y = 0; y < mazeSide; y++)
        {
            for (int x = 0; x < mazeSide; x++)
            {
                Vector3 pos = new Vector3(x, y,0) + transform.position;

                GameObject cell = Instantiate(cellPrefab, pos, Quaternion.identity, this.transform) as GameObject;

                cell.transform.name = "Cell " + x + " "  + y;
                cell.GetComponent<MazeCell>().x = x;
                cell.GetComponent<MazeCell>().y = y;

                cellsArray[x, y] = cell.GetComponent<MazeCell>();

            }
        }

    }



    //test
    //void testStackValues
}
