using System;
using System.Collections;
using System.Data;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Timeline;

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
    //Is the tetromino falling.
    private bool isFalling;
    //time delay before tetrimino locks into place upon landing on floor or another tetromino.
    private float delay = 0.5f;
    //Enumerator that locks tetromino in place after a period of time.
    private IEnumerator lockDelayRoutine;
    //Is the coroutine currently running.
    private bool CRActive;
    //Current position of each block in tetromino.
    private int[] currentPositionsX = new int[4];
    private int[] currentPositionsY = new int[4];
    private Vector2[] currentPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        float boxEdgeRight = 5f;
        float boxEdgeLeft = -4f;
            //Allows for tetromino movement if it is falling.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
            if(isFalling) {
                if(Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x + spriteCenterX < boxEdgeRight) {
                    print("down");
                    transform.position = new Vector2(transform.position.x + 1, transform.position.y);
                } else if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x - spriteCenterX >= boxEdgeLeft) {
                    transform.position = new Vector2(transform.position.x - 1, transform.position.y);
                } else if(Input.GetKeyDown(KeyCode.DownArrow) && math.ceil(transform.position.y - spriteCenterY) != -10) {
                    transform.position = new Vector2(transform.position.x, transform.position.y - 1);
                }
            } else {
                //Cancels falling invokation.
                CancelInvoke("Fall");
 
                //Sets the end point position of the tetromino in the grid.
                Game.SetGridPoints(currentPositionsX, currentPositionsY);
                print("land position" + currentPositionsX + ", " + currentPositionsY);
                print(Game.GetGrid()[0, 0]);

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
        if(isFalling) {
            for (int i=0; i < children.Length; i++) {
                currentPositionsX[i] = (int) math.ceil(transform.GetChild(i).gameObject.transform.position.x + 0.5f) + 4;
                currentPositionsY[i] = (int) math.ceil(transform.GetChild(i).gameObject.transform.position.y + 0.5f) + 9;

            if (currentPositionsY[i] < bottomPos) {
                bottomPos = currentPositionsY[i];
            }
            
           /// Game.SetGridPoint(currentPositionX, currentPositionY);
            print(children[i].name + " " + currentPositionsX + ", " + currentPositionsY);

            //landPositionX = (int) math.ceil(transform.GetChild(i).gameObject.transform.position.x + 0.5f) + 4;
            //landPositionY = (int) math.ceil(transform.GetChild(i).gameObject.transform.position.y + 0.5f) - 10;
            }
        } else {
            //Sets the end point position of the tetromino in the grid.
            Game.SetGridPoints(currentPositionsX, currentPositionsY);
            print("land position" + currentPositionsX + ", " + currentPositionsY);
            print(Game.GetGrid());
        }

        print(bottomPos);

        //tetrimino stops falling once it hits the floor.
        if(math.ceil(transform.position.y - spriteCenterY) != -10) {
            transform.position = new Vector2(transform.position.x, transform.position.y - 1);
        } else {
            lockDelayRoutine = LockDelay(delay);
            StartCoroutine(lockDelayRoutine);
        }
    }

    //On colliding with another tetromino.
    private void OnCollisionEnter2D(Collision2D other) {
        //Initiates lock delay if tetromino collides with another tetromino.
        if(other.gameObject.tag == "Tetromino") {
            print("here");
            lockDelayRoutine = LockDelay(delay);
            StartCoroutine(lockDelayRoutine);
        }
    }

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
}
