using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Game : MonoBehaviour
{
    private GameObject[,] grid;
    //Is a tetromino falling.

    //time delay before tetrimino locks into place upon landing on floor or another tetromino.
    public void Start() {
        grid = new GameObject[10, 20];

        //Build the grid array.
        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++) {
                grid[i, j] = null;
            }
        }
    }

    public GameObject[,] GetGrid() {
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

    public void IsDeletable(GameObject[] blocks)
    {
        GameObject[] deletableObjs = new GameObject[] { };

        /*for (int i = 0; i < blocks.Length; i++)
        {
            TetrominoBlock tb = blocks[i].GetComponent<TetrominoBlock>();

            //print();
            for (int j = 0; j < grid.GetLength(0); j++)
            {
                //

                //print(j + ", " + tb.GetPos(0, 0).y);
            }
        }*/
        /*bool isDeletable = false;
        GameObject[] deletableObjs = new GameObject[grid.GetLength(0)];
        Vector2 gridPos = block.GetComponent<TetrominoBlock>().GetPos(0, 0);

        print("land " + grid[(int)gridPos.x, (int)gridPos.y]);

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            GameObject currentBlock = grid[i, (int)gridPos.y];
            print(currentBlock);

            if (currentBlock == null)
            {
                isDeletable = false;
                break;
            }
            else
            {
                isDeletable = true;
            }

            deletableObjs[i] = currentBlock;
            print(deletableObjs);
            //print(grid[i, (int)block.GetComponent<TetrominoBlock>().GetPos(0, 0).y] + ", " + block.transform.position);
            /*  if (grid[i, (int)block.GetComponent<TetrominoBlock>().GetPos(0, 0).y] != null)
                {
                    deletableObjs[i] = block;
                    print("made it" + );
                }
                else
                {
                    isDeletable = false;
                    break;
                }
        }

        if (isDeletable) DeleteRows(deletableObjs);*/
    }

    public void DeleteRows(GameObject[] deletableObj) {
        foreach (GameObject i in deletableObj) {
            Destroy(i);
        }
    }
}

