import { browser, by, element } from 'protractor';

export class CreateContactPage {
  navigateTo() {
    return browser.get('/addContact');
  }
}