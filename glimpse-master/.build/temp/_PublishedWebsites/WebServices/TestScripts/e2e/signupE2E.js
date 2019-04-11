describe('Sign Up E2E', function() {

  it('should try to login with a invalid vendor', function () {
      browser.get('http://localhost:55709/#!/login');
      expect(browser.getCurrentUrl()).toContain('login');
      element(by.model('user.username')).sendKeys('end2end@gmail.com');
      element(by.model('user.password')).sendKeys('testpass');
      element(by.buttonText("Login")).click();
      expect(browser.getCurrentUrl()).toContain('login');
      el = element(by.css('form-control-feedback'));
      e1.getText().then(function (text) { expect(text).toContain('Incorrect') });
      expect(browser.getCurrentUrl()).toContain('login');
  });

  it('should sign up using the new vendor information', function () {
      browser.get('http://localhost:55709/#!/signup');
      element(by.model('user.company')).sendKeys('end2end');
      element(by.model('user.email')).sendKeys('end2end@gmail.com');
      element(by.model('user.password')).sendKeys('testpass');
      element(by.model('user.confirmpassword')).sendKeys('testpass');
      element(by.model('user.country')).sendKeys('Canada');
      element(by.model('user.province')).sendKeys('Qc');
      element(by.model('user.city')).sendKeys('Montreal');
      element(by.model('user.postal')).sendKeys('H3A0G4');
      element(by.model('user.streetname')).sendKeys('Sherbrook');
      element(by.model('user.streetnumber')).sendKeys('845');
      element(by.model('user.personalphone')).sendKeys('3906936682392');
     
      var elm = element(by.id('signupBtn'));
      var EC = protractor.ExpectedConditions;

      browser.wait(EC.elementToBeClickable(elm), 5000);
      elm.click();
      browser.driver.sleep(3);
      browser.waitForAngular();

      expect(browser.getCurrentUrl()).toContain('login');
  });

  it('should login using the new vendor information', function () {
      browser.get('http://localhost:55709/#!/login');
      element(by.model('user.username')).sendKeys('end2end@gmail.com');
      element(by.model('user.password')).sendKeys('testpass');

      element(by.buttonText("Login")).click();
      expect(browser.getCurrentUrl()).toContain('promotions');
  });
});