/**
 * Created by Razvan Dinita on 26/09/2016.
 */

app.controller (
	'NavigationController', [
		'$scope', function ( $scope )
		{

			// JSON notation
			$scope.navItems = [
				{
					link : '/instructions',
					title: 'Instructions'
				},
				{
					link : '/asteroids',
					title: 'Asteroids'
				},
				{
				  link: '/leaderboard',
				  title: 'Leaderboard'
				}
			];

		}
	]
);
