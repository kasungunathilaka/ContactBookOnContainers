import { call, put, takeLatest } from "redux-saga/effects";
import {all} from 'redux-saga/effects';
import { REQUEST_CUSTOMER_DETAILS, receiveCustomerDetails } from "../actions";
import { REQUEST_CUSTOMER_NAMES, receiveCustomerNames } from "../actions";
import { fetchCustomerDetails, fetchCustomerNames } from "../services/customerService";

// worker Saga: will be fired on USER_FETCH_REQUESTED actions
function* getCustomerDetails(action) {
  try {
    const customer = yield call(fetchCustomerDetails);
    yield put(receiveCustomerDetails(customer));
  } catch (e) {
    console.log(e);
  }
}

function* getCustomerNames(action){
  try {
    const customerName = yield call(fetchCustomerNames);
    yield put(receiveCustomerNames(customerName));
  } catch (e) {
    console.log(e);
  }
}

export default function* customerSaga() {
  yield all([
    takeLatest(REQUEST_CUSTOMER_DETAILS, getCustomerDetails),
    takeLatest(REQUEST_CUSTOMER_NAMES, getCustomerNames)
  ]);
}

// export function* customerSaga2() {
//   yield takeLatest(REQUEST_CUSTOMER_NAMES, getCustomerNames);
// }
