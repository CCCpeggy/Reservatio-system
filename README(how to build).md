# Reservatio-system

軟體工程作業──會議室預約系統

### Demo 網址

[會議室預約系統](https://system20201222123240.azurewebsites.net/)

### 下載專案

``` cmd
git clone https://github.com/CCCpeggy/Reservatio-system.git
```

### 系統環境

* Visual Studio 2019
* 需安裝 [ASP.NET and web development], [Data Storage and processing]

### 程式如何建置與發布

* 共同修改內容
  * 在 Web.config 中更新 connectionStrings 連資料庫的字串
  * HomeController 的 409 行，放入希望發布的使用者的 email 與 password
* 使用 Visual Studio 發布功能，可發布於
  * 本機資料夾
  * 發布在本機的 IIS 上
  * 發布在 azure 上