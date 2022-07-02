using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimonSpedsbjerg.Classes {
    public class Repo {
        public string Name { get { return name; } }
        public string Url { get { return url; } }
        private string name;
        private string url;

        public Repo(string name, string url) {
            this.name = name;
            this.url = url;
            }
        }
    }
