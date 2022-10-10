namespace YouTubeDemo_
{
    //bir sınıf sadece bir sınıfı inherite edebilir. Ancak bir sınıf birden fazla interface i implement edebilir
    //interface ile yapılan işlemler abstract işlemlerdir
    //abstract sınıflar ile tekrarın önüne geçilir
    //abstract sınıflar sınıf özelliği gösterdiği için sadece bir sınıf sadece bir abstract sınıfı veya başka bir sınıfı inherite edebilir
    class Program
    {
        static void Main(string[] args)
        {/*
            CreditManager creditManager = new CreditManager();
            creditManager.Calculate();
            creditManager.Save();

            Customer customer = new Customer();
            // örneğini oluşturmak, instance oluşturmak, instance cration
            customer.Id = 1;
            customer.City = "Ankara";

            CustomerManager customerManager = new CustomerManager(customer);
            customerManager.Save();

            Company company = new Company();
            company.TaxtNumber = "4651651";
            company.CompanyName = "CASPER";
            company.Id = 100;
            CustomerManager customerManager1 = new CustomerManager(new Person());

            Person person = new Person();
            person.NationalIdentity = "12345678901";
            person.FirstName = "Ersin";

            Customer c1 = new Customer();
            Customer c2 = new Person();//birbirinin hit deki referans numaralarını tutabilirler
            Customer c3 = new Customer();
            */
            
            CustomerManager customerManager = new CustomerManager(new Customer(),new MilitaryCreditManager());//bu şekilde credi yöntemini değiştirmek çok kolay
            //new kullanılması zaafiyettir.  IoC Container ile kontrol edilebilir
            customerManager.GiveCredit();

            CustomerManager customerManager1 = new CustomerManager(new Customer(), new CarCreditManager());
            customerManager1.GiveCredit();//bu hızlı geçişler veritabanına kaydetme biçimini değiştirmek için de kullanılabilir

            Console.ReadLine();
            //interface lerin amacı bağımlılıkları engellemek. Debamlı if else kullanılmasının önüne geçmek
        }
        class CreditManager
        {
            public void Calculate(int creditType)//bu şekilde alıp
            {//bu şekilde ifler ile kontrol etmek yazılım kalitesini düşürür
                //"sonar qube "   projedeki if sayısına bakıp zafiyetleri söyleyebilir
                if (creditType == 1)//esnaf
                {

                }
                if (creditType == 2)//ogretmen
                {
                    //bu yanlış yöntem
                }
                Console.WriteLine("Hesaplandı");
            }
            public void Save()
            {
                Console.WriteLine("Kredi Verildi");
            }
        }
        interface ICreditManager//kredi yöneticisi
        {
            //interface metodun ne döndürdüğünü, ismini, ve varsa parametrelerini tutar
            void Calculate();//bu bir imzadır
            void Save();
        }
        abstract class BaseCreditManager : ICreditManager
        {//abstract sınıflarda tamamlanmamaış "Calculate" ve tamamlanmış "Save" operastonları yazabiliriz
            public abstract void Calculate();
                //Calculate hepsinde değişecek olan bir işlem olduğundan ona abstract dedik
            public virtual void Save()
            {// hepsinde aynı olması durumunda public void Save()
                Console.WriteLine("Kaydedildi");
            }
        }
        class TeacherCreditManager : BaseCreditManager, ICreditManager// TeacherCreditManager IcreditManager interface ininn  operasyonlarını doldurmak zorundadır
        {
            public override void Calculate()//BaseCreditManager kullandığımız için override yani üstüne yaz diyoruz
            {
                //öğretmen kredisine göre hesaplamalar
                Console.WriteLine("Öğretmen kredisi hesaplandı");
            }

        }
        //militaryCreditManager:IcreditManager        dedikten sonra virgül verip başka interface ler de implement edebilirdik
        class MilitaryCreditManager : BaseCreditManager, ICreditManager//JAVA daki yazım şekli " implement " .  interface kendisini implement edenlerin referansını tutar
        {
            public override void Calculate()
            {
                Console.WriteLine("Asker Kredisi Hesaplandı");
            }
            //DRY   DO NOT REPEAT YOURSELF    KENDİNİ TEKRARLAMA
        }
        class CarCreditManager : BaseCreditManager, ICreditManager// yeni bir seçenek eklerken mevcut kodu bozmak zorunda değiliz
        {
            public override void Calculate()
            {
                Console.WriteLine("Taşıt Kredisi Hesaplandı");
            }

            public override void Save()
            {//base.Save()  temel sınıf /inherit ettiği sınıf  demektir.
                //inherit edilen kodların 
                //öncesinde kod çalıştırabiliriz
                base.Save();//kendisini sile de biliriz
                //sonrasına da kod çalıştırabiliriz
            }
        }
        class Customer//bu bir temel sınıf, kendi özelliklerini de içerir
        {
            public Customer()//her nesne new lendiğinde bura çalışır
            {
                Console.WriteLine("Müşteri nesnesi başladı");
            }

            public int Id { get; set; }
            public string City { get; set; }//sonradan eklemesi kolay
        }
        class Company:Customer// Company extends Customer
        {
            public string CompanyName { get; set; }
            public string TaxtNumber { get; set; }
        }
        class Person:Customer// bu sınıflar temel sınıfı kullanmanın yanında kendi özellikleri vardır
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string NationalIdentity { get; set; }
        }
        //katmanlı mimariler
        class CustomerManager 
        {
            private ICreditManager _creditManager;
            private Customer _customer;
            public CustomerManager(Customer customer, ICreditManager creditManager)//interface ler referans tiptir. Buradaki durum poliformizm dir. Yani çok biçimlilik.
            {
                _customer = customer; //diğer metotlardan ulaşmak için yapıyoruz
                _creditManager = creditManager;
            }
            public void Save()//encapsulation. Müşteri nesnesi parametre olarak gidiyor
            {
                Console.WriteLine("Müşteri Kaydedildi:");//önceki örenkten kaldı      +_customer.FirstName
            }
            public void Delete()
            {
                Console.WriteLine("Müşteri Silindi:");
            }
            public void GiveCredit() 
            {
                _creditManager.Calculate();//krediyi vermeden önce bilgilerine göre hesaplıyoruz
                Console.WriteLine("Kredi Verildi");

            }
        }
    }


}
