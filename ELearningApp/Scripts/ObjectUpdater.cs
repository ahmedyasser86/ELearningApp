using System.Collections;
using System.Reflection;

namespace ELearningApp.Scripts
{
    public static class ObjectUpdater
    {
        // دالة Generic لتحديث القيم
        public static void UpdateValues<T>(T originalObject, T updatedObject)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    var newValue = property.GetValue(updatedObject);
                    var originalValue = property.GetValue(originalObject);

                    // التحقق مما إذا كانت الخاصية عبارة عن قائمة أو مجموعة
                    if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
                    {
                        // استبدال القائمة بالكامل
                        if (newValue != null)
                        {
                            property.SetValue(originalObject, newValue);
                        }
                    }
                    // التحقق مما إذا كانت الخاصية عبارة عن كائن (object) وتجاهلها
                    else if (!property.PropertyType.IsClass || property.PropertyType == typeof(string))
                    {
                        // تحديث القيم غير القائمة وغير الكائنات
                        property.SetValue(originalObject, newValue);
                    }
                }
            }
        }
    }
}
