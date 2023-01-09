using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.OAuth2SampleConsoleResourceOwner
{

    public class IonAPICredential
    {

        public string ResourceOwnerClientId { get { return ci; } } 
        public string ResourceOwnerClientSecret { get { return cs; } } 
        public string OAuth2TokenEndpoint { get { return pu + '/' + ot; } } 
        public string OAuth2TokenRevocationEndpoint { get { return pu + '/' + or; } } 
        public string OAuth2AuthorizationEndpoint { get { return pu + '/' + oa; } } 
        public string IONAPIBaseUrl { get { return iu + '/' + ti; } } 
        public string ServiceAccountAccessKey { get { return saak; } } 
        public string ServiceAccountSecretKey { get { return sask; } }


        public string ti { get; set; }
        public string cn { get; set; }
        public string dt { get; set; }
        public string ci { get; set; }
        public string cs { get; set; }
        public string iu { get; set; }
        public string pu { get; set; }
        public string oa { get; set; }
        public string ot { get; set; }
        public string or { get; set; }
        public string ev { get; set; }
        public string v { get; set; }
        public string saak { get; set; }
        public string sask { get; set; }
    }

}
