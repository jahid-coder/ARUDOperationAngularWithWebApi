using EmployeeWebApiApp.Data;
using EmployeeWebApiApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly UserDbContext _context;

        public EmployeeController(UserDbContext userDbContext)
        {
            _context = userDbContext;
        }

        [HttpPost("add_employee")]
        public IActionResult AddEmployee([FromBody] EmployeeModel employeeObj )
        {
            if(employeeObj == null)
            {
                return BadRequest();
            }
            else
            {
                _context.employeeModel.Add(employeeObj);
                _context.SaveChanges();
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Employee Add Successfully "
                });
            }

        }

        [HttpPut("update_employee")]
        public IActionResult UpdateEmployee([FromBody] EmployeeModel employeeObj)
        {
            if (employeeObj== null)
            {
                return BadRequest();
            }
            else
            {
                var user = _context.employeeModel.AsNoTracking().FirstOrDefault(x => x.Id == employeeObj.Id);

                if(user == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "User not fond",
                    });
                }
                else
                {
                    _context.Entry(employeeObj).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Employee Updated Successfully"
                    });
                 }
                
            }
        }

        [HttpDelete("delete_employee/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var user = _context.employeeModel.Find(id);
            if(user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User not found",
                });
            }
            else
            {
                _context.employeeModel.Remove(user);
                _context.SaveChanges();
                return Ok(new
                {
                    status = 200,
                    Message = "Employee is deleted Sucessfully",
                });


            }
        }

        [HttpGet("get_all_employees")]

        public IActionResult GetAllEmployees()
        {
            var employees = _context.employeeModel.AsQueryable();

            return Ok(new
            {
                StatusCode = 200,
                EmployeeDetails = employees,
            }); 

        }

        [HttpGet("get_employee/id")]

        public IActionResult GetEmployee(int id)
        {
            var employee = _context.employeeModel.Find(id);
            if(employee == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User not found"
                });

            }
            else
            {
                return Ok(new
                {
                    StatusCode = 200,
                    EmployeeDetail = employee,
                });
               
            }
        }


    }
}
