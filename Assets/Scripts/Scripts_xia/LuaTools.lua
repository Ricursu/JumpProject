
LuaTools={}
 --����ֵΪ32λMD5���ַ���
LuaTools.GetMD5=function (fileBytes)
	
	return Util.GetMD5(fileBytes)
	-- body
end
--����ֵΪУ��͵�16������
LuaTools.GetCRC=function (fileBytes )

	return Util.GetCRC(fileBytes)
	-- body
end
--ѹ���ļ�,����һ����Ҫѹ�����ļ����ֽ�����,����������ѹ���󱣴��·��,û�з���ֵ
LuaTools.CompressFile=function ( fileBytes,path )
	
	Util.CompressFile(fileBytes,path)	
	-- body
end
--����Ҫ��ѹ������·��,����ֵΪ��ѹ����ļ����ֽ�����
LuaTools.DecompressFile=function ( path )

    return Util.DecompressFile(path)
	-- body
end
LuaTools.DebugProxy=function(mg)
	Util.DebugProxy(mg)
	-- body
end


 













