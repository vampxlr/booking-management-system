
	function isNumberKey(evt)
      {
         var charCode = (evt.which) ? evt.which : event.keyCode
         if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;

         return true;
      }
function togglethis(divid)
	{
    if (document.getElementById(divid).style.display == 'none') {
     
        document.getElementById(divid).style.display = 'block';
 
    }
    else {
      
        document.getElementById(divid).style.display = 'none';
 
    }
	}
	
	
function Toggle()
{
	// Try to get the FCKeditor instance, if available.
	var oEditor ;
	if ( typeof( FCKeditorAPI ) != 'undefined' )
		oEditor = FCKeditorAPI.GetInstance( 'DataFCKeditor' ) ;

	// Get the _Textarea and _FCKeditor DIVs.
	var eTextareaDiv	= document.getElementById( 'Textarea' ) ;
	var eFCKeditorDiv	= document.getElementById( 'FCKeditor' ) ;

	// If the _Textarea DIV is visible, switch to FCKeditor.
	if ( eTextareaDiv.style.display != 'none' )
	{
		// If it is the first time, create the editor.
		if ( !oEditor )
		{
			CreateEditor() ;
		}
		else
		{
			// Set the current text in the textarea to the editor.
			oEditor.SetData( document.getElementById('DataTextarea').value ) ;
		}

		// Switch the DIVs display.
		eTextareaDiv.style.display = 'none' ;
		eFCKeditorDiv.style.display = '' ;

		// This is a hack for Gecko 1.0.x ... it stops editing when the editor is hidden.
		if ( oEditor && !document.all )
		{
			if ( oEditor.EditMode == FCK_EDITMODE_WYSIWYG )
				oEditor.MakeEditable() ;
		}
	}
	else
	{
		// Set the textarea value to the editor value.
		document.getElementById('DataTextarea').value = oEditor.GetXHTML() ;

		// Switch the DIVs display.
		eTextareaDiv.style.display = '' ;
		eFCKeditorDiv.style.display = 'none' ;
	}
}



// The FCKeditor_OnComplete function is a special function called everytime an
// editor instance is completely loaded and available for API interactions.
function FCKeditor_OnComplete( editorInstance )
{
	// Enable the switch button. It is disabled at startup, waiting the editor to be loaded.
	document.getElementById('BtnSwitchTextarea').disabled = false ;
}

function PrepareSave()
{
	// If the textarea isn't visible update the content from the editor.
	if ( document.getElementById( 'Textarea' ).style.display == 'none' )
	{
		var oEditor = FCKeditorAPI.GetInstance( 'DataFCKeditor' ) ;
		document.getElementById( 'DataTextarea' ).value = oEditor.GetXHTML() ;
	}
}
	