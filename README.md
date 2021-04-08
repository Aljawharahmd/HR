# HR
Basic Human Resource system for managing employee leaves. Developed using .NET Core. only back-end development.
System functionalities have been tested using swagger.

About system entities:
Leave information:
When a user requests a vacation, he has to fill the following information:
- Vacation type (annual, sick leave, exceptional)
- The duration of the vacation 
- Optional attachment 

User main information: 
When a manager adds a new employee, he must enter the following main information:
- Name 
- Mobile number 
- Email
- Job title

The system has two basic users which are the manager and the 
employee.
The employee will use this system to request for vacations and leaves and the 
user manger can login to the system and manage the vacation’s request for the employee 
users.
Each user has a specific number of vacation days yearly, for example he can only 
take 14 days a year.

Functional Requirements
The Employee user can: 
- Login to the system 
- Review his information (name, mobile number, email, his manager info...etc.)
- Request a new vacation 
- When he requests a vacation, he can save it as draft to edit it later or submit it to 
his manager
- Review all the vacations he requested with their statuses with the ability to filter 
on a specific vacation

The Manager user can:
- Login to the system 
- Add a new employee under his management
- Review the list of all users under his management 
Vacation request management: 
- Review the list of all vacation’s requests for all employees under his management 
- Filter on vacation a specific vacation by (date, employee name, duration, 
type...etc.)
- Approve or reject the pending requests (he must enter a reason for rejection)48

Software Requirements
MS Visual Studio as a work environment, MS SQL Server for the database.

![HR ERD](https://user-images.githubusercontent.com/68808585/114030726-59d1ee00-9883-11eb-890b-c41b2acf200c.PNG)
![Swagger](https://user-images.githubusercontent.com/68808585/114030942-8980f600-9883-11eb-841c-926c209fb28c.PNG)
