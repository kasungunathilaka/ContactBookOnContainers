export const REQUEST_CUSTOMER_DETAILS = "REQUEST_CUSTOMER_DETAILS";
export const RECEIVE_CUSTOMER_DETAILS = "RECEIVE_CUSTOMER_DETAILS";

export const requestCustomerDetails = () => ({ type: REQUEST_CUSTOMER_DETAILS });
export const receiveCustomerDetails = customer => ({ type: RECEIVE_CUSTOMER_DETAILS, customer });


export const REQUEST_PRODUCT_DETAILS = "REQUEST_PRODUCT_DETAILS";
export const RECEIVE_PRODUCT_DETAILS = "RECEIVE_PRODUCT_DETAILS";

export const requestProductDetails = () => ({ type: REQUEST_PRODUCT_DETAILS });
export const receiveProductDetails = product => ({ type: RECEIVE_PRODUCT_DETAILS, product });