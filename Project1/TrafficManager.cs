using Microsoft.Extensions.Caching.Memory;
using Project1.Models.Classes;
using Project1.Models.Enums;

namespace Project1
{
    public class TrafficManager
    {
        private List<TrafficLight> _yAxisLights = new List<TrafficLight>();
        private List<TrafficLight> _xAxisLights = new List<TrafficLight>();
        private Axis activeAxis;
        private Light currentLight;
        private IMemoryCache _memoryCache;

        private int _normalGreenDuration = 20;
        private int _peakYAxisGreenDuration = 40;
        private int _peakXAxisGreenDuration = 10;
        private int _normalRedCrossTrafficDuration = 4;
        private int _normalYellowDuration = 5;
        private int _peakHourMorningStart = 8;
        private int _peakHourMorningEnd = 10;
        private int _peakHourEveningStart = 17;
        private int _peakHourEveningEnd = 19;
        private int _northBoundGreenDuration = 10;
        public TrafficManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }        
        public TrafficResponse GetTrafficRules(int hour)
        {
            activeAxis = (Axis)_memoryCache.Get("axis");
            currentLight = (Light)_memoryCache.Get("currentLight");
            UpdateLights();
            var timer = GetCountDownTimer(hour);
            UpdateAxis();
            return new TrafficResponse
            {
                ChangeTimer = timer,
                xAxisLights = _xAxisLights,
                yAxisLights = _yAxisLights
            };
        }
        public TrafficResponse InitialiseLights(int hour)
        {
            var r = new Random();
            var flip = r.Next(2);
            if(flip == 0)
            {
                _memoryCache.Set("axis",Axis.y);
                foreach(var light in _yAxisLights)
                {
                    if (light.Direction == TrafficDirection.NorthBoundRight.ToString())
                        continue;
                    light.Light = Light.Green.ToString();
                }
            }
            else
            {
                _memoryCache.Set("axis", Axis.x);
                foreach (var light in _xAxisLights)
                {
                    light.Light = Light.Green.ToString();
                }
            }
            var changeTimer = GetCountDownTimer(hour);
            _memoryCache.Set("currentLight", Light.Green);
            return new TrafficResponse
            {
                ChangeTimer = changeTimer,
                xAxisLights = _xAxisLights,
                yAxisLights = _yAxisLights
            };
        }
        public void AddYAxisLight(TrafficLight light)
        {
            if(light.Axis != Axis.y)
            {
                throw new Exception("Can only add Y axis lights to Y axis");
            }
            _yAxisLights.Add(light);
        }
        public void AddXAxisLight(TrafficLight light)
        {
            if (light.Axis != Axis.x)
            {
                throw new Exception("Can only add X axis lights to X axis");
            }
            _xAxisLights.Add(light);
        }
        private bool isPeak(int hour)
        {
            return (hour >= _peakHourMorningStart && hour<= _peakHourMorningEnd)
                || (hour >= _peakHourEveningStart && hour<= _peakHourEveningEnd);
        }
        private void UpdateLights()
        {
            
            currentLight =  Enum.Parse<Light>(TrafficLight.ChangeLight(currentLight.ToString()));
            if(activeAxis == Axis.x)
            {
                foreach(var  light in _xAxisLights)
                {
                    light.Light = currentLight.ToString();
                }
            }
            else
            {
                foreach (var light in _yAxisLights)
                {
                    if(activeAxis == Axis.y && light.Direction != TrafficDirection.NorthBoundRight.ToString())
                        light.Light = currentLight.ToString();
                    else if(activeAxis == Axis.ny && light.Direction == TrafficDirection.NorthBoundRight.ToString())
                        light.Light = currentLight.ToString();
                }
            }
            _memoryCache.Set("currentLight", currentLight);
        }
        private int GetCountDownTimer(int hour)
        {
            if (currentLight == Light.Green && isPeak(hour) && activeAxis == Axis.y)
                return _peakYAxisGreenDuration;
            if (currentLight == Light.Green && isPeak(hour) && activeAxis == Axis.x)
                return _peakXAxisGreenDuration;
            if (currentLight == Light.Green && activeAxis == Axis.ny)
                return _northBoundGreenDuration;
            if (currentLight == Light.Green)
                return _normalGreenDuration;
            if (currentLight == Light.Yellow)
                return _normalYellowDuration;
            return _normalRedCrossTrafficDuration;
        }
        private void UpdateAxis()
        {

            if (currentLight == Light.Red && activeAxis == Axis.x)
                _memoryCache.Set("axis", Axis.y);
            else if (currentLight == Light.Red && activeAxis == Axis.y)
                _memoryCache.Set("axis", Axis.ny);
            else if (currentLight == Light.Red && activeAxis == Axis.ny)
                _memoryCache.Set("axis", Axis.x);
        }
    }
}
