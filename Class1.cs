using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APILoadTest
{
    class Class1
    {
        public async void MainMethod()
        {
            counter = 0;
            var sw = Stopwatch.StartNew();

            //Create Actions
            var actions = Enumerable.Range(0, 10000)
                .Select(i => ((Action)(() => DoSomething(i))));

            //Run all parallel with 25 Tasks-in-parallel
            await DoAll(actions, 1000);

            Console.WriteLine("Total Time: " + sw.ElapsedMilliseconds);
        }

        public static int counter { get; set; }


        public void DoSomething(int i)
        {
            var result = ApiServices.ApiGet("http://dev.intsvc.nelnet.net:4106/api/v1/states/validate/", "USA/CO");
            Console.WriteLine("Result: " + result.StatusCode + " count - " + counter);
            counter++;
        }

        public async Task DoAll(IEnumerable<Action> actions, int maxTasks)
        {
            SemaphoreSlim semaphore = new SemaphoreSlim(maxTasks);

            foreach (var action in actions)
            {
                semaphore.WaitAsync().ConfigureAwait(false);
                Task.Factory.StartNew(() => action(), TaskCreationOptions.LongRunning)
                    .ContinueWith((task) => semaphore.Release());
            }

            for (int i = 0; i < maxTasks; i++)
                semaphore.WaitAsync().ConfigureAwait(false);
        }
    }
}
