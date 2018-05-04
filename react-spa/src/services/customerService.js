import { ordersUrl } from "../urls";

export const fetchCustomerDetails = async () => {
  const customerUrl = ordersUrl + '/customers';

  try {
    const response = await fetch(customerUrl);
    const customer = await response.json();
    //console.log(response);
    return customer;
  } catch (e) {
      console.log(e);
  }
};


export const fetchCustomerNames = async () => {
  const customerUrl = ordersUrl + '/names';

  try {
    const response = await fetch(customerUrl);
    const customerNames = await response.json();
    // console.log(customerNames);
    return customerNames;
  } catch (e) {
      console.log(e);
  }
};
