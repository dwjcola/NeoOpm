//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using LitJson;
using System;
using System.Collections.Generic;

namespace NeoOPM
{
    /// <summary>
    /// LitJSON 函数集辅助器。
    /// </summary>
    internal class LitJsonHelper : Utility.Json.IJsonHelper
    {
        /// <summary>
        /// 将对象序列化为 JSON 字符串。
        /// </summary>
        /// <param name="obj">要序列化的对象。</param>
        /// <returns>序列化后的 JSON 字符串。</returns>
        public string ToJson(object obj)
        {
            return JsonMapper.ToJson(obj);
        }

        /// <summary>
        /// 将 JSON 字符串反序列化为对象。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="json">要反序列化的 JSON 字符串。</param>
        /// <returns>反序列化后的对象。</returns>
        public T ToObject<T>(string json)
        {
            return JsonMapper.ToObject<T>(json);
        }

        /// <summary>
        /// 将 JSON 字符串反序列化为对象。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <param name="json">要反序列化的 JSON 字符串。</param>
        /// <returns>反序列化后的对象。</returns>
        public object ToObject(Type objectType, string json)
        {
            System.Reflection.MethodInfo method;
            if(!s_CachedMethod.TryGetValue(objectType,out method))
            {
                method = toObject.MakeGenericMethod(objectType);
                s_CachedMethod.Add(objectType, method);
            }
            return method.Invoke(default, new object[] { json });
        }
        private readonly System.Reflection.MethodInfo toObject = typeof(LitJsonWrapper<>).GetMethod("ToObject");
        private static Dictionary<Type, System.Reflection.MethodInfo> s_CachedMethod = new Dictionary<Type, System.Reflection.MethodInfo>();
    }
    internal class LitJsonWrapper<T>
    {
        public T ToObject<T>(string json)
        {
            return JsonMapper.ToObject<T>(json);
        }
    }
}
