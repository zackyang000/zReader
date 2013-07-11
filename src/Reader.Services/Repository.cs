using System.ComponentModel.Composition;
using Newegg.Contract.Infrastructure;
using Newegg.Contract.Model;

namespace Newegg.Contract.ServiceProxy
{
    public static class Repository
    {
        public static GuidRepository<Rule> Rule
        {
            get { return InstanceLocator.Current.GetInstance<GuidRepository<Rule>>(); }
        }

        public static GuidRepository<Role> Role
        {
            get { return InstanceLocator.Current.GetInstance<GuidRepository<Role>>(); }
        }

        public static GuidRepository<User> User
        {
            get { return InstanceLocator.Current.GetInstance<GuidRepository<User>>(); }
        }

        public static GuidRepository<Template> Template
        {
            get { return InstanceLocator.Current.GetInstance<GuidRepository<Template>>(); }
        }

        public static GuidRepository<Document> Document
        {
            get { return InstanceLocator.Current.GetInstance<GuidRepository<Document>>(); }
        }

        public static GuidRepository<DocumentConfig> DocumentConfig
        {
            get { return InstanceLocator.Current.GetInstance<GuidRepository<DocumentConfig>>(); }
        }

        public static GuidRepository<Condition> Condition
        {
            get { return InstanceLocator.Current.GetInstance<GuidRepository<Condition>>(); }
        }

        public static GuidRepository<ContractInfo> Contract
        {
            get { return InstanceLocator.Current.GetInstance<GuidRepository<ContractInfo>>(); }
        }

        public static GuidRepository<Event> Event
        {
            get { return InstanceLocator.Current.GetInstance<GuidRepository<Event>>(); }
        }
    }
}