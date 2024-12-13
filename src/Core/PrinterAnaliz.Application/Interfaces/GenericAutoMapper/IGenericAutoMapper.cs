namespace PrinterAnaliz.Application.Interfaces.GenericAutoMapper
{
    public interface IGenericAutoMapper
    {
        TDestination Map<TDestination, TSource>(TSource source, string? ignore = null, bool setNull = true);
        IList<TDestination> Map<TDestination, TSource>(IList<TSource> source, string? ignore = null, bool setNull = true);
        TDestination Map<TDestination, TSource>(TSource source, TDestination destination, string? ignore = null, bool setNull = true);
        IList<TDestination> Map<TDestination, TSource>(IList<TSource> source, IList<TDestination> destination, string? ignore = null, bool setNull = true);
        TDestination Map<TDestination>(object source, string? ignore = null, bool setNull = true );
        IList<TDestination> Map<TDestination>(IList<object> source, string? ignore = null, bool setNull = true);
    }
}
