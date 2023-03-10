//Clientside gamelogic for Battleship

var cells = document.getElementsByClassName("attack");


function init() {
  placeBoats();
}

//Shoot on click
for (var i = 0; i < cells.length; i++) {
  //Add eventlistener to all cells
  cells[i].addEventListener("click", function () {
    shoot(this.id);
  });
}

//Shoot function
function shoot(cellID) {
  var cell = document.getElementById(cellID);

  
  
  console.log(cellID);
}



//boat object
function Boat(name, length, orientation) {
  this.name = name;
  this.length = length;
  this.orientation = orientation;
  this.cells = [];

  //Create cells
  var startCell = getRandomCell();

  //Check if cell is already taken


  for (var i = 0; i < length; i++) {
    var cell = startCell;

    //change the last number of the cell + 1
    var cellArray = cell.split("-");
    var x = cellArray[0];
    var y = cellArray[1];
    x++;
  }
}

//Create boats
var Carrier = new Boat("Carrier", 5, "horizontal");
var Battleship = new Boat("Battleship", 4, "horizontal");
var Cruiser = new Boat("Cruiser", 3, "vertical");
var Submarine = new Boat("Submarine", 3, "horizontal");
var Destroyer = new Boat("Destroyer", 2, "horizontal");

//Create array of boats
var boats = [Carrier, Battleship, Cruiser, Submarine, Destroyer];

//Returns random cell id between 0 and 10
function getRandomCell() {
  var x = Math.floor(Math.random() * 10);
  var y = Math.floor(Math.random() * 10);
  var cell = x + "-" + y;
  return cell;
}