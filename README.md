# Overview #

CameraWorks is an experimental camera utility with LOD loading and granular controls, fit for capturing continuous footage over long distances.

Requires MelonLoader.

![example1](https://i.ibb.co/HP24pKM/CW-Demo-A1.png)

![example2](https://i.ibb.co/NZ2dQJ3/CW-Demo-A2.png)

## Features ##

 - Observer camera
 - Smoothed framerate-independent movement, rotation, field of view
 - Per-axis motion settings
 - Different movement modes: align rotation with world plane, align movement with world plane, etc.
 - Optional flight control scheme
 - LOD loading, useful for long-distance aerial footage
 - UI & HUD toggle
 - Configurable hotkeys
 - etc ...

## Note ##
It is highly recommended to **NOT** use any kind of third-party software for anime game.  
Using this may get you banned. Use at your own risk.

### Environment ###

- Visual Studio 2019 for building

- MelonLoader modified for anime game

## Hotkeys ##

### General ###
| Description | Key |
|--|--|
| Initialize | F9 |
| Toggle Free Camera | Insert |
| Toggle UI (While in Free Camera is enabled)| F10 |
| Toggle Cursor Focus | Quote (") |

### Camera Controls ###
| Description | Key |
|--|--|
| Movement | I / J / K/ L
| Vertical Movement Up / Down | RightAlt / B |
| Look | Cursor Move |
| Roll Left / Right | U / O |
| Reset Roll | Y |
| Pitch Up / Down | P / Semicolon (;) |
| Fast Camera Movement | G (hold) |
| Slow Camera Movement | V (hold) |
| Field of View Increase / Decrease | 9 / 8 |
| Reset Field of View | 0 |

### Camera Controls - Advanced Mode Override ###
| Description | Key |
|--|--|
| Roll Left / Right | Cursor Move (horizontal) |
| Yaw Left / Right | U / O |

### Functionality ###
| Description | Key |
|--|--|
| Toggle Game HUD | PageDown |
| Toggle Game Damage HUD | RightBracket (]) |
| Remove Enemy HP HUD | LeftBracket ([) |
| Increase / Decrease Game Speed by 0.1 | Up / Down|
| Increase / Decrease Game Speed by 0.5 | Left / Right |
| Toggle Game Speed to 5.0 | CapsLock |
| Toggle Pause Game | Delete |
| Reset Game Speed | End |
| Set Screen Resolution to 3840x2160 | KeypadPlus (+)|
| Set Screen Resolution to 1920x1080 | KeypadMinus (-)|




You can customize the hotkeys by editing `MelonPreferences.cfg` located in `\UserData`. 
Refer to this list of key codes: https://docs.unity3d.com/ScriptReference/KeyCode.html 

<br>

---
If you want a ligher alternative, check out portra400nc's [CameraTools](https://github.com/portra400nc/CameraTools) (Now ported to [Akebi-GC](https://github.com/Akebi-Group/Akebi-GC)). CameraTools is a free camera utility with intuitive controls.
