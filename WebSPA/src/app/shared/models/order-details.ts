export class OrderDetails {
  orderId: string;
  orderDate: Date;
  isCompleted: boolean;
  customerId: string;
  firstName: string;
  lastName: string;
  orderItems: OrderItem[];
}

export class OrderItem {
  orderItemId: string;
  quantity: number;
  productId: string;
  productName: string;
  description: string;
  price: number;
  isAvailable: boolean;
  productCategoryId: string;
  productCategoryName: string;
}
