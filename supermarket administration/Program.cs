using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace supermarket_administration
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Cashbox cashbox = new Cashbox();
            Product[] optionsProducts = new Product[] { new Product("молоко", 68), new Product("хлеб", 16), new Product("сыр", 120), new Product("сливки", 250), new Product("сметана", 82), new Product("шоколад", 65) };
            Random random = new Random();
            int minProductsCount = 2;
            int productsCount = random.Next(minProductsCount, optionsProducts.Length + 1);
            int minClientsCount = 3;
            int maxClientsCount = 11;
            int clientsCount = random.Next(minClientsCount, maxClientsCount);

            for(int i = 0; i < clientsCount; i++)
            {
                List<Product> clientProducts = new List<Product>();
                int clientMoney = random.Next(20, 321);

                for (int j = 0; j < productsCount; j++)
                {
                    int productIndex = random.Next(0, optionsProducts.Length - 1);
                    bool isMeet = clientProducts.Contains(optionsProducts[productIndex]);

                    if(isMeet == false)
                        clientProducts.Add(optionsProducts[productIndex]);
                }

                cashbox.AddClient(new Client(clientProducts, clientMoney));
            }

            cashbox.Work();
        }
    }

    class Cashbox
    {
        private Queue<Client> _clients;
        private int _money;

        public Cashbox()
        {
            _clients = new Queue<Client>();
            _money = 0;
        }

        public void AddClient(Client client)
        {
            _clients.Enqueue(client);
        }

        public void Work()
        {
            int clientIndex = 0;

            while (_clients.Count > 0)
            {
                Client servicedClient = _clients.Peek();
                bool canPay = false;
                clientIndex++;

                while (canPay == false)
                {
                    int totalSum = servicedClient.CalculatePurchaseAmount();

                    Console.Clear();

                    ShowMoney();

                    Console.WriteLine($"{clientIndex} клиент обслуживается на кассе, у него на счету {servicedClient.Money} рублей\nОбщая сумма покупки: {totalSum}");

                    if (totalSum > servicedClient.Money)
                    {
                        servicedClient.RemoveRandomProduct();
                    }
                    else
                    {
                        canPay = true;

                        if(totalSum == 0)
                        {
                            Console.WriteLine("Клиент не смог ничего оплатить и ушел");
                        }
                        else
                        {
                            servicedClient.Pay(totalSum);
                            _money += totalSum;
                            
                            Console.WriteLine("Клиент заплатил и ушел");
                        }

                        _clients.Dequeue();
                    }

                    Console.ReadKey();
                }
            }
        }

        private void ShowMoney()
        {
            Console.SetCursorPosition(0, 15);
            Console.WriteLine($"Количество денег в кассе: {_money}");
            Console.SetCursorPosition(0, 0);
        }

        private int CalculatePurchaseAmount(List<Product> products)
        {
            int totalSum = 0;

            foreach(var product in products)
            {
                totalSum += product.Price;
            }

            return totalSum;
        }
    }

    class Client
    {
        private List<Product> _products;
        public int Money { get; private set; }

        public Client (List<Product> products, int money)
        {
            _products = products;
            Money = money;
        }

        public void RemoveRandomProduct ()
        {
            Random random = new Random();
            int productIndex = random.Next(0, _products.Count - 1);

            Console.WriteLine($"Клиент выложил {_products[productIndex].Title}");

            _products.RemoveAt(productIndex);
        }

        public int CalculatePurchaseAmount()
        {
            int totalSum = 0;

            foreach (var product in _products)
            {
                totalSum += product.Price;
            }

            return totalSum;
        }

        public void Pay(int prise)
        {
            Money -= prise;
        }
    }

    class Product
    {
        public string Title { get; private set; }
        public int Price { get; private set; }

        public Product(string title, int price)
        {
            Title = title;
            Price = price;
        }
    }
}
