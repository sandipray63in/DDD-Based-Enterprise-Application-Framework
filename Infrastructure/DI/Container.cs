using System;
using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Infrastructure.DI
{
    /// <summary>
    /// A singleton wrapper class over MS Unity Container.
    /// For Singleton use-cases, refer - http://codeofdoom.com/wordpress/2008/04/20/how-and-when-to-use-singleton-classes/ 
    /// and http://programmers.stackexchange.com/questions/235527/when-to-use-a-singleton-and-when-to-use-a-static-class.
    /// For the Multiton pattern refer - http://gen5.info/q/2008/07/25/the-multiton-design-pattern/.
    /// </summary>
    public class Container
    {
        private const string SECTION_NAME = "unity";
        // Lazy initialization inherently implements the double-checked locking pattern
        private static readonly Lazy<IUnityContainer> _container = new Lazy<IUnityContainer>(() => new UnityContainer(),true);

        static Container()
        {
            var unitySection = ConfigurationManager.GetSection(SECTION_NAME) as UnityConfigurationSection;
            if (unitySection.IsNotNull())
            {
                unitySection.Containers.ForEach(c => 
                    {
                        _container.Value.LoadConfiguration(c.Name);
                    }
                 );
            }
        }

        private Container()
        {
            
        }

        public static IUnityContainer Instance { get; } = _container.Value;

    }
}
