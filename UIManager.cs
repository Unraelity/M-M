using UnityEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private float volumeScalerWhenPaused = 0.5f;
    private bool paused;
    public static UIManager Instance;
    public bool Paused => paused;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Resume()
    {
        if (pauseMenu == null)
        {
            return;
        }
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        paused = false;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ResetVolume();
        }
    }

    public void Pause()
    {
        if (pauseMenu == null)
        {
            return;
        }
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ScaleVolume(volumeScalerWhenPaused);
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
