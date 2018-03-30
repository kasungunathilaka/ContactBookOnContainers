import { CustomerDetails } from "./customer-details";

export class OrderDetails {
  orderId: string;
  orderDate: Date;
  isCompleted: boolean;
  customer: CustomerDetails;
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
