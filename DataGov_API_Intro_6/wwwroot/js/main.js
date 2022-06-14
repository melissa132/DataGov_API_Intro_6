/**
 * Your JS code here
 */

/**
 * Sample code
 */
document.addEventListener('DOMContentLoaded', function() {
  var ulElement = document.getElementById('links');
  var liElement = document.createElement('li');
  liElement.appendChild(document.createTextNode('created by main.js'));

  ulElement.appendChild(liElement);
});


var ctx = document.getElementById("myChart2").getContext("2d");
var xValues = ["White", "African American", "Asian", "Two or More Races", "Some Other Race", "American Indian Alaskan Native", "Native Hawaiian and Other Pacific Islander"];
var yValues = [65.41, 23.62, 4.31, 3.86, 2.46, 0.28, 0.07];
var barColors = ["red", "green", "blue", "orange", "brown", "gold", "black", "skyblue", "darkgreen"];
var mychart = new Chart(ctx, {
    type: "doughnut",
    data: {
        labels: xValues,
        datasets: [{
            backgroundColor: barColors,
            data: yValues
        }]
    },
    options: {
        legend: { display: true },
        title: {
            display: true,
            text: "Population by Ethinicity"
        }
    }
})


var ctx1 = document.getElementById("myChart").getContext("2d");
var xValues = ["0-9", "10-19", "20-29", "30-39", "40-49", "50-59", "60-69", "70-79", "80+"];
var yValues = [12, 13, 16, 15, 13, 13, 10, 5, 3];
var barColors = ["red", "green", "blue", "orange", "brown", "gold", "black", "skyblue", "darkgreen"];
var mychartbar = new Chart(ctx1, {
    type: "bar",
    data: {
        labels: xValues,
        datasets: [{
            backgroundColor: barColors,
            data: yValues
        }]
    },
    options: {
        legend: { display: true },
        title: {
            display: false,
            text: "Population by Age Group"
        }
    }
})


