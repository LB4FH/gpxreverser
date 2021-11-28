using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LB4FH.GpxReverser
{
    class GpxTrack
    {
        public string Name { get; set; }
        public List<TrackPoint> points = new List<TrackPoint>();

        internal void Save(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            XmlElement gpx = doc.CreateElement(string.Empty, "gpx", "http://www.topografix.com/GPX/1/1");
            gpx.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            gpx.SetAttribute("creator", "LB4FH");
            gpx.SetAttribute("version", "1.0");
            gpx.SetAttribute("xsi:schemaLocation", "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd");
            doc.AppendChild(gpx);

            XmlElement trk = doc.CreateElement("trk");
            XmlElement trkName = doc.CreateElement("name");
            trkName.InnerText = DateTime.Now.ToString();
            trk.AppendChild(trkName);

            XmlElement trkSeg = doc.CreateElement("trkseg");
            trk.AppendChild(trkSeg);

            for (int i = points.Count - 1; i > 0; i--)
            {
                XmlElement trkpt = doc.CreateElement("trkpt");
                trkpt.SetAttribute("lat", points[i].lat);
                trkpt.SetAttribute("lon", points[i].lon);


                XmlElement ele = doc.CreateElement("ele");
                ele.InnerText = points[i].ele;
                trkpt.AppendChild(ele);

                XmlElement time = doc.CreateElement("time");
                time.InnerText = points[points.Count - i].time.ToString();
                trkpt.AppendChild(time);

                trkSeg.AppendChild(trkpt);
            }



            gpx.AppendChild(trk);

            doc.Save(fileName);
        }
    }

}
