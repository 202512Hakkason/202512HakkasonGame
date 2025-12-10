# main.py

"""
input動画に対して骨格推定を行い、output動画として保存する
"""

import cv2
import os
from pose_estimater import estimate_pose_frame
from config import PATHS

def main():
	# 入力動画の存在確認
	INPUT_PATH = PATHS["INPUT_PATH"]
	OUTPUT_PATH = PATHS["OUTPUT_PATH"]
	
	if not os.path.exists(INPUT_PATH):
		print(f"入力動画が見つかりません: {INPUT_PATH}")
		return
	
    # 動画の読み込みと書き込みの準備
	cap = cv2.VideoCapture(INPUT_PATH)
	width = int(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
	height = int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))
	fps = cap.get(cv2.CAP_PROP_FPS)
	fourcc = cv2.VideoWriter_fourcc(*'mp4v')
	os.makedirs(os.path.dirname(OUTPUT_PATH), exist_ok=True)
	out = cv2.VideoWriter(OUTPUT_PATH, fourcc, fps, (width, height))

	while cap.isOpened():
		ret, frame = cap.read()
		if not ret:
			break
		# 骨格推定
		result_frame = estimate_pose_frame(frame)
		out.write(result_frame)

	cap.release()
	out.release()
	print(f"推定結果を保存しました: {OUTPUT_PATH}")

if __name__ == "__main__":
	main()