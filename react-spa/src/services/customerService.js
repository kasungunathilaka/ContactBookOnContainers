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
  