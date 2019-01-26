const admin = require('firebase-admin');
const serviceAccount = require('joyleaf-c142c-firebase-adminsdk-t7cmb-5448723bf8.json');
admin.initializeApp({
    credential: admin.credential.cert(serviceAccount),
    databaseURL: 'https://joyleaf-c142c.firebaseio.com/'
});
const cron = require('node-cron');
const puppeteer = require('puppeteer');

const content = admin.database().ref().child('content');
const temp = admin.database().ref().child('temp');
const error_log = admin.database().ref().child('error_log');

const NEXT = 'i[class="icn-sqdc-arrow-right fa-lg"]';

//order: name, brand, species, strength, thc, type
const SPECIFICATIONS = [
    'h1[property="name"]',
    'div[property="brand"]',
    'div.product-attribute',
    'div.product-attribute:nth-of-type(2)',
    'ul > li > span:nth-child(2)',
    'div.col-sm-6.js-spec-col-1 > p > span'
];

const AROMAS = 'div.col-sm-6.js-spec-col-2 > p:nth-child(4) > span';

//order: id, quantity + availability, price, grams
const OPTIONS = [
    'span.js-spec-sku',
    'button.btn.btn-default.btn-outline.mr-20.active',
    'span[property="price"]',
    'span.js-spec-quota'
]

const OPTIONS_NEXT = 'div.mb-10 > div > button';

const OPTIONS_PARENT = 'div.btn-multiline.bg-white.p-20';

cron.schedule("*/15 * * * *", function() {

    var URL = 'https://www.sqdc.ca/en-CA/Search?keywords=*&sortDirection=asc&page=1';

    puppeteer.launch({ args: ['--no-sandbox', '--disable-setuid-sandbox'] })
    .then(async browser => {

        await temp.set({});

        const page = await browser.newPage();

        await page.setUserAgent('Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.39 Safari/537.36');

        await page.evaluateOnNewDocument(() => {

            Object.defineProperty(navigator, 'webdriver', {
                get: () => false,
            });
        });

        await page.evaluateOnNewDocument(() => {

            window.navigator.chrome = {
                runtime: {}
            };
        });

        await page.evaluateOnNewDocument(() => {
            const originalQuery = window.navigator.permissions.query;
            return window.navigator.permissions.query = (parameters) => (
                parameters.name === 'notifications' ?
                Promise.resolve({ state: Notification.permission }) :
                originalQuery(parameters)
            );
        });

        await page.evaluateOnNewDocument(() => {

            Object.defineProperty(navigator, 'plugins', {
              get: () => [1, 2, 3, 4, 5],
            });
        });

        await page.evaluateOnNewDocument(() => {

            Object.defineProperty(navigator, 'languages', {
                get: () => ['en-CA', 'en'],
            });
        });

        await page.setViewport({ width: 1920, height: 1080 });

        await page.setRequestInterception(true);
        page.on('request', (req) => {
            if(req.resourceType() == 'stylesheet' || req.resourceType() == 'font' || req.resourceType() == 'image'){
                req.abort();
            }
            else {
                req.continue();
            }   
        });

        await page.goto(URL, { waitUntil: 'networkidle2' });

        while(true){

            let next = await page.evaluate((NEXT) => {
                let t = document.querySelector(NEXT);
                
                try{
                    return t.parentElement.getAttribute('href');
                }catch(e){
                    return null;
                }
            }, NEXT);
    
            let links = await page.evaluate(() => {
                let t = [];
                let elements = document.getElementsByClassName('product-tile-media image-background');

                for (var element of elements){
                    t.push('https://www.sqdc.ca' + element.getAttribute('href'));
                }
                return t;
            });

            var j, x, i;

            for (i = links.length - 1; i > 0; i--) {
                j = Math.floor(Math.random() * (i + 1));
                x = links[i];
                links[i] = links[j];
                links[j] = x;
            }

            for(var link of links){
                await page.goto(link, { waitUntil: 'networkidle2' });
                
                await page.waitFor('input[id="month"]');

                const month = Math.ceil(Math.random() * 12);
                await page.type('input[id="month"]', '' + month);

                const day = Math.ceil(Math.random() * 25);
                await page.type('input[id="day"]', '' + day);

                const year = Math.ceil(Math.random() * 50 + 1950);
                await page.type('input[id="year"]', '' + year);

                await page.click('button[type="submit"]');

                var item = temp.push();

                let specifications = await page.evaluate((SPECIFICATIONS) => {
                    let t = [];

                    for(var i = 0; i < 6; i++){
                        let property = document.querySelector(SPECIFICATIONS[i]);
                        t.push(property.textContent);
                    }
                    return t;
                }, SPECIFICATIONS);

                specifications.push(link);

                await item.set({
                    name: specifications[0],
                    brand: specifications[1],
                    species: specifications[2],
                    strength: specifications[3],
                    thc: specifications[4],
                    type: specifications[5],
                    link: specifications[6]
                });

                let aromas = await page.evaluate((AROMAS) => {
                    let property = document.querySelector(AROMAS);
                    return property.textContent.split(/,?\s+/);
                }, AROMAS);

                for(var i = 0; i < aromas.length; i++){

                    await item.child('aromas').update({
                        ['aroma' + i]: aromas[i]
                    });
                }

                let n = await page.evaluate((OPTIONS_PARENT) => {
                    let childCount = document.querySelector(OPTIONS_PARENT).childNodes.length;
                    return ((Math.floor(childCount/2))-1);
                }, OPTIONS_PARENT);

                let options = [];

                for(var i = 1; i <= n; i++){

                    let option = await page.evaluate((i, OPTIONS) => {
                        let t = [];

                        for(var j = 0; j < 4; j++){
                            let property = document.querySelector(OPTIONS[j]);
                                
                            if(j == 1){
                                t.push((property.textContent.replace('\n', '')).trim());

                                if(('' + property.getAttribute('class')).search('unavailable') != -1){
                                    t.push('0');
                                }else{
                                    t.push('1');
                                }
                            }else{
                                t.push(property.textContent);
                            }
                        }
                        return t;
                    }, i, OPTIONS);

                    options.push(option);

                    var btn = OPTIONS_NEXT + ':nth-child(' + (i+2) + ')';

                    if (await page.$(btn) != null) {
                        await page.click(btn);

                        while (await page.$(OPTIONS[0]) == null || await page.$eval(OPTIONS[0], el => el.textContent) == option[0]){
                            await page.waitFor(1000);
                        }
                    }
                }

                for(var i = 0; i < options.length; i++){

                    await item.child('options').child('option' + i).update({
                        id: options[i][0],
                        size: options[i][1],
                        availability: options[i][2],
                        price: options[i][3],
                        grams: options[i][4]
                    });
                }
            ;}

            if(next != null){
                await page.goto('https://www.sqdc.ca' + next, { waitUntil: 'networkidle2' });
            }else{
                await browser.close();

                var d = new Date();
                var timestamp = '' + (d.getMonth()+1) + '/' + d.getDate() + '/' + d.getFullYear() + '-' + d.getHours() + ':' + d.getMinutes() + ':' + d.getSeconds();

                await temp.once('value', function(snap) {

                    content.set(snap.val(), function() {
                        temp.remove();
                    });
                });

                await content.update({
                    ['!timestamp']: timestamp
                });

                process.exit();
            }
        }
    })
    .catch(async function(e) {
        await temp.set({});

        var d = new Date();
        var timestamp = '' + (d.getMonth()+1) + '/' + d.getDate() + '/' + d.getFullYear() + '-' + d.getHours() + ':' + d.getMinutes() + ':' + d.getSeconds();
        await error_log.push().set('' + timestamp + '---' + e.stack);
        
        process.exit();
    });
});