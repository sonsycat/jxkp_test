using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoldNet.JXKP
{
    public class DataHelper
    {
        public DataHelper()
        {
        }
        private static bool CheckStruct(Type type)
        {
            if (!type.IsValueType || type.IsEnum)
            {
                return false;
            }
            return (!type.IsPrimitive && !type.IsSerializable);
        }
        public static object ConvertValue(Type type, object value)
        {
            if (Convert.IsDBNull(value) || (value == null))
            {
                return null;
            }
            if (CheckStruct(type))
            {
                string data = value.ToString();
                return SerializationManager.Deserialize(type, data);
            }
            Type type2 = value.GetType();
            if (type == type2)
            {
                return value;
            }
            if (((type == typeof(Guid)) || (type == typeof(Guid?))) && (type2 == typeof(string)))
            {
                if (string.IsNullOrEmpty(value.ToString()))
                {
                    return null;
                }
                return new Guid(value.ToString());
            }
            if (((type == typeof(DateTime)) || (type == typeof(DateTime?))) && (type2 == typeof(string)))
            {
                if (string.IsNullOrEmpty(value.ToString()))
                {
                    return null;
                }
                return Convert.ToDateTime(value);
            }
            if (type.IsEnum)
            {
                try
                {
                    return Enum.Parse(type, value.ToString(), true);
                }
                catch
                {
                    return Enum.ToObject(type, value);
                }
            }
            if ((type == typeof(bool)) || (type == typeof(bool?)))
            {
                bool result = false;
                if (bool.TryParse(value.ToString(), out result))
                {
                    return result;
                }
                if (string.IsNullOrEmpty(value.ToString()))
                {
                    return false;
                }
                return true;
            }
            if (type.IsGenericType)
            {
                type = type.GetGenericArguments()[0];
            }
            return Convert.ChangeType(value, type);
        }
    }
}
