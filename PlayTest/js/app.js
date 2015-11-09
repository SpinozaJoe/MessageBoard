// app.js

(function ()
{
    var app = angular.module('nurseryApp', []);

    app.controller('AppController', function ()
    {
        var now = new Date(Date.now());

        this.currentYear = now.getFullYear();
    });

    app.directive('pageHeader', function ()
    {
        return {
            // E stands for Element (A is for Attribute)
            restrict: 'E',
            templateUrl: 'templates/page-header.html'
        };
    });

    app.directive('pageFooter', function ()
    {
        return {
            // E stands for Element (A is for Attribute)
            restrict: 'E',
            templateUrl: 'templates/page-footer.html'
        };
    });

    app.controller('FormController', ['$http', function ($http)
    {
        var contact = {
            childName: '',
            childBirthDate: null,
            parentName: '',
            address: '',
            email: '',
            homePhone: '',
            mobilePhone: '',
            comments: ''
        };
        
        this.contact = contact;

        this.submit = function ()
        {
            var serviceUrl = 'http://localhost/Playground/WebsiteService/api/email/';

//            serviceUrl = 'http://daveltest.azurewebsites.net/api/email';
            
            $http.post(serviceUrl, {
                from: 'dd@set',
                to: this.email,
                subject: 'East Craigs Playgroup request',
                body: 'Please accept ' + this.childName

            }).then(function (result) {
                alert('woo' + result)
            },
            function () {
                // error
                alert('boo')
            });
            /*

            $.ajax(serviceUrl, {
                method: 'POST',
                data: {
                    from: 'dd@set',
                    to: this.email,
                    subject: 'East Craigs Playgroup request',
                    body: 'Please accept ' + this.childName

                },
                xhrFields: {
                    // The 'xhrFields' property sets additional fields on the XMLHttpRequest.
                    // This can be used to set the 'withCredentials' property.
                    // Set the value to 'true' if you'd like to pass cookies to the server.
                    // If this is enabled, your server must respond with the header
                    // 'Access-Control-Allow-Credentials: true'.
                    withCredentials: false
                }
            }).done(function(msg) {
                alert('woo' + msg)
            }).fail(function () {
                alert('boo')
            });
            */
//            alert('Eventually this will do something for ' + this.childName);
        }
    }]);

})();
