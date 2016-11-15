using Autofac;

namespace Common
{
    public class IoC
    {
        private static IContainer _container;

        public static IContainer Container => _container ?? (_container = new ContainerBuilder().Build());

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
