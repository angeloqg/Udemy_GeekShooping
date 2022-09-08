using System.Reflection;

namespace GeekShooping.ProductApi.Utils
{
    public static class Compare
    {
        public static bool ObjectTo(object object1, object object2)
        {
            int eDifferent = 0;

            Type objType = object1.GetType();
            PropertyInfo[] properties = objType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (PropertyInfo property in properties)
            {
                object? obj1 = property.GetValue(object1, null);
                object? obj2 = property.GetValue(object2, null);

                if (obj1?.ToString() != obj2?.ToString())
                {
                    eDifferent++;
                }
            }

            return eDifferent > 0;
        }
    }
}
