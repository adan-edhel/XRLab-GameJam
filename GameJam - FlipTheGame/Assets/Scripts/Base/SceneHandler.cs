using UnityEngine.SceneManagement;
using UnityEngine;

public static class SceneHandler
{
    /// <summary>
    /// Returns whether the game is in menu scene
    /// </summary>
    public static bool inMenu => GetCurrentSceneIndex < 1;

    /// <summary>
    /// Returns the build index of the currently active scene
    /// </summary>
    /// <returns></returns>
    public static int GetCurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;

    /// <summary>
    /// Returns the total amount of scenes currently in Build Index
    /// </summary>
    /// <returns></returns>
    public static int GetTotalSceneCount => SceneManager.sceneCountInBuildSettings;

    /// <summary>
    /// Transitions to a specified scene with a loading screen
    /// </summary>
    /// <param name="index"></param>
    public static void TransitionScene(int index)
    {
        if (index < 0 || index > GetTotalSceneCount - 1)
        {
            Debug.Log("Scene index out of bounds");
            return;
        }

        SceneManager.LoadSceneAsync(index);
    }

    /// <summary>
    /// Reloads the current active scene
    /// </summary>
    public static void ResetScene() => TransitionScene(GetCurrentSceneIndex);

    /// <summary>
    /// Ends the application
    /// </summary>
    public static void QuitGame() => Application.Quit(0);
}
