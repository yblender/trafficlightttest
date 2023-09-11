using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Project1.Models.Classes;
using Project1.Models.Enums;

namespace Project1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrafficController : ControllerBase
    {
        TrafficManager _trafficManager;
        public TrafficController(IMemoryCache memoryCache)
        {
            _trafficManager = new TrafficManager(memoryCache);
            _trafficManager.AddXAxisLight(new TrafficLight(Axis.x, TrafficDirection.EastBound));
            _trafficManager.AddXAxisLight(new TrafficLight(Axis.x, TrafficDirection.WestBound));
            _trafficManager.AddYAxisLight(new TrafficLight(Axis.y, TrafficDirection.SouthBound));
            _trafficManager.AddYAxisLight(new TrafficLight(Axis.y, TrafficDirection.NorthBound));
            _trafficManager.AddYAxisLight(new TrafficLight(Axis.y, TrafficDirection.NorthBoundRight));
        }      
        [HttpGet("{hour:int}")]
        public TrafficResponse Get(int hour)
        {            
            return _trafficManager.GetTrafficRules(hour);
        }
        [HttpGet("Initialise")]
        public TrafficResponse Initialise(int hour)
        {
            return _trafficManager.InitialiseLights(hour);
        }
    }
}
