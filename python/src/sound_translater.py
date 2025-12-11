# sound_translater.py
from typing import Optional


def y_to_scale_index(y: float, height: int, scale_num: int) -> Optional[int]:
	"""
	Y座標（0.0～1.0）を画像高さ方向でscale_num分割し、
	下から0,1,2...の音階インデックスを返す
	Args:
		y (float): 手首のY座標（0.0～1.0）
		height (int): 画像高さ
		scale_num (int): 音階数
	Returns:
		Optional[int]: 音階インデックス（0～scale_num-1）
	"""
	# 範囲外チェック
	if y < 0 or y > 1:
		return None
	# 画像下端が0、上端がscale_num-1となるように分割
	# ピクセルの座標に変換
	pixel_y = int(y * height)
	# 音階の幅を計算
	band_height = height / scale_num
	# 音階インデックスを計算
	index = int(pixel_y // band_height)
	return min(max(index, 0), scale_num - 1)

