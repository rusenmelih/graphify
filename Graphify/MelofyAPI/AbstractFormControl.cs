using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MelofyAPI
{
    public delegate bool MelofyFn(AbstractFormControl control);
    public delegate object NormalizeFn(object value);

    public abstract class AbstractFormControl
    {
        protected object value;
        protected List<Melofy> melofies;
        protected List<NormalizeFn> normalizes;
        protected bool valid = true;
        protected string key;

        public AbstractFormControl(string key)
        {
            this.melofies = new List<Melofy>();
            this.normalizes = new List<NormalizeFn>();
            this.key = key;
        }
        public AbstractFormControl(string key, object value, params Melofy[] melofies)
        {
            this.key = key;
            this.value = value;
            this.melofies = melofies.ToList();
            this.normalizes = new List<NormalizeFn>();
        }

        protected abstract void Validate();
        public abstract void Normalize();

        public object Value
        {
            get { return this.value; }
        }
        public bool Valid
        {
            get 
            {
                this.Validate();
                return this.valid; 
            }
        }

        public string Key
        {
            get { return this.key; }
        }

        public abstract void AddMelofy(Melofy melofy);
        public abstract List<Melofy> Melofies { get; }
        public abstract void RemoveMelofy(string fnName);
    }
}
