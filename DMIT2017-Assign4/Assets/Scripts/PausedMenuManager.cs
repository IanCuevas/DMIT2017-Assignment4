using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PausedMenuManager : MonoBehaviour
{
    [Header("Paused Menu Objects")]
    [SerializeField] private GameObject _pausedMenu;

    //Miscellaneous
    public InputSystem_Actions userInput;

    private void Awake()
    {
        _pausedMenu.SetActive(false);
        userInput = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        userInput.UI.Enable();
        userInput.UI.Escape.performed += TogglePausedMenu;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        _pausedMenu.SetActive(false);
    }

    void TogglePausedMenu(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _pausedMenu.SetActive(true);
        }
    }
}
