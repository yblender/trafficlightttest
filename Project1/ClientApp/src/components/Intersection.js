import React, { useState, useEffect } from 'react';

export function Intersection() {
    const [clock, setClock] = useState({
        hours: new Date().getHours(),
        minutes: 0,
        seconds: 0
    });
    const [isRunning, setIsRunning] = useState(false);
    const [trafficData, setTrafficData] = useState(null);
    const [changeTimer, setChangeTimer] = useState(0);
    const initialise = async () => {
        try {
            const response =  await fetch('traffic/initialise/?hour=' + clock.hours);
            const apiData =  await response.json();
            console.log(apiData);
            setChangeTimer(apiData.changeTimer);
            setTrafficData(apiData);
            
        } catch (error) {
            console.error('Error fetching data:', error);
        }
    }
    const fetchData = async () => {
        try {
            const response = await fetch('traffic/' + clock.hours);
            const apiData = await response.json();
            console.log(apiData);
            
            setIsRunning(true);
            setChangeTimer(apiData.changeTimer);
            setTrafficData(apiData);
            console.log(isRunning);
        } catch (error) {
            console.error('Error fetching data:', error);
        }
    }
    useEffect(() => {       
        initialise();
        setIsRunning(true);
    },[])
   
    useEffect(() => {
        const intervalId = setInterval(() => {

            if (!isRunning)
                return;
            let newHours = clock.hours;
            let newMinutes = clock.minutes;
            let newSeconds = clock.seconds + 1;
           
           
            if (newSeconds === 60) {
                newMinutes += 1;
                newSeconds = 0;
            }

            if (newMinutes === 60) {
                newHours += 1;
                newMinutes = 0;
            }
            console.log(changeTimer);
            setClock({
                hours: newHours,
                minutes: newMinutes,
                seconds: newSeconds
            });

            if (changeTimer === 0) {
                setIsRunning(false);
                fetchData();
            }
            else
                setChangeTimer(changeTimer - 1);
           
           
        }, 1000)
        return () => clearInterval(intervalId);
    }, [changeTimer])

    const handleHourChange = async (e) => {
        const newHours = parseInt(e.target.value, 10);
        setClock({
            hours: newHours,
            minutes: clock.minutes,
            seconds: clock.seconds
        });
        try {
            const response =  await fetch('traffic/' + newHours);
            const apiData = await response.json();
            console.log(apiData);
            setTrafficData(apiData);
            setChangeTimer(apiData.changeTimer);
        } catch (error) {
            console.error('Error fetching data:', error);
        }
    }
    const renderYTrafficLights = () => {
        if (trafficData == null)
            return;
        return trafficData.yAxisLights.map((light, index) => (
            <div key={index} className={'traffic-light ' + light.direction}>
                <div className={light.light == "Green" ? "circle green" : "circle white"}></div>
                <div className={light.light == "Yellow" ? "circle yellow" : "circle white"}></div>
                <div className={light.light == "Red" ? "circle red" : "circle white"}></div>
            </div>
        ))
}
    const renderXTrafficLights = () => {
        if (trafficData == null)
            return;
    return trafficData.xAxisLights.map((light, index) => (
        <div key={index} className={'traffic-light ' + light.direction}>
            <div className={light.light == "Green" ? "circle green" : "circle white"}></div>
            <div className={light.light == "Yellow" ? "circle yellow" : "circle white"}></div>
            <div className={light.light == "Red" ? "circle red" : "circle white"}></div>
        </div>
    ))
    }
    return (
        <div>
            <div class="timer">
            <input
                type="number"
                placeholder={String(clock.hours).padStart(2, '0')}
                value={clock.hours}
                onChange={handleHourChange}
            />:
            <span>{String(clock.minutes).padStart(2, '0')}:</span>
                <span>{String(clock.seconds).padStart(2, '0')}</span>
            </div>
            <div class="intersection">
                <div class="xAxis">
                    {renderXTrafficLights()}
                </div>
                <div class="yAxis">
                    {renderYTrafficLights()}
                   
                </div>
            </div>
        </div>

    );
   
   
}