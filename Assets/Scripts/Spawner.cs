using NUnit.Framework;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //An array of possible tetromino spawns.
    public GameObject[] tetrominos;
    public GameObject nextTetrominoDisplay;
    private GameObject nextTetrominoSpawn;
    private GameObject tetrominoSpawn;
    private NewTetromino nt;
    private MoveTetromino mv;

    public void Start()
    {
        nt = nextTetrominoDisplay.GetComponent<NewTetromino>();
        //nextTetrominoSpawn = tetrominos[Random.Range(0, 6)];
        ChooseNext(tetrominos[Random.Range(0, 6)]);
        Spawn();
    }

    public void Spawn()
    {
        tetrominoSpawn = Instantiate(nextTetrominoSpawn);
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

        //print(tetrominoSpawn.transform.position);
        //game.OverlappingPieceCheck(tetrominoSpawn);
        ChooseNext(tetrominoSpawn);
    }

    public void ChooseNext(GameObject lastSpawn)
    {

        do
        {
            int rand = Random.Range(0, 6);
            nextTetrominoSpawn = tetrominos[rand];
            nt.DisplayNextTetromino(rand);
            //print(lastSpawn == null || nextTetrominoSpawn.CompareTag(lastSpawn.tag));
        } while (nextTetrominoSpawn.CompareTag(lastSpawn.tag));

    }

    /*private void OverlappingPieceCheck()
    {
        print(tetrominoSpawn.transform.childCount);

        for (int i = 0; i < tetrominoSpawn.transform.childCount; i++)
        {
            print("i " + i);
            GameObject block = tetrominoSpawn.transform.GetChild(i).gameObject;
            TetrominoBlock tb = block.GetComponent<TetrominoBlock>();
            Vector2 blockPos = tb.GetPos(0, 0);

            print(block);
//            print(game.GetGrid()[(int)blockPos.x, (int)blockPos.y]);
            if (game.GetGrid()[(int)blockPos.x, (int)blockPos.y] != null)
            {
                //print(game.GetGrid()[(int)blockPos.x, (int)blockPos.y]);
                // print(blockPos.x);
                //print(blockPos.y);
                print("game over");
                //return;
            }
        }*/
    //}
}