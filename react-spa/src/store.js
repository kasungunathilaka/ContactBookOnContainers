import { createStore, applyMiddleware } from "redux";
import createSagaMiddleware from "redux-saga";
import { logger } from 'redux-logger';

import reducer from "./reducers";
import saga from "./sagas";

// create the saga middleware
const sagaMiddleware = createSagaMiddleware();
// mount it on the Store
export default createStore(reducer, applyMiddleware(sagaMiddleware, logger));

// then run the saga
sagaMiddleware.run(saga);
