
var submitbutton=document.getElementById("submit");
submitbutton.addEventListener("click", function (event){
	event.preventDefault();
	var fname=document.getElementById("firstname").value;
	var lname=document.getElementById("lastname").value;
	var age=document.getElementById("age").value;
	var cals= age*5;
alert("Thank you" +" "+ fname +" "+ lname+" "+ "for registering." +" "+ "Your number is"+" "+cals);


})


