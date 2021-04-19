import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
//import { FetchData } from './components/FetchData';
//import { Counter } from './components/Counter';

import './custom.css'
import { Query3 } from './components/query3';
import { FetchData } from './components/query2'
import {Query1} from './components/query1'
import {Query4} from './components/query4'
import {Query5} from './components/query5'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/query-1' component={Query1} />
        <Route path='/query-2' component={FetchData} />
        <Route path='/query-3' component={Query3} />
        <Route path='/query-4' component={Query4} />
        <Route path='/query-5' component={Query5} />

        </Layout>
            
    );
  }
}
