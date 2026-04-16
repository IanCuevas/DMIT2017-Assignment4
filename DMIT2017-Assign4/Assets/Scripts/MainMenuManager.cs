using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu Objects")]
    [SerializeField] private GameObject _loadingBarObject;
    [SerializeField] private Image _loadingBar;
    [SerializeField] private GameObject[] _gameObjects;

    [Header("Scenes to Load")]
    [SerializeField] private SceneField _persistentGameplay;
    [SerializeField] private SceneField _levelScene;

    private List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>();
    private void Awake()
    {
        _loadingBarObject.SetActive(false);
    }

    public void StartGame()
    {
        HideMenu();

        _loadingBarObject.SetActive(true);
        _scenesToLoad.Add(SceneManager.LoadSceneAsync(_persistentGameplay));
        _scenesToLoad.Add(SceneManager.LoadSceneAsync(_levelScene, LoadSceneMode.Additive));

        StartCoroutine(ProgressLoadingBar());
    }

    public void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    private void HideMenu()
    {
        for (int i = 0; i < _gameObjects.Length; i++)
        {
            _gameObjects[i].SetActive(false);
        }
    }

    private IEnumerator ProgressLoadingBar()
    {
        float loadProgress = 0f;
        for (int i = 0; i < _scenesToLoad.Count; i++)
        {
            while (!_scenesToLoad[i].isDone)
            {
                loadProgress += _scenesToLoad[i].progress;
                _loadingBar.fillAmount = loadProgress / _scenesToLoad.Count;
                yield return null;
            }
        }
    }


}
