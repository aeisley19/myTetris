using NUnit.Framework;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //An array of possible tetromino spawns.
    public GameObject[] tetrominos;
    private GameObject tetrominoSpawn;
    private MoveTetromino mv;
    Game game;

    public void Start()
    {
        game = GameObject.Find("GameController").GetComponent<Game>();
        Spawn();
    }

    public void Spawn()
    {
        int rand = Random.Range(0, 6);
        tetrominoSpawn = Instantiate(tetrominos[rand]);
        mv = tetrominoSpawn.GetComponent<MoveTetromino>();
        Collider2D col = tetrominoSpawn.GetComponent<Collider2D>();

        if (tetrominoSpawn.tag == "Long")
        {
            tetrominoSpawn.transform.position = new Vector2(0.5f, transform.position.y - col.bounds.size.y + 0.5f);
        }
        else if (tetrominoSpawn.tag == "Square")
        {
            tetrominoSpawn.transform.position = new Vector2(0, transform.position.y - col.bounds.size.y / 2);
        }
        else
        {
            tetrominoSpawn.transform.position = new Vector2(0.5f, transform.position.y - col.bounds.size.y + 1.5f);
        }
    }

    /*private void OverlappingPieceCheck()
    {
        for (int i = 0; i < tetrominoSpawn.transform.childCount; i++)
        {
            GameObject block = tetrominoSpawn.transform.GetChild(i).gameObject;
            TetrominoBlock tb = block.GetComponent<TetrominoBlock>();
            Vector2 blockPos = tb.GetPos(0, 0);

            if (game.GetGrid()[(int)blockPos.x, (int)blockPos.y] != null)
            {
                print("game over");
                return;
            }
        }
    }*/
}