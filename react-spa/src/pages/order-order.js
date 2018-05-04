import React from 'react';
import OrderForm from '../components/order-form';

export default class OrderPage extends React.Component {

 render() {
     //console.log(this.props.customerName);
     return(
       <div className="container-fluid">
        <div className="row-sm">
            <OrderForm/>
        </div>    
       </div>      
     );
 }
}



