using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace URCP.SqlServerRepository
{
    public static class Extenstions
    {
        /// <summary>
        /// Extension method for map private properties
        /// <example>
        /// modelBuilder.Entity{Customer}()
        ///             .Property{Customer,int}("Age")
        ///             .IsOptional()
        /// </example>
        /// </summary>
        /// <typeparam name="TEntityType">The type of entity to map</typeparam>
        /// <typeparam name="KProperty">The type of private property to map</typeparam>
        /// <param name="entityConfiguration">Asociated EntityTypeConfiguration</param>
        /// <param name="propertyName">The name of private property</param>
        /// <returns>A PrimitivePropertyConfiguration for this map</returns>
        public static PrimitivePropertyConfiguration Property<TEntityType, KProperty>(this EntityTypeConfiguration<TEntityType> entityConfiguration, string propertyName)
            where TEntityType : class
            where KProperty : struct
        {

            var propertyInfo = typeof(TEntityType).GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (propertyInfo != null) // if private property exists
            {
                ParameterExpression arg = Expression.Parameter(typeof(TEntityType), "parameterName");
                MemberExpression memberExpression = Expression.Property((Expression)arg, propertyInfo);

                //Create the expression to map
                Expression<Func<TEntityType, KProperty>> expression = (Expression<Func<TEntityType, KProperty>>)Expression.Lambda(memberExpression, arg);


                return entityConfiguration.Property(expression);
            }
            else
                throw new InvalidOperationException("The property not exist");
        }

        public static ManyNavigationPropertyConfiguration<TEntityType, TTargetEntity> HasMany<TEntityType, TTargetEntity>(this EntityTypeConfiguration<TEntityType> configuration, string propertyName)
            where TEntityType : class // parent
            where TTargetEntity : class // child
        {

            var propertyInfo = typeof(TEntityType).GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (propertyInfo != null) // if private property exists
            {
                ParameterExpression arg = Expression.Parameter(typeof(TEntityType), "parameterName");
                MemberExpression memberExpression = Expression.Property((Expression)arg, propertyInfo);

                //Create the expression to map
                Expression<Func<TEntityType, ICollection<TTargetEntity>>> expression = (Expression<Func<TEntityType, ICollection<TTargetEntity>>>)Expression.Lambda(memberExpression, arg);


                return configuration.HasMany(expression);
            }
            else
                throw new InvalidOperationException("The property not exist");
        }
    }
}
