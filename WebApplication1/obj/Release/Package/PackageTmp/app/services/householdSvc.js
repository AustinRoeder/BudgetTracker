'use strict';
angular.module('budget_tracker').factory('householdSvc', ['authSvc', '$state', '$http', function (authSvc, $state, $http) {

    var factory = {};

    factory.getUser = function () {
        return $http.post('/api/Account/GetUser').then(function (response) {
            return response.data;
        })
    }

    factory.getHousehold = function () {
        return $http.post('/api/Account/Household').then(function (response) {
            return response.data;
        });
    }
    factory.joinHousehold = function (inviteEmail, inviteCode) {
        return $http.post('/api/Account/JoinHousehold?inviteEmail=' + inviteEmail + '&inviteCode=' + inviteCode)
    }
    factory.createHousehold = function (houseName) {
        return $http.post('/api/Account/CreateHousehold?name=' + houseName)
    }
    factory.leaveHousehold = function (understood) {
        if (understood) {
            return $http.post('/api/Account/LeaveHousehold')
        }
    }
    factory.sendInvite = function (email) {
        return $http.post('/api/Account/SendInvite?email=' + email)
    }
    return factory;
}])