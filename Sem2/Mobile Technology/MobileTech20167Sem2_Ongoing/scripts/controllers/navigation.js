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
					link : '/dashboard',
					title: 'Dashboard'
				},
				{
					link : '/login',
					title: 'Login'
				}
			];

		}
	]
);
