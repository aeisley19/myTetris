using UnityEngine;

public class NewTetromino : MonoBehaviour
{
    public SpriteRenderer renderer;
    private Sprite nextTetromino;
    public Sprite[] tetrominos;

    public void DisplayNextTetromino(int index)
    {
        nextTetromino = tetrominos[index];
        print(nextTetromino);
        renderer.sprite = nextTetromino;
    }
}
