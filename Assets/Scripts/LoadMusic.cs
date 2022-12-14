using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleFileBrowser;

public class LoadMusic : MonoBehaviour
{
    string path  = "";
    // Start is called before the first frame update
    void Start()
    {
        		// Set filters (optional)
		// It is sufficient to set the filters just once (instead of each time before showing the file browser dialog), 
		// if all the dialogs will be using the same filters
		FileBrowser.SetFilters( true, new FileBrowser.Filter( "Musics", ".wav"), new FileBrowser.Filter( "Text Files", ".txt", ".pdf" ) );
		// Set default filter that is selected when the dialog is shown (optional)
		// Returns true if the default filter is set successfully
		// In this case, set Images filter as the default filter
		FileBrowser.SetDefaultFilter( ".wav" );

		// Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
		// Note that when you use this function, .lnk and .tmp extensions will no longer be
		// excluded unless you explicitly add them as parameters to the function
		FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe", ".mp3" );

		// Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
		// It is sufficient to add a quick link just once
		// Name: Users
		// Path: C:\Users
		// Icon: default (folder icon)
		//FileBrowser.AddQuickLink( "Users", "C:\\Users", null );

		// Show a save file dialog 
		// onSuccess event: not registered (which means this dialog is pretty useless)
		// onCancel event: not registered
		// Save file/folder: file, Allow multiple selection: false
		// Initial path: "C:\", Initial filename: "Screenshot.png"
		// Title: "Save As", Submit button text: "Save"
		// FileBrowser.ShowSaveDialog( null, null, FileBrowser.PickMode.Files, false, "C:\\", "Screenshot.png", "Save As", "Save" );

		// Show a select folder dialog 
		// onSuccess event: print the selected folder's path
		// onCancel event: print "Canceled"
		// Load file/folder: folder, Allow multiple selection: false
		// Initial path: default (Documents), Initial filename: empty
		// Title: "Select Folder", Submit button text: "Select"
		// FileBrowser.ShowLoadDialog( ( paths ) => { Debug.Log( "Selected: " + paths[0] ); },
		//						   () => { Debug.Log( "Canceled" ); },
		//						   FileBrowser.PickMode.Folders, false, null, null, "Select Folder", "Select" );

		// Coroutine example
		//LoadNewMusic();
    }

	public void LoadNewMusic()
	{
		if (Application.platform == RuntimePlatform.WebGLPlayer)
			return;
		StartCoroutine(ShowLoadDialogCoroutine());

	}
    // Update is called once per frame
    IEnumerator ShowLoadDialogCoroutine()
	{
		// Show a load file dialog and wait for a response from user
		// Load file/folder: both, Allow multiple selection: true
		// Initial path: default (Documents), Initial filename: empty
		// Title: "Load File", Submit button text: "Load"
		yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load" );

		// Dialog is closed
		// Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
		Debug.Log( FileBrowser.Success );

		if( FileBrowser.Success )
		{
			// Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
			for( int i = 0; i < FileBrowser.Result.Length; i++ )
				Debug.Log( FileBrowser.Result[i] );

			// Read the bytes of the first file via FileBrowserHelpers
			// Contrary to File.ReadAllBytes, this function works on Android 10+, as well
			byte[] bytes = FileBrowserHelpers.ReadBytesFromFile( FileBrowser.Result[0] );

			// Or, copy the first file to persistentDataPath
			//string destinationPath = Path.Combine( Application.persistentDataPath, FileBrowserHelpers.GetFilename( FileBrowser.Result[0] ) );
			//string destinationPath = Path.Combine( Application.dataPath, "Resources", FileBrowserHelpers.GetFilename( FileBrowser.Result[0] ) );
			string destinationPath = Path.Combine( Application.streamingAssetsPath, FileBrowserHelpers.GetFilename( FileBrowser.Result[0] ) );

			Debug.Log("dest path : " + destinationPath.ToString());
			FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath );
            Debug.Log("Path: " + FileBrowser.Result[0]);
            GLOBAL.CurrentMusic.name = GetName(FileBrowser.Result[0]);
			GLOBAL.CurrentMusic.isLoaded = false;
			//Debug.Log("name :" + name);
		}
	}

    string GetName(string path)
    {
        int nameStartIndex = 0;
        string backslash  = @"\";
        string name = ""; 

        for (int i = 0; i < path.Length; i++) {
            if (path[i] == '/' || path[i] == backslash[0] ) {
                nameStartIndex = i;
            }
        }
        nameStartIndex ++;
        Debug.Log("startIndex " + nameStartIndex.ToString());
        for (int i = 0; i + nameStartIndex < path.Length; i++) {
            //Debug.Log("WIll insert: " + path[i + nameStartIndex].ToString());
            //name = name.Insert(i, "a");
            name = name.Insert(i, path[i + nameStartIndex].ToString());
            //Debug.Log("name : " + name);
        }
        Debug.Log("name: " + name);
		return (name);
    }
}
///home/tLacheroy/CMGT/Visualizer-V3/Assets/StreamingAssets/onlymp3.to - Disturbed - The Sound Of Silence [Official Music Video]-u9Dg-g7t2l4-192k-1655753090867.wav
///home/tLacheroy/CMGT/Visualizer-V3/Assets/StreamingAssets/silence.wav