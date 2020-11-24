# Software Requirements Specification

## Introduction

### Purpose

This document is written mainly to let the users understand how to use our project software, and it describes the functions, developing systems and the methods used in our
project. This document can also be used for the developers to maintain and modify the software, as it includes the whole structure of the system.
本文件主要是在描述我們專案的功能、開發系統及方法,以及讓使用者了解如何使用本專案的軟體,此外,本文件也適用於開發者的經營及維護,因為此文件包含了整體系統的架構。

<!--文件中的専有詞彙解釋 -->
### Glossary

### Intended Audience and Reading Suggestions

### Product Scope

This is a convenient reservation system of the conference room. By using our system, you can know about which periods the conference rooms are free, and can reserve it
immediately, so that you can avoid the situation of having no place to go when calling a meeting.
The main goal of our project is that we are able to let users reserve conference rooms, remind the users that the reserved date and time are around the corner, and show what the
reference rooms are equipped to the user, so that the user can choose the most appropriate room for them to use.
本專案主要是用於會議室的預約統,而使用本蒸統的好處為可以較為方便,迅速地知道何時有空間的會議室可供使用,並提供預約,以防要開會卻沒有會議室可用的窘境。
而本專案主要的目標為,可提供使用者預約會議室、可提醒使用者預約時間快到了,以及提供使用者各會議室的配備、設施,以供使用者選擇最適合自己的會議室。

### References

## Overall Description

### System Environment

![](../Drawio/2.1.1.drawio.svg)

* c#
  * asp.net
* sql server(microsoft)
* 

### Functional Requirements Definition

1. 登入user (進入首頁)
2. 登入manager(進入首頁)
3. 登出
4. user借: 選Room->確認->選擇日期->選擇時間->借用
5. user看借Room紀錄
6. user取消Room紀錄
7. manager編輯Room整體設定
7. manager編輯單一Room設定
7. manager編輯User權限設定
8. manager看個別user借Room紀錄
9. 搜尋user

### User Interface Specifications

![](../Drawio/2.3.1.drawio.svg)

### Non-Functional Requirements

2. sql
  1. 定期備份
3. 驗證所有使用者輸入資料，避免 injection 攻擊
   1. email 格式符合
4. 避免使用者撈到原始碼
5. 避免使用者越權
5. 避免使用者進行非預期操作

## Requirements Specification

### External Interface Requirements

* 瀏覽器
* 網路
* Google 帳號

### Functional Requirements

詳細的 Functional Requirements Definition

## Other Nonfunctional Requirements

### Performance Requirements

|描述|
|每個載入畫面不超過3秒|
||

### Safety Requirements

* 使用者使用這網頁不會造成任何硬體上的損壞
* 請勿使用過久

### Security Requirements

3. 驗證所有使用者輸入資料，避免 injection 攻擊
   1. email 格式符合
4. 避免使用者撈到原始碼
5. 避免使用者越權
5. 避免使用者進行非預期操作

ffffffffff