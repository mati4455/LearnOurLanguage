import { LearnOurLanguage.FrontPage } from './app.po';

describe('learn-our-language.front App', () => {
  let page: LearnOurLanguage.FrontPage;

  beforeEach(() => {
    page = new LearnOurLanguage.FrontPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
