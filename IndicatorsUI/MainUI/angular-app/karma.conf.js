// Karma configuration file, see link for more information
// https://karma-runner.github.io/0.13/config/configuration-file.html

module.exports = function (config) {
    config.set({
        basePath: '',
        frameworks: ['jasmine', '@angular-devkit/build-angular'],
        plugins: [
            require('karma-jasmine'),
            require('karma-chrome-launcher'),
            require('karma-remap-istanbul'),
            require('karma-junit-reporter'),
            require('@angular-devkit/build-angular/plugins/karma'),
        ],
        files: [
            
        ],
        preprocessors: {
            
        },
        mime: {
            'text/x-typescript': ['ts', 'tsx']
        },
        remapIstanbulReporter: {
            dir: require('path').join(__dirname, 'coverage'), reports: {
                html: 'coverage',
                lcovonly: './coverage/coverage.lcov'
            }
        },
        junitReporter: {
            outputDir: './',
            outputFile: 'AngularTest-results.xml'
        },
        angularCli: {
            config: './angular-cli-test.json',
            environment: 'dev'
        },
        customLaunchers: {
            ChromeHeadless: {
                base: 'Chrome',
                flags: [
                    '--headless',
                    '--disable-gpu',
                    // Without a remote debugging port, Google Chrome exits immediately.
                    '--remote-debugging-port=9222',
                ],
            }
        },
        reporters: config.angularCli && config.angularCli.codeCoverage ? ['progress', 'karma-remap-istanbul', 'junit'] : ['progress', 'junit'],
        port: 9876,
        colors: true,
        logLevel: config.LOG_INFO,
        autoWatch: true,
        browsers: ['ChromeHeadless'],
        singleRun: true
    });
};