var ngSimchaApp = angular.module('ngSimchaApp', ['ngAnimate']);




ngSimchaApp.directive('bsPopover', function () {
    return function (scope, element, attrs) {
        
       var htmlContent = "<div>";
        htmlContent += "<ul>";
        if (attrs.type == "TimeSlots") {
            tjq.each(scope.x.timeslots, function (index, value) {
                htmlContent += "<li>" + value + "</li>";
            });
        }
        else if (attrs.type == "Events") {
            tjq.each(scope.x.EventTypes, function (index, value) {
              
                htmlContent += "<li>" + value + "</li>";
            });

        }
        else if (attrs.type == "Locations") {
            

            tjq.each(scope.x.Locations, function (index, value) {
                htmlContent += "<li>" + value + "</li>";
            });

        }
        htmlContent += "</ul>";
        htmlContent += "</div>";
      
        element.popover({ placement: 'top', html: 'true', trigger: 'hover', content: htmlContent });
    };
});