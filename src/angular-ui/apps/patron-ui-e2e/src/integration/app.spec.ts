import { getGreeting } from '../support/app.po';

describe('patron-ui', () => {
  beforeEach(() => cy.visit('/'));

  it('should display welcome message', () => {
    getGreeting().contains('Welcome to patron-ui!');
  });
});
