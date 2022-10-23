using System;
using System.Collections.Generic;
using System.Xml;

namespace LB4FH.GpxReverser
{
    class GpxTrack
    {
        /// <summary>
        /// Display name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of points on this track
        /// </summary>
        public List<TrackPoint> points = new List<TrackPoint>();

        /// <summary>
        /// Outputs the tracks to a new GPX file
        /// </summary>
        /// <param name="fileName">File name to output to</param>
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

            // For simplicity all points are added to a single track segment
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

                // Point times are not reversed. This gives a rough estimate of progress for human movement speed, as most logging software makes tracks
                // based on time. It is not an accurate method. Another option would be to interpolate time or generate a fresh time series. 
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
