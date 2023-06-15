
using System;
using System.Collections.Generic;

namespace HumanResourceManagementSystem
{
    public class Department
    {
        public string Name { get; set; }
        public int EmployeeLimit { get; set; }
        public decimal SalaryLimit { get; set; }
        public List<Employee> Employees { get; set; }

        public Department(string name, int employeeLimit, decimal salaryLimit)
        {
            Name = name;
            EmployeeLimit = employeeLimit;
            SalaryLimit = salaryLimit;
            Employees = new List<Employee>();
        }

        public decimal CalcSalaryAverage()
        {
            if (Employees.Count == 0)
            {
                return 0;
            }

            decimal totalSalary = 0;
            foreach (Employee employee in Employees)
            {
                totalSalary += employee.Salary;
            }

            return totalSalary / Employees.Count;
        }
    }

    public class Employee
    {
        public string No { get; set; }
        public string Fullname { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }

        public Employee(string no, string fullname, string position, decimal salary)
        {
            No = no;
            Fullname = fullname;
            Position = position;
            Salary = salary;
        }
    }

    public interface IHumanResourceManager
    {
        void AddDepartment(string name, int employeeLimit, decimal salaryLimit);
        void RemoveDepartment(string name);
        void AddEmployee(string departmentName, string employeeNo, string fullname, string position, decimal salary);
        void RemoveEmployee(string departmentName, string employeeNo);
        void EditEmployee(string employeeNo, string fullname, decimal salary, string position);
        List<Department> GetDepartments();
    }

    public class HumanResourceManager : IHumanResourceManager
    {
        public List<Department> Departments { get; set; }

        public HumanResourceManager()
        {
            Departments = new List<Department>();
        }

        public void AddDepartment(string name, int employeeLimit, decimal salaryLimit)
        {
            Department department = Departments.Find(d => d.Name == name);
            if (department != null)
            {
                Console.WriteLine("Bu şöbe artıq mövcuddur.");
                return;
            }

            department = new Department(name, employeeLimit, salaryLimit);
            Departments.Add(department);
            Console.WriteLine("Şöbe uğurla elave edildi.");
        }

        public void RemoveDepartment(string name)
        {
            Department department = Departments.Find(d => d.Name == name);
            if (department == null)
            {
                Console.WriteLine("Bu adda heç bir şöbe tapılmadı.");
                return;
            }

            if (department.Employees.Count > 0)
            {
                Console.WriteLine("Bu şöbede işçiler var. Siz şöbeni sile bilmezsiniz.");
                return;
            }

            Departments.Remove(department);
            Console.WriteLine("Şöbe uğurla silindi.");
        }

        public void AddEmployee(string departmentName, string employeeNo, string fullname, string position, decimal salary)
        {
            Department department = Departments.Find(d => d.Name == departmentName);
            if (department == null)
            {
                Console.WriteLine("Bu adda heç bir şöbe tapılmadı.");
                return;
            }

            if (department.Employees.Count >= department.EmployeeLimit)
            {
                Console.WriteLine("İdarenin işçilerinin sayı heddinə çatıb.");
                return;
            }

            if (department.CalcSalaryAverage() + salary > department.SalaryLimit)
            {
                Console.WriteLine("Departamentin orta emək haqqı onun heddini keçecək.");
                return;
            }

            string departmentCode = department.Name.Substring(0, 2).ToUpper();
            Employee employee = department.Employees.Find(e => e.No == employeeNo);
            if (employee != null)
            {
                Console.WriteLine("Bu işçi nömresine malik işçi artıq mövcuddur.");
                return;
            }

            employee = new Employee(departmentCode + employeeNo, fullname, position, salary);
            department.Employees.Add(employee);
            Console.WriteLine("İşçi uğurla elave edildi.");
        }

        public void RemoveEmployee(string departmentName, string employeeNo)
        {
            Department department = Departments.Find(d => d.Name == departmentName);
            if (department == null)
            {
                Console.WriteLine("Bu isimde bir departman bulunamadı.");
                return;
            }

            Employee employee = department.Employees.Find(e => e.No == employeeNo);
            if (employee == null)
            {
                Console.WriteLine("Bu işçi numarasına sahip bir çalışan bulunamadı.");
                return;
            }

            department.Employees.Remove(employee);
            Console.WriteLine("Isci ugurla silindi.");
        }

        public void EditEmployee(string employeeNo, string fullname, decimal salary, string position)
        {
            foreach (Department department in Departments)
            {
                Employee employee = department.Employees.Find(e => e.No == employeeNo);
                if (employee != null)
                {
                    employee.Fullname = fullname;
                    employee.Salary = salary;
                    employee.Position = position;
                    Console.WriteLine("Isci ugurla güncellendi.");
                    return;
                }
            }

            Console.WriteLine("Bu işçi nömresine malik heç bir işçi tapılmadı.");
        }

        public List<Department> GetDepartments()
        {
            return Departments;
        }
    }

    public class Program
    {
        private static readonly IHumanResourceManager _humanResourceManager = new HumanResourceManager();

        static void Main()
        {
          

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("İnsan Resurslarının İdareetme Sistemi");
                Console.WriteLine("1. Şobelerin ve iscilerin siyahısı");
                Console.WriteLine("2. Şobe elave et");
                Console.WriteLine("3. Şobe Sil");
                Console.WriteLine("4. İşçi elave et");
                Console.WriteLine("5. İşçi Sil");
                Console.WriteLine("6. İşçi Güncelle");
                Console.WriteLine("7. Cixis");
                Console.Write("Seciminizi edin (1-7): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowDepartments();
                        break;
                    case "2":
                        AddDepartment();
                        break;
                    case "3":
                        RemoveDepartment();
                        break;
                    case "4":
                        AddEmployee();
                        break;
                    case "5":
                        RemoveEmployee();
                        break;
                    case "6":
                        EditEmployee();
                        break;
                    case "7":
                        exit = true;
                        Console.WriteLine("Cixis edilir...");
                        break;
                    default:
                        Console.WriteLine("Kecersiz bir secim etdiniz.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void ShowDepartments()
        {


            List<Department> departments = _humanResourceManager.GetDepartments();

            if (departments.Count == 0)
            {
                Console.WriteLine("Qeydiyyatdan keçmiş şöbeni tapmaq mümkün olmadı.");
                return;
            }

            Console.WriteLine("Sobeler:");
            foreach (var department in departments)
            {
                Console.WriteLine($"Şobe Adı: {department.Name}");
                Console.WriteLine($"Isci Sayısı: {department.Employees.Count}");
                Console.WriteLine($"Maaş Ortalaması: {department.CalcSalaryAverage()}");

                foreach (var employee in department.Employees)
                {


                    Console.WriteLine($"İsci No: {employee.No}");
                    Console.WriteLine($"Ad: {employee.Fullname}");
                    Console.WriteLine($"Vezife: {employee.Position}");
                    Console.WriteLine($"Maaş: {employee.Salary}");
                    Console.WriteLine();
                }
                Console.WriteLine();

            
        

        }
        }

        static void AddDepartment()
        {
            Console.Write("Sobe adı: ");
            string name = Console.ReadLine();

            Console.Write("İşçi limiti: ");
            int employeeLimit = Convert.ToInt32(Console.ReadLine());

            Console.Write("Maaş limiti: ");
            decimal salaryLimit = Convert.ToDecimal(Console.ReadLine());

            _humanResourceManager.AddDepartment(name, employeeLimit, salaryLimit);
        }

        static void RemoveDepartment()
        {
            Console.Write("Silmek istediyiniz şöbe adını daxil edin: ");
            string name = Console.ReadLine();

            _humanResourceManager.RemoveDepartment(name);
        }

        static void AddEmployee()
        {
            Console.Write("İşçinin yerleşdiyi sobenin adı: ");
            string departmentName = Console.ReadLine();

            Console.Write("İşçi nomresi: ");
            string employeeNo = Console.ReadLine();

            Console.Write("Ad ve soyad: ");
            string fullname = Console.ReadLine();

            Console.Write("Vezife: ");
            string position = Console.ReadLine();

            Console.Write("Maaş: ");
            decimal salary = Convert.ToDecimal(Console.ReadLine());

            _humanResourceManager.AddEmployee(departmentName, employeeNo, fullname, position, salary);
        }

        static void RemoveEmployee()
        {
            Console.Write("İşçinin yerleşdiyi şöbenin adı: ");
            string departmentName = Console.ReadLine();

            Console.Write("Silmek istediyiniz isci nomresini daxil edin: ");
            string employeeNo = Console.ReadLine();

            _humanResourceManager.RemoveEmployee(departmentName, employeeNo);

        }

        static void EditEmployee()
        {
            Console.Write("Yenilenecek isci nomresini daxil edin: ");
            string employeeNo = Console.ReadLine();

            Console.Write("Yeni ad ve soyad: ");
            string fullname = Console.ReadLine();

            Console.Write("Yeni maaş: ");
            decimal salary = Convert.ToDecimal(Console.ReadLine());

            Console.Write("Yeni vezife: ");
            string position = Console.ReadLine();

            _humanResourceManager.EditEmployee(employeeNo, fullname, salary, position);
        }
    }
}

