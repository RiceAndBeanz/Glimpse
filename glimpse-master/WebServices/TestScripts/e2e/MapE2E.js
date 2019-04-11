describe("Map Page", function() {

    beforeEach(function() {

        browser.get('http://localhost:55709/#!/login');
        browser.sleep(2000);
        browser.waitForAngular();
        element(by.model('user.username')).sendKeys('testaccount@gmail.com');
        element(by.model('user.password')).sendKeys('testpass');

        element(by.buttonText("Login")).click().then(function(){
            browser.sleep(2000);
            browser.waitForAngular('Promotions Page should be visible in atmost 20sec');
            browser.sleep(4000);
            browser.waitForAngular();
        });

        element(by.id('map')).click().then(function(){
            browser.sleep(3000);
            browser.waitForAngular();
        });
    });


    it('should load the map', function() {
        expect(element(by.className('map-container')).isPresent()).toBe(true);
        expect(element(by.tagName('ng-map')).isPresent()).toBe(true);
        expect(browser.getCurrentUrl()).toContain('map');
    });

    it('should show promotion on map', function() {
        var promotionElmCount = element.all(by.repeater('marker in randomMarkers')).count();
        promotionElmCount.then(function (count) {
            expect(count).toBeGreaterThan(0);
        });
    });

});