using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace TorchPoints.Core
{
    public class CommonHelper
    {
        /// <summary>
        /// 获取枚举的description
        /// </summary>
        /// <param name="enumSubitem">枚举值</param>
        /// <returns>枚举对应的介绍</returns>
        public static string GetEnumDescription(object enumSubitem)
        {
            enumSubitem = (Enum)enumSubitem;
            string strValue = enumSubitem.ToString();

            FieldInfo fieldinfo = enumSubitem.GetType().GetField(strValue);

            if (fieldinfo != null)
            {

                Object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (objs == null || objs.Length == 0)
                {
                    return strValue;
                }
                else
                {
                    DescriptionAttribute da = (DescriptionAttribute)objs[0];
                    return da.Description;
                }
            }
            else
            {
                return "不限";
            }

        }

        /// <summary>
        /// 当前时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDateTimeNow()
        {
            return DateTime.Now;
        }
    }
}
