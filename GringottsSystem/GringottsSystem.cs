using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GringottsSystem
{
    class GringottsSystem
    {
        static void Main(string[] args)
        {
            var context = new GringottsContext();

            var wiz = context.WizzardDeposits.Where(w => w.MagicWandCreator == "Ollivander family").Select(d => new { d.DepositGroup, d.DepositAmount}).ToList();

            foreach (var w in wiz)
            {
                foreach (var d in w.DepositGroup)
                {
                    d.s
                }
                Console.WriteLine($"{w.DepositGroup} - {w.DepositAmount}");
            }
        }
    }
}
