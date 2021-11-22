using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailPanelUI : MonoBehaviour
{
    readonly string OpenAnimation = "DetailUIOpen";
    readonly string CloseAnimation = "DetailUIClose";

    public Image bookImage;
    public Text bookTitle;
    public Text subTitle;
    public Text authors;
    public Text isbn10;
    public Text isbn13;
    public Text price;
    public Text pages;
    public Text year;
    public Text rating;
    public Text url;
    public Text desc;
    public Text pdf;
    public Animation uiAnimtion;

    JsonDetailData _data;
    ScrollRect _scrollRect;
    private void Awake()
    {
        _scrollRect = GetComponentInChildren<ScrollRect>();
    }

    public void OnClick()
    {
        OnClose();
    }

    public void OnOpen(JsonDetailData data)
    {
        gameObject.SetActive(true);
        _scrollRect.Rebuild(CanvasUpdate.Layout);

        uiAnimtion.Rewind(OpenAnimation);
        uiAnimtion.Play(OpenAnimation);

        _data = data;

        SetViewInfo();
    }

    public void SetViewInfo()
    {
        bookTitle.text = _data.title;
        subTitle.text = _data.subtitle;
        authors.text = $"Authors:{_data.authors}";
        isbn10.text = $"ISBN10:{_data.isbn10}";
        isbn13.text = $"ISBN13:{_data.isbn13}";
        price.text = _data.price;
        pages.text = $"Pages:{_data.pages}";
        year.text = $"Year:{_data.year}";
        rating.text = $"Rating:{_data.rating}";
        desc.text = _data.desc;
        url.text = $"URL:{_data.url}";

        pdf.text = "";

        if (_data.pdf != null)
        {
            pdf.text = "PDF:";

            foreach (var pdfText in _data.pdf)
                pdf.text += $"\n{pdfText.chapter}";
        }
    }

    public void AnimationCloseCallBack()
    {
        gameObject.SetActive(false);
    }

    private void OnClose()
    {
        uiAnimtion.Rewind(CloseAnimation);
        uiAnimtion.Play(CloseAnimation);
    }
}
