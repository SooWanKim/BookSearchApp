using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewSlotUI : MonoBehaviour
{
    public Image bookImage;
    public Text bookTitle;
    public Text bookSubTitle;
    public Text bookISBN13;
    public Text bookPrice;

    int _cellIdx;
    JsonSearchBooksData _data;

    public void OnClickSlot()
    {
        AppManager.Instance.OnOpenBookDetail(_data.isbn13);
    }

    public void ScrollCellIndex(int cellIdx)
    {
        SetCellInfo(cellIdx);

        //if(AppManager.Instance.IsUpdateNextPage(cellIdx) == true)
        //{
        //    AppManager.Instance.NextPageSearchBooks();
        //}
        //Debug.Log($"{cellIdx} + ScrollCellIndex");
    }

    public void SetCellInfo(int cellIdx)
    {
        _cellIdx = cellIdx;

        var searchBooksData = AppManager.Instance.SearchBooksData;

        _data = searchBooksData[cellIdx];

        SetViewInfo();
    }

    private void SetViewInfo()
    {
        bookImage.color = new Color(bookImage.color.r, bookImage.color.g, bookImage.color.b, 0);
        if (_data.bookTexture == null)
        {
            StartCoroutine(AppManager.Instance.UpdatePreviewBookTexture(_data.image, _cellIdx, bookImage));
        }
        else
        {
            _data.bookTexture = AppManager.ScaleTexture(_data.bookTexture, (int)bookImage.rectTransform.sizeDelta.x, (int)bookImage.rectTransform.sizeDelta.y);
            Rect rect = new Rect(0, 0, bookImage.rectTransform.sizeDelta.x, bookImage.rectTransform.sizeDelta.y);
            bookImage.sprite = Sprite.Create(_data.bookTexture, rect, new Vector2(0.5f, 0.5f));
            bookImage.color = new Color(bookImage.color.r, bookImage.color.g, bookImage.color.b, 1.0f);
        }

        bookTitle.text = _data.title;
        if (string.IsNullOrEmpty(_data.subTitle) == true)
        {
            bookSubTitle.gameObject.SetActive(false);
        }
        else
        {
            bookSubTitle.gameObject.SetActive(true);
            bookSubTitle.text = _data.subTitle;
        }

        bookISBN13.text = $"ISBN:{_data.isbn13}";
        bookPrice.text = _data.price;
        bookImage.SetNativeSize();
    }
}