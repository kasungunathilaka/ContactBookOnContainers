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
  Addresses: Address[];
}

export class Address {
  AddressId: string;
  street: string;
  city: string;
  province: string;
  zipCode: string;
}
