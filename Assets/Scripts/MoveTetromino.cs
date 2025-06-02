using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class moveTetromino : MonoBehaviour
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
    private float spriteCenterX;
    private float spriteCenterY;
    //time delay before tetrimino locks into place upon landing on floor or another tetromino.
    private float delay = 0.5f;
    //Enumerator that locks tetromino in place after a period of time.
    private IEnumerator lockDelayRoutine;
    //Is the coroutine currently running.
    private bool CRActive;
    //Current position of each block in tetromino.
    public static int[] currentPositionsX = new int[4];
    public static int[] currentPositionsY = new int[4];
    public static bool isFalling;
    // Start is called once before the first execution of Update after the MonoBehaviour is created.
    void Start() {
        col = GetComponent<Collider2D>();
        spriteCenterX = col.bounds.size.x/2;
        spriteCenterY = col.bounds.size.y/2;
        isFalling = true;

        if(isFalling) {
            InvokeRepeating("Fall", 1f, 1f);
        }
    }

   void Update()
    {   
        PlayerInput();
    }

    //Controls player left, right, down movement.
    private void PlayerInput() {
        //Right and left edges of the screen.
        const float RIGHTEDGE = 5f;
        float LEFTEDGE = -5f;
        float playerRight = col.bounds.center.x + (col.bounds.size.x/2f);
        float playerLeft = col.bounds.center.x - (col.bounds.size.x/2f);
        //print(col.bounds.center.x - (col.bounds.size.x/2f));
        
        //Allows for tetromino movement if it is falling.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
        if(isFalling) {
            if(Input.GetKeyDown(KeyCode.RightArrow) && playerRight < RIGHTEDGE) {
                transform.position = new Vector2(transform.position.x + 1, transform.position.y);
                //print(transform.TransformPoint(col.bounds.center));
            } else if (Input.GetKeyDown(KeyCode.LeftArrow) && playerLeft > LEFTEDGE) {
                transform.position = new Vector2(transform.position.x - 1, transform.position.y);
                //print(playerLeft);
            } else if(Input.GetKeyDown(KeyCode.DownArrow) && math.ceil(transform.position.y - spriteCenterY) != -10) {
                transform.position = new Vector2(transform.position.x, transform.position.y - 1);
            }
        } else {
            //Cancels falling invokation.
            CancelInvoke("Fall");

            //Sets the end point position of the tetromino in the grid.
            Game.SetGridPoints(currentPositionsX, currentPositionsY);
            gameObject.GetComponent<Rotation>().enabled = false;

            //Ends Update method.
            enabled = false;
        }
    }

    //Tetromino falls 1 unit every invocation unless player has reached the game floor.
    private void Fall() {
        //int[] currentPositionsX = new int[3];
        //int[] currentPositionsY = new int[4];
        int bottomPos = 20;

        //charts the tetromino's position while falling.
        if (isFalling)
        {
            for (int i = 0; i < children.Length; i++)
            {
                currentPositionsX[i] = (int)math.ceil(transform.GetChild(i).gameObject.transform.position.x) + 4;
                currentPositionsY[i] = (int)math.ceil(transform.GetChild(i).gameObject.transform.position.y) + 9;

                if (currentPositionsY[i] < bottomPos)
                {
                    bottomPos = currentPositionsY[i];
                }
                //print(bottomPos);
                //Game.SetGridPoint(currentPositionX, currentPositionY);

                //landPositionX = (int) math.ceil(transform.GetChild(i).gameObject.transform.position.x + 0.5f) + 4;
                //landPositionY = (int) math.ceil(transform.GetChild(i).gameObject.transform.position.y + 0.5f) - 10;
            }
        }
        else
        {
            //Sets the end point psquareosition of the tetromino in the grid.
            Game.SetGridPoints(currentPositionsX, currentPositionsY);
            gameObject.GetComponent<Rotation>().enabled = false;
            //print("land position" + currentPositionsX + ", " + currentPositionsY);
            //print(Game.GetGrid());
        }

        //World position of the bottom of collider.
        float bottom = col.bounds.center.y - (col.bounds.size.y/2f);

        //tetrimino stops falling once it hits the floor.
        if(bottom > -10f) {
            transform.position = new Vector2(transform.position.x, transform.position.y - 1);
        } else {
            lockDelayRoutine = LockDelay(delay);
            StartCoroutine(lockDelayRoutine);
        }
    }

    //On colliding with another tetromino.
    /*private void OnCollisionEnter2D(Collision2D other) {

        Vector2 contactPoint = other.GetContact(0).point;
        Vector2 center = col.bounds.center;
        

        bool bottom = contactPoint.x > center.x;
        print("points " + contactPoint);
        print("center " + other.GetContact(0).point);

        if(bottom) {
            lockDelayRoutine = LockDelay(delay);
            StartCoroutine(lockDelayRoutine);
        }
    }*/

    //Allows tetromino to move for a number of seconds before locking in place.
    private IEnumerator LockDelay(float delay) {
        CRActive = true;
        
        yield return new WaitForSeconds(delay);
        isFalling = false;
    }

    /*private void DelayCounter(int movesCount) {
        if (CRActive) {
            StopCoroutine(lockDelayRoutine);
        }
    }*/

    //Collision of any tetromino block from the bottom.
    /*private bool floorCol(int[] currentPosY) {
        bool result = false;
        int belowPos; //The y position below where the tetromino block is located (where the collision will occur).

        for(int i=0; i < currentPosY.Length; i++) {
            belowPos = currentPosY[i] - 1;
            
            if (Game.isFilled(currentPositionsX[i], belowPos) == 1) {
                result = true;
            }
        }

        return result;
    }*/
}
