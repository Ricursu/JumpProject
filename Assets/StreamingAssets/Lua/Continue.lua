Continue = {}
local this = Continue
function this.Restart(obj)
	local reStartBtn = obj.transform:Find("Restart").gameObject
	UIEvent.AddButtonOnClick(reStartBtn,RestartOnClick)

end

function RestartOnClick()
	SceneManagement.SceneManager.LoadScene("Jump")
	Time.timeScale = 1
end