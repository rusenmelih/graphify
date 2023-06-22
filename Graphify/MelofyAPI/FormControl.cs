using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MelofyAPI
{
    public class FormControl : AbstractFormControl //Yabani Köpek = string email
    {
        private string message;
        public FormControl(string key)
            : base(key)
        {


        }
        public FormControl(string key, object property, params Melofy[] melofies)
            : base(key)
        {
            base.value = property;
            base.melofies = (melofies != null) ? (melofies.ToList()) : (new List<Melofy>());
        }

        protected override void Validate()
        {
            foreach (Melofy melofy in melofies)
            {

                if (!melofy.MelofyFn(this))
                {
                    this.message = melofy.ErrorMsg;
                    base.valid = false;
                    return;
                }
            }

            base.valid = true;
        }

        public string Message
        {
            get { return this.message; }
        }

        public override void Normalize()
        {
            foreach (NormalizeFn fn in normalizes)
            {
                value = fn(value);
            }
        }
        public override void AddMelofy(Melofy melofy)
        {
            base.melofies.Add(melofy);
        }
        public override List<Melofy> Melofies
        {
            get { return base.melofies; }
        }
        public override void RemoveMelofy(string fnName)
        {
            foreach (Melofy melofy in melofies)
            {
                if (melofy.MelofyFn.Method.Name == fnName)
                {
                    base.melofies.Remove(melofy);
                    return;
                }
            }
        }
        public FormControl AddNormalization(NormalizeFn fn)
        {
            base.normalizes.Add(fn);
            return this;
        }
        public void RemoveNormalization(string fnName)
        {
            foreach (NormalizeFn fn in normalizes)
            {
                if (fn.Method.Name == fnName)
                {
                    base.normalizes.Remove(fn);
                    return;
                }
            }
        }
    }
}
