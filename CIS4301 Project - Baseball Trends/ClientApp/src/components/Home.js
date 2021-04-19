import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;
    shoot() {
        alert('318948');
    }
  render () {
    return (
      <div>
        <h1>Welcome to Baseball Trends</h1>
            <p>Click on any of the pages above to see a graphical representation of an interesting baseball trend</p>
            <button onClick={this.shoot}>Number of Tuples</button>
      </div>
    );
  }
}
