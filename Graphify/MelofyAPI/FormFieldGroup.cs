using System;
using System.Collections.Generic;
using System.Text;

namespace MelofyAPI
{
    public class FormFieldGroup
    {
        private Dictionary<string, FormField> fields = new Dictionary<string, FormField>();
        private Dictionary<string, FormFieldGroup> groups = new Dictionary<string, FormFieldGroup>();
        private bool valid;
        private List<MessageResult> messages;

        public FormFieldGroup(params FormField[] fields)
        {
            this.valid = true;
            this.messages = new List<MessageResult>();

            foreach (FormField field in fields)
            {
                this.fields.Add(field.Key, field);
            }
        }

        public FormField GetFormField(string key)
        {
            return this.fields[key];
        }
        public FormFieldGroup GetFormFieldGroup(string key)
        {
            return this.groups[key];
        }
        public object GetValue(string fieldKey)
        {
            if (this.fields.ContainsKey(fieldKey))
                return this.fields[fieldKey].Value;

            return null;
        }

        public void AddField(FormField field)
        {
            this.fields.Add(field.Key, field);
        }
        public bool RemoveField(string key)
        {
            return this.fields.Remove(key);
        }

        public void AddGroup(string key, FormFieldGroup group)
        {
            this.groups.Add(key, group);
        }
        public bool RemoveGroup(string key)
        {
            return this.groups.Remove(key);
        }

        public void AddNormalization(NormalizeFn fn)
        {
            foreach (FormField field in fields.Values)
            {
                field.AddNormalization(fn);
            }

            foreach (FormFieldGroup group in groups.Values)
            {
                group.AddNormalization(fn);
            }
        }
        public void AddNormalization(NormalizeFn fn, Type type)
        {
            foreach (FormField field in fields.Values)
            {
                if (field.Value != null && field.Value.GetType() == type)
                    field.AddNormalization(fn);
            }

            foreach (FormFieldGroup group in groups.Values)
            {
                group.AddNormalization(fn, type);
            }
        }

        public void Normalize()
        {
            foreach (FormField field in fields.Values)
            {
                field.Normalize();
            }

            foreach (FormFieldGroup group in groups.Values)
            {
                group.Normalize();
            }
        }

        public bool Valid
        {
            get
            {
                foreach (FormField field in this.fields.Values)
                {
                    if (!field.Valid)
                    {
                        this.valid = false;
                        this.messages.Add(field.ErrorMsg);
                    }
                }

                foreach (FormFieldGroup group in this.groups.Values)
                {
                    if (!group.Valid)
                    {
                        this.valid = false;
                        this.messages.AddRange(group.Messages);
                    }
                }

                return this.valid;
            }
        }
        public List<MessageResult> Messages
        {
            get { return this.messages; }
        }

    }
}
