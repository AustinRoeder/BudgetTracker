'use strict';
angular.module('budget_tracker').controller('accountDetailsCtrl', ['accountSvc','transactionSvc','$state', '$stateParams', 'account','categories', function (accountSvc, transactionSvc, $state, $stateParams, account, categories) {
    var self = this;
    this.categories = categories;
    this.display = account;
    this.id = $stateParams.id;
    this.model = {
        AccountId: account.Id,
        IsReconciled: '',
    };
    this.managePanel = 'c';
    this.createTrans = function () {
        self.model.IsReconciled = !self.model.IsReconciled;
        transactionSvc.createTransaction(self.model).then(function (data) {
            $state.go($state.current, {}, {reload : true})
        })
    }
    this.beginNew = function () {
        self.model = {
            AccountId: account.Id,
            IsReconciled: '',
        }
        self.managePanel = 'c';
    }
    this.beginEdit = function (id) {
        transactionSvc.getTransaction(id).then(function (data) {
            data.IsReconciled = !data.IsReconciled;
            if (data.Amount < 0)
                data.Amount *= -1
            self.model = data;
            self.managePanel = 'e'
        })
    }
    this.editTrans = function (id) {
        transactionSvc.editTransaction(id, self.model).then(function (data) {
            $state.go($state.current, {}, { reload: true })
        })
    }
    this.deleteTrans = function (id) {
        transactionSvc.deleteTransaction(id).then(function (data) {
            $state.go($state.current, {}, { reload: true })
        })
    }
}])