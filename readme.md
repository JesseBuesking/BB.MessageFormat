BB.MessageFormat
================
A .NET library for running [messageformat.js](https://github.com/SlexAxton/messageformat.js "messageformat.js github page") on the server.

Choose a JavaScript engine
------------------
This library supports three different js engines:

1. [node.js](http://nodejs.org/ "nodejs's site") (requires that you have a node.js server) via ```MessageFormat.Instance.ConfigureEngine(new NodeJsEngine("my url"));```
2. [ClearScript](https://clearscript.codeplex.com/ "clearscript's site") (requires that you can run native binaries) via ```MessageFormat.Instance.ConfigureEngine(new ClearScriptEngine());```
3. [Jurassic](https://jurassic.codeplex.com/ "jurassic's site") via ```MessageFormat.Instance.ConfigureEngine(new JurassicEngine());```

Use the ```Message``` struct for convenience
--------------------------------------------
The ```Message``` struct will make working with ```MessageFormat``` a bit easier, but requires that you supply some additional configuration via ```MessageFormat.Instance.ConfigureOther(...);```

[See the tests](https://github.com/JesseBuesking/BB.MessageFormat/blob/master/BB.MessageFormat.Tests/MessageTests.cs) for example usages of the ```Message``` object.

Running on node.js
-----------------

If you want to run the version that utilizes node.js:

1. Install node.js
2. ```npm install express```
3. ```npm install messageformat```

Start it by running `node node-server.js` in a command prompt from the project's root directory.

Issues/Requests
---------------
Feel free to [request features, report any bugs/issues you find](https://github.com/JesseBuesking/BB.MessageFormat/issues), or make a [pull request](https://github.com/JesseBuesking/BB.MessageFormat/pulls). I might not get to it as soon as you'd like (I have other projects  / a job that also require my attention), but I'll do my best to fix/merge what comes in as soon as I can.

Benchmarks
----------
Machine: i7 950 @ 3.07GHz running Windows 8 (node.js running in Windows as well)

1. node.js (requires a node.js server): ~3,000 string generations/s
2. ClearScript (requires running native binaries): ~3,000 string generations/s
3. Jurassic: ~30 string generations/s (ouch!)