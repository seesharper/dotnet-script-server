using System;
using System.Reflection.Emit;

namespace Dotnet.Script.Server.CQRS
{
    /// <summary>
    /// A class that is capable of dynamically creating 
    /// a method that performs object cloning.
    /// </summary>
    public class CloneMethodBuilder : ICloneMethodBuilder
    {
        /// <summary>
        /// Creates a dynamic method that is used to clone an object of 
        /// type <typeparamref name="T"/> into an instance of a <paramref name="derivedType"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to be cloned.</typeparam>
        /// <param name="derivedType">The type of the derived type to be returned from the clone operation.</param>
        /// <returns>A method capable of cloning an instance of <typeparamref name="T"/> to an instance of the <paramref name="derivedType"/>.</returns>
        public Func<T, T> CreateCloneMethod<T>(Type derivedType)
        {
            var constructor = derivedType.GetConstructor(Type.EmptyTypes);

            DynamicMethod dynamicMethod = new DynamicMethod("Clone", typeof(T), new[] { typeof(T) }, typeof(ObjectExtensions).Module);
            var ilGenerator = dynamicMethod.GetILGenerator();
            var properties = typeof(T).GetProperties();
            ilGenerator.Emit(OpCodes.Newobj, constructor);
            foreach (var propertyInfo in properties)
            {
                ilGenerator.Emit(OpCodes.Dup);
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Callvirt, propertyInfo.GetMethod);
                ilGenerator.Emit(OpCodes.Callvirt, propertyInfo.SetMethod);
            }
            ilGenerator.Emit(OpCodes.Ret);

            var del = (Func<T, T>)dynamicMethod.CreateDelegate(typeof(Func<T, T>));
            return del;
        }
    }
}