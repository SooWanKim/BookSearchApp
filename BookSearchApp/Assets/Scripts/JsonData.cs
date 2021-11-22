using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonSearchData
{
    public int error;
    public int total;
    public int page;
    public IList<JsonSearchBooksData> books;
}

public class JsonSearchBooksData
{
    public string title;
    public string subTitle;
    public string isbn13;
    public string price;
    public string image;
    public string url;
    public Texture2D bookTexture;
}

public class JsonDetailData
{
    public int error;
    public string title;
    public string subtitle;
    public string authors;
    public string publisher;
    public string isbn10;
    public string isbn13;
    public int pages;
    public int year;
    public int rating;
    public string desc;
    public string price;
    public string image;
    public string url;

    public IList<JsonPDFData> pdf;
}

public class JsonPDFData
{
    public string chapter;
}
