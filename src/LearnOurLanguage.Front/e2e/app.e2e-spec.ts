import { LearnOurLanguage.Web.FrontPage } from './app.po';

describe('learn-our-language.web.front App', () => {
  let page: LearnOurLanguage.Web.FrontPage;

  beforeEach(() => {
    page = new LearnOurLanguage.Web.FrontPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
