using UnityEngine;
using Unity.Mathematics;

public class TetrominoBlock : MonoBehaviour
{

    private int gamePosX;
    private int gamePosY;
    private Game game;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        game = GameObject.Find("GameController").GetComponent<Game>();
    }
    void Update()
    {
        gamePosX = (int)math.ceil(transform.position.x) + 4;
        gamePosY = (int)math.ceil(transform.position.y) + 9;
    }

    public Vector2 GetPos(int offSetX, int offSetY)
    {
        Vector2 pos = new Vector2(gamePosX + offSetX, gamePosY + offSetY);

        return pos;
    }

    public void SetGridPoint()
    {
        game.SetGridPoint(gamePosX, gamePosY, transform.gameObject);
    }

    public void RemoveGridPoint()
    {
        game.SetGridPoint(gamePosX, gamePosY, null);
    }

    public GameObject GetBlockFromPos(int x, int y)
    {
        return game.GetGrid()[x, y];
    }

    public void MoveDownOneBlock()
    {
        transform.position -= Vector3.down;
    }
}
