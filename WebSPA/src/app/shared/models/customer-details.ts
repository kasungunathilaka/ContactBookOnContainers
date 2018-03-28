export class CustomerDetails {
  customerId: string;
  firstName: string;
  lastName: string;
  gender: string;
  contactDetailsId: string;
  email: string;
  mobilePhone: string;
  homePhone: string;
  facebookId: string;
  addresses: Address[];
}

export class Address {
  addressId: string;
  street: string;
  city: string;
  province: string;
  zipCode: string;
}
