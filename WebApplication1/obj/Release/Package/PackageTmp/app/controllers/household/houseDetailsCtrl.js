'use strict';
angular.module('budget_tracker').controller('houseDetailsCtrl', ['householdSvc', '$state', 'household', function (houseSvc, $state, household) {
    var self = this;
    
    this.display = household;
    this.alerts = [{
        type: 'success',
        msg: 'Invitation Sent'
    }]
    this.closeAlert = function (index) {
        this.alerts.splice(index, 1);
    };
}])