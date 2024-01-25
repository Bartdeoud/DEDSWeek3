using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Stapel<T>
    {
        public StapelObject<T> CurrentObject;

        public void AddToStapel(T objectToAdd)
        {
            StapelObject<T> stapelObject = new StapelObject<T>();
            stapelObject.PreviousObject = CurrentObject;
            stapelObject.MainObject = objectToAdd;
            CurrentObject = stapelObject;
        }

        public T TakeFromStapel() 
        {
            T returnValue = CurrentObject.MainObject;
            
            if(CurrentObject.PreviousObject == null)
                throw new NullReferenceException("De stapel is leeg");

            CurrentObject = CurrentObject.PreviousObject;
            return returnValue;
        }

        public class StapelObject<T>
        {
            public T MainObject { get; set; }
            public StapelObject<T> PreviousObject { get; set; }
        }
    }
}
