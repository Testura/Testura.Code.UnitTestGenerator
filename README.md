![Testura Logo](http://testura.net/Content/Images/logo2.png)


Testura.Code.UnitTestGenerator is a simple framework/tool to inspect assemblies and generate unit tests from exported types. It's created as a simple example on how you can use [Testura.Code](https://github.com/Testura/Testura.Code) 

# Client 

[https://github.com/Testura/Testura.Code.UnitTestGenerator/releases](https://github.com/Testura/Testura.Code.UnitTestGenerator/releases)

# Basic info

## Supported test frameworks

* MsTest
* NUnit 

## Supported mock frameworks

* Moq 

## Will generate

* A unit test file for all types that are exported and isn't static or abstract
* A setUp that initialize the type under test (TuT) by looking at it's first constructor. 
 * All required parameters will either be mocked, initialized or set to some default value.
 * Will generate fields for TuT and required parameters
* All files created will follw your assembly namespace/directory strcture. 

## Example usage from api 

```c#
var unitTestGenerator = new UnitTestGenerator();
unitTestGenerator.GenerateUnitTests(DllPath, TestFrameworks.NUnit, MockFrameworks.Moq, OutputDirectory);
 ```

## GUI

![Gui](http://i.imgur.com/unqTfan.png)

## Example of generation 

### Type under test

```c#
namespace Testura.Android.Device.Services.Default
{
    public class UiService
    {
        public UiService(IScreenDumper screenDumper, SomeClass someClass, string myString, int number)
        {
        }
    }
 }
```

### Generated unit test

```c#
namespace Testura.Android.Tests.Device.Services.Default
{
    [TestFixture]
    public class UiServiceTest
    {
        private Mock<IScreenDumper> screenDumperMock; 
        private SomeClass someClass; 
        private UIService uiService; 
    
        [SetUp]
        public SetUp()
        {
            screenDumperMock = new Mock<IScreenDumper>(); 
            uiService = new UiService(screenDumperMock.Object, someClass, string.Empty, 0)
        }
    }
 }
```

