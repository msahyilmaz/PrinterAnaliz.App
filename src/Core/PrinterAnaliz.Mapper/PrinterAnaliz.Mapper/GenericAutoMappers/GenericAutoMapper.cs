using AutoMapper;
using AutoMapper.Internal;
using PrinterAnaliz.Application.Interfaces.GenericAutoMapper;

namespace PrinterAnaliz.Mapper.GenericAutoMappers
{
    public class GenericAutoMapper : IGenericAutoMapper
    {
        public static List<TypePair> typePairs = new List<TypePair>();
        private IMapper MapperContainer;

        public TDestination Map<TDestination, TSource>(TSource source, string? ignore = null, bool setNull = true)
        {
            Config<TDestination, TSource>(5, ignore, setNull);
            return MapperContainer.Map<TSource, TDestination>(source);
        }

        public IList<TDestination> Map<TDestination, TSource>(IList<TSource> source, string? ignore = null, bool setNull = true)
        {
            Config<TDestination, TSource>(5, ignore, setNull);
            return MapperContainer.Map<IList<TSource>, IList<TDestination>>(source);
        }
        public TDestination Map<TDestination, TSource>(TSource source, TDestination destination, string? ignore = null, bool setNull = true)
        {
            Config<TDestination, TSource>(5, ignore, setNull);
            return MapperContainer.Map<TSource, TDestination>(source, destination);
        }

        public IList<TDestination> Map<TDestination, TSource>(IList<TSource> source, IList<TDestination> destination, string? ignore = null, bool setNull = true)
        {
            Config<TDestination, TSource>(5, ignore, setNull);
            return MapperContainer.Map<IList<TSource>, IList<TDestination>>(source, destination);
        }
        public TDestination Map<TDestination>(object source, string? ignore = null, bool setNull = true)
        {
            Config<TDestination, object>(5, ignore, setNull);
            return MapperContainer.Map<TDestination>(source);
        }

        public IList<TDestination> Map<TDestination>(IList<object> source, string? ignore = null, bool setNull = true)
        {
            Config<TDestination, IList<object>>(5, ignore, setNull);
            return MapperContainer.Map<IList<TDestination>>(source);
        }

        protected void Config<TDestination, TSourcde>(int depth = 5, string ignore = null, bool setNull = true)
        {
            var typePair = new TypePair(typeof(TSourcde), typeof(TDestination));
            if (typePairs.Any(a => a.DestinationType == typePair.DestinationType && a.SourceType == typePair.SourceType) && ignore == null)
                return;

            typePairs.Add(typePair);

            var config = new MapperConfiguration(cfg =>
            {

                foreach (var pair in typePairs)
                {
                    if (ignore is not null)
                        cfg.CreateMap(pair.SourceType, pair.DestinationType).MaxDepth(depth).ForMember(ignore, x => x.Ignore())//.ReverseMap() 
                        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                    else
                        cfg.CreateMap(pair.SourceType, pair.DestinationType).MaxDepth(depth)//.ReverseMap()
                         .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                }
            });
            MapperContainer = config.CreateMapper();
        }
    }
}
