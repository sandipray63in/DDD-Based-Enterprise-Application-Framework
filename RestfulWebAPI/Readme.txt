Most of the code in this project is copy-pasted(may be with some minor refactorings) from downloadable code available at 
http://www.apress.com/9781430261759 and http://www.apress.com/9781430257820.

Validations - For Validations, one can use Data Annotations and if needed, one can go via the path of custom validations
overriding ValidationAttribute class(alongwith using Data Annotations) etc but a better approach is to use Fluent 
Validations(https://github.com/JeremySkinner/FluentValidation) for server side validations and Unobtrusive JQuery 
(http://stackoverflow.com/questions/11534910/what-is-jquery-unobtrusive-validation) 
for client side validations and this overall approach is better since it keeps the validation logic seperated from your 
domain logic and Seperation of Concerns is a best practice to follow.If you want to build a custon Fluent 
Validation Framework, here is one nice example - 
https://blog.longle.net/2013/06/03/building-an-extensible-fluent-validation-framework-using-generic-funcs-and-wiring-it-up-to-mvc-4/.
A nice article on how to use Fluent Validations alongwith Unobtrusive JQuery is 
http://www.jerriepelser.com/blog/remote-client-side-validation-with-fluentvalidation.
N.B. - Although the suggested links above for validations might be applicable for ASP.NET MVC but the same is applicable for 
ASP.NET Web API as well.

Micro-Services - If your application is quite large and different functionalities have different scalability requirements,
go for the Micro-Services way and you should ideally then have different Services(in ASP.NET REST Web API, different dll projects 
for each group of Services) for different functionalities e.g say in an e-Commerce app, the Adding Products module(may be 
done via some admin) will definitely have less load compared to the actual Customer Product page where in the customer can buy products and 
so considering say  the load on Customer Products page is 10 times that of the Add Products page, you can have different set of Service(s)
for Add Product and Customer Product and deploy the services for Add Product in 1 server while for Customer Product you can use 10 Servers
(in a load-balanced way- if needed). A cloud environment is ideal for Micro-Services atleast for deployment related infrastructure(alongwith lots of 
other advantages).
For more info on Microservices visit - http://martinfowler.com/articles/microservices.html and http://microservices.io/.
A very important pattern related to Micro-Services is API Gateway pattern(http://microservices.io/patterns/apigateway.html)
. You can build your own custom API Gateway as per requirements or opt for buying COTS(Commercial Off the Shelf) products like
Apigee(https://www.quora.com/What-does-Apigee-do), Oracle OAG, 3Scale, Layer7, IBM API Management etc.