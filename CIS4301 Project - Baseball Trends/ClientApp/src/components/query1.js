import React, { Component } from 'react';
import { Line } from 'react-chartjs-2';
export class Query1 extends Component {
    static displayName = Query1.name;

    constructor(props) {
        super(props);
        this.state = {
            chart_labels: [],
            loading: true,
            chart_datasets: [],
            submitted: false,
            from: 'Enter Starting Year',
            to: 'Enter Ending Year',
            weight: 'Enter Weight'
        };
        this.handleChangeTo = this.handleChangeTo.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChangeFrom = this.handleChangeFrom.bind(this);
        this.handleChangeWeight = this.handleChangeWeight.bind(this);
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
                            text: "Comparing Average Homeruns hit by the entire league vs Average Homeruns hit by players above a given weight",
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
    handleChangeWeight(event) {
        this.setState({ weight: event.target.value });
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
        this.populatePitcherData(this.state.from, this.state.to, this.state.weight);
    }
    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Query1.renderPitchersTable(this.state.chart_labels, this.state.chart_datasets);

        return (
            <div>
                <h1 id="tabelLabel" >Homeruns</h1>
                <form onSubmit={this.handleSubmit}>
                    <input type="text" value={this.state.from} onChange={this.handleChangeFrom} id="from" />
                   
                    <input type="text" value={this.state.to} onChange={this.handleChangeTo} id="to" />
                    <input type="text" value={this.state.weight} onChange={this.handleChangeWeight} id="weight" />
                    <input type="submit" value="Submit" />
                </form>
                {this.state.submitted ? contents : null}
            </div>
        );
    }

    async populatePitcherData(from, to,weight) {
        var str = 'api/values/Query1?dateFrom=';
        str = str.concat(from);
        str = str.concat('&dateTo=');
        str = str.concat(to);
        str = str.concat('&weight=');
        str = str.concat(weight);
        //'api/values/Query1?dateFrom=1920&dateTo=2010'
        const response = await fetch(str);
        const data = await response.json();
        const temp_labels = [];
        const hr_avg = [];
        const hr_avg_weight = [];
        for (var i in data) {
            temp_labels.push(data[i].yearId);
            hr_avg.push(data[i].avgHrLeague);
            hr_avg_weight.push(data[i].avgHrWeight);
        }
        var hr_avg_weight_label = 'Homerun Average for players above ';
        hr_avg_weight_label = hr_avg_weight_label.concat(weight);
        hr_avg_weight_label = hr_avg_weight_label.concat(' pounds');
        const temp_datasets = [{
            label: 'Average Homeruns hit by entire league',
            data: hr_avg,
            fill: false,
            lineTension: 0.5,
            backgroundColor: '#332DDB',
            borderColor: '#332DDB',
            borderWidth: 2,
            pointRadius: 1.5,
        },
            {
            label: hr_avg_weight_label,
            data: hr_avg_weight,
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
