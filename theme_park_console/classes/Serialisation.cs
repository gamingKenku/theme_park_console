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
    interface ISerialiser<T> where T : Attraction
    {
        void Serialise(AttractionLinkedList<T> list);
        AttractionLinkedList<T> Deserialise();
    }
    class XMLSerialiser : ISerialiser<Attraction>
    {
        public AttractionLinkedList<Attraction> Deserialise()
        {
            XmlDocument xDoc = new XmlDocument();
            AttractionLinkedList<Attraction> list = new AttractionLinkedList<Attraction>();
            string attractionTypeAttr;
            Attraction attraction;

            xDoc.Load(Config.XML_Path);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlElement xNode in xRoot)
            {
                attractionTypeAttr = xNode.GetAttribute("attraction");

                switch(attractionTypeAttr)
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

                foreach (XmlNode childnode in xNode.ChildNodes)
                {
                    string nodeName = childnode.Name;

                    switch(nodeName)
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

                list.Add(attraction);
            }

            return list;
        }

        public void Serialise(AttractionLinkedList<Attraction> list)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Config.XML_Path);
            XmlElement xRoot = xDoc.DocumentElement;
            XmlAttribute attractionTypeAttr;
            XmlElement xAttractionElem, nameElem, ticketPriceElem, sessionTimeElem, customersGroupElem, priceElem;
            XmlText nameText, typeText, ticketPriceText, sessionTimeText, customersGroupText, priceText;
            AttractionTypes type;

            xRoot.RemoveAll();

            foreach (Attraction element in list)
            {
                xAttractionElem = xDoc.CreateElement("attraction");

                attractionTypeAttr = xDoc.CreateAttribute("attraction_type");
                ticketPriceElem = xDoc.CreateElement("ticket_price");
                sessionTimeElem = xDoc.CreateElement("session_time");
                customersGroupElem = xDoc.CreateElement("customers_group");
                priceElem = xDoc.CreateElement("price");
                nameElem = xDoc.CreateElement("name");

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
                
                ticketPriceText = xDoc.CreateTextNode(Convert.ToString(element.ticket_price));
                sessionTimeText = xDoc.CreateTextNode(Convert.ToString(element.session_time));
                customersGroupText = xDoc.CreateTextNode(Convert.ToString(element.customers_group));
                priceText = xDoc.CreateTextNode(Convert.ToString(element.price));
                nameText = xDoc.CreateTextNode(element.name);

                attractionTypeAttr.AppendChild(typeText);
                ticketPriceElem.AppendChild(ticketPriceText);
                sessionTimeElem.AppendChild(sessionTimeText);
                customersGroupElem.AppendChild(customersGroupText);
                priceElem.AppendChild(priceText);
                nameElem.AppendChild(nameText);

                xAttractionElem.Attributes.Append(attractionTypeAttr);
                xAttractionElem.AppendChild(nameElem);
                xAttractionElem.AppendChild(ticketPriceElem);
                xAttractionElem.AppendChild(sessionTimeElem);
                xAttractionElem.AppendChild(customersGroupElem);
                xAttractionElem.AppendChild(priceElem);

                xRoot.AppendChild(xAttractionElem);
            }
            xDoc.Save(Config.XML_Path);
        }
    }
}
