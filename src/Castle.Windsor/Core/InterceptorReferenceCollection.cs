// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.Core
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	using Castle.DynamicProxy;
	using Castle.MicroKernel;

	/// <summary>
	///   Collection of <see cref = "InterceptorReference" />
	/// </summary>
#if !SILVERLIGHT
	[Serializable]
#endif
	public class InterceptorReferenceCollection : ICollection<InterceptorReference>
	{
		private readonly DependencyModelCollection dependencies;
		private readonly IList<InterceptorReference> list = new List<InterceptorReference>();

		public InterceptorReferenceCollection(DependencyModelCollection dependencies)
		{
			this.dependencies = dependencies;
		}

		/// <summary>
		///   Gets a value indicating whether this instance has interceptors.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance has interceptors; otherwise, <c>false</c>.
		/// </value>
		public bool HasInterceptors
		{
			get { return Count != 0; }
		}

		/// <summary>
		///   Gets the number of
		///   elements contained in the <see cref = "T:System.Collections.ICollection" />.
		/// </summary>
		/// <value></value>
		public int Count
		{
			get { return list.Count; }
		}

		bool ICollection<InterceptorReference>.IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		///   Adds the specified interceptor as the first.
		/// </summary>
		/// <param name = "item">The interceptor.</param>
		public void AddFirst(InterceptorReference item)
		{
			Insert(0, item);
		}

		/// <summary>
		///   Adds the interceptor to the end of the interceptors list if it does not exist already.
		/// </summary>
		/// <param name = "interceptorReference">The interceptor reference.</param>
		public void AddIfNotInCollection(InterceptorReference interceptorReference)
		{
			if (list.Contains(interceptorReference) == false)
			{
				AddLast(interceptorReference);
			}
		}

		/// <summary>
		///   Adds the specified interceptor as the last.
		/// </summary>
		/// <param name = "item">The interceptor.</param>
		public void AddLast(InterceptorReference item)
		{
			list.Add(item);
			Attach(item);
		}

		/// <summary>
		///   Inserts the specified interceptor at the specified index.
		/// </summary>
		/// <param name = "index">The index.</param>
		/// <param name = "item">The interceptor.</param>
		public void Insert(int index, InterceptorReference item)
		{
			list.Insert(index, item);
			Attach(item);
		}

		/// <summary>
		///   Adds the specified item.
		/// </summary>
		/// <param name = "item">The interceptor.</param>
		public void Add(InterceptorReference item)
		{
			AddLast(item);
		}

		public void Clear()
		{
			var references = list.ToArray();
			list.Clear();
			foreach (var reference in references)
			{
				Detach(reference);
			}
		}

		public bool Contains(InterceptorReference item)
		{
			return list.Contains(item);
		}

		public bool Remove(InterceptorReference item)
		{
			if (list.Remove(item))
			{
				Detach(item);
				return true;
			}
			return false;
		}

		/// <summary>
		///   Returns an enumerator that can iterate through a collection.
		/// </summary>
		/// <returns>
		///   An <see cref = "T:System.Collections.IEnumerator" />
		///   that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		private void Attach(IReference<IInterceptor> interceptor)
		{
			interceptor.Attach(dependencies);
		}

		private void Detach(IReference<IInterceptor> interceptor)
		{
			interceptor.Detach(dependencies);
		}

		void ICollection<InterceptorReference>.CopyTo(InterceptorReference[] array, int arrayIndex)
		{
			list.CopyTo(array, arrayIndex);
		}

		IEnumerator<InterceptorReference> IEnumerable<InterceptorReference>.GetEnumerator()
		{
			return list.GetEnumerator();
		}
	}
}