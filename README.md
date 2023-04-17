# Coding Challenge: Calculator Service
This calculator service allows you to do mathematical operations such as addition, subtraction, multiplication, division and square root. It also has a journal that stores the results of each calculation during the application runtime and also has daily logs that keep track of all the internal functioning of the application.

##### _This calculator service is coded in C# using Microsoft Visual Studio 2022 and .NET 6.0, so keep that in mind to avoid possible incompatibilities._
<br/>

---

## 1. How to build the solution
- To build the solution, is as simple as opening the project with Visual Studio, and in the _**Solution Explorer**_ just right click the solution and hit **Build Solution**, or just select the solution and press **Ctrl+Shift+B**:

![How to build the solution](https://i.imgur.com/TTDjNGl.png)

## 2. How to run the application
- To run the application you just simply have to click the **Start without debugging** button positioned at the top center of Visual Studio's interface or just press **Ctrl+F5**:

![How to run the application](https://i.imgur.com/tgeioFo.png)
<br/><br/>

---
## Usage
### - Application usage
When you run the solution, two terminal windows will pop up, one will be the client, and the other one will be the server.
For now we will focus on the client usage.
<br/>
This is the calculator menu, here you will be able to select one of six options:
- Addition.
- Subtraction.
- Multiplication.
- Division.
- Square Root.
- Journal.

> _**SIDE NOTE**: Each operation has it's own validations, for example, the default max number of digits for each number is nine._ 

To select one of these options just simply write the words as they are displayed on the terminal window.
![Client menu](https://i.imgur.com/0SaEKiA.png)

#### 1. Addition
The addition operation works this way:
- Just enter the numbers you want to add separated by commas.
> This means that if you want to enter a decimal number you **MUST** use "." as the decimal separator, otherwise it will count as a separate number.

![Addition input](https://i.imgur.com/lHdAOhA.png)
- After you input the numbers you want to add the client will make a request to the server with the inputted numbers and a tracking ID and once the server calculates the result it will return it to the client and it will be printed on the screen's terminal window.
> The tracking ID is used to store the operation in the journal.

![Addition result](https://i.imgur.com/WAB7vYu.png)

<br/>

#### 2. Subtraction
The subtraction operation works this way:
- Just enter the numbers you want first as the dividend and second as the divisor.
> In this case, you can use "." or "," as the decimal separator.

![Subtraction input and result 01](https://i.imgur.com/1fskvDN.png)

![Subtraction input and result 02](https://i.imgur.com/MllCn8D.png)

<br/>

#### 3. Multiplication
The multiplication operation works exactly like the addition:
- Enter the numbers separated by commas and it will calculate each number and return the result:

![Multiplication input and result](https://i.imgur.com/IZaHYje.png)

<br/>

#### 4. Division
The division operation works this way:
- Just enter the numbers you want first as the dividend and second as the divisor.
> Keep in mind that the inputted numbers **MUST** be of type INT, due to how the division outputs the results.

![Division input and result](https://i.imgur.com/YEtQZXB.png)

<br/>

#### 5. Square Root
The square root operation works this way:
- Just input the number you want to calculate its square root and it will calculate it and return the result:

![Square Root input and result](https://i.imgur.com/MxEcMzH.png)

<br/>

#### 6. Journal 
The journal is a history of all the operations that the user has done.
> Keep in mind that the journal will **only** exist during the execution of the application, once you close it, the journal will **empty** itself.

![Journal result](https://i.imgur.com/lkjVtOf.png)

<br/>

## Logs output
Here is an example of how the logs get outputted:
- Console:

![Logs in the console](https://i.imgur.com/vU2UNzs.png)

- File:

![Logs in file](https://i.imgur.com/5b3rvSP.png)

<br/><br/>

---
## NuGets used

Here is a list of all the NuGets I used to create this Calculator Service:

| NuGet name (Client) | Links |
| ------ | ------ |
| NLog | [Nlog NuGet page](https://www.nuget.org/packages/NLog/) |
| RestSharp | [RestSharp NuGet page](https://www.nuget.org/packages/RestSharp) |
| Newtonsoft Json | [Newtonsoft Json NuGet page](https://www.nuget.org/packages/Newtonsoft.Json/) |

| NuGet name (Models Library) | Links |
| ------ | ------ |
| Microsoft AspNet MVC | [MS AspNet MVC NuGet page](https://www.nuget.org/packages/Microsoft.AspNet.Mvc) |
| Microsoft AspNet MVC Abstractions | [MS AspNet MVC Abstractions Nuget page](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Abstractions) |
| Microsoft OpenAPI | [MS OpenAPI NuGet page](https://www.nuget.org/packages/Microsoft.OpenApi) |
| NLog | [Nlog NuGet page](https://www.nuget.org/packages/NLog/) |
| NLog Web AspNetCore | [Nlog NuGet page](https://www.nuget.org/packages/NLog.Web.AspNetCore) |
| Swashbuckle AspNetCore SwaggerGen | [SwaggerGen NuGet page](https://www.nuget.org/packages/swashbuckle.aspnetcore.swaggergen/) |

| NuGet name (Server) | Links |
| ------ | ------ |
| Newtonsoft Json | [Newtonsoft Json NuGet page](https://www.nuget.org/packages/Newtonsoft.Json/) |
| NLog Web AspNetCore | [Nlog NuGet page](https://www.nuget.org/packages/NLog.Web.AspNetCore) |
| RestSharp | [RestSharp NuGet page](https://www.nuget.org/packages/RestSharp) |
| Swashbuckle AspNetCore | [Swashbuckle AspNetCore NuGet page](https://www.nuget.org/packages/Swashbuckle.AspNetCore) |

| NuGet name (Utils Library) | Links |
| ------ | ------ |
| NLog | [Nlog NuGet page](https://www.nuget.org/packages/NLog/) |
| RestSharp | [RestSharp NuGet page](https://www.nuget.org/packages/RestSharp) |
