using UnityEngine;

public class MenuCanvas : MonoBehaviour
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
