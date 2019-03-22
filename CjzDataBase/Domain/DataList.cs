using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace CjzDataBase
{
    /// <summary>
    /// 数据列表基类
    /// </summary>
    public class DataList : DataPacket, IList
    {
        #region 私有字段
        private ArrayList _list;
        private Type _ItemType;

        private class Enumerator : IEnumerator
        {
            private int a = -1;
            private DataList datalist;
            public Enumerator(DataList list)
            {
                this.datalist = list;
            }
            public bool MoveNext()
            {
                if (this.a < this.datalist.Count - 1)
                {
                    this.a++;
                    return true;
                }
                return false;
            }
            public void Reset()
            {
                this.a = -1;
            }
            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return this.datalist[this.a];
                }
            }
        }

        #endregion

        #region 构造函数
        public DataList()
        {
            _list = new ArrayList();
        }

        #endregion

        #region 属性定义
        /// <summary>
        /// IList成员，获取一个值，该值指示 DataList 是否具有固定大小。
        /// </summary>
        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// IList成员，获取一个值，该值指示 DataList 是否为只读。
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// ICollection 成员,获取一个值，该值指示是否同步对 System.Collections.ICollection 的访问（线程安全）。
        /// <remarks>如果对 System.Collections.ICollection 的访问是同步的（线程安全），则为 true；否则为 false。</remarks>
        /// </summary>
        bool ICollection.IsSynchronized
        {
            get
            {
                return this.GetIsSynchronized();
            }
        }
        /// <summary>
        /// ICollection 成员,获取可用于同步 System.Collections.ICollection 访问的对象。
        /// <remarks>返回可用于同步对 System.Collections.ICollection 的访问的对象。</remarks>
        /// </summary>
        object ICollection.SyncRoot
        {
            get
            {
                return this.GetSyncRoot();
            }
        }
        /// <summary>
        ///  获取或设置指定索引处的元素。
        /// </summary>
        /// <param name="index">要获得或设置的元素从零开始的索引。</param>
        /// <returns>指定索引处的元素。</returns>
        public DataPacket this[int index]
        {
            get
            {
                return this.GetItems(index);
            }
            set
            {
                this.SetItems(index, value);
            }
        }
        /// <summary>
        /// 获取 DataList 中实际包含的元素数。
        /// </summary>
        public int Count
        {
            get
            {
                return this.GetCount();
            }
        }
        /// <summary>
        /// 设置或获取被管理元素的缺省类型，可以为 null。如果要从Xml中恢复，必须指定该属性。
        /// </summary>
        public Type ItemType
        {
            get
            {
                return GetItemType();
            }
            set
            {
                _ItemType = value;
            }
        }
        /// <summary>
        /// IList成员，获取或设置指定索引处的元素。
        /// </summary>
        /// <param name="index">要获得或设置的元素从零开始的索引。</param>
        /// <returns>指定索引处的元素。</returns>
        object IList.this[int index]
        {
            get
            {
                return this.GetItems(index);
            }
            set
            {
                this.SetItems(index, (DataPacket)value);
            }
        }
        
        #endregion

        #region 私有方法

        #endregion

        #region 保护方法
        /// <summary>
        /// 用于实现ICollection.CopyTo()的虚函数，需重载。
        /// </summary>
        /// <param name="array">作为从 System.Collections.ICollection 复制的元素的目标位置的一维 System.Array。System.Array 必须具有从零开始的索引。</param>
        /// <param name="index">array 中从零开始的索引，从此处开始复制。</param>
        protected void DoCopyTo(Array array, int index)
        {
            this._list.CopyTo(array, index);
        }
        /// <summary>
        /// DataList虚函数实现
        /// </summary>
        protected bool GetIsSynchronized()
        {
            return this._list.IsSynchronized;
        }
        /// <summary>
        /// DataList虚函数实现
        /// </summary>
        protected object GetSyncRoot()
        {
            return this._list.SyncRoot;
        }
        /// <summary>
        /// ICollection 成员,从特定的 System.Array 索引处开始，将 System.Collections.ICollection 的元素复制到一个 System.Array 中。
        /// </summary>
        /// <param name="array">作为从 System.Collections.ICollection 复制的元素的目标位置的一维 System.Array。System.Array 必须具有从零开始的索引。</param>
        /// <param name="index">array 中从零开始的索引，从此处开始复制。</param>
        void ICollection.CopyTo(Array array, int index)
        {
            this.DoCopyTo(array, index);
        }

        /// <summary>
        /// 根据ItemType创建一个新 DataPacket 元素
        /// </summary>
        /// <returns>新 DataPacket 元素, 如果 ItemType 为 null 则返回 null。</returns>
        protected virtual DataPacket CreateNewItem()
        {
            Type itemType = this.ItemType;
            if (itemType != null)
            {
                return Activator.CreateInstance(itemType) as DataPacket;
            }
            return null;
        }
        /// <summary>
        /// 取得ItemType的虚函数, 如果未设ItemType且有元素存在，则取第一个元素类型
        /// </summary>
        /// <returns>ItemType</returns>
        protected virtual Type GetItemType()
        {
            if (_ItemType == null && this.Count > 0)
            {
                _ItemType = this[0].GetType();
            }
            return _ItemType;
        }
        /// <summary>
        ///  获取指定索引处的元素。
        /// </summary>
        /// <param name="index">要获得或设置的元素从零开始的索引。</param>
        /// <returns>指定索引处的 DataPacket 元素。</returns>
        protected virtual DataPacket GetItems(int Index)
        {
            return _list[Index] as DataPacket;
        }
        /// <summary>
        ///  设置指定索引处的元素。
        /// </summary>
        /// <param name="Index">要获得或设置的元素从零开始的索引。</param>
        /// <param name="value">指定的 DataPacket 元素。</param>
        protected virtual void SetItems(int Index, DataPacket value)
        {
            _list[Index] = value;
        }
        /// <summary>
        /// 获取 DataList 中实际包含的元素数。
        /// </summary>
        protected virtual int GetCount()
        {
            return _list.Count;
        }
        /// <summary>
        /// 清除 DataList 中的所有元素。
        /// </summary>
        public virtual void ClearItems()
        {
            if (_list != null)
            {
                _list.Clear();
            }
        }
        /// <summary>
        /// 将元素添加到 DataList 中。
        /// </summary>
        /// <param name="item">要添加到集合的 DataPacket 元素</param>
        /// <returns>新元素的插入位置</returns>
        protected virtual int AddItem(DataPacket Item)
        {
            _list.Add(Item);
            return _list.Count;
        }
        /// <summary>
        ///  将一个新元素插入指定索引处的 DataList。
        /// </summary>
        /// <param name="index">从零开始的索引，应在该位置插入 item。</param>
        /// <param name="item">要插入 DataList 中的 DataPacket 元素。</param>
        protected virtual void InsertItem(int Index, DataPacket Item)
        {
            _list.Insert(Index, Item);
        }
        
        #endregion

        #region 公有方法
        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>可用于循环访问集合的 System.Collections.IEnumerator 对象。</returns>
        public IEnumerator GetEnumerator()
        {
            return new DataList.Enumerator(this);
        }
        /// <summary>
        /// IList成员，将某项添加到 DataList 中。
        /// </summary>
        /// <param name="value">要添加到 DataList 的 System.Object。必须从 DataPacket 继承</param>
        /// <returns>新元素的插入位置。</returns>
        public int Add(object value)
        {
            return this.Add((DataPacket)value);
        }
        /// <summary>
        /// IList成员，确定 DataList 是否包含特定值。
        /// </summary>
        /// <param name="value">要在 DataList 中查找的 System.Object。</param>
        /// <returns>如果在 DataList 中找到 System.Object，则为 true；否则为 false。</returns>
        public bool Contains(object value)
        {
            return this.IndexOf(value) >= 0;
        }
        /// <summary>
        /// IList成员，确定 DataList 中特定项的索引。
        /// </summary>
        /// <param name="value">要在 DataList 中查找的 System.Object。</param>
        /// <returns>如果在列表中找到，则为 value 的索引；否则为 -1。</returns>
        public int IndexOf(object value)
        {
            return this.IndexOf((DataPacket)value);
        }
        /// <summary>
        /// IList成员，将一个项插入指定索引处的 DataList。
        /// </summary>
        /// <param name="index">从零开始的索引，应在该位置插入 value。</param>
        /// <param name="value">要插入 DataList 中的 System.Object。</param>
        public void Insert(int index, object value)
        {
            this.Insert(index, (DataPacket)value);
        }
        /// <summary>
        /// IList成员，从 DataList 中移除特定对象的第一个匹配项。
        /// </summary>
        /// <param name="value">要从 DataList 移除的 System.Object。</param>
        public void Remove(object value)
        {
            this.Remove((DataPacket)value);
        }
        /// <summary>
        /// IList成员，移除指定索引处的 DataList 项。
        /// </summary>
        /// <param name="index">从零开始的索引（属于要移除的项）。</param>
        public void RemoveAt(int index)
        {
            this.Delete(index);
        }

        /// <summary>
        /// 清除DataList中所有数据。
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            this.ClearItems();
        }
        /// <summary>
        /// 移除指定索引处的 DataList 项。
        /// </summary>
        /// <param name="index">从零开始的索引（属于要移除的项）。</param>
        public virtual void Delete(int index)
        {
            this._list.RemoveAt(index);
        }
        /// <summary>
        /// 确定 DataList 中特定元素的索引。
        /// </summary>
        /// <param name="item">要在 DataList 中查找的 DataPacket 元素。</param>
        /// <returns>如果在列表中找到，则为 item 的索引；否则为 -1。</returns>
        public virtual int IndexOf(DataPacket item)
        {
            return this._list.IndexOf(item);
        }
        /// <summary>
        /// 移动指定索引处的 DataList 项到新位置。
        /// </summary>
        /// <param name="CurIndex">从零开始的索引（要移动的项）。</param>
        /// <param name="NewIndex">从零开始的索引（新位置）。</param>
        public virtual void Move(int CurIndex, int NewIndex)
        {
            DataPacket dataPacket = (DataPacket)this._list[CurIndex];
            this._list.Remove(dataPacket);
            this._list.Insert(NewIndex, dataPacket);
        }
        /// <summary>
        /// 从 DataList 中移除特定元素的第一个匹配项。
        /// </summary>
        /// <param name="item">要从 DataList 移除的 DataPacket 元素。</param>
        public virtual void Remove(DataPacket item)
        {
            this._list.Remove(item);
        }
        /// <summary>
        /// 将一个 DataList 中元素附本复制到现有集合, 元素必须是 DataPacket 。
        /// </summary>
        /// <param name="sou">被复制的 DataList</param>
        public override void AssignFrom(DataPacket sou)
        {
            base.AssignFrom(sou);
            DataList dataList = sou as DataList;
            if (dataList != null)
            {
                this.ClearItems();
                int count = dataList.Count;
                for (int i = 0; i < count; i++)
                {
                    DataPacket dataPacket = dataList[i];
                    DataPacket dataPacket2 = this.CreateNewItem();
                    if (dataPacket2 == null)
                    {
                        dataPacket2 = (DataPacket)Activator.CreateInstance(dataPacket.GetType());
                    }
                    this.Add(dataPacket2);
                    dataPacket2.AssignFrom(dataPacket);
                }
            }
        }
        /// <summary>
        /// 用指定节点名称序列化整个 DataList。
        /// </summary>
        /// <param name="node">用于序列化 DataList 的 XmlNode 节点。</param>
        public override void XMLEncode(XmlNode node)
        {
            base.XMLEncode(node);
            XmlNode xmlNode = DataPacket.NewXmlChildNode(node, "FDataList");
            int count = this.Count;
            if (count > 0)
            {
                DataPacket items = this.GetItems(0);
                XmlAttribute xmlAttribute = xmlNode.OwnerDocument.CreateAttribute("ItemType");
                xmlAttribute.Value = CommUtils.GetObjectTypeName(items);
                xmlNode.Attributes.SetNamedItem(xmlAttribute);
            }
            for (int i = 0; i < count; i++)
            {
                DataPacket items = this.GetItems(i);
                items.XMLEncode(DataPacket.NewXmlChildNode(xmlNode, items.GetTypeName().Replace("`", "_")));
            }
        }
        /// <summary>
        /// 指定节点中反序列化整个 ObjList。需指定 ItemType, 如 ItemType 为 null, 
        /// 则试图从子节点的 ItemType 属性中建立 ItemType。 
        /// </summary>
        /// <param name="node">反序列化 ObjList 的 XmlNode 节点。</param>
        public override void XMLDecode(XmlNode node)
        {
            base.XMLDecode(node);
            XmlNode xmlNode = DataPacket.SelectChildNode(node, "FDataList");
            if (xmlNode != null)
            {
                XmlNodeList childNodes = xmlNode.ChildNodes;
                int count = childNodes.Count;
                for (int i = 0; i < count; i++)
                {
                    XmlNode xmlNode2 = childNodes.Item(i);
                    if (xmlNode2.NodeType == XmlNodeType.Element)
                    {
                        DataPacket dataPacket = this.CreateNewItem();
                        if (dataPacket == null)
                        {
                            XmlAttribute xmlAttribute = xmlNode.Attributes.GetNamedItem("ItemType") as XmlAttribute;
                            if (xmlAttribute != null)
                            {
                                dataPacket = (CommUtils.CreateObjectByType(xmlAttribute.Value) as DataPacket);
                            }
                        }
                        dataPacket.XMLDecode(xmlNode2);
                        this.AddItem(dataPacket);
                    }
                }
            }
        }
        /// <summary>
        /// 将对象写入到流
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void SaveToStream(Stream stream)
        {
            base.SaveToStream(stream);
            int count = this.Count;
            WriteToStream(stream, count);
            for (int i = 0; i < count; i++)
            {
                this[i].SaveToStream(stream);
            }
        }
        /// <summary>
        /// 从流中读出对象
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void LoadFromStream(Stream stream)
        {
            base.LoadFromStream(stream);
            int num;
            ReadFromStream(stream, out num);
            this.ClearItems();
            for (int i = 0; i < num; i++)
            {
                DataPacket dataPacket = this.CreateNewItem();
                dataPacket.LoadFromStream(stream);
                this.AddItem(dataPacket);
            }
        }

        /// <summary>
        /// 将元素添加到 DataList 中。
        /// </summary>
        /// <param name="item">要添加到集合的 DataPacket 元素</param>
        /// <returns>新元素的插入位置</returns>
        public int Add(DataPacket item)
        {
            return this.AddItem(item);
        }
        /// <summary>
        /// 创建一个 ItemType 指定的新元素并添加到 ObjList 中， 要求 ItemType 必须不为 null。
        /// </summary>
        /// <returns>新元素的插入位置</returns>
        public DataPacket AddNew()
        {
            DataPacket dataPacket = CreateNewItem();
            this.Add(dataPacket);
            return dataPacket;
        }
        /// <summary>
        ///  将一个新元素插入指定索引处的 DataList。
        /// </summary>
        /// <param name="index">从零开始的索引，应在该位置插入 item。</param>
        /// <param name="item">要插入 DataList 中的 DataPacket 元素。</param>
        public void Insert(int index, DataPacket item)
        {
            InsertItem(index, item);
        }

        /// <summary>
		/// 使用指定的比较器对整个 DataList 中的字符串进行排序。
		/// </summary>
		/// <param name="comparer">比较元素时要使用的 System.Collections.IComparer 实现。若为 null，则使用则使用每个元素的 System.IComparable</param>
		public virtual void Sort(IComparer comparer)
        {
            this._list.Sort(comparer);
        }


        #endregion
    }
}
