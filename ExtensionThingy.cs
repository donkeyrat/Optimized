using System.Reflection;

namespace Optimized_
{
    public static class ExtensionThingy
    {
        public static object InvokeMethod<T>(this T obj, string methodName, params object[] args)
        {
            var type = typeof(T);
            var method = type.GetTypeInfo().GetDeclaredMethod(methodName);
            return method.Invoke(obj, args);
        }

        public static T SetField<T>(this T self, string name, object value) where T : class
        {
            FieldInfo field = typeof(T).GetField(name, (BindingFlags)(-1));
            if (field != null)
            {
                field.SetValue(self, value);
            }
            return self;
        }

        public static object GetField<T>(this T self, string name) where T : class
        {
            FieldInfo field = typeof(T).GetField(name, (BindingFlags)(-1));
            if (field != null)
            {
                return field.GetValue(self);
            }
            return null;
        }
    }
}