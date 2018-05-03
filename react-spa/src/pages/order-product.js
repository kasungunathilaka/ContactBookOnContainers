import React from 'react';
import { ProductForm } from '../components/product-form';
import ProductTable from '../components/product-table';
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import { requestProductDetails } from "../actions";

class ProductPage extends React.Component {
componentDidMount() {
    this.props.requestProductDetails();
    }

 render() {
     return(
       <div className="container-fluid">
        <div className="row-sm">
            <ProductTable product = {this.props.product}/>
            <br/>
            <ProductForm/>
        </div>    
       </div>      
     );
 }
}

const mapStateToProps = state => ({ product: state.product });

const mapDispatchToProps = dispatch =>
  bindActionCreators({ requestProductDetails }, dispatch);

export default connect(mapStateToProps, mapDispatchToProps)(ProductPage);