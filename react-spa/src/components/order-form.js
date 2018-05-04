import React from 'react';
import { Button, Form, FormGroup, Label, Input, Col } from 'reactstrap';
import CustomerAutoSuggestField from './customerAutoSuggestField';
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import { requestCustomerNames } from "../actions";

class OrderForm extends React.Component {
componentDidMount() {
  this.props.requestCustomerNames();
  }

 render() {
     return(
       <div className="container-fluid">
        <div className="row-sm">
        <h5>Create a New Order</h5>
        <hr/>
        </div>

        <div className="container-fluid">
          <Form>
          <p>To create a new order, select an exisiting customer or create a new one.</p>
            <FormGroup row>
              <Col sm ="8">
                <CustomerAutoSuggestField customer = { this.props.customer }/>
              </Col>
              <Col sm="1">or</Col>
              <Col sm="3">
                <Button size="sm" color="primary" className="float-right">Create New Customer</Button>
              </Col>             
            </FormGroup>
            <div className="row-sm">Product Details :</div>
            <FormGroup row size="sm">
              <Col sm="2">
                <FormGroup size="sm">
                  <Label for="productName" size="sm">Product Name</Label>    
                  <Input type="text" name="productName" id="productName" placeholder="Product Name" bsSize="sm"/>           
                </FormGroup>                     
              </Col>
              <Col sm="2">
              <FormGroup size="sm">
                <Label for="productCategory" size="sm">Product Category</Label>
                <Input type="text" name="productCategory" id="productCategory" placeholder="Product Category" bsSize="sm"/>
              </FormGroup>               
              </Col> 
              <Col sm="3">
                <FormGroup size="sm">
                    <Label for="description" size="sm">Description</Label>
                    <Input type="text" name="description" id="description" placeholder="Description" bsSize="sm"/>
                </FormGroup>
              </Col>                 
              <Col sm="1">
                <FormGroup size="sm">
                  <Label for="price" size="sm">Unit Price</Label>    
                  <Input type="text" name="price" id="price" placeholder="Unit Price" bsSize="sm"/>           
                </FormGroup>                     
              </Col>
              <Col sm="2">
                <FormGroup size="sm">
                  <Label for="price" size="sm">Availability</Label>    
                  <Input type="text" name="availability" id="availability" placeholder="Availability" bsSize="sm"/>           
                </FormGroup>                     
              </Col>  
              <Col sm="1">
                <FormGroup size="sm">
                  <Label for="price" size="sm">Quantity</Label>    
                  <Input type="text" name="quantity" id="quantity" placeholder="Quantity" bsSize="sm"/>           
                </FormGroup>                     
              </Col>    
              <Col sm="1">
                <FormGroup size="sm">
                    <Label for="price" size="sm">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</Label>  
                    <Button size="sm" color="primary">Add Item</Button>
                </FormGroup>
              </Col>
            </FormGroup>
            


            <Button size="sm" color="primary" className="float-right">Create Order</Button>
          </Form>
        </div>      
       </div>      
     );
 }
}

const mapStateToProps = state => ({ customer: state.customer });

const mapDispatchToProps = dispatch =>
  bindActionCreators({ requestCustomerNames }, dispatch);

export default connect(mapStateToProps, mapDispatchToProps)(OrderForm);