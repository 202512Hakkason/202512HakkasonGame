# config.py
"""
設定ファイル
パスやパラメータをここで管理
"""

PATHS = {
    "INPUT_PATH": "../videos/input/test01.mp4",   # 入力動画のパス
    "OUTPUT_PATH": "../videos/output/test01.mp4"  # 出力動画のパス
}

# UDP送信先
UDP_IP = "127.0.0.1"
UDP_PORT = 5005

# 音階数
SCALE_NUM = 8