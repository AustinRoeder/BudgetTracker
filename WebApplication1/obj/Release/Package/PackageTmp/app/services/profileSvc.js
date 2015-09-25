'use strict';
angular.module('budget_tracker').factory('profileSvc', ['authSvc', '$state', '$http', function (authSvc, $state, $http) {

    var factory = {};
    
    factory.getUser = function () {
        return $http.post('/api/Account/GetUser').then(function (response) {
            return response.data;
        })
    }
    factory.editProfile = function (model) {
        return $http.post('/api/Account/ChangePassword', model).then(function (response) {
            return response.data;
        })
    }

    return factory;
}])