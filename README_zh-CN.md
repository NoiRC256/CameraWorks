# CameraWorks 简介 #

高自由度、支持长距离影像拍摄的试验型摄影工具。

![example1](https://i.ibb.co/HP24pKM/CW-Demo-A1.png)

![example2](https://i.ibb.co/NZ2dQJ3/CW-Demo-A2.png)


## 特性 ##

 - 自由平滑运镜
 - 帧数独立
 - 支持各轴单独设定
 - 支持多种移动设定，能实现复杂运镜
 - 航拍模式 / 运动模式
 - LOD 加载
 - 隐藏 UI
 - 自定义按键
 - etc ...

## 注意 ##

※ 不建议使用此工具。使用此工具很可能导致封号。

### 环境 ###

- Visual Studio 2019

- 某 arpg 游戏的修改版 MelonLoader 

## 默认按键 ##

### 基础 ###
| Description | Key |
|--|--|
| 初始化 | F9 |
| 切换相机 | Insert |
| 切换UI （使用自由相机时有效）| F10 |
| 切换光标锁定 | Quote (") |

### 运镜控制 ###
| Description | Key |
|--|--|
| 移动 | I / J / K/ L
| 升降 | RightAlt / B |
| 旋转 | 鼠标移动
| 翻滚 顺时针 / 逆时针 | U / O |
| 翻滚角度重置 | Y |
| 俯仰 上 / 下 | P / Semicolon (;) |
| 快速移动 2x | G（长按） |
| 慢速移动 0.25x | V（长按）|
| 视场角 增 / 减 | 9 / 8 |
| 视场角重置 | 0 |

运动模式下，部分按键功能不同
| Description | Key |
|--|--|
| 翻滚 顺时针 / 逆时针 | 鼠标移动（横向） |
| 偏航 左 / 右 | U / O |

### 其它 ###
| Description | Key |
|--|--|
| 隐藏游戏 UI & HUD | PageDown |
| 隐藏伤害数字 HUD | RightBracket (]) |
| 隐藏其它单位血量显示 HUD | LeftBracket ([) |
| 时间流速 增 / 减 0.1 | Up / Down|
| 时间流速 增 / 减 0.5 | Left / Right |
| 切换时间流速 5.0x | CapsLock |
| 暂停 | Delete |
| 时间流速 重置 | End |
| 分辨率切换至 4K | KeypadPlus (+)|
| 分辨率切换至 1080p | KeypadMinus (-)|




按键可在 `\UserData` 路径下的 `MelonPreferences.cfg` 进行设置。

按键 KeyCode 参照表：https://docs.unity3d.com/ScriptReference/KeyCode.html 

<br>

---
派生自 portra400nc 的 CameraTools: https://github.com/portra400nc/CameraTools
