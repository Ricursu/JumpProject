TestPlayer = {}
local this = TestPlayer

local testPlayer
local rigidbody
local force 

function this.Start()
	testPlayer = GameObject.Find("testPlayer(Clone)")
	testPlayer:AddComponent(typeof(Rigidbody))
	rigidbody = testPlayer:GetComponent("Rigidbody")
	force = 5
end

function this.Update()
	local h = Input.GetAxis("Horizontal")
	local v = Input.GetAxis("Vertical")
	rigidbody:AddForce(Vector3(h,0,v)*force)
end