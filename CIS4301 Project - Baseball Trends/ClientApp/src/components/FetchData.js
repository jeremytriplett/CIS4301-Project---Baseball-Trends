import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { pitchers: [], loading: true };
  }

  componentDidMount() {
    this.populatePitcherData();
  }

  static renderPitchersTable(pitchers) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>playerID</th>
            <th>stint</th>
            <th>yearId</th>
            <th>wins</th>
          </tr>
        </thead>
            <tbody>
          {pitchers.map(pitchers =>
            <tr key={pitchers.playerId}>
              <td>{pitchers.stint}</td>
              <td>{pitchers.yearId}</td>
              <td>{pitchers.wins}</td>
              <td>{pitchers.losses}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderPitchersTable(this.state.pitchers);

    return (
      <div>
        <h1 id="tabelLabel" >Pitchers</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populatePitcherData() {
    const response = await fetch('api/values');
    const data = await response.json();
    this.setState({ pitchers: data, loading: false });
  }
}
