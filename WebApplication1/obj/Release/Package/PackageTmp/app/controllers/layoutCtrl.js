'use strict';
angular.module('budget_tracker').controller('layoutCtrl', ['householdSvc','authSvc', '$state', function(householdSvc,authSvc, $state) {
    var self = this;
    this.$state = $state;
    
    this.user = {};
    householdSvc.getUser().then(function (data) { self.user = data; });
    this.logout = function () {
        console.log('made it')
        authSvc.logout();
        $state.go('signin.login');
    }
}])