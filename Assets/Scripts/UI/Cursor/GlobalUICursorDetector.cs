using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class GlobalUICursorDetector : MonoBehaviour
{
    private EventSystem eventSystem;
    private PointerEventData pointerData;
    private List<RaycastResult> raycastResults = new();

    void Start()
    {
        eventSystem = EventSystem.current;
        pointerData = new PointerEventData(eventSystem);
    }

    void Update()
    {
        pointerData.position = Input.mousePosition;
        raycastResults.Clear();

        eventSystem.RaycastAll(pointerData, raycastResults);

        bool hoveringSelectable = false;

        foreach (var result in raycastResults)
        {
            if (result.gameObject.GetComponent<Selectable>() != null)
            {
                hoveringSelectable = true;
                break;
            }
        }

        if (hoveringSelectable)
            CursorHandler.Instance.SetUICursor();
        else
            CursorHandler.Instance.ResetToDefault();
    }
}
