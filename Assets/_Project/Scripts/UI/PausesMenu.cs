using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;


    private GameControls controls;
    bool IsPaused { get; set; }




    private void Awake() => controls = new GameControls();
    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Pause.performed += OnPausePerformed;
        resumeButton.onClick.AddListener(Resume);
    }


    private void OnDisable()
    {
        controls.Player.Disable();
        controls.Player.Pause.performed += OnPausePerformed;
        resumeButton.onClick.RemoveListener(Resume);
    }


    private void OnDistroy() => controls.Dispose();


    void Setpause(bool paused)
    {
        pausePanel.SetActive(paused);
        //if(paused)
        //{
        //    Time.timeScale = 0;
        //}
        //else
        //{
        //   Time.timeScale = 1 ;
        //}
        Time.timeScale = paused ? 0 : 1;
    }

    public void Resume()
    {
        Setpause(false);
    }


    private void OnPausePerformed(InputAction.CallbackContext context) => Setpause(!IsPaused);
}