using job;

using job2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace job2.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
       

        public HomeController(ApplicationContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.announcements.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(announcement announcement)
        {
            
                db.announcements.Add(announcement);
                await db.SaveChangesAsync();

            
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id!=null)
            {
                var details = db.announcements.SingleOrDefault(i => i.Id == id);
                if (details!=null)                
                    return View(details);
                
              
            }
            return NotFound();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var details = db.announcements.SingleOrDefault(i => i.Id == id);
                if (details != null)
                    return View(details);


            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(announcement announcement)
        {
            db.announcements.Update(announcement);
          await  db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id!=null)
            {
                db.announcements.Remove(db.announcements.SingleOrDefault(i=>i.Id==id));
                await db.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            else           
                return NotFound();
            
        } 
       public bool foo(params string[][] arr)
        {
            var temp = new List<string>() { };
            temp.AddRange(arr[0]);
            temp.AddRange(arr[ 1]);
            temp = temp.Distinct().ToList();
            return temp.Count < arr[0].Length + arr[1].Length;
          

        }
        public async Task<IActionResult> Similar(/*List<List<announcement>> announcements*/)
        {
            var announcements = db.announcements.ToList();
            var announcementsForSite = new List<List<announcement>>();
            var arr = db.announcements.ToList().Select(i=>i.Title.Split(' ')).ToList();
            var arr1 = db.announcements.ToList().Select(i=>i.Description.Split(' ')).ToList();


            for (int j = 0; 0< arr.Count; )
            {
                var temp = new List<announcement>();
                temp.Add(announcements[j]);
                for (int i = j+1; i < arr.Count ; i++)
                {

                    if (foo(arr[j],arr[i])&&foo(arr1[j],arr1[i]))
                    {

                        temp.Add(announcements[i]);
                        arr.RemoveAt(i);
                        arr1.RemoveAt(i);

                        announcements.RemoveAt(i);
                        i--;
                    }
                }
                announcementsForSite.Add(temp);
                arr.RemoveAt(j);
                arr1.RemoveAt(j);

                announcements.RemoveAt(j);

            }

            return View(announcementsForSite);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
