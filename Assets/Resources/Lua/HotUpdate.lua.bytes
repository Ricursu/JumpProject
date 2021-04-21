HotUpdate = {}
local this = HotUpdate

function this.Awake()
    Debugger.Log("HotUpdate");
    this.CreateVersionFile();
    if not this.IsUpdate() then
        return;
    end
    if not Directory.Exists(Path.Combine(Application.streamingAssetsPath, "Lua")) then
        this.GetApkFromFileManager();
        UnZipTool.UnZipApk(Application.persistentDataPath + "/base.apk", Path.Combine(Application.persistentDataPath, "Lua"));
    end

    this.GetZipFromServer()
    this.UnZipFile()
    this.RedutionFile()

    FileUtils.CreateFile(Path.Combine(Application.persistentDataPath, "config.ini"), WebUtils.GetByteFromServer("version.txt"));
end

function this.CreateVersionFile()
    local filename = Path.Combine(Application.persistentDataPath, "config.ini");
    if File.Exists(filename) then
        return;
    end
    FileUtils.CreateFile(filename, Encoding.UTF8.GetBytes("version = " + HotUpdate.mReleaseVersion + "." + HotUpdate.mMajorVersion));
end

function this.IsUpdate()
    if Directory.Exists(Application.persistentDataPath + "/Temp") then
        Directory.Delete(Application.persistentDataPath + "/Temp", true);
    end
    local serverVersion = Encoding.UTF8.GetString(WebUtils.GetByteFromServer("version.txt"));
    local localVersion = Encoding.UTF8.GetString(FileUtils.ReadFileBytes(Path.Combine(Application.persistentDataPath, "config.ini")));
    local serverInfo = serverVersion.Split('=');
    local localInfo = localVersion.Split('=');
    if serverInfo[1].Trim() == localInfo[1].Trim() then
        return false;
    end
    return true;
end

function this.GetApkFromFileManager()
    WebUtils.GetApkFromFile();
end

function this.GetZipFromServer()
    local version = Encoding.UTF8.GetString(FileUtils.ReadFileBytes(Path.Combine(Application.persistentDataPath, "config.ini")));
    local value = version:Split('=');
    value[1] = value[1]:Trim();
    local serverVersion = Encoding.UTF8.GetString(WebUtils.GetByteFromServer("version.txt"));
    local serverValue = serverVersion:Split('=');
    serverValue[1] = serverValue[1]:Trim();

    local filename = "v" + value[1] + "-v" + serverValue[1] + ".zip";
    WebUtils.GetFileFromServer(filename);
end

function this.UnZipFile()
    local version = Encoding.UTF8.GetString(FileUtils.ReadFileBytes(Path.Combine(Application.persistentDataPath, "config.ini")));
    local value = version:Split('=');
    value[1] = value[1]:Trim();
    local serverVersion = Encoding.UTF8.GetString(WebUtils.GetByteFromServer("version.txt"));
    local serverValue = serverVersion:Split('=');
    serverValue[1] = serverValue[1]:Trim();

    local filename = "v" + value[1] + "-v" + serverValue[1] + ".zip";
    UnZipTool.UnZip(filename, Path.Combine(Application.persistentDataPath, "Temp"));
end

function this.RedutionFile()
    local files = Directory.GetFiles(Path.Combine(Application.persistentDataPath, "Temp"));
    


    DiffUtils.ReductionFile(Path.Combine(Application.persistentDataPath, "Lua"), Path.Combine(Application.persistentDataPath, "Temp/filelist.txt"));
end

