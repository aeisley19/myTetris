using UnityEngine;
using Unity.Mathematics;

public class TetrominoBlock : MonoBehaviour
{

    private int gamePosX;
    private int gamePosY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        gamePosX = (int) math.ceil(transform.position.x) + 4;
        gamePosY = (int) math.ceil(transform.position.y) + 9;
    }

    public Vector2 GetPos(int offSetX, int offSetY)
    {
        Vector2 belowPos = new Vector2(gamePosX + offSetX, gamePosY + offSetY);

        return belowPos;
    }

    public void SetGridPoint()
    {
        Game.grid[gamePosX, gamePosY] = 1;
    }

    public void RemoveGridPoint()
    {
        Game.grid[gamePosX, gamePosY] = 0;
    }

}
