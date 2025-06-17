using UnityEngine;

public class Game : MonoBehaviour
{
    public static int[,] grid;
    //Is a tetromino falling.

    //time delay before tetrimino locks into place upon landing on floor or another tetromino.
    public void Start () {
        grid = new int[10, 20];

        //Build the grid array.
        for(int i=0; i < grid.GetLength(0); i++) {
            for(int j=0; j < grid.GetLength(1); j++) {
                grid[i, j] = 0;
            }
        }
    }

    public static int[,] GetGrid() {
        return grid;
    }


    //Checks if a tetromino is located at a given position or not.
    public static int isFilled(int x, int y) {
        return grid[x, y];
    }

    public static void SetGridPoints(int[] x, int[] y) {
        for(int i=0; i < 4; i++) {
            grid[x[i], y[i]] = 0;
        }
    }
}

