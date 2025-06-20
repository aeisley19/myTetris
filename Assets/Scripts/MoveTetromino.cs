using System.Collections;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class MoveTetromino : MonoBehaviour
{
    //The starting position of the player piece.
    public Vector2 startPos;
    //cooldown time between "jumps" from one grid space to the other. Decrease for increased difficulty.
    public float coolDown;
    //object collider
    public Collider2D col;
    //An array of squares;
    public GameObject[] children;
    //Center of the sprite on the x and y axis.
    private float delay = 0.5f;
    //Enumerator that locks tetromino in place after a period of time.
    private IEnumerator lockDelayRoutine;
    //Current position of each block in tetromino.
    public static int[] currentPositionsX = new int[4];
    public static int[] currentPositionsY = new int[4];
    public static bool isFalling;
    //Right and left edges of the screen.
    private const float RIGHTEDGE = 5f;
    private const float LEFTEDGE = -5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created.
    void Start()
    {
        col = GetComponent<Collider2D>();
        isFalling = true;

        if (isFalling)
        {
            InvokeRepeating("Fall", 1f, 1f);
        }
    }

    void Update()
    {
        PlayerInput();
    }

    //Controls player left, right, down movement.
    private void PlayerInput()
    {
        float playerRight = col.bounds.center.x + (col.bounds.size.x / 2f);
        float playerLeft = col.bounds.center.x - (col.bounds.size.x / 2f);

        //Allows for tetromino movement if it is falling.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
        if (isFalling)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && playerRight < RIGHTEDGE && !getSideCollision()) 
            {
                Move(Vector2.right);
                
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && playerLeft > LEFTEDGE && !getSideCollision())
            {
                Move(Vector2.left);
                //print(playerLeft);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(Vector2.down);
            }
        }
        else
        {
            //Cancels falling invokation.
            CancelInvoke("Fall");

            //Sets the end point position of the tetromino in the grid.
            //Game.SetGridPoints(currentPositionsX, currentPositionsY);
            gameObject.GetComponent<Rotation>().enabled = false;
            //print("collided " + currentPositionsX[0] + ", " + currentPositionsY[0]);
            // print(Game.isFilled(currentPositionsX[0], currentPositionsY[1]));

            //Ends Update method.
            enabled = false;
        }
    }

    private void Move(Vector2 direction)
    {
        transform.position += (Vector3)direction;
    }

    //Tetromino falls 1 unit every invocation unless player has reached the game floor.
    private void Fall()
    {
        bool collided = getBottomCollision();

        //World position of the bottom of collider.
        float bottom = col.bounds.center.y - (col.bounds.size.y / 2f);

        //print("collided " + collided);
        //tetrimino stops falling once it hits the floor.
        if (collided == false)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 1);
        }
        else
        {
            lockDelayRoutine = LockDelay(delay);
            StartCoroutine(lockDelayRoutine);
        }
    }

    private bool getBottomCollision()
    {
        bool collided = false;
        for (int i = 0; i < children.Length; i++)
        {
            Vector2 below = transform.GetChild(i).gameObject.GetComponent<TetrominoBlock>().GetPos(0, -1);

            if (below.y > -1)
            {
                if (Game.grid[(int)below.x, (int)below.y] == 1)
                {
                    collided = true;
                    break;
                }
            }
            else {
                    collided = true;
                    break;
            }
        }

        return collided;
    }

    private bool getSideCollision()
    {
        bool col = false;
        for (int i = 0; i < children.Length; i++)
        {
            Vector2 right = transform.GetChild(i).gameObject.GetComponent<TetrominoBlock>().GetPos(1, 0);
            Vector2 left = transform.GetChild(i).gameObject.GetComponent<TetrominoBlock>().GetPos(-1, 0);

            if (left.x > 0 && right.x < 10)
            {
                if (Game.grid[(int)left.x, (int)left.y] == 1 || Game.grid[(int)right.x, (int)right.y] == 1)
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
        yield return new WaitForSeconds(delay);

        if(getBottomCollision()) {
            isFalling = false;
            setGridPositions();
        }
    }

    private void setGridPositions()
    {
        GameObject block;
        TetrominoBlock tb;
        int[] allBlocksY = new int[4];

        for (int i = 0; i < transform.childCount; i++)
        {
            block = transform.GetChild(i).gameObject;
            tb = transform.GetChild(i).gameObject.GetComponent<TetrominoBlock>();
            allBlocksY[i] = (int) tb.GetPos(0, 0).x;
            tb.SetGridPoint();
        }

        Game.CheckIfFullRow(allBlocksY);
    }

    public bool GetIsFalling()
    {
        return isFalling;
    }
}
