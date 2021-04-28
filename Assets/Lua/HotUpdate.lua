HotUpdate = {}
local this = HotUpdate

function this.Awake(object)
    -- 1、创建版本文件config.ini
    this.CreateVersionFile();
    -- 2、判断是否需要更新
    if not this.IsUpdate() then
        return;
    end
    -- 3、判断是否需要释放APK
    if not Directory.Exists(Path.Combine(Application.persistentDataPath, "Lua")) then
        -- 3.1、从文件管理器中获取APK
        this.GetApkFromFileManager();
        -- 3.2、 解压APK
        UnZipTool.UnZipApk(Application.persistentDataPath + "/base.apk", Path.Combine(Application.persistentDataPath, "Lua"));
    end

    -- 4、获取服务器压缩文件
    this.GetZipFromServer();
    -- 5、解压第4步中获取的压缩文件
    this.UnZipFile();
    -- 6、还原压缩文件中的差分包或将压缩文件中的Lua文件复制到本地文件
    this.RedutionFile();

    -- 7、 更新本地版本文件version.ini的内容
    FileUtils.CreateFile(Path.Combine(Application.persistentDataPath, "config.ini"), WebUtils.GetByteFromServer("version.txt"));
end

function this.CreateVersionFile()
    local filename = Path.Combine(Application.persistentDataPath, "config.ini");
    if File.Exists(filename) then
        return;
    end
    FileUtils.CreateFile(filename, Encoding.GetBytes("version = " + HotUpdate.mReleaseVersion + "." + HotUpdate.mMajorVersion));
end

function this.IsUpdate()
    if Directory.Exists(Application.persistentDataPath .. "/Temp") then
        Directory.Delete(Application.persistentDataPath .. "/Temp", true);
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
    local files = Directory.GetFiles(Path.Combine(Application.persistentDataPath, "Temp"));
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