using UnityEngine;

public class CanvasHandler : MonoBehaviour
{
    public void ChangeScene(int index)
    {
        SceneHandler.TransitionScene(index);
    }

    public void QuitGame()
    {
        SceneHandler.QuitGame();
    }
}
