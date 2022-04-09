This Web API runs on ASP.NET core framework 6.0. 

1) This Web API is also deployed to Azure App service. Refer below link to access directly 
https://appservicemgmt.azurewebsites.net/swagger/index.html
2) To run the Web API locally in Windows
  a.Please download the zip from below one drive link 
https://1drv.ms/u/s!AjnsIWvfwtEah28KxLFJ1IghvzRR?e=TirAfN
  b. Extract the files from the folder
  c. Click on UserMgmt Application file. A console window will open as which will have link to open the api locally
  d. Link would look something like http://localhost:5000
  e. Copy the link into the browser along with /swagger/index.html as suffix for example http://localhost:5000/swagger/index.html
  f. This will open the API to be run locally without any installation.
 ______________________________________________________________________________________________________________________________
 
 API Information: 
 This WEB API uses JWT Authenticaiton and Authorization. Below are the methods exposed and more information about each API
 1) GetToken: This is used to generate token for each user. Its a JSON web token which consists of HmacSha256Signature Algorithm for encryption.
 
 2) GetAllUsers: Gives information about all the users stored in DB with JSON format. This API is exposed to all Anonymous users.


 3) GetUserByID: Give a user information based on ID. This is is exposed to users which have view access 
 
 4) AddNewUser: Requires JSON body containing User information. Igonre ID field here and remember to input roles only between Admin, View, Edit and Delete. This can be only be user by Admin users.
 
 5) UpdateUser: Update user information using this API. Requires JSON body containing User information. Igonre ID field here and remember to input roles only between Admin, View, Edit and Delete. Can only be used by users having Edit role.


 6) Delete: Provide an ID to delete specific user. Can only be userd by Admin


_________________________________________________________________________________________________________________________________
User Information:
First try to hit "GetAllUsers" API which contains JSON output of all users which are stored in In Memory DB of the API

JSON will display username and Role information of each user. Below are the password for each user

 raviahuja(ID: 1, Role: Edit): ravipassword
 
 testuser(ID:2, Role: View): testpassword
 
 admin(ID: 3, Role: Admin): admin
 
 newuser(ID: 4, Role: View): View
 
________________________________________________________________________________________________________________________________
To generate Jason Web Token please login with valid user for example 
  Id: admin
  password: admin
 1) This step will generate a token which will be useful to authorize other APIs. For Example your admin authentication woud look something like below
  
  {
  "id": 3,
  "firstName": "Admin",
  "lastName": "Admin",
  "username": "Admin",
  "role": "Admin",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjMiLCJuYmYiOjE2NDk0MzE2NjgsImV4cCI6MTY1MDAzNjQ2NywiaWF0IjoxNjQ5NDMxNjY4fQ.Uai4AFNazHJ7rjspOsONMrNjMrT6X4XGbB0-gsBypBY"
}

2) copy the freshly generated token value between "" i.e. 
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjMiLCJuYmYiOjE2NDk0MzE2NjgsImV4cCI6MTY1MDAzNjQ2NywiaWF0IjoxNjQ5NDMxNjY4fQ.Uai4AFNazHJ7rjspOsONMrNjMrT6X4XGbB0-gsBypBY

Now At the right top of Swagger UI you would see "Authorize" button which will act as input for the token.

3) Paste the token and click on Authorize button, then close the frame. 

4) Now you are ready to access APIs like Admin. 
 
  



