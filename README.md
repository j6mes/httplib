HttpLib is a free (Apache 2.0 License) web request helper for .Net that makes it easier for developers to access and download resources from the internet.

Dev build ![build status master](https://ci.appveyor.com/api/projects/status/github/j6mes/httplib)

Version [2.0.16](https://ci.appveyor.com/api/projects/status/cfxsekd76ap47fej/branch/2.0.16)

## About

The library was first released in 2012 and has since had over 50,000 downloads.

#### Download

Releases are available from the release page on this github repo. The Codeplex repo is no longer maintained.

#### Source code

Source code is available on [GitHub](https://github.com/j6mes/httplib/ "Download Sourcecode from github")

#### Latest Release:

The most recent release is **2.0.16** which supports the following features:

*   Supports most HTTP Verbs: GET / POST / PUT / DELETE and more
*   Upload and download files to disk
*   Progress monitor for file uploads/downloads
*   Completely asynchronous operation
*   Custom authentication providers can be added (currently supports basic auth)
*   Content stream can be customised
*   Cookies are static and persist between requests
*   Bug fix 16 - setting custom headers
*   Bug fix 17 - sending request without body
*   Request 18 - removed stats collection 
*   Bug fix 20 - multipart form boundary

Supported platforms: .Net4.0+ (WinForms, WCF, ASP.Net, Silverlight 5, Windows Phone 8.0+, Windows 8.0+).

#### Upcoming Releases:

**2.1:** OAuth2 authentication provider


## Examples

### GET Web Page

Performs a HTTP GET on a given URL and executes the lambda function provided to the OnSuccess method. This example prints the content of the web page to the command line.

	Http.Get("https://jthorne.co.uk/httplib").OnSuccess(result =>
	{
		Console.Write(result);
    }).Go();


Errors can be caught through using the OnFail method as show below:

    Http.Get("https://jthorne.co.uk/httplib").OnSuccess(result =>
    {
        Console.Write(result);
	}).OnFail(webexception =>
	{
	Console.Write(webexception.Message);
	}).Go();


### POST Web Page

Web page form data can be posted to a web service using the .Form method as shown below. From user requests, this method also supports posting of dictionaries.

	Http.Post("https://jthorne.co.uk/httplib").Form(new { name = "James", username = "j6mes" }).Go();

Alternatively, a raw message (such as SOAP or JSON) can be posted using the .Body method.


### Upload File to Web Service

Multiple files from the local computer can be uploaded to the remote server through specifying a list of NamedFileStreams in the .Upload method.

	Http.Post("https://jthorne.co.uk/httplib")
	.Upload(files:
		new[] { 
			new NamedFileStream("myfile", "photo.jpg", "application/octet-stream", File.OpenRead(@"C:\photo.jpg"))
				}).Go();
				
And of course, fitting with the true flexibility of HttpLib, a progress monitor and onsuccess method can be added too:

	Http.Post("https://jthorne.co.uk/httplib")
		.Upload(
			files:
				new[] { 
					new NamedFileStream("myfile", "photo.jpg", "application/octet-stream", File.OpenRead(@"C:\photo.jpg"))
					}, 
					onProgressChanged:
					(bytesSent, totalBytes) => 
					{
						Console.WriteLine("Uploading: " + (bytesSent / totalBytes)*100 + "% completed");
					})
					.OnSuccess(result=>
					{
						Console.WriteLine(result);
					}).Go();

### Download file from Web Service

Files can be downloaded directly to disk using the DownloadTo extension. An OnSuccess method can be added as a parameter here.

If the server doesn&#8217;t reply with a content length header, the totalBytes value will be null meaning that you won&#8217;t be able to give a percentage of how much of the file has been downloaded.

	Http.Get("https://jthorne.co.uk/httplib").DownloadTo(@"C:\httplib.html", onProgressChanged: (bytesCopied,totalBytes) =>; 
	{
		if (totalBytes.HasValue)
		{
			("Downloaded: " + (bytesCopied/totalBytes)*100 + "%");
		}
		Console.Write("Downloaded: " + bytesCopied.ToString() + " bytes");
		},
			onSuccess: (headers) =>
		{
			UpdateText("Download Complete");
		}).Go();
	

### DELETE Web Page

Performs a HTTP DELETE on a given URL

	Http.Delete("https://jthorne.co.uk/httplib").Go();
