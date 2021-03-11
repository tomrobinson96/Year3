/**
 * Created by Razvan Dinita on 26/09/2016.
 */

app.controller (
	'DashboardController', [
		'$scope', function ( $scope )
		{

			$scope.DisplayName = function ()
			{
				alert ( $scope.name );
			}

		}
	]
);
