# PeproV1

![Make WinForms Great Again](https://img.shields.io/badge/WinForms-Great_Again-blue?style=for-the-badge&logo=windows)

*Make WinForms Great Again!* 🎉

**Pepro** - Personnel and Project Management System (short for *Pepro Management System*) is a Windows Forms (WinForms) application built with C# for managing personnel, projects, payroll, and internal documentation. It is designed for desktop environments with a focus on modular design, 3-layer model architecture, and SQL Server integration.

## 📌 Features

- 👨‍💻 Personnel, Project, task and department management
- 💰 Salary and payroll processing
- 🔐 Secure data with symmetric encryption
- 📊 Role-based access and permission control

## 🖥️ Technologies Used

- 💻 **.NET 9+**
- 🧱 **WinForms (Windows Forms)**
- 🗄️ **SQL Server**

## 🛠️ Project Structure

```
Pepro/
├── Pepro.Presentation/		# WinForms UI Layer
├── Pepro.Business/         # Business Logic Layer
├── Pepro.DataAccess/       # SQL Data Access Layer
└── Database/               # SQL scripts (schema + seed)
```

- Layered architecture, 3-layer model (Presentation, Business, DataAccess)
- Client and database.

## 🚀 Getting Started

1. Clone the Repository
2. Open in Visual Studio

	* Open [`Pepro.sln`](Pepro.sln) with Visual Studio 2022 or newer

3. Set up the Database

    * Open `cmd` and run script `SetupDatabase.bat`
	* Update the connection string in [`Pepro.DataAccess\Utilities\DataProvider.cs`](Pepro.DataAccess\Utilities\DataProvider.cs#L8)

4. Build and Run

	* Set `Pepro.Presentation` as the startup project
	* Press `F5` to run the application

## 🧑‍💻 Contributing

We welcome contributions!

* Check out our [Contributing Guide](CONTRIBUTING.md)
* Follow our [Code Style Guidelines](CODE_STYLE.md)

## 📷 Screenshots

![image1](Docs/Screenshots/image1.png)
*Figure 1: Login Form*

![image2](Docs/Screenshots/image2.png)
*Figure 2: Account with Admin role in Main page*

<details>
  <summary>More</summary>

![image3](Docs/Screenshots/image3.png)
*Figure 3: Personal information page*

![image4](Docs/Screenshots/image4.png)
*Figure 4: Accounts with roles of Department Head and HR in Employee page*

![image5](Docs/Screenshots/image5.png)
*Figure 5: Employee information editing page*

![image6](Docs/Screenshots/image6.png)
*Figure 6: Account with roles of Department Head and Finance in Payroll page*

![image7](Docs/Screenshots/image7.png)
*Figure 7: Account with IT role in Assignment page*

</details>

## Shout out to the Contributors

Big thanks to everyone who's contributed to this repo!

<a href="https://github.com/GeMiNiOranGe/PeproV1/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=GeMiNiOranGe/PeproV1" />
</a>

## 📃 License

<!-- This project is open source and available under the MIT License. -->
*Not available*
