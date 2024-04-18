using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class Resolution_Manager : MonoBehaviour
{
    #region Variables & References
    [Header("Screen Resolution Dropdown")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    // --- Screen Resolution Settings --- //
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private int currentResolutionIndex = 0;
    #endregion

    #region Default Unity
    // Runs at the beginning before Start() and initializes the primary display.
    void Awake()
    {
        MoveToPrimaryDisplayGameStart();
    }

    // Runs at the beginning of the game and sets up resolution options.
    [System.Obsolete]
    private void Start()
    {
        InitializeResolutionOptions();
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
    }
    #endregion

    #region Force Main Display
    // Moves the game window to the primary display at the start of the game.
    void MoveToPrimaryDisplayGameStart()
    {
        #if !UNITY_EDITOR
        StartCoroutine(MoveToPrimaryDisplay());
        #endif
    }

    // Coroutine to move the game window to the primary display.
    [System.Obsolete]
    IEnumerator MoveToPrimaryDisplay()
    {
        List<DisplayInfo> displays = new();
        Screen.GetDisplayLayout(displays);
        if (displays?.Count > 0)
        {
            var moveOperation = Screen.MoveMainWindowTo(displays[0], new Vector2Int(displays[0].width / 2, displays[0].height / 2));
            yield return moveOperation;

            // Load resolution after moving to primary display
            LoadResolution();
        }
    }

    // Load the resolution settings when the game starts.
    [System.Obsolete]
    private void LoadResolution()
    {
        int screenWidth = PlayerPrefs.GetInt("ScreenWidth", Screen.currentResolution.width);
        int screenHeight = PlayerPrefs.GetInt("ScreenHeight", Screen.currentResolution.height);
        int currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            Resolution res = filteredResolutions[i];
            if (res.width == screenWidth && res.height == screenHeight && res.refreshRate == currentRefreshRate)
            {
                SetResolution(i);
                return;
            }
        }

        // If the saved resolution is not found, set to the default resolution
        SetResolution(currentResolutionIndex);
    }
    #endregion

    #region Resolution Functions
    // Initializes the options for screen resolution.
    [System.Obsolete]
    private void InitializeResolutionOptions()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();

        if (resolutions != null && resolutions.Length > 0)
        {
            int screenWidth = PlayerPrefs.GetInt("ScreenWidth", Screen.currentResolution.width);
            int screenHeight = PlayerPrefs.GetInt("ScreenHeight", Screen.currentResolution.height);
            int currentRefreshRate = Screen.currentResolution.refreshRate;

            foreach (Resolution res in resolutions)
            {
                if (res.refreshRate == currentRefreshRate)
                {
                    filteredResolutions.Add(res);
                }
            }

            List<string> options = new List<string>();

            foreach (Resolution res in filteredResolutions)
            {
                string aspectRatio = GetAspectRatio(res.width, res.height);
                string resolutionOption = $"{res.width}x{res.height} ({aspectRatio}) {res.refreshRate} Hz";
                options.Add(resolutionOption);

                if (res.width == screenWidth && res.height == screenHeight)
                {
                    currentResolutionIndex = filteredResolutions.IndexOf(res);
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

            SetResolution(currentResolutionIndex);
        }
        else
        {
            Debug.LogWarning("No resolutions found.");
        }
    }

    // Calculates and returns the aspect ratio of a resolution.
    private string GetAspectRatio(int width, int height)
    {
        float aspectRatio = (float)width / height;
        return aspectRatio.ToString("0.##");
    }

    // Sets the screen resolution based on the selected index in the dropdown.
    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex >= 0 && resolutionIndex < filteredResolutions.Count)
        {
            Resolution resolution = filteredResolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, true);

            PlayerPrefs.SetInt("ScreenWidth", resolution.width);
            PlayerPrefs.SetInt("ScreenHeight", resolution.height);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogWarning("Invalid resolution index.");
        }
    }

    // Called when the resolution dropdown value changes.
    private void OnResolutionChanged(int resolutionIndex)
    {
        SetResolution(resolutionIndex);
    }

    // Saves the current screen resolution settings.
    private void OnApplicationQuit()
    {
        SaveResolution();
    }

    // Saves the current screen resolution settings when the application is paused.
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveResolution();
        }
    }

    // Saves the current screen resolution settings.
    private void SaveResolution()
    {
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;
        PlayerPrefs.SetInt("ScreenWidth", screenWidth);
        PlayerPrefs.SetInt("ScreenHeight", screenHeight);
        PlayerPrefs.Save();
    }
    #endregion
}
