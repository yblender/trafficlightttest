using Project1.Models.Enums;

namespace Project1.Models.Classes
{
    public class TrafficLight
    {
        public readonly Axis Axis;
        
        public string Direction { get; set; }
        public string Light { get; set; }
        public TrafficLight(Axis axis, TrafficDirection direction)
        {
            Axis = axis;
            Direction = direction.ToString();
            Light = Enums.Light.Red.ToString();
        }
        public static string ChangeLight(string light)
        {
            switch (light)
            {
                case "Green":
                    return Enums.Light.Yellow.ToString();
                case "Yellow":
                    return Enums.Light.Red.ToString();
                case "Red":
                    return Enums.Light.Green.ToString();
                default:
                    return Enums.Light.Red.ToString();
            }
        }
    }
}
