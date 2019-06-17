KMCCC
=====

An OpenSource Minecraft Launcher for .Net Developers

## KMCCC.Shared

Shared Code Between KMCCC.Basic & KMCCC.Pro

### Included:

- Basic Launch Model (LauncherCore, LaunchOptions, ...)
- Authentication Model (Yggdrasil, Offline, ...)
- Java & System Information Finder
- Zip Module using .Net Internal API ( MS.Internal.IO.Zip.* )
- Basic VersionLocator (JVersion)
- A set of useful extensions
- A launch reporter that can be disabled
- Custom authentication server

## KMCCC

Basic Version of KMCCC

### Included:

- Everything in kMCCC.Shared
- No more

# Sample

## How to initialize a LauncherCore

```csharp

LauncherCore core = LauncherCore.Create(
	new LauncherCoreCreationOption(
		javaPath: Config.Instance.JavaPath, // by default it will be the first version finded
		gameRootPath: null, // by defualt it will be ./.minecraft/
		versionLocator: the Version Locator // by default it will be new JVersionLocator()
	));

```

## How to find Versions

```csharp

var versions = core.GetVersions();

var version = core.GetVersion("1.8");

```

*unlaunchable Version will be ignored*

## How to launch Minecraft


```csharp
var result = core.Launch(new LaunchOptions
{
	Version = App.LauncherCore.GetVersion(server.VersionId)
	Authenticator = new OfflineAuthenticator("Steve"), // offline
	//Authenticator = new YggdrasilLogin("*@*.*", "***", true), // online
	MaxMemory = Config.Instance.MaxMemory, // optional
	MinMemory = Config.Instance.MaxMemory, // optional
	Mode = LaunchMode.MCLauncher, // optional
	Server = new ServerInfo {Address = "mc.hypixel.net"}, //optional
	Size = new WindowSize {Height = 768, Width = 1280} //optional
}, (Action<MinecraftLaunchArguments>) (x => { })); // optional ( modify arguments before launching
```

# Enjoy!
