import { TheSecretCode.CrmPage } from './app.po';

describe('the-secret-code.crm App', () => {
  let page: TheSecretCode.CrmPage;

  beforeEach(() => {
    page = new TheSecretCode.CrmPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!');
  });
});
