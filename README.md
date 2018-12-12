# WebAssert
Simple assert class to be used with ASP.NET MVC projects.

I wanted a way to call asserts inside ASP.NET MVC code. Using the usual `Debug.Assert` ins't ideal - yes you can configure your ASP.NET MVC website to show
the failed assert dialog, but it's not a great developer experience.

I've created a custom assert class named [`WebAssert`](https://github.com/MrClyfar/WebAssert/blob/master/WebAssert/WebAssert.cs).
Here is an example of how to use it...

```csharp
public ActionResult Index(string imagePath = "")
{
    WebAssert.Assert(() => !string.IsNullOrEmpty(imagePath), "The param 'imagePath' must not be empty.");

    return View();
}
```

When an assert fails, one of two things will happen, depending on whether you are attached to the website with a debugger.

### Debugger Attached

The code will break inside `WebAssert.Assert()` giving you the chance to navigate the call stack in order to investigate why the assertion
has failed. Also, some debug information will be outputted to the 'Immediate Window' to give you a head start on what has happened...


![2018-12-12_15-19-08](https://user-images.githubusercontent.com/700064/49879170-4b917580-fe21-11e8-9f57-26169374ab08.png)


[
![2018-12-12_15-16-46](https://user-images.githubusercontent.com/700064/49879112-2b61b680-fe21-11e8-92db-30d282032191.png)
](url)

### Debugger Not Attached

If no debugger is attached, then the failed assertion details will be outputted to the browser, using a HTML file as a template.

The HTML file is called [`AssertFailed.cshtml`](https://github.com/MrClyfar/WebAssert/blob/master/WebAssert/ViewTemplates/AssertFailed.cshtml). Here is an example of what you will see when an assertion fails...

![2018-12-12_15-21-06](https://user-images.githubusercontent.com/700064/49879280-927f6b00-fe21-11e8-8461-66d846c999df.png)
