using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Model
{
    /// <summary>
    /// 创建不自动排序的hashtable
    /// </summary>
    ///----------------------------------------------------------
    ///修改历史
    ///日期       修改人      修改
    ///----------------------------------------------------------
    ///20111130    王强      创建代码
    ///----------------------------------------------------------
    public class NoSortHashtable : Hashtable
    {

        private ArrayList list = new ArrayList();
        public override object this[object key]
        {
            get
            {
                return base[key];
            }
            set
            {
                if (!base.ContainsKey(key))
                {
                    base.Add(key, value);
                    list.Add(key);
                }
            }
        }

        public override void Add(object key, object value)
        {
            if (!base.ContainsKey(key))
            {
                base.Add(key, value);
                list.Add(key);
            }

        }
        public override void Clear()
        {
            base.Clear();
            list.Clear();
        }
        public override void Remove(object key)
        {
            base.Remove(key);
            list.Remove(key);
        }
        public override ICollection Keys
        {
            get
            {
                return list;
            }
        }

        public override IDictionaryEnumerator GetEnumerator()
        {
            return base.GetEnumerator();
        }

    }

}
