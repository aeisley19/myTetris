using NUnit.Framework;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //An array of possible tetromino spawns.
    public GameObject[] tetrominos;

    //Spawn position.
    private Vector2 startPos;
    private GameObject tetrominoSpawn;
    private MoveTetromino mv;

    public void Start()
    {
        Spawn();
    }

    public void Update() {
        if(!mv.GetIsFalling()) {
            Spawn();
        }
    }

    public void Spawn() {
        int rand = Random.Range(0, 6);
        tetrominoSpawn = Instantiate(tetrominos[1]);
        mv = tetrominoSpawn.GetComponent<MoveTetromino>();
        Collider2D col = tetrominoSpawn.GetComponent<Collider2D>();

        if(tetrominoSpawn.tag == "Long") {
            tetrominoSpawn.transform.position = new Vector2(0.5f, transform.position.y - col.bounds.size.y + 0.5f);
        } else if(tetrominoSpawn.tag == "Square") {
            tetrominoSpawn.transform.position = new Vector2(0, transform.position.y - col.bounds.size.y/2);
        } else {
            tetrominoSpawn.transform.position = new Vector2(0.5f, transform.position.y - col.bounds.size.y + 1.5f);
        }

        //tetrominoSpawn.transform.position = startPos;
    }
}