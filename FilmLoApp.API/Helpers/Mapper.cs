using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLoApp.API.Helpers
{
    public static class Mapper
    {
        //mapira npr Usera u UserModel, to se pise u <>, znaci User je ulazni parametar, a povr je UserModel
        public static TDestination AutoMap<TSource, TDestination>(TSource source)
            where TDestination : class
            where TSource : class
        {
            var config = new MapperConfiguration(c => c.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            return mapper.Map<TDestination>(source);
        }

        // ovde dodati jos mapera po potrebi
    }
}
