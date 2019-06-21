import { getGreeting } from '../support/app.po';

describe('ui', () => {
  beforeEach(() => cy.visit('/'));

  it('should display welcome message', () => {
    getGreeting().contains('Welcome to ui!');
  });
});
