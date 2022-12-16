using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theme_park_console
{
    interface ISerialisator<T> where T : Attraction
    {
        void Serialise(AttractionLinkedList<T> list, string path);
        AttractionLinkedList<T> Deserialise(string path);
    }
    class XMLSerialisator : ISerialisator<Attraction>
    {
        public AttractionLinkedList<Attraction> Deserialise(string path)
        {
            throw new NotImplementedException();
        }

        public void Serialise(AttractionLinkedList<Attraction> list, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AttractionLinkedList<Attraction>));
                serializer.Serialize(writer, list);
                writer.Close();
            }
        }
    }
}
