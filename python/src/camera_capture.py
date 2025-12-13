# camera_capture.py
import cv2
from typing import Optional

class CameraCapture:
	"""
	Webカメラデバイスから映像を取得するクラス。
	カメラのオープン・フレーム取得・解放を簡単に扱えるようにラップしている。
	"""
	def __init__(self, camera_id: int = 0) -> None:
		"""
		camera_id: 使用するカメラデバイス番号（通常は0が内蔵カメラ）
		"""
		self.camera_id = camera_id
		self.cap: Optional[cv2.VideoCapture] = None  # カメラキャプチャオブジェクト

	def open(self) -> None:
		"""
		カメラデバイスをオープンする。
		"""
		self.cap = cv2.VideoCapture(self.camera_id)

	def read(self):
		"""
		1フレーム分の画像を取得する。
		Returns:
			ret (bool): 画像取得成功フラグ
			frame (ndarray): 取得した画像
		Raises:
			RuntimeError: カメラがオープンされていない場合
		"""
		if self.cap is None:
			raise RuntimeError("カメラがオープンされていません")
		return self.cap.read()

	def release(self) -> None:
		"""
		カメラデバイスを解放する。
		"""
		if self.cap is not None:
			self.cap.release()
			self.cap = None