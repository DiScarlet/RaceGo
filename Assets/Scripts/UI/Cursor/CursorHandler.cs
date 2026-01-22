using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    public static CursorHandler Instance;

    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D uiCursor;
    [SerializeField] private Vector2 hotspot = Vector2.zero;

    private bool uiCursorActive;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SetDefaultCursor();
    }

    public void SetDefaultCursor()
    {
        if (uiCursorActive) return;
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
    }

    public void SetUICursor()
    {
        if (uiCursorActive) return;
        Cursor.SetCursor(uiCursor, hotspot, CursorMode.Auto);
        uiCursorActive = true;
    }

    public void ResetToDefault()
    {
        uiCursorActive = false;
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
    }
}
