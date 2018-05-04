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
function createDetails(name, email, mobilePhone, homePhone, address) {
  id += 1;
  return { id, name, email, mobilePhone, homePhone, address };
}

function CustomerTable(props) {
  const { classes } = props;

  const data = props.customer;
  const details =[];
  for (let index = 0; index < data.length; index++) {   
    details.push(createDetails(data[index].firstName + ' ' + data[index].lastName, data[index].email, data[index].mobilePhone, data[index].homePhone, data[index].addresses[0].street +', '+ data[index].addresses[0].city +', '+ data[index].addresses[0].zipCode));
  }
  //console.log(props);

  return (
      <div className="container-fluid">
        <div className="row-sm">
        <h5>Customer Details</h5>
        <hr/>
        </div>
        <Paper className={classes.root}>
            <Table className={classes.table}>
                <TableHead>
                <TableRow>
                    <TableCell>Customer</TableCell>
                    <TableCell>Email</TableCell>
                    <TableCell>Mobile Phone</TableCell>
                    <TableCell>Home Phone</TableCell>
                    <TableCell>Billing Address</TableCell>
                </TableRow>
                </TableHead>
                <TableBody>
                {details.map(n => {
                    return (
                    <TableRow key={n.id}>
                        <TableCell>{n.name}</TableCell>
                        <TableCell>{n.email}</TableCell>
                        <TableCell>{n.mobilePhone}</TableCell>
                        <TableCell>{n.homePhone}</TableCell>
                        <TableCell>{n.address}</TableCell>
                    </TableRow>
                    );
                })}
                </TableBody>
            </Table>
            </Paper>
      </div>
    
  );
}

CustomerTable.propTypes = {
  classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(CustomerTable);