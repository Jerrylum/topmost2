<p align="center">
	<a href="https://github.com/jerrylum/topmost2"><img src="https://i.imgur.com/r7PW6a2.png" alt="IntroIcon" width="100"></a>
</p>
<h3 align="center">TopMost2</h3>
<p align="center">This tool allows you to make any windows always on top.</p>

<h4 align="center"><a href="https://github.com/jerrylum/topmost2/releases">Download Now</a></h4>

---

### Double Click

Double Click the tray icon, make the current selected window stay on top.

<h5 align="left">
<img src="https://i.imgur.com/kuBflkz.gif">
</h5>

<br>

### Global Hotkey

Use the default hotkey `Ctrl + Alt + Space` to make the current selected window stay on top.

<h5 align="left">
<img src="https://i.imgur.com/NokjMLd.gif">
</h5>
<br>

### Change The Hotkey

Right click the tray icon and go to `options` page to change the hotkey to your own favorite combination.

<h5 align="left">
<img src="https://i.imgur.com/LfNdpHR.gif">
</h5>

<br>

### Window List

Click on a menu item from the `Window List` to pin or unpin any windows.

<h5 align="left">
<img src="https://i.imgur.com/6KIfi3d.gif">
</h5>

<br>

### Other Features

- Dynamic icon
- Clear all function
- Automatically startup option
- Enable/Disable keyboard shortcut option
- Freely customizable hotkey
- Global hotkey
- Command-line support
- High compatibility with other programs  
- Negligible system resources usage



---

### Why I need this?

`Topmost` or `Always On Top` is a property of every window you see on your computer. A window whose Topmost property is set to `true` appears above all windows whose Topmost properties are set to `false`.  <br>

Many windows applications don’t offer an option to make itself topmost. When you are browsing multiple windows at the same time, this may make you annoyed by frequent switching to different windows. With TopMost2,  you can add this feature to any applications and solve the above problem.



### Details

- **Tray Icon**  
  The color of the icon represents the top-most state of the current selected window.  
  🟥RED = Normal, 🟩GREEN = Top-most
  
- **Clear All Function**  
  This function will set all windows to normal states.
  
- **Elevated Privileges**  
  If you are trying to set an elevated window, TopMost2 will ask you to elevate the privileges in order to have higher permission to finish the action. Obviously, the reason is that they are protected by the operating system. You can also start TopMost2 as administrator to avoid the above problem.
  
- **Hotkey**  
  You can freely set any hotkey combination. By clicking the `Edit` button, you can then press a new combination. After that, click `Done` to finish. If you leave or close the option form. The hotkey setting will be auto-saved by the system.  
  ![Hot Key Demo](https://i.imgur.com/jGFi1tC.gif)  
  If TopMost2 starts with normal permission, it may not be able to listen to the keyboard in the elevated window.

- **Exit**  
  This function will set all windows to normal state and shut down the program.


### Command Line

Usage:

```powershell
.\topmost2 [--autostart] {action hWnd}
```

**action:**

- Set top-most: `--set` or `-S` or `/S`
- Remove top-most: `--remove` or `-R` or `/R`

**hWnd:**

The handle pointer in hexadecimal. HWND is a "handle to a window" and is part of the Win32 API.

<br>

For example:

```powershell
.\topmost2 -S 0x311A0 -S 0x190D4E
.\topmost2 -R 0x311A0
```



---

### Other Software

There is some software on the Internet are doing the same thing too. Like [DeskPins](https://efotinis.neocities.org/deskpins/) and [Window TopMost Control](https://www.sordum.org/9182/window-topmost-control-v1-2/). I am trying to compare with them in several ways. Keep in mind, this is not a strict comparison and you might have your own answer base on your daily need and style.



|                                                 | TopMost2          | DeskPins                 | Window TopMost Control |
| ----------------------------------------------- | ----------------- | ------------------------ | ---------------------- |
| Set Elevated application's Window <sup>#0</sup> | ✔️                 | ✔️ <sup>#1</sup>          | ✔️                      |
| Command Line Support                            | ✔️                 | ❌                        | ✔️                      |
| Portable                                        | ✔️                 | ❌                        | ✔️                      |
| Auto Start                                      | ✔️                 | ❌                        | ✔️                      |
| Auto Pin                                        | ❌                 | ✔️                        | ❌                      |
| Open Source                                     | ✔️                 | ✔️                        | ❌                      |
| State visibility                                | 🟡Good             | 🟢Excellent <sup>#2</sup> | 🟠Limited <sup>#3</sup> |
| CPU Usage                                       | 🟢Least            | 🟠Highest                 | 🟡Medium                |
| Customize                                       | 🟡Good             | 🟢Excellent               | 🟡Good                  |
| Compatibility With Programs                     | 🟢Excellent        | 🟡Good <sup>#4</sup>      | 🟢Excellent             |
| Hotkey                                          | More Combinations | More shortcuts           | Limited                |
| Size                                            | 47KB              | 103KB                    | 680KB                  |

#0 Able to change a window that belongs to a process with elevated privileges (run as the administrator).  
#1 Only if the application starts as administrator. Otherwise, trying to do that will cause unknown behavior.  
#2 Pin icon at the top-right corner of the top-most window.  
#3 Only provide the "Window List" feature.  
#4 Not Compatible with windows which also have top-most setting.  

<br> 

---

### Download

Please go to [the release page](https://github.com/jerrylum/topmost2/releases) to download the latest version.  
This tool requires .Net Framework 4.7.2 (or above). Support Windows 7 SP1 or later.  

<br>

### Special Thanks

Thank you SamNg and Theo for their suggestion and testing the software.
