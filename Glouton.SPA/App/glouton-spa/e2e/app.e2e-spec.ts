import { Ingame.SpaPage } from './app.po';

describe('ingame.spa App', () => {
  let page: Ingame.SpaPage;

  beforeEach(() => {
    page = new Ingame.SpaPage();
  });

  it('should display welcome message', done => {
    page.navigateTo();
    page.getParagraphText()
      .then(msg => expect(msg).toEqual('Welcome to app!!'))
      .then(done, done.fail);
  });
});
