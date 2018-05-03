import { call, put, takeLatest } from "redux-saga/effects";

import { REQUEST_CUSTOMER_DETAILS, receiveCustomerDetails } from "../actions";
import { fetchCustomerDetails } from "../services/customerService";

// worker Saga: will be fired on USER_FETCH_REQUESTED actions
function* getCustomerDetails(action) {
  try {
    // do api call
    const customer = yield call(fetchCustomerDetails);
    yield put(receiveCustomerDetails(customer));
  } catch (e) {
    console.log(e);
  }
}

export default function* customerSaga() {
  yield takeLatest(REQUEST_CUSTOMER_DETAILS, getCustomerDetails);
}
