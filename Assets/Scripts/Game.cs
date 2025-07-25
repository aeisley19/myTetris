using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Game : MonoBehaviour
{
    private GameObject[,] grid;
    public static int level = 0;
    private bool gameOver = false;
    private ScoreManager scr;

    //time delay before tetrimino locks into place upon landing on floor or another tetromino.
    public void Start()
    {
        scr = GetComponent<ScoreManager>();
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

    public void DeleteAnimation()
    {

    }
    public void DeleteRows(int yAxisSquare, int numOfDeletedRows)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            Destroy(grid[i, yAxisSquare]);
            grid[i, yAxisSquare].GetComponent<TetrominoBlock>().RemoveGridPoint();
            //if parent is empty, destroy parent. cleans up unused objects.
            //if (grid[i, yAxisSquare].transform.parent.childCount <= 0) Destroy(grid[i, yAxisSquare].transform.parent.gameObject);
        }

        scr.AddToScore(numOfDeletedRows);
        AdjustRows(yAxisSquare);
    }

    public void AdjustRows(int clearedRow)
    {
        for (int y = clearedRow + 1; y < grid.GetLength(1); y++)
        {
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

    public void SetGridPositions(GameObject tetromino)
    {
        GameObject block;
        TetrominoBlock tb;
        GameObject[] allBlocksY = new GameObject[4];
        List<int> rowsToCheck = new List<int>();

        for (int i = 0; i < tetromino.transform.childCount; i++)
        {
            block = tetromino.transform.GetChild(i).gameObject;
            tb = block.GetComponent<TetrominoBlock>();
            int row = (int)tb.GetPos(0, 0).y;
            //allBlocksY[i] = block;
            tb.SetGridPoint();
            if (!rowsToCheck.Contains(row)) rowsToCheck.Add(row);
        }

        //Sorts rows in descending order to prevent issues with deleting out of order lines.
        rowsToCheck.Sort();
        rowsToCheck.Reverse();

        foreach (int y in rowsToCheck)
        {
            if (IsDeletable(y))
            {
                DeleteRows(y, rowsToCheck.Count);
            }
        }
    }
    
    public void OverlappingPieceCheck(GameObject spawnedTetromino)
    {
        GameObject block;

        for (int i = 0; i < spawnedTetromino.transform.childCount; i++)
        {
            block = spawnedTetromino.transform.GetChild(i).gameObject;
            TetrominoBlock tb = block.GetComponent<TetrominoBlock>();
            Vector2 blockPos = tb.GetPos(0, 0);

            if (grid[(int)blockPos.x, (int)blockPos.y] != null)
            {
                gameOver = true;
            }
        }
    }

    public bool isGameOver()
    {
        return gameOver;
    }
}