using RankingLibrary.Properties;
using RankingLibrary.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RankingLibrary.SupportObjects.PlayerObjects
{
    public class Player
    {
        private TextValidator textValidator = new TextValidator();

        public int Id { get; set; }
        public string Name { get; set; }

        public Player(string usName)
        {
            Id = -1;

            string sName = String.Empty;
            if (textValidator.ValidateText(usName, out sName))
            {
                Name = sName;
            }
            else
            {
                Name = Resources.DefaultPlayerName;
            }
        }

        public Player(int id, string usName)
        {
            Id = id;

            string sName = String.Empty;
            if(textValidator.ValidateText(usName, out sName))
            {
                Name = sName;
            }
            else
            {
                Name = Resources.DefaultPlayerName;
            }
            
        }
    }
}
