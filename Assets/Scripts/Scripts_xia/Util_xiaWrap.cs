using LuaInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Util_xiaWrap
{
    public static void Register(LuaState L)
    {
        L.BeginClass(typeof(Util_xia), typeof(System.Object));
       
        L.RegFunction("DebugProxy", DebugProxy);
        L.RegFunction("GetMD5", GetMD5);
        L.RegFunction("GetCRC", GetCRC);
        L.RegFunction("CompressFile", CompressFile);
        L.RegFunction("DecompressFile", DecompressFile);
        L.EndClass();

    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int DecompressFile(IntPtr L)//这里传进来的是一个路径
    {

        try
        {
            int parameterCount = LuaDLL.lua_gettop(L);
            if (parameterCount == 1 && TypeChecker.CheckTypes<string>(L, 1))
            {
                string path = ToLua.ToString(L, 1);
                byte[] bytes = Util.DecompressFile(path);
                ToLua.PushByteBuffer(L, bytes);
                return 1;

            }

            return 1;
        }
        catch (Exception e)
        {


            return LuaDLL.toluaL_exception(L, e);
        }

    }


    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int CompressFile(IntPtr L)
    {

        try
        {
            Util.DebugProxy("压缩文件调用");
            int parameterCount = LuaDLL.lua_gettop(L);
            if (parameterCount == 2 && TypeChecker.CheckTypes<int[], string>(L, 1))
            {
                int[] numbers = ToLua.CheckNumberArray<int>(L, 1);
                string path = ToLua.ToString(L, 2);

                List<byte> bytes = new List<byte>();

                for (int i = 0; i < numbers.Length; i++)
                {
                    bytes.Add(Convert.ToByte(numbers[i]));
                }

                Util.CompressFile(bytes.ToArray(), path);
                return 1;

            }
            else if (parameterCount == 2 && TypeChecker.CheckTypes<byte[], string>(L, 1))
            {
                byte[] bytes = ToLua.ToByteBuffer(L, 1);
                string path = ToLua.ToString(L, 2);

                Util.CompressFile(bytes, path);
                return 1;
            }

            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }


    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int GetCRC(IntPtr L)
    {
        try
        {
            int parameterCount = LuaDLL.lua_gettop(L);
            if (parameterCount == 1 && TypeChecker.CheckTypes<byte[]>(L, 1))
            {
                byte[] bytes = ToLua.CheckByteBuffer(L, 1);
                ushort short16 = Util.GetCRC(bytes);
                LuaDLL.lua_pushnumber(L, short16);
                return 1;
            }
            else if (parameterCount == 1 && TypeChecker.CheckTypes<int[]>(L, 1))
            {
                int[] numbers = ToLua.CheckNumberArray<int>(L, 1);

                List<byte> bytes = new List<byte>();

                for (int i = 0; i < numbers.Length; i++)
                {
                    bytes.Add(Convert.ToByte(numbers[i]));
                }
                ushort short16 = Util.GetCRC(bytes.ToArray());
                LuaDLL.lua_pushnumber(L, short16);
                return 1;
            }
            else if (parameterCount == 2 && TypeChecker.CheckTypes<int[], int>(L, 1))
            {
                int[] numbers = ToLua.CheckNumberArray<int>(L, 1);
                int option = (int)LuaDLL.luaL_checknumber(L, 2);

                List<byte> bytes = new List<byte>();
                for (int i = 0; i < numbers.Length; i++)
                {
                    bytes.Add(Convert.ToByte(numbers[i]));
                }
                ushort short16 = Util.GetCRC(bytes.ToArray(), option);
                LuaDLL.lua_pushnumber(L, short16);
                return 1;
            }
            else if (parameterCount == 2 && TypeChecker.CheckTypes<byte[], int>(L, 1))
            {
                byte[] bytes = ToLua.CheckByteBuffer(L, 1);
                int option = (int)LuaDLL.luaL_checknumber(L, 2);

                ushort short16 = Util.GetCRC(bytes, option);
                LuaDLL.lua_pushnumber(L, short16);
                return 1;
            }

            return 0;
        }
        catch (Exception e)
        {

            return LuaDLL.toluaL_exception(L, e);
        }



    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int GetMD5(IntPtr L)
    {
        try
        {
            int parameterCount = LuaDLL.lua_gettop(L);
            if (parameterCount == 1 && TypeChecker.CheckTypes<int[]>(L, 1))
            {
                int[] numbers = ToLua.CheckNumberArray<int>(L, 1);

                List<byte> bytes = new List<byte>();

                for (int i = 0; i < numbers.Length; i++)
                {
                    bytes.Add(Convert.ToByte(numbers[i]));
                }

                string str = Util.GetMD5(bytes.ToArray());
                LuaDLL.lua_pushstring(L, str);
                return 1;
            }
            else if (parameterCount == 1 && TypeChecker.CheckTypes<byte[]>(L, 1))
            {
                byte[] bytes = ToLua.ToByteBuffer(L, 1);

                string str = Util.GetMD5(bytes);
                LuaDLL.lua_pushstring(L, str);
                return 1;
            }

            return 0;
        }
        catch (Exception e)
        {

            return LuaDLL.toluaL_exception(L, e);
        }


    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int DebugProxy(IntPtr L)
    {
        try
        {
            int parameterCount = LuaDLL.lua_gettop(L);
            if (parameterCount == 1 && TypeChecker.CheckTypes<int>(L, 1))
            {
                int number = (int)LuaDLL.luaL_checknumber(L, 1);
                Util.DebugProxy(number);
            }
            else if (parameterCount == 1 && TypeChecker.CheckTypes<string>(L, 1))
            {
                string str = ToLua.ToString(L, 1);
                Util.DebugProxy(str);
            }


            object obj1 = ToLua.ToVarObject(L, 1);
            byte[] bytes = ToLua.ToByteBuffer(L, 1);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }


    }





}

