Login = {}
local this = Login
require('Music')
local ui
local manager
function this.Awake(object)
    print("Login")
    manager = GameObject.Find('Manager')
    manager : AddComponent(typeof(AudioSource))
    coroutine.start(Music.PlaySound)--协程开始
    ui = object
    local loginBtn = ui.transform : Find("Login").gameObject
    Debugger.Log(loginBtn)
    UIEvent.AddButtonOnClick(loginBtn, LoginOnClick)
end

function LoginOnClick()
    SceneManagement.SceneManager.LoadScene("Jump")
end

