using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Controls.SkinForm
{
    public class XmlHelper
    {
        public static T ReadConfig<T>(string file)
        {
            TextReader textReader = new StreamReader(file);
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(T));
                T t = (T)s.Deserialize(textReader);
                return t;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(textReader!=null)
                textReader.Close();
            }
        }

        public static void WriteConfig<T>(string file, T t)
        {
            TextWriter textWriter = new StreamWriter(file);
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(T));
                s.Serialize(textWriter, t);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (textWriter != null)
                    textWriter.Close();
            }
        }
    }
}
