'use strict';
angular.module('budget_tracker').controller('budgetListCtrl', ['budgetSvc1', '$state', 'budgets',  function (budgetSvc1, $state, budgets) {
    var self = this;

    this.budgetExpenses = budgets.expenses;
    this.budgetIncomes = budgets.incomes;
}])