'use strict';
angular.module('budget_tracker').controller('budgetCtrl', ['budgetSvc1', 'categorySvc', '$state',  function (budgetSvc1, categorySvc, $state) {
    var self = this;
    this.categories = {};
    this.managePanel = 'c';
    this.model = {
    };
    this.beginNew = function () {
        self.managePanel = 'c';
        self.model = {};
    }
    this.beginEdit = function (id) {
        self.getBudgetItem(id).then(function (data) {
            self.managePanel = 'e';
            self.model = data;
        });
    }
    this.getCategories = function () {
        categorySvc.getCategories().then(function (data) {
            self.categories = data;
        });
    }
    self.getCategories()

    this.getBudgetItem = function (id) {
        return budgetSvc1.getBudgetItem(id)
    }
    this.deleteBudgetItem = function (id) {
        budgetSvc1.deleteBudgetItem(id).then(function (data) {
            $state.go($state.current, {}, { reload: true });
        })
    }
    this.editBudgetItem = function (id) {
        budgetSvc1.editBudgetItem(id, self.model).then(function (data) {
            $state.go($state.current, {}, { reload: true });
        })
    }
    this.createBudgetItem = function () {
        budgetSvc1.createBudgetItem(self.model).then(function (data) {
            $state.go($state.current, {}, { reload: true });
        })
    }
}])