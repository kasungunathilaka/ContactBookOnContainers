import { RECEIVE_PRODUCT_DETAILS } from "../actions";

export default (state = {}, { type, product }) => {
  switch (type) {
    case RECEIVE_PRODUCT_DETAILS:
      return product;
    default:
      return state;
  }
};
