NCV2MikuMikuMouth
===============
NCV2MikuMikuMouthはNiconamaCommentViewer用プラグインです。  
ニコ生のコメントをSocket接続された対象にコメント情報を逐次PUSH通知します。

機能
-------
起動するとポート*50082*でTCP Socketの待受を開始します。
ここにTCPで繋ぐことでニコ生のコメントを受け取ることができます。  コメント情報は新しいコメントが来る度に以下の要素が含まれたJSONがPUSHされます。

* Anonymity
* IsCaster
* UserId
* ProfName（空文字列）
* Name
* NickName（空文字列） 
* Premium
* No
* Message
* Mail

profNameとNickNameは空文字列になっています


使い方
------
Unityで使うなら以下のスクリプトを使うと便利です。
https://github.com/TORISOUP/CommentViewer2Unity

