import React, { Component } from 'react';
import { Line } from 'react-chartjs-2';
export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
      this.state = { chart_labels: [], loading: true, chart_datasets: [], submitted: false, from: 'Enter Starting Year', to: 'Enter Ending Year', };
      this.handleChangeTo = this.handleChangeTo.bind(this);
      this.handleSubmit = this.handleSubmit.bind(this);
      this.handleChangeFrom = this.handleChangeFrom.bind(this);
  }

  //componentDidMount() {
    
 // }

  static renderPitchersTable(chart_labels,chart_datasets) {
      const temp_data = {
          labels: chart_labels,
          datasets: chart_datasets,
      }
      return (
        <div>
            <Line
                data={
                    temp_data
                }
                options={{
                    title: {
                        display: true,
                        text: "Comparing World Series Winner's attendance vs League Average",
                        fontSize: 20
                    },
                    legend: {
                        display: true,
                        position: 'right'
                    }
                }}
            />
        </div>
        
    );
    }
   
    handleChangeTo(event) {
        this.setState({ to: event.target.value });
    }
    handleChangeFrom(event) {
        this.setState({ from: event.target.value });
    }
    handleSubmit(event) {
        event.preventDefault();
        this.setState({ submitted: true });
        this.populatePitcherData(this.state.from,this.state.to);
    }
  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderPitchersTable(this.state.chart_labels, this.state.chart_datasets);

    return (
      <div>
        <h2 id="tabelLabel" >Comparing Attendance of World Series Winner to the League Average</h2>
            <form onSubmit={this.handleSubmit}>
                <input type="text" value={this.state.from} onChange={this.handleChangeFrom} id="from"/>
                <p>-</p>
                <input type="text" value={this.state.to} onChange={this.handleChangeTo} id="to" />
                <input type="submit" value="Submit" />
            </form>
            {this.state.submitted ? contents : null}
      </div>
    );
  }

  async populatePitcherData(from, to) {
      var str = 'api/values/Query2?dateFrom=';
      str = str.concat(from);
      str = str.concat('&dateTo=');
      str = str.concat(to);
      //'api/values/Query1?dateFrom=1920&dateTo=2010'
      const response = await fetch(str);
      const data = await response.json();
      const temp_labels = [];
      const winner_attendance = [];
      const league_avg = [];
      for (var i in data) {
          temp_labels.push(data[i].yearId);
          winner_attendance.push(data[i].attendanceWsChamps);
          league_avg.push(data[i].avgAttendanceLeague);
      }

      const temp_datasets = [{
          label: 'World Series Winner Total Attendance',
          data: winner_attendance,
          fill: false,
          lineTension: 0.5,
          backgroundColor: '#332DDB',
          borderColor: '#332DDB',
          borderWidth: 2,
          pointRadius: 1.5,
      },
      {
          label: 'League Average Attendance',
          data: league_avg,
          fill: false,
          lineTension: 0.5,
          backgroundColor: '#DB332D',
          pointRadius: 1.5,
          borderColor: '#DB332D',
          borderWidth: 2,
          }
      ];
        this.setState({ chart_datasets:temp_datasets,chart_labels:temp_labels, loading: false });
  }
}
