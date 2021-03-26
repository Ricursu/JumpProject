Login = {}
local this = Login
require('Music')
local ui
local manager
function this.Awake(object)
    Debugger.Log("=================\n=================\n=================\n 新的Awake \n===================\n===================\n===================")
    manager = GameObject.Find('Manager')
    manager : AddComponent(typeof(AudioSource))
    coroutine.start(Music.PlaySound)--协程开始
    ui = object
    local loginBtn = ui.transform : Find("Login").gameObject
    loginBtn:GetComponent("Button").onClick:AddListener(LoginOnClick)
end

function LoginOnClick()
    SceneManagement.SceneManager.LoadScene("Jump")
end

