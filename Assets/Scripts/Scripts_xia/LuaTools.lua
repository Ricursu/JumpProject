
LuaTools={}
 --返回值为32位MD5的字符串
LuaTools.GetMD5=function (fileBytes)
	
	return Util_xia.GetMD5(fileBytes)
	-- body
end
--返回值为校验和的16进制数
LuaTools.GetCRC=function (fileBytes )

	return Util_xia.GetCRC(fileBytes)
	-- body
end
--压缩文件,参数一传入要压缩的文件的字节数组,参数二传入压缩后保存的路径,没有返回值
LuaTools.CompressFile=function ( fileBytes,path )
	
	Util_xia.CompressFile(fileBytes,path)	
	-- body
end
--传入要解压的完整路径,返回值为解压后的文件的字节数组
LuaTools.DecompressFile=function ( path )

    return Util_xia.DecompressFile(path)
	-- body
end
LuaTools.DebugProxy=function(mg)
	Util_xia.DebugProxy(mg)
	-- body
end


 













