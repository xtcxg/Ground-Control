# Ground-Control
## 当完成时应当达到
### 1. 通过输入预设的或自定义的命令，执行相应的脚本，完成预期的动作。命令基本格式为:

  > \<cmd\> \<args\> \<prop\>

  - cmd：命令，提供默认值，可由用户设置别名。（op = open）
  - args：参数,由用户输入，可以预设参数。（git = -u https://github.com）
  - prop：配置信息，提供默认值，也可以由用户设置（非输入），在执行脚本时自动携带，由<[{prop}]>包裹。(browser=chrome)

脚本中的变量应当提供默认别名功能（预设的变量）

  例如原始命令为 
  > open -u https://github.com
  
  __cmd__ - <span style="background:rgba(255,255,240,0.1)">opensite</span>

  __args__ - <span style="background:rgba(255,255,240,0.1)">-u https://github.com</span>

  __prop__ - <span style="background:rgba(255,255,240,0.1)">browser=chrome</span>

  通过设置命令 op = open ,预设参数 git = -u https://github.com

  用户输入的命令为
  > op git
  
  实际完整的命令为
  > open -u https://github.com

  脚本实际收到的命令为
  > open -u https://github.com <[{browser=chrome}]>

### 2. 在工具栏提供入口，参考工具栏中的 cortana 与 搜索
### 3. 通过快捷键 ctrl + i 调出命令输入栏
### 4. 不同的命令处理脚本，放在主目录 __plugs__ 文件夹中，脚本应当提供两个基本的文件

  - app.ps1 或者 app.bat (脚本运行入口)
  - cfg.json (配置信息)

<pre style="font-family:Serif">
-| ./
-| ./plugs
- -| ./open
- - -| ./app.ps1
- - -| ./cfg.json
</pre>
