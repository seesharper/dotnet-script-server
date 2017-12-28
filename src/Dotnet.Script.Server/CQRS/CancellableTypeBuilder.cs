using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Dotnet.Script.Server.CQRS
{
    /// <summary>
    /// A class that is capable of building a parentType that implements <see cref="ICancellable"/>
    /// </summary>
    public class CancellableTypeBuilder : ICancellableTypeBuilder
    {
        private static readonly MethodInfo GetCancellationTokenMethod;
        private static readonly MethodInfo SetCancellationTokenMethod;

        static CancellableTypeBuilder()
        {
            GetCancellationTokenMethod = typeof(ICancellable).GetProperty("CancellationToken").GetGetMethod();
            SetCancellationTokenMethod = typeof(ICancellable).GetProperty("CancellationToken").GetSetMethod();
        }

        /// <summary>
        /// Creates a new <see cref="Type"/> that inherits from 
        /// </summary>
        /// <param name="parentType">The <see cref="Type"/> for which the new parentType inherits from.</param>
        /// <returns>A <see cref="Type"/> that inherits from <paramref name="parentType"/> and implements <see cref="ICancellable"/>.</returns>
        public Type CreateCancellableType(Type parentType)
        {
            var typeBuilder = CreateTypeBuilder(parentType);            
            ImplementICancellable(typeBuilder);
            return typeBuilder.CreateTypeInfo();
        }

        private static void ImplementICancellable(TypeBuilder typeBuilder)
        {
            typeBuilder.AddInterfaceImplementation(typeof(ICancellable));

            var cancellationTokenField =
                typeBuilder.DefineField("_cancellationToken", typeof(CancellationToken), FieldAttributes.Private);

            var setMethod = typeBuilder.DefineMethod("set_CancellationToken",
                SetCancellationTokenMethod.Attributes ^ MethodAttributes.Abstract, typeof(void), new[] { typeof(CancellationToken) });

            var setGenerator = setMethod.GetILGenerator();
            setGenerator.Emit(OpCodes.Ldarg_0);
            setGenerator.Emit(OpCodes.Ldarg_1);
            setGenerator.Emit(OpCodes.Stfld, cancellationTokenField);
            setGenerator.Emit(OpCodes.Ret);

            var getMethod = typeBuilder.DefineMethod("get_CancellationToken",
                GetCancellationTokenMethod.Attributes ^ MethodAttributes.Abstract, typeof(CancellationToken), Type.EmptyTypes);

            var getGenerator = getMethod.GetILGenerator();
            getGenerator.Emit(OpCodes.Ldarg_0);
            getGenerator.Emit(OpCodes.Ldfld, cancellationTokenField);
            getGenerator.Emit(OpCodes.Ret);
        }

        private static TypeBuilder CreateTypeBuilder(Type parentType)
        {
            AssemblyBuilder assemblyBuilder =
                AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("CancellationAssembly"), AssemblyBuilderAccess.Run);

            var module = assemblyBuilder.DefineDynamicModule("CancellationModule");

            var typeBuilder = module.DefineType($"Cancellable{parentType.Name}", parentType.Attributes, parentType);

            return typeBuilder;
        }
    }
}