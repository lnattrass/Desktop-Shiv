Desktop-Shiv
============

Open applications in a new desktop view.


Desktop-Shiv allows you to start an executable in a New Desktop.
This has similar functionality to UAC's Secure Desktop, in that all other windows are not visible.

The code primarily utilises the user32.dll API's CreateDesktop,SwitchDesktop and CloseDesktop.
It also has to use the Kernel32.dll API's CreateProcess to support creating the process in a named desktop.

Usage
----

Desktop-Shiv requires one argument, which is the command to run on the new desktop.

```
desktop-shiv C:\Windows\System32\calc.exe
```

It also has the following switches
```
Usage: Desktop-Shiv [OPTIONS]+ Command Line
Open an application on a new Win32 desktop.

Options:
  -n, --name=VALUE           The name of the desktop you will run your command in. [Defaults to DesktopShiv]
  -t, --timeout=VALUE        Timeout in seconds waiting for the user to respond on the secondary desktop. [Defaults to INFINITY]
  -h, -?, --help             Show this message and exit
  -s, --suppress             Dont actually create the alternate desktop. Useful for testing your command_line if necessary.
  -d, --debug                Print debug logging stuff.

```

Thanks
---
 * [NDesk.Options] - Awesome options parser for C#
 * [PInvoke] - Interop signatures used to access API's

License
---
Licensed under the [MIT] License

[MIT]:http://opensource.org/licenses/mit-license.php
[NDesk.Options]:http://www.ndesk.org/Options
[Pinvoke]:http://pinvoke.net/
