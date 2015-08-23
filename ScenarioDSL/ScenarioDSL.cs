using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScenarioDSL
{
    public interface IHave<T>
    {}

    public interface IScenario
    {}

    public class Scenario : IScenario, IHave<ArchetypeOf<Institution>>, IHave<ArchetypeOf<User>> 
    {
        private Scenario() { }
        public static IHave<T> With<T>() 
        {
            var scenario = new Scenario();
            return (IHave<T>)scenario;
        }
    }

    public static class Extensions
    {
        public static IHave<T> AndWith<T>(this IScenario scenario) 
        {
            return (IHave<T>) scenario;
        }

        public static IScenario AndWith<T>(this IScenario scenario, Action<T> customize)
        {
            return scenario;
        }

        public static IScenario HavingAnOrganizationWith(
            this IHave<ArchetypeOf<Institution>> scenarioInsitution, Levels levels) 
        {
            return (IScenario)scenarioInsitution;
        }
        public static ArchetypeOf<User> Having(this ArchetypeOf<User> user, IEnumerable<PermissionTo> permissions, Func<IEnumerable<OrganizationNode>, OrganizationNode> at)
        {
            return user;
        }

        public static Levels Levels(this int levels)
        {
            return new Levels(levels);
        }

        public static IEnumerable<PermissionTo> And(this PermissionTo permission, PermissionTo anotherPermission)
        {
            yield return permission;
            yield return anotherPermission;
        }

        public static IEnumerable<PermissionTo> AndAlso(this IEnumerable<PermissionTo> permissions, PermissionTo anotherPermission)
        {
            foreach (var permission in permissions)
                yield return permission;
            yield return anotherPermission;
        }

        public static ArchetypeOf<User> WithName(this ArchetypeOf<User> user, String name)
        {
            return user;
        }
    }

    public class Institution{}

    public class User
    {
        public User(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }

    public interface ArchetypeOf<T>
    {}

    public class Levels
    {
        private int _levels;

        public Levels(int levels)
        {
            _levels = levels;
        }
    }

    public class PermissionTo
    {
        public static readonly PermissionTo QueryTrends = new PermissionTo();
        public static readonly PermissionTo QueryUserOrganizations = new PermissionTo();
        public static readonly PermissionTo QueryTrendsHierarchy = new PermissionTo();
        private PermissionTo() { }
    }

    public class QueryTrends{}

    public class OrganizationNode{}

    public class MyScenario
    {
        public void MyMethod() 
        {
               Scenario
                    .With<ArchetypeOf<Institution>>()
                    .HavingAnOrganizationWith(3.Levels())
                    .AndWith<ArchetypeOf<User>>(_ => _
                                                     .WithName("Luis Moros")
                                                     .Having(
                                                                PermissionTo.QueryTrends
                                                                 .And(PermissionTo.QueryUserOrganizations)
                                                                 .AndAlso(PermissionTo.QueryTrendsHierarchy),
                                                                 at: organizations =>
                                                                        organizations.First()));
        }
    }
}
