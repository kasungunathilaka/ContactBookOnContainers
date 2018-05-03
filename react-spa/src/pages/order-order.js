import React from 'react';
import { OrderForm } from '../components/order-form';

export default class OrderPage extends React.Component {
 render() {
     return(
       <div className="container-fluid">
        <div className="row-sm">
            <OrderForm/>
        </div>    
       </div>      
     );
 }
}