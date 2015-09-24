'use strict';
angular.module('budget_tracker').controller('layoutCtrl', ['householdSvc', '$state', function(householdSvc, $state) {
    var self = this;
    this.$state = $state;
    
    this.user = {};
    householdSvc.getUser().then(function (data) { self.user = data; });
}])