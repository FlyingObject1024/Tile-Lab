# 概要
本リポジトリは、タイルラボというパズルゲームを実装するためのUnity用ファイル群です。


# 自身のPCに落とす方法
①Unity6の環境をPC上に作り、Unity Hubから扱えるようにする。
②GitHubからリポジトリをクローン(ダウンロード)し、どこかにフォルダとして作る。
③Unity HubのAddからAdd diskを選び、クローンしたリポジトリを選択。
④Unity Hubから起動

# 扱っている名称
## Stage(ステージ)
パズルを複数個含んでいる。
1つのパズルをクリアすると、次のパズルを遊べる。全てのパズルをクリアすると、ステージクリア。

## Puzzle(パズル)
タイルを複数個含んでいる。
タイルを組み合わせ、指示されたゴールを全て

## Tile(タイル)
ファイル概要
ほとんどの主要要素はAssetsの中に入っているので、Assets以下のファイルのみ説明します。

## Scenes
Unityエンジンが実装している"シーン"が入っている。


### Mainシーン
エントリーポイントとしてのシーン。
ここからTitleシーンが起動する。

### Titleシーン
文字通りタイトルを表記するシーン。
現在は実装していないが、ここで「モノクロモード」「カラーモード」「工場モード」を選択する。

### StageSelectシーン
ステージを選択するシーン。
ここで選んだ数値を元に、Stageシーンが立ち上がる。

### Puzzleシーン(Stageシーン)
現在はPuzzleという名前になっているが、今後Stageに変更する可能性有り。
指定された.csvファイルからステージ情報を読み込み、ゲームプレイ環境を提供する。

## Resources
画像、プレハブ(クラス的なGameObject)を保管する場所。基本的にこの中は弄らない。

## Scripts
全プログラムを入れているフォルダ。

### Mainフォルダ
Mainシーン及び、シーン遷移用のスクリプトを入れている。
Titleシーン用の処理も含んでいる。

#### Main.cs
Titleシーンを開くためのもの。編集は不要。

#### Change_scene.cs
シーン遷移を担っている。
ここにあるメソッド以外から遷移することのないように。
「ステージボタン」や「戻るボタン」などにアタッチする。

### Stage_Selectフォルダ
Stage_selectシーン用のスクリプト群。

#### Stage_select.cs

#### StageClickHandler.cs

### Puzzleフォルダ(Stageフォルダ)
ここの比重が最も重い。パズルのゲームそのものを構成している。

#### Puzzle.cs(Stage.cs)
ステージを起動するためのもの。

#### StageManager.cs
Stageという名前の空GameObjectにアタッチされている。
.csvファイルを読み込み、puzzleプレハブ、tileプレハブからインスタンスを生成する。

#### UiManager.cs
Stageという名前の空GameObjectにアタッチされている。
起動しているpuzzleに従ってUIを構成する。
UiCanvas、InventoryField、GoalFieldのUIを操作するためのもの。

#### PuzzleManager.cs
puzzleプレハブにアタッチされている。
中間処理を行っているため、TileManagerに数値を受け渡すような処理が多い。

#### GridManager.cs
puzzleにアタッチされている。
タイルが配置されている場所を管理するためのもの。
同じ座標には二つのタイルが入らない。
が、Outputタイルのみ例外で、上からGateタイルを入れられる。
座標とタイルは辞書で保存されている。
(keyがVector2Int、valueがTileManager)

#### TileManager.cs
タイル1つにアタッチされている。
タイルの種類にはInput、Gate、Goal、Outputがある。
それぞれの種類のタイルによって使用するメソッドが異なる。

#### TileDragHandler.cs
タイルをドラッグアンドドロップするためのスクリプト。
【未実装】ホイール情報から、Gateを回転させる。

### Dataフォルダ
csvファイルで構成されたステージデータが入っている。

## 
