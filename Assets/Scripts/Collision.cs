using Unity.Mathematics;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private Vector2 belowPos;

    private int gamePosX;
    private int gamePosY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        belowPos = new Vector2(transform.position.x, transform.position.y - 1);
        print(belowPos);
    }

    public bool Collided()
    {
        return Game.grid[(int)math.ceil(belowPos.x) + 9, (int)math.ceil(belowPos.y + 4)] == 1;
    }

    public Vector2 getGamePos()
    {
        return new Vector2(gamePosX, gamePosY);
    }
}
