using System;
using System.IO;
using System.Xml;

namespace LB4FH.GpxReverser
{
    class Program
    {
        static void Main(string[] args)
        {
            if (string.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("Error: specify file to reverse:  'gpxreverser.exe sometrack.gpx'");
                return;
            }
            if (!args[0].EndsWith(".gpx"))
            {
                Console.WriteLine("Error: input filename must end with .gpx.");
                return;
            }

            string inputFileName = args[0];
            string outputFileName = inputFileName.Replace(".gpx", string.Empty) + "-reversed.gpx";

            var xmlData = string.Empty;
            using (StreamReader sr = new StreamReader(inputFileName))
            {
                xmlData = sr.ReadToEnd();
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData);
            var track = new GpxTrack();

            var elems = doc.GetElementsByTagName("trk");
            Console.WriteLine($"Found {elems.Count} track segments");
            foreach (XmlElement curSegment in elems.Item(0))
            {
                if (curSegment.Name == "name")
                {
                    track.Name = curSegment.InnerText;
                }
                if (curSegment.Name == "trkseg")
                {
                    foreach (XmlElement curPoint in curSegment.ChildNodes)
                    {
                        var tp = new TrackPoint()
                        {
                            lat = curPoint.Attributes["lat"].Value,
                            lon = curPoint.Attributes["lon"].Value,
                            ele = curPoint.ChildNodes[0].InnerText,
                            time = DateTime.Parse(curPoint.ChildNodes[1].InnerText)
                        };
                        track.points.Insert(track.points.Count, tp);
                    }
                }
            }
            Console.WriteLine($"Found {track.points.Count} points");
            try
            {
                track.Save(outputFileName);
                Console.WriteLine($"Reversed track saved to: {outputFileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save track: {ex.Message}");
            }
      

        }
    }
}
