# ペアプロボンバー

## セットアップ
### Unityをインストール
[こちら](https://unity.com/ja/download)からUnityHubをインストールする．
![unity1](https://user-images.githubusercontent.com/73092824/218959511-30929ebe-d41d-423d-bde4-8cb9ec739981.png)

[こちら](https://unity.com/releases/editor/qa/lts-releases)からバージョン`2021.3.16f1`を探し，ファイルをインストールする．その後，同じ場所のComponent Installersにある`WebGL Build Support`を加える．
![unity2](https://user-images.githubusercontent.com/73092824/218961211-ed7ead38-2dc1-4ae6-893d-e6b12e3880ff.png)

UnityHubのインストール画面で`リストに追加`からインストールしたファイルを選択して，UnityHubで使えるようにする．
![unity3](https://user-images.githubusercontent.com/73092824/218961608-b0721a31-e777-4ef4-9f59-fa0bcb4a13c8.png)

### ペアプロボンバーのダウンロード
本リポジトリのCodeからDownload ZIPを選択してファイルをダウンロードして解凍し，UnityHubからプロジェクトを開く．
![git](https://user-images.githubusercontent.com/73092824/218962208-277976ef-476d-4f55-8dd2-fcb615245974.png)

### Photonアカウントの作成とサインイン
[こちら](https://www.photonengine.com/ja-JP/PUN)でアカウントを作成し，サインインを行う．

### アプリケーションの作成
ダッシュボード画面で`CREATE A NEW APP`から新規アプリケーション作成画面に進み，Application TypeでMultiplayer Gameを選択，Photon SDKでPunを選択，適当なアプリケーション名を入力してCREATEボタンを押す．
![photon1](https://user-images.githubusercontent.com/73092824/218963420-4c4139fa-ae2b-4b6c-b748-9b863d9476bc.png)

ここからAppIDを取得できる．
![photon2](https://user-images.githubusercontent.com/73092824/218964794-ebf41668-4b87-4747-9e02-d8011b06ac47.png)

### アセットのインポート
[こちら](https://assetstore.unity.com/packages/tools/network/pun-2-free-119922)のアセットをプロジェクトにインポートする．

### Photonのセットアップ
`Window`>`Photon Unity Networking`>`PUN Wizard`からPUN Wizardを開き，先程取得したAppIDを入力してSetup Projectを押す．
![unity4](https://user-images.githubusercontent.com/73092824/218967433-dc083ee3-fe36-4c5c-aca1-477421299377.png)
![unity5](https://user-images.githubusercontent.com/73092824/218967477-25f3ee4c-1f22-4ad4-8fb6-9719b409b564.png)

### ビルド
`File`>`Build Settings`から`Build Settings`ウインドウを開き`WebGL`を選択して`Switch Platform`を押す．
![unity6](https://user-images.githubusercontent.com/73092824/218967488-5699d7d7-5099-4111-864a-fea46bed2546.png)
切り替えが完了すると`Switch Platform`が`Build`に変わるのでクリックしてビルドする．この時，ビルドファイルの名前は`web`でなければならない．

### Netlifyでサイトを公開
GitHubで新しいリポジトリを作成し、ビルドで生成されたフォルダの中身をすべてpushする．その後，[こちら](https://app.netlify.com/)からNetlifyにアクセスし，GitHubを選択して手順に従い登録する．
![netlify1](https://user-images.githubusercontent.com/73092824/218970696-1a202f6c-41e9-4e83-b4e2-fb67e8388010.png)

登録が完了すると`Sites`タブに移動し，`Add new site`からImport an existing projectを選択する．
![netlify2](https://user-images.githubusercontent.com/73092824/218970726-74f82321-1788-42e9-a13c-9ea3219c1d21.png)

GutHubを選択する．
![netlify3](https://user-images.githubusercontent.com/73092824/218970745-7eb76f6e-c58b-4768-9ab4-d50c21ccc975.png)

ビルドファイルをアップロードしたリポジトリを選択し，`Deploy site`を押す．
![netlify4](https://user-images.githubusercontent.com/73092824/218970780-0eb42da8-a1a8-42ed-889b-f9aa48d46fb2.png)

デプロイが完了するとURLが表示される．このURLに複数人でアクセスすることでペアプロボンバーをプレイできる．
![netlify5](https://user-images.githubusercontent.com/73092824/218971250-3b3cf514-42ac-41f1-a249-db27087ee27e.png)



## 遊び方
### 概要
ペアプロボンバーは，2つのチームに分かれてフィールド上のコインを取り合いスコアを競うゲームである．  
ペアプロボンバーでは，ブロックを組み合わせて作成したプログラムでキャラクターを操作する．  
プレイヤーにはドライバとナビゲータの役割があり，ドライバがブロックを使ってプログラミングを行い，ナビゲータは画面に共有されるブロックのプログラムを見て提案や指摘を行う．  
ドライバとナビゲータで相談しながら毎ターン1つのプログラムを完成させる．また，同じチームのプレイヤーと外部の音声通話アプリを利用して会話しながらプレイする．

### トップ画面
名前を入力してルーム選択画面に進む．入力はアルファベットの大文字小文字と一部の記号のみ可能．
![game1](https://user-images.githubusercontent.com/73092824/218977072-2f9e7857-b3e2-404a-ba48-038504b65789.png)

### ルーム選択画面
1から5までのルームのいずれかのボタンを押すと入室できて，チーム選択画面へと進む．1つのルームに入室できるのは4人までとなっており，ゲームプレイ中に途中入室することはできない．また，一緒にプレイする相手と同じルームを選択する必要がある．
![game2](https://user-images.githubusercontent.com/73092824/218977092-92cff413-fda1-4625-b4f0-b4ca0414e35b.png)

### チーム選択画面
AチームかBチームを選択できる．ルーム内のプレイヤー全員がチームを決定すると，一番最初に入室したプレイヤーのゲーム開始ボタンが押せるようになる．
![game3](https://user-images.githubusercontent.com/73092824/218977138-0f5dedf8-56ab-46f1-a8d8-4601841703eb.png)

### ゲーム画面
中央にゲーム画面が配置されており，その左側が自分のチームのワークスペース，右側が相手のチームのワークスペースである．ゲーム画面には7×7マスのフィールドが表示されている．また，役割によって画面の表示は異なり，役割は2ターン毎に入れ替わる．

#### ドライバの画面
役割がドライバのとき，自分のワークスペースでキャラクターの行動をプログラミングできる他，フィールド上のトラップ(フィールド上の赤い円)が見える．トラップについては後述する．
![ドライバー](https://user-images.githubusercontent.com/73092824/218981686-052d70ec-e88e-4914-8dc4-4c67d527cd62.png)

#### ナビゲータの画面
役割がナビゲータのとき，自分のチームと相手チーム両方のプログラムを見ることができる．
![ナビゲータ](https://user-images.githubusercontent.com/73092824/218981633-47ae5f2c-afed-4049-ba12-08fecb6a30a2.png)

#### ペアがいないとき
同じチームに他のプレイヤーがいない場合，トラップと相手チームのプログラムの両方が見える．
![image](https://user-images.githubusercontent.com/73092824/218985609-23b483e7-dc5c-43d2-b6f5-3c55d4da0338.png)

### ゲームの流れ
「先攻のプログラミング→後攻のプログラミング→後攻のプログラムが実行→先攻のプログラムが実行」の流れを1ターンとして，8回繰り返す．先攻と後攻のチームは毎ターン交互に入れ替わる．プログラミングは制限時間3分となっている．また，使用できるブロックの最大個数が先攻時は30個，後攻時は15個までとなっている．

### ルール
毎ターン1枚フィールド上に生成されるコインのあるマスをキャラクターが通ると，スコアが100点加点される．コインの取得は早い者勝ちとなっている．爆弾の爆発にあたるとスコアが50点減点される．爆弾はプレイヤーが設置できるものでターン終了時に爆発して2マス先まで爆風を広げる．1ターンに設置できる数は3個までだが，爆弾回収ブロックで相手の爆弾を回収することで一時的に設置できる個数を増やすことができる．  
![bomb](https://user-images.githubusercontent.com/73092824/218987374-3d2db257-56ea-4238-8cfc-83307971e785.png)![bomb_explode](https://user-images.githubusercontent.com/73092824/218986117-67200ac8-9f0c-4cb9-93c4-58531fb0e316.png)  
また，壁や障害物のあるマスに進む，障害物のあるマスに爆弾を設置するなどの実行できないことを命令するブロックを実行した場合，無効なプログラムとして扱われ5点ずつ減点される．

### 使用できるブロック
使用できるブロックの種類として，「キャラ操作」「設置・破壊」「方向」「オブジェクト」「条件分岐」「繰り返し」の6種類がある．  
![block](https://user-images.githubusercontent.com/73092824/218989512-193613a1-0a0f-4e60-bc56-3741e61a4bd7.png)  
「キャラ操作」にはキャラクターの移動と方向転換を行うブロックがあり，「設置・破壊」には障害物と爆弾の設置，障害物の破壊，爆弾の回収を行うブロックがある．「条件分岐」には指定した方向に指定したオブジェクトがあるかどうかで条件分岐を行うブロック，「繰り返し」には間に挟んだプログラムを繰り返すブロックがあり，繰り返し可能な回数は0から5回までとなっている．キャラクターを移動させるブロックは同じ方向は3個まで，繰り返しブロックは2個までと1度に使用できる個数が決まっている．「方向」と「オブジェクト」のブロックはそれ単体では意味がなく，他のブロックで方向やオブジェクトを指定するときに使用する．  
　　
## 前のリポジトリは[こちら](https://github.com/e1b19059/nakamura_sotuken)
