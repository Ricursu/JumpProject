# 热更新项目仓库

该代码基于https://github.com/YomiIsayama/Tolua_JumpProject 进行修改

### 使用流程：

​	1、Build出APK 存放到D:/APK下，命名为1.0.apk

​	2、通过菜单项zip/unzip解压apk

​	3、通过菜单项zip/build version file生成版本文件

​	4、安装1.0.apk

​	5、修改StreamingAssets中Lua代码

​	6、更新HotUpdate的mMajorVersion

​	7、打包1.1.apk

​	8、通过菜单项zip/unzip解压apk

​	9、通过菜单项zip/build zip压缩差分文件

​	10、通过菜单项zip/build version file生成版本文件

​	11、将D:\Apk\version\v1.1\Different下的压缩文件和版本文件放到服务器中

​	12、启动游戏

#### 注意事项：

​	1、需要更改WebUtils下的IP地址才能成功读取，可以通过Unzip/Get IP获得当前的IP地址，同时，手机和电脑需要处于同一个网络环境下