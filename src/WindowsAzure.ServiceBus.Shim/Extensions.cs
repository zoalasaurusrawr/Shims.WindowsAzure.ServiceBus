namespace Microsoft.ServiceBus;
internal static class Extensions
{
    public static TTarget? CopyFrom<TTarget, TSource>(this TTarget target, TSource source)
        where TTarget : class, TSource
    {
        if (target == null || source == null)
            return default(TTarget);

        var tprops = target.GetType().GetProperties();

        tprops.Where(x => x.CanWrite == true).ToList().ForEach(prop =>
        {
            // check whether source object has the the property
            var sp = source.GetType().GetProperty(prop.Name);
            if (sp != null)
            {
                // if yes, copy the value to the matching property
                var value = sp.GetValue(source, null);
                var tprop = target.GetType().GetProperty(prop.Name);

                if (tprop != null)
                    tprop.SetValue(target, value, null);
            }
        });

        return target;
    }
}