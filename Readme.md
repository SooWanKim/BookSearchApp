# 앱 설명

## Unity Version: Unity 2017.4.40f1
## Platform : Android, Window, IOS(ios는 xcode proj 생성까지 진행)

## 검색단어가 책제목에 무조건 포함되어야 검색목록에 나온다

```
Unity로 만들었으며 간단한 UI를 제공한다.
https://api.itbook.store에서 책정보를 얻어와서 UI에 표시해준다.


‘or(|)’ operator는 두 키워드중 하나의 키워드가 제목에 포함된 서적들을 보여준다. (e.g.
‘unity|C#’ : unity 또는 C#이 제목에 포함된 서적들을 검색해 보여준다.)

‘not(-)’ operator는 앞의 키워드가 제목에 포함된 서적을을 검색하되 뒤의 키워드가 포함되지
않은 서적들을 보여준다. (e.g. ‘unity-C#’ : unity가 제목에 포함된 서적을 검색하되 C#라는
키워드를 가지고 있는 서적은 제외한다.
```

![검색정보](https://github.com/SooWanKim/BookSearchApp/blob/master/BookSearchApp.gif)


# 코드 설명

- AppManager.cs
  ```
  책 목록 검색과, 개별 책정보를 얻어오며 PreviewUI, DetailUI 갱신과 열기/닫기 기능을 한다.
  ```
- PreviewUI.cs
  ```
  책 목록 검색정보를 표시해주며, PreviewSlotUI를 생성 및 갱신 해준다.
  InputField를 통해 검색 텍스트를 입력받아 처리한다.
  ```
- PreviewSlotUI.cs
  ```
  책 목록검색에서 나온 개별 책정보를 표시한다.
  ```
- DetailPanelUI
  ```
  개별 책정보를 표시한다.
  ```


