using UnityEngine;

namespace ACFW.Controllers
{
    public abstract class AbstractGameObjectInstantiator<TResult> where TResult : Object
    {
        protected readonly TResult prefab;
        protected readonly Transform parent;
        public AbstractGameObjectInstantiator(TResult prefab, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;
        }

        protected abstract void Construct(TResult result);
        public TResult Create()
        {
            var result = GameObject.Instantiate<TResult>(prefab, parent);
            Construct(result);
            return result;
        }
    }
    public class AbstractGameObjectFactory<TResult> : AbstractGameObjectInstantiator<TResult>, IConstructableFactory<TResult> where TResult : Object, IConstructable
    {
        public AbstractGameObjectFactory(TResult prefab, Transform parent) : base(prefab, parent)
        {
        }
        protected override void Construct(TResult result)
        {
            result.Construct();
        }
    }

    public class AbstractGameObjectFactory<TResult, T1> : AbstractGameObjectInstantiator<TResult>, IConstructableFactory<TResult, T1> where TResult : Object, IConstructable<T1>
    {
        private readonly T1 arg1;
        public AbstractGameObjectFactory(TResult prefab, Transform parent, T1 arg1) : base(prefab, parent)
        {
            this.arg1 = arg1;
        }
        protected override void Construct(TResult result)
        {
            result.Construct(arg1);
        }
    }

    public class AbstractGameObjectFactory<TResult, T1, T2> : AbstractGameObjectInstantiator<TResult>, IConstructableFactory<TResult, T1, T2> where TResult : Object, IConstructable<T1, T2>
    {
        private readonly T1 arg1;
        private readonly T2 arg2;
        public AbstractGameObjectFactory(TResult prefab, Transform parent, T1 arg1, T2 arg2) : base(prefab, parent)
        {
            this.arg1 = arg1;
            this.arg2 = arg2;
        }
        protected override void Construct(TResult result)
        {
            result.Construct(arg1, arg2);
        }
    }

    public class AbstractGameObjectFactory<TResult, T1, T2, T3> : AbstractGameObjectInstantiator<TResult>, IConstructableFactory<TResult, T1, T2, T3> where TResult : Object, IConstructable<T1, T2, T3>
    {
        private readonly T1 arg1;
        private readonly T2 arg2;
        private readonly T3 arg3;
        public AbstractGameObjectFactory(TResult prefab, Transform parent, T1 arg1, T2 arg2, T3 arg3) : base(prefab, parent)
        {
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
        }
        protected override void Construct(TResult result)
        {
            result.Construct(arg1, arg2, arg3);
        }
    }

    public class AbstractGameObjectFactory<TResult, T1, T2, T3, T4> : AbstractGameObjectInstantiator<TResult>, IConstructableFactory<TResult, T1, T2, T3, T4> where TResult : Object, IConstructable<T1, T2, T3, T4>
    {
        private readonly T1 arg1;
        private readonly T2 arg2;
        private readonly T3 arg3;
        private readonly T4 arg4;
        public AbstractGameObjectFactory(TResult prefab, Transform parent, T1 arg1, T2 arg2, T3 arg3, T4 arg4) : base(prefab, parent)
        {
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
        }
        protected override void Construct(TResult result)
        {
            result.Construct(arg1, arg2, arg3, arg4);
        }
    }

    // fill in other generics from T5 to T9
}
