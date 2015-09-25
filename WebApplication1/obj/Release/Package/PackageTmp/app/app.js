(function () {
    var app = angular.module('budget_tracker', ['ui.router', 'uiSwitch', 'LocalStorageModule', 'ui.bootstrap', 'nvd3', 'trNgGrid']);

    app.config(function ($stateProvider, $urlRouterProvider) {
        //
        // For any unmatched url, redirect to /state1
        $urlRouterProvider.otherwise("/login");
        //
        // Now set up the states
        $stateProvider
            .state('dashboard', {
                url: "/dashboard",
                templateUrl: "/app/templates/dashboard.html",
                controller: "dashboardCtrl as dash",
                data: {
                    requiresHousehold: true
                }
            })
            .state('profile', {
                url: "/profile",
                templateUrl: "/app/templates/profile.html",
                controller: "profileCtrl as profile"
            })
          .state('signin', {
              url: "",
              templateUrl: "/app/templates/signin.html",
              abstract: true,
              controller: "signinCtrl as signin"
          })
            .state('signin.login', {
                url: "/login",
                templateUrl: "/app/templates/login.html"
            })
            .state('signin.register', {
                url: "/register",
                templateUrl: "/app/templates/register.html"
            })
            // Household States
          .state('household', {
              url: "/household",
              templateUrl: "/app/templates/household/household.html",
              abstract: true,
              controller: "houseCtrl as house"
          })
            .state('household.details', {
                url: "",
                templateUrl: "/app/templates/household/household.details.html",
                resolve: {
                    household: function (householdSvc) {
                        return householdSvc.getHousehold();
                    }
                },
                controller: "houseDetailsCtrl as houseDetails"
            })
            .state('household.create', {
                url: "/create",
                templateUrl: "/app/templates/household/household.create.html",
                controller: "houseCreateCtrl as houseCreate"
            })
            .state('household.join', {
                url: "/join",
                templateUrl: "/app/templates/household/household.join.html",
                controller: "houseJoinCtrl as houseJoin"
            })

            // Account States
          .state('accounts', {
              url: "/accounts",
              templateUrl: "/app/templates/accounts/accounts.html",
              abstract: true,
              controller: "accountCtrl as account"
          })
            .state('accounts.list', {
                url: '',
                templateUrl: '/app/templates/accounts/accounts.list.html',
                resolve: {
                    accounts: function (accountSvc) {
                        return accountSvc.getAccounts();
                    }
                },
                controller: 'accountListCtrl as accountList'
            })
            .state('accounts.details', {
                url: '/details/:id',
                templateUrl: '/app/templates/accounts/accounts.details.html',
                controller: 'accountDetailsCtrl as accountDetails',
                resolve: {
                    account: ['accountSvc', '$stateParams', function (accountSvc, $stateParams) {
                        return accountSvc.getAccount($stateParams.id);
                    }],
                    categories: function (categorySvc) {
                        return categorySvc.getCategories();
                    }
                }
            })
        // Budget States
            .state('budget', {
                url: '/budget',
                templateUrl: '/app/templates/budget/budget.html',
                controller: 'budgetCtrl as budget',
            })
            .state('budget.list', {
                url: '/list',
                templateUrl: '/app/templates/budget/budget.list.html',
                resolve: {
                    budgets: function (budgetSvc1) {
                        return budgetSvc1.getBudgetItems();
                    }
                },
                controller: 'budgetListCtrl as budgetList'
            });
    });
    var serviceBase = 'https://aroeder-budget.azurewebsites.net/';
    //var serviceBase = 'http://localhost:64752/';
    app.constant('ngAuthSettings', {
        apiServiceBaseUri: serviceBase
    });

    app.config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptorSvc');
    });

    app.run(['authSvc', '$rootScope', '$state', '$stateParams', function (authService, $rootScope, $state, $stateParams) {
        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;
        authService.fillAuthData();

        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
            if (toState.data && toState.data.requiresHousehold === true) {
                if (!authService.authentication.isAuth) {
                    $state.go('signin.login');
                }
                if (authService.authentication.householdId == null ||
                    authService.authentication.householdId == "") {
                    console.log(authService.authentication);
                    event.preventDefault()
                    $state.go('household.create');
                }
            }
        })
    }]);
})();