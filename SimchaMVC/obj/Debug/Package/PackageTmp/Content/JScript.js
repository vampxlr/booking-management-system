/*****************************************************************************
* Function Name		: CompareDates
* Author			: MKumar
* Date				: 20th Feb 2006
* Purpose			: Compare two dates
compareInd  0: Less than	Depricated
compareInd  1: Less than equals to	 Depricated

* 03 May 2006		Akhilesh Sharma		Modified Compare date function
If return value is -1 : First date is smaller then second
If return value is 0  : Dates are equal
If return value is 1  : First date is greater then second
*****************************************************************************/
//function CompareDates(date1, date2, compareInd)
function CompareDates(date1, date2) {
    var followUp = new Date();
    var assigned = new Date();
    var Date1Month = new Date();
    var Date2Month = new Date();
    var Date1Year = new Date();
    var Date2Year = new Date();

    var Date1Date = new Date(date1);
    Date1 = Date1Date.getDate();
    Date1Month = Date1Date.getMonth();
    Date1Year = Date1Date.getFullYear();


    var Date2Date = new Date(date2);
    Date2 = Date2Date.getDate();
    Date2Month = Date2Date.getMonth();
    Date2Year = Date2Date.getFullYear();

    //alert(Date1);
    //alert(Date2);

    if ((Date1Year > Date2Year) || ((Date1Year == Date2Year) && (Date1Month > Date2Month)) || ((Date1Year == Date2Year) && (Date1Month == Date2Month) && (Date1 > Date2))) {
        return 1;
    }
    else if ((Date1Year > Date2Year) || ((Date1Year == Date2Year) && (Date1Month > Date2Month)) || ((Date1Year == Date2Year) && (Date1Month == Date2Month) && (Date1 == Date2))) {
        return 0;
    }
    else {
        return -1;
    }

    /*
    if (compareInd == 0)
    {
    if ((Date1Year < Date2Year)||(Date1Month < Date2Month)||(Date1< Date2))
    {return false;}
    }
    else if (compareInd == 1)
    {
    if ((Date1Year <= Date2Year)||(Date1Month <= Date2Month)||(Date1<= Date2))
    {return false;}
    }
    */
}


function fnSetDate(sDateID, hdnStartDateID, eDateID, hdnEndDateID)
{
    document.getElementById(sDateID).value = trimStr(document.getElementById(sDateID).value);
    document.getElementById(eDateID).value = trimStr(document.getElementById(eDateID).value);
    
    var errMsg = "";
    if (document.getElementById(sDateID).value == "")
    {
        errMsg = '\nSelect the Event Date.';
        document.getElementById(sDateID).focus();
    }
    if (document.getElementById(eDateID).value == "")
    {
        errMsg += '\nSelect the Range Date.';
        document.getElementById(eDateID).focus();
    }
    if (CompareDates(document.getElementById(sDateID).value,document.getElementById(eDateID).value) == 1)
    {
        errMsg += '\nEvent date must be less than range date.';
    }
    
    if (errMsg != "")
    {
        alert('\n Please fill the following info :'+errMsg);
        return false;
    }
    document.getElementById(hdnStartDateID).value = document.getElementById(sDateID).value;
    document.getElementById(hdnEndDateID).value = document.getElementById(eDateID).value;  
    
    return true;
}

function fnMoveViewDetail(hallID,dt) {
    
    window.open("ViewDetails.aspx?id="+hallID+"&date=" + dt);
    return false;
}

function fnMoveBook(hallID, dt, SlotId) {

    window.open("Book.aspx?id=" + hallID + "&date=" + dt + "&SlotId=" + SlotId, '_self');
    return false;
}

function fnValidateAccount(txtEmailID, txtPassID, txtUserID, txtFirstNameID, txtLastNameID, txtAdd1ID, txtAdd2ID, txtCityID, txtStateID, txtZipID, txtPhoneID, txtFaxID, chk1ID)
{
    document.getElementById(txtEmailID).value = trimStr(document.getElementById(txtEmailID).value)
    document.getElementById(txtPassID).value = trimStr(document.getElementById(txtPassID).value)
    document.getElementById(txtUserID).value = trimStr(document.getElementById(txtUserID).value)
    document.getElementById(txtFirstNameID).value = trimStr(document.getElementById(txtFirstNameID).value)
    document.getElementById(txtLastNameID).value = trimStr(document.getElementById(txtLastNameID).value)
    document.getElementById(txtAdd1ID).value = trimStr(document.getElementById(txtAdd1ID).value)
    document.getElementById(txtAdd2ID).value = trimStr(document.getElementById(txtAdd2ID).value)
    document.getElementById(txtCityID).value = trimStr(document.getElementById(txtCityID).value)
    document.getElementById(txtStateID).value = trimStr(document.getElementById(txtStateID).value)
    document.getElementById(txtZipID).value = trimStr(document.getElementById(txtZipID).value)
    document.getElementById(txtPhoneID).value = trimStr(document.getElementById(txtPhoneID).value)
    document.getElementById(txtFaxID).value = trimStr(document.getElementById(txtFaxID).value)
    
    var errMsg = "";
    
    if (document.getElementById(txtEmailID).value == "")
    {
        errMsg = "\n 1) Enter Email-ID.";
        document.getElementById(txtEmailID).focus();
    }
    if (!validateEmail(document.getElementById(txtEmailID).value))
    {
        errMsg = "\n 2) Enter correct Email-ID.";
        document.getElementById(txtEmailID).focus();
    }
    if (document.getElementById(txtPassID).value == "")
    {
        errMsg += "\n 3) Enter Password.";
        document.getElementById(txtPassID).focus();
    }
    if (document.getElementById(txtUserID).value == "")
    {
        errMsg += "\n 4) Enter User Name.";
        document.getElementById(txtUserID).focus();
    }
    if (document.getElementById(txtFirstNameID).value == "")
    {
        errMsg += "\n 5) Enter First Name.";
        document.getElementById(txtFirstNameID).focus();
    }
    if (document.getElementById(txtLastNameID).value == "")
    {
        errMsg += "\n 6) Enter Last Name.";
        document.getElementById(txtLastNameID).focus();
    }
    if (document.getElementById(txtAdd1ID).value == "")
    {
        errMsg += "\n 7) Enter Primay Address.";
        document.getElementById(txtAdd1ID).focus();
    }
    if (document.getElementById(txtAdd2ID).value == "")
    {
        errMsg += "\n 8) Enter Secondary Address.";
        document.getElementById(txtAdd2ID).focus();
    }
    if (document.getElementById(txtCityID).value == "")
    {
        errMsg += "\n 9) Enter City.";
        document.getElementById(txtCityID).focus();
    }
    if (document.getElementById(txtStateID).value == "")
    {
        errMsg += "\n 10) Enter State.";
        document.getElementById(txtStateID).focus();
    }
    if (document.getElementById(txtZipID).value == "")
    {
        errMsg += "\n 11) Enter Zip-code.";
        document.getElementById(txtZipID).focus();
    }
    if (document.getElementById(txtPhoneID).value == "")
    {
        errMsg += "\n 12) Enter Phone no.";
        document.getElementById(txtPhoneID).focus();
    }
    if (document.getElementById(txtFaxID).value == "")
    {
        errMsg += "\n 13) Enter Fax.";
        document.getElementById(txtFaxID).focus();
    }
    if (document.getElementById(chk1ID).checked == false )
    {
        errMsg += "\n 14) Select Terms and conditions.";
        document.getElementById(chk1ID).focus();
    }
    
    if (errMsg != "")
    {
        alert("Please fill the following info."+errMsg);
        return false;
    }
    return true;
}

function fnValidateAddHall(txtHallNameID, txtContactNameID, txtAdd1ID, txtAdd2ID, txtCityID, txtStateID, txtZipID, txtPhone1ID, txtPhone2ID, txtFaxID, txtEmailID, txtWebsiteID, txtPoliciesID, txtFeaturesID, drpCapacityID, lstServiceAreaID, drpPreferContactID)
{
    document.getElementById(txtHallNameID).value = trimStr(document.getElementById(txtHallNameID).value)
    document.getElementById(txtContactNameID).value = trimStr(document.getElementById(txtContactNameID).value)
    document.getElementById(txtAdd1ID).value = trimStr(document.getElementById(txtAdd1ID).value)
    document.getElementById(txtAdd2ID).value = trimStr(document.getElementById(txtAdd2ID).value)
    document.getElementById(txtCityID).value = trimStr(document.getElementById(txtCityID).value)
    document.getElementById(txtStateID).value = trimStr(document.getElementById(txtStateID).value)
    document.getElementById(txtZipID).value = trimStr(document.getElementById(txtZipID).value)
    document.getElementById(txtPhone1ID).value = trimStr(document.getElementById(txtPhone1ID).value)
    document.getElementById(txtPhone2ID).value = trimStr(document.getElementById(txtPhone2ID).value)
    document.getElementById(txtFaxID).value = trimStr(document.getElementById(txtFaxID).value)
    document.getElementById(txtEmailID).value = trimStr(document.getElementById(txtEmailID).value)
    document.getElementById(txtWebsiteID).value = trimStr(document.getElementById(txtWebsiteID).value)
    document.getElementById(txtPoliciesID).value = trimStr(document.getElementById(txtPoliciesID).value)
    document.getElementById(txtFeaturesID).value = trimStr(document.getElementById(txtFeaturesID).value)
    
    var errMsg = "";
    
    if (document.getElementById(txtHallNameID).value == "")
    {
        errMsg = "\n 1)Enter Hall Name."
        document.getElementById(txtHallNameID).focus();
    }
    if (document.getElementById(txtContactNameID).value == "")
    {
        errMsg += "\n 2) Enter Contact Name.";
        document.getElementById(txtContactNameID).focus();
    }
    if (document.getElementById(txtAdd1ID).value == "")
    {
        errMsg += "\n 3) Enter Address 1.";
        document.getElementById(txtAdd1ID).focus();
    }
    if (document.getElementById(txtAdd2ID).value == "")
    {
        errMsg += "\n 4) Enter Address 2.";
        document.getElementById(txtAdd2ID).focus();
    }
    if (document.getElementById(txtCityID).value == "")
    {
        errMsg += "\n 5) Enter City.";
        document.getElementById(txtCityID).focus();
    }
    if (document.getElementById(txtStateID).value == "")
    {
        errMsg += "\n 6) Enter State.";
        document.getElementById(txtStateID).focus();
    }
    if (document.getElementById(txtZipID).value == "")
    {
        errMsg += "\n 7) Enter Zip Code.";
        document.getElementById(txtZipID).focus();
    }
    if (document.getElementById(txtPhone1ID).value == "")
    {
        errMsg += "\n 8) Enter Phone 1.";
        document.getElementById(txtPhone1ID).focus();
    }
    if (document.getElementById(txtPhone2ID).value == "")
    {
        errMsg += "\n 9) Enter Phone 2.";
        document.getElementById(txtPhone2ID).focus();
    }
    if (document.getElementById(txtFaxID).value == "") 
    {
        errMsg += "\n 10) Enter Fax.";
        document.getElementById(txtFaxID).focus();
    }
    if (document.getElementById(txtEmailID).value == "")
    {
        errMsg += "\n 11) Enter EmailID.";
        document.getElementById(txtEmailID).focus();
    }
    if (validateEmail(document.getElementById(txtEmailID).value) == false )
    {
        errMsg += "\n 12) Enter correct EmailID.";
        document.getElementById(txtEmailID).focus();
    }
    if (document.getElementById(txtWebsiteID).value == "")
    {
        errMsg += "\n 13) Enter Website.";
        document.getElementById(txtWebsiteID).focus();
    }
    if (document.getElementById(txtPoliciesID).value == "")
    {
        errMsg += "\n 14) Enter Policies.";
        document.getElementById(txtPoliciesID).focus();
    }
    if (document.getElementById(txtFeaturesID).value == "")
    {
        errMsg += "\n 15) Enter Features.";
    }
    if (document.getElementById(drpCapacityID).selectedIndex == -1)
    {
        errMsg += "\n 16) Select Couples Capacity.";
        document.getElementById(drpCapacityID).focus();
    }
    
    if (document.getElementById(lstServiceAreaID).selectedIndex == -1)
    {
        errMsg += "\n 17) Select Service Area.";
        document.getElementById(lstServiceAreaID).focus();
    }
    if (document.getElementById(drpPreferContactID).selectedIndex == -1)
    {
        errMsg += "\n 18) Select prefered Contact method.";
        document.getElementById(drpPreferContactID).focus();
    }
    
    if (errMsg != "")
    {
        alert('\n Please fill the following info.'+errMsg);
        return false;
    }
    return true;
}

function fnValidateSearchKeyword(txtSearchID, dvSrchID, dvSrchErrorID)
{
    if (document.getElementById(txtSearchID).value == "")
    {
        document.getElementById(dvSrchID).style.display = "none";
        document.getElementById(dvSrchErrorID).style.display = "block";
        return false;
    }
    return true;
}
//remove the preceeding and ending spaces from a string.
function trimStr(str)
{
	var i, pBegin, pEnd, strTemp
 	//find the preceeding spaces
 	for (i = 0 ; i < String(str).length; i++)
 	{
		if (String(str).charAt(i) != " ")
		{
			pBegin = i;
	 		break;
		}
 	}
 
 	//find ending spaces
 	for (i = String(str).length -1; i >= 0; i--)
 	{
		if (String(str).charAt(i) != " ")
		{
			pEnd = i;
	 		break;
		} 
	}
 
 	//the new string.
 	strTemp = String(str).substr(pBegin, pEnd - pBegin +1 );
 	return (strTemp);
}

//FUNCTION TO VALIDATE EMAIL
function validateEmail(email)
{
//a valid email would have only one @ and one . after the @ in the string and a value before and after @ and .
	var iAtF = 0;
 	var iDotF = 0;
 	var iDotL = 0;
 	var iSpace = 0;
 	var iStrLength = 0; //for length of string

 	iAtF = String(email).indexOf("@");
 	iAtL = String(email).lastIndexOf("@");
 	if (iAtF < 1 )
		return (false);
 	if (iAtF != iAtL)
		return(false);
	
 	iDotL = String(email).lastIndexOf(".");
 	if (iDotL < iAtF + 2)
		return(false);

 	iSpace = String(email).indexOf(" ");
	 if (iSpace > 0 )
		return(false);

 	iStrLength = String(email).length;
 	if(iDotL==iStrLength-1 ||iDotL==iStrLength-2) 
		return(false);
		
	 return(true);
}

function fnCheckvalidation(txtFirstNameID, txtLastNameID, txtAddressID, txtCityID, txtStateID, txtZipCodeID, txtPhoneID, ddlCardTypeID, txtCardNumberID, txtCvvID, ddlMonthID, ddlYearID) 
{
    document.getElementById(txtFirstNameID).value = trimStr(document.getElementById(txtFirstNameID).value);
    document.getElementById(txtLastNameID).value = trimStr(document.getElementById(txtLastNameID).value);
    document.getElementById(txtAddressID).value = trimStr(document.getElementById(txtAddressID).value);
    document.getElementById(txtCityID).value = trimStr(document.getElementById(txtCityID).value);
    document.getElementById(txtStateID).vale = trimStr(document.getElementById(txtStateID).value);
    document.getElementById(txtZipCodeID).value = trimStr(document.getElementById(txtZipCodeID).value);
    document.getElementById(txtPhoneID).value = trimStr(document.getElementById(txtPhoneID).value);
    document.getElementById(txtCardNumberID).value = trimStr(document.getElementById(txtCardNumberID).value);
    document.getElementById(txtCvvID).value = trimStr(document.getElementById(txtCvvID).value);
    
    if(document.getElementById(txtFirstNameID).value.length == 0)
    {
        alert('Please endter the First Name.');
        document.getElementById(txtFirstNameID).focus();
        return false;
    }
    else if(document.getElementById(txtLastNameID).value.length == 0)
    {
        alert('Please enter the Last Name.');
        document.getElementById(txtLastNameID).focus();
        return false;
    }
    else if(document.getElementById(txtAddressID).value.length == 0)
    {
        alert('Please enter the Address.');
        document.getElementById(txtAddressID).focus();
        return false;
    }
    else if(document.getElementById(txtCityID).value.length == 0)
    {
        alert('Please enter the City.');
        document.getElementById(txtCityID).focus();
        return false;
    }
    else if(document.getElementById(txtStateID).value.length == 0)
    {
        alert('Please enter the State.');
        document.getElementById(txtStateID).focus();
        return false;
    }
    else if(document.getElementById(txtZipCodeID).value.length == 0)
    {
        alert('Please enter the Zip Code.');
        document.getElementById(txtZipCodeID).focus();
        return false;
    }
    else if(document.getElementById(txtPhoneID).value.length == 0)
    {
        alert('Please enter the Phone No.');
        document.getElementById(txtPhoneID).focus();
        return false;
    }
    else if(document.getElementById(ddlCardTypeID).selectedIndex == 0)
    {
        alert('Please select the Card Type.');
        document.getElementById(ddlCardTypeID).focus();
        return false;
    }
    else if(document.getElementById(txtCardNumberID).value.length == 0)
    {
        alert('Please enter the Card No.');
        document.getElementById(txtCardNumberID).focus();
        return false;
    }
    else if(document.getElementById(txtCvvID).value.length == 0)
    {
        alert('Please enter the CVV.');
        document.getElementById(txtCvvID).focus();
        return false;
    }
    else if(document.getElementById(ddlMonthID).selectedIndex == 0)
    {
        alert('Please select the credit card expiration month.');
        document.getElementById(ddlMonthID).focus();
        return false;
    }
    else if(document.getElementById(ddlYearID).selectedIndex == 0)
    {
        alert('Please select the credit card expiration year.');
        document.getElementById(ddlYearID).focus();
        return false;
    }
    else if(isNaN(document.getElementById(txtCardNumberID).value) == true)
    {
        alert('Please enter only numeric value into Credit Card No.');
        document.getElementById(txtCardNumberID).focus();
        return false;
    }
    else if(isNaN(document.getElementById(txtCvvID).value) == true)
    {
        alert('Please enter only numeric value into CVV.');
        document.getElementById(txtCvvID).focus();
        return false;
    }
    else if(document.getElementById(txtCvvID).value.length > 3 || document.getElementById(txtCvvID).value.length < 3)
    {
        alert('Please enter three digits numeric value into CVV.');
        document.getElementById(txtCvvID).focus();
        return false;
    }
    return true;
} 
