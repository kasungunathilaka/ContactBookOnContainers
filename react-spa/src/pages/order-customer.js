import React from 'react';
import CustomerTable from '../components/customer-table';
import { CustomerForm } from '../components/customer-form';
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import { requestCustomerDetails } from "../actions";

class CustomerPage extends React.Component {
  componentDidMount() {
    this.props.requestCustomerDetails();
  }

  render() {    
     return(
       <div className="container-fluid">
        <div className="row-sm">
        <CustomerTable customer = {this.props.customer}/>
        <br/>
        <CustomerForm/>
        </div>    
       </div>      
     );
  }
}

const mapStateToProps = state => ({ customer: state.customer });

const mapDispatchToProps = dispatch =>
  bindActionCreators({ requestCustomerDetails }, dispatch);

export default connect(mapStateToProps, mapDispatchToProps)(CustomerPage);