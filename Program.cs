using System;
using System.IO;
using System.Xml;

namespace LB4FH.GpxReverser
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Input validation
            if (string.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("Error, no input file specified");
                Console.WriteLine("Start application with: 'gpxreverser.exe track.gpx'");
                return;
            }
            if (args.Length> 1)
            {
                Console.WriteLine("Error, too many arguments");
                Console.WriteLine("Start application with: 'gpxreverser.exe track.gpx'");
                return;
            }
            if (!args[0].EndsWith(".gpx"))
            {
                Console.WriteLine("Error: input filename must end with .gpx");
                return;
            }
            #endregion

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

            // Gpx files are separated into elements called "Track segments", with the shortname trk
            var elems = doc.GetElementsByTagName("trk");          
            
            Console.WriteLine($"Found {elems.Count} track segments");

            // Iterate over the segments to get different values
            foreach (XmlElement curSegment in elems.Item(0))
            {

                // Name is commonly used as the display name
                if (curSegment.Name == "name")
                {
                    track.Name = curSegment.InnerText;
                }

                // Each GPX file consists of one or more segments. These segments are usually set up automatically by the track logging software.
                // For simplicity this program combines all track segments into one segment in the reversed track.
                if (curSegment.Name == "trkseg")
                {
                    // Each point consists of at least four elements; latitude, longtitude, elevation and time.
                    // The GPX sources I've seen all make these the same way, but there may be some that reverse elevation or time, so the code below here should be improved to handle that.
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
