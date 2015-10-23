﻿NCV2MikuMikuMouth
===============
NCV2MikuMikuMouthはNiconamaCommentViewer用プラグインです。  
ニコ生のコメントをSocket接続された対象にコメント情報を逐次PUSH通知します。

機能
-------
起動するとポート*50082*でTCP Socketの待受を開始します。
ここにTCPで繋ぐことでニコ生のコメントを受け取ることができます。  コメント情報は新しいコメントが来る度に以下の要素が含まれたJSONがPUSHされます。

* name
* text
* emotion
* tag
* isInterrupted

例

```
{
    "name": "ゆかり",
    "text": "こんにちはみなさん",
    "emotion": "greeting",
    "tag": "white",
    "isInterrupted": false,
}

使い方
------
Unityで使うなら以下のスクリプトを使うと便利です。
https://github.com/TORISOUP/CommentViewer2Unity

