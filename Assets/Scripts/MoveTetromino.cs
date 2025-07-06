using System.Collections;
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
    float movementTimer = 0f;
    //
    float movePause = 0f;

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

    private void moveSingle(float playerRight, float playerLeft) {
        if (Input.GetKeyDown(KeyCode.RightArrow) && playerRight < RIGHTEDGE && !GetSideCollision())
        {
            Move(Vector2.right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && playerLeft > LEFTEDGE && !GetSideCollision())
        {
            Move(Vector2.left);
        }
    }

    //Movement when arrow keys are held down.
  /*  private void MoveHoldDown(Vector2 direction)
    {
        float moveSpd = 0.5f;

        movementTimer += Time.deltaTime;

        if (movementTimer >= moveSpd)
        {
            Move(direction);
            movementTimer = 0f;
        }
    }

    private IEnumerator MovePause()
    {
        yield return 0.5;
    }
*/
    private void Move(Vector2 direction)
    {
        transform.position += (Vector3)direction;
    }

    //Tetromino falls 1 unit every invocation unless player has reached the game floor.
    private void Fall()
    {
        if (!isFalling) return;

        LandedCheck();

        if (preSpawnDownInput) ReleaseDownCheck();

        fallTimer += Time.deltaTime;
        float currentSpd = Input.GetKey(KeyCode.DownArrow) && !GetBottomCollision() && !preSpawnDownInput ? fastFallSpd : fallSpd;

        print(fallTimer);

        if (fallTimer >= currentSpd)
        {
            if (!GetBottomCollision())
            {
                transform.position += Vector3.down;
            }
            else
            {
                if (lockDelayRoutine == null)
                {
                    //lockDelayRoutine = LockDelay(delay);
                    //StartCoroutine(lockDelayRoutine);
                    //print("locked");
                    //LockAndSpawn();
                }
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
                lockDelayRoutine = LockDelay(delay);
                StartCoroutine(lockDelayRoutine);
            }
        }
    }

    private void LockAndSpawn()
    {
        if (!isFalling) return; // prevent double calls
        isFalling = false;

        setGridPositions();
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

    private bool GetSideCollision()
    {
       // print("here in side");
        bool col = false;
        for (int i = 0; i < children.Length; i++)
        {
           // print("aisw");
            GameObject child = transform.GetChild(i).gameObject;
            Vector2 right = child.GetComponent<TetrominoBlock>().GetPos(1, 0);
            Vector2 left = child.GetComponent<TetrominoBlock>().GetPos(-1, 0);
            print(game.PointIsFilled(Mathf.RoundToInt(right.x), Mathf.RoundToInt(right.y)));

            if (left.x > 0 && right.x < 9)
            {
                if (game.PointIsFilled(Mathf.RoundToInt(right.x), Mathf.RoundToInt(right.y)) ||
                     game.PointIsFilled(Mathf.RoundToInt(left.x), Mathf.RoundToInt(left.y)))
                {
                    col = true;
                    break;
                }
            }
        }

        return col;
    }

    //Allows tetromino to move for a number of seconds before locking in place.
    private IEnumerator LockDelay(float delay)
    {
        //print("not skipped");

        yield return new WaitForSeconds(delay);

        if (GetBottomCollision())
        {
            LockAndSpawn();
        }

        lockDelayRoutine = null;
    }

    private void setGridPositions()
    {
        GameObject block;
        TetrominoBlock tb;
        GameObject[] allBlocksY = new GameObject[4];

        for (int i = 0; i < transform.childCount; i++)
        {
            block = transform.GetChild(i).gameObject;
            tb = block.GetComponent<TetrominoBlock>();
            allBlocksY[i] = block;
            tb.SetGridPoint();
        }

        game.IsDeletable(allBlocksY);
    }

    public bool GetIsFalling()
    {
        return isFalling;
    }
}
