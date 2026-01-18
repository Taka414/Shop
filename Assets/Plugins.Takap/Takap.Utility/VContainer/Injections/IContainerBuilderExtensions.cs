//
// (C) 2025 Takap.
//

using VContainer;
using VContainer.Unity;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="IContainerBuilder"/> を拡張します。
    /// </summary>
    public static class IContainerBuilderExtensions
    {
        public static RegistrationBuilder RegisterComponentInHierarchy<TImpl, T1>(this IContainerBuilder builder, bool includeSelf)
            where TImpl : UnityEngine.Component, T1
            where T1 : class
        {
            return includeSelf
                ? builder.RegisterComponentInHierarchy<TImpl>().As<TImpl>().As<T1>()
                : builder.RegisterComponentInHierarchy<TImpl>().As<T1>();
        }

        public static RegistrationBuilder RegisterComponentInHierarchy<TImpl, T1, T2>(this IContainerBuilder builder, bool includeSelf)
            where TImpl : UnityEngine.Component, T1, T2
            where T1 : class
            where T2 : class
        {
            return includeSelf
                ? builder.RegisterComponentInHierarchy<TImpl>().As<TImpl>().As<T1>().As<T2>()
                : builder.RegisterComponentInHierarchy<TImpl>().As<T1>().As<T2>();
        }

        public static RegistrationBuilder RegisterComponentInHierarchy<TImpl, T1, T2, T3>(this IContainerBuilder builder, bool includeSelf)
            where TImpl : UnityEngine.Component, T1, T2, T3
            where T1 : class
            where T2 : class
            where T3 : class
        {
            return includeSelf
                ? builder.RegisterComponentInHierarchy<TImpl>().As<TImpl>().As<T1>().As<T2>().As<T3>()
                : builder.RegisterComponentInHierarchy<TImpl>().As<T1>().As<T2>().As<T3>();
        }
    }
}
