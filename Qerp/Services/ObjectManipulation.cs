using Newtonsoft.Json;
using System.Text.Json;

namespace Qerp.Services
{
    
    public class ObjectManipulation
    {

        static JsonSerializerOptions jsonOptions =
                    new()
                    {
                        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
                        WriteIndented = false,
                        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                        DefaultBufferSize = 200
    };

        public static T CastObject<T>(Object? input)
        {
            if (input == null) return default(T);

            try
            {



                //Type v = input.GetType();


                //var config = new MapperConfiguration(cfg => {
                //    cfg.CreateMap<v, T>();
                //});

                //CreateMap<>
                //T output = _mapper.Map<T>(input);
                //T test = (T)input;

                //return test;
                //return _mapper.Map<T>(input) ?? null;
                //return (T)input;
                string t = JsonConvert.SerializeObject(input, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,

                });
                return JsonConvert.DeserializeObject<T>(t);
            }
            catch(Exception ex)
            {
                return default(T);
            }
        }



    }
}
