/**
 * Created by Razvan Dinita on 26/09/2016.
 * Edited by 1409046
 */

var app = angular.module ( 'MobileTech', [ 'ngRoute', 'ngStorage' ] );

app.config (
	[
		'$routeProvider',
		function ( $routeProvider )
		{

			// DEFAULT APP PAGE
			var defaultRoute = '/instructions';

			// ROUTE CONFIGURATION - WHEN USER ACCESSES THE FOLLOWING LINKS, LOAD THE APPROPRIATE VIEWS / PAGES
			$routeProvider
				.when (
					'/instructions', {
						templateUrl: 'views/instructions.html'
					}
				)
				.when (
					'/asteroids', {
						templateUrl: 'views/asteroids.html'
					}
				)
				.when (
					'/leaderboard', {
						templateUrl: 'views/leaderboard.html'
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
