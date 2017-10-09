# DmmGameRankingObserver

DMMのゲームランキング情報を監視するAzureFunctionsです。

## やってること

1. 毎時10分に[DMM GAMES R18のランキングページ](http://games.dmm.co.jp/ranking/)にGET HTTPして、htmlを取得する
2. htmlをパースしてモデルにマッピングする
3. モデルをAzure Storage Tableに格納する

## 使い方

VisualStudioからAzure Functionsにデプロイすれば動きます。追加設定などは特に必要ありません。

### 注意

- 格納するデータは正規化されてない
- 格納したランキング情報をいい感じにグラフィカルに表示する機能は存在しない
