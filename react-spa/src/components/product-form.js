import React from 'react';
import { Button, Form, FormGroup, Label, Input, Col } from 'reactstrap';

export class ProductForm extends React.Component {
 render() {
     return(
       <div className="container-fluid">
        <div className="row-sm">
        <h5>Create a New Product</h5>
        <hr/>
        </div>

        <div className="container-fluid">
          <Form>
            <div className="row-sm">Product Details :</div>
            <FormGroup row size="sm">
              <Col>
                <FormGroup size="sm">
                  <Label for="productName" size="sm">Product Name</Label>    
                  <Input type="text" name="productName" id="productName" placeholder="Product Name" bsSize="sm"/>           
                </FormGroup>                     
              </Col>
              <Col>
              <FormGroup size="sm">
                <Label for="productCategory" size="sm">Product Category</Label>
                <Input type="text" name="productCategory" id="productCategory" placeholder="Product Category" bsSize="sm"/>
              </FormGroup>               
              </Col>       
            </FormGroup>
            <FormGroup size="sm">
                <Label for="description" size="sm">Description</Label>
                <Input type="textarea" name="description" id="description" placeholder="Description" bsSize="sm"/>
            </FormGroup>
            <FormGroup row size="sm">
              <Col>
                <FormGroup size="sm">
                  <Label for="price" size="sm">Unit Price</Label>    
                  <Input type="text" name="price" id="price" placeholder="Unit Price" bsSize="sm"/>           
                </FormGroup>                     
              </Col>
              <Col>
              <FormGroup check size="sm">
                <div className="row">
                    <Label for="isAvailable" size="sm">Availability</Label>
                </div>
                <Label check>
                    <Input type="checkbox"  name="isAvailable" id="isAvailable"  bsSize="sm" />{' '}
                    Is Available
                </Label>
              </FormGroup>               
              </Col>       
            </FormGroup>
            


            <Button size="sm" color="primary" className="float-right">Create Product</Button>
          </Form>
        </div>      
       </div>      
     );
 }
}