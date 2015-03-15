/*
Left-Right image slideshow Script-
By Dynamic Drive (www.dynamicdrive.com)
For full source code, terms of use, and 100's more scripts, visit http://dynamicdrive.com
*/

///////configure the below four variables to change the style of the slider///////
//set the scrollerwidth and scrollerheight to the width/height of the LARGEST image in your slideshow!
var scrollerwidth='935px'
var scrollerheight='340px'
var scrollerbgcolor='white'
//3000 miliseconds=3 seconds
var pausebetweenimages=1000


//configure the below variable to change the images used in the slideshow. If you wish the images to be clickable, simply wrap the images with the appropriate <a> tag
var slideimages=new Array()
slideimages[0]=''
slideimages[1]=''
slideimages[2]=''
//extend this list

///////Do not edit pass this line///////////////////////
     
var ie=document.all
var dom=document.getElementById

if (slideimages.length>1)
i=2
else
i=0

function move1(whichlayer){
tlayer=eval(whichlayer)
if (tlayer.left>0&&tlayer.left<=5){
tlayer.left=0
setTimeout("move1(tlayer)",pausebetweenimages)
setTimeout("move2(document.main.document.second)",pausebetweenimages)
return
}
if (tlayer.left>=tlayer.document.width*-1){
tlayer.left-=5
setTimeout("move1(tlayer)",15)
}
else{
tlayer.left=parseInt(scrollerwidth)+5
tlayer.document.write(slideimages[i])
tlayer.document.close()
if (i==slideimages.length-1)
i=0
else
i++
}
}

function move2(whichlayer){
tlayer2=eval(whichlayer)
if (tlayer2.left>0&&tlayer2.left<=5){
tlayer2.left=0
setTimeout("move2(tlayer2)",pausebetweenimages)
setTimeout("move1(document.main.document.first)",pausebetweenimages)
return
}
if (tlayer2.left>=tlayer2.document.width*-1){
tlayer2.left-=5
setTimeout("move2(tlayer2)",15)
}
else{
tlayer2.left=parseInt(scrollerwidth)+5
tlayer2.document.write(slideimages[i])
tlayer2.document.close()
if (i==slideimages.length-1)
i=0
else
i++
}
}

function move3(whichdiv){
tdiv=eval(whichdiv)
if (parseInt(tdiv.style.left)>0&&parseInt(tdiv.style.left)<=5){
tdiv.style.left=0+"px"
setTimeout("move3(tdiv)",pausebetweenimages)
setTimeout("move4(scrollerdiv2)",pausebetweenimages)
return
}
if (parseInt(tdiv.style.left)>=tdiv.offsetWidth*-1){
tdiv.style.left=parseInt(tdiv.style.left)-5+"px"
setTimeout("move3(tdiv)",15)
change_images(i)
}
else{
	
tdiv.style.left=scrollerwidth
tdiv.innerHTML=slideimages[i]
if (i==slideimages.length-1)
i=0
else
i++
}
}

function move4(whichdiv){
tdiv2=eval(whichdiv)
if (parseInt(tdiv2.style.left)>0&&parseInt(tdiv2.style.left)<=5){
tdiv2.style.left=0+"px"
setTimeout("move4(tdiv2)",pausebetweenimages)
setTimeout("move3(scrollerdiv1)",pausebetweenimages)
return
}
if (parseInt(tdiv2.style.left)>=tdiv2.offsetWidth*-1){
tdiv2.style.left=parseInt(tdiv2.style.left)-5+"px"
setTimeout("move4(scrollerdiv2)",15)
change_images(i)
}
else{
	
tdiv2.style.left=scrollerwidth
tdiv2.innerHTML=slideimages[i]
if (i==slideimages.length-1)
i=0
else
i++
}
}

function startscroll(){
if (ie||dom){
scrollerdiv1=ie? first2 : document.getElementById("first2")
scrollerdiv2=ie? second2 : document.getElementById("second2")
move3(scrollerdiv1)
scrollerdiv2.style.left=scrollerwidth
}
else if (document.layers){
document.main.visibility='show'
move1(document.main.document.first)
document.main.document.second.left=parseInt(scrollerwidth)+5
document.main.document.second.visibility='show'
}
}


function changescroll(){
if (ie||dom){
scrollerdiv1=ie? first2 : document.getElementById("first2")
scrollerdiv2=ie? second2 : document.getElementById("second2")
move3(scrollerdiv1)
scrollerdiv2.style.left=scrollerwidth
}
else if (document.layers){
document.main.visibility='show'
move1(document.main.document.first)
document.main.document.second.left=parseInt(scrollerwidth)+5
document.main.document.second.visibility='show'
}
}


function change_images(theimage)
{
	document.getElementById('img0').innerHTML = '<img src="images/hp_offimg.gif">'
	document.getElementById('img1').innerHTML = '<img src="images/hp_offimg.gif">'
	document.getElementById('img2').innerHTML = '<img src="images/hp_offimg.gif">'
	
	//alert(theimage)
	
	document.getElementById('img'+theimage).innerHTML = '<img src="images/hp_onimg.gif">'
}



//window.onload=startscroll