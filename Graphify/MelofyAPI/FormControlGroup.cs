using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelofyAPI
{
    public class FormControlGroup
    {
        private Dictionary<string, FormControl> controls = new Dictionary<string, FormControl>();
        private Dictionary<string, FormControlGroup> groups = new Dictionary<string, FormControlGroup>();
        private bool valid;
        private List<string> messages;

        public FormControlGroup(params FormControl[] controls)
        {
            this.valid = true;
            this.messages = new List<string>();

            foreach (FormControl control in controls)
            {
                this.controls.Add(control.Key, control);
            }
        }

        public FormControl GetFormControl(string key)
        {
            return this.controls[key];
        }
        public FormControlGroup GetFormControlGroup(string key)
        {
            return this.groups[key];
        }
        public object GetValue(string controlKey)
        {
            if (this.controls.ContainsKey(controlKey))
                return this.controls[controlKey].Value;

            return null;
        }

        public void AddField(FormControl control)
        {
            this.controls.Add(control.Key, control);
        }
        public bool RemoveField(string key)
        {
            return this.controls.Remove(key);
        }

        public void AddGroup(string key, FormControlGroup group)
        {
            this.groups.Add(key, group);
        }
        public bool RemoveGroup(string key)
        {
            return this.groups.Remove(key);
        }

        public void AddNormalization(NormalizeFn fn)
        {
            foreach (FormControl control in controls.Values)
            {
                control.AddNormalization(fn);
            }

            foreach (FormControlGroup group in groups.Values)
            {
                group.AddNormalization(fn);
            }
        }
        public void AddNormalization(NormalizeFn fn, Type type)
        {
            foreach (FormControl control in controls.Values)
            {
                if (control.Value != null && control.Value.GetType() == type)
                    control.AddNormalization(fn);
            }

            foreach (FormControlGroup group in groups.Values)
            {
                group.AddNormalization(fn, type);
            }
        }

        public void Normalize()
        {
            foreach (FormControl control in controls.Values)
            {
                control.Normalize();
            }

            foreach (FormControlGroup group in groups.Values)
            {
                group.Normalize();
            }
        }

        public bool Valid
        {
            get
            {
                foreach (FormControl control in controls.Values)
                {
                    if (!control.Valid)
                    {
                        this.valid = false;
                        this.messages.Add(control.Message);
                    }
                }

                foreach (FormControlGroup group in this.groups.Values)
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
        public List<string> Messages
        {
            get { return this.messages; }
        }
    }
}
