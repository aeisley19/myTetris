using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveTetromino : MonoBehaviour
{
    //object collider
    public Collider2D col;
    //An array of squares;
    public GameObject[] children;
    private float delayTimer = 0f;
    public bool isFalling;
    //Right and left edges of the screen.
    private const float RIGHTEDGE = 5f;
    private const float LEFTEDGE = -5f;
    private Game game;
    private float framesSinceLastFall;
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
            game.OverlappingPieceCheck(transform.gameObject);
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

        framesSinceLastFall ++;
        float framesTillFall = Input.GetKey(KeyCode.DownArrow) && !preSpawnDownInput ? 2 : SetFallSpd();

        if (framesSinceLastFall >= framesTillFall)
        {
            if (!GetBottomCollision())
            {
                transform.position += Vector3.down;
            }

            framesSinceLastFall = 0f; // Reset timer after falling
        }
    }

    private int SetFallSpd()
    {
        switch (ScoreManager.Level)
        {
            case 0: return 48;
            case 1: return 43;
            case 2: return 38;
            case 3: return 33;
            case 4: return 28;
            case 5: return 23;
            case 6: return 18;
            case 7: return 13;
            case 8: return 8;
            case 9: return 6;
            case 10:
            case 11:
            case 12:
                return 5;
            case 13:
            case 14:
            case 15:
                return 4;
            case 16:
            case 17:
            case 18:
                return 3;
            case 19:
            case 20:
            case 21:
            case 22:
            case 23:
            case 24:
            case 25:
            case 26:
            case 27:
            case 28:
                return 2;
            case 29:
                return 1;
        }

        return 0;
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
        }
    }

    private void LockAndSpawn()
    {
        if (!isFalling) return;

        isFalling = false;

        game.SetGridPositions(transform.gameObject);
        gameObject.GetComponent<Rotation>().enabled = false;

        if (!game.isGameOver()) spawner.Spawn();

        RemoveParent();
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

    private void LockDelay(float delay)
    {
        delayTimer += Time.deltaTime;

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
    
    public void RemoveParent () {
        print("here " + gameObject.transform.childCount);
        /* for (int i = 0; i < gameObject.transform.childCount; i++)
         {
             gameObject.transform.GetChild(i).transform.parent = null;
             print(i);
             print(gameObject.transform.GetChild(i));
         }*/

        gameObject.transform.DetachChildren();
        Destroy(gameObject);
    }
}