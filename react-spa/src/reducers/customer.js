import { RECEIVE_CUSTOMER_DETAILS, RECEIVE_CUSTOMER_NAMES } from "../actions";

export default (state = {}, { type, customer }) => {
 
  switch (type) {
    case RECEIVE_CUSTOMER_DETAILS:
      return customer;
    case RECEIVE_CUSTOMER_NAMES:
      return customer;
    default:
      return state;
  }
};
