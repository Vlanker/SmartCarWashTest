using System;

namespace SmartCarWashTest.EventBus.Infos
{
    /// <summary>
    /// Subscription info.
    /// </summary>
    public class SubscriptionInfo
    {
        /// <summary>
        /// SubscriptionInfo is dynamic.
        /// </summary>
        public bool IsDynamic { get; }

        /// <summary>
        /// Handler type.
        /// </summary>
        public Type HandlerType { get; }

        /// <summary>
        /// .ctor.
        /// </summary>
        /// <param name="isDynamic"> SubscriptionInfo is dynamic. </param>
        /// <param name="handlerType"> Handler type. </param>
        private SubscriptionInfo(bool isDynamic, Type handlerType)
        {
            IsDynamic = isDynamic;
            HandlerType = handlerType;
        }

        /// <summary>
        /// Dynamic.
        /// </summary>
        /// <param name="handlerType"> Handler type. </param>
        /// <returns> <see cref="SubscriptionInfo"/>. </returns>
        public static SubscriptionInfo Dynamic(Type handlerType) => new(true, handlerType);

        /// <summary>
        /// Typed.
        /// </summary>
        /// <param name="handlerType"> Handler type. </param>
        /// <returns> <see cref="SubscriptionInfo"/>. </returns>
        public static SubscriptionInfo Typed(Type handlerType) => new(false, handlerType);
    }
}