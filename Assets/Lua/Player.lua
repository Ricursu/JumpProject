Player = {}
local this = Player
require("Continue")
require("Login")
local player
local rigidbody
local body
local head
local particle
local cameraRelativePosition

function this.Awake(object)
	print("Player")
	player = object
	rigidbody = player:GetComponent("Rigidbody")
	head = player.transform:Find("Head").gameObject
	body = player.transform : Find('Body').gameObject
	ui = GameObject.Find("UI").gameObject
	particle = GameObject.Find("Particle")
	particle:GetComponent("ParticleSystem"):Stop()
	cameraRelativePosition = Camera.main.transform.position - player.transform.position
	ui:SetActive(false)
end

function this.StartJump(time)
	rigidbody:AddForce(Vector3(1,1,0)*time*7,UnityEngine.ForceMode.Impulse)
end

function this.Update()
	if Input.GetKeyDown(KeyCode.Space) then
		startTime = Time.time
		particle:GetComponent("ParticleSystem"):Play()
	end

	if Input.GetKeyUp(KeyCode.Space) then
		Util.DoScale(body,Vector3(0.25,0.5,0.25),0.5)
		Util.DoLocalMoveY(head,0.188,0.5)
		endTime = Time.time - startTime
		particle : GetComponent('ParticleSystem') : Stop()
	    this.StartJump(endTime)
	end

	if Input.GetKey(KeyCode.Space) then
		if body.transform.localScale.y < 0.51 and body.transform.localScale.y > 0.3 then
			body.transform.localScale = body.transform.localScale+Vector3(0,-1,0) * 0.05 * Time.deltaTime
			head.transform.localPosition = head.transform.localPosition+Vector3(0,-1,0) * 0.05 * Time.deltaTime
		end
	end

end

function this.OnCollisionStay(object)
	if object.transform.tag == "Cube" then
		Debugger.Log("OnCollisionStay Update")
		if(object:GetComponent("BoxControl")==nil) then
			this.CameraMove()
			object:AddComponent(typeof(BoxControl))
		end
	elseif object.transform.tag == "Plane" then
		Time.timeScale = 0
		ui:SetActive(true)
		Continue.Restart(ui)
	end
end

function this.CameraMove()
	Util.DoMove(Camera.main,(player.transform.position+cameraRelativePosition),1)
end