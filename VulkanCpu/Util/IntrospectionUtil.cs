/*
MIT License

Copyright (c) 2019 Jose Ferreira (Bazoocaze)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace VulkanCpu.Util
{
	public static class IntrospectionUtil
	{
		// -------------------------------------------------
		// ----- CONCRETE ACTION AND FUNCS FOR METHODS -----
		// -------------------------------------------------


		public static Action GetAction(object instance, string methodName)
		{
			return (Action)Delegate.CreateDelegate(typeof(Action), instance, methodName);
		}

		public static Action<TPar> GetAction<TPar>(object instance, string methodName)
		{
			return (Action<TPar>)Delegate.CreateDelegate(typeof(Action<TPar>), instance, methodName);
		}

		public static Func<TRet> GetFunc<TRet>(object instance, string methodName)
		{
			return (Func<TRet>)Delegate.CreateDelegate(typeof(Func<TRet>), instance, methodName);
		}

		public static Func<TPar, TRet> GetFunc<TPar, TRet>(object instance, string methodName)
		{
			return (Func<TPar, TRet>)Delegate.CreateDelegate(typeof(Func<TPar, TRet>), instance, methodName);
		}


		// ------------------------------------------------------------
		// ----- CONCRETE DELEGATES FOR FIELD READERS AND WRITERS -----
		// ------------------------------------------------------------


		public static Func<TRet> GetFieldReader<TRet>(object instance, string fieldName)
		{
			Expression source = Expression.Constant(instance);
			MemberExpression body = Expression.Field(source, fieldName);
			LambdaExpression lambda = Expression.Lambda(typeof(Func<TRet>), body);
			return (Func<TRet>)lambda.Compile();
		}

		public static Func<TRet> GetFieldReader<TRet>(object instance, FieldInfo field)
		{
			Expression source = Expression.Constant(instance);
			MemberExpression body = Expression.Field(source, field);
			LambdaExpression lambda = Expression.Lambda(typeof(Func<TRet>), body);
			return (Func<TRet>)lambda.Compile();
		}

		public static Action<TPar> GetFieldWriter<TPar>(object instance, FieldInfo field)
		{
			ParameterExpression parameter = Expression.Parameter(field.FieldType, "value");
			Expression source = Expression.Constant(instance);
			MemberExpression body = Expression.Field(source, field);
			LambdaExpression lambda = Expression.Lambda(typeof(Action<TPar>), body, parameter);
			return (Action<TPar>)lambda.Compile();
		}

		public static Action<TPar> GetFieldWriter<TPar>(object instance, string fieldName)
		{
			Expression source = Expression.Constant(instance);
			MemberExpression body = Expression.Field(source, fieldName);
			ParameterExpression parameter = Expression.Parameter(body.Type, "value");
			LambdaExpression lambda = Expression.Lambda(typeof(Action<TPar>), body, parameter);
			return (Action<TPar>)lambda.Compile();
		}


		// -------------------------------------------------------
		// ----- EXPRESSIONS FOR FIELDS, VECTORS AND METHODS ----- 
		// -------------------------------------------------------


		private static Expression GetInstanceOrStaticExpression(object instance)
		{
			if (instance == null)
				return null;
			return Expression.Constant(instance);
		}

		public static MemberExpression GetFieldExpression(Type classType, string fieldName)
		{
			return Expression.Field(null, classType, fieldName);
		}

		public static MemberExpression GetFieldExpression(object instance, string fieldName)
		{
			return Expression.Field(Expression.Constant(instance), instance.GetType(), fieldName);
		}

		public static MemberExpression GetFieldExpression(object instance, Type instanceType, string fieldName)
		{
			return Expression.Field(GetInstanceOrStaticExpression(instance), instanceType, fieldName);
		}

		public static MemberExpression GetFieldExpression(object instance, FieldInfo fieldInfo)
		{
			return Expression.Field(GetInstanceOrStaticExpression(instance), fieldInfo);
		}

		public static IndexExpression GetArrayAccessExpression(Expression array, Expression index)
		{
			return Expression.ArrayAccess(array, index);
		}

		public static IndexExpression GetArrayAccessExpression(object instance, string fieldName, Expression index)
		{
			var arrayExpression = GetFieldExpression(instance, fieldName);
			return Expression.ArrayAccess(arrayExpression, index);
		}

		public static MethodCallExpression GetMethodCallExpression(object instance, string methodName, params Expression[] parameters)
		{
			return BaseGetGenericMethodCallExpression(instance, instance.GetType(), methodName, null, parameters);
		}

		public static MethodCallExpression GetMethodCallExpression(Type instanceType, string methodName, params Expression[] parameters)
		{
			return BaseGetGenericMethodCallExpression(null, instanceType, methodName, null, parameters);
		}

		public static MethodCallExpression GetMethodCallExpression(object instance, Type instanceType, string methodName, params Expression[] parameters)
		{
			return BaseGetGenericMethodCallExpression(instance, instanceType, methodName, null, parameters);
		}

		public static MethodCallExpression GetGenericMethodCallExpression(object instance, string methodName, Type[] genericTypes, params Expression[] parameters)
		{
			return BaseGetGenericMethodCallExpression(instance, instance.GetType(), methodName, genericTypes, parameters);
		}

		public static MethodCallExpression GetGenericMethodCallExpression(Type instanceType, string methodName, Type[] genericTypes, params Expression[] parameters)
		{
			return BaseGetGenericMethodCallExpression(null, instanceType, methodName, genericTypes, parameters);
		}

		public static MethodCallExpression GetGenericMethodCallExpression(object instance, Type instanceType, string methodName, Type[] genericTypes, params Expression[] parameters)
		{
			return BaseGetGenericMethodCallExpression(instance, instanceType, methodName, genericTypes, parameters);
		}

		private static MethodCallExpression BaseGetGenericMethodCallExpression(object instance, Type instanceType, string methodName, Type[] genericTypes, params Expression[] parameters)
		{
			if (instance != null)
			{
				return Expression.Call(Expression.Constant(instance), methodName, genericTypes, parameters);
			}
			else
			{
				return Expression.Call(instanceType, methodName, genericTypes, parameters);
			}
		}

		public static Expression GetExpressionFor<T>(Expression<Func<T>> lambdaExpression)
		{
			return lambdaExpression.Body;
		}


		// --------------------------------------------
		// ----- CREATE DELEGATE FROM EXPRESSIONS ----- 
		// --------------------------------------------


		public static Action<T> CreateWriter<T>(Expression destination)
		{
			ParameterExpression param = Expression.Parameter(typeof(T), "value");
			BinaryExpression body = Expression.Assign(destination, param);
			LambdaExpression lambda = Expression.Lambda(typeof(Action<T>), body, param);
			return (Action<T>)lambda.Compile();
		}

		public static Func<T> CreateReader<T>(Expression source)
		{
			LambdaExpression lambda = Expression.Lambda(typeof(Func<T>), source);
			return (Func<T>)lambda.Compile();
		}

		public static Action CreateAssign(Expression destination, Expression source)
		{
			Expression body;
			if (source.Type != destination.Type)
			{
				body = Expression.Assign(destination, Expression.Convert(source, destination.Type));
			}
			else
			{
				body = Expression.Assign(destination, source);
			}
			LambdaExpression lambda = Expression.Lambda(typeof(Action), body);
			return (Action)lambda.Compile();
		}

		public static Action CreateCall(Expression call)
		{
			LambdaExpression lambda = Expression.Lambda(typeof(Action), call);
			return (Action)lambda.Compile();
		}

		public static Func<TRet> CreateFunc<TRet>(Expression call)
		{
			LambdaExpression lambda = Expression.Lambda(typeof(Func<TRet>), call);
			return (Func<TRet>)lambda.Compile();
		}

	}
}
