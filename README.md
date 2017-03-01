![Testura Logo](http://testura.net/Content/Images/logo2.png)


Testura.Code.UnitTestGenerator is a simple framework/software that inspect your assembly and generate unit tests for you. 

## Supported test frameworks

* MsTest
* NUnit 

## Supported mock frameworks

* Moq 

## Will generate

* A unit test file for all types that are exported and isn't static or abstract
* A setUp that initialize the type under test (TUT) by looking at it's first constructor. 
 * All required parameters will either be mocked, initialized or set to some default value.
 * Will generate fields for TUT and required parameters
* All files created will follw your assembly namespace/directory strcture. 
