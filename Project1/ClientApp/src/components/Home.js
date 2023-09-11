import React, { useState, Component } from 'react';
import { Intersection } from './Intersection';

export class Home extends Component {
  static displayName = Home.name;

 
    
  render() {
    return (
        <div>
            <Intersection></Intersection>       
      </div>
        );
    }

}
