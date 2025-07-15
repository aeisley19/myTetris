using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTetromino : MonoBehaviour
{
    //object collider
    public Collider2D col;
    //An array of squares;
    public GameObject[] children;
    //Center of the sprite on the x and y axis.
    private float delay = 1f;
    //Enumerator that locks tetromino in place after a period of time.
    private IEnumerator lockDelayRoutine;
    //Current position of each block in tetromino.
    private float delayTimer = 0f;
    public bool isFalling;
    //Right and left edges of the screen.
    private const float RIGHTEDGE = 5f;
    private const float LEFTEDGE = -5f;
    private Game game;
    private float fallTimer = 0f;
    private float fallSpd = 1f;
    private float fastFallSpd = 0.05f;
    private Spawner spawner;
    private bool init = false;
    bool preSpawnDownInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created.
    void Start()
    {
        game = GameObject.Find("GameController").GetComponent<Game>();
        col = GetComponent<Collider2D>();
        isFalling = true;
        spawner = GameObject.Find("TetrominoSpawner").GetComponent<Spawner>();
    }
 
    void Update()
    {
        //Skips 1 frame so that the tetromino could spawn before taking the prefabs initial position (0, 0).
        if (!init)
        {
            //Is down key pressed at initialization.
            preSpawnDownInput = Input.GetKey(KeyCode.DownArrow);
            init = true;
            return;
        }

        PlayerInput();
        Fall();
    }

    private void ReleaseDownCheck()
    {
        if (Input.GetKeyUp(KeyCode.DownArrow)) preSpawnDownInput = false;
    }

    //Controls player left, right, down movement.
    private void PlayerInput()
    {
        float playerRight = col.bounds.center.x + (col.bounds.size.x / 2f);
        float playerLeft = col.bounds.center.x - (col.bounds.size.x / 2f);

        //Allows for tetromino movement if it is falling.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
        if (isFalling)
        {
            moveSingle(playerRight, playerLeft);
        }
        else
        {
            //Sets the end point position of the tetromino in the grid.
            //Game.SetGridPoints(currentPositionsX, currentPositionsY);
            gameObject.GetComponent<Rotation>().enabled = false;
            //print("collided " + currentPositionsX[0] + ", " + currentPositionsY[0]);
            // print(Game.isFilled(currentPositionsX[0], currentPositionsY[1]));

            //Ends Update method.
            enabled = false;
        }
    }

    private void moveSingle(float playerRight, float playerLeft)
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && playerRight < RIGHTEDGE && !GetSideCollision(1))
        {
            Move(Vector2.right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && playerLeft > LEFTEDGE && !GetSideCollision(-1))
        {
            Move(Vector2.left);
        }
    }

    private void Move(Vector2 direction)
    {
        transform.position += (Vector3)direction;
    }

    //Tetromino falls 1 unit every invocation unless player has reached the game floor.
    private void Fall()
    {
        if (!isFalling) return;

       // if(!get)
        LandedCheck();

        if (preSpawnDownInput) ReleaseDownCheck();

        fallTimer += Time.deltaTime;
        float currentSpd = Input.GetKey(KeyCode.DownArrow) && !GetBottomCollision() && !preSpawnDownInput ? fastFallSpd : fallSpd;

        if (fallTimer >= currentSpd)
        {
            if (!GetBottomCollision())
            {
                transform.position += Vector3.down;
            }

            fallTimer = 0f; // Reset timer after falling
        }
    }

    private void LandedCheck()
    {
        if (GetBottomCollision())
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                LockAndSpawn();
                //  print("print");
                return;
            }
            else
            {
                //// lockDelayRoutine = LockDelay(delay);
                //StartCoroutine(lockDelayRoutine);
                LockDelay(1f);
            }
        }
        else
        {
            delayTimer = 0f;
            print("here please");
        }
    }

    private void LockAndSpawn()
    {
        if (!isFalling) return;

        isFalling = false;

        game.SetGridPositions(transform.gameObject);
        gameObject.GetComponent<Rotation>().enabled = false;

        spawner.Spawn();

        enabled = false;
    }

    private bool GetBottomCollision()
    {
        bool collided = false;
        for (int i = 0; i < children.Length; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            Vector2 below = child.GetComponent<TetrominoBlock>().GetPos(0, -1);

            if (below.y > -1)
            {
                if (game.PointIsFilled(Mathf.RoundToInt(below.x), Mathf.RoundToInt(below.y)))
                {
                    // print("collided");
                    collided = true;
                    break;
                }
            }
            else
            {
                // print("collided 2");
                //  print(below);
                collided = true;
                break;
            }
        }

        return collided;
    }

    private bool GetSideCollision(int dir)
    {
        // print("here in side");
        bool col = false;
        for (int i = 0; i < children.Length; i++)
        {
            // print("aisw");
            GameObject child = transform.GetChild(i).gameObject;
            Vector2 sideCol = child.GetComponent<TetrominoBlock>().GetPos(dir, 0);

            if (sideCol.x >= 0 && sideCol.x < 10)
            {
                if (game.PointIsFilled(Mathf.RoundToInt(sideCol.x), Mathf.RoundToInt(sideCol.y)))
                {
                    col = true;
                    break;
                }
            }
        }

        return col;
    }

    //Allows tetromino to move for a number of seconds before locking in place.
    /*private IEnumerator LockDelay(float delay)
    {
        //print("not skipped");

        yield return new WaitForSeconds(delay);

        LockAndSpawn();

        lockDelayRoutine = null;
    }*/

    private void LockDelay(float delay)
    {
        delayTimer += Time.deltaTime;
        print(delayTimer);

        if (delayTimer >= delay)
        {
            LockAndSpawn();
            delayTimer = 0f;
        }

    }

    public bool GetIsFalling()
    {
        return isFalling;
    }
}
