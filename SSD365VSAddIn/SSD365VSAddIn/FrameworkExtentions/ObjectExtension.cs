using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.FrameworkExtentions
{
    public static class ObjectExtension
    {
        public static ReturnType GetPropertyValueSS<ReturnType>(this object obj, string propertyName)
        {
            ReturnType value = default(ReturnType);
            if(obj.HasPropertySS(propertyName))
            {
                var propertyInfo = obj.GetType().GetProperty(propertyName);
                var propValue = propertyInfo.GetValue(obj);
                if(propValue != null && propValue is ReturnType)
                {
                    value = (ReturnType)propValue;// as ReturnType;
                }
            }
            return value;
        }

        public static bool HasPropertySS(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

        public static void SetPropertyValueSS(this object obj, string propertyName, object value)
        {
            if(obj.HasPropertySS(propertyName))
            {
                var propertyInfo = obj.GetType().GetProperty(propertyName);
                propertyInfo.SetValue(obj, value);
            }
        }
    }
}
