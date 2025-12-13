# udp_sender.py
import socket
from typing import Tuple

class UDPSender:
	"""
	UDP通信でデータを送信するラッパークラス
	"""
	def __init__(self, ip: str, port: int) -> None:
		self.address: Tuple[str, int] = (ip, port)
		self.sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

	def send(self, message: str) -> None:
		"""
		指定したメッセージをUDPで送信
		Args:
			message (str): 送信するデータ
		"""
		self.sock.sendto(message.encode('utf-8'), self.address)
		# pythonのターミナルログにデバッグ用出力
		print(f"Sent UDP message to {self.address}: {message}")
