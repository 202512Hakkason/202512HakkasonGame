# pose_estimate.py

"""
Mediapipeのモジュール
"""

import cv2
import mediapipe as mp

# MediaPipe Poseモデルの初期化
mp_pose = mp.solutions.pose
pose = mp_pose.Pose(static_image_mode=False, # 動画ストリームの設定
					model_complexity=1, # 1: 高精度, 0: 軽量
					enable_segmentation=False, # セグメンテーション無効
					min_detection_confidence=0.5, # 検出信頼度閾値
					min_tracking_confidence=0.5) # トラッキング信頼度閾値

def estimate_pose_frame(frame):
	"""
	1フレーム画像に対して骨格推定を行い、ランドマークを描画した画像を返す
	"""
	# BGR画像をRGBに変換
	image_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
	# 骨格推定の実行
	results = pose.process(image_rgb)
	# ランドマークの描画
	if results.pose_landmarks:
		mp.solutions.drawing_utils.draw_landmarks(
			frame, results.pose_landmarks, mp_pose.POSE_CONNECTIONS)
	# 画像を返す
	return frame