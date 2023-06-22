using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MelofyAPI
{
    public class FormField : AbstractFormField
    {
        public FormField(string key)
            : base(key)
        {

        }
        public FormField(string key, object property, params Melofy[] melofies)
            : base(key)
        {
            if (property == null)
                throw new AbstractFormFieldError("Property must be in component type.");
            else
            {
                Type type = property.GetType();
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Component<>))
                {
                    base.melofies = (melofies != null) ? (melofies.ToList()) : (new List<Melofy>());
                    base.code = (string)type.GetProperty("Code").GetValue(property);
                    base.value = type.GetProperty("Content").GetValue(property);
                }
                else
                    throw new AbstractFormFieldError("Property must be in component type.");
            }
        }

        protected override void Validate()
        {
            foreach (Melofy melofy in melofies)
            {

                if (!melofy.MelofyFn(this))
                {
                    base.message = new MessageResult(false, base.code, melofy.ErrorMsg);
                    base.valid = false;
                    return;
                }
            }

            base.valid = true;
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
        public FormField AddNormalization(NormalizeFn fn)
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
