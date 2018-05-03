import { call, put, takeLatest } from "redux-saga/effects";

import { REQUEST_PRODUCT_DETAILS, receiveProductDetails } from "../actions";
import { fetchProductDetails } from "../services/productService";

// worker Saga: will be fired on USER_FETCH_REQUESTED actions
function* getProductDetails(action) {
  try {
    // do api call
    const product = yield call(fetchProductDetails);
    yield put(receiveProductDetails(product));
  } catch (e) {
    console.log(e);
  }
}

export default function* productSaga() {
  yield takeLatest(REQUEST_PRODUCT_DETAILS, getProductDetails);
}
