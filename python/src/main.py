# main.py

"""
input動画に対して骨格推定を行い、output動画として保存する
"""

import cv2
import os
from pose_estimater import estimate_pose_and_landmarks
from sound_translater import y_to_scale_index
from udp_sender import UDPSender
from config import PATHS, UDP, SCALE_NUM, CAMERA_ID
from camera_capture import CameraCapture

def main():

    # カメラ映像の取得
    # id=0は内蔵カメラ
    camera = CameraCapture(camera_id=CAMERA_ID)
    camera.open()
    # fordebug
    print("Camera opened.")

    # UDP送信初期化
    sender = UDPSender(UDP["IP"], UDP["PORT"])
    # fordebug
    print(f"UDP sender initialized to {UDP['IP']}:{UDP['PORT']}")

    frame_count = 0
    while True:
        ret, frame = camera.read()
        if not ret:
            break
        frame_count += 1
        # 10フレームごとに骨格推定・UDP送信
        if frame_count % 10 == 0:
            result_frame, landmarks = estimate_pose_and_landmarks(frame)
            if len(landmarks) > 16:
                wrist_y = landmarks[16][1]
                sender.send(str(wrist_y))

    camera.release()
if __name__ == "__main__":
	main()