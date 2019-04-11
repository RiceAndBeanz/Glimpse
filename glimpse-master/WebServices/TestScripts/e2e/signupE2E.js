describe('Sign Up E2E', function() {

    var e2eEmailAddress;

    function generateEmailAddress() {
        var text = "";
        var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        for (var index = 0; index < 5; index++)
            text += possible.charAt(Math.floor(Math.random() * possible.length));

        return text + "@gmail.com";
    }

    e2eEmailAddress = generateEmailAddress();

    it('should try to login with a invalid vendor', function () {
        browser.get('http://localhost:55709/#!/login');
        expect(browser.getCurrentUrl()).toContain('login');
        element(by.model('user.username')).sendKeys(e2eEmailAddress);
        element(by.model('user.password')).sendKeys('testpass');
        element(by.buttonText("Login")).click();
        expect(browser.getCurrentUrl()).toContain('login');
        var el = element(by.css('.form-control-feedback'));
        if (!!el)
            el.getText().then(function (text) { expect(text).toContain('Incorrect') });
                expect(browser.getCurrentUrl()).toContain('login');
    });

    it('should sign up using the new vendor information', function () {

        browser.get('http://localhost:55709/#!/signup');
        element(by.model('user.company')).sendKeys('end2end');
        element(by.model('user.email')).sendKeys(e2eEmailAddress);
        element(by.model('user.password')).sendKeys('testpass');
        element(by.model('user.confirmpassword')).sendKeys('testpass');
        element(by.model('user.country')).sendKeys('Canada');
        element(by.model('user.province')).sendKeys('Qc');
        element(by.model('user.city')).sendKeys('Montreal');
        element(by.model('user.postal')).sendKeys('H3A0G4');
        element(by.model('user.streetname')).sendKeys('Sherbrook');
        element(by.model('user.streetnumber')).sendKeys('845');
        element(by.model('user.personalphone')).sendKeys('5146666666');

        var elm = element(by.id('signupBtn'));
        var EC = protractor.ExpectedConditions;

        browser.wait(EC.elementToBeClickable(elm), 5000);
        elm.click().then(function () {
            browser.sleep(5000);
            browser.waitForAngular();

            expect(browser.getCurrentUrl()).toContain('login');
        });
    });

    it('should login using the new vendor information', function () {
        browser.get('http://localhost:55709/#!/login');
        browser.sleep(5000);
        browser.waitForAngular();
        element(by.model('user.username')).sendKeys(e2eEmailAddress);
        element(by.model('user.password')).sendKeys('testpass');

        element(by.buttonText("Login")).click();
        expect(browser.getCurrentUrl()).toContain('promotions');
    });
});