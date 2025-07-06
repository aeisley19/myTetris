using Unity.Mathematics;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private Vector2 belowPos;

    private int gamePosX;
    private int gamePosY;
    private Game game;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        game = GameObject.Find("GameController").GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        belowPos = new Vector2(transform.position.x, transform.position.y - 1);
    }

    public bool Collided()
    {
        return game.PointIsFilled(Mathf.RoundToInt(belowPos.x + 9), Mathf.RoundToInt(belowPos.y + 4));
    }

    public Vector2 getGamePos()
    {
        return new Vector2(gamePosX, gamePosY);
    }
}
