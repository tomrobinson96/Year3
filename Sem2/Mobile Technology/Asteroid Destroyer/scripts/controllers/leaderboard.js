/**
 * Created by Razvan Dinita on 26/09/2016.
 */

app.controller (
	'LeaderboardController', [
		'$scope', '$localStorage', function ( $scope, $localStorage )
		{
		  
		  // initialise scores array if not there
		  if (!$localStorage.scoresFull) {
		    $localStorage.scoresFull = [ ];
		  }
		  
		  // share the scores array with the view
		  this.scoresFull = $localStorage.scoresFull;
      
      this.ButtonClicked = function () {
        angular.element("table").each(function() {
          angular.element(this).toggle();
        });
      };
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
		}
	]
);
