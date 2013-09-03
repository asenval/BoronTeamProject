using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace VotingSystem.Services
{
    public static class CopyClassProperties
    {
        public static void Fill(object me, object from, bool ignoreEmpty = false, params string[] ignoreHeaders)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo[] myProps = me.GetType().GetProperties(flags);
            var hisProps = from.GetType().GetProperties(flags).ToDictionary(p => p.Name, p => p);
            foreach (PropertyInfo mine in myProps)
            {
                PropertyInfo his;
                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(mine.PropertyType) &&
                    typeof(string) != mine.PropertyType)
                    continue;
                if (ignoreHeaders.Contains(mine.Name))
                    continue;
                if (hisProps.TryGetValue(mine.Name, out his))
                {
                    var value = his.GetValue(from);
                    if (ignoreEmpty)
                    {
                        if (value == null)
                            continue;
                        if (value == "")
                            continue;
                        if (mine.PropertyType.IsValueType && value.Equals(Activator.CreateInstance(mine.PropertyType)))
                            continue;
                    }
                    mine.SetValue(me, value, null);
                }
            }
        }
    }
}