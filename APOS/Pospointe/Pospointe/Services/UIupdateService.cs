using Pospointe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pospointe.Services
{
    class UIupdateService
    {
        public static string GetButtonName(string data)
        {

            using (var context = new PosDb1Context())


            {
                try
                {
                    string input = data;

                    string[] parts = input.Split(':');

                    // Assign to individual variables if needed
                    string part1 = parts[0]; // "DEPT"
                    string part2 = parts[1]; // "ITEM1"

                    if (part1 == "XITEM")
                    {
                        try
                        {
                            //var item = context.TblItems.Where(x => x.ItemId == part2).FirstOrDefault();
                            //if (item == null)
                            //{
                            //    MessageBox.Show("Item Not Found");
                            //}

                            //return item.ItemName;
                        }
                        catch { }
                    }

                    else if (part1 == "XDEPT")
                    {
                        //try
                        //{
                        //    var dept = context.TblDepartments.Where(x => x.DeptId == part2).FirstOrDefault();
                        //if (dept == null)
                        //{
                        //    MessageBox.Show("Department Not Found");
                        //}
                        //return dept.DeptName;
                        //}
                        //catch { }
                    }

                    return "Custom Button";
                }
                catch
                {
                    return "Custom Button";
                }
            }
           
            }


        
    }
}
