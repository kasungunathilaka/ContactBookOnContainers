import { combineReducers } from "redux";

import customer from "./customer";
import product from "./product";

export default combineReducers({
  customer,
  product
});
