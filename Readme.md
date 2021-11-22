# 앱 설명

## Unity Version: Unity 2017.4.40f1
## Platform : Android, Window, IOS(ios는 xcode proj 생성까지 진행)

```
Unity로 만들었으며 간단한 UI를 제공한다.
https://api.itbook.store에서 책정보를 얻어와서 UI에 표시해준다.
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


