/**
 * Created by Razvan Dinita on 26/09/2016.
 */

app.controller(
  'instructionsController', [
    '$scope', '$localStorage',
    function($scope, $localStorage) {

      $scope.DisplayName = function() {
        // construct the name object
        var
          nameObject = {
            playerName: $scope.name
          };

        // record the name in localstorage
        // add score object to the end of the array
        $localStorage.scoresFull.push(nameObject);
        
        alert ( $scope.name );
      }

    }
  ]
);