

ngSimchaApp.controller('ngSearchController', function ($scope,$http) {

    $scope.resultscount = 0;
 
    $scope.search = function() {
        toastr.info('Fetching Search Results. Please Wait.');
       
        var url = '/api/webapi/GetBookingsWithStartDateAndEndDate';
        $http.post(url,
         $scope.postdata).
      success(function (data, status, headers, config) {
          $scope.data = data;
          console.log(data);
          var count = data.length;
                $scope.resultscount = count;
                toastr.success('Successfully Loaded '+ count +' result(s)');


            }).
      error(function (data, status, headers, config) {
            toastr.error(data);

        });
    }


    tjq(document).ready(function() {
        $scope.search();

    });

   

});


