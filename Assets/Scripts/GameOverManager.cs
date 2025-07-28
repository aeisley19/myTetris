using UnityEngine;

public class GameOverManager : MonoBehaviour
{

    public GameObject canvas;
    
    public void ShowGameOver()
    {
        print("here");
        canvas.SetActive(true);
    }
}
