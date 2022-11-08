using System.Net.Mail;

namespace ShredStore.Models.Utility
{
    public class ListCorrector
    {
        public List<string> SetProductList(List<string> prods)
        {
            List<string> prodList = new List<string>();
            foreach(var pro in prods)
            {
                int dupeCount = 0;
                foreach(var prod in prods)
                {
                    if(pro == prod)
                    {
                        dupeCount++;
                    }
                }
                prodList.Add(pro + " " + "x" + dupeCount);
            }

            return prodList.Distinct().ToList();
        }
        
        public bool IsEmailValid(string Email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(Email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
            
        }
    
    
    }

   
}
