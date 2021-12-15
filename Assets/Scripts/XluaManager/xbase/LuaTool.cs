using System.Collections;
using LitJson;
using XLua;
public static class LuaTool
{
    static public LuaTable CreateLuaTable()
    {
        return XluaManager.instance.LuaEnv.NewTable();
    }

    static public LuaTable CreateLuaTable(IEnumerable objs)
    {
        var table = CreateLuaTable();
        int index = 0;
        foreach (var obj in objs)
        {
            table.Set(index.ToString(),obj);
            index++;
        }
        return table;
    }
    static public LuaTable CreateLuaTable ( JsonData objs )
        {
        return JsonToLua (objs);
        }

    static public LuaTable CreateLuaTable(IList objs)
    {
        var table = CreateLuaTable();
        for ( int i = 0; i < objs.Count; i++ )
            {
            table.Set ( i+1, objs[i] );
            }
        return table;
    }

    static public LuaTable CreateLuaTable(IDictionary objs)
    {
        var table = CreateLuaTable();

        foreach (var key in objs.Keys)
        {
            table.Set(key, objs[key]);
        }
        return table;
    }

    public static LuaTable toLuaTable(this IEnumerable objs)
    {
        return CreateLuaTable(objs);
    }

    public static LuaTable toLuaTable(this IList objs)
    {
        return CreateLuaTable(objs);
    }

    public static LuaTable toLuaTable(this IDictionary objs)
    {
        return CreateLuaTable(objs);
    }

    public static LuaTable toLuaTable(this LitJson.JsonData json )
        {
        return CreateLuaTable (json );
        }
    public static LuaTable JsonToLua(JsonData data)
    {
        var table = XluaManager.instance.LuaEnv.NewTable();

        return ToLua(data, table);
    }
    private static LuaTable ToLua(JsonData data, LuaTable lua)
    {

        if (data.IsObject)
        {
            for (int i = 0; i < 1; i++)
            {
                foreach (var key in data.Keys)
                {

                    if (data[key].IsObject)
                    {
                        var table = XluaManager.instance.LuaEnv.NewTable();
                        lua.Set<string, object>(key, ToLua(data[key], table));

                    }
                    else if (data[key].IsArray)
                    {
                        var table = XluaManager.instance.LuaEnv.NewTable();
                        lua.Set<string, object>(key, ToLua(data[key], table));

                    }
                    else
                    {
                        if (data[key].IsBoolean)
                        {
                            lua.Set<string, bool>(key, (bool)data[key]);
                        }
                        else if (data[key].IsDouble)
                        {
                            lua.Set<string, double>(key, (double)data[key]);
                        }
                        else if (data[key].IsInt)
                        {
                            lua.Set<string, int>(key, (int)data[key]);
                        }
                        else if (data[key].IsLong)
                        {
                            lua.Set<string, long>(key, (long)data[key]);
                        }
                        else if (data[key].IsString)
                        {
                            lua.Set<string, string>(key, (string)data[key]);
                        }
                        else
                        {
                            lua.Set<string, object>(key, data[key]);
                        }


                    }

                }
            }
        }
        else if (data.IsArray)
        {
            int i = 1;
            foreach (JsonData item in data)
            {
                if (item.IsObject)
                {
                    var table = XluaManager.instance.LuaEnv.NewTable();
                    lua.Set<int, object>(i, ToLua(item, table));

                }
                else if (item.IsArray)
                {
                    var table = XluaManager.instance.LuaEnv.NewTable();
                    lua.Set<int, object>(i, ToLua(item, table));

                }
                else
                {
                    if (item.IsBoolean)
                    {
                        lua.Set<int, bool>(i, (bool)item);
                    }
                    else if (item.IsDouble)
                    {
                        lua.Set<int, double>(i, (double)item);
                    }
                    else if (item.IsInt)
                    {
                        lua.Set<int, int>(i, (int)item);
                    }
                    else if (item.IsLong)
                    {
                        lua.Set<int, long>(i, (long)item);
                    }
                    else if (item.IsString)
                    {
                        lua.Set<int, string>(i, (string)item);
                    }
                    else
                    {
                        lua.Set<int, object>(i, item);
                    }
                }
                i++;
            }
        }

        return lua;
    }
}