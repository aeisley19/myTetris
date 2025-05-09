using UnityEngine;

public class Game : MonoBehaviour
{
    private static int[,] grid;

    public void Start () {
        grid = new int[10, 20];
        print(grid.GetLength(0));
        print(grid.GetLength(1));
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

    public static void SetGridPoints(int[] x, int[] y) {
        for(int i=0; i < 4; i++) {
            grid[x[i], y[i]] = 1;
        }
    }
}

