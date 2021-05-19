--主入口函数。从这里开始lua逻辑
--全局变量
function Main()
	Debugger.Log("\nMain: \n=================\n 新的Main.lua \n===================")
	GameObject = UnityEngine.GameObject
	Transform = UnityEngine.Transform
	ParticleSystem = UnityEngine.ParticleSystem
	Color = UnityEngine.Color
	Util = Util.New()
	SceneManagement = UnityEngine.SceneManagement
	Input = UnityEngine.Input
	KeyCode = UnityEngine.KeyCode
	Time = UnityEngine.Time
	Camera = UnityEngine.Camera
	AudioSource = UnityEngine.AudioSource
	Resources = UnityEngine.Resources
	Rigidbody = UnityEngine.Rigidbody
	www = UnityEngine.WWW		
	print("logic start")	 		

	Directory = System.IO.Directory
	Path = System.IO.Path
	Application = UnityEngine.Application
	File = System.IO.File
	-- Encoding = System.Text.Encoding

	HotUpdateClass = HotUpdate.New()
	Encoding = EncodingUtils.New()
	UnZipTool = UnZipTool.New()
	FileUtils = FileUtils.New()
	WebUtils = WebUtils.New()
	DiffUtils = DiffUtils.New()
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()
end