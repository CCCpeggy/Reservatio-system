# Software Requirements Specification

## Introduction

### 1.1 Purpose

This document is written mainly to let the users understand how to use our project software, and it describes the functions, developing systems and the methods used in our
project. This document can also be used for the developers to maintain and modify the software, as it includes the whole structure of the system.

本文件主要是在描述我們專案的功能、開發系統及方法,以及讓使用者了解如何使用本專案的軟體,此外,本文件也適用於開發者的經營及維護,因為此文件包含了整體系統的架構。

<!--文件中的専有詞彙解釋 -->
### 1.2 Glossary

### 1.3 Intended Audience and Reading Suggestions

### 1.4 Product Scope

This is a convenient reservation system of the conference room. By using our system, you can know about which periods the conference rooms are free, and can reserve it
immediately, so that you can avoid the situation of having no place to go when calling a meeting.
The main goal of our project is that we are able to let users reserve conference rooms, remind the users that the reserved date and time are around the corner, and show what the
reference rooms are equipped to the user, so that the user can choose the most appropriate room for them to use.

本專案主要是用於會議室的預約系統,而使用本蒸統的好處為可以較為方便,迅速地知道何時有空間的會議室可供使用,並提供預約,以防要開會卻沒有會議室可用的窘境。
而本專案主要的目標為,可提供使用者預約會議室、可提醒使用者預約時間快到了,以及提供使用者各會議室的配備、設施,以供使用者選擇最適合自己的會議室。

### 1.5 References

## 2. Overall Description

### 2.1 System Environment

![](../Drawio/2.1.1.drawio.svg)

We will use asp.net to create our website, and the language we use is C#. Also, we will use sql server from Microsoft to build our web, we'll use it to save data.

The following are our system functions:
  * Manager / User login
  * Manager managing users
  * Manager managing booking records
  * Manager search users / booking records
  * Manager giving users authority
  * User reserving conference rooms
  * User cancel booking
  * System reminds user about their reservation time

System interface:
  There will be a few kinds of interfaces, we'll introduce it more at Chapter 3.1

  * Login interface 
  * Reserving interface
  * Logout interface

The following are the Non-interactive operations and the Back-up and Recovery methods of our system :

* Non-interactive operation:
  The system will send an e-mail to the user to remind him / her that the time the user booked is coming soon.

* Back-up and Recovery:
  For this part, we'll use google's gcp service to back-up all the data every fixed period. (ex: 1 month)


### 2.2 Functional Requirements Definition
First of all, the following picture is the overall flow chart of our program interface. This section also manuals several cases based on the different operations of each user or manager.
//首先，下面這張圖是我們程式介面整體的流程，本節還依據每個使用者或管理員的不同操作預設了數種案例。

![](../Drawio/2.2.1.drawio.svg)

#### **2.2.1**
**User cases** User login (Enter homepage) //登入user (進入首頁)

**Diagram**

![](../Drawio/2.2.2.drawio.svg)

**Brief Description**
User logs in this reservation system. //使用者登入本預約系統。

**Initial Step-By-Step Description**
Before logging in, the user is required to have a Google account, and the account has already had access to our website.
在登入之前，使用者必須擁有google帳號，並access到我們的網頁了。
1. User presses the login button. //1. 使用者按下登入按鈕
2. The system connects to the google account automatically //系統自動連結至google帳號
3. After connecting to user's google account, user succeeds in entering user's homepage. //3. 連結至google帳號成功後進入user的主要頁面

**Xref:** Section 3.2.1  User login (Enter homepage) //登入user (進入首頁)


### **2.2.2**
**User cases** Manager login (Enter homepage) //登入manager(進入首頁)

**Diagram**

![](../Drawio/2.2.3.drawio.svg)

**Brief Description**
Manager logs in this reservation system. //管理員登入本預約系統。
**Initial Step-By-Step Description**
Before logging in, the manager is required to have a Google account, and the account has already had access to our website.
在登入之前，管理員必須擁有google帳號，並access到我們的網頁了。
1. Manager presses the login button  //管理員按下登入按鈕
2. The system connects to the google account automatically. //系統自動連結至google帳號
3. After connecting to the manager's google account, the manager succeeds in entering manager's homepage. //連結至google帳號成功後進入管理員的主要頁面

**Xref:** Section 3.2.2 Manager login (Enter homepage) //登入manager(進入首頁)


### **2.2.3**
**User cases** Log out //登出

**Diagram**

![](../Drawio/2.2.4.drawio.svg)

**Brief Description**
When the user or manager wants to leave the system, they need to log out first.
 //使用者或管理員欲離開本系統時，要做一個登出的動作。

**Initial Step-By-Step Description**

Before logging out, the user or manager needs to have logged in to our web page.  //在登出之前，使用者或管理員需要已經登入我們的網頁了。
1. The user or manager presses the logout button in the system. // 使用者或管理員在系統中按下登出按鈕。
2. The system returns that the user / manager wants to log out. //系統回傳使用者系統回傳欲登出。
3. After successfully logging out, system returns to the login page. //登出成功後返回登入頁面。

**Xref:** Section 3.2.3 Log out //登出

### **2.2.4**
**User cases** User reserve conference room. //使用者借用會議室

**Diagram**

![](../Drawio/2.2.5.drawio.svg)
**Brief Description**
User wants to reserve a conference room. //使用者欲借用會議室。
**Initial Step-By-Step Description**

Before reserving the conference room, the user needs to have logged in to our webpage.  //在借用會議室之前，使用者需要已經登入我們的網頁了。

1. The user presses the reserve button.  //1. 使用者按下reserve按鈕。
2. The system displays the reservation interface. //2. 系統顯示reserve介面。
3. The user selects the conference room number. //3. 使用者選擇會議室房號。
4. The system prompts to confirm whether the meeting room is required by the user.  //4. 系統提示確認該會議室是否為使用者需要。
5. User confirms. //5. 使用者確認。
6. The system displays the available date and time.  //6. 系統顯示可借用的日期與時間。
7. The user selects the date and time that can be reserved, and fills in the accounts of other meeting room users according to the number of users.  //7. 使用者選擇還可借用的日期與時間，並依照使用者人數填上其他該會議室用戶的帳號。
8. The system will mark this period of time in the conference room as "reserved", and send a confirmation email to other users of this conference room.  //8. 系統將該會議室的該段時間標註為已預約，並向其他會議室用戶發出確認函。
9. Other users confirm. //9. 其他用戶確認。
10. The system prompts that the reservation is successful. //10. 系統提示成功預約。

**Xref:** Section 3.2.4  User reserve conference room. //使用者借用會議室

### **2.2.5**
**User cases** User views reserving record. //user看借Room紀錄

**Diagram**

![](../Drawio/2.2.6.drawio.svg)

**Brief Description**
User wants to view one's own reserving record. //使用者欲看借會議室之紀錄。

**Initial Step-By-Step Description**

Before querying the conference room record, the user needs to have logged in to our webpage and needs to have relevant reserving records, otherwise there will be no results. //在查詢會議室紀錄之前，使用者需要已經登入我們的網頁了，並有相關的借用紀錄，才會找到該使用者的借用紀錄，否則會查無結果。

1. The user presses the record button. //1. 使用者按下record按鈕。
2. The system searches the reserving record of the user from the database. //2. 系統將該使用者的借用紀錄從database中調出
3. The system displays the record interface. //3. 系統顯示record介面。
4. The user can view the historical reserving record. //4. 使用者可以觀看借用紀錄了。


**Xref:** Section 3.2.5 User views reserving records. //user看借Room紀錄

### **2.2.6**
**User cases** User cancels room reservation. //user取消Room預約

**Diagram**
![](../Drawio/2.2.7.drawio.svg)
**Brief Description**

User wants to cancel the room's reservation.  //使用者欲取消借會議室之預約。

**Initial Step-By-Step Description**

Before canceling the reservation of the conference room, the user needs to log in to our webpage and perform the cancellation operation before the specified time. (1 hour before the reserved time) //在取消借會議室之預約之前，使用者需要已經登入我們的網頁，並有在規定時間(1hr?)前執行取消的操作。

1. The user presses the record button. //. 使用者按下record按鈕
2. The system searches the reservation record of the user from the database. //系統將該使用者的借用紀錄從database中調出
3. The system displays the record interface. //系統顯示record介面。
4. The user can view the reservation records. //使用者可以觀看借用紀錄了。
5. The user presses the "cancel reservation button" in the record. //使用者按下記錄裡預約中的取消借用按鈕。
6. The system clears the period of the conference room as unreserved and prompts the user that the cancelation succeeds. //系統將該會議室的該時段清除為未借用，並提示使用者成功取消。

**Xref:** Section 3.2.6  User cancels room reservation. //user取消Room預約

### **2.2.7**
**User cases** Manager edits the room's overall settings //manager編輯Room整體設定
**Diagram**
![](../Drawio/2.2.8.drawio.svg)
**Brief Description**

The manager edits the usage period or the overall description of the conference room. //管理者對於整體會議室的使用時段或描述做編輯。

**Initial Step-By-Step Description**

Before the manager changes the meeting room information, the manager needs to have logged in to our webpage and entered the room settings interface. //在管理者變更會議室資料之前，管理者需要已經登入我們的網頁，並且進入room settings的介面。

1. The manager presses the room settings button. //管理者按下room settings按鈕。
2. The system recalls the overall room settings from database. //系統將整體的room的設定從database中調出。
3. The system displays the overall room settings interface. //系統顯示整體的room settings介面。
4. The manager makes changes to the overall room settings. //管理者對於整體的room settings做變更。
5. The manager presses the save button to save the changes. //管理者按下儲存變更的按鈕。
6. The system saves the changes made by the manager, and prompts the manager that the edit is successful. //系統將管理者所做的變更儲存，並提示管理者編輯成功。

**Xref:** Section 3.2.7 Manager edits the room's overall settings //manager編輯Room整體設定

### **2.2.8**
**User cases** Manager edits a single room's settings //manager編輯單一Room設定
**Diagram**
![](../Drawio/2.2.9.drawio.svg)
**Brief Description**

The manager edits the usage period or detailed description of a single specific conference room. //管理者對於單一特定的會議室使用時段或詳細描述做編輯。

**Initial Step-By-Step Description**

Before the manager changes the meeting room information, the manager needs to have logged in to our webpage and entered the room settings interface. //在管理者變更會議室資料之前，管理者需要已經登入我們的網頁，並且進入room settings的介面。

1. The manager presses the room settings button. // 管理者按下room settings按鈕。
2. The system recalls the overall room settings from the database. // 系統將整體room的設定從database中調出。
3. The system displays the overall room settings interface. //系統顯示整體的room settings介面。
4. The manager chooses to edit the details of a single room. //管理者選擇編輯單一room的詳細資料。
5. The system displays the information of a specific room to the manager.  //系統將特定room的資料顯示給管理者。
6. The manager makes changes to the specific room's settings. //管理者對於特定room settings做變更。
7. The manager presses the save button to save the changes. //管理者按下儲存變更的按鈕。
8. The system saves the changes made by the manager, and prompts the manager that the edit is successful. // 系統將管理者所做的變更儲存，並提示管理者編輯成功。

**Xref:** Section 3.2.8 Manager edits a single room's settings //manager編輯單一Room設定

### **2.2.9**
**User cases** Search users //搜尋user

**Diagram**
![](../Drawio/2.2.10.drawio.svg)
**Brief Description**

Manager can find related accounts by searching users. //管理者可以透過搜尋使用者來找到相關的帳號。

**Initial Step-By-Step Description**

The manager must have logged in to the system, and the user account he / she wants to search must exist. //管理者必須要已登入系統，且他所想要搜尋的使用者帳號必須存在。

1. The manager presses the user settings button. //管理者按下user settings按鈕。
2. The system calls out the detailed information of the overall user from the database. //系統將整體user的詳細資料從database中調出。
3. The system displays the overall user settings interface. //系統顯示整體user settings介面。
4. The manager types in the account or name of the user to be searched. //管理者選擇欲搜尋user的帳號或名字。 (我翻成他輸入要搜尋的帳號或名字  by旻)
5. The manager presses the search button. //管理者按下搜尋的按鈕。
6. The system lists the matching accounts searched by the manager for the manager to watch. //系統將管理者所搜尋到符合的項目(我翻帳號 by旻)列出，供管理者觀看。

**Xref:** Section 3.2.9 Search users //搜尋user

### **2.2.10**
**User cases** Manager edits user's authority settings //manager編輯user的權限設定
**Diagram**
![](../Drawio/2.2.11.drawio.svg)

**Brief Description**
The manager sets or edits user's authority. //管理者對於使用者的權限進行設定或編輯。

**Initial Step-By-Step Description**

The manager must have logged in to the system, and the user account he / she wants to edit must exist. // 管理者必須要已登入系統，且他所想要編輯的使用者帳號必須存在。

1. The manager presses the user settings button. //管理者按下user settings按鈕。
2. The system calls out the detailed information of the overall user from the database. //系統將整體user的詳細資料從database中調出。
3. The system displays the overall user settings interface. //3. 系統顯示整體user settings介面。

4. The manager selects the user's details to be edited and makes changes.  //4. 管理者選擇欲編輯user的詳細資料，並進行變更。

5. The administrator presses the save button to save the changes.  //管理者按下儲存變更的按鈕。
6. The system saves the changes made by the manager, and prompts the manager that the edit is successful //6. 系統將管理者所做的變更儲存，並提示管理者編輯成功。


**Xref:** Section 3.2.10 Manager edits the user's authority settings  //manager編輯User權限設定

### **2.2.11**
**User cases** Manager views individual user's room reservation record. //看個別user借Room紀錄

**Diagram**
![](../Drawio/2.2.12.drawio.svg)
**Brief Description**

Managers can find related accounts by searching users. //管理者可以透過搜尋使用者來找到相關的帳號。

**Initial Step-By-Step Description**

The manager must have logged in to the system, and the user account he wants to search must exist, and the account must have a related reserving record, otherwise there will be no results. //管理者必須要已登入系統，且他所想要搜尋的使用者帳號必須存在，且該帳號需要有相關借用紀錄，否則將會查無結果。

1. The manager presses the user settings button.  //管理者按下user settings按鈕。
2. The system calls out the detailed information of the overall user from the database. //系統將整體user的詳細資料從database中調出
3. The system displays the overall user settings interface. //系統顯示整體user settings介面。
4. The administrator types in the account or name of the user to be searched.  //管理者選擇欲搜尋user的帳號或名字。 (我翻他輸入用戶名or 帳號)
5. The manager presses the search button.  //管理者按下搜尋的按鈕。
6. The system lists the matching accounts searched by the manager for the manager to watch. //系統將管理者所搜尋到符合的項目列出，供管理者觀看。
7. The manager presses the viewing button of the reserving record of the specific user. //管理者按下特定使用者的觀看借用紀錄按鈕。
8. The system takes out the reserving record of the user from the database and displays it to the manager. //系統將該使用者的借用紀錄從database中取出，並顯示給管理者。

**Xref:** Section 3.2.11 Manager views individual user's room reservation record. //manager看個別user借Room紀錄




### 2.3 User Interface Specifications
The following figure is the overall structure diagram of our program, which will be explained in detail in this section. //下圖為整體程式架構圖，接下來此節會有詳細說明。
![](../Drawio/2.3.1.drawio.svg)

#### Common parts (user and manager use the same interface) //共同的部分(使用者與管理者使用相同的介面)

* ##### **login page**
  This login page will require users or managers to log in with a google account and will be displayed in the center of the screen. //這個登入頁面會要求使用者或管理員藉由google帳號登入，會在畫面中央顯示。

  ![](../Drawio/2.3.2.drawio.svg)

* ##### **reserve interface**
  The number and detailed information of the conference room will be displayed on the left side of the screen, so users can choose the room which is suitable for their use.  Also, the details (equipment provided) and restrictions (max / min number of people, time) of the conference room will be displayed. 

  The right side of the screen will display the date, time, and reserving status of the conference room, so that the user can choose the time when the conference room is free, and let the user enter the account of all the people who uses the conference room with him. 
  

  ![](../Drawio/2.3.3.drawio.svg)


  //畫面左側會顯示會議室的編號與詳細資料，供使用者挑選適合自己使用的會議室，而且會顯示該會議室的細節(設備提供)與限制(人數、時間)等，而畫面右側則會顯示指定會議室的日期、時間、與借用狀況，以供使用者選擇會議室空閒且自己有空的時間，並讓使用者輸入與自己一同使用會議室的人的帳號。

#### User parts (interface only available to users) //使用者的部分(使用者才有的介面)
* ##### **user main page**
  The default screen of the user's main page is the homepage, and the horizontal taskbar on the top of the page are home, reserve, record, user info,and logout, respectively.

  ![](../Drawio/2.3.4.drawio.svg)

  //使用者的main page的預設畫面是home page，上方工作列的橫條選項分別依序是home、reserve、record、user info、logout。

*  ##### **record interface**
   
   The function of the user's record interface is to view the historical reserving records of their own account. The screen will display the detailed reserving information from left to right, followed by the user name, reserving time, conference room number, meeting room details, and canceling appointments. (Need to cancel within the time limit). There is a search function in the upper right corner of the page.

    ![](../Drawio/2.3.5.drawio.svg)
   //使用者的record interface的功用為觀看自己帳號的歷史借用紀錄，畫面由左至右分別會顯示借用的詳細資料，依序為 使用者名稱、借用時間、會議室編號、會議室詳細資料、取消預約(需在時限內取消)，右上角有搜尋功能。
#### Manager parts (interface only available to managers)  //管理者的部分(管理者才有的介面)

* ##### **manager main page**

  The default screen of the manager's main page is the homepage, and the horizontal taskbar on the top of the page are home, reserve, record, room settings,user settings,user info, and logout, respectively.

    ![](../Drawio/2.3.6.drawio.svg)

  管理者的main page的預設畫面是home page，上方工作列的橫條選項分別依序是home、reserve、record、room settings、user settings、user info、logout。

* ##### **record interface**

  The function of the manager’s record interface is to view the historical reserving records of oneself and the entire system. The detailed information of the borrowing will be displayed from left to right on the screen, followed by the user name, reserving time, conference room number, conference room details, cancel the appointment. (need to cancel within the time limit) There is also a search function in the upper right corner, and there is a switch button on the upper left corner, which can switch to watch your own records or watch the records of the entire system.

  ![](../Drawio/2.3.7.drawio.svg)

  管理者的record interface的功用為觀看自己及整個系統的歷史借用紀錄，畫面由左至右分別會顯示借用的詳細資料，依序為 使用者名稱、借用時間、會議室編號、會議室詳細資料、取消預約(需在時限內取消)，右上角也有搜尋功能，在左上角多了一個switch，這可以切換要看自己的record或是觀看整個系統的record。
* ##### **user settings interface**

  The main function of the user setting management interface is to manage the authority of other users, and there is also a search function in the upper right corner.

  ![](../Drawio/2.3.8.drawio.svg)
  //使用者設定管理介面主要功能為管理其他使用者的權限，右上角一樣有搜尋功能。

* ##### **room settings interface**

  The main function of the conference room setting management interface is to manage the usage time of the conference room, the limit the number of users, time settings, and edit detailed information of the conference room (equipments that are available, whether there are charging socket or projectors, etc.). There is also a search function in the upper right corner.

  ![](../Drawio/2.3.9.drawio.svg)

  會議室設定管理介面主要功能為管理會議室的使用時間、限制使用人數、時段設定、會議室詳細資料的編寫(設備支援 有無插座、投影機...等)，右上角也有搜尋功能。
### 2.4 Non-Functional Requirements
This section illustrates the non-functional requirements which includes the backup of database, security and reliability. 

* The backup of database
  As the database is created by using mySQL, to avoid missing data while the database errors occured it take once backup every month.

* Security
  1. User identity verification, to avoid injection attack
     The user must input the correct format of the email address as verifing user identity.
  2. Avoid user access to the source code
  3. Avoid privilege escalation
     
* Reliability 
  By the user operating unexpected action, serious errors may occured. The system is reliable to avoid unexpected user operations.


2. sql (back of database)
  1. 定期備份 
3. 驗證所有使用者輸入資料，避免 injection 攻擊 /(security)
   1. email 格式符合
4. 避免使用者撈到原始碼 /(security)
5. 避免使用者越權(Privilege escalation) /(security)
5. 避免使用者進行非預期操作 /(avoid unexpected user operation)


## 3.Requirements Specification

### 3.1 External Interface Requirements

The external interfaces that would be used are:
* Browser
  * Our project is a website
* Internet
  * User has to have access to the Internet to use our reservation system
* Google account
  * Our system requires users to use Google accounts to log into the system

Other interfaces are software interfaces, which are developed in our system.
We had already introduced them briefly in Chapter 2.1, the following are the more detailed introductions: 

  * Login interface 
    * Use google account to login
  * Reserving interface
    * Lists details of every conference room
    * Requires user to enter all participants' email address
    * Show calander to user to let him / her choose the date
    * After date is chosen, system shows the available time
    * Press "Confirm" after filling up all the details

  * Logout interface
    * There will be an logout button of the right-top of every other interfaces
    * After logging out, the web goes to the Google login page.

### 3.2 Functional Requirements

### **3.2.1** User login (Enter homepage)
|Use case ID|1|
|-----|--------|
|Use Case Name|User login (Enter homepage)|
|Actors|User, reservation system, google login system|
|Description|User logs in this reservation system.|
|Trigger|User enters login page|
|Preconditions|1. User is required to have a Google account.<br> 2. The account has already had access to our website.|
|Postconditions|User completes logging in the system|
|Normal Flow|1. User presses the login button.<br>2. The system connects to the google account automatically<br>3. After connecting to user's google account, user succeeds in entering user's homepage. |

### **3.2.2** Manager login (Enter homepage)
|Use case ID|2|
|-----|--------|
|Use Case Name|Manager login (Enter homepage)|
|Actors|Manager, reservation system, google login system|
|Description|Manager logs in this reservation system.|
|Trigger|Manager enters login page|
|Preconditions|1. Manager is required to have a Google account.<br>2.The account has already had access to our website.|
|Postconditions|Manager completes logging in the system|
|Normal Flow|1. Manager presses the login button.<br>2. The system connects to the google account automatically<br>3. After connecting to manager's google account, user succeeds in entering manager's homepage. |

### **3.2.3** Log out
|Use case ID|3|
|-----|--------|
|Use Case Name|Log out|
|Actors|User / Manager, reservation system, google login system|
|Description|When the user or manager wants to leave the system, they need to log out first|
|Trigger|User / Manager presses the logout button|
|Preconditions|User or manager needs to have logged in to our web page. |
|Postconditions|User / Manager completes logging in the system|
|Normal Flow|1. The user or manager presses the logout button in the system. <br>2. The system returns that the user / manager wants to log out. <br>3. After successfully logging out, system returns to the login page.|


|Use case ID|4|
|-----|--------|
|Use Case Name|User reserve conference room|
|Actors|User, reservation system|
|Description|User wants to reserve a conference room.|
|Trigger|User presses the reserve button|
|Preconditions|The user needs to have logged in to our webpage.|
|Postconditions|The user completes reserving a conference room.|
|Normal Flow|1. The user presses the reserve button.<br>2.  The system displays the reservation interface. <br>3. The user selects the conference room number.<br>4. The system prompts to confirm whether the meeting room is required by the user.<br>5. User confirms.<br>6. The system displays the available date and time. <br>7. The user selects the date and time that can be borrowed, and fills in the account of other meeting room users according to the number of users.<br>8. The system will mark this period of time in the meeting room as "reserved", and send a confirmation email to other conference room users.<br>9. Other users confirm.<br>10. The system prompts that the reservation is successful.|


|Use case ID|5|
|-----|--------|
|Use Case Name|User views reserving record.|
|Actors|User, Reservation system|
|Description|User wants to view one's reserving record.|
|Trigger|The user presses the record button.|
|Preconditions|1. User needs to have logged in to our webpage.<br>2. User needs to have relevant reserving records.|
|Postconditions|System displays reserving record to user.|
|Normal Flow|1. The user presses the record button.<br>2. The system searches the reserving record of the user from the database.<br>3. The system displays the record interface. <br>4. The user can view the historical reserving record.|


|Use case ID|6|
|-----|--------|
|Use Case Name|User cancels room reservation.|
|Actors|User, Reservation system|
|Description|User wants to cancel the room's reservation.|
|Trigger|User presses the cancel reservation button after pressesing the record button.|
|Preconditions|1.User needs to log in to our webpage.<br> 2. The time must be at least an hour before the reserved time. <br>3. User must have reserved a conference room before cancling it.(Or there will be no reservations to be canceled.)|
|Postconditions|User completes canceling reservation.|
|Normal Flow|1. The user presses the record button.<br>2. The system searches the reservation record of the user from the database.<br>3. The system displays the record interface.<br>4. The user can view the reservation record.<br>5. The user presses the "cancel reservation button" in the record.<br>6. The system clears the period of the conference room as unreserved and prompts the user that the cancelation succeeds.|


|Use case ID|7|
|-----|--------|
|Use Case Name|Manager edits the room's overall settings|
|Actors|Manager, Reservation system|
|Description|The manager edits the usage period or the overall description of the conference room.|
|Trigger|The manager presses the room settings button.|
|Preconditions|1. Manager needs to have logged in to our webpage.<br>2. Manager needs to have entered the room settings interface.|
|Postconditions|Manager completes editing the room's overall settings|
|Normal Flow|1. The manager presses the room settings button. <br>2. The system recalls the overall room settings from database.<br>3. The system displays the overall room settings interface.<br>4. The manager makes changes to the overall room settings.<br>5. The manager presses the save button to save the changes.<br>6. The system saves the changes made by the manager, and prompts the manager that the edit is successful.|


|Use case ID|8|
|-----|--------|
|Use Case Name|Manager edits a single room's settings|
|Actors|Manager, Reservation system|
|Description|The manager edits the usage period or detailed description of a single specific conference room.|
|Trigger|The manager selects a specified room after pressing the room settings button.|
|Preconditions|1. The manager needs to have logged in to our webpage.<br>2. Manager needs to have entered the room settings interface.|
|Postconditions|Manager completes editing a specific conference room's settings.|
|Normal Flow|1. The manager presses the room settings button.<br>2. The system recalls the overall room settings from the database.<br>3. The system displays the overall room settings interface.<br>4. The manager chooses to edit the details of a single room.<br>5. The system displays the information of a specific room to the manager.<br>6. The manager makes changes to specific room's settings.<br>7. The manager presses the save button to save the changes.<br>8. The system saves the changes made by the manager, and prompts the manager that the edit is successful.|


|Use case ID|9|
|-----|--------|
|Use Case Name|Search users|
|Actors|Manager, Reservation system|
|Description|Manager can find related accounts by searching users.|
|Trigger|Manager types in the name of the user to be searched and presses the search button.|
|Preconditions|1.The manager must have logged in to the system. <br>2. The user account that the manager wants to search must exist.|
|Postconditions|Manager completes searching the user and views the user's account.|
|Normal Flow|1. The manager presses the user settings button. <br>2. The system calls out the detailed information of the overall user from the database.<br>3. The system displays the overall user settings interface.<br>4. The manager types in the account or name of the user to be searched.<br>5. The manager presses the search button.<br>6. The system lists the matching accounts searched by the manager for the manager to watch.|


|Use case ID|10|
|-----|--------|
|Use Case Name|Manager edits user's authority settings|
|Actors|Manager, Reservation system|
|Description|The manager sets or edits user's authority.|
|Trigger|The manager chooses the user account to be edited after pressing the user settings button.|
|Preconditions|1. The manager must have logged in to the system.<br>2. The user account that the manager wants to edit must exist.|
|Postconditions|Manager completes editing user's authority.|
|Normal Flow|1. The manager presses the user settings button.<br>2. The system calls out the detailed information of the overall user from the database.<br>3. The system displays the overall user settings interface.<br>4. The manager selects the user's details to be edited and makes changes.<br>5. The administrator presses the save button to save the changes.<br>6. The system saves the changes made by the manager, and prompts the manager that the edit is successful. |


|Use case ID|11|
|-----|--------|
|Use Case Name|Manager views individual user's room reservation record|
|Actors|Manager, Reservation system|
|Description|Managers can find related accounts by searching users.|
|Trigger|The manager chooses the user account and presses the view button after pressing the user settings button.|
|Preconditions|1. The manager must have logged in to the system.<br>2. The user account that the manager wants to search must exist.<br>3. The account to be searched must have a related reserving record.|
|Postconditions|System displays the user's reservation records to the manager.|
|Normal Flow|1. The manager presses the user settings button.<br>2. The system calls out the detailed information of the overall user from the database.<br>3. The system displays the overall user settings interface.<br>4. The administrator types in the account or name of the user to be searched.<br>5. The manager presses the search button.<br>6. The system lists the matching accounts searched by the manager for the manager to watch.<br>7. The manager presses the viewing button of the reserving record of the specific user. <br>8. The system takes out the reserving record of the user from the database and displays it to the manager.|


## 4. Other Nonfunctional Requirements

### 4.1 Performance Requirements

1. Every booking submission and modification should be updated in the common database within 3 seconds after each submission and modification activity
2. Loading the announcement about the reservation website within 3 seconds 
3. Results for the availability of each room within 5minute
4. After confirm the reservation of the booking, the system redirects to the main page within 3 seconds
5. Confirmation email shall be sent into the user's mentioned email within 1minute after confirmation page termination
|描述|
|每個載入畫面不超過3秒|


### 4.2 Safety Requirements

* 使用者使用這網頁不會造成任何硬體上的損壞
* 請勿使用過久

### 4.3 Security Requirements

3. 驗證所有使用者輸入資料，避免 injection 攻擊
   1. email 格式符合
4. 避免使用者撈到原始碼
5. 避免使用者越權
5. 避免使用者進行非預期操作

