using Models;
using Moq;
using Spackle;

namespace Test;

[TestFixture]
public class Tests
{
    Assignments assignments;
    
    [SetUp]
    public void Setup()
    {
        List<Assignment> mockedValues =
        [
            new Assignment("CS101", "Setup Python", "google.com", DateTime.Now),
            new Assignment("CS101", "Break Python", "google.com", DateTime.Now.AddDays(1)),
            new Assignment("CS101", "Try to Fix Python", "google.com", DateTime.Now.AddDays(2)),
            new Assignment("CS101", "Cry", "google.com", DateTime.Now.AddDays(3))
        ];
        
        var mock = new Mock<Assignments>();
        
        mock.Setup(x => x.GetAssignments()).Returns(mockedValues);
    }
    
    [Test]
    public void Test1()
    {
        var spackle = new Spackler();
        spackle.Main();
    }
}