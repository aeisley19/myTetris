using System.ComponentModel;
using Unity.Mathematics;
using UnityEditor.Animations;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    private bool isVertical;
    private MoveTetromino mv;
    private Game game;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isVertical = false;
        mv = GetComponent<MoveTetromino>();
        game = GameObject.Find("GameController").GetComponent<Game>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (mv.GetIsFalling())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                if (gameObject.tag == "Long" || gameObject.tag == "LeftStairs" || gameObject.tag == "RightStairs")
                {
                    HalfRotate();
                }
                else
                {
                    FullRotate();
                }
            }
        }
    }

    //Rotation for long tetromino.
    private void HalfRotate()
    {
        Vector3 simulatedRotation = isVertical ? new Vector3(0, 0, 0) : simulatedRotation = new Vector3(0, 0, -90);

        SimulateRotation(simulatedRotation);
        isVertical = !isVertical;
    }

    //Roation for all tetrominos except long and square.
    private void FullRotate()
    {
        Vector3 simulatedRotation = new Vector3(0, 0, transform.eulerAngles.z - 90);
        SimulateRotation(simulatedRotation);
    }

    private void SimulateRotation(Vector3 simulatedRotation)
    {
        Quaternion parentQuaternian = Quaternion.Euler(simulatedRotation);
        bool isValid = true;

        //print("old  " + );
        //print("new" + parentTransform.position);

        foreach (Transform child in transform)
        {
            //isValid = false;

            Vector3 localPosition = child.localPosition;
            Vector3 predictedLocation = parentQuaternian * localPosition + transform.position;

            //Tetromino can rotate outside of bounds above y = 10, but can't for outside 0 < x > 10 or y > 0.
            if (!(predictedLocation.y > 10))
            {
                if (predictedLocation.x < -5 || predictedLocation.x > 5 ||
                        predictedLocation.y < -10 ||
                        game.GetGrid()[(int)math.ceil(predictedLocation.x) + 4, (int)math.ceil(predictedLocation.y) + 9] != null)
                {
                    isValid = false;
                    print(predictedLocation.y);
                    break;
                }
            }
        }

       // print("out " + isValid);
        if (isValid)
        {
            transform.eulerAngles = simulatedRotation;
        }
    }

    private void GetFullBottom()
    {
        
    }
}