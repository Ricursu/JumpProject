BoxController = {}
local this = BoxController
local player
local plane
local currentBox
local randomScale
local boxPrefab
local orginScale
local maxDistance = 3

function this.Awake(obj)
	currentBox = obj
	orginScale = currentBox.transform.localScale
	player = GameObject.Find("Player")
	plane = GameObject.Find("Plane")
	this.GenerateBox()
end

function this.Update(obj)
    if this.IsInView(obj.transform.position) then
        GameObject.Destroy(obj, 1)
    end

	if Input.touchCount == 0 then
		return
	end

	if Input.GetKey(KeyCode.Space) or Input.GetTouch(0).phase == TouchPhase.Stationary then
		if currentBox.transform.localScale.y < 0.6 and currentBox.transform.localScale.y>0.39 then
			currentBox.transform.localScale = currentBox.transform.localScale + Vector3(0, -1, 0) * 0.15 * Time.deltaTime
			--currentBox.transform.localPosition = currentBox.transform.localPosition + Vector3(0, -1, 0) * 0.15 * Time.deltaTime
		end
	end

	if Input.GetKeyUp(KeyCode.Space) or Input.GetTouch(0).phase == TouchPhase.Ended then
        --Util.DoLocalMoveY(currentBox, 0.25, 0.2)
        Util.DoScale(currentBox, Vector3(orginScale.x, orginScale.y, orginScale.z), 0.2)
    end
end

function this.IsInView(worldPos)
	local cameraTrans = Camera.main.transform
	local viewPos = Camera.main:WorldToViewportPoint(worldPos)
	local dir = (worldPos - cameraTrans.position).normalized
	local dot = Vector3.Dot(cameraTrans.forward,dir)

	if dot>0 and viewPos.x > 0 and viewPos.x < 1 and viewPos.y > 0 and viewPos.y < 1 then
		return false
	end
	return true
end

function this.GenerateBox()
	boxPrefab = Resources.Load("Prefab/Cube")
	local newBox = GameObject.Instantiate(boxPrefab)

	randomScale = Util.Random(0.5,1)
	newBox.transform.position = currentBox.transform.position + Vector3(-Util.Random(1.5, maxDistance), 0, 0)
	plane.transform.localPosition = plane.transform.localPosition + Vector3(-Util.Random(1.5, maxDistance), 0, 0)

	newBox.transform.localScale = Vector3(randomScale, 0.5, randomScale)
    newBox : GetComponent('Renderer').material.color = Color(Util.Random(0.0, 1.0), Util.Random(0.0, 1.0), Util.Random(0.0, 1.0))
end