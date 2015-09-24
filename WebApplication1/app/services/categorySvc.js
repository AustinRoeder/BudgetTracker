'use strict';
angular.module('budget_tracker').factory('categorySvc', ['authSvc', '$state', '$http', function (authSvc, $state, $http) {
    var factory = {};
    factory.getCategories = function () {
        return $http.post('/api/Categories').then(function (response) {
            return response.data;
        });
    }
    return factory;
}])