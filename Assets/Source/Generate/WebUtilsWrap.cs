﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class WebUtilsWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(WebUtils), typeof(System.Object));
		L.RegFunction("GetIPAddress", GetIPAddress);
		L.RegFunction("GetFileFromServer", GetFileFromServer);
		L.RegFunction("IsExistFileInServer", IsExistFileInServer);
		L.RegFunction("GetByteFromServer", GetByteFromServer);
		L.RegFunction("GetApkFromFile", GetApkFromFile);
		L.RegFunction("New", _CreateWebUtils);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("IP", get_IP, set_IP);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateWebUtils(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				WebUtils obj = new WebUtils();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: WebUtils.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetIPAddress(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			string o = WebUtils.GetIPAddress();
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetFileFromServer(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			WebUtils.GetFileFromServer(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsExistFileInServer(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			bool o = WebUtils.IsExistFileInServer(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetByteFromServer(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			byte[] o = WebUtils.GetByteFromServer(arg0);
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetApkFromFile(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			WebUtils.GetApkFromFile();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IP(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, WebUtils.IP);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_IP(IntPtr L)
	{
		try
		{
			string arg0 = ToLua.CheckString(L, 2);
			WebUtils.IP = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

