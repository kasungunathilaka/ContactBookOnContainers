const orders_host_port = '172.20.3.94:32104';
const order_server = 'http://' + orders_host_port + '/';
const contacts_host_port = '172.20.3.94:32103';
const contact_server = 'http://' + contacts_host_port + '/';
const ApiUrl = 'api/v1/';

const orderServerWithApiUrl = order_server + ApiUrl;
export const ordersUrl = orderServerWithApiUrl + 'orders';

const contactServerWithApiUrl = contact_server + ApiUrl;
export const contactsUrl = contactServerWithApiUrl + 'contacts';


