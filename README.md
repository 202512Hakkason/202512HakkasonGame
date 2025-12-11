# 202512HakkasonGame

## 前提環境

本リポジトリの実行環境は以下を前提としています。

* Windows 11
* Python がインストール済みであること（3.x 系推奨）
* `$` で始まる行はコマンドプロンプトで入力するコマンドを表します。

## リポジトリのクローン

本リポジトリは GitHub 等にホストされている前提です。
クローン方法は CVIML（仮） または一般的な Git クローン手順を参考にしてください。

## 仮想環境の作成と起動

### 1. クローンしたリポジトリの場所を確認

例：
C:\Users(ユーザ名)\repos\202512HakkasonGame

### 2. コマンドプロンプトでリポジトリに移動

$ cd C:\Users(ユーザ名)\repos\202512HakkasonGame

### 3. python フォルダに移動

$ cd python

### 4. 仮想環境の作成

以下のコマンドを実行すると venv フォルダが作成されます。
$ python -m venv venv

### 5. 仮想環境の起動

$ .\venv\Scripts\activate.bat

成功すると、コマンドプロンプトの左に (venv) が表示されます。
例：
(venv) C:\Users...

### 6. 必要なライブラリのインストール

正しいコマンドは以下です。
$ pip install -r requirements.txt

## VSCode の起動

現在のフォルダを VSCode で開きます。
$ code .

## main.py の実行

$ python main.py

必要な要素の追加や細かい説明も付けられるので、必要なら言ってください。

