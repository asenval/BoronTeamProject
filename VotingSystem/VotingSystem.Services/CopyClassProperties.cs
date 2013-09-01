using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace VotingSystem.Services
{
    public static class CopyClassProperties
    {
        public static void Fill(object me, object from)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo[] myProps = me.GetType().GetProperties(flags); 
            var hisProps = from.GetType().GetProperties(flags).ToDictionary(p => p.Name, p => p);
            foreach (PropertyInfo mine in myProps)
            {
                PropertyInfo his;
                if (hisProps.TryGetValue(mine.Name, out his))
                {
                    var value = his.GetValue(from); 
                    mine.SetValue(me, value, null);
                }
            }
        }
    }
}