'use strict';
angular.module('budget_tracker').factory('transactionSvc', ['authSvc', '$state', '$http', function (authSvc, $state, $http) {
    var factory = {};
    factory.recent = function () {
        return $http.post('/api/Transactions/RecentTransactions').then(function (response) {
            return response.data;
        })
    }
    factory.getAccountTransactions = function (accountId) {
       return $http.post('/api/Transactions/AccountTransactions?aId='+ accountId).then(function (response) {
            return response.data;
        });
    }
    factory.getTransaction = function (id) {
        return $http.post('/api/Transactions/Find?id='+ id).then(function (response) {
            return response.data;
        });
    }
    factory.getTransactionByCategory = function (category) {
        return $http.post('/api/Transactions/FindByCategory', category).then(function (response) {
            return response.data;
        });
    }
    factory.editTransaction = function (id, model) {
        return $http.post('/api/Transactions/Edit?id=' + id, model).then(function (response) {
            return response.data;
        });
    }
    factory.createTransaction = function (transaction) {
        //transaction.CategoryId = transaction.Category.Id;
        return $http.post('/api/Transactions/Create', transaction).then(function (response) {
            return response.data;
        });
    }
    factory.deleteTransaction = function (id) {
        return $http.post('/api/Transactions/Delete?id='+ id).then(function (response) {
            return response.data;
        });
    }
    return factory;
}])