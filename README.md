# projects

Angular app for project submission and approval workflow

# Project Form Submit App

This is a web app that allows user singup, auth, and project form submission.

3 roles have been defined

1. Project Submitter
2. Approver
3. Admin/developer


**Project submitters** are the primary users that are allowed to register and submit project forms. Users are allowed to view their own project submissions only and edit the projects that haven't been summitted. 

When a new project record is created, it can be updated by the user. However, once submitted, the form is no longer editable. The user will have readonly access to their submitted project forms. Upon submission the form enters a Pending Approval State.

Once a user submits a project form

**Approver** is the user that has readonly access to 
* Can view all submissions but cannot modify content


**Admin/developer**
* Has full database access
* Can query project entries
* Should backup DB periodically


### Workflows

### User Sign up
1. Click Register
2. Enter valid email address and password
3. Click Sign up
4. Email message with activation link is sent out
5. User opens email and clicks the activation link
6. User is prompted to complete Profile

### Project Form Flow

> New --> {Submit} --> Pending Approval --> [ Approved | Rejected | ToBeRevised ]

## Project Form States

* New
  * can be edited
* Pending Approval
  * Project is waiting to be approved
* Rejected
  * Rejected, user cannot edit
* ToBeRevised
  * Same as New, can be edited by the user
* Approved
  * Done


