import { all, fork } from "redux-saga/effects";

import * as customerSagas from "./customerSaga";
import * as productSagas from "./productSaga";

export default function* rootSaga() {
    yield all(
      [...Object.values(customerSagas), ...Object.values(productSagas)].map(fork)
    );
  }