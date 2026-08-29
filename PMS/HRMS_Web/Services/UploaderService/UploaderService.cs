using B_DB_Context;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Formats.Asn1;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace HRMS_Web.Services.UploaderService
{
    public class UploaderService : IUploaderService
    {
        private readonly DataBase_Context _dbContext;

        public UploaderService(DataBase_Context dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task ProcessCsvAsync(IFormFile file, string tableName)
        {
            var records = new List<object>();
            var modelType = GetModelTypeByName(tableName);

            if (modelType == null)
            {
                throw new InvalidOperationException("Invalid table name.");
            }

            using (var stream = file.OpenReadStream())
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.Context.TypeConverterCache.AddConverter<DateTime>(new FlexibleDateConverter());
                csv.Context.TypeConverterCache.AddConverter<DateTime?>(new FlexibleDateConverter());

                var map = GenerateCsvMapForModel(modelType);
                csv.Context.RegisterClassMap(map);


                records = csv.GetRecords(modelType).Cast<object>().ToList();


                records = records.Select(SanitizeModel).ToList();
            }

            const int batchSize = 999;
            for (int i = 0; i < records.Count; i += batchSize)
            {
                var batch = records.Skip(i).Take(batchSize).ToList();
                await InsertBatchAsync(batch, modelType);
            }
        }

        private async Task InsertBatchAsync(List<object> batch, Type modelType)
        {

            var dbSet = _dbContext.GetType()
                .GetProperties()
                .FirstOrDefault(p =>
                    p.PropertyType.IsGenericType &&
                    p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                    p.PropertyType.GenericTypeArguments[0] == modelType)
                ?.GetValue(_dbContext);

            if (dbSet == null)
            {
                throw new InvalidOperationException($"DbSet for type '{modelType.Name}' not found in the context.");
            }


            var addRangeMethod = dbSet.GetType()
                .GetMethod("AddRange", new[] { typeof(IEnumerable<>).MakeGenericType(modelType) });

            if (addRangeMethod == null)
            {
                throw new InvalidOperationException($"AddRange method for type '{modelType.Name}' not found.");
            }


            var listType = typeof(List<>).MakeGenericType(modelType);
            var genericBatch = (IList)Activator.CreateInstance(listType);

            foreach (var item in batch)
            {

                var typedItem = Convert.ChangeType(item, modelType);
                genericBatch.Add(typedItem);
            }


            addRangeMethod.Invoke(dbSet, new[] { genericBatch });
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (System.Exception)
            {

                throw;
            }


        }


        private Type GetModelTypeByName(string tableName)
        {
            return _dbContext.GetType()
                .GetProperties()
                .Where(p => p.PropertyType.IsGenericType &&
                            p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(p => p.PropertyType.GenericTypeArguments.FirstOrDefault())
                .FirstOrDefault(t => t.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase));
        }

        private object SanitizeModel(object model)
        {
            var properties = model.GetType().GetProperties();

            foreach (var prop in properties)
            {
                if (prop.CanWrite)
                {
                    var value = prop.GetValue(model);

                    if (prop.PropertyType == typeof(string))
                    {
                        var stringValue = value as string;

                        if (string.IsNullOrWhiteSpace(stringValue))
                        {
                            prop.SetValue(model, null);
                        }
                        //else if (IsDateLike(stringValue))
                        //{
                        //    if (DateTime.TryParse(stringValue, out var parsedDate))
                        //    {

                        //        prop.SetValue(model, parsedDate.ToString("yyyy-MM-dd"));
                        //    }
                        //}
                    }


                    if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                    {
                        if (value == null || (value is DateTime dateTime && dateTime == DateTime.MinValue))
                        {
                            prop.SetValue(model, new DateTime(2000, 1, 1));
                        }
                    }
                }
            }

            return model;
        }

        private bool IsDateLike(string value)
        {
            return DateTime.TryParse(value, out _);
        }

        private ClassMap GenerateCsvMapForModel(Type modelType)
        {
            var map = Activator.CreateInstance(typeof(DefaultClassMap<>).MakeGenericType(modelType)) as ClassMap;
            if (map == null) throw new InvalidOperationException($"Unable to create CSV map for type {modelType.Name}.");

            foreach (var property in modelType.GetProperties())
            {

                if (property.GetCustomAttribute<NotMappedAttribute>() != null)
                {
                    continue;
                }


                if (property.Name.EndsWith("Id") &&
                    property.GetCustomAttribute<ForeignKeyAttribute>() != null)
                {
                    map.Map(modelType, property);
                    continue;
                }


                if (property.GetCustomAttribute<ForeignKeyAttribute>() != null ||
                    (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string)) ||
                    (property.PropertyType.IsClass && property.PropertyType != typeof(string) && !property.PropertyType.IsPrimitive))
                {
                    map.Map(modelType, property).Ignore();
                }
                else
                {
                    map.Map(modelType, property);
                }
            }

            return map;
        }

        public class FlexibleDateConverter : CsvHelper.TypeConversion.DateTimeConverter
        {
            private readonly string[] _dateFormats = { "dd/MM/yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "dd-MM-yyyy" };

            public override object ConvertFromString(string text, IReaderRow row, CsvHelper.Configuration.MemberMapData memberMapData)
            {
                if (string.IsNullOrWhiteSpace(text))
                    return null;

                if (Regex.IsMatch(text, @"^\d{1,4}[-/]\d{1,2}[-/]\d{1,4}$"))
                {
                    if (DateTime.TryParseExact(text, _dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                    {
                        return date;
                    }
                }

                throw new CsvHelper.TypeConversion.TypeConverterException(
                    this,
                    memberMapData,
                    text,
                    row.Context,
                    $"Invalid date format: '{text}'. Supported formats: {string.Join(", ", _dateFormats)}.");
            }
        }

    }
}
