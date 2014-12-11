using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model;
using System.IO;
using System.Reflection;


namespace DAL
{
    public class pxComDal
    {
        public static bool SetBeanValue(object entity, Hashtable ht)
        {
            if (entity == null || ht == null || ht.Count == 0)
            {
                return false;
            }
            try
            {
                PropertyInfo[] props = entity.GetType().GetProperties();
                DataFieldAttribute FieldAttr = null;
                object[] CustomAttributes;
                string strFieldName = string.Empty;

                foreach (PropertyInfo prop in props)
                {
                    CustomAttributes = prop.GetCustomAttributes(typeof(DataFieldAttribute), false);
                    if (CustomAttributes.Length > 0)
                    {
                        FieldAttr = CustomAttributes[0] as DataFieldAttribute;
                        string propName = prop.Name.ToString().ToLower();
                        if (ht.Contains(propName))
                        {
                            //    prop.SetValue(beanObj,ht[propName],
                            if (!prop.PropertyType.IsGenericType)
                            {
                                prop.SetValue(entity, Convert.ChangeType(ht[propName], prop.PropertyType), null);
                            }
                            else
                            {
                                //用于处理nullable类型数据
                                Type genericTypeDef = prop.PropertyType.GetGenericTypeDefinition();
                                if (genericTypeDef == typeof(Nullable<>))
                                {
                                    prop.SetValue(entity, Convert.ChangeType(ht[propName], Nullable.GetUnderlyingType(prop.PropertyType)), null);
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {

            }
            return false;
        }
    }
}
