HotUpdate = {}
local this = HotUpdate
local cor = coroutine.create(
    function() 
        HotUpdate.HotUpdateProcess() 
    end
    );

function this.Awake(object)
    coroutine.resume(cor);
    -- coroutine.resume(cor);
end

function this.Update()
    if WebUtils.isDone then
        WebUtils.isDone = false;
        coroutine.resume(cor);
    end
end

function this.HotUpdateProcess()
    HotUpdateClass.ChangeLoadingimformation("检查更新中");
    HotUpdateClass.ClearSlider()
    -- HotUpdateClass.FullSlider()
    if not WebUtils.isDone then
        coroutine.yield()
    end
    -- 1、创建版本文件config.ini
    this.CreateVersionFile();
    -- 2、判断是否需要更新
    if not this.IsUpdate() then
        HotUpdateClass.ChangeLoadingimformation("目前为最新版本\n正在进入游戏");
        HotUpdateClass.ClearSlider()
        -- HotUpdateClass.FullSlider()
        if not WebUtils.isDone then
            coroutine.yield()
        end
        SceneManagement.SceneManager.LoadScene("MainMenu")
        return;
    end
    HotUpdateClass.ChangeLoadingimformation("检测本地资源");
    HotUpdateClass.ClearSlider()
    -- HotUpdateClass.FullSlider()
    if not WebUtils.isDone then
        coroutine.yield()
    end
    -- 3、判断是否需要释放APK
    if not FileUtils.DirectoryExists(Path.Combine(Application.persistentDataPath, "Lua")) then
        -- 3.1、从文件管理器中获取APK
        HotUpdateClass.ChangeLoadingimformation("获取本地资源");
        this.GetApkFromFileManager();
        if not WebUtils.isDone then
            coroutine.yield()
        end
        -- 3.2、 解压APK
        HotUpdateClass.ChangeLoadingimformation("释放本地资源");
        UnZipTool.UnZipApk(Application.persistentDataPath .. "/base.apk", Path.Combine(Application.persistentDataPath, "Lua"));
    end
    HotUpdateClass.ChangeLoadingimformation("本地资源加载完成");
    HotUpdateClass.ClearSlider()
    -- HotUpdateClass.FullSlider()
    if not WebUtils.isDone then
        coroutine.yield()
    end

    -- 4、获取服务器压缩文件
    HotUpdateClass.ChangeLoadingimformation("正在获取服务器端资源");
    this.GetZipFromServer();
    if not WebUtils.isDone then
        coroutine.yield()
    end

    -- 5、解压第4步中获取的压缩文件
    HotUpdateClass.ChangeLoadingimformation("释放服务器端资源");
    this.UnZipFile();
    HotUpdateClass.ClearSlider()
    -- HotUpdateClass.FullSlider()
    if not WebUtils.isDone then
        coroutine.yield()
    end
    -- 6、还原压缩文件中的差分包或将压缩文件中的Lua文件复制到本地文件
    HotUpdateClass.ChangeLoadingimformation("更新本地资源中");
    this.RedutionFile();
    HotUpdateClass.ClearSlider()
    -- HotUpdateClass.FullSlider()
    if not WebUtils.isDone then
        coroutine.yield()
    end

    -- 7、 更新本地版本文件version.ini的内容
    FileUtils.CreateFile(Path.Combine(Application.persistentDataPath, "config.ini"), WebUtils.GetByteFromServer("version.txt"));
    HotUpdateClass.ChangeLoadingimformation("更新完成");
    HotUpdateClass.ClearSlider()
    -- HotUpdateClass.FullSlider()
    if not WebUtils.isDone then
        coroutine.yield()
    end
    SceneManagement.SceneManager.LoadScene("MainMenu")
end

function this.CreateVersionFile()
    local filename = Path.Combine(Application.persistentDataPath, "config.ini");
    if FileUtils.FileExists(filename) then
        return;
    end
    FileUtils.CreateFile(filename, Encoding.GetBytes("version = " .. HotUpdateClass.mReleaseVersion .. "." .. HotUpdateClass.mMajorVersion));
end

function this.IsUpdate()
    if FileUtils.DirectoryExists(Application.persistentDataPath .. "/Temp") then
        FileUtils.DirectoryDelete(Application.persistentDataPath .. "/Temp", true);
    end
    local serverVersion = Encoding.GetString(WebUtils.GetByteFromServer("version.txt"));
    local localVersion = Encoding.GetString(FileUtils.ReadFileBytes(Path.Combine(Application.persistentDataPath, "config.ini")));
    local serverInfo = Split(serverVersion, '=');
    local localInfo = Split(localVersion, '=');
    if Trim(serverInfo[2]) == Trim(localInfo[2]) then
        return false;
    end
    return true;
end

function this.GetApkFromFileManager()
    WebUtils.GetApkFromFile();
end

function this.GetZipFromServer()
    local version = Encoding.GetString(FileUtils.ReadFileBytes(Path.Combine(Application.persistentDataPath, "config.ini")));
    local value = Split(version, '=');
    value[2] = Trim(value[2]);
    local serverVersion = Encoding.GetString(WebUtils.GetByteFromServer("version.txt"));
    local serverValue = Split(serverVersion, '=');
    serverValue[2] = Trim(serverValue[2]);

    
    local filename = "v" .. value[2] .. "-v".. serverValue[2] .. ".zip";
    WebUtils.GetFileFromServer(filename);
end

function this.UnZipFile()
    local version = Encoding.GetString(FileUtils.ReadFileBytes(Path.Combine(Application.persistentDataPath, "config.ini")));
    local value = Split(version, '=');
    value[2] = Trim(value[2]);
    local serverVersion = Encoding.GetString(WebUtils.GetByteFromServer("version.txt"));
    local serverValue = Split(serverVersion, '=');
    serverValue[2] = Trim(serverValue[2]);

    local filename = "v" .. value[2] .. "-v" .. serverValue[2] .. ".zip";
    UnZipTool.UnZip(filename, Path.Combine(Application.persistentDataPath, "Temp"));
end

function this.RedutionFile()
    local files = FileUtils.DirectoryGetFiles(Path.Combine(Application.persistentDataPath, "Temp"));
    Debugger.Log(files.Length)
    local i = 0
    while i < files.Length do
        if EndWith(files[i], ".lua") then
            local t = string.gsub(files[i],"Temp","Lua",1)
            FileUtils.CopyFileToPath(files[i], t);
        end
        i = i + 1
    end
    -- foreach (string file in files)
    -- if (file.EndsWith(".lua"))
    DiffUtils.ReductionFile(Path.Combine(Application.persistentDataPath, "Lua"), Path.Combine(Application.persistentDataPath, "Temp/filelist.txt"));
end

function Split(szFullString, szSeparator)
    local nFindStartIndex = 1
    local nSplitIndex = 1
    local nSplitArray = {}
    while true do
       local nFindLastIndex = string.find(szFullString, szSeparator, nFindStartIndex)
       if not nFindLastIndex then
        nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, string.len(szFullString))
        break
       end
       nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, nFindLastIndex - 1)
       nFindStartIndex = nFindLastIndex + string.len(szSeparator)
       nSplitIndex = nSplitIndex + 1
    end
    return nSplitArray
end

function Trim(s)
   return s:match "^%s*(.-)%s*$"
end

function EndWith(str, substr)  
    if str == nil or substr == nil then  
        return nil, "the string or the sub-string parameter is nil"  
    end  
    local str_tmp = string.reverse(str)  
    local substr_tmp = string.reverse(substr)  
    if string.find(str_tmp, substr_tmp) ~= 1 then  
        return false  
    else  
        return true  
    end  
end