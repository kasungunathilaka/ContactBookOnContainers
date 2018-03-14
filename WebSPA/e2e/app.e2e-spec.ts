import { AppPage } from './app.po';
import { CreateContactPage } from './create-contact.po';

describe('phone-book-spa App', () => {
  let page: AppPage;
  let createPage: CreateContactPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should get title', () => {
    page.navigateTo();
    expect(page.getTitleText()).toEqual('Phone Book');
  });

  it('should display Index page Header', () => {
    page.navigateTo();
    expect(page.getIndexText()).toEqual('Contacts');
  });

  it('redirect to create page', () => {
    page.navigateToCreatePage();
    expect(page.getPageTitle()).toEqual('Create a New Contact');
  }); 

  it('redirect to details page', () => {
    page.navigateToDetailsPage();
    expect(page.getPageTitle()).toEqual('Contact Details');
  }); 

  it('redirect to edit page', () => {
    page.navigateToEditPage();
    expect(page.getPageTitle()).toEqual('Edit Contact Details');
  }); 

  it('redirect to create page', () => {
    page.navigateTo();
    page.clickCreateContact();
    expect(page.getPageTitle()).toEqual('Create a New Contact');
  });
});
