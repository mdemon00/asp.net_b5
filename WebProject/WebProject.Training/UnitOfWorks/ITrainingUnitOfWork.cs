using WebProject.Data;
using WebProject.Training.Repositories;

namespace WebProject.Training.UnitOfWorks
{
    public interface ITrainingUnitOfWork : IUnitOfWork
    {
        IStudentRepository Students { get; }
        ICourseRepository Courses { get; }
    }
}