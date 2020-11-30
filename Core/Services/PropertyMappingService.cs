using Domain;
using Models.SavedMovies;
using Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {

        #region PrivateFields

        //mapiranje, jer sort po godini znaci da sortiramo datum rodjenja entiteta npr... 
        private Dictionary<string, PropertyMappingValue> _userPropertyMapping =
          new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
          {
               { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
               { "Name", new PropertyMappingValue(new List<string>() { "Name"}) },
               { "Surname", new PropertyMappingValue(new List<string>() { "Surname"}) },
               { "Email", new PropertyMappingValue(new List<string>() { "Email"}) }

          };

        private Dictionary<string, PropertyMappingValue> _moviePropertyMapping =
          new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
          {
               { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
               { "Name", new PropertyMappingValue(new List<string>() { "Name"}) }
               //{ "UserId", new PropertyMappingValue(new List<string>() { "UserId"}) },
               //{ "Actors", new PropertyMappingValue(new List<string>() { "Actors"}) },
               //{ "Year", new PropertyMappingValue(new List<string>() { "Year"}) },
               //{ "Director", new PropertyMappingValue(new List<string>() { "Director"}) },
               //{ "Duration", new PropertyMappingValue(new List<string>() { "Duration"}) },
               //{ "Genre", new PropertyMappingValue(new List<string>() { "Genre"}) },
               //{ "Country", new PropertyMappingValue(new List<string>() { "Country"}) }


          };

        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        #endregion

        #region Constructors

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<UserModel, User>(_userPropertyMapping));
            _propertyMappings.Add(new PropertyMapping<AddSavedMovieModel, MovieJMDBApi>(_moviePropertyMapping));
        }

        #endregion

        #region Methods

        //provera da li postoji property po kom klijent zeli da sortira podatke, proveravamo
        //u kontroleru!!! ne zaboravi
        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the string is separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');

            // run through the fields clauses
            foreach (var field in fieldsAfterSplit)
            {
                // trim
                var trimmedField = field.Trim();

                // remove everything after the first " " - if the fields 
                // are coming from an orderBy string, this part must be 
                // ignored
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                // find the matching property
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }


        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
           <TSource, TDestination>()
        {
            // get matching mapping
            var matchingMapping = _propertyMappings
                .OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance " +
                $"for <{typeof(TSource)},{typeof(TDestination)}");
        }

        #endregion
    }
}
