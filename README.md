# ImageChecker v3 

画像の位置と拡大率を考慮した表示をシミュレートするためのソフト。自分用。

## 使い方

画像ファイルが入ったフォルダをウィンドウにドラッグアンドドロップすると、中の画像が読み込まれる。

画像ファイルは以下の命名規則に沿っていることを想定している。

    [A-D]\d{2}\d{2}$

### 命名規則の各部詳細

この規則に沿っている場合、先頭の `[A-D]` の部分でグループ分けされて画像が読み込まれます。

グループ `A` の画像を選択した時点で、残りのグループの画像が次の `\d{2}` の値でフィルタリングされます。

最後の `\d{2}` は動作に影響しません。画像を識別するための任意の番号を割り振ります。

上記の条件に当てはまらない画像ファイルでも、単体での表示は可能です。

## キーボード操作

以下のキーボードショートカットをサポートしています。

| Key              | 機能                                    |
|------------------|---------------------------------------|
| J                | プレビューの位置を上に動かす                        |
| K                | プレビューの位置を下に動かす                        |
| H                | プレビューの位置を左に動かす                        |
| L                | プレビューの位置を右に動かす                        |
| Ctrl + 0         | プレビューの位置初期値に戻す                        |
| Ctrl + 1         | プレビューの位置と拡大率を初期値に戻す                   |
| Ctrl + I         | 現在の表示状態を image タグとしてクリップボードに転送        |
| Ctrl + D         | 現在の表示状態を draw タグとしてクリップボードに転送         |
| Ctrl + Shift + I | 現在の表示状態を image(anime) タグとしてクリップボードに転送 |
| Ctrl + Shift + D | 現在の表示状態を draw(anime) タグとしてクリップボードに転送  |
| Ctrl + S         | スライドタグを出力                             |
| Ctrl + T         | タグ（テキスト）からデータを読み込むための画面を開きます          |

## タグのフォーマット

タグのフォーマットの文字列内では、以下の変数が使用可能です。コピーする際に、下記の文字列が実際の値に置き換えられます。

| 文字列       | 説明                       |
|-----------|--------------------------|
| $a        | グループA の画像ファイル名           |
| $b        | グループB の画像ファイル名           |
| $c        | グループC の画像ファイル名           |
| $d        | グループD の画像ファイル名           |
| $x        | 画像の X 座標                 |
| $y        | 画像の y 座標                 |
| $scale    | 画像の拡大率                   |
| $duration | Slide アニメーションの長さ         |
| $distance | Slide アニメーションの移動距離       |
| $degree   | Slide アニメーションの移動の角度（度数法） |

### 設定画面記述例

    <image a="$a" b="$b" c="$c" d="$d" x="$x" y="$y" scale="$scale" targetLayerIndex="0" />
    <anime name="slide" duration="$duration" distance="$distance" degree="$degree"  repeatCount="0" />

### パース可能なタグ例

    <image a="A0101" b="B0102" c="C0103" d="D0103" x="100" y="200" scale="1" targetLayerIndex="0" />
    <draw a="A0101" b="B0102" c="C0103" d="D0103" targetLayerIndex="0" />
    <anime name="image" a="A0101" b="B0102" c="C0103" d="D0103" x="100" y="200" scale="1" targetLayerIndex="0" />
    <anime name="draw" a="A0101" b="B0102" c="C0103" d="D0103" targetLayerIndex="0" />