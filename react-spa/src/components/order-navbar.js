import React from 'react';
import {
  Collapse,
  Navbar,
  NavbarToggler,
  NavbarBrand,
  Nav,
  NavItem,
  NavLink } from 'reactstrap';

 export class OrderNavBar extends React.Component {
  constructor(props) {
    super(props);

    this.toggle = this.toggle.bind(this);
    this.state = {
      isOpen: false
    };
  }

  toggle() {
    this.setState({
      isOpen: !this.state.isOpen
    });
  }

  render() {
    return (
      <div id="navBar">
      <Navbar color="light" light expand="md">
        <NavbarBrand href="/">Order Management System</NavbarBrand>
        <NavbarToggler onClick={this.toggle} />
        <Collapse isOpen={this.state.isOpen} navbar>
          <Nav navbar>
            <NavItem>
              <NavLink href="/orders/orders">Orders</NavLink>
            </NavItem>
            <NavItem>
              <NavLink href="/orders/customers">Customers</NavLink>
            </NavItem>
            <NavItem>
              <NavLink href="/orders/products">Products</NavLink>
            </NavItem>
            <NavItem>
              <NavLink href="/contacts">Contact Details</NavLink>
            </NavItem>
          </Nav>
        </Collapse>
      </Navbar>
    </div>   
    );
  }
}
