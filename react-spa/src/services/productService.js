import { ordersUrl } from "../urls";

export const fetchProductDetails = async () => {
    const productsUrl = ordersUrl + '/products';

    try {
      const response = await fetch(productsUrl);
      const product = await response.json();
      //console.log(response);
      return product;
    } catch (e) {
        console.log(e);
    }
  };
  