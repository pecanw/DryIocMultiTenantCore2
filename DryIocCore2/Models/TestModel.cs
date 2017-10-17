namespace DryIocCore2.Models
{
    public class TestModel
    {
        public string Global { get; set; }
        public string Tenant { get; set; }
        public string Request { get; set; }
        public string Transient { get; set; }
        public string Dependent { get; set; }
    }

    public class TenantTestModel
    {
        public TenantTestModel()
        {
            Services = new TestModel();
        }

        public TenantTestModel(string tenant, TestModel services)
        {
            Tenant = tenant;
            Services = services;
        }

        public string Tenant { get; set; }

        public TestModel Services { get; }
    }
}