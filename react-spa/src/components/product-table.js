import React from 'react';
import PropTypes from 'prop-types';
import { withStyles } from 'material-ui/styles';
import Table, { TableBody, TableCell, TableHead, TableRow } from 'material-ui/Table';
import Paper from 'material-ui/Paper';

const styles = theme => ({
  root: {
    width: '100%',
    marginTop: theme.spacing.unit * 3,
    overflowX: 'auto',
  },
  table: {
    minWidth: 700,
  },
});

let id = 0;
function createDetails(productName, productCategoryName, description, price, availability) {
  id += 1;
  return { id, productName, productCategoryName, description, price, availability };
}

function ProductTable(props) {
  const { classes } = props;

  const data = props.product;
  //console.log(data);
  const details =[];
  for (let index = 0; index < data.length; index++) {   
    details.push(createDetails(data[index].productName, data[index].productCategoryName, data[index].description, data[index].price, data[index].isAvailable === true?"In Stocks" : "Out of Stocks"));
  }
  //console.log(details);

  return (
      <div className="container-fluid">
        <div className="row-sm">
        <h5>Product Details</h5>
        <hr/>
        </div>
        <Paper className={classes.root}>
            <Table className={classes.table}>
                <TableHead>
                <TableRow>
                    <TableCell>Product</TableCell>
                    <TableCell>Product Category</TableCell>
                    <TableCell>Description</TableCell>
                    <TableCell>Unit Price</TableCell>
                    <TableCell>Availability</TableCell>
                </TableRow>
                </TableHead>
                <TableBody>
                {details.map(n => {
                    return (
                    <TableRow key={n.id}>
                        <TableCell>{n.productName}</TableCell>
                        <TableCell>{n.productCategoryName}</TableCell>
                        <TableCell>{n.description}</TableCell>
                        <TableCell>{n.price}</TableCell>
                        <TableCell>{n.availability}</TableCell>
                    </TableRow>
                    );
                })}
                </TableBody>
            </Table>
            </Paper>
      </div>
    
  );
}

ProductTable.propTypes = {
  classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(ProductTable);