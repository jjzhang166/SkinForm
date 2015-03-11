using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace Controls.SkinForm
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        public SerializableDictionary() { }
        /// <summary>
        /// Serializer
        /// </summary>
        /// <param name="write"></param>
        public void WriteXml(XmlWriter write)
        {
            XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));

            foreach (KeyValuePair<TKey, TValue> kv in this)
            {
                write.WriteStartElement("key");
                KeySerializer.Serialize(write, kv.Key);
                write.WriteEndElement();
                write.WriteStartElement("value");
                ValueSerializer.Serialize(write, kv.Value);
                write.WriteEndElement();
            }
        }
        /// <summary>
        /// Deserializer
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(XmlReader reader)
        {
            reader.Read();
            XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("key");
                TKey tk = (TKey)KeySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("value");
                TValue vl = (TValue)ValueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                this.Add(tk, vl);
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }
    }
}



