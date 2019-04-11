describe("Promotions Page", function () {

    beforeEach(function () {

        browser.get('http://localhost:55709/#!/login');
        browser.sleep(2000);
        browser.waitForAngular();
        element(by.model('user.username')).sendKeys('testaccount@gmail.com');
        element(by.model('user.password')).sendKeys('testpass');

        element(by.buttonText("Login")).click().then(function () {
            browser.sleep(2000);
            browser.waitForAngular('Promotions Page should be visible for 20sec');
            browser.sleep(5000);
            browser.waitForAngular();
        });
    });

    it('should able to search promotions', function () {
        element(by.model('searchPromotionText')).sendKeys('more!');
        browser.sleep(5000);

        var promotionsElmCount = element.all(by.repeater('(key, promotion) in promotions | reverse | filter:searchPromotionText | filter:{Category: selectVal}')).count();
        promotionsElmCount.then(function (count) {
            expect(count).toEqual(1);
        });
    });


    it('should able to select promotions category', function () {
        element(by.model('searchPromotionText')).clear();
        element(by.id('electronics')).click();
        browser.sleep(5000);

        var promotionsElm = element.all(by.repeater('(key, promotion) in promotions | reverse | filter:searchPromotionText | filter:{Category: selectVal}'));
        for (var i = 0; i < promotionsElm.length; i++) {
            expect(promotionsElm[i].Category).toEqual(1);
        }
    });

});