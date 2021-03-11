/**
 * Created by Razvan Dinita on 26/09/2016.
 */

var app = angular.module ( 'MobileTech', [ 'ngRoute' ] );

app.config (
	[
		'$routeProvider',
		function ( $routeProvider )
		{

			// DEFAULT APP PAGE
			var defaultRoute = '/dashboard';

			// ROUTE CONFIGURATION - WHEN USER ACCESSES THE FOLLOWING LINKS, LOAD THE APPROPRIATE VIEWS / PAGES
			$routeProvider
				.when (
					'/dashboard', {
						templateUrl: 'views/dashboard.html'
					}
				)
				.when (
					'/login', {
						templateUrl: 'views/login.html'
					}
				)
				// IF THE USER REQUESTS AN UNRECOGNISED LINK, REDIRECT THEM TO THE DEFAULT ONE
				.otherwise (
					{
						redirectTo: defaultRoute
					}
				);
		}
	]
);
