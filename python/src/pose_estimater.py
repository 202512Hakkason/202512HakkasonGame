# pose_estimater.py

"""
Mediapipeのモジュール
"""


import cv2
import mediapipe as mp
from typing import List, Tuple

# MediaPipe Poseモデルの初期化
mp_pose = mp.solutions.pose
pose = mp_pose.Pose(
	static_image_mode=False,  # 動画ストリームの設定
	model_complexity=1,       # 1: 高精度, 0: 軽量
	enable_segmentation=False, # セグメンテーション無効
	min_detection_confidence=0.5, # 検出信頼度閾値
	min_tracking_confidence=0.5   # トラッキング信頼度閾値
)


def estimate_pose_and_landmarks(frame: cv2.Mat) -> Tuple[cv2.Mat, List[Tuple[float, float, float, float]]]:
    """
    1フレーム画像に対して骨格推定を1回だけ行い、
    ランドマークを描画した画像とランドマークリストを返す
    Args:
        frame (cv2.Mat): 入力画像
    Returns:
        Tuple[cv2.Mat, List[Tuple[float, float, float, float]]]:
            (ランドマーク描画済み画像, (x, y, z, visibility)のリスト)
    """
    # frame画像をRGBに変換してから処理
    image_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    # 骨格推定の実行
    results = pose.process(image_rgb)
    # ランドマーク描画とリスト作成
    landmarks = []
    if results.pose_landmarks:
        # ランドマーク描画
        mp.solutions.drawing_utils.draw_landmarks(
            frame, results.pose_landmarks, mp_pose.POSE_CONNECTIONS)
        # ランドマークリスト作成
        for landmark in results.pose_landmarks.landmark:
            landmarks.append((landmark.x, landmark.y, landmark.z, landmark.visibility))
    return frame, landmarks