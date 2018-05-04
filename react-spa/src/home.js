import React from 'react';
import {BrowserRouter as Router, Route, Switch} from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import { OrderNavBar } from './components/order-navbar';
import CustomerPage  from './pages/order-customer';
import IndexPage  from './pages/order-index';
import OrderPage from './pages/order-order';
import ProductPage from './pages/order-product';
import { ContactNavBar } from './components/contact-navbar';

export class Home extends React.Component {
  render() {
    return(
        <Router>
          <div className="container-fluid">         
          <OrderNavBar/>
          <br/>
            <div>
              <Switch>
                <Route exact path='/orders' component={IndexPage}/> 
                <Route path='/orders/customers' component ={CustomerPage}/>
                <Route path='/orders/orders' component ={OrderPage}/>
                <Route path='/orders/products' component ={ProductPage}/>
                <Route path='/contacts' component ={ContactNavBar}/>
              </Switch>     
            </div>
          </div>   
        </Router>
    );
  }
}
