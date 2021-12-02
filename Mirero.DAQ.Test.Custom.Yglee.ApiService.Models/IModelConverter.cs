using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mirero.DAQ.Test.Custom.Yglee.ApiService.Models
{
    public interface IModelConverter
    {
        public object ConvertToModel(object obj);
        public object ConvertToDto(object obj);
    }

    public class ModelConverter : IModelConverter
    {
        private readonly string DtoNamespace = "Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.DTO";
        private readonly string EntityNamespace = "Mirero.DAQ.Test.Custom.Yglee.ApiService.Models.Entity";

        public object ConvertToModel(object obj)
        {
            Assembly creator = Assembly.GetExecutingAssembly();

            var type= obj.GetType();

            string typeString = GetEntityTypeString(type.ToString());
            
            Console.WriteLine(typeString);

            object result = creator.CreateInstance(typeString);

            var fields = obj.GetType().GetProperties();
            Console.WriteLine(fields.Length);

            foreach (var field in fields)
            {
                var propertyInfo = result.GetType().GetProperty(field.Name);
                propertyInfo.SetValue(result, field.GetValue(obj));
            }

            return result;
        }

        public object ConvertToDto(object obj)
        {
            Assembly creator = Assembly.GetExecutingAssembly();

            var type = obj.GetType();

            string typeString = GetEntityTypeString(type.ToString());

            Console.WriteLine(typeString);

            object result = creator.CreateInstance(typeString);

            var fields = obj.GetType().GetProperties();
            Console.WriteLine(fields.Length);

            foreach (var field in fields)
            {
                var propertyInfo = result.GetType().GetProperty(field.Name);
                propertyInfo.SetValue(result, field.GetValue(obj));
            }

            return result;
        }


        private string GetEntityTypeString(string objTypeString)
        {
            var names = objTypeString.Split(".");

            var typeName = names.Last().Replace("DTO", String.Empty);

            var space = names.ElementAt(names.Length - 2);

            return EntityNamespace + "." + space + "." + typeName;
        }

        private string GetDtoTypeString(string objTypeString)
        {
            var names = objTypeString.Split(".");

            var typeName = names.Last() + "DTO";

            var space = names.ElementAt(names.Length - 2);

            return DtoNamespace + "." + space + "." + typeName;
        }
    }
}
