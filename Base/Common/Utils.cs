using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Base.Common
{
    public static class Utils
    {
        public static string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        public static string GetEname(this object ob)
        {
            return ob.ToString();
        }
        public static string GetFilePath(string relPath)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relPath.Replace("/", "\\"));
        }
        //public static bool CheckAuthen(out UserProfile cProfile, out string errMsg)
        //{
        //    cProfile = PortalContext.CurrentUser;
        //    errMsg = string.Empty;
        //    if (cProfile == null)
        //    {
        //        errMsg = "No Anthentication (need relogin)";
        //        return false;
        //    }
        //    return true;
        //}
        public static bool CheckNull(object ob, out string errMsg, string _default = null)
        {
            if (ob == null)
            {
                errMsg = _default ?? "Object is null";
                return false;
            }
            else
            {
                errMsg = string.Empty;
                return true;
            }
        }
        public static bool CheckRequire(string ob, out string errMsg, string _default = null)
        {
            if (string.IsNullOrWhiteSpace(ob))
            {
                errMsg = _default ?? "Object is null";
                return false;
            }
            else
            {
                errMsg = string.Empty;
                return true;
            }
        }
        public static void Serialize<T>(T obj, string filename)
        {
            XmlSerializer x = new XmlSerializer(typeof(T));
            var fstr = File.Create(filename);
            using (fstr)
            {
                x.Serialize(fstr, obj);
            }
        }
       
        public static string FromDMY2YMD(this string me)
        {
            if (!string.IsNullOrWhiteSpace(me))
            {
                Regex reg = new Regex(@"(\d{2})/(\d{2})/(\d{4})");
                return reg.Replace(me, "$3-$2-$1");
                //return me.Replace()
            }
            return me;
        }
        public static int GetInt(this string me, int _default = 0)
        {
            if (!string.IsNullOrWhiteSpace(me) && int.TryParse(me.Trim(), out int ret))
            {
                return ret;
            }
            return _default;
        }

        
    }
}
