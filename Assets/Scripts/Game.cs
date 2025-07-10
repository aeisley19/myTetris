using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Game : MonoBehaviour
{
    private GameObject[,] grid;
    //Is a tetromino falling.
    GameObject[] blockRow = new GameObject[10];

    //time delay before tetrimino locks into place upon landing on floor or another tetromino.
    public void Start()
    {
        grid = new GameObject[10, 20];

        //Build the grid array.
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j] = null;
            }
        }
    }

    public GameObject[,] GetGrid()
    {
        return grid;
    }

    /*public void SetGridPoints(int[] x, int[] y)
    {
        for (int i = 0; i < 4; i++)
        {
            grid[x[i], y[i]] = null;
        }
    }*/

    public void SetGridPoint(int x, int y, GameObject val)
    {
        grid[x, y] = val;
    }

    public bool PointIsFilled(int x, int y)
    {
        //wierd, but I need to check that the x axis is valid before checking array to prevent error. Should have predicted this.
        return x < 10 && x >= 0 && y < 20 && grid[x, y] != null;
    }

    public bool CollisionCheck(int x, int y, GameObject val)
    {
        print("position " + x + ", " + y + " --- " + val);
        return grid[x, y] == val;
    }

    public bool IsDeletable(int blockYPos)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            if (grid[i, blockYPos] == null)
            {
                return false;
            }
        }

        return true;
    }

    public void DeleteRows(int yAxisSquare)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            //            print("destroy");
            Destroy(grid[i, yAxisSquare]);
            grid[i, yAxisSquare].GetComponent<TetrominoBlock>().RemoveGridPoint();
        }

        AdjustRows(yAxisSquare);
    }

    /*public void MoveOneRowDown(int yAxisSquare)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            if (yAxisSquare - 1 > 0 && grid[i, yAxisSquare] != null)
            {

                grid[i, yAxisSquare - 1] = grid[i, yAxisSquare];
                //grid[i, yAxisSquare].GetComponent<TetrominoBlock>().RemoveGridPoint();
                // grid[i, yAxisSquare - 1].GetComponent<TetrominoBlock>().SetGridPoint();
                print(grid[i, yAxisSquare]);
                grid[i, yAxisSquare].transform.position += new Vector3(0, -1);
            }
        }
    }*/

    public void AdjustRows(int clearedRow)
    {
        for (int y = clearedRow+1; y < grid.GetLength(1); y++)
        {
            //            MoveOneRowDown(i);
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;
                    grid[x, y - 1].transform.position += new Vector3(0, -1, 0);
                }
            }
        }
    }
}

