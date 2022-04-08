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
 
 About API
 
 1) This api uses JWT Authenticaiton and Authorization
 2) To generate Jason Web Token please login with Admin 
  Id: admin
  password: admin
  this step will generate a token which will be useful to authorize other APIs. For Example your admin authentication woud look something like below
  
  {
  "id": 3,
  "firstName": "Admin",
  "lastName": "Admin",
  "username": "Admin",
  "role": "Admin",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjMiLCJuYmYiOjE2NDk0MzE2NjgsImV4cCI6MTY1MDAzNjQ2NywiaWF0IjoxNjQ5NDMxNjY4fQ.Uai4AFNazHJ7rjspOsONMrNjMrT6X4XGbB0-gsBypBY"
}

copy the freshly generated token value between "" i.e. 
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjMiLCJuYmYiOjE2NDk0MzE2NjgsImV4cCI6MTY1MDAzNjQ2NywiaWF0IjoxNjQ5NDMxNjY4fQ.Uai4AFNazHJ7rjspOsONMrNjMrT6X4XGbB0-gsBypBY


 3) Now At the right top of Swagger UI you would see Authorize button which will act as input for the token. 
 4) Paste the token and click on Authorize button, then close the frame. 
 5) Now you are ready to access APIs like Admin. 
 
  



