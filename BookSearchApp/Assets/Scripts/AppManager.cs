using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.Linq;
using System.Web;


public class AppManager : Singleton<AppManager>
{
    //readonly int PagePadingCount = 2;
    readonly int PagePerCount = 10;
    readonly int SlotUIImageWidth = 400;
    readonly int SlotUIImageHeight = 450;

    public PreviewUI previewUI;
    public DetailPanelUI detailPanelUI;
    public GraphicRaycaster graphicRayCaster;
    public LoadingCircleUI loadingCircleUI;
    public Text searchResultText;

    string _handlerText;
    string _searchText;

    List<JsonSearchBooksData> _searchBooksData = new List<JsonSearchBooksData>();
    public List<JsonSearchBooksData> SearchBooksData { get { return _searchBooksData; } }

    JsonDetailData _detailData;
    public JsonDetailData DetailData { get { return _detailData; } }

    int _currentPage = 1;
    int _totalCount = 0;
    int _filterdCount = 0;

    Coroutine _currentSearchCoroutine;
    List<string> _containFilter = new List<string>();
    List<string> _exceptFilter = new List<string>();

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
            Screen.SetResolution(600, 800, false);
    }

    //private void OnGUI()
    //{
    //    if(GUI.Button(new Rect(200, 200, 300, 300), "test")== true)
    //    {
    //        string file_path = Application.dataPath + "/json.txt";
    //        string file_text = System.IO.File.ReadAllText(file_path);
    //        var api = JsonConvert.DeserializeObject<JsonDetailData>(file_text);
    //        Debug.Log(api);
    //    }
    //}

    public void CloseApp()
    {
        Application.Quit();
    }

    private void ClearSearchBook()
    {
        _searchBooksData.Clear();
        _containFilter.Clear();
        _exceptFilter.Clear();
        _currentPage = 1;
        _totalCount = 0;
        _filterdCount = 0;
    }

    public void OnSearchBooks(string text)
    {
        if (_currentSearchCoroutine != null)
        {
            StopCoroutine(_currentSearchCoroutine);
            _currentSearchCoroutine = null;
        }

        _currentSearchCoroutine = StartCoroutine(SearchBooks(text, true));
    }

    public void SearchNextPageBooks()
    {
        if (_currentSearchCoroutine != null)
            return;

        _currentSearchCoroutine = StartCoroutine(SearchBooks(_searchText, false));
    }

    public void OnOpenBookDetail(string isbn)
    {
        StartCoroutine(OpenBookDetail(isbn));
    }

    public bool IsSearchingBooks()
    {
        if (_currentSearchCoroutine != null)
            return true;

        return false;
    }

    IEnumerator SearchBooks(string searchText, bool isFirstPage = true)
    {
        _searchText = searchText;

        if (isFirstPage == true)
        {
            ClearSearchBook();

            if (searchText.Contains("|") == true)
            {
                var splitText = searchText.Split(new string[] { "|" }, System.StringSplitOptions.None);
                _containFilter.Add(splitText[0]);
                if (splitText.Length >= 2)
                {
                    _containFilter.Add(splitText[1]);
                }
            }
            else if (searchText.Contains("-") == true)
            {
                var splitText = searchText.Split(new string[] { "-" }, System.StringSplitOptions.None);
                _containFilter.Add(splitText[0]);
                if (splitText.Length >= 2)
                {
                    _exceptFilter.Add(splitText[1]);
                }
            }
            else
            {
                _containFilter.Add(searchText);
            }
        }
        else
        {
            if (_totalCount > 0 && (_totalCount <= _currentPage * PagePerCount))
            {
                ActiveLoadingUI(false);
                yield break;
            }
            else
            {
                _currentPage++;
            }
        }

        //_searchText = WWW.EscapeURL(_searchText);
        ActiveLoadingUI(true);

        yield return StartCoroutine(GetRequestHandlerText($"https://api.itbook.store/1.0/search/{_searchText}/{_currentPage}"));

        JsonSearchData data = GetJsonData<JsonSearchData>();
        if (data != null)
        {
            if (data.total == 0)
            {
                SetResultEmptyText();

                _totalCount = 1;
                _currentSearchCoroutine = null;
                previewUI?.UpdateUI(_searchBooksData);
                yield break;
            }

            _currentPage = data.page;
            _totalCount = data.total;

            IList<JsonSearchBooksData> filteredData = FilterText(data.books, _containFilter, _exceptFilter);

            for (int i = 0; i < filteredData.Count; i++)
            {
                yield return StartCoroutine(GetTexture(filteredData[i].image, (tex) =>
                {
                    filteredData[i].bookTexture = ScaleTexture(tex, SlotUIImageWidth, SlotUIImageHeight);
                }
                ));
            }

            _searchBooksData.AddRange(filteredData);
            _filterdCount += filteredData.Count;

            if (_filterdCount < PagePerCount)
            {
                yield return StartCoroutine(SearchBooks(searchText, false));
            }
            else
            {
                _filterdCount = 0;
            }

            if (isFirstPage == true)
            {
                previewUI?.MakeUI(_searchBooksData);
                searchResultText.gameObject.SetActive(false);
            }
            else
            {
                previewUI?.UpdateUI(_searchBooksData);
            }

            searchResultText.gameObject.SetActive(false);
        }
        else
        {
            SetResultEmptyText();
        }

        ActiveLoadingUI(false);
        _currentSearchCoroutine = null;
    }

    private void SetResultEmptyText()
    {
        searchResultText.text = "?????? ????????? ???????????? ????????????.";
        searchResultText.gameObject.SetActive(true);
    }

    private void ActiveLoadingUI(bool active)
    {
        loadingCircleUI?.gameObject.SetActive(active);
    }

    IEnumerator OpenBookDetail(string isbn)
    {
        ActiveLoadingUI(true);

        graphicRayCaster.enabled = false;

        yield return StartCoroutine(GetRequestHandlerText($"https://api.itbook.store/1.0/books/{isbn}"));

        _detailData = GetJsonData<JsonDetailData>();
        
        yield return UpdateDetailBookTexture(_detailData.image, detailPanelUI.bookImage);

        ActiveLoadingUI(false);

        detailPanelUI.OnOpen(_detailData);

        yield return new WaitForSeconds(0.5f);

        graphicRayCaster.enabled = true;
    }

    private IList<JsonSearchBooksData> FilterText(IList<JsonSearchBooksData> data, List<string> containText, List<string> exceptText)
    {
        var query = data.Where(x => containText.Any(a => x.title.ToLower().Contains(a.ToLower())) && !exceptText.Any(b => x.title.ToLower().Contains(b.ToLower())));

        return query.ToList();
    }

    public IEnumerator UpdatePreviewBookTexture(string imageUrl, int i, Image image)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //Debug.Log(www.error);
        }
        else
        {
            var tex = ((DownloadHandlerTexture)www.downloadHandler).texture;
            _searchBooksData[i].bookTexture = ScaleTexture(tex, (int)image.rectTransform.sizeDelta.x, (int)image.rectTransform.sizeDelta.y);

            Rect rect = new Rect(0, 0, image.rectTransform.sizeDelta.x, image.rectTransform.sizeDelta.y);
            image.sprite = Sprite.Create(_searchBooksData[i].bookTexture, rect, new Vector2(0.5f, 0.5f));
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
        }
    }

    private IEnumerator GetTexture(string imageUrl, System.Action<Texture2D> callBack)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //Debug.Log(www.error);
        }
        else
        {
            var tex = ((DownloadHandlerTexture)www.downloadHandler).texture;
            callBack(tex);
        }
    }

    private IEnumerator UpdateDetailBookTexture(string imageUrl, Image image)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //Debug.Log(www.error);
        }
        else
        {
            var tex = ((DownloadHandlerTexture)www.downloadHandler).texture;
            var scaleTex = ScaleTexture(tex, (int)image.rectTransform.sizeDelta.x, (int)image.rectTransform.sizeDelta.y);
            Rect rect = new Rect(0, 0, image.rectTransform.sizeDelta.x, image.rectTransform.sizeDelta.y);
            image.sprite = Sprite.Create(scaleTex, rect, new Vector2(0.5f, 0.5f));
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
        }
    }

    private IEnumerator GetRequestHandlerText(string uri)
    {
        _handlerText = null;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
                yield break;
            }

            _handlerText = webRequest.downloadHandler.text;
        }
    }

    private T GetJsonData<T>()
    {
        if (_handlerText == null)
            return default(T);
        else if (_handlerText.Contains("Invalid request") == true)
            return default(T);

        return JsonConvert.DeserializeObject<T>(_handlerText);
    }

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
        Color[] rpixels = result.GetPixels(0);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);

        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
        }

        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }
}
