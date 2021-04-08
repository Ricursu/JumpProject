BoxController = {}
fxnumber = 0
fxnumber2 = 0
type = 0
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
	if Input.GetKey(KeyCode.Space) then
		if currentBox.transform.localScale.y < 0.6 and currentBox.transform.localScale.y>0.39 then
			currentBox.transform.localScale = currentBox.transform.localScale + Vector3(0, -1, 0) * 0.15 * Time.deltaTime
			--currentBox.transform.localPosition = currentBox.transform.localPosition + Vector3(0, -1, 0) * 0.15 * Time.deltaTime
		end
	end

	if Input.GetKeyUp(KeyCode.Space) then
        --Util.DoLocalMoveY(currentBox, 0.25, 0.2)
        Util.DoScale(currentBox, Vector3(orginScale.x, orginScale.y, orginScale.z), 0.2)

    end
    if this.IsInView(obj.transform.position) then
        GameObject.Destroy(obj, 1)
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
--选择下一跳的方块的距离和方向
function this.chosefx()
                --根据type判断此时player向前跳还是向右跳
                if (type == 0) then
                    c=math.random(0,1)
                    --生成随机数大小决定下一个方块是放在前面还是右边
                    if c<=0.5 then
                          fxnumber = math.random(1.5, maxDistance)
                          fxnumber2 = 0
                    end
                    if c>0.5 then
                          fxnumber2 = math.random(1.5, maxDistance)
                          fxnumber = 0
                   end
                   if (fxnumber2 ~= 0) then
                      type =1
                   end
                end
                if (type == 1) then
                   c=math.random(0,1)
                   if c<=0.5 then
                          fxnumber2 = 0-math.random(1.5, maxDistance)
                          fxnumber = 0
                   end
                   if c>0.5 then
                          fxnumber = math.random(1.5, maxDistance)
                          fxnumber2 = 0
                   end
                   if (fxnumber ~= 0) then
                      type =0
                   end
                end
                return fxnumber,fxnumber2
end

function this.GenerateBox()
	boxPrefab = Resources.Load("Prefab/Cube")
	local newBox = GameObject.Instantiate(boxPrefab)

	randomScale = Util.Random(0.5,1)
                fx,fx2=this.chosefx()
	newBox.transform.position = currentBox.transform.position + Vector3(fx, 0, fx2)
	plane.transform.localPosition = plane.transform.localPosition + Vector3(fx, 0, fx2)

	newBox.transform.localScale = Vector3(randomScale, 0.5, randomScale)
    newBox : GetComponent('Renderer').material.color = Color(Util.Random(0.0, 1.0), Util.Random(0.0, 1.0), Util.Random(0.0, 1.0))
end