using Microsoft.EntityFrameworkCore;
using ElearningPortal.Models;

namespace ElearningPortal.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions <ApplicationDbContext> options) : base(options)
    {

    }
    public DbSet<CourseDetails> courseDbSet{get; set;}

    public DbSet<UserDetails> traineeDbSet{get; set;}

    public DbSet<MentorDetails> mentorDbSet{get; set;}

    public DbSet<EnrollDetails> enrollDbSet{get; set;}

}
