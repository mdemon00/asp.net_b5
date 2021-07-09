using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.Data;
using WebProject.Training.Context;
using WebProject.Training.Entities;

namespace WebProject.Training.Repositories
{
    public class StudentRepository : Repository<Student, int>,
        IStudentRepository
    {

        public StudentRepository(ITrainingContext context)
            : base((DbContext)context)
        {

        }
    }
}
