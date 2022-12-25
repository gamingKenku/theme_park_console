using System;
using System.IO;
using System.Text;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

// для работы с файлами JSON использовался Newtonsoft Json.NET
using Newtonsoft.Json;

namespace theme_park_console
{
    // интерфейс, используемый в классах сериализаторов
    interface ISerialiser<T> where T : Attraction
    {
        void Serialise(AttractionLinkedList<T> list);
        AttractionLinkedList<T> Deserialise();
    }

    // класс сериализатора XML
    class XMLSerialiser : ISerialiser<Attraction>
    {
        public AttractionLinkedList<Attraction> Deserialise()
        {
            XmlDocument xDoc = new XmlDocument();
            AttractionLinkedList<Attraction> list = new AttractionLinkedList<Attraction>();
            string attractionTypeAttr;
            Attraction attraction;

            // документ загружается по пути, указанном в App.config
            xDoc.Load(Config.XML_Path);
            XmlElement xRoot = xDoc.DocumentElement;
            // проверить каждый элемент attraction
            foreach (XmlElement xNode in xRoot)
            {
                attractionTypeAttr = xNode.GetAttribute("attraction_type");

                // в зависимости от атрибута создать объект нужного класса
                switch (attractionTypeAttr)
                {
                    case "ferris_wheel":
                        attraction = new FerrisWheel();
                        break;
                    case "roller_coaster":
                        attraction = new RollerCoaster();
                        break;
                    case "bumping_cars":
                        attraction = new BumpingCars();
                        break;
                    default:
                        attraction = new FerrisWheel();
                        break;
                }

                // проверить все дочерние элементы 
                foreach (XmlNode childnode in xNode.ChildNodes)
                {
                    string nodeName = childnode.Name;

                    // поочередно задать значения созданного объекта attraction
                    switch (nodeName)
                    {
                        case "name":
                            attraction.name = childnode.InnerText;
                            break;
                        case "ticket_price":
                            attraction.ticket_price = Convert.ToDouble(childnode.InnerText);
                            break;
                        case "session_time":
                            attraction.session_time = Convert.ToDouble(childnode.InnerText);
                            break;
                        case "customers_group":
                            attraction.customers_group = Convert.ToInt32(childnode.InnerText);
                            break;
                        case "price":
                            attraction.price = Convert.ToDouble(childnode.InnerText);
                            break;
                    }
                }

                // добавить в список 
                list.Add(attraction);
            }

            return list;
        }

        public void Serialise(AttractionLinkedList<Attraction> list)
        {
            XmlDocument xDoc = new XmlDocument();
            // документ загружается по пути, указанном в App.config
            xDoc.Load(Config.XML_Path);
            // корень xml документа ("attractions")
            XmlElement xRoot = xDoc.DocumentElement;
            // атрибут, указывающий на тип объекта
            XmlAttribute attractionTypeAttr;
            // элементы XML, представляющие добавляемый аттракцион и все его открытые поля
            XmlElement xAttractionElem, nameElem, ticketPriceElem, sessionTimeElem, customersGroupElem, priceElem;
            // элементы, представляющие значения полей объекта
            XmlText nameText, typeText, ticketPriceText, sessionTimeText, customersGroupText, priceText;
            AttractionTypes type;

            // очистить файл для перезаписи
            xRoot.RemoveAll();

            foreach (Attraction element in list)
            {
                // элемент добавляемого аттракциона
                xAttractionElem = xDoc.CreateElement("attraction");

                // поочередно записать все поля объекта
                nameElem = xDoc.CreateElement("name");
                nameText = xDoc.CreateTextNode(element.name);
                nameElem.AppendChild(nameText);
                xAttractionElem.AppendChild(nameElem);

                // задать тип атрибутом в зависимости от записываемого объекта
                attractionTypeAttr = xDoc.CreateAttribute("attraction_type");
                type = AmusementPark.GetAttractionType(element);
                switch (type)
                {
                    case AttractionTypes.FerrisWheel:
                        typeText = xDoc.CreateTextNode("ferris_wheel");
                        break;
                    case AttractionTypes.RollerCoaster:
                        typeText = xDoc.CreateTextNode("roller_coaster");
                        break;
                    case AttractionTypes.BumpingCars:
                        typeText = xDoc.CreateTextNode("bumping_cars");
                        break;
                    default:
                        throw new ArgumentException();
                }
                attractionTypeAttr.AppendChild(typeText);
                xAttractionElem.Attributes.Append(attractionTypeAttr);

                ticketPriceElem = xDoc.CreateElement("ticket_price");
                ticketPriceText = xDoc.CreateTextNode(Convert.ToString(element.ticket_price));
                ticketPriceElem.AppendChild(ticketPriceText);
                xAttractionElem.AppendChild(ticketPriceElem);

                sessionTimeElem = xDoc.CreateElement("session_time");
                sessionTimeText = xDoc.CreateTextNode(Convert.ToString(element.session_time));
                sessionTimeElem.AppendChild(sessionTimeText);
                xAttractionElem.AppendChild(sessionTimeElem);

                customersGroupElem = xDoc.CreateElement("customers_group");
                customersGroupText = xDoc.CreateTextNode(Convert.ToString(element.customers_group));
                customersGroupElem.AppendChild(customersGroupText);
                xAttractionElem.AppendChild(customersGroupElem);

                priceElem = xDoc.CreateElement("price");
                priceText = xDoc.CreateTextNode(Convert.ToString(element.price));
                priceElem.AppendChild(priceText);
                xAttractionElem.AppendChild(priceElem);

                // добавить в корневой элемент
                xRoot.AppendChild(xAttractionElem);
            }
            // сохранить файл
            xDoc.Save(Config.XML_Path);
        }
    }

    // класс сериализатора JSON
    class JSONSerialiser : ISerialiser<Attraction>
    {
        public AttractionLinkedList<Attraction> Deserialise()
        {
            // возвращаемый список
            AttractionLinkedList<Attraction> list = new AttractionLinkedList<Attraction>();
            // очередной объект Attraction, добавляемый в список
            Attraction attraction;

            // строка, читаемая из файла
            string json;
            // файл загружается по пути, указанном в App.config
            using (StreamReader sr = new StreamReader(Config.JSON_Path))
            {
                json = sr.ReadToEnd();
                sr.Close();
            }

            // для чтения json используется JsonTextReader
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            // проверять, пока в строке ещё остались токены
            while (reader.Read())
            {
                // когда текущий токен является началом объекта (или "{")
                if (reader.TokenType == JsonToken.StartObject)
                {
                    reader.Read(); // пропустить токен с назваением поля
                    reader.Read();

                    // создать нужный объект в зависимости от типа
                    if ((string) reader.Value == "ferris_wheel")
                    {
                        attraction = new FerrisWheel();
                    }
                    else if ((string)reader.Value == "roller_coaster")
                    {
                        attraction = new RollerCoaster();
                    }
                    else if ((string)reader.Value == "bumping_cars")
                    {
                        attraction = new BumpingCars();
                    }
                    else
                    {
                        attraction = new FerrisWheel();
                    }
                    
                    // поочередно задать значения объекта Attraction 
                    reader.Read();
                    reader.Read();
                    attraction.name = (string)reader.Value;

                    reader.Read();
                    reader.Read();
                    attraction.ticket_price = (double)reader.Value;

                    reader.Read();
                    reader.ReadAsInt32();
                    attraction.customers_group = (int)reader.Value;

                    reader.Read();
                    reader.Read();
                    attraction.session_time = (double)reader.Value;

                    reader.Read();
                    reader.Read();
                    attraction.price = (double)reader.Value;

                    // добавить созданный объект в список
                    list.Add(attraction);
                }
            }

            return list;
        }

        public void Serialise(AttractionLinkedList<Attraction> list)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            AttractionTypes type;

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                // начать массив объектов
                writer.WriteStartArray();

                foreach (Attraction element in list)
                {
                    writer.WriteStartObject();

                    writer.WritePropertyName("type");
                    type = AmusementPark.GetAttractionType(element);
                    // задать тип в зависимости от записываемого объекта
                    switch (type)
                    {
                        case AttractionTypes.FerrisWheel:
                            writer.WriteValue("ferris_wheel");
                            break;
                        case AttractionTypes.RollerCoaster:
                            writer.WriteValue("roller_coaster");
                            break;
                        case AttractionTypes.BumpingCars:
                            writer.WriteValue("bumping_cars");
                            break;
                        default:
                            writer.WriteValue("ferris_wheel");
                            break;
                    }

                    // поочередно записать все поля объекта в строку
                    writer.WritePropertyName("name");
                    writer.WriteValue(element.name);

                    writer.WritePropertyName("ticker_price");
                    writer.WriteValue(element.ticket_price);

                    writer.WritePropertyName("customers_group");
                    writer.WriteValue(element.customers_group);

                    writer.WritePropertyName("session_time");
                    writer.WriteValue(element.session_time);

                    writer.WritePropertyName("price");
                    writer.WriteValue(element.price);

                    writer.WriteEndObject();
                }

                // закончить массив объектов
                writer.WriteEndArray();
            }

            try
            {
                // файл загружается по пути, указанном в App.config
                using (StreamWriter writer = new StreamWriter(Config.JSON_Path, false))
                {
                    // записать строку в файл
                    writer.Write(sw.ToString());
                    writer.Close();
                }
            }
            catch
            {
                throw new FileNotFoundException();
            }
        }
    }
}
