import React, { Component } from 'react';
import { Line } from 'react-chartjs-2';
export class Query3 extends Component {
    static displayName = Query3.name;

    constructor(props) {
        super(props);
        this.state = {
            chart_labels: [],
            loading: true,
            chart_datasets: [],
            submitted: false,
            from: 'Enter Starting Year',
            to: 'Enter Ending Year',
            percentile: 'Enter Percentile'
        };
        this.handleChangeTo = this.handleChangeTo.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChangeFrom = this.handleChangeFrom.bind(this);
        this.handleChangePercentile = this.handleChangePercentile.bind(this);
    }

    //componentDidMount() {

    // }

    static renderPitchersTable(chart_labels, chart_datasets) {
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
                            text: "Compares ERA of all pitchers vs ERA of pitchers in a given salary percentile",
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
    handleChangePercentile(event) {
        this.setState({ percentile: event.target.value });
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
        this.populatePitcherData(this.state.from, this.state.to, this.state.percentile);
    }
    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Query3.renderPitchersTable(this.state.chart_labels, this.state.chart_datasets);

        return (
            <div>
                <h2 id="tabelLabel" >Comparing ERA of all pitchers vs ERA of pitchers in a given percentile</h2>
                <form onSubmit={this.handleSubmit}>
                    <input type="text" value={this.state.from} onChange={this.handleChangeFrom} id="from" />

                    <input type="text" value={this.state.to} onChange={this.handleChangeTo} id="to" />
                    <input type="text" value={this.state.percentile} onChange={this.handleChangePercentile} id="percentile" />
                    <input type="submit" value="Submit" />
                </form>
                {this.state.submitted ? contents : null}
            </div>
        );
    }

    async populatePitcherData(from, to, percentile) {
        var str = 'api/values/Query3?dateFrom=';
        str = str.concat(from);
        str = str.concat('&dateTo=');
        str = str.concat(to);
        str = str.concat('&percentile=');
        str = str.concat(percentile);
        //'api/values/Query1?dateFrom=1920&dateTo=2010'
        const response = await fetch(str);
        const data = await response.json();
        const temp_labels = [];
        const league_avg = [];
        const percent_avg = [];
        for (var i in data) {
            temp_labels.push(data[i].yearId);
            league_avg.push(data[i].avgLeagueEra);
            percent_avg.push(data[i].avgPercentileEra);
        }
        var temp_label = 'Average ERA for pitchers in the ';
        temp_label = temp_label.concat(percentile);
        temp_label = temp_label.concat(' percentile of salary')
        const temp_datasets = [{
            label: 'Average ERA by all pitchers',
            data: league_avg,
            fill: false,
            lineTension: 0.5,
            backgroundColor: '#332DDB',
            borderColor: '#332DDB',
            borderWidth: 2,
            pointRadius: 1.5,
        },
        {
            label: temp_label,
            data: percent_avg,
            fill: false,
            lineTension: 0.5,
            backgroundColor: '#DB332D',
            pointRadius: 1.5,
            borderColor: '#DB332D',
            borderWidth: 2,
        }
        ];
        this.setState({ chart_datasets: temp_datasets, chart_labels: temp_labels, loading: false });
    }
}
