﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TicketsDownLoad.aspx.cs"
    Inherits="TicketsDownLoad" %>

<%@ Register Src="UserControl/LoginName.ascx" TagName="LoginName" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>..:: United Car Exchange ::..</title>
    <link rel="shortcut icon" href="images/favicon.ico" type="image/x-icon" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="Static/Css/calender.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript" src="js/jquery-1.7.min.js"></script>

    <script type='text/javascript' src='js/jquery.alphanumeric.pack.js'></script>

    <script src="Static/JS/calendar.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
   function isNumberKeyForDt(evt)
              {	
	    
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)&& charCode != 47)
                return false;
            return true;
        }
       
        
    </script>

    <script type="text/javascript" language="javascript">

       
              var dtCh= "/";
  var Chktoday=new Date();    
            var minYear=Chktoday.getFullYear() - 1;
            var maxYear=Chktoday.getFullYear();

function isInteger(s){
	var i;
    for (i = 0; i < s.length; i++){   
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}

function stripCharsInBag(s, bag){
	var i;
    var returnString = "";
    // Search through string's characters one by one.
    // If character is not in bag, append to returnString.
    for (i = 0; i < s.length; i++){   
        var c = s.charAt(i);
        if (bag.indexOf(c) == -1) returnString += c;
    }
    return returnString;
}

function daysInFebruary (year){
	// February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28 );
}
function DaysArray(n) {
	for (var i = 1; i <= n; i++) {
		this[i] = 31
		if (i==4 || i==6 || i==9 || i==11) {this[i] = 30}
		if (i==2) {this[i] = 29}
   } 
   return this
}

function isDate(dtStr){
	var daysInMonth = DaysArray(12)
	var pos1=dtStr.indexOf(dtCh)
	var pos2=dtStr.indexOf(dtCh,pos1+1)
	var strMonth=dtStr.substring(0,pos1)
	var strDay=dtStr.substring(pos1+1,pos2)
	var strYear=dtStr.substring(pos2+1)
	strYr=strYear
	if (strDay.charAt(0)=="0" && strDay.length>1) strDay=strDay.substring(1)
	if (strMonth.charAt(0)=="0" && strMonth.length>1) strMonth=strMonth.substring(1)
	for (var i = 1; i <= 3; i++) {
		if (strYr.charAt(0)=="0" && strYr.length>1) strYr=strYr.substring(1)
	}
	month=parseInt(strMonth)
	day=parseInt(strDay)
	year=parseInt(strYr)
	if (pos1==-1 || pos2==-1){
		alert("The date format should be : mm/dd/yyyy")
		return false
	}
	if (strMonth.length<1 || month<1 || month>12){
		alert("Please enter a valid month")
		return false
	}
	if (strDay.length<1 || day<1 || day>31 || (month==2 && day>daysInFebruary(year)) || day > daysInMonth[month]){
		alert("Please enter a valid day")
		return false
	}
	if (strYear.length != 4 || year==0 || year<minYear || year>maxYear){
		alert("You cannot enter date older than one year")
		return false
	}
	if (dtStr.indexOf(dtCh,pos2+1)!=-1 || isInteger(stripCharsInBag(dtStr, dtCh))==false){
		alert("Please enter a valid date")
		return false
	}
return true
}

    </script>

    <script type='text/javascript'>
	$(function() {
		$('.number').numeric();
	});
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="Progress" runat="server" AssociatedUpdatePanelID="updtpnlTableShow"
        DisplayAfter="0">
        <ProgressTemplate>
            <div id="spinner">
                <h4>
                    <div>
                        Processing
                        <img src="images/loading.gif" />
                    </div>
                </h4>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="headder">
        <uc1:LoginName ID="LoginName1" runat="server" />
        <div class="clear">
            &nbsp;</div>
    </div>
    <div class="main">
        <h1 class="h1">
            Tickets</h1>
        <div class="clear">
            &nbsp;</div>
        <asp:UpdatePanel ID="updtpnlTableShow" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td style="width: 85px; padding-top: 10px;">
                            <%-- <input type="radio" name="showType" />
                    Pending--%>
                            <asp:RadioButton ID="rdbtnPending" runat="server" GroupName="ShowType" Text="Pending"
                                AutoPostBack="true" OnCheckedChanged="btnShow_Click" Checked="true" />
                        </td>
                        <td style="width: 95px; padding-top: 10px;">
                            <%--  <input type="radio" name="showType" />
                    All--%>
                            <asp:RadioButton ID="rdbtnProcessed" runat="server" GroupName="ShowType" Text="Processed"
                                AutoPostBack="true" OnCheckedChanged="btnShow_Click" />
                        </td>
                        <td style="width: 50px; padding-top: 10px;">
                            <%--  <input type="radio" name="showType" />
                    All--%>
                            <asp:RadioButton ID="rdbtnAll" runat="server" GroupName="ShowType" Text="All" AutoPostBack="true"
                                OnCheckedChanged="btnShow_Click" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="clear" style="height: 10px;">
            &nbsp;</div>
        <br />
    </div>
    </form>
</body>
</html>
