# main.py

"""
input動画に対して骨格推定を行い、output動画として保存する
"""

import cv2
import os
from pose_estimater import estimate_pose_and_landmarks
from sound_translater import y_to_scale_index
from udp_sender import UDPSender
from config import PATHS, UDP_IP, UDP_PORT, SCALE_NUM

def main():
	# 入力動画の存在確認
	INPUT_PATH = PATHS["INPUT_PATH"]
	OUTPUT_PATH = PATHS["OUTPUT_PATH"]

	if not os.path.exists(INPUT_PATH):
		print(f"入力動画が見つかりません: {INPUT_PATH}")
		return

	# ---動画の読み込みと書き込みの準備
	cap = cv2.VideoCapture(INPUT_PATH)
	# 動画の基本情報取得
	width = int(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
	height = int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))
	fps = cap.get(cv2.CAP_PROP_FPS)
	# 動画書き込み準備
	fourcc = cv2.VideoWriter_fourcc(*'mp4v')
	os.makedirs(os.path.dirname(OUTPUT_PATH), exist_ok=True)
	out = cv2.VideoWriter(OUTPUT_PATH, fourcc, fps, (width, height))

	# UDP送信初期化
	sender = UDPSender(UDP_IP, UDP_PORT)

	while cap.isOpened():
		ret, frame = cap.read()
		if not ret:
			break
		# 骨格推定（描画画像とランドマークリストを同時取得）
		result_frame, landmarks = estimate_pose_and_landmarks(frame)
		# 右手首（16番）のY座標をUDPでそのまま送信
		if len(landmarks) > 16:
			wrist_y = landmarks[16][1]
			sender.send(str(wrist_y))
		out.write(result_frame)

	cap.release()
	out.release()
	print(f"推定結果を保存しました: {OUTPUT_PATH}")

if __name__ == "__main__":
	main()