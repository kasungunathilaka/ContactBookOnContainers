import { RECEIVE_CUSTOMER_DETAILS } from "../actions";

export default (state = {}, { type, customer }) => {
  switch (type) {
    case RECEIVE_CUSTOMER_DETAILS:
      return customer;
    default:
      return state;
  }
};
