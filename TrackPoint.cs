using System;

namespace LB4FH.GpxReverser
{
    /// <summary>
    /// Contains a single 4D point (latitude, longtitude, elevation and time)
    /// </summary>
    class TrackPoint
    {
        public string lat { get; set; }
        public string lon { get; set; }
        public string ele { get; set; }
        public DateTime time { get; set; }
    }
}