import React from 'react';
import { Button, Form, FormGroup, Label, Input, Col } from 'reactstrap';

export class CustomerForm extends React.Component {
 render() {
     return(
       <div className="container-fluid">
        <div className="row-sm">
        <h5>Create a New Customer</h5>
        <hr/>
        </div>
        <div className="container-fluid">
          <Form>
            <div className="row-sm">Personal Details :</div>
            <FormGroup row size="sm">
              <Col>
                <FormGroup size="sm">
                  <Label for="firstName" size="sm">First Name</Label>    
                  <Input type="text" name="firstName" id="firstName" placeholder="First Name" bsSize="sm"/>           
                </FormGroup>                     
              </Col>
              <Col>
              <FormGroup size="sm">
                <Label for="lastName" size="sm">Last Name</Label>
                <Input type="text" name="lastName" id="lastName" placeholder="Last Name" bsSize="sm"/>
              </FormGroup>               
              </Col>       
            </FormGroup>
            <FormGroup tag="fieldset" size="sm">
              <Label for="gender" size="sm">Gender</Label>
              &nbsp;&nbsp;&nbsp;
              <FormGroup check inline>
                <Label check size="sm">
                  <Input type="radio" name="gender" />{' '}
                  Male
                </Label>
              </FormGroup>
              <FormGroup check inline>
                <Label check size="sm">
                  <Input type="radio" name="gender" />{' '}
                  Female
                </Label>
              </FormGroup>
            </FormGroup>

            <div className="row-sm">Contact Details :</div>
            <FormGroup row size="sm">
              <Col>
                <FormGroup size="sm">
                  <Label for="email" size="sm">Email</Label>
                  <Input type="email" name="email" id="email" placeholder="name@example.com" bsSize="sm"/>
                </FormGroup> 
              </Col>
              <Col>
                <FormGroup size="sm">
                  <Label for="facebookId" size="sm">Facebook Id</Label>    
                  <Input type="text" name="facebookId" id="facebookId" placeholder="Facebook Id" bsSize="sm"/>           
                </FormGroup> 
              </Col>
            </FormGroup> 
            <FormGroup row size="sm">
              <Col>
                <FormGroup size="sm">
                  <Label for="mobilePhone" size="sm">Mobile Phone</Label>
                  <Input type="text" name="mobilePhone" id="mobilePhone" placeholder="XXXXXXXXXX" bsSize="sm"/>
                </FormGroup> 
              </Col>
              <Col>
                <FormGroup size="sm">
                <Label for="homePhone" size="sm">Home Phone</Label>
                  <Input type="text" name="homePhone" id="homePhone" placeholder="XXXXXXXXXX" bsSize="sm"/>          
                </FormGroup> 
              </Col>
            </FormGroup>   

            <div className="row-sm">Billing Address Details :</div>
            <FormGroup row size="sm">
              <Col sm="4">
                <FormGroup size="sm">
                  <Label for="street" size="sm">Street</Label>
                  <Input type="text" name="street" id="street" placeholder="Main St." bsSize="sm"/>
                </FormGroup> 
              </Col>
              <Col sm="3">
                <FormGroup size="sm">
                <Label for="city" size="sm">City</Label>
                  <Input type="text" name="city" id="city" placeholder="City" bsSize="sm"/>          
                </FormGroup> 
              </Col>
              <Col sm="3">
                <FormGroup size="sm">
                  <Label for="province" size="sm">Province</Label>
                  <Input type="text" name="province" id="province" placeholder="Province" bsSize="sm"/>
                </FormGroup> 
              </Col>
              <Col sm="2">
                <FormGroup size="sm">
                <Label for="zipcode" size="sm">Zip Code</Label>
                  <Input type="text" name="zipcode" id="zipcode" placeholder="Zip" bsSize="sm"/>          
                </FormGroup> 
              </Col>
            </FormGroup> 


            <Button size="sm" color="primary" className="float-right">Create Customer</Button>
          </Form>
        </div>      
       </div>      
     );
 }
}