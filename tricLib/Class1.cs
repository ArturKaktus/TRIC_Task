using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tricLib
{
    
    public class Class1
    {
        public List<Service> ServiceList = new List<Service>();
        public Class1(int count)
        {
            if (count <= 0) return;

            for (int i = 0; i < count; i++)
            {
                ServiceList.Add(new Service());
            }
        }
        public class Service
        {
            public Service(bool rand = true)
            {
                if (!rand) return;
                int randTime = 10;

                var rnd = new Random();
                Name = $"Service-{rnd.Next(1, 1000)}";
                Thread.Sleep(randTime);
                ServiceStatus = (Status)rnd.Next(0, 3);
                Description = "Some Text";
                Thread.Sleep(randTime);
                UpdateTime = DateTime.Today.AddDays(rnd.Next(-3,1));

                for (int j = 1; j < 3; j++)
                {
                    Thread.Sleep(randTime);
                    StatusHistories.Add(new History()
                    {
                        UpdateTime = DateTime.Today.AddDays(rnd.Next(-2-j, j)),
                        ServiceStatus = (Status)rnd.Next(0, 2),
                        Description = "Report Text"
                    });
                }
                Thread.Sleep(randTime);
                TotalTimeChill = rnd.Next(1, 120);

            }
            public string Name { get; set; }
            public Status ServiceStatus { get; set; }
            public string Description { get; set; }
            public DateTime UpdateTime { get; set; }
            public List<History> StatusHistories { get; set; } = new List<History>();

            /*
             * Для SLA
             * TotalTimeWorker - Согласованное время работоспособности. Берется в минутах за месяц (43200)
             * TotalTimeChill - Суммарное время простоя. Берется рандомное в минутах 1..120
             */
            public double TotalTimeWorker { get; } = 43200;
            public double TotalTimeChill { get; set; }
        }

        public class History
        {
            public DateTime UpdateTime { get; set; }
            public Status ServiceStatus { get; set; }
            public string Description { get; set; }
        }
        public enum Status
        {
            On,
            Off,
            Unstable
        }

        //Вывод октуальных сервисов (последний апдейт был сегодня)
        public List<Service> GetActualStatus()
        {
            if (ServiceList.Count == 0) return null;
            var actual = ServiceList.Where(x => x.UpdateTime == DateTime.Today).ToList();
            return actual;
        }

        //Вывод истории сервиса по имени
        public List<History> GetHistoryByName(string name)
        {
            if (ServiceList.Count == 0) return null;
            var history = ServiceList.Where(x => x.Name == name).Select(x => x.StatusHistories).FirstOrDefault();
            return history;
        }

        public double? GetPercentSlaByName(string name)
        {
            if (ServiceList.Count == 0) return null;
            Service service = ServiceList.FirstOrDefault(x => x.Name == name);
            double sla = (service.TotalTimeWorker - service.TotalTimeChill) / service.TotalTimeWorker * 100;
            return Math.Round(sla, 3);
        }
    }

    
}
