'use strict';
angular.module('budget_tracker').factory('accountSvc', ['authSvc', '$state', '$http', function (authSvc, $state, $http) {

    var factory = {};
    factory.getAccounts = function () {
        return $http.post('/api/Accounts/HouseAccounts').then(function (response) {
            return response.data;
        });
    }
    factory.getAccount = function (id) {
        return $http.post('/api/Accounts/Find?id=' + id).then(function (response) {
            return response.data;
        })
    }
    factory.archiveAccount = function (id) {
        return $http.post('/api/Accounts/Archive?id=' + id);
    }
    factory.editAccount = function (id, model) {
        return $http.post('/api/Accounts/Edit?id='+id, model);
    }
    factory.createAccount = function (model) {
        return $http.post('/api/Accounts/Create', model);
    }
    return factory;
}])