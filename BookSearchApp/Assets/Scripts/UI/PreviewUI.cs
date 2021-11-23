using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PreviewUI : MonoBehaviour
{
    public InputField inputField;

    LoopVerticalScrollRect _loopScrollRect;
    PreviewSlotUI[] _previewSlotUI;
    RectTransform _loopScrollRectTransform;

    private void Awake()
    {
        _loopScrollRect = GetComponentInChildren<LoopVerticalScrollRect>();

        _loopScrollRect.prefabSource?.InitPool();

        _loopScrollRect.RefillCells();
        _loopScrollRect.Rebuild(CanvasUpdate.PreRender);
        _loopScrollRect.RefreshCells();

        _loopScrollRect.verticalNormalizedPosition = 0.001f;

        _loopScrollRectTransform = _loopScrollRect.GetComponent<RectTransform>();
    }

    public void OnEnterInputText()
    {
        if (inputField.text.Length > 0)
        {
            inputField.enabled = false;
            AppManager.Instance.OnSearchBooks(inputField.text);
        }
    }

    void Update()
    {
        if (AppManager.Instance.IsSearchingBooks() == false)
        {
            inputField.enabled = true;
        }

        if (_loopScrollRect.verticalNormalizedPosition >= 1.0f)
        {
            AppManager.Instance.SearchNextPageBooks();
        }
    }

    public void UpdateUI(List<JsonSearchBooksData> searchBooksData)
    {
        int totalCount = searchBooksData.Count;
        _loopScrollRect.totalCount = totalCount;
        //_loopScrollRect.RefillCells();
        _loopScrollRect.Rebuild(CanvasUpdate.Layout);
        _loopScrollRect.RefreshCells();
        LayoutRebuilder.ForceRebuildLayoutImmediate(_loopScrollRectTransform);
        _loopScrollRect.verticalNormalizedPosition = _loopScrollRect.verticalNormalizedPosition + 0.001f;
    }

    public void MakeUI(List<JsonSearchBooksData> searchBooksData)
    {
        int totalCount = searchBooksData.Count;

        _loopScrollRect.totalCount = totalCount;
        _loopScrollRect.RefillCells();
        _loopScrollRect.RefreshCells();

        _previewSlotUI = _loopScrollRect.GetComponentsInChildren<PreviewSlotUI>(true);
        if (_previewSlotUI == null)
            return;

        for (int i = 0; i < totalCount; i++)
        {
            if (_previewSlotUI.Length <= i)
                break;

            var slotUI = _previewSlotUI[i];
            slotUI.SetCellInfo(i);
        }
        _loopScrollRect.verticalNormalizedPosition = 0.001f;

        LayoutRebuilder.ForceRebuildLayoutImmediate(_loopScrollRectTransform);
    }
}
