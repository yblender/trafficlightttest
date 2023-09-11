namespace Project1.Models.Classes
{
    public class TrafficResponse
    {
        public int ChangeTimer { get; set; }       
        public List<TrafficLight> yAxisLights { get; set; }
        public List<TrafficLight> xAxisLights { get; set; }
    }
}
